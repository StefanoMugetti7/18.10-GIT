using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Servicio.AccesoDatos;
using Comunes.Entidades;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.IO;
using SharpCompress.Writer;
using SharpCompress.Common;
using SharpCompress.Archive.Zip;
using SharpCompress.Archive;
using OfficeOpenXml;

namespace ProcesosDatos.LogicaNegocio
{
    public class BackupsLN
    {
        private class msCallback
        {
            public int Progress { get; set; }
            public string Msg { get; set; }
        }
        const string DIRECTORY_BACKUP = "tempBackups";
        public delegate void ProcesosDatosEjecutarSPMensajes(List<string> e);
        public event ProcesosDatosEjecutarSPMensajes ProcesoDatosEjecutarSPMensajesCallback;

        public List<string> mensajesBasedatos;

        public DataTable ObtenerGrilla()
        {
            Objeto objeto = new Objeto
            {
                Filtro = string.Concat(AppDomain.CurrentDomain.BaseDirectory, DIRECTORY_BACKUP)
            };
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("SisProcesosBackupListar", objeto);
        }

        public bool Backup(Objeto pObjeto)
        {
            bool resultado = false;

            mensajesBasedatos = new List<string>();

            string filename = string.Empty;
            try
            {
                string directory = string.Concat(AppDomain.CurrentDomain.BaseDirectory, DIRECTORY_BACKUP);
                if (!System.IO.Directory.Exists(directory))
                    System.IO.Directory.CreateDirectory(directory);

                string extraData = string.Empty;
                //string msgInicio = "Generando Backup de la Base de datos. Este proceso puede tardar, no salga de la pagina!";
                string separador = string.Empty;
                string msg = string.Empty;
                int cantidadTablas=0;
                int cantidadTotal = 0;
                int contador = 0;
                int porcentaje = 0;

                msg = string.Concat(porcentaje.ToString(), "|Generando Backup de la Base de datos. Este proceso puede tardar, no salga de la pagina!");
                this.mensajesBasedatos.Add(msg);
                if (this.ProcesoDatosEjecutarSPMensajesCallback != null)
                    ProcesoDatosEjecutarSPMensajesCallback(this.mensajesBasedatos);

                FileInfo fi;
                string[] archivos = Directory.GetFiles(directory);
                foreach (string file in archivos)
                {
                    fi = new FileInfo(file);
                    if (fi.Extension == ".xlsx")
                        fi.Delete();
                }

                DataTable dt = BaseDatos.ObtenerBaseDatos().ObtenerLista("SisProcesosBackupSeleccionarTablas", new Objeto());
                cantidadTablas= dt.Rows.Count;
                cantidadTotal = dt.Rows.Count + 20;
                DataTable datos;
                Objeto filtro;                
                bool msgTabla = false;
                foreach (DataRow row in dt.Rows)
                {
                    contador++;
                    porcentaje = contador * 100 / cantidadTotal;
                    filename = string.Concat(directory, "\\", row["TABLE_NAME"], ".xlsx");
                    if (System.IO.File.Exists(filename))
                        System.IO.File.Delete(filename);
                    msg = string.Concat(porcentaje.ToString(), "|Tabla: ", row["TABLE_NAME"].ToString());
                    if (!msgTabla)
                    {
                        this.mensajesBasedatos.Add(msg);
                        msgTabla = true;
                    }
                    this.mensajesBasedatos[this.mensajesBasedatos.Count - 1] = msg;
                    if (this.ProcesoDatosEjecutarSPMensajesCallback != null)
                        ProcesoDatosEjecutarSPMensajesCallback(this.mensajesBasedatos);
                    filtro = new Objeto
                    {
                        Filtro = row["TABLE_NAME"].ToString()
                    };
                    datos = BaseDatos.ObtenerBaseDatos().ObtenerLista("SisProcesosBackupSeleccionarTablasDatos", filtro);
                    using (ExcelPackage pck = new ExcelPackage(new FileInfo(filename)))
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add(row["TABLE_NAME"].ToString());
                        ws.Cells["A1"].LoadFromDataTable(datos, true);
                        pck.Save();
                    }

                }
                porcentaje = (contador + 5) * 100 / cantidadTotal;
                msg = string.Concat(porcentaje.ToString(), "|Backup de Tablas generado.");
                this.mensajesBasedatos.Add(msg);
                if (this.ProcesoDatosEjecutarSPMensajesCallback != null)
                    ProcesoDatosEjecutarSPMensajesCallback(this.mensajesBasedatos);
                //e.UpdateProgress(porcentaje, string.Concat(msgInicio, "<BR/>", "Backup de Tablas generado. Preparando el archivo ", filename));

                string ZipName = string.Concat("Backup_", DateTime.Now.ToString("yyyyMMdd"), ".zip");
                string ZipPathName = string.Concat(directory, "\\", ZipName);
                if (File.Exists(ZipPathName))
                    File.Delete(ZipPathName);

                porcentaje = (contador+5) * 100 / cantidadTotal;
                msg = string.Concat(porcentaje.ToString(), "|Comprimiendo archivo  ", ZipName);
                this.mensajesBasedatos.Add(msg);
                if (this.ProcesoDatosEjecutarSPMensajesCallback != null)
                    ProcesoDatosEjecutarSPMensajesCallback(this.mensajesBasedatos);
                //e.UpdateProgress(porcentaje, string.Concat(msgInicio, "<BR/>", "Comprimiendo archivo ", filename));

                using (var archive = ZipArchive.Create())
                {
                    string[] excels = Directory.GetFiles(directory);
                    foreach (string file in excels)
                    {
                        fi = new FileInfo(file);
                        if (fi.Extension == ".xlsx")
                            archive.AddEntry(string.Concat(fi.Name), fi);
                    }
                    Stream fs = new FileStream(ZipPathName, FileMode.OpenOrCreate, FileAccess.Write);
                    CompressionInfo comInfo = new CompressionInfo();
                    comInfo.DeflateCompressionLevel = SharpCompress.Compressor.Deflate.CompressionLevel.BestCompression;
                    comInfo.Type = CompressionType.Deflate;
                    archive.SaveTo(fs, comInfo);
                    fs.Close();
                }

                porcentaje = (contador+5) * 100 / cantidadTotal;
                msg = string.Concat(porcentaje.ToString(), "|Verificando archivos...  ", ZipName);
                this.mensajesBasedatos.Add(msg);
                if (this.ProcesoDatosEjecutarSPMensajesCallback != null)
                    ProcesoDatosEjecutarSPMensajesCallback(this.mensajesBasedatos);

                string[] archivosBorrar = Directory.GetFiles(directory);
                foreach (string file in archivosBorrar)
                {
                    fi = new FileInfo(file);
                    if (fi.Extension == ".xlsx")
                        fi.Delete();
                }

                msg = string.Concat(porcentaje.ToString(), "|Se ha generado el backup ", ZipName, " de forma correcta");
                this.mensajesBasedatos.Add(msg);
                if (this.ProcesoDatosEjecutarSPMensajesCallback != null)
                    ProcesoDatosEjecutarSPMensajesCallback(this.mensajesBasedatos);
                //e.UpdateProgress(100, string.Concat("Se ha generado el backup ", ZipName, " de forma correcta", "<BR/>", " "));
                resultado = true;
                pObjeto.CodigoMensaje = "BackupResultadoOK";
                pObjeto.CodigoMensajeArgs.Add(ZipName);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, "LogicaNegocio");
                pObjeto.CodigoMensaje = ex.Message;
                resultado = false;
            }
            finally
            {
                if (File.Exists(filename))
                    File.Delete(filename);
            }
            return resultado;
        }
    }
}
