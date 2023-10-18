using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using Reportes.Entidades;
using Comunes.Entidades;
using System.Reflection;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Reportes.FachadaNegocio;
using Haberes.Entidades;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Collections.Generic;
using Servicio.AccesoDatos;

namespace IU.Modulos.Comunes
{
    public partial class popUpComprobantes : ControlesSeguros
    {

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }

        public void CargarReporte(Objeto pDatosReporte, EnumTGEComprobantes pComprobante)
        {
            this.CargarReporte(pDatosReporte, pComprobante, true);
        }

        public void CargarReporte(Objeto pDatosReporte, EnumTGEComprobantes pComprobante, bool pGenerarComprobante)
        {
            this.btnVolver.Enabled = true;
            try
            {
                if (pGenerarComprobante)
                    this.GenerarReporte(pDatosReporte, pComprobante);
                this.ShowPdf1.FilePath = this.VirtualFilePath(pDatosReporte);
                this.mpePopUp.Show();
            }
            catch (Exception ex)
            {
                bool rthrow = ExceptionHandler.HandleException(ex, "AccesoDatos");
                this.MostrarMensaje(ex.Message, true);
            }
        }

        public void CargarReporte(List<HabArchivosDetalles> pListaArchivos, RepReportes pReporte, EnumTGEComprobantes pComprobante)
        {
            string nombre=string.Empty;
            string nombreArchivo = string.Empty;
            Document document = new Document(PageSize.A4, 0, 0, 0, 0);
            PdfReader reader;

            if (pListaArchivos.Count > 0)
            {
                    nombreArchivo = pListaArchivos[0].NombreArchivo;
                if (nombreArchivo.Length > 0)
                {
                    nombre = string.Concat(this.Request.PhysicalApplicationPath, "tempPDF\\", pListaArchivos[0].NombreArchivo);
                }
            }
            else
            {
                nombreArchivo = "tmpRecibo.pdf";
                nombre = string.Concat(this.Request.PhysicalApplicationPath, "tempPDF\\", nombreArchivo);
            }

            //if (!nombre.ToLower().EndsWith(".pdf"))
            //{
            //    nombre = string.Concat(nombre, ".pdf");
            //    nombreArchivo = string.Concat(nombreArchivo, ".pdf");
            //}
            nombre = string.Concat(nombre, "_", this.UsuarioActivo.IdUsuario.ToString().PadLeft(5, '0'), ".pdf");
            nombreArchivo = string.Concat(nombreArchivo, "_", this.UsuarioActivo.IdUsuario.ToString().PadLeft(5, '0'), ".pdf");

            if (File.Exists(nombre))
                File.Delete(nombre);

            //RECIBO COM
            Stream reciboCom = new System.IO.MemoryStream();
            TGEComprobantes comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(pComprobante);
            string archivoReporteLeer = string.Concat(this.ObtenerAppPath(), comprobante.NombreRPT);
            pReporte.StoredProcedure = comprobante.NombreSP;
            DataSet ds = ReportesF.ReportesObtenerDatos(pReporte);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                CrystalReportSource CryReportSource = new CrystalReportSource();
                try
                {
                    CryReportSource.CacheDuration = 1;
                    CryReportSource.Report.FileName = Server.MapPath(archivoReporteLeer);
                    //CryReportSource.ReportDocument.ParameterFields["UsuarioActivo"].CurrentValues.Add(this.UsuarioActivo.ApellidoNombre);
                    CryReportSource.ReportDocument.SetDataSource(ds);
                    reciboCom = CryReportSource.ReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
                }
                catch (Exception ex)
                {
                    throw new Exception("Ha ocurrido un error al generar el reporte. - " + ex.Message);
                }
                finally
                {
                    CryReportSource.ReportDocument.Close();
                    CryReportSource.ReportDocument.Dispose();
                    CryReportSource = null;
                }
            }
            //Recibos IAF

            if (pListaArchivos.Count == 0 && (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0))
            {
                this.MostrarMensaje("ArchivoReciboNoExiste", true);
                return;
            }

            int pageCountIAF=0;
            Stream pdfFinal = new FileStream(nombre, FileMode.Create);
            using (PdfCopy writer = new PdfCopy(document, pdfFinal))
                {
                    writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_5);
                    writer.CompressionLevel = PdfStream.BEST_COMPRESSION;
                    writer.SetFullCompression();

                    document.Open();
                    foreach (HabArchivosDetalles pArchivo in pListaArchivos)
                    {
                        //create PdfReader object
                        reader = new PdfReader(pArchivo.Archivo);
                        pageCountIAF += reader.NumberOfPages;
                        reader.ConsolidateNamedDestinations();
                        writer.AddDocument(reader);
                        reader.Close();
                    }
                    //Agrego el Recibo COM
                    if (reciboCom != null && reciboCom.Length > 0)
                    {
                        reader = new PdfReader(reciboCom);
                        reader.ConsolidateNamedDestinations();
                        writer.AddDocument(reader);
                        reader.Close();
                        document.Close();
                    }

                }

            //File.WriteAllBytes(nombre, pArchivo.Archivo);
            this.ShowPdf1.FilePath = string.Concat(this.ObtenerAppPath(), "tempPDF/", nombreArchivo);
            //this.ShowPdf1.FilePath = string.Concat(this.ObtenerAppPath(), "tempPDF/", "nombreFinal.pdf");// nombreArchivo);
            this.mpePopUp.Show();
        }

        public void GenerarReporte(Objeto pDatosReporte, EnumTGEComprobantes pComprobante)
        {
            TGEComprobantes comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(pComprobante);
            this.ArmarReporte(pDatosReporte, comprobante);
        }

        public void GenerarReporte(Objeto pDatosReporte, TGEComprobantes pComprobante)
        {
            TGEComprobantes comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(pComprobante);
            this.ArmarReporte(pDatosReporte, comprobante);
        }

        private void ArmarReporte(Objeto pDatosReporte, TGEComprobantes comprobante)
        {
            string archivoReporteLeer = string.Concat(this.ObtenerAppPath(), comprobante.NombreRPT);
            //DataSet ds = CardexF.PORequisicionesReporteNotaPedido(pReqPO);
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
            DataSet ds = ReportesF.ReportesObtenerDatos(reporte);

            //ds.WriteXmlSchema(string.Concat(this.Page.Request.PhysicalApplicationPath, string.Format("\\tempPDF\\{0}XML.xml", comprobante.NombreSP)));

            CrystalReportSource CryReportSource = new CrystalReportSource();
            try
            {
                CryReportSource.CacheDuration = 1;
                CryReportSource.Report.FileName = Server.MapPath(archivoReporteLeer);
                //CryReportSource.ReportDocument.ParameterFields["UsuarioActivo"].CurrentValues.Add(this.UsuarioActivo.ApellidoNombre);
                CryReportSource.ReportDocument.SetDataSource(ds);
                string ruta = string.Concat(this.Request.PhysicalApplicationPath, "tempPDF\\", this.FileName(pDatosReporte));
                CryReportSource.ReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, ruta);
                //Stream var = CryReportSource.ReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            }
            catch (Exception ex)
            {
                //throw ex;
                throw new Exception("Ha ocurrido un error al generar el reporte. - " + ex.Message );
            }
            finally
            {
                CryReportSource.ReportDocument.Close();
                CryReportSource.ReportDocument.Dispose();
                CryReportSource = null;
            }
        }

        public void CargarReporte(RepReportes pReporte, EnumTGEComprobantes pComprobante)
        {
            this.btnVolver.Enabled = true;
            try
            {
                this.GenerarReporte(pReporte, pComprobante);
                this.ShowPdf1.FilePath = this.VirtualFilePath(this.NombreArchivo());
                this.mpePopUp.Show();
            }
            catch (Exception ex)
            {
                bool rthrow = ExceptionHandler.HandleException(ex, "AccesoDatos");
                this.MostrarMensaje(ex.Message, true);
            }
        }

        public void CargarReporte(Objeto pReporte, TGEComprobantes pComprobante)
        {
            this.btnVolver.Enabled = true;
            try
            {
                this.GenerarReporte(pReporte, pComprobante);
                //this.ShowPdf1.FilePath = this.VirtualFilePath(this.NombreArchivo());
                this.ShowPdf1.FilePath = this.VirtualFilePath(this.FileName(pReporte));
                this.mpePopUp.Show();
            }
            catch (Exception ex)
            {
                bool rthrow = ExceptionHandler.HandleException(ex, "AccesoDatos");
                this.MostrarMensaje(ex.Message, true);
            }
        }

        public void GenerarReporte(RepReportes pReporte, EnumTGEComprobantes pComprobante)
        {
            TGEComprobantes comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(pComprobante);

            pReporte.StoredProcedure = comprobante.NombreSP;
            DataSet ds = ReportesF.ReportesObtenerDatos(pReporte);

                string archivoReporteLeer = string.Concat(this.ObtenerAppPath(), comprobante.NombreRPT.Trim());
                //ds.WriteXmlSchema(string.Concat(this.Page.Request.PhysicalApplicationPath, string.Format("\\Modulos\\Reportes\\Comprobantes\\{0}XML.xml", comprobante.NombreSP)));
                CrystalReportSource CryReportSource = new CrystalReportSource();
                try
                {
                    CryReportSource.CacheDuration = 1;
                    CryReportSource.Report.FileName = Server.MapPath(archivoReporteLeer);
                    //CryReportSource.ReportDocument.ParameterFields["UsuarioActivo"].CurrentValues.Add(this.UsuarioActivo.ApellidoNombre);
                    CryReportSource.ReportDocument.SetDataSource(ds);
                    CryReportSource.ReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, string.Concat(this.Request.PhysicalApplicationPath, "tempPDF\\", this.NombreArchivo()));
                }
                catch (Exception ex)
                {
                    throw new Exception("Ha ocurrido un error al generar el reporte. - " + ex.Message);
                }
                finally
                {
                    CryReportSource.ReportDocument.Close();
                    CryReportSource.ReportDocument.Dispose();
                    CryReportSource = null;
                }
        }

        public Stream GenerarReporteArchivo(RepReportes pReporte, EnumTGEComprobantes pComprobante)
        {
            TGEComprobantes comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(pComprobante);
            string archivoReporteLeer = string.Concat(this.ObtenerAppPath(), comprobante.NombreRPT);
            pReporte.StoredProcedure = comprobante.NombreSP;
            DataSet ds = ReportesF.ReportesObtenerDatos(pReporte);

            //ds.WriteXmlSchema(string.Concat(this.Page.Request.PhysicalApplicationPath, string.Format("\\Modulos\\Reportes\\Comprobantes\\{0}XML.xml", comprobante.NombreSP)));
            Stream archivo;
            CrystalReportSource CryReportSource = new CrystalReportSource();
            try
            {
                CryReportSource.CacheDuration = 1;
                CryReportSource.Report.FileName = Server.MapPath(archivoReporteLeer);
                //CryReportSource.ReportDocument.ParameterFields["UsuarioActivo"].CurrentValues.Add(this.UsuarioActivo.ApellidoNombre);
                CryReportSource.ReportDocument.SetDataSource(ds);
                archivo = CryReportSource.ReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al generar el reporte. - " + ex.Message);
            }
            finally
            {
                CryReportSource.ReportDocument.Close();
                CryReportSource.ReportDocument.Dispose();
                CryReportSource = null;
            }

            return archivo;
        }

        public void CargarArchivo(TGEArchivos pArchivo)
        {
            this.btnVolver.Enabled = true;
            try
            {
                string nombreArchivo = pArchivo.NombreArchivo.Replace(" ", "_");
                string nombre = string.Concat(this.Request.PhysicalApplicationPath, "tempPDF\\", nombreArchivo);

                //create Document object
                Document document = new Document();
                switch (pArchivo.TipoArchivo)
                {
                    case "application/pdf":
                        //create PdfReader object
                        PdfReader reader = new PdfReader(pArchivo.Archivo);
                        PdfReader.unethicalreading = true;
                        PdfCopy copy = new PdfCopy(document, new FileStream(nombre, FileMode.Create));
                        //open the document 
                        document.Open();
                        //add page to PdfCopy 
                        copy.AddPage(copy.GetImportedPage(reader, 1));
                        //close the document object
                        break;
                    case "image/gif":
                    case "image/jpeg":
                    case "image/pjpeg":
                    case "image/png":
                    case "image/x-png":
                    case "image/tiff":
                    case "image/bmp":
                    case "image/x-xbitmap":
                    case "image/x-jg":
                    case "image/x-emf":
                    case "image/x-wmf":
                        nombreArchivo = string.Concat(nombreArchivo, ".pdf");
                        nombre = string.Concat(nombre, ".pdf");
                        PdfWriter.GetInstance(document, new FileStream(nombre, FileMode.Create));
                        document.Open();
                        iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(pArchivo.Archivo);
                        document.Add(pic);
                        break;
                    default:
                        break;
                }


                document.Close();

                //File.WriteAllBytes(nombre, pArchivo.Archivo);
                this.ShowPdf1.FilePath = string.Concat(this.ObtenerAppPath(), "tempPDF/", nombreArchivo);
                this.mpePopUp.Show();
            }
            catch (Exception ex)
            {
                bool rthrow = ExceptionHandler.HandleException(ex, "AccesoDatos");
                this.MostrarMensaje(ex.Message, true);
            }
        }

        public void CargarArchivo(List<TGEArchivos> pListaArchivos, string nombreArchivo)
        {
            this.btnVolver.Enabled = true;
            try
            {
                Document document = new Document(PageSize.A4, 0, 0, 0, 0);
                PdfReader reader;

                //nombreArchivo = string.Concat(nombreArchivo, ".pdf");
                nombreArchivo = string.Concat(nombreArchivo, "_", this.UsuarioActivo.IdUsuario.ToString().PadLeft(5, '0'), ".pdf");
                string nombre = string.Concat(this.Request.PhysicalApplicationPath, "tempPDF\\", nombreArchivo);

                using (PdfCopy writer = new PdfCopy(document, new FileStream(nombre, FileMode.Create)))
                {
                    writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_5);
                    writer.CompressionLevel = PdfStream.BEST_COMPRESSION;
                    writer.SetFullCompression();

                    document.Open();

                    foreach (TGEArchivos pArchivo in pListaArchivos)
                    {
                        //create PdfReader object
                        reader = new PdfReader(pArchivo.Archivo);
                        reader.ConsolidateNamedDestinations();
                        writer.AddDocument(reader);
                        reader.Close();
                    }
                    document.Close();
                }               
                this.ShowPdf1.FilePath = string.Concat(this.ObtenerAppPath(), "tempPDF/", nombreArchivo);
                this.mpePopUp.Show();
            }
            catch (Exception ex)
            {
                bool rthrow = ExceptionHandler.HandleException(ex, "AccesoDatos");
                this.MostrarMensaje(ex.Message, true);
            }
        }

        private string VirtualFilePath(Objeto pDatosReporte)
        {
            return string.Concat(this.ObtenerAppPath(), "tempPDF/", this.FileName(pDatosReporte));
        }

        private string FileName(Objeto pDatosReporte)
        {
            PropertyInfo prop = pDatosReporte.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() == 1).ToList()[0];
            string valor = prop.GetValue(pDatosReporte, null).ToString();
            string nombre = string.Concat(pDatosReporte.GetType().Name, "_", valor.PadLeft(10,'0')) ;
            return string.Format("{0}.pdf", nombre);
        }

        private string VirtualFilePath(string nombre)
        {
            return string.Concat(this.ObtenerAppPath(), "tempPDF/", nombre);
        }

        public string NombreArchivo()
        {
            return string.Concat("ReportesVarios_", this.UsuarioActivo.IdUsuario.ToString(), ".pdf");
        }
    }
}