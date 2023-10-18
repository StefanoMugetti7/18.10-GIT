using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.pdf.parser;
using Haberes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Servicio.AccesoDatos;
using Afiliados;
using Afiliados.Entidades;
//using EO.Web;
using Comunes.Entidades;
using Generales.Entidades;
using Comunes.LogicaNegocio;
using Auditoria;
using Generales.FachadaNegocio;
using SharpCompress.Archive;

namespace Haberes.LogicaNegocio
{
    public class ImportarArchivosLN
    {
        //ProgressTaskEventArgs progreso;
        string extraData = string.Empty;
        int porcentajeProgreso = 0;

        public delegate void ImportarArchivosEjecutarSPMensajes(List<string> e);
        public event ImportarArchivosEjecutarSPMensajes ImportarArchivosEjecutarSPMensajesCallback;

        public List<string> mensajesCallback;

        //public bool ImportarPdfIaf(HabArchivosCabeceras pObjteo, ProgressTaskEventArgs pProgreso)
        //{
        //    AyudaProgramacionLN.LimpiarMensajesError(pObjteo);

        //    PdfCopy copy;
        //    PdfReader reader;
        //    //Document document;
        //    Document document;// = new Document(PageSize.A4, 0, 0, 0, 0);
        //    PdfImportedPage importedPage;
        //    //string ruta = "C:\\Users\\alaplacette\\Google Drive\\Auge Consultores\\Clientes\\COM\\Anio 2014\\Aplicacion\\ERP COM\\IU\\";

        //    AfiAfiliados afiliado;
        //    HabArchivosDetalles archivoDetalle;

        //    this.progreso = pProgreso;
        //    String source_file = pObjteo.AppPath + "PdfEntrada\\";
        //    String result = pObjteo.AppPath + "PdfSalida\\";

        //    bool resultado = true;
        //    extraData = string.Empty;
        //    string msgProgreso = "Importando datos IAF {0} de {1}.<BR /><BR />";

        //    porcentajeProgreso = 1;
        //    int cantidadTotal = 0;
        //    int contador = 0;

        //    pObjteo.Estado.IdEstado = (int)Estados.Activo;

        //    DbTransaction tran;
        //    DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

        //    using (DbConnection con = bd.CreateConnection())
        //    {
        //        con.Open();
        //        tran = con.BeginTransaction();

        //        try
        //        {
        //            //Guardo los archivos ingresados
        //            pObjteo.IdArchivoCabecera = BaseDatos.ObtenerBaseDatos().Agregar(pObjteo, bd, tran, "HabArchivosCabecerasInsertar");
        //            if (pObjteo.IdArchivoCabecera == 0)
        //                resultado = false;

        //            if (resultado && !TGEGeneralesF.ArchivosActualizar(pObjteo, bd, tran))
        //                resultado = false;

        //            if (resultado)
        //            {
        //                foreach (TGEArchivos archivo in pObjteo.Archivos)
        //                {
        //                    //create PdfReader object
        //                    reader = new PdfReader(archivo.Archivo);
        //                    cantidadTotal += reader.NumberOfPages;
        //                }

        //                foreach (TGEArchivos archivo in pObjteo.Archivos)
        //                {
        //                    //create PdfReader object
        //                    reader = new PdfReader(archivo.Archivo);

        //                    for (int i = 1; i <= reader.NumberOfPages; i++)
        //                    {
        //                        //Empieza Importación de datos
        //                        contador++;
        //                        extraData = string.Format(msgProgreso, contador, cantidadTotal);
        //                        porcentajeProgreso = (contador * 100 / cantidadTotal);
        //                        this.ActualizarProgreso(porcentajeProgreso, extraData);

        //                        document = new Document(reader.GetPageSizeWithRotation(i));
        //                        copy = new PdfCopy(document, new FileStream(result + "ReciboTemp.pdf", FileMode.Create));
        //                        copy.SetPdfVersion(PdfWriter.PDF_VERSION_1_7);
        //                        copy.CompressionLevel = PdfStream.BEST_COMPRESSION;
        //                        copy.SetFullCompression();
        //                        document.Open();
        //                        importedPage = copy.GetImportedPage(reader, i);
        //                        copy.AddPage(importedPage);
        //                        document.Close();

        //                        archivoDetalle = new HabArchivosDetalles();
        //                        archivoDetalle.Archivo = File.ReadAllBytes(result + "ReciboTemp.pdf");
        //                        archivoDetalle.NombreArchivo = NombreArchivoPDF(ExtractTextFromPdf(archivoDetalle.Archivo), pObjteo.Anio, pObjteo.Mes);
        //                        archivoDetalle.TipoArchivo = "pdf";
        //                        archivoDetalle.Tamanio = archivoDetalle.Archivo.Length;
        //                        //archivoDetalle.NombreArchivo = string.Concat(nombre, ".pdf");
        //                        afiliado = new AfiAfiliados();
        //                        afiliado.TipoDocumento.TipoDocumento = archivoDetalle.NombreArchivo.Split('-')[0];
        //                        afiliado.NumeroDocumento = Convert.ToInt32(archivoDetalle.NombreArchivo.Split('-')[1]);
        //                        afiliado = AfiliadosF.AfiliadosObtenerPorTipoDocumento(afiliado);
        //                        archivoDetalle.IdAfiliado = afiliado.IdAfiliado;

        //                        archivoDetalle.Estado.IdEstado = (int)Estados.Activo;
        //                        archivoDetalle.UsuarioLogueado = archivo.UsuarioLogueado;

        //                        if (File.Exists(result + archivoDetalle.NombreArchivo))
        //                            File.Delete(result + archivoDetalle.NombreArchivo);

        //                        File.Move(result + "ReciboTemp.pdf", result + archivoDetalle.NombreArchivo);

        //                        archivoDetalle.ArchivoCabecera.IdArchivoCabecera = pObjteo.IdArchivoCabecera;
        //                        archivoDetalle.IdArchivoDetalle = BaseDatos.ObtenerBaseDatos().Agregar(archivoDetalle, bd, tran, "HabArchivosDetallesInsertar");
        //                        if (archivoDetalle.IdArchivoDetalle == 0)
        //                        {
        //                            AyudaProgramacionLN.MapearError(archivoDetalle, pObjteo);
        //                            resultado = false;
        //                            break;
        //                        }
        //                    }

        //                    if (!resultado)
        //                        break;
        //                }
        //            }
        //            if (contador != cantidadTotal)
        //            {
        //                resultado = false;
        //            }

        //            if (resultado)
        //            {
        //                this.ActualizarProgreso(100, extraData + "<BR/> Importacion de Datos Finalizadas.<BR/>");
        //                tran.Commit();
        //                pObjteo.CodigoMensaje = "ResultadoTransaccion";
        //            }
        //            else
        //            {
        //                tran.Rollback();
        //                this.ActualizarProgreso(100, extraData + "<BR/> EL PROCESO NO PUDO EJECUTARSE.<BR/>");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            tran.Rollback();
        //            ExceptionHandler.HandleException(ex, "LogicaNegocio");
        //            pObjteo.CodigoMensaje = ex.Message;
        //            return false;
        //        }
        //        finally
        //        {
        //        }
        //    }

        //    return resultado;
        //}

        private void ActualizarProgreso(int porcentaje, string datos)
        {
            //if (this.progreso != null)
            //{
            //    this.progreso.UpdateProgress(porcentaje, datos);
            //}
        }

        private bool DividirPDF(string ruta)
        {
            bool resultado = true;

            //variables
            int nombreArchivo = 0;

            //string ruta = Server.MapPath("~\\"); // this.Page.Request.ApplicationPath.EndsWith("/") ? this.Page.Request.ApplicationPath : string.Concat(this.Page.Request.ApplicationPath, "/");
            String source_file = ruta + "PdfEntrada\\";
            String result = ruta + "PdfSalida\\";

            string[] archivosLeer = Directory.GetFiles(source_file);

            PdfCopy copy;
            PdfReader reader;
            Document document;

            foreach (string archivo in archivosLeer)
            {
                //create PdfReader object
                reader = new PdfReader(archivo);

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    //create Document object
                    document = new Document();
                    copy = new PdfCopy(document, new FileStream(result + (nombreArchivo + i) + ".pdf", FileMode.Create));
                    //open the document 
                    document.Open();
                    //add page to PdfCopy 
                    copy.AddPage(copy.GetImportedPage(reader, i));
                    //close the document object
                    document.Close();
                }
                nombreArchivo += reader.NumberOfPages;
            }

            return resultado;
        }

        private bool ImportarPdfBase(string ruta)
        {
            string[] archivosLeer = Directory.GetFiles(ruta);
            string text, nombre, mes, anio, filaNombre, filaAnioMes, anioMes, dni, numero;
            int posNombre = 8, posAnioMes = 12;
            List<HabArchivosDetalles> lista = new List<HabArchivosDetalles>();
            HabArchivosDetalles archivo;
            AfiAfiliados afiliado;

            foreach (string rutaArchivo in archivosLeer)
            {
                archivo = new HabArchivosDetalles();
                text = ExtractTextFromPdf(rutaArchivo);
                //posNombre = text.IndexOf("CUIT/CUIL:");
                //nombre = text.Substring(posNombre + 10, 15);
                //posAnioMes = text.IndexOf("CONCEPTO");
                //mes = text.Substring(posAnioMes - 7, 3);
                //anio = text.Substring(posAnioMes - 3, 2);
                
                filaNombre = text.Split('\n')[posNombre];
                dni = filaNombre.Split(' ')[filaNombre.Split(' ').Length - 2];
                if (dni.Length > 3)
                    dni = dni.Substring(dni.Length - 3);
                numero = filaNombre.Split(' ')[filaNombre.Split(' ').Length - 1];
                nombre = string.Concat(dni, "-", numero);
                filaAnioMes = text.Split('\n')[posAnioMes];
                anioMes = filaAnioMes.Split(' ')[filaAnioMes.Split(' ').Length - 1];
                anio = anioMes.Substring(4, 2);
                mes = anioMes.Substring(0, 3);

                archivo.Archivo = File.ReadAllBytes(rutaArchivo);
                archivo.TipoArchivo = "pdf";
                archivo.NombreArchivo = string.Concat( nombre,".pdf");
                afiliado =new AfiAfiliados();
                afiliado.TipoDocumento.TipoDocumento = dni;
                afiliado.NumeroDocumento = Convert.ToInt32(dni);
                afiliado = AfiliadosF.AfiliadosObtenerPorTipoDocumento(afiliado);
                archivo.IdAfiliado = afiliado.IdAfiliado;
                lista.Add(archivo);
                File.Move(rutaArchivo, string.Concat(ruta, "\\", nombre, "-", anio, mes, ".pdf"));

            }

            //if (!this.Agregar(lista))
            //    return false;

            return true;
        }

        private static string NombreArchivoPDF(string text, int anio, int mes)
        {
            string nombre, filaNombre, filaAnioMes, anioMes, dni, numero;
            int posNombre = 8, posAnioMes = 12;
            int posInicial=0, posFinal=0, posTmpInicial=0;

            //filaNombre = text.Split('\n')[posNombre];
            
            //Modificacion
            if (text.Contains(" DNI"))
                posInicial = text.IndexOf(" DNI");
            if (text.Contains(" LC"))
            {
                posTmpInicial = text.IndexOf(" LC");
                if (posInicial==0 || posTmpInicial < posInicial) posInicial = posTmpInicial;
            }
            if (text.Contains(" LE"))
            {
                posTmpInicial = text.IndexOf(" LE");
                if (posInicial == 0 || posTmpInicial < posInicial) posInicial = posTmpInicial;
            }
            if (text.Contains(" CI"))
            {
                posTmpInicial = text.IndexOf(" CI");
                if (posInicial == 0 || posTmpInicial < posInicial) posInicial = posTmpInicial;
            }
            if (text.Contains(" PAS"))
            {
                posTmpInicial = text.IndexOf(" PAS");
                if (posInicial == 0 || posTmpInicial < posInicial) posInicial = posTmpInicial;
            }

            //Arreglo para DNI pegado al nombre            
            if (text.Contains("ADNI"))
            {
                posTmpInicial = text.IndexOf("ADNI");
                if (posInicial == 0 || posTmpInicial < posInicial) posInicial = posTmpInicial;
            }
            //Fin arreglo para DNI pegado al nombre

            if (posInicial == 0)
                return string.Empty;
            
            posFinal = text.IndexOf("\n", posInicial);
            filaNombre = text.Substring(posInicial, posFinal - posInicial);

            dni = filaNombre.Split(' ')[filaNombre.Split(' ').Length - 2];
         
            if (dni.Length > 3)
            {
                if (dni.EndsWith("DNIF") || dni.EndsWith("DNIM"))
                {
                    dni = dni.Substring(dni.Length - 4);
                }
                else
                    dni = dni.Substring(dni.Length - 3);
            }
            numero = filaNombre.Split(' ')[filaNombre.Split(' ').Length - 1];
            //filaAnioMes = text.Split('\n')[posAnioMes];
            //anioMes = filaAnioMes.Split(' ')[filaAnioMes.Split(' ').Length - 1];
            //anio = anioMes.Substring(4, 2);
            //mes = anioMes.Substring(0, 3);


            nombre = string.Concat(dni, "-", numero, "-", anio.ToString(), mes.ToString().PadLeft(2,'0'), ".pdf");
            return nombre;
        }

        private static string ExtractTextFromPdf(byte[] pArchivo)
        {
            using (PdfReader reader = new PdfReader(pArchivo))
            {
                StringBuilder archivo = new StringBuilder();

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    archivo.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }
                return archivo.ToString();
            }
        }

        private static string ExtractTextFromPdf(string path)
        {
            using (PdfReader reader = new PdfReader(path))
            {
                StringBuilder text = new StringBuilder();

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }

                return text.ToString();
            }
        }

        private bool Agregar(HabArchivosCabeceras pArchivos)
        {
            bool resultado = true;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pArchivos.IdArchivoCabecera = BaseDatos.ObtenerBaseDatos().Agregar(pArchivos, bd, tran, "HabArchivosCabecerasInsertar");
                    if (pArchivos.IdArchivoCabecera == 0)
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pArchivos, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        foreach (HabArchivosDetalles archi in pArchivos.ArchivosDetalles)
                        {
                            archi.ArchivoCabecera.IdArchivoCabecera = pArchivos.IdArchivoCabecera;
                            archi.IdArchivoDetalle = BaseDatos.ObtenerBaseDatos().Agregar(archi, bd, tran, "HabArchivosDetallesInsertar");
                            if (archi.IdArchivoDetalle == 0)
                            {
                                AyudaProgramacionLN.MapearError(archi, pArchivos);
                                resultado = false;
                                break;
                            }

                        }
                    }

                    if (resultado)
                    {
                        tran.Commit();
                        pArchivos.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                        tran.Rollback();

                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    //pResultado.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    //pResultado.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }

            return resultado;
        }

        public bool ImportarPdfIaf(HabArchivosCabeceras pObjteo)
        {
            this.mensajesCallback = new List<string>();
            AyudaProgramacionLN.LimpiarMensajesError(pObjteo);
            Int64 cuil=0;
            Int32 tipoDoc = 0;
            Int64 nroDoc = 0;
            string sNumeroDoc;
            AfiAfiliados afiliado;
            HabArchivosDetalles archivoDetalle;

            //this.progreso = pProgreso;
            String source_file = pObjteo.AppPath + "PdfEntrada\\";
            String result = pObjteo.AppPath + "PdfSalida\\";

            bool resultado = true;
            extraData = string.Empty;
            string msgProgreso = "Importando datos IAF. Archivo: {0}. Cantidad {1} de {2}.<BR /><BR />";
            string msgArchivosNoImportado = string.Empty;
            string separador = string.Empty;

            porcentajeProgreso = 1;
            int cantidadTotal = 0;
            int contador = 0;
            int cantidadImportada = 0;

            pObjteo.Estado.IdEstado = (int)Estados.Activo;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    //Guardo los archivos ingresados
                    pObjteo.IdArchivoCabecera = BaseDatos.ObtenerBaseDatos().Agregar(pObjteo, bd, tran, "HabArchivosCabecerasInsertar");
                    if (pObjteo.IdArchivoCabecera == 0)
                        resultado = false;

                    //if (resultado && !TGEGeneralesF.ArchivosActualizar(pObjteo, bd, tran))
                    //    resultado = false;

                    if (resultado)
                    {
                        foreach (TGEArchivos archivo in pObjteo.Archivos)
                        {
                            //create PdfReader object
                            //reader = new PdfReader(archivo.Archivo);
                            var archive = ArchiveFactory.Open(new MemoryStream(archivo.Archivo));
                            
                            cantidadTotal = archive.Entries.Count();
                            foreach (var entry in archive.Entries)
                            {
                                if (!entry.IsDirectory)
                                {
                                    //for (int i = 1; i <= reader.NumberOfPages; i++)
                                    //{
                                    //Empieza Importación de datos
                                    contador++;
                                    extraData = string.Format(msgProgreso, archivo.NombreArchivo, cantidadImportada, cantidadTotal, msgArchivosNoImportado);
                                    porcentajeProgreso = (contador * 100 / cantidadTotal);
                                    mensajesCallback = new List<string>();
                                    mensajesCallback.Add(string.Concat(porcentajeProgreso, "|", extraData));
                                    ImportarArchivosEjecutarSPMensajesCallback(mensajesCallback);
                                    //this.ActualizarProgreso(porcentajeProgreso, extraData);

                                    archivoDetalle = new HabArchivosDetalles();
                                    using (var pdfImagen = new MemoryStream())
                                    {
                                        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(AyudaProgramacionLN.StreamToByteArray(entry.OpenEntryStream()));
                                        using (Document document = new Document(PageSize.A4, 0, 0, 0, 0))
                                        {
                                            using (PdfWriter writer = PdfWriter.GetInstance(document, pdfImagen))
                                            {
                                                document.Open();
                                                //image.SetAbsolutePosition(0, 0);
                                                PdfPTable table = new PdfPTable(1);
                                                table.WidthPercentage=100;
                                                //PdfPCell cell1 = new PdfPCell();
                                                //cell1.DisableBorderSide(PdfPCell.NO_BORDER);
                                                //table.AddCell(cell1);
                                                PdfPCell cell2 = new PdfPCell(image, true);
                                                cell2.DisableBorderSide(PdfPCell.NO_BORDER);
                                                table.AddCell(cell2);
                                                document.Add(table);
                                                //writer.DirectContent.Add(table);
                                                //writer.DirectContent.AddImage(image);
                                                document.Close();
                                            }
                                        }
                                        archivoDetalle.Archivo = pdfImagen.ToArray();
                                    }                                   
                                    
                                    //archivoDetalle.Archivo = AyudaProgramacionLN.StreamToByteArray(entry.OpenEntryStream()); //File.ReadAllBytes(result + "ReciboTemp.pdf");
                                    archivoDetalle.NombreArchivo = entry.FilePath; //NombreArchivoPDF(ExtractTextFromPdf(archivoDetalle.Archivo), pObjteo.Anio, pObjteo.Mes);
                                    archivoDetalle.TipoArchivo = "pdf";
                                    archivoDetalle.Tamanio = archivoDetalle.Archivo.Length;
                                    afiliado = new AfiAfiliados();
                                    //BUSCO POR CUILT
                                    if (Int64.TryParse(archivoDetalle.NombreArchivo.Split('_')[2], out cuil)
                                        && cuil > 0)
                                    {
                                        afiliado.CUIL = cuil;
                                        afiliado = AfiliadosF.AfiliadosObtenerPorCUIL(afiliado);
                                    }
                                    //SI NO ENCUENTRO BUSCO POR TIPO Y NRO DOCUMENTO IAF
                                    sNumeroDoc = archivoDetalle.NombreArchivo.Split('_')[4];
                                    if (afiliado.IdAfiliado == 0 
                                        && Int32.TryParse(archivoDetalle.NombreArchivo.Split('_')[3], out tipoDoc)
                                        && Int64.TryParse(sNumeroDoc.Substring(0, sNumeroDoc.Length - 4), out nroDoc)
                                        && nroDoc > 0)
                                    {
                                        afiliado.TipoDocumento.Codigo = tipoDoc;
                                        afiliado.NumeroDocumento = nroDoc;
                                        afiliado = AfiliadosF.AfiliadosObtenerPorTipoDocumento(afiliado);
                                    }
                                    if (afiliado.IdAfiliado == 0)
                                    {
                                        msgArchivosNoImportado = string.Concat(separador, msgArchivosNoImportado, archivoDetalle.NombreArchivo);
                                        separador = ", ";
                                        cantidadImportada--;
                                        continue;
                                    }
                                    archivoDetalle.IdAfiliado = afiliado.IdAfiliado;
                                    archivoDetalle.Estado.IdEstado = (int)Estados.Activo;
                                    archivoDetalle.UsuarioLogueado = archivo.UsuarioLogueado;
                                    archivoDetalle.ArchivoCabecera.IdArchivoCabecera = pObjteo.IdArchivoCabecera;
                                    archivoDetalle.IdArchivoDetalle = BaseDatos.ObtenerBaseDatos().Agregar(archivoDetalle, bd, tran, "HabArchivosDetallesInsertar");
                                    if (archivoDetalle.IdArchivoDetalle == 0)
                                    {
                                        AyudaProgramacionLN.MapearError(archivoDetalle, pObjteo);
                                        resultado = false;
                                        break;
                                    }
                                    cantidadImportada++;
                                }
                                if (!resultado)
                                    break;
                            }
                            if (!resultado)
                                break;
                        }
                    }
                    if (contador != cantidadTotal)
                    {
                        resultado = false;
                    }

                    mensajesCallback = new List<string>();
                    if (resultado)
                    {
                        if (cantidadImportada < 0) cantidadImportada = 0;
                        extraData = string.Format(msgProgreso, "", cantidadImportada, cantidadTotal);
                        if(msgArchivosNoImportado.Length>0)
                            extraData = string.Format(extraData, "<BR/><span style=\"color:red\">Archivos NO importados: ", msgArchivosNoImportado, "</span>");

                        if (cantidadImportada == cantidadTotal)
                        {
                            mensajesCallback.Add(string.Concat(100, "|", "Importacion de Datos Finalizadas."));
                            ImportarArchivosEjecutarSPMensajesCallback(mensajesCallback);
                        }
                        else
                        {
                            mensajesCallback.Add(string.Concat(100, "|", "Importacion de Datos Finalizadas. <b>No todos los archivos se pudieron importar.</b>."));
                            ImportarArchivosEjecutarSPMensajesCallback(mensajesCallback);
                        }
                        
                        tran.Commit();
                        pObjteo.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        tran.Rollback();
                        mensajesCallback.Add(string.Concat(100, "|", "EL PROCESO NO PUDO EJECUTARSE"));
                        ImportarArchivosEjecutarSPMensajesCallback(mensajesCallback);
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    pObjteo.CodigoMensaje = ex.Message;
                    return false;
                }
                finally
                {
                }
            }

            return resultado;
        }
    }
}
