using Mailing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Reportes.FachadaNegocio;
using Comunes.Entidades;
using System.Reflection;
using iTextSharp.text.pdf.codec.wmf;
using Reportes.Entidades;
using Haberes.Entidades;
using System.IO;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System.Data;
using CrystalDecisions.Web;
using Haberes;
using CrystalDecisions.Shared;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace IU.Modulos.Reportes
{
    /// <summary>
    /// Descripción breve de ReportesWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class ReportesWS : System.Web.Services.WebService
    {

        [WebMethod]
        public byte[] GenerarComprobante(string strNamesapace, string strPlantilla, string key, string value)
        {
            if (strPlantilla.ToLower() == "reciboiafpropio")
            {
                string[] values = value.Split('|');
                RepReportes reporte = new RepReportes();
                //reporte.IdReporte = (int)EnumReportes.ReciboCOM;
                //reporte = ReportesF.ReportesObtenerUno(reporte);
                RepParametros parametro = new RepParametros();
                parametro.Parametro = "Periodo";
                parametro.ValorParametro = values[2]; //, this.ddlMeses.SelectedValue);
                parametro.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
                reporte.Parametros.Add(parametro);
                parametro = new RepParametros();
                parametro.Parametro = "IdAfiliado";
                parametro.ValorParametro = values[1];
                parametro.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
                reporte.Parametros.Add(parametro);
                parametro = new RepParametros();
                parametro.Parametro = "IdTipoRecibo";
                parametro.ValorParametro = values[3];
                parametro.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
                reporte.Parametros.Add(parametro);
                UsuarioLogueado usuario = new UsuarioLogueado();
                usuario.IdUsuario = 1;
                usuario.IdUsuarioEvento = 1;
                reporte.UsuarioLogueado = usuario;

                //reporte.AppPath = this.Request.PhysicalApplicationPath;
                //this.ctrPopUpComprobantes.CargarArchivo(HaberesF.HabRecibosReporteRecibosCOMIAF(reporte));

                HabArchivosDetalles archivo = new HabArchivosDetalles();
                archivo.IdAfiliado = Convert.ToInt32(values[1]);
                archivo.UsuarioLogueado = usuario;

                int perido = Convert.ToInt32(values[2]);
                archivo.ArchivoCabecera.Anio = Convert.ToInt32(perido.ToString().Substring(0, 4));
                archivo.ArchivoCabecera.Mes = Convert.ToInt32(perido.ToString().Substring(4, 2));
                archivo.ArchivoCabecera.RemesaTipo.IdRemesaTipo = Convert.ToInt32(values[3]);
                List<HabArchivosDetalles> reporteIAF = HaberesF.ArchivosDetallesSeleccionar(archivo);

                //RECIBO COM
                Stream reciboCom = new System.IO.MemoryStream();
                TGEComprobantes comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(EnumTGEComprobantes.ReciboCOM);
                string archivoReporteLeer = string.Concat(this.ObtenerAppPath(), comprobante.NombreRPT);
                reporte.StoredProcedure = comprobante.NombreSP;
                DataSet ds = ReportesF.ReportesObtenerDatos(reporte);
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

                string nombre = string.Empty;
                string nombreArchivo = string.Empty;
                Document document = new Document(PageSize.A4, 0, 0, 0, 0);
                PdfReader reader;

                if (reporteIAF.Count > 0)
                {
                    nombreArchivo = reporteIAF[0].NombreArchivo;
                    if (nombreArchivo.Length > 0)
                    {
                        nombre = string.Concat(HttpContext.Current.Server.MapPath("~/"), "tempPDF\\", reporteIAF[0].NombreArchivo);
                    }
                }
                else
                {
                    nombreArchivo = "tmpRecibo.pdf";
                    nombre = string.Concat(HttpContext.Current.Server.MapPath("~/"), "tempPDF\\", nombreArchivo);
                }

                nombre = string.Concat(nombre, "_", usuario.IdUsuario.ToString().PadLeft(5, '0'), ".pdf");
                nombreArchivo = string.Concat(nombreArchivo, "_", usuario.IdUsuario.ToString().PadLeft(5, '0'), ".pdf");

                if (File.Exists(nombre))
                    File.Delete(nombre);

                int pageCountIAF = 0;
                byte[] recibo;
                Stream pdfFinal = new FileStream(nombre, FileMode.Create);
                using (PdfCopy writer = new PdfCopy(document, pdfFinal))
                {
                    writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_5);
                    writer.CompressionLevel = PdfStream.BEST_COMPRESSION;
                    writer.SetFullCompression();

                    document.Open();
                    foreach (HabArchivosDetalles pArchivo in reporteIAF)
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
                    }

                    document.Close();

                }
                recibo = File.ReadAllBytes(nombre);

                return recibo;
            }

            Type t = Type.GetType(strNamesapace);
            
            if (t == null) { throw new Exception("El Tipo de datos (clase) es incorrecto o no esta definido"); };
            var obj = Activator.CreateInstance(t);

            t.GetProperty(key).SetValue(obj, Convert.ToInt32(value));
            UsuarioLogueado usu = new UsuarioLogueado();
            usu.IdUsuario = 120;

            byte[] doc = ReportesF.ExportPDFGenerarReportePDF(Generales.Entidades.EnumTGEComprobantes.SinComprobante, strPlantilla, (Objeto)obj, usu);
            return doc;
        }
        protected string ObtenerAppPath()
        {
            //return HttpContext.Current.Server.MapPath("~/");
            return HttpContext.Current.Request.ApplicationPath.EndsWith("/") ? HttpContext.Current.Request.ApplicationPath : string.Concat(HttpContext.Current.Request.ApplicationPath, "/");
        }
    }
}
