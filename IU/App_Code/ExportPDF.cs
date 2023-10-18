using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Web.UI;
using System.Data;
using Generales.Entidades;
using Comunes.Entidades;
using Reportes.Entidades;
using System.Reflection;
using Reportes.FachadaNegocio;
using Generales.FachadaNegocio;

namespace IU
{
    public class ExportPDF : Page
    {
        public static void ExportarPDF(string nombreArchivo, Control control)
        {
            string urlPath = string.Concat(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), HttpContext.Current.Request.ApplicationPath);
            urlPath = string.Concat( urlPath.EndsWith("/") ? urlPath : string.Concat(urlPath, "/"), nombreArchivo);
            string script = string.Format("<script>window.open('{0}');</script>", urlPath);
            ScriptManager.RegisterStartupScript(control, control.GetType(), "Print", script, false);
        }

        public static void ExportarPDF(byte[] pdf, Control control, UsuarioLogueado usuario)
        {
            string fileSystemPath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "tempPDF\\", usuario.NombreArchivo);
            string urlPath = string.Concat(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), HttpContext.Current.Request.ApplicationPath, "/tempPDF/", usuario.NombreArchivo);
            File.WriteAllBytes(fileSystemPath, pdf);
            string script = string.Format("<script>window.open('{0}');</script>", urlPath);
            ScriptManager.RegisterStartupScript(control, control.GetType(), "Print", script, false);
        }

        public static void ExportarPDF(byte[] pdf, Control control, string nombreArchivo, Usuarios usuario)
        {
            nombreArchivo = string.Concat(nombreArchivo, "_", usuario.Usuario, ".pdf");
            string fileSystemPath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "tempPDF\\", nombreArchivo);
            string urlPath = string.Concat(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), HttpContext.Current.Request.ApplicationPath, "/tempPDF/", nombreArchivo);
            File.WriteAllBytes(fileSystemPath, pdf);
            string script = string.Format("<script>window.open('{0}');</script>", urlPath);
            ScriptManager.RegisterStartupScript(control, control.GetType(), "Print", script, false);
        }     

        public static void ConvertirHtmlEnPdf(Control control, TGEPlantillas plantilla, DataSet datos, Usuarios usuario)
        {
            //Remplazo URL de la Imagen por URL Absoluta : http://.....
            plantilla.HtmlPlantilla = plantilla.HtmlPlantilla.Replace(string.Format("src=\"{0}/ImagenesCliente/", HttpContext.Current.Request.ApplicationPath), string.Format("src=\"{0}/ImagenesCliente/", string.Concat(HttpContext.Current.Request.Url.Scheme, "://", HttpContext.Current.Request.Url.Authority, HttpContext.Current.Request.ApplicationPath)));
            string dbValor = string.Empty;

            /************************************************/
            List<StringStartEnd> tables = FindAllString(plantilla.HtmlPlantilla, "<table ", "</table>");
            //string dbValor;
            string htmlTable;
            foreach (StringStartEnd item in tables)
            {
                htmlTable = plantilla.HtmlPlantilla.Substring(item.start, item.end - item.start);

                if (htmlTable.Contains("id=\"RepetirFilas"))
                {
                    string cuerpoTabla = getBetween(htmlTable, "<tbody>", "</tbody>");
                    string cuerpoTablaNueva = string.Empty;
                    string fila = cuerpoTabla.Replace("<tbody>", "").Replace("</tbody>", "");
                    string filaNueva = string.Empty;
                    string campo = string.Empty;
                    int rowMax = 0;
                    int count = 0;
                    DataColumn columna;
                    foreach (DataTable dt in datos.Tables)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Columns.Contains("RepetirFilas"))
                            {
                                rowMax = Convert.ToInt32(dt.Rows[0]["RepetirFilas"]);
                            }
                            foreach (DataRow row in dt.Rows)
                            {
                                count++;
                                filaNueva = fila;
                                while (filaNueva.Contains("{"))
                                {
                                    dbValor = string.Empty;
                                    campo = getBetween(filaNueva, "{", "}");
                                    if (campo.Length == 0)
                                        break;
                                    if (dt.Columns.Contains(campo))
                                    {
                                        columna = dt.Columns[campo];
                                        if (row[columna.ColumnName] == DBNull.Value)
                                            dbValor = string.Empty;
                                        else if (columna.DataType == typeof(decimal))
                                            dbValor = Convert.ToDecimal(row[columna.ColumnName]).ToString("N2");
                                        else if (columna.DataType == typeof(DateTime))
                                            dbValor = Convert.ToDateTime(row[columna.ColumnName]).ToShortDateString();
                                        //else if (columna.DataType == typeof(System.Byte[]))
                                        //{
                                        //    byte[] data = (byte[])row[columna.ColumnName];
                                        //    string base64String = Convert.ToBase64String(data);
                                        //    if (base64String != string.Empty)
                                        //    {
                                        //        dbValor = string.Format("<img src=\"data:image/png;base64,{0}\">", base64String);
                                        //    }
                                        //}
                                        else
                                            dbValor = row[columna.ColumnName].ToString();
                                    }
                                    filaNueva = filaNueva.Replace(string.Concat("{", campo, "}"), dbValor);
                                }
                                cuerpoTablaNueva = string.Concat(cuerpoTablaNueva, filaNueva);
                            }
                            /* Agrego filas vacias */
                            if (rowMax - count > 0)
                            {
                                filaNueva = fila;
                                while (filaNueva.Contains("{"))
                                {
                                    dbValor = string.Empty;
                                    campo = getBetween(filaNueva, "{", "}");
                                    if (campo.Length == 0)
                                        break;
                                    filaNueva = filaNueva.Replace(string.Concat("{", campo, "}"), "&nbsp;");
                                }
                                for (int i = 1; i < rowMax - count; i++)
                                    cuerpoTablaNueva = string.Concat(cuerpoTablaNueva, filaNueva);
                            }
                        }
                        else
                        {
                            filaNueva = fila;
                            while (filaNueva.Contains("{"))
                            {
                                dbValor = string.Empty;
                                campo = getBetween(filaNueva, "{", "}");
                                if (campo.Length == 0)
                                    break;
                                filaNueva = filaNueva.Replace(string.Concat("{", campo, "}"), string.Empty);
                            }
                            cuerpoTablaNueva = string.Concat(cuerpoTablaNueva, filaNueva);
                        }
                        
                        break;
                    }
                    plantilla.HtmlPlantilla = plantilla.HtmlPlantilla.Replace(htmlTable, htmlTable.Replace(cuerpoTabla, cuerpoTablaNueva));
                }

            }
            /************************************************/

            foreach (DataTable dt in datos.Tables)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (plantilla.HtmlPlantilla.Contains(string.Concat("{", col.ColumnName, "}")))
                        {
                            if (dt.Rows[0][col.ColumnName] == DBNull.Value)
                                dbValor = string.Empty;
                            else if (col.DataType == typeof(decimal))
                                dbValor = Convert.ToDecimal(dt.Rows[0][col.ColumnName]).ToString("N2");
                            else if (col.DataType == typeof(DateTime))
                                dbValor = Convert.ToDateTime(dt.Rows[0][col.ColumnName]).ToShortDateString();
                            else if (col.DataType == typeof(System.Byte[]))
                            {
                                byte[] data = (byte[])dt.Rows[0][col.ColumnName];
                                //string base64String = Convert.ToBase64String(data);
                                File.WriteAllBytes(string.Concat(HttpContext.Current.Request.PhysicalApplicationPath, string.Concat("ImagenesCliente\\", col.ColumnName, ".jpg")), data);
                                //if (base64String != string.Empty)
                                if(data.Length>0)
                                {
                                    //dbValor = string.Format("<img src=\"data:image/Jpeg;base64,{0}\" />", base64String);
                                    dbValor = string.Format("<img src=\"{0}\" />", string.Concat(HttpContext.Current.Request.Url.Scheme, "://", HttpContext.Current.Request.Url.Authority, HttpContext.Current.Request.ApplicationPath, string.Concat("/ImagenesCliente//", col.ColumnName, ".jpg")));
                                }
                            }
                            else
                                dbValor = dt.Rows[0][col.ColumnName].ToString();
                            plantilla.HtmlPlantilla = plantilla.HtmlPlantilla.Replace(string.Concat("{", col.ColumnName, "}"), dbValor);
                            dbValor = string.Empty;
                        }
                    }
                }
            }

            string nombreArchivo = string.Concat(plantilla.Codigo, "_", usuario.Usuario, ".pdf");
            string filePath = string.Concat(HttpContext.Current.Request.PhysicalApplicationPath, "tempPDF\\", nombreArchivo);

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
            Document document = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            document.Open();
            //HTMLWorker hw = new HTMLWorker(document);
            StringReader sr = new StringReader(plantilla.HtmlPlantilla);
            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
            //hw.Parse(new StringReader(HTML));
            document.Close();

            string urlPath = string.Concat(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), HttpContext.Current.Request.ApplicationPath, "/tempPDF/", nombreArchivo);
            string script = string.Format("<script>window.open('{0}');</script>", urlPath);
            ScriptManager.RegisterStartupScript(control, control.GetType(), "Print", script, false);
        }

        public static void ConvertirHtmlEnPdf(Control control, TGEPlantillas plantilla, Objeto pParametros, Usuarios usuario)
        {
            TGEComprobantes comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(plantilla);
            DataSet datos = ExportPDF.ObtenerDatosReporteComprobante(pParametros, comprobante);
            //Remplazo URL de la Imagen por URL Absoluta : http://.....
            plantilla.HtmlPlantilla = plantilla.HtmlPlantilla.Replace(string.Format("src=\"{0}/ImagenesCliente/", HttpContext.Current.Request.ApplicationPath), string.Format("src=\"{0}/ImagenesCliente/", string.Concat(HttpContext.Current.Request.Url.Scheme, "://", HttpContext.Current.Request.Url.Authority, HttpContext.Current.Request.ApplicationPath)));
            string dbValor = string.Empty;
            foreach (DataTable dt in datos.Tables)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (plantilla.HtmlPlantilla.Contains(string.Concat("{", col.ColumnName, "}")))
                        {
                            if (dt.Rows[0][col.ColumnName] == DBNull.Value)
                                dbValor = string.Empty;
                            else if (col.DataType == typeof(decimal))
                                dbValor = Convert.ToDecimal(dt.Rows[0][col.ColumnName]).ToString("N2");
                            else if (col.DataType == typeof(DateTime))
                                dbValor = Convert.ToDateTime(dt.Rows[0][col.ColumnName]).ToShortDateString();
                            else
                                dbValor = dt.Rows[0][col.ColumnName].ToString();
                            plantilla.HtmlPlantilla = plantilla.HtmlPlantilla.Replace(string.Concat("{", col.ColumnName, "}"), dbValor);
                            dbValor = string.Empty;
                        }
                    }
                }
            }


            if (plantilla.HtmlPlantilla.Contains("{Plantilla:"))
            {
                string codigoPlantilla;
                while (plantilla.HtmlPlantilla.Contains("{Plantilla:"))
                {
                    codigoPlantilla = getBetween(plantilla.HtmlPlantilla, "{Plantilla:", "}");
                    if (codigoPlantilla.Length == 0)
                        break;
                    plantilla.HtmlPlantilla = plantilla.HtmlPlantilla.Replace(string.Concat("{Plantilla:", codigoPlantilla,"}"), SubPlantilla(codigoPlantilla, pParametros));
                }
            }

            string nombreArchivo = string.Concat(plantilla.Codigo, "_", usuario.Usuario, ".pdf");
            string filePath = string.Concat(HttpContext.Current.Request.PhysicalApplicationPath, "tempPDF\\", nombreArchivo);
                        
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
            Document document = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            document.Open();
            //HTMLWorker hw = new HTMLWorker(document);
            StringReader sr = new StringReader(plantilla.HtmlPlantilla);
            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
            //hw.Parse(new StringReader(HTML));
            document.Close();

            string urlPath = string.Concat(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), HttpContext.Current.Request.ApplicationPath, "/tempPDF/", nombreArchivo);
            string script = string.Format("<script>window.open('{0}');</script>", urlPath);
            ScriptManager.RegisterStartupScript(control, control.GetType(), "Print", script, false);
        }

        private static string SubPlantilla(string pCodigoPlantilla, Objeto pParametros)
        {
            TGEPlantillas subPlantilla = new TGEPlantillas();
            subPlantilla.Codigo = pCodigoPlantilla;
            subPlantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(subPlantilla);
            TGEComprobantes subComprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(subPlantilla);
            DataSet subDatos = ExportPDF.ObtenerDatosReporteComprobante(pParametros, subComprobante);
            subPlantilla.HtmlPlantilla = subPlantilla.HtmlPlantilla.Replace(string.Format("src=\"{0}/ImagenesCliente/", HttpContext.Current.Request.ApplicationPath), string.Format("src=\"{0}/ImagenesCliente/", string.Concat(HttpContext.Current.Request.Url.Scheme, "://", HttpContext.Current.Request.Url.Authority, HttpContext.Current.Request.ApplicationPath)));

            List<StringStartEnd> tables = FindAllString(subPlantilla.HtmlPlantilla, "<table ", "</table>");
            string dbValor;
            string htmlTable;
            foreach(StringStartEnd item in tables)
            {
                htmlTable = subPlantilla.HtmlPlantilla.Substring(item.start, item.end - item.start);

                if (htmlTable.Contains("id=\"RepetirFilas"))
                {
                    string cuerpoTabla = getBetween(htmlTable, "<tbody>", "</tbody>");
                    string cuerpoTablaNueva = string.Empty;
                    string fila = cuerpoTabla.Replace("<tbody>", "").Replace("</tbody>", "");
                    string filaNueva = string.Empty;   
                    string campo = string.Empty;
                    DataColumn columna;
                    foreach (DataTable dt in subDatos.Tables)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                filaNueva = fila;
                                while (filaNueva.Contains("{"))
                                {
                                    dbValor = string.Empty;
                                    campo = getBetween(filaNueva, "{", "}");
                                    if (campo.Length == 0)
                                        break;
                                    if (dt.Columns.Contains(campo))
                                    {
                                        columna = dt.Columns[campo];
                                        if (row[columna.ColumnName] == DBNull.Value)
                                            dbValor = string.Empty;
                                        else if (columna.DataType == typeof(decimal))
                                            dbValor = Convert.ToDecimal(row[columna.ColumnName]).ToString("N2");
                                        else if (columna.DataType == typeof(DateTime))
                                            dbValor = Convert.ToDateTime(row[columna.ColumnName]).ToShortDateString();
                                        else
                                            dbValor = row[columna.ColumnName].ToString();
                                    }
                                    filaNueva = filaNueva.Replace(string.Concat("{", campo, "}"), dbValor);
                                }
                                cuerpoTablaNueva = string.Concat(cuerpoTablaNueva, filaNueva);
                            }
                        }
                        else
                        {
                            filaNueva = fila;
                            while (filaNueva.Contains("{"))
                            {
                                dbValor = string.Empty;
                                campo = getBetween(filaNueva, "{", "}");
                                if (campo.Length == 0)
                                    break;
                                filaNueva = filaNueva.Replace(string.Concat("{", campo, "}"), string.Empty);
                            }
                            cuerpoTablaNueva = string.Concat(cuerpoTablaNueva, filaNueva);
                        }
                    }
                    subPlantilla.HtmlPlantilla = subPlantilla.HtmlPlantilla.Replace(htmlTable, htmlTable.Replace(cuerpoTabla, cuerpoTablaNueva));
                }

            }
            dbValor = string.Empty;
            if (subDatos.Tables.Count > 0 && subDatos.Tables[0].Rows.Count > 0)
            {
                DataTable dt = subDatos.Tables[0];
                foreach (DataColumn col in dt.Columns)
                {
                    if (subPlantilla.HtmlPlantilla.Contains(string.Concat("{", col.ColumnName, "}")))
                    {
                        if (dt.Rows[0][col.ColumnName] == DBNull.Value)
                            dbValor = string.Empty;
                        else if (col.DataType == typeof(decimal))
                            dbValor = Convert.ToDecimal(dt.Rows[0][col.ColumnName]).ToString("N2");
                        else if (col.DataType == typeof(DateTime))
                            dbValor = Convert.ToDateTime(dt.Rows[0][col.ColumnName]).ToShortDateString();
                        else
                            dbValor = dt.Rows[0][col.ColumnName].ToString();
                        subPlantilla.HtmlPlantilla = subPlantilla.HtmlPlantilla.Replace(string.Concat("{", col.ColumnName, "}"), dbValor);
                        dbValor = string.Empty;
                    }
                }

            }

            return subPlantilla.HtmlPlantilla;
        }

        private static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        public static List<StringStartEnd> FindAllString(string str, string start, string end)
        {            
            List<StringStartEnd> indexes = new List<StringStartEnd>();
            StringStartEnd item;
            for (int index = 0; ; index+=end.Length)
            {
                index = str.IndexOf(start, index);
                if (index == -1)
                    break;
                else {
                    item = new StringStartEnd();
                    item.start = index;
                    index = str.IndexOf(end, index+start.Length);
                    item.end = index+end.Length;
                }
                indexes.Add(item);
            }
            return indexes;
        }

        public static void ConvertirArchivoEnPdf(Control control, TGEArchivos pArchivo)
        {
            List<TGEArchivos> lista = new List<TGEArchivos>();
            lista.Add(pArchivo);
            string nombreArchivo = string.Concat(pArchivo.NombreArchivo, "_", pArchivo.UsuarioLogueado.Usuario, ".pdf");
            ConvertirArchivoEnPdf(control, lista, nombreArchivo);
        }
        public static void ConvertirArchivoEnPdf(Control control, List<TGEArchivos> pListaArchivos, string nombreArchivo)
        {
            string filePath = string.Concat(HttpContext.Current.Request.PhysicalApplicationPath, "tempPDF\\", nombreArchivo);
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            Stream pdfFinal = new FileStream(filePath, FileMode.Create);
            Document document = new Document(PageSize.A4, 0, 0, 0, 0);
            PdfReader reader;
            int pageCount = 0;
            using (PdfCopy writer = new PdfCopy(document, pdfFinal))
            {
                writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_5);
                writer.CompressionLevel = PdfStream.BEST_COMPRESSION;
                writer.SetFullCompression();
                document.Open();
                foreach (TGEArchivos pArchivo in pListaArchivos)
                {
                    //create PdfReader object
                    reader = new PdfReader(pArchivo.Archivo);
                    pageCount += reader.NumberOfPages;
                    reader.ConsolidateNamedDestinations();
                    writer.AddDocument(reader);
                    reader.Close();
                }
                document.Close();
            }
            string urlPath = string.Concat(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), HttpContext.Current.Request.ApplicationPath, "/tempPDF/", nombreArchivo);
            string script = string.Format("<script>window.open('{0}');</script>", urlPath);
            ScriptManager.RegisterStartupScript(control, control.GetType(), "Print", script, false);
        }

        public static DataSet ObtenerDatosReporteComprobante(Objeto pDatosReporte, TGEComprobantes comprobante)
        {
            /**/
            RepReportes reporte = new RepReportes();
            RepParametros param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            //param.Parametro = "IdStockMovimiento";
            PropertyInfo prop = pDatosReporte.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() == 1).ToList()[0];
            param.Parametro = prop.Name;
            param.ValorParametro = Convert.ToInt32(prop.GetValue(pDatosReporte, null));
            reporte.Parametros.Add(param);
            reporte.StoredProcedure = comprobante.NombreSP;
            reporte.UsuarioLogueado = pDatosReporte.UsuarioLogueado;
            return ReportesF.ReportesObtenerDatos(reporte);
        }


        //public void ConvertirHtmlEnPDF(string html)
        //{
        //    //Create a byte array that will eventually hold our final PDF
        //    Byte[] bytes;

        //    //Boilerplate iTextSharp setup here
        //    //Create a stream that we can write to, in this case a MemoryStream
        //    using (var ms = new MemoryStream())
        //    {

        //        //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
        //        using (var doc = new Document())
        //        {

        //            //Create a writer that's bound to our PDF abstraction and our stream
        //            using (var writer = PdfWriter.GetInstance(doc, ms))
        //            {

        //                //Open the document for writing
        //                doc.Open();

        //                //Our sample HTML and CSS
        //                var example_html = html; //@"<p>This <em>is </em><span class=""headline"" style=""text-decoration: underline;"">some</span> <strong>sample <em> text</em></strong><span style=""color: red;"">!!!</span></p>";
        //                var example_css = @".headline{font-size:200%}";

        //                /**************************************************
        //                 * Example #1                                     *
        //                 *                                                *
        //                 * Use the built-in HTMLWorker to parse the HTML. *
        //                 * Only inline CSS is supported.                  *
        //                 * ************************************************/

        //                //Create a new HTMLWorker bound to our document
        //                using (var htmlWorker = new iTextSharp.text.html.simpleparser.HTMLWorker(doc))
        //                {

        //                    //HTMLWorker doesn't read a string directly but instead needs a TextReader (which StringReader subclasses)
        //                    using (var sr = new StringReader(example_html))
        //                    {

        //                        //Parse the HTML
        //                        htmlWorker.Parse(sr);
        //                    }
        //                }

        //                /**************************************************
        //                 * Example #2                                     *
        //                 *                                                *
        //                 * Use the XMLWorker to parse the HTML.           *
        //                 * Only inline CSS and absolutely linked          *
        //                 * CSS is supported                               *
        //                 * ************************************************/

        //                //XMLWorker also reads from a TextReader and not directly from a string
        //                using (var srHtml = new StringReader(example_html))
        //                {

        //                    //Parse the HTML
        //                    iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, srHtml);
        //                }

        //                /**************************************************
        //                 * Example #3                                     *
        //                 *                                                *
        //                 * Use the XMLWorker to parse HTML and CSS        *
        //                 * ************************************************/

        //                //In order to read CSS as a string we need to switch to a different constructor
        //                //that takes Streams instead of TextReaders.
        //                //Below we convert the strings into UTF8 byte array and wrap those in MemoryStreams
        //                using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css)))
        //                {
        //                    using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_html)))
        //                    {

        //                        //Parse the HTML
        //                        iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss);
        //                    }
        //                }


        //                doc.Close();
        //            }
        //        }

        //        //After all of the PDF "stuff" above is done and closed but **before** we
        //        //close the MemoryStream, grab all of the active bytes from the stream
        //        bytes = ms.ToArray();
        //    }

        //    //Now we just need to do something with those bytes.
        //    //Here I'm writing them to disk but if you were in ASP.Net you might Response.BinaryWrite() them.
        //    //You could also write the bytes to a database in a varbinary() column (but please don't) or you
        //    //could pass them to another function for further PDF processing.
        //    var testFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.pdf");
        //    System.IO.File.WriteAllBytes(testFile, bytes);
        //}
    }
}