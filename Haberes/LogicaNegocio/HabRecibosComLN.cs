using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Servicio.AccesoDatos;
using Haberes.Entidades;
using System.Collections;
using Comunes;
using Generales.Entidades;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Data;
using Generales.FachadaNegocio;
using Reportes.Entidades;
using Reportes.FachadaNegocio;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Comunes.LogicaNegocio;
//using EO.Web;
using System.Net.Mail;
using Auditoria.LogicaNegocio;
using Servicio.Conectividad;
using Afiliados.Entidades;
//using Ionic.Zip;
//using ICSharpCode.SharpZipLib.Zip;

namespace Haberes.LogicaNegocio
{
    class HabRecibosComLN : BaseLN<HabRecibosCom>
    {
        /// <summary>
        /// Devuelve el último Recibo COM
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public HabRecibosCom ObtenerUltimoRecibo(HabRecibosCom pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<HabRecibosCom>("HabRecibosComSeleccionarUltimo", pParametro);
        }

        public DataTable ObtenerRecibosGrilla(HabRecibosCom pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("HabRecibosComSeleccionarGrilla", pParametro);
        }

        public override List<HabRecibosCom> ObtenerListaFiltro(HabRecibosCom pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<HabRecibosCom>("[HabRecibosComSeleccionarFiltro]", pParametro);
        }
        
        /// <summary>
        /// Ejecuta el proceso para la Anulacion de un Recibo COM y reversion de cobros de cargos
        /// </summary>
        /// <param name="pParametro">IdReciboCOM, IdUsuarioEvento</param>
        /// <returns></returns>
        public bool Anular(HabRecibosCom pParametro)
        {

            Hashtable listaParametros = new Hashtable();
            listaParametros.Add("IdReciboCom", pParametro.IdReciboCom);
            listaParametros.Add("IdUsuarioEvento", pParametro.UsuarioLogueado.IdUsuarioEvento);
            string sp = "HabRecibosComProcesoBorrar";
            try
            {
                int resultado = BaseDatos.ObtenerBaseDatos().EjecutarSP(pParametro, listaParametros, sp, 0);
                if (resultado == 0)
                {
                    return false;
                }
                pParametro.ErrorAccesoDatos = false;
                pParametro.CodigoMensaje = "ResultadoTransaccion";
                return true;
            }
            catch (Exception ex)
            {
                pParametro.ErrorAccesoDatos = true;
                pParametro.ErrorException = ex.Message;
                pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                pParametro.CodigoMensajeArgs.Add(ex.Message);
                return false;
            }
        }

        public override HabRecibosCom ObtenerDatosCompletos(HabRecibosCom pParametro)
        {
            throw new NotImplementedException();
        }

        public override bool Agregar(HabRecibosCom pParametro)
        {
            throw new NotImplementedException();
        }

        public override bool Modificar(HabRecibosCom pParametro)
        {
            throw new NotImplementedException();
        }

        public bool AgregarPagos(HabRecibosComPagos pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            pParametro.IdReciboCOMPago = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "HabRecibosComPagosInsertar");
            if (pParametro.IdReciboCOMPago == 0)
                resultado = false;

            return resultado;            
        }

        public TGEArchivos ReporteRecibosCOMIAF(RepReportes pReporte)
        {
            TGEArchivos archivoResultado = new TGEArchivos();
            try
            {
                string nombreFinal = string.Empty;
                string nombre = string.Empty;
                string nombreArchivo = string.Empty;
                Document document;
                PdfReader reader;
                string parametrosNombre=string.Empty;
                foreach (RepParametros param in pReporte.Parametros)
                    parametrosNombre = string.Concat(parametrosNombre, param.ValorParametro.ToString(), "_");

                string nombreArchivoResultado = string.Concat(pReporte.AppPath, "tempPDF\\", string.Format("ReporteRecibosCOM_{0}{1}.pdf", parametrosNombre, pReporte.UsuarioLogueado.Usuario));
                string rutaRelativaResultado = string.Format("tempPDF/ReporteRecibosCOM_{0}{1}.pdf", parametrosNombre, pReporte.UsuarioLogueado.Usuario);
                archivoResultado.RutaFisica = nombreArchivoResultado;
                archivoResultado.NombreArchivo = rutaRelativaResultado;
                archivoResultado.TipoArchivo = "pdf";

                nombreArchivo = string.Format("Recibo_{0}{1}.pdf", parametrosNombre, pReporte.UsuarioLogueado.Usuario);
                nombre = string.Concat(pReporte.AppPath, "tempPDF\\", nombreArchivo);
                
                string nombreFormularioPdf = string.Concat(pReporte.AppPath, "tempPDF\\FormularioDepositoHaberes_{0}.pdf");

                if (File.Exists(nombreArchivoResultado))
                    File.Delete(nombreArchivoResultado);

                if (File.Exists(nombre))
                    File.Delete(nombre);

                //RECIBO COM
                Stream reciboCom;
                CrystalReportSource CryReportSource;
                TGEComprobantes comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(EnumTGEComprobantes.ReciboCOM);
                string archivoReporteLeer = string.Concat(pReporte.AppPath, comprobante.NombreRPT);
                RepReportes reporteCOM = new RepReportes();
                reporteCOM.StoredProcedure = comprobante.NombreSP;
                reporteCOM.Parametros.AddRange(pReporte.Parametros);

                //FORMULARIO DE AUTORIZACION DE DEPOSITO
                Stream formularioDeposito = new System.IO.MemoryStream();
                TGEComprobantes comprobanteFormulario=new TGEComprobantes();
                //CrystalReportSource crFormulario=new CrystalReportSource();
                DataSet dsFormulario=new DataSet();
                //TGEParametrosValores paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ImprimirFormularioAdicionalConRecibosHaberes);
                bool imprimirFormularioAdicional = false;// paramValor.ParametroValor.Trim() == string.Empty ? false : Convert.ToBoolean(paramValor.ParametroValor);

                reporteCOM.UsuarioLogueado = pReporte.UsuarioLogueado;
                DataSet dsRecibosCOM = ReportesF.ReportesObtenerDatos(reporteCOM);
                if (dsRecibosCOM.Tables[0].Rows.Count == 0)
                    return archivoResultado;

                RepReportes rpFormulario;
                RepParametros paramForm;
                int idAfiliado = 0;
                string afiliados = string.Empty;
                string separador = string.Empty;
                if (imprimirFormularioAdicional)
                {
                    foreach (DataRow drRciboCom in dsRecibosCOM.Tables[0].Rows)
                    {
                        if (idAfiliado != (int)drRciboCom["IdAfiliado"])
                        {
                            idAfiliado = (int)drRciboCom["IdAfiliado"];
                            afiliados = string.Concat(afiliados, separador, idAfiliado.ToString());
                            separador = ",";
                        }
                    }
                    comprobanteFormulario = TGEGeneralesF.ComprobantesObtenerDatosCompletos(EnumTGEComprobantes.HabFormularioDepositoHaberes);
                    rpFormulario = new RepReportes();
                    rpFormulario.StoredProcedure = comprobanteFormulario.NombreSP;
                    paramForm = new RepParametros();
                    paramForm.Parametro = "Afiliados";
                    paramForm.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.TextBox;
                    paramForm.ValorParametro = afiliados;
                    rpFormulario.Parametros.Add(paramForm);
                    dsFormulario = ReportesF.ReportesObtenerDatos(rpFormulario);

                    DataSet dsSocioForm;
                    int idAfi = 0;
                    if (dsFormulario.Tables.Count > 0)
                    {
                        foreach (DataRow row in dsFormulario.Tables[0].Rows)
                        {
                            idAfi = (int)row["IdAfiliado"];
                            dsSocioForm = new DataSet();
                            dsSocioForm.Tables.Add(dsFormulario.Tables[0].Clone());
                            dsSocioForm.Tables.Add(dsFormulario.Tables[1].Clone());
                            dsSocioForm.Tables.Add(dsFormulario.Tables[2].Clone());
                            dsSocioForm.Tables[0].ImportRow(dsFormulario.Tables[0].Select(string.Concat("IdAfiliado=", idAfi))[0]);

                            foreach (DataRow dr in dsFormulario.Tables[1].Rows)
                                dsSocioForm.Tables[1].ImportRow(dr);
                            foreach (DataRow dr in dsFormulario.Tables[2].Rows)
                                dsSocioForm.Tables[2].ImportRow(dr);

                            CryReportSource = new CrystalReportSource();
                            CryReportSource.CacheDuration = 1;
                            CryReportSource.Report.FileName = string.Concat(AppDomain.CurrentDomain.BaseDirectory, comprobanteFormulario.NombreRPT.Replace("/", "\\")); // Server.MapPath(archivoReporteLeer);
                            CryReportSource.ReportDocument.SetDataSource(dsSocioForm);
                            //formularioDeposito = crFormulario.ReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
                            CryReportSource.ReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, string.Format(nombreFormularioPdf, idAfi.ToString()));
                            CryReportSource.ReportDocument.Close();
                            CryReportSource.Dispose();
                        }
                    }
                    else
                    {
                        imprimirFormularioAdicional = false;
                    }
                }

                DataSet dsSocio = new DataSet();
                HabArchivosDetalles archivo;
                List<HabArchivosDetalles> reporteIAF;
                HabArchivosCabecerasLN archivoCabeceraLN = new HabArchivosCabecerasLN();
                XmlDocument xmlDoc = TGEGeneralesF.TGELeerXMLArchivo("Recursos\\ReportesConfiguraciones.xml");
                Document documentResultado = new Document(PageSize.A4, 0, 0, 0, 0);
                PdfReader readerResultado;
                Stream pdfResultado = new FileStream(nombreArchivoResultado, FileMode.Create);
                using (PdfCopy writerResultado = new PdfCopy(documentResultado, pdfResultado))
                {
                    writerResultado.SetPdfVersion(PdfWriter.PDF_VERSION_1_5);
                    writerResultado.CompressionLevel = PdfStream.BEST_COMPRESSION;
                    writerResultado.SetFullCompression();
                    documentResultado.Open();
                    
                    int idReciboCOM = 0; //(int)dsRecibosCOM.Tables[0].Rows[0]["IdReciboCOM"];
                    int rowCount = 0;
                    foreach (DataRow drRciboCom in dsRecibosCOM.Tables[0].Rows)
                    {
                        rowCount++;
                        if (idReciboCOM != (int)drRciboCom["IdReciboCOM"])
                        {
                            dsSocio = new DataSet();
                            dsSocio.Tables.Add(dsRecibosCOM.Tables[0].Clone());
                            dsSocio.Tables.Add(dsRecibosCOM.Tables[1].Clone());
                            dsSocio.Tables.Add(dsRecibosCOM.Tables[2].Clone());
                            idReciboCOM = (int)drRciboCom["IdReciboCOM"];
                        }
                        dsSocio.Tables[0].ImportRow(drRciboCom);
                        //Datos de la Empresa y LOGO
                        dsSocio.Tables[2].ImportRow(dsRecibosCOM.Tables[2].Rows[0]);

                        if (rowCount == dsRecibosCOM.Tables[0].Rows.Count
                            || idReciboCOM != (int)dsRecibosCOM.Tables[0].Rows[rowCount]["IdReciboCOM"])
                        {
                            #region Reporte

                            //Plantilla versión nueva o Cristal report
                            TGEPlantillas Plantilla = new TGEPlantillas();
                            Plantilla.Codigo = "HaberesRecibos";
                            Plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(Plantilla);

                            if (Plantilla.HtmlPlantilla.Trim().Length > 0)
                            {
                                //byte[] pdf = ReportesF.ExportPDFConvertirHtmlEnPdfMultiple(plantilla, this.MiResultadoReporte, this.MiReporte.KeysPDFCorte, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(Plantilla, dsSocio, "IdAfiliado", pReporte.UsuarioLogueado);
                                reciboCom = new MemoryStream(pdf);
                            }
                            else
                            {
                                CryReportSource = new CrystalReportSource();

                                CryReportSource.CacheDuration = 1;
                                CryReportSource.Report.FileName = string.Concat(AppDomain.CurrentDomain.BaseDirectory, comprobante.NombreRPT.Replace("/", "\\")); //archivoReporteLeer.Replace("/", "\\");
                                //CryReportSource.ReportDocument.ParameterFields["UsuarioActivo"].CurrentValues.Add(this.UsuarioActivo.ApellidoNombre);
                                CryReportSource.ReportDocument.SetDataSource(dsSocio);
                                reciboCom = CryReportSource.ReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);

                                CryReportSource.ReportDocument.Close();
                                CryReportSource.Dispose();
                            }

                            //Recibos IAF
                            int pageCountIAF = 0;
                            Stream pdfFinal = new FileStream(nombre, FileMode.Create);
                            document = new Document(PageSize.A4, 0, 0, 0, 0);
                            using (PdfCopy writer = new PdfCopy(document, pdfFinal))
                            {
                                writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_5);
                                writer.CompressionLevel = PdfStream.BEST_COMPRESSION;
                                writer.SetFullCompression();

                                document.Open();
                                archivo = new HabArchivosDetalles();
                                archivo.IdAfiliado = (int)drRciboCom["IdAfiliado"]; // this.MiAfiliado.IdAfiliado;

                                int perido = (int)drRciboCom["Periodo"];// Convert.ToInt32(this.ddlAnios.SelectedValue);
                                archivo.ArchivoCabecera.Anio = Convert.ToInt32(perido.ToString().Substring(0, 4));
                                archivo.ArchivoCabecera.Mes = Convert.ToInt32(perido.ToString().Substring(4, 2));
                                archivo.ArchivoCabecera.RemesaTipo.IdRemesaTipo = (int)drRciboCom["IdTipoRecibo"]; //Convert.ToInt32(this.ddlTipo.SelectedValue);
                                archivo.UsuarioLogueado = pReporte.UsuarioLogueado;
                                reporteIAF = archivoCabeceraLN.ObtenerDatosCompletos(archivo);
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
                                reader = new PdfReader(reciboCom);
                                reader.ConsolidateNamedDestinations();
                                writer.AddDocument(reader);
                                reader.Close();

                                //FORMULARIO DE AUTORIZACION DE DEPOSITO
                                if (imprimirFormularioAdicional)
                                {
                                    reader = new PdfReader(string.Format(nombreFormularioPdf, archivo.IdAfiliado.ToString()));
                                    reader.ConsolidateNamedDestinations();
                                    writer.AddDocument(reader);
                                    reader.Close();
                                }

                                document.Close();
                            }

                            #region SelloPago
                                //pReporte.StoredProcedure = "ReportesHabRecibosComSelloComprobante";
                                //ds = ReportesF.ReportesObtenerDatos(pReporte);
                                bool grabarSello = false;
                                if (dsSocio.Tables.Count > 0 && dsSocio.Tables[0].Rows.Count > 0
                                    && dsSocio.Tables[0].Rows[0]["GrabarSello"] != DBNull.Value && (int)dsSocio.Tables[0].Rows[0]["GrabarSello"] == 1)
                                {
                                    DataRow dr = dsSocio.Tables[0].Rows[0];
                                    PdfPTable table = new PdfPTable(1);
                                    PdfPTable tableSupervivencia = new PdfPTable(1);
                                    Font font = FontFactory.GetFont("ARIAL", 9);
                                    Font fontBold = FontFactory.GetFont("ARIAL", 10, Font.BOLD);

                                    PdfPCell cell;

                                    if (dr["ImprimirSelloSupervivencia"] != DBNull.Value && (int)dr["ImprimirSelloSupervivencia"] == 1)
                                    {
                                        /* Sello Supervivencia (Pedido por Carlos Anaya para Recibos de Abril */
                                        cell = new PdfPCell(new Phrase("IMPORTANTE", fontBold));
                                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        cell.DisableBorderSide(PdfPCell.BOTTOM_BORDER);
                                        cell.BorderColor = new iTextSharp.text.BaseColor(0, 100, 0);
                                        cell.BorderWidth = 2;
                                        tableSupervivencia.AddCell(cell);

                                        cell = new PdfPCell(new Phrase("Por requerimiento del I.A.F.P.R.P.M., informamos a Ud. que con los Recibos de Haberes correspondientes al mes de MAYO/17, recibirá y deberá cumplimentar el “CERTIFICADO DE SUPERVIVENCIA Y/O DECLARACION DE ESTADO CIVIL”.", fontBold));
                                        cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                                        cell.DisableBorderSide(PdfPCell.TOP_BORDER);
                                        cell.BorderColor = new iTextSharp.text.BaseColor(0, 100, 0);
                                        cell.BorderWidth = 2;
                                        tableSupervivencia.AddCell(cell);
                                    }

                                    /* Sello Fecha Pago Recibo IAF */
                                    cell = new PdfPCell(new Phrase(dr["Empresa"].ToString(), font));
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    cell.DisableBorderSide(PdfPCell.BOTTOM_BORDER);
                                    cell.BorderColor = new iTextSharp.text.BaseColor(0, 100, 0);
                                    cell.BorderWidth = 2;
                                    table.AddCell(cell);
                                    if (dr["IdFilial"] != DBNull.Value && (int)dr["IdFilial"] > 0)
                                    {
                                        grabarSello = true;
                                        cell = new PdfPCell(new Phrase(string.Concat(((DateTime)dr["FechaPago"]).ToShortDateString(), " - Caja Nro. ", dr["NumeroCaja"].ToString()), font));
                                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        cell.DisableBorderSide(PdfPCell.TOP_BORDER);
                                        cell.DisableBorderSide(PdfPCell.BOTTOM_BORDER);
                                        cell.BorderColor = new iTextSharp.text.BaseColor(0, 100, 0);
                                        cell.BorderWidth = 2;
                                        table.AddCell(cell);
                                        cell = new PdfPCell(new Phrase("PAGADO", fontBold));
                                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        cell.DisableBorderSide(PdfPCell.TOP_BORDER);
                                        cell.DisableBorderSide(PdfPCell.BOTTOM_BORDER);
                                        cell.BorderColor = new iTextSharp.text.BaseColor(0, 100, 0);
                                        cell.BorderWidth = 2;
                                        table.AddCell(cell);
                                        cell = new PdfPCell(new Phrase(string.Concat("Responsable: ", dr["Usuario"].ToString()), font));
                                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        cell.DisableBorderSide(PdfPCell.TOP_BORDER);
                                        cell.BorderColor = new iTextSharp.text.BaseColor(0, 100, 0);
                                        cell.BorderWidth = 2;
                                        cell.PaddingBottom = 5;
                                        table.AddCell(cell);
                                    }
                                    else if (dr["IdBanco"] != DBNull.Value && (int)dr["IdBanco"] > 0)
                                    {
                                        grabarSello = true;
                                        cell = new PdfPCell(new Phrase("POR PODER", fontBold));
                                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        cell.DisableBorderSide(PdfPCell.TOP_BORDER);
                                        cell.DisableBorderSide(PdfPCell.BOTTOM_BORDER);
                                        cell.BorderColor = new iTextSharp.text.BaseColor(0, 100, 0);
                                        cell.BorderWidth = 2;
                                        table.AddCell(cell);

                                        cell = new PdfPCell(new Phrase(string.Concat("Fecha Acreditacion: ", dr["FechaAcreditacion"] == DBNull.Value ? string.Empty : ((DateTime)dr["FechaAcreditacion"]).ToShortDateString()), font));
                                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        cell.DisableBorderSide(PdfPCell.TOP_BORDER);
                                        cell.DisableBorderSide(PdfPCell.BOTTOM_BORDER);
                                        cell.BorderColor = new iTextSharp.text.BaseColor(0, 100, 0);
                                        cell.BorderWidth = 2;
                                        //cell.PaddingBottom = 5;
                                        table.AddCell(cell);

                                        cell = new PdfPCell(new Phrase(dr["Banco"].ToString(), font));
                                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        cell.DisableBorderSide(PdfPCell.TOP_BORDER);
                                        cell.BorderColor = new iTextSharp.text.BaseColor(0, 100, 0);
                                        cell.BorderWidth = 2;
                                        cell.PaddingBottom = 5;
                                        table.AddCell(cell);
                                    }
                                    else
                                    {
                                        grabarSello = true;

                                        cell = new PdfPCell(new Phrase(" ", font));
                                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        cell.DisableBorderSide(PdfPCell.TOP_BORDER);
                                        cell.DisableBorderSide(PdfPCell.BOTTOM_BORDER);
                                        cell.BorderColor = new iTextSharp.text.BaseColor(0, 100, 0);
                                        cell.BorderWidth = 2;
                                        //cell.PaddingBottom = 5;
                                        table.AddCell(cell);

                                        cell = new PdfPCell(new Phrase(string.Concat("Fecha PAGO IAF: ", dr["FechaPagoIAF"] == DBNull.Value ? string.Empty : ((DateTime)dr["FechaPagoIAF"]).ToShortDateString()), font));
                                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        cell.DisableBorderSide(PdfPCell.TOP_BORDER);
                                        //cell.DisableBorderSide(PdfPCell.BOTTOM_BORDER);
                                        cell.BorderColor = new iTextSharp.text.BaseColor(0, 100, 0);
                                        cell.BorderWidth = 2;
                                        cell.PaddingBottom = 5;
                                        table.AddCell(cell);

                                        cell = new PdfPCell(new Phrase(" ", font));
                                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        cell.DisableBorderSide(PdfPCell.TOP_BORDER);
                                        //cell.DisableBorderSide(PdfPCell.BOTTOM_BORDER);
                                        cell.BorderColor = new iTextSharp.text.BaseColor(0, 100, 0);
                                        cell.BorderWidth = 2;
                                        cell.PaddingBottom = 5;
                                        table.AddCell(cell);
                                    }

                                    if (grabarSello)
                                    {
                                        PdfStamper pdfStamper;// = new PdfStamper(
                                        PdfContentByte waterMark;
                                        nombreFinal = string.Concat(nombre.Substring(0, nombre.Length - 3), "Sello.pdf");
                                        nombreArchivo = string.Concat(nombreArchivo.Substring(0, nombreArchivo.Length - 3), "Sello.pdf");
                                        pdfFinal = new FileStream(nombreFinal, FileMode.Create);
                                        reader = new PdfReader(nombre);
                                        pdfStamper = new PdfStamper(reader, pdfFinal);

                                        float lowerLeftXSup, lowerLeftYSup, upperRightXSup, upperRightYSup;
                                        lowerLeftXSup = float.Parse(xmlDoc.SelectSingleNode("/RecibosIAF/SelloSupervivencia/PDFPosicion/lowerLeftX").InnerText);// 310; //Eje X desde
                                        upperRightXSup = float.Parse(xmlDoc.SelectSingleNode("/RecibosIAF/SelloSupervivencia/PDFPosicion/upperRightX").InnerText);//550; //Eje X hasta (Ancho de la tabla)
                                        upperRightYSup = float.Parse(xmlDoc.SelectSingleNode("/RecibosIAF/SelloSupervivencia/PDFPosicion/upperRightY").InnerText);//305; // - lo baja, + lo sube, no puede ser <= a lowerLeftY
                                        lowerLeftYSup = float.Parse(xmlDoc.SelectSingleNode("/RecibosIAF/SelloSupervivencia/PDFPosicion/lowerLeftY").InnerText);//247;  // Le doy el Alto con la Dif a upperRighty

                                        float lowerLeftX, lowerLeftY, upperRightX, upperRightY;
                                        lowerLeftX = float.Parse(xmlDoc.SelectSingleNode("/RecibosIAF/SelloCajaSocio/PDFPosicion/lowerLeftX").InnerText);// 310; //Eje X desde
                                        upperRightX = float.Parse(xmlDoc.SelectSingleNode("/RecibosIAF/SelloCajaSocio/PDFPosicion/upperRightX").InnerText);//550; //Eje X hasta (Ancho de la tabla)
                                        upperRightY = float.Parse(xmlDoc.SelectSingleNode("/RecibosIAF/SelloCajaSocio/PDFPosicion/upperRightY").InnerText);//305; // - lo baja, + lo sube, no puede ser <= a lowerLeftY
                                        lowerLeftY = float.Parse(xmlDoc.SelectSingleNode("/RecibosIAF/SelloCajaSocio/PDFPosicion/lowerLeftY").InnerText);//247;  // Le doy el Alto con la Dif a upperRighty

                                        float lowerLeftX2, lowerLeftY2, upperRightX2, upperRightY2;
                                        lowerLeftX2 = float.Parse(xmlDoc.SelectSingleNode("/RecibosIAF/SelloCajaCOM/PDFPosicion/lowerLeftX").InnerText);// 310; //Eje X desde
                                        upperRightX2 = float.Parse(xmlDoc.SelectSingleNode("/RecibosIAF/SelloCajaCOM/PDFPosicion/upperRightX").InnerText);//550; //Eje X hasta (Ancho de la tabla)
                                        upperRightY2 = float.Parse(xmlDoc.SelectSingleNode("/RecibosIAF/SelloCajaCOM/PDFPosicion/upperRightY").InnerText);//305; // - lo baja, + lo sube, no puede ser <= a lowerLeftY
                                        lowerLeftY2 = float.Parse(xmlDoc.SelectSingleNode("/RecibosIAF/SelloCajaCOM/PDFPosicion/lowerLeftY").InnerText);//247;  // Le doy el Alto con la Dif a upperRighty

                                        PdfPTable table2 = new PdfPTable(1);
                                        cell = new PdfPCell(new Phrase(" ", font));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.DisableBorderSide(PdfPCell.TOP_BORDER);
                                        cell.DisableBorderSide(PdfPCell.BOTTOM_BORDER);
                                        cell.BorderColor = new iTextSharp.text.BaseColor(0, 100, 0);
                                        cell.BorderWidth = 2;
                                        //cell.PaddingBottom = 5;
                                        table2.AddCell(cell);
                                        cell = new PdfPCell(new Phrase(string.Concat(xmlDoc.SelectSingleNode("/RecibosIAF/SelloCajaCOM/Aclaracion").InnerText, dr["PagoApellidoNombre"].ToString()), font));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.DisableBorderSide(PdfPCell.TOP_BORDER);
                                        cell.DisableBorderSide(PdfPCell.BOTTOM_BORDER);
                                        cell.BorderColor = new iTextSharp.text.BaseColor(0, 100, 0);
                                        cell.BorderWidth = 2;
                                        //cell.PaddingBottom = 5;
                                        table2.AddCell(cell);

                                        //cell = new PdfPCell(new Phrase(" ", font));
                                        //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //cell.DisableBorderSide(PdfPCell.TOP_BORDER);
                                        //cell.DisableBorderSide(PdfPCell.BOTTOM_BORDER);
                                        //cell.BorderColor = new iTextSharp.text.BaseColor(0, 100, 0);
                                        //cell.BorderWidth = 2;
                                        ////cell.PaddingBottom = 5;
                                        //table.AddCell(cell);
                                        cell = new PdfPCell(new Phrase(string.Concat(xmlDoc.SelectSingleNode("/RecibosIAF/SelloCajaCOM/TipoNroDoc").InnerText, dr["PagoTipoNumeroDocumento"].ToString()), font));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.DisableBorderSide(PdfPCell.TOP_BORDER);
                                        cell.DisableBorderSide(PdfPCell.BOTTOM_BORDER);
                                        cell.BorderColor = new iTextSharp.text.BaseColor(0, 100, 0);
                                        cell.BorderWidth = 2;
                                        //cell.PaddingBottom = 5;
                                        table2.AddCell(cell);

                                        //cell = new PdfPCell(new Phrase(" ", font));
                                        //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //cell.DisableBorderSide(PdfPCell.TOP_BORDER);
                                        //cell.DisableBorderSide(PdfPCell.BOTTOM_BORDER);
                                        //cell.BorderColor = new iTextSharp.text.BaseColor(0, 100, 0);
                                        //cell.BorderWidth = 2;
                                        ////cell.PaddingBottom = 5;
                                        //table.AddCell(cell);
                                        cell = new PdfPCell(new Phrase(string.Concat(xmlDoc.SelectSingleNode("/RecibosIAF/SelloCajaCOM/Domicilio").InnerText, dr["PagoDomicilio"].ToString()), font));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.DisableBorderSide(PdfPCell.TOP_BORDER);
                                        cell.DisableBorderSide(PdfPCell.BOTTOM_BORDER);
                                        cell.BorderColor = new iTextSharp.text.BaseColor(0, 100, 0);
                                        cell.BorderWidth = 2;
                                        //cell.PaddingBottom = 5;
                                        table2.AddCell(cell);

                                        cell = new PdfPCell(new Phrase(" ", font));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.DisableBorderSide(PdfPCell.TOP_BORDER);
                                        //cell.DisableBorderSide(PdfPCell.BOTTOM_BORDER);
                                        cell.BorderColor = new iTextSharp.text.BaseColor(0, 100, 0);
                                        cell.BorderWidth = 2;
                                        //cell.PaddingBottom = 5;
                                        table2.AddCell(cell);

                                        ColumnText ct;
                                        ColumnText ct2;
                                        //int PageCount = reader.NumberOfPages;
                                        for (int x = 1; x <= pageCountIAF; x++)
                                        {
                                            waterMark = pdfStamper.GetOverContent(x);

                                            ct = new ColumnText(waterMark);
                                            ct.Alignment = Element.ALIGN_CENTER;
                                            ct.SetSimpleColumn(lowerLeftXSup, lowerLeftYSup, upperRightXSup, upperRightYSup);
                                            ct.AddElement(tableSupervivencia);
                                            ct.Go();

                                            ct = new ColumnText(waterMark);
                                            ct.Alignment = Element.ALIGN_CENTER;
                                            ct.SetSimpleColumn(lowerLeftX, lowerLeftY, upperRightX, upperRightY);
                                            ct.AddElement(table);
                                            ct.Go();

                                            //ct2 = new ColumnText(waterMark);
                                            //ct2.SetSimpleColumn(lowerLeftX2, lowerLeftY2, upperRightX2, upperRightY2);
                                            //ct2.AddElement(table);
                                            //ct2.AddElement(table2);
                                            //ct2.Go();
                                        }
                                        pdfStamper.Close();
                                        reader.Close();
                                    }
                            }
                            #endregion

                            #endregion
                            if (grabarSello)
                                readerResultado = new PdfReader(nombreFinal);
                            else
                                readerResultado = new PdfReader(nombre);
                            readerResultado.ConsolidateNamedDestinations();
                            writerResultado.AddDocument(readerResultado);
                            readerResultado.Close();

                        }

                    }

                    documentResultado.Close();
                }

                ////Comprimo a RAR.
                //string rutaZip = archivoResultado.RutaFisica;
                //rutaZip = @"C:\Users\Agula\Documents\EVOL\Clientes\COM\Anio 2014\Aplicacion\ERP COM\IU\tempPDF\tmpResultadoRecibo_alaplacette.pdf";
                //archivoResultado.NombreArchivo = string.Concat(archivoResultado.NombreArchivo, ".zip");
                //archivoResultado.RutaFisica = string.Concat(archivoResultado.RutaFisica, ".zip");
                //if (File.Exists(archivoResultado.NombreArchivo))
                //    File.Delete(archivoResultado.NombreArchivo);


                //System.Diagnostics.Process p = new System.Diagnostics.Process();
                //p.StartInfo.CreateNoWindow = true;
                //p.StartInfo.UseShellExecute = false;
                //p.StartInfo.WorkingDirectory = @"C:\Users\Agula\Documents\EVOL\Clientes\COM\Anio 2014\Aplicacion\ERP COM\IU\tempPDF\";
                //p.StartInfo.FileName = @"C:\Program Files\WinRAR\rar.exe";
                //p.StartInfo.Arguments = string.Format("a -ep \"{0}\" \"{1}\"", "tmpResultadoRecibo_alaplacette.pdf", "tmpResultadoRecibo_alaplacette.rar");
                //p.Start();
                //p.WaitForExit();

                //using (ZipFile zip = new ZipFile())
                //{
                //    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                //    zip.AddFile(rutaZip, string.Empty);
                //    //zip.CompressionMethod = CompressionMethod.BZip2;
                //    zip.UseZip64WhenSaving = Zip64Option.AsNecessary;
                //    zip.Save(archivoResultado.RutaFisica);
                //}

                // 'using' statements guarantee the stream is closed properly which is a big source
                // of problems otherwise.  Its exception safe as well which is great.

                //using (ZipOutputStream s = new ZipOutputStream(File.Create(archivoResultado.RutaFisica)))
                //{

                //    s.SetLevel(9); // 0 - store only to 9 - means best compression

                //    byte[] buffer = new byte[4096];


                //    // Using GetFileName makes the result compatible with XP
                //    // as the resulting path is not absolute.
                //    ZipEntry entry = new ZipEntry(Path.GetFileName(rutaZip));

                //    // Setup the entry data as required.

                //    // Crc and size are handled by the library for seakable streams
                //    // so no need to do them here.

                //    // Could also use the last write time or similar for the file.
                //    entry.DateTime = DateTime.Now;
                //    s.PutNextEntry(entry);

                //    using (FileStream fs = File.OpenRead(rutaZip))
                //    {
                //        // Using a fixed size buffer here makes no noticeable difference for output
                //        // but keeps a lid on memory usage.
                //        int sourceBytes;
                //        do
                //        {
                //            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                //            s.Write(buffer, 0, sourceBytes);
                //        } while (sourceBytes > 0);
                //    }

                //    // Finish/Close arent needed strictly as the using statement does this automatically

                //    // Finish is important to ensure trailing information for a Zip file is appended.  Without this
                //    // the created file would be invalid.
                //    s.Finish();

                //    // Close is important to wrap things up and unlock the file.
                //    s.Close();
                //}

            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message, "Haberes Logica de Negocio", 1, 0, System.Diagnostics.TraceEventType.Critical, "ReporteRecibosCOMIAF");
                throw new Exception("Ha ocurrido un error al generar el Reporte PDF Recibo COM/IAF - " + ex.Message);
            }
            finally
            {
            }
            return archivoResultado;
        }

        public bool ArmarMailReciboHaberes(HabRemesasDetalles pParametro, MailMessage mail)
        {
            try
            {
                bool resultado = true;
                string template = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "templates\\MailEnviarReciboHaberes.htm");

                //mail.From = new MailAddress("argentina.proveedores@hpidc.com", "Argentina proveedores");
                //mail.Bcc.Add(new MailAddress("argentina.proveedores@hpidc.com", "Argentina proveedores"));
                if (pParametro.Afiliado.CorreoElectronico.Trim() != string.Empty)
                {
                    if (pParametro.Afiliado.CorreoElectronico.Trim().Contains(";")
                        || pParametro.Afiliado.CorreoElectronico.Trim().Contains(","))
                    {
                        pParametro.Afiliado.CorreoElectronico = pParametro.Afiliado.CorreoElectronico.Replace(",", ";");
                        List<string> lista = pParametro.Afiliado.CorreoElectronico.Trim().Split(';').ToList();
                        foreach (string item in lista)
                        {
                            if (AyudaProgramacionLN.IsEmailValid(item.Trim()))
                            {
                                mail.To.Add(new MailAddress(item.Trim(), pParametro.Afiliado.ApellidoNombre.Trim()));
                            }
                        }
                    }
                    else
                    {
                        if (AyudaProgramacionLN.IsEmailValid(pParametro.Afiliado.CorreoElectronico.Trim()))
                        {
                            mail.To.Add(new MailAddress(pParametro.Afiliado.CorreoElectronico.Trim(), pParametro.Afiliado.ApellidoNombre.Trim()));
                        }
                    }
                }
                if (mail.To.Count == 0)
                    return false;
                
                mail.Subject = string.Concat("Recibos de Haberes - ", pParametro.Periodo.ToString());
                mail.IsBodyHtml = true;
                mail.Body = new StreamReader(template).ReadToEnd();
                mail.Body = mail.Body.Replace("%ApellidoNombre%", string.Concat(pParametro.Afiliado.Nombre, " ", pParametro.Afiliado.Apellido));
                mail.Body = mail.Body.Replace("%Periodo%", pParametro.Periodo.ToString());
                mail.Body = mail.Body.Replace("%Empresa%", string.Empty);

                RepReportes reporte = new RepReportes();
                reporte.IdReporte = (int)EnumReportes.RecibosHaberes;
                reporte = ReportesF.ReportesObtenerUno(reporte);
                reporte.Parametros.Find(x => x.Parametro == "Periodo").ValorParametro = pParametro.Periodo;
                reporte.Parametros.Find(x => x.Parametro == "IdTipoRecibo").ValorParametro = pParametro.RemesaTipo.IdRemesaTipo;
                if (reporte.Parametros.Exists(x => x.Parametro == "IdAfiliado"))
                    reporte.Parametros.Find(x => x.Parametro == "IdAfiliado").ValorParametro = pParametro.Afiliado.IdAfiliado;
                else
                {
                    RepParametros paramAfi = new RepParametros();
                    paramAfi.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
                    paramAfi.Parametro = "IdAfiliado";
                    paramAfi.ValorParametro = pParametro.Afiliado.IdAfiliado;
                    reporte.Parametros.Add(paramAfi);
                }

                reporte.AppPath = System.AppDomain.CurrentDomain.BaseDirectory;
                reporte.UsuarioLogueado = pParametro.UsuarioLogueado;
                TGEArchivos archivo = this.ReporteRecibosCOMIAF(reporte);
                FileInfo fi = new FileInfo(archivo.RutaFisica);
                string nombre = string.Format("RECIBO_HABERES_{0}_{1}.pdf", pParametro.Periodo.ToString(), string.Concat(pParametro.Afiliado.TipoDocumento.TipoDocumento, "_", pParametro.Afiliado.NumeroDocumento.ToString()));
                string pathNombre = string.Concat(System.AppDomain.CurrentDomain.BaseDirectory, "tempPDF\\", nombre);

                if (File.Exists(pathNombre))
                    File.Delete(pathNombre);
                fi.MoveTo(pathNombre);

                archivo.Archivo = System.IO.File.ReadAllBytes(pathNombre);
                archivo.NombreArchivo = nombre;
                if (archivo.Archivo != null && archivo.Archivo.LongLength > 0)
                    mail.Attachments.Add(new Attachment(new MemoryStream(archivo.Archivo), archivo.NombreArchivo));

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat(ex.Message, " ", pParametro.Afiliado.CorreoElectronico.Trim(), " - ", pParametro.ApellidoNombre), ex.InnerException);
            }
        }

    }
}
