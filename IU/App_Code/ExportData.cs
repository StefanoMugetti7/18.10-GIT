using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using Generales.Entidades;
using Reportes.Entidades;
using System.IO.Compression;
using SharpCompress.Writer;
using SharpCompress.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace IU
{
    public class ExportData
    {
        public void ExportExcel(Page WebPage, DataTable dtExcel, string[] NombresGrilla, string[] NombresTabla)
        {
            HttpResponse resp;
            string colHeaders = "", colRow = "";

            if (dtExcel.Rows.Count > 0)
            {
                resp = WebPage.Response;
                resp.ContentType = "application/download";
                resp.AppendHeader("Content-Disposition", "attachment;filename=Exportacion.xls");

                foreach (string Nombre in NombresGrilla)
                    colHeaders = colHeaders + Nombre.Trim() + Convert.ToChar(9);

                if (colHeaders.Length > 0)
                    colHeaders = colHeaders.Substring(0, colHeaders.LastIndexOf(Convert.ToChar(9)));

                colHeaders = colHeaders + Convert.ToChar(10);
                resp.Write(colHeaders);

                foreach (DataRow dr in dtExcel.Rows)
                {
                    colRow = "";

                    foreach (string Nombre in NombresTabla)
                    {
                        colRow = colRow + dr[Nombre].ToString().Trim() + Convert.ToChar(9);
                    }

                    colRow = colRow.Replace("\r\n", " ") + Convert.ToChar(10);
                    resp.Write(colRow);
                }

                resp.End();
            }
            else
                throw new Exception("No existen datos para la consulta!");
        }

        public void ExportExcel(Page WebPage, DataTable dtExcel)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dtExcel.Copy());
            this.ExportExcel(WebPage, ds, true, string.Empty, string.Empty);
        }

        public void ExportExcel_viejo(Page WebPage, DataSet dsExcel, bool exportcolumnsheader, string reportName)
        {
            HttpResponse resp;
            String colHeaders = String.Empty, colRow = String.Empty;

            if (dsExcel.Tables.Count > 0)
            {
                resp = WebPage.Response;
                resp.ContentType = "application/download";
                //resp.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                resp.AppendHeader("Content-Disposition", "attachment;filename=Exportacion" + reportName + ".xls");
                resp.Charset = string.Empty;
                resp.ContentEncoding = System.Text.Encoding.UTF32;

                foreach (DataTable dtExcel in dsExcel.Tables)
                {
                    colHeaders = String.Empty;

                    //LLENO LA CABECERA, PERO AQUI CON LOS NOMBRES DE LA COLUMNA DEL DATATABLE

                    if (exportcolumnsheader)
                    {
                        foreach (DataColumn drCol in dtExcel.Columns)
                            colHeaders = colHeaders + drCol.Caption.Trim() + Convert.ToChar(9);

                        if (colHeaders.Length > 0)
                            colHeaders = colHeaders.Substring(0, colHeaders.LastIndexOf(Convert.ToChar(9)));

                        colHeaders = (dtExcel == dsExcel.Tables[0] ? Convert.ToChar(" ") : Convert.ToChar(10)) + colHeaders + Convert.ToChar(10);
                        resp.Write(colHeaders);
                    }
                    foreach (DataRow dr in dtExcel.Rows)
                    {
                        colRow = "";

                        foreach (DataColumn ColName in dtExcel.Columns)
                        {
                            colRow += this.ParseColumnData(dr[ColName.ColumnName].ToString()) + Convert.ToChar(9); //REVIEW
                        }

                        colRow = colRow + Convert.ToChar(10);
                        resp.Write(colRow);
                    }
                }
                resp.End();
            }
            else
                throw new Exception("No existen datos para la consulta!");
        }

        /// <summary>
        /// Metodo version 2.0 para exportar el datatable de las grillas a excel definiendo las columnas a exportar
        /// </summary>
        /// <param name="WebPage"></param>
        /// <param name="dsExcel"></param>
        /// <param name="exportcolumnsheader"></param>
        /// <param name="reportName"></param>
        /// <param name="solapa"></param>
        /// <param name="exportColumns"></param>
        public void ExportExcel(Page WebPage, DataTable dtExcel, bool exportcolumnsheader, string reportName, string solapa, Dictionary<string, string> exportColumns)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dtExcel);
            ExportExcel(WebPage, ds, exportcolumnsheader, reportName, solapa, exportColumns);
        }

        /// <summary>
        /// Metodo version 2.0 para exportar el datatable de las grillas a excel definiendo las columnas a exportar
        /// </summary>
        /// <param name="WebPage"></param>
        /// <param name="dsExcel"></param>
        /// <param name="exportcolumnsheader"></param>
        /// <param name="reportName"></param>
        /// <param name="solapa"></param>
        /// <param name="exportColumns"></param>
        public void ExportExcel(Page WebPage, DataSet dsExcel, bool exportcolumnsheader, string reportName, string solapa, Dictionary<string,string> exportColumns)
        {
            DataSet ds = new DataSet();
            if (dsExcel.Tables.Count == 1 && exportColumns.Count > 0)
            {
                System.Data.DataView view = new System.Data.DataView(dsExcel.Tables[0]);
                System.Data.DataTable selected = view.ToTable(false, exportColumns.Keys.ToArray());
                ds.Tables.Add(selected);
                foreach (DataColumn col in ds.Tables[0].Columns)
                    col.ColumnName = exportColumns[col.ColumnName];
                ExportExcel(WebPage, ds, exportcolumnsheader, reportName, solapa );
            }
            else
                ExportExcel(WebPage, dsExcel, exportcolumnsheader, reportName, solapa);
        }
        /// <summary>
        /// Metodo version 2.0 para exportar el datatable de las grillas a excel
        /// </summary>
        /// <param name="WebPage"></param>
        /// <param name="dsExcel"></param>
        /// <param name="exportcolumnsheader"></param>
        /// <param name="reportName"></param>
        /// <param name="solapa"></param>
        /// <param name="exportColumns"></param>
        public void ExportExcel(Page WebPage, DataSet dsExcel, bool exportcolumnsheader, string reportName, string solapa)
        {

            //DataTable tbl = dsExcel.Tables[0];
            if (reportName.Trim().Length == 0)
                reportName = "Reporte";
            if (solapa.Trim().Length == 0)
                solapa = "Datos";

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create the worksheet                
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(solapa);

                int posicionFila = 1;

                if (dsExcel.Tables.Count == 1 && dsExcel.Tables[0].Rows.Count == 0)
                    dsExcel.Tables[0].Rows.Add(dsExcel.Tables[0].NewRow());

                foreach (DataTable dt in dsExcel.Tables)
                {
                    ws.Cells[posicionFila, 1].LoadFromDataTable(dt, true);

                    //Format the header for column 1-3
                    using (ExcelRange rng = ws.Cells[posicionFila, 1, posicionFila, dt.Columns.Count])
                    {
                        rng.Style.Font.Bold = true;
                        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                        rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));  //Set color to dark blue
                        rng.Style.Font.Color.SetColor(Color.White);
                        rng.AutoFitColumns();
                    }

                    if (dt.Rows.Count > 0)
                    {
                        //using (ExcelRange filtro = ws.Cells[posicionFila, 1, posicionFila + dt.Rows.Count, dt.Columns.Count])
                        //{
                        //    filtro.AutoFilter = true;
                        //}

                        using (ExcelRange estilo = ws.Cells[posicionFila, 1, posicionFila + dt.Rows.Count, dt.Columns.Count])
                        {
                            foreach (DataColumn dtColum in dt.Columns)
                            {
                                if (dtColum.DataType == typeof(DateTime))
                                    estilo[posicionFila + 1, dtColum.Ordinal + 1, posicionFila + dt.Rows.Count, dtColum.Ordinal + 1].Style.Numberformat.Format = "dd/MM/yyyy";
                                else if (dtColum.DataType == typeof(string))
                                    estilo[posicionFila + 1, dtColum.Ordinal + 1, posicionFila + dt.Rows.Count, dtColum.Ordinal + 1].Style.Numberformat.Format = "@";
                                else if (dtColum.DataType == typeof(decimal))
                                    //ws.Column(dtColum.Ordinal + 1).Style.Numberformat.Format = "0.00";
                                    estilo[posicionFila + 1, dtColum.Ordinal + 1, posicionFila + dt.Rows.Count, dtColum.Ordinal + 1].Style.Numberformat.Format = "###,###,##0.00";
                                //ws.Column(dtColum.Ordinal + 1).Style.Numberformat.Format = @"_(""$""* #,##0.00_);_(""$""* \(#,##0.00\);_(""$""* ""-""??_);_(@_)";
                                else if (dtColum.DataType == typeof(Int32) || dtColum.DataType == typeof(Int64))
                                    estilo[posicionFila + 1, dtColum.Ordinal + 1, posicionFila + dt.Rows.Count, dtColum.Ordinal + 1].Style.Numberformat.Format = "0";
                            }
                        }
                    }
                    posicionFila += dt.Rows.Count + 2;
                }

                //Formato para el EXCEL TITULO

                if (dsExcel.Tables.Count >= 2)
                {
                    if (dsExcel.Tables[0].Rows.Count == 1)
                    {
                        ws.Cells[1, 1, 1, dsExcel.Tables[1].Columns.Count].Merge = true;
                        ws.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[2, 1, 2, dsExcel.Tables[1].Columns.Count].Merge = true;
                    }
                }

                //Write it back to the client
                //WebPage.Response.Clear();
                //WebPage.Response.AddHeader("content-disposition", "attachment; filename=" + reportName + ".xlsx");
                //WebPage.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //var excelbytes = pck.GetAsByteArray();
                //WebPage.Response.BinaryWrite(excelbytes);
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                //Response.End();

                //convert the excel package to a byte array
                byte[] bin = pck.GetAsByteArray();

                //clear the buffer stream
                WebPage.Response.ClearHeaders();
                WebPage.Response.Clear();
                WebPage.Response.Buffer = true;

                //set the correct contenttype
                WebPage.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                //set the correct length of the data being send
                WebPage.Response.AddHeader("content-length", bin.Length.ToString());

                //set the filename for the excel package
                WebPage.Response.AddHeader("content-disposition", "attachment; filename=" + reportName + ".xlsx");

                //send the byte array to the browser
                WebPage.Response.OutputStream.Write(bin, 0, bin.Length);

                //cleanup
                WebPage.Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            //using (ExcelPackage pck = new ExcelPackage())
            //{
            //    //Create the worksheet                
            //    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Reporte");
            //    //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1                ws.Cells["A1"].LoadFromDataTable(dt, true);
            //    //prepare the range for the column headers                
            //    string cellRange = "A1:" + Convert.ToChar('A' + dt.Columns.Count - 1) + 1;
            //    //Format the header for columns                
            //    using (ExcelRange rng = ws.Cells[cellRange])
            //    {
            //        rng.Style.WrapText = false;
            //        rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            //        rng.Style.Font.Bold = true;
            //        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
            //        //Set Pattern for the background to Solid                                      rng.Style.Fill.BackgroundColor.SetColor(Color.Gray);
            //        rng.Style.Font.Color.SetColor(Color.White);
            //    }
            //    //prepare the range for the rows                
            //    string rowsCellRange = "A2:" + Convert.ToChar('A' + dt.Columns.Count - 1) + dt.Rows.Count * dt.Columns.Count;
            //    //Format the rows               
            //    using (ExcelRange rng = ws.Cells[rowsCellRange])
            //    {
            //        rng.Style.WrapText = false;
            //        rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            //    }
            //    //Read the Excel file in a byte array                
            //    Byte[] fileBytes = pck.GetAsByteArray();

            //    //Clear the response                
            //    WebPage.Response.Clear();
            //    WebPage.Response.ClearContent();
            //    WebPage.Response.ClearHeaders();
            //    WebPage.Response.Cookies.Clear();
            //    //Add the header & other information                 Response.Cache.SetCacheability(HttpCacheability.Private);
            //    WebPage.Response.CacheControl = "private";
            //    WebPage.Response.Charset = System.Text.UTF8Encoding.UTF8.WebName;
            //    WebPage.Response.ContentEncoding = System.Text.UTF8Encoding.UTF8;
            //    WebPage.Response.AppendHeader("Content-Length", fileBytes.Length.ToString());
            //    WebPage.Response.AppendHeader("Pragma", "cache");
            //    WebPage.Response.AppendHeader("Expires", "60");
            //    WebPage.Response.AppendHeader("Content-Disposition",
            //    "attachment; " +
            //    "filename=\"ExcelReport.xlsx\"; " +
            //    "size=" + fileBytes.Length.ToString() + "; " +
            //    "creation-date=" + DateTime.Now.ToString("R") + "; " +
            //    "modification-date=" + DateTime.Now.ToString("R") + "; " +
            //    "read-date=" + DateTime.Now.ToString("R"));
            //    WebPage.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //    //Write it back to the client                
            //    WebPage.Response.BinaryWrite(fileBytes);
            //    WebPage.Response.End();
        }

        private string ParseColumnData(string ColumnData)
        {
            return ColumnData.Trim().Replace("\r", " ").Replace("\n", " ").Replace("\t", " ");
        }

        /// <summary>
        /// Exports the datagrid to an Excel file with the name of the datasheet provided by the passed in parameter
        /// </summary>
        /// <param name="reportName">Name of the datasheet.
        /// </param>
        public virtual void Export(string reportName, Page CurrentPage, Control NtGridView)
        {
            System.Web.UI.HtmlControls.HtmlForm htmlForm = new System.Web.UI.HtmlControls.HtmlForm();
            CurrentPage.Controls.Add(htmlForm);
            htmlForm.Controls.Add(NtGridView);

            ClearChildControls((GridView)NtGridView);

            CurrentPage.Response.Clear();
            CurrentPage.Response.Buffer = true;

            CurrentPage.Response.AddHeader("Content-Disposition", "attachment; filename=" + reportName);
            CurrentPage.Response.ContentType = "application/vnd.ms-excel";
            CurrentPage.Response.Charset = string.Empty;
            CurrentPage.Response.ContentEncoding = System.Text.Encoding.UTF32;
            CurrentPage.EnableViewState = false;

            using (StringWriter stringWriter = new StringWriter())
            {
                HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
                htmlForm.RenderControl(htmlWriter);
                htmlWriter.Flush();

                CurrentPage.Response.Write(stringWriter.ToString());
                CurrentPage.Response.End();
            }
        }

        /// <summary>
        /// Iterates a control and its children controls, ensuring they are all LiteralControls
        /// <remarks>
        /// Only LiteralControl can call RenderControl(System.Web.UI.HTMLTextWriter htmlWriter) method. Otherwise 
        /// a runtime error will occur. This is the reason why this method exists.
        /// </remarks>
        /// </summary>
        /// <param name="control">The control to be cleared and verified</param>
        private void RecursiveClear(Control control)
        {
            //Clears children controls
            for (int i = control.Controls.Count - 1; i >= 0; i--)
            {
                RecursiveClear(control.Controls[i]);
            }

            //If it is a LinkButton, convert it to a LiteralControl
            if (control is LinkButton)
            {
                LiteralControl literal = new LiteralControl();
                control.Parent.Controls.Add(literal);
                literal.Text = ((LinkButton)control).Text;
                control.Parent.Controls.Remove(control);
            }
            //We don't need a button in the excel sheet, so simply delete it
            else if (control is Button)
            {
                control.Parent.Controls.Remove(control);
            }

            else if (control is System.Web.UI.WebControls.Image)
            {
                if (((System.Web.UI.WebControls.Image)control).Visible)
                {
                    control.Parent.Controls.Add(new LiteralControl("&lt;span style='font-size:px;'>o</span>"));
                }
                control.Parent.Controls.Remove(control);
            }
            //If it is a ListControl, copy the text to a new LiteralControl
            else if (control is ListControl)
            {
                LiteralControl literal = new LiteralControl();
                control.Parent.Controls.Add(literal);
                try
                {
                    literal.Text = ((ListControl)control).SelectedItem.Text;
                }
                catch
                {
                }
                control.Parent.Controls.Remove(control);

            }
            //You may add more conditions when necessary

            return;
        }

        /// <summary>
        /// Clears the child controls of a Datagrid to make sure all controls are LiteralControls
        /// </summary>
        /// <param name="dg">Datagrid to be cleared and verified</param>
        protected void ClearChildControls(System.Web.UI.WebControls.GridView dg)
        {

            for (int i = dg.Columns.Count - 1; i >= 0; i--)
            {
                if (dg.Columns[i].GetType().Name == "ButtonColumn" || dg.Columns[i].GetType().Name == "CheckBoxField")
                {
                    dg.Columns.Remove(dg.Columns[i]);
                }
            }

            this.RecursiveClear(dg);

        }

        public void ExportFile(Page WebPage, DataTable datatable, string delimited, bool exportcolumnsheader)
        {
            this.ExportFile(WebPage, datatable, delimited, exportcolumnsheader, WebPage.Session.SessionID);
            #region Anterior
            //HttpResponse resp;
            //StringBuilder txtfile = new StringBuilder();
            //if (datatable.Rows.Count > 0)
            //{
            //    resp = WebPage.Response;
            //    resp.ContentType = "text/plain";
            //    resp.AppendHeader("Content-Disposition", "attachment;filename=" + WebPage.Session.SessionID + ".txt");
            //    resp.Charset = string.Empty;
            //    resp.ContentEncoding = System.Text.Encoding.ASCII;

            //    if (exportcolumnsheader)
            //    {
            //        string Columns = string.Empty;
            //        foreach (DataColumn column in datatable.Columns)
            //        {
            //            Columns += column.ColumnName + delimited;
            //        }
            //        txtfile.AppendLine(Columns.Remove(Columns.Length - 1, 1));
            //        //resp.Write(Columns.Remove(Columns.Length - 1, 1));
            //    }
            //    foreach (DataRow datarow in datatable.Rows)
            //    {
            //        string row = string.Empty;

            //        foreach (object items in datarow.ItemArray)
            //        {

            //            row += items.ToString() + delimited;
            //        }
            //        txtfile.AppendLine(row.Remove(row.Length - 1, 1));
            //        //resp.Write(row.Remove(row.Length - 1, 1));
            //    }
            //    resp.Write(txtfile);
            //    resp.End();
            //}
            //else
            //{
            //    throw new Exception("No existen datos para la consulta!");
            //}    
            #endregion
        }

        public void ExportFile(Page WebPage, DataTable datatable, string delimited, bool exportcolumnsheader, string reportName)
        {
            HttpResponse resp;
            StringBuilder txtfile = new StringBuilder();

            if (reportName.Trim().Length == 0)
                reportName = "Reporte";

            if (datatable.Rows.Count > 0)
            {
                resp = WebPage.Response;
                resp.ContentType = "text/plain";
                resp.AppendHeader("Content-Disposition", "attachment;filename=" + reportName + ".txt");
                //CurrentPage.Response.AddHeader("Content-Disposition", "attachment; filename=" + reportName);
                resp.Charset = string.Empty;
                resp.ContentEncoding = System.Text.Encoding.ASCII;

                if (exportcolumnsheader)
                {
                    string Columns = string.Empty;
                    foreach (DataColumn column in datatable.Columns)
                    {
                        Columns += column.ColumnName + delimited;
                    }
                    txtfile.AppendLine(Columns.Remove(Columns.Length - 1, 1));
                    //resp.Write(Columns.Remove(Columns.Length - 1, 1));
                }
                int count = 0;
                string row;
                foreach (DataRow datarow in datatable.Rows)
                {
                    row = string.Empty;

                    foreach (object items in datarow.ItemArray)
                    {

                        row += items.ToString() + delimited;
                    }
                    count++;
                    if (row.Length > 0)
                        txtfile.AppendLine(row.Remove(row.Length - 1, delimited.Length));
                    else
                        row = string.Empty;
                    //resp.Write(row.Remove(row.Length - 1, 1));
                }
                resp.Write(txtfile);
                resp.End();
            }
            else
            {
                throw new Exception("No existen datos para la consulta!");
            }
        }

        public enum TipoExportacion
        {
            attachment,
            inline,
        }

        public void ExportGenericFile(Page WebPage, TGEArchivos archivo, bool archivoFisico)
        {
            this.ExportGenericFile(WebPage, archivo, archivoFisico, TipoExportacion.attachment);
        }

        public void ExportGenericFile(Page WebPage, TGEArchivos archivo, bool archivoFisico, TipoExportacion pTipoExp)
        {
            WebPage.Response.Clear();
            WebPage.Response.Buffer = true;

            switch (archivo.TipoArchivo.ToLower())
            {
                case "pdf":
                    WebPage.Response.ContentType = "application/pdf";
                    break;
                case "sql":
                        WebPage.Response.ContentType = "text/plain";
                    break;
                default:
                    WebPage.Response.ContentType = archivo.TipoArchivo;
                    break;
            }
            
            WebPage.Response.AddHeader("content-disposition", String.Format(string.Concat(pTipoExp.ToString(), ";filename=\"{0}\""), string.Concat(archivo.NombreArchivo, ".", archivo.TipoArchivo)));

            if (archivoFisico)
            {
                //string rutaArchivo = WebPage.Request.ApplicationPath.EndsWith("/") ? WebPage.Request.ApplicationPath : string.Concat(WebPage.Request.ApplicationPath, "/");
                //rutaArchivo += archivo.NombreArchivo;
                WebPage.Response.WriteFile(archivo.RutaFisica);
                //WebPage.Response.TransmitFile(archivo.RutaFisica);
            }
            else
            {
                //MemoryStream ms = new MemoryStream(archivo.Archivo);
                //ms.WriteTo(WebPage.Response.OutputStream);
                WebPage.Response.BinaryWrite(archivo.Archivo);
            }

            WebPage.Response.End();
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public void ExportFilesToZip(Page WebPage)
        {
            //Stream exportZip;
            //Stream txtFile;
            //using (var zipWriter = WriterFactory.Open(exportZip, SharpCompress.Common.ArchiveType.Zip, CompressionType.Deflate)
            //{
            //    zipWriter.Write("test" , txtFile, DateTime.Now);
            //}
        }

        public void createPDF(DataSet ds, string destinationPath)
        {
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(destinationPath, FileMode.Create));
            document.Open();
            PdfPTable table;
            foreach (DataTable dataTable in ds.Tables)
            {
                table = new PdfPTable(dataTable.Columns.Count);
                table.WidthPercentage = 100;

                //Set columns names in the pdf file
                for (int k = 0; k < dataTable.Columns.Count; k++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(dataTable.Columns[k].ColumnName));

                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(51, 102, 102);

                    table.AddCell(cell);
                }

                //Add values of DataTable in pdf file
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(dataTable.Rows[i][j].ToString()));

                        //Align the cell in the center
                        cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;

                        table.AddCell(cell);
                    }
                }
                document.Add(table);
            }
            document.Close();
        }
    }
}
