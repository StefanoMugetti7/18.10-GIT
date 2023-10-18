using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Caching;
using System.Reflection;
using System.Diagnostics;
using IU.Backend.App_Code;
using Servicio.AccesoDatos;

namespace IU.Modulos.Seguridad
{
    public partial class SegAplicacionActualizar : PaginaSegura
    {
        private string SessionId
        {
            get { return (string)Session["SegAplicacionActualizarSessionId"]; }
            set { Session["SegAplicacionActualizarSessionId"] = value; }
        }

        private string miProcesando
        {
            get { return (string)Session["SegAplicacionActualizarmiProcesando"]; }
            set { Session["SegAplicacionActualizarmiProcesando"] = value; }
        }

        protected GRPGruposEmpresas MiEmpresa
        {
            get { return (GRPGruposEmpresas)Session[ "SegAplicacionActualizarMiProceso"]; }
            set { Session["SegAplicacionActualizarMiProceso"] = value; }
        }

        protected List<string> msgs
        {
            get { return (List<string>)Session[ "SegAplicacionActualizarmsgs"]; }
            set { Session["SegAplicacionActualizarmsgs"] = value; }
        }

        protected bool resultado
        {
            get { return (bool)Session["SegAplicacionActualizarresultado"]; }
            set { Session["SegAplicacionActualizarresultado"] = value; }
        }

        string m;

        protected List<GRPGruposEmpresas> MisGRPGruposEmpresas
        {
            get { return (List<GRPGruposEmpresas>)Session["SegAplicacionActualizarMisGRPGruposEmpresas"]; }
            set { Session["SegAplicacionActualizarMisGRPGruposEmpresas"] = value; }
        }

        const string path = "C:\\inetpub\\wwwroot\\";

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!IsPostBack)
            {                
                this.MiEmpresa = new GRPGruposEmpresas();                
                AyudaProgramacion.AgregarItemSeleccione(ddlEmpresa, "Seleccione una opción");
            }
        }

        protected void ddlTipoVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblDirAplicacionDatos.Text = string.Empty;
            lblVersionDatos.Text = string.Empty;
            lblVersionNuevaDatos.Text = string.Empty;
            lblDirectorioDatos.Text = string.Empty;

            ddlEmpresa.Items.Clear();
            ddlEmpresa.ClearSelection();
            ddlEmpresa.SelectedIndex = -1;

            if (string.IsNullOrEmpty(this.ddlTipoVersion.SelectedValue))
            {
                AyudaProgramacion.AgregarItemSeleccione(ddlEmpresa, "Seleccione una opción");
                MostrarMensaje("Seleccione un Tipo de Versión a actualizar", true);
                return;
            }
            GRPGruposEmpresas filtro = new GRPGruposEmpresas();
            filtro.Version = ddlTipoVersion.SelectedValue;
            MisGRPGruposEmpresas = new BackendLN().EmpresasSeleccionar(filtro);
            
            ddlEmpresa.DataSource = MisGRPGruposEmpresas;
            ddlEmpresa.DataValueField = "BaseDatos";
            ddlEmpresa.DataTextField = "Empresa";
            ddlEmpresa.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(ddlEmpresa, "Seleccione una opción");

        }

        protected void ddlEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblDirAplicacionDatos.Text = string.Empty;
            lblVersionDatos.Text = string.Empty;
            lblVersionNuevaDatos.Text = string.Empty;
            lblDirectorioDatos.Text = string.Empty;
            gvDatos.DataSource = null;
            gvDatos.DataBind();

            if (string.IsNullOrEmpty(this.ddlTipoVersion.SelectedValue))
            {
                MostrarMensaje("Seleccione una Empresa para actualizar", true);
                return;
            }

            if (!string.IsNullOrEmpty(this.ddlEmpresa.SelectedValue))
            {
                string tipoVersion = ddlTipoVersion.SelectedValue;
                MiEmpresa = MisGRPGruposEmpresas.FirstOrDefault(x => x.BaseDatos == ddlEmpresa.SelectedValue);
                MiEmpresa.SourcePath = string.Concat(path, tipoVersion);
                MiEmpresa.TargetPath = string.Concat(path, MiEmpresa.NombreAplicacion);
                lblDirAplicacionDatos.Text = MiEmpresa.TargetPath;
                FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(string.Concat(MiEmpresa.TargetPath, "\\bin\\IU.dll"));
                lblVersionDatos.Text = myFileVersionInfo.FileVersion;
                myFileVersionInfo = FileVersionInfo.GetVersionInfo(string.Concat(MiEmpresa.SourcePath, "\\bin\\IU.dll"));
                lblVersionNuevaDatos.Text = myFileVersionInfo.FileVersion;
                lblDirectorioDatos.Text = MiEmpresa.SourcePath;

                gvDatos.DataSource = BaseDatos.ObtenerBaseDatos().ObtenerDataSet("BackEndBaseDatosSeleccionarEstado", MiEmpresa);
                gvDatos.DataBind();
            }
            else
                MostrarMensaje("Seleccione una Empresa para actualizar", true);
        }

        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            this.resultado = true;
            //procesando = true;
            //this.MisMensajes = new List<string>();
            btnContinuar.Visible = false;
            this.MiEmpresa.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

            if (string.IsNullOrWhiteSpace(this.ddlEmpresa.SelectedValue))
            {
                MostrarMensaje("Debe seleccionar una Aplicacion para actualizar", true);
                return;
            }

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "wsEjecutarProcesoScript", " fnEjecutarProceso();", true);

            HttpRuntime.Cache.Insert(Session.SessionID + "CacheProcesoProcesando", "#PROCESANDO", null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            //this.miProcesando = "#PROCESANDO";
            Thread workerThread = new Thread(new ParameterizedThreadStart(EjecutarActualizacion));
            workerThread.CurrentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            workerThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            
            this.MiEmpresa.Filtro = Session.SessionID;
            workerThread.Start(this.MiEmpresa);
            
        }

        private void EjecutarActualizacion(object pProcesoProcesamiento)
        {
            //bool resultado = true;
            GRPGruposEmpresas objProc = (GRPGruposEmpresas)pProcesoProcesamiento;
            this.SessionId = this.MiEmpresa.Filtro;
                        
            msgs = new List<string>();
            m = "5|Actualizando la Base de Datos";
            msgs.Add(m);

            HttpRuntime.Cache.Insert(this.SessionId + "CacheMensajes", msgs, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            resultado = new BackendLN().ActualizarBaseDatos(objProc);
            if (resultado)
                m = "50|La Base de Datos se actualizo de forma correcta";
            else
                m = "50|La Base de Datos NO se pudo actualizar";
            msgs.Add(m);
            HttpRuntime.Cache.Insert(this.SessionId + "CacheMensajes", msgs, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);

            if (resultado)
                CopyFilesRecursively(objProc.SourcePath, objProc.TargetPath);
                        
            string CacheProcesoProcesando;
            if (resultado)
            {
                CacheProcesoProcesando = "#FINALIZADO";
                m = "100|La Aplicación se actualizo de forma correcta";
            }
            else
            {
                CacheProcesoProcesando = "#ERROR";
                m = "100|La Aplicación NO se pudo actualizar";
            }
            msgs.Add(m);
            HttpRuntime.Cache.Insert(this.SessionId + "CacheMensajes", msgs, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);


            HttpRuntime.Cache.Insert(this.SessionId + "CacheProcesoProcesando", CacheProcesoProcesando, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            HttpRuntime.Cache.Insert(this.SessionId + "Resultado", resultado, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            HttpRuntime.Cache.Insert(this.SessionId + "objProcesoProcesamiento", objProc, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
        }

        protected void btnFinalizarProceso_Click(object sender, EventArgs e)
        {
            HttpRuntime.Cache.Remove(Session.SessionID + "CacheMensajes");
            HttpRuntime.Cache.Remove(Session.SessionID + "CacheProcesoProcesando");
            object proc = HttpRuntime.Cache.Get(Session.SessionID + "Resultado");
            bool res = false;
            if (proc != null)
                Boolean.TryParse(proc.ToString(), out res);
            HttpRuntime.Cache.Remove(Session.SessionID + "Resultado");

            Objeto sisProcProcesa = (Objeto)HttpRuntime.Cache.Get(Session.SessionID + "objProcesoProcesamiento");
            HttpRuntime.Cache.Remove(Session.SessionID + "objProcesoProcesamiento");

            if (res)
            {
                btnVolver.Visible = true;
                btnProcesar.Visible = false;
                ddlTipoVersion.Enabled = false;
                ddlEmpresa.Enabled = false;
                this.btnContinuar.Visible = false;
                this.MiEmpresa.CodigoMensaje = "La aplicación se actualizo de forma correcta";
                this.MostrarMensaje(MiEmpresa.CodigoMensaje, false);
                UpdatePanel1.Update();
            }
            else
            {
                this.btnContinuar.Visible = true;
                if (MiEmpresa == null)
                {
                    MiEmpresa = new GRPGruposEmpresas();
                }
                MiEmpresa.CodigoMensaje = "El proceso no se pudo ejecutar. Vuelvalo a intentar.";
                this.MostrarMensaje(MiEmpresa.CodigoMensaje, true, MiEmpresa.CodigoMensajeArgs);

            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/SegAplicacionActualizar.aspx"));

            //ddlTipoVersion.Enabled = true;
            //ddlEmpresa.Enabled = true;
            //btnVolver.Visible = false;
            //btnProcesar.Visible = true;
            //UpdatePanel1.Update();
        }

        private void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            List<string> mensajes = new List<string>();
            msgs.Add("55|Copiando Directorios");
            mensajes.AddRange(msgs);            
            HttpRuntime.Cache.Insert(this.SessionId + "CacheMensajes", mensajes, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);

            string[] directorios = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories);
            int cantidad = directorios.Length;
            int porcentaje = 0;
            int contador = 0;
            string msgArchivo;
            foreach (string dirPath in directorios)
            {
                contador++;
                porcentaje = 65;// + (contador / cantidad * 100) * 100 / 20;
                mensajes = new List<string>();
                mensajes.AddRange(msgs);
                msgArchivo = dirPath.Split('\\').Last();
                mensajes.Add(string.Concat(porcentaje.ToString(), "|Actualizando Directorio: ", msgArchivo));
                HttpRuntime.Cache.Insert(this.SessionId + "CacheMensajes", mensajes, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
                //System.Threading.Thread.Sleep(1000);
            }

            msgs.Add("75|Copiando Archivos");
            HttpRuntime.Cache.Insert(this.SessionId + "CacheMensajes", mensajes, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);

            var allowedExtensions = new[] { ".txt", ".aspx", ".ascx", ".asmx", ".ashx", ".master", ".css", ".skin", ".dll", ".js", ".xml", ".svg", ".htm", ".ico", ".png", ".jpg", ".gif" };
            //Copy all the files & Replaces any files with the same name
            var filteredFiles = Directory
            .EnumerateFiles(sourcePath, "*.*", SearchOption.AllDirectories) //<--- .NET 4.5
            .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
            .ToList();
            
            cantidad = filteredFiles.Count;
            porcentaje = 0;
            contador = 0;
            foreach (string newPath in filteredFiles)
            {
                contador++;
                porcentaje = 85;// + (contador / cantidad * 100) * 100 / 20;
                mensajes = new List<string>();
                mensajes.AddRange(msgs);
                msgArchivo = newPath.Split('\\').Last();
                mensajes.Add(string.Concat(porcentaje.ToString(), "|Actualizando Archivo: ", msgArchivo));
                HttpRuntime.Cache.Insert(this.SessionId + "CacheMensajes", mensajes, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
                if (!File.Exists(newPath.Replace(sourcePath, targetPath)))
                    File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
                else if (!CompareFileBytes(newPath, newPath.Replace(sourcePath, targetPath)))
                    File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }

        private static bool CompareFileSizes(string fileName1, string fileName2)
        {
            bool fileSizeEqual = true;

            // Create System.IO.FileInfo objects for both files
            FileInfo fileInfo1 = new FileInfo(fileName1);
            FileInfo fileInfo2 = new FileInfo(fileName2);

            // Compare file sizes
            if (fileInfo1.Length != fileInfo2.Length)
            {
                // File sizes are not equal therefore files are not identical
                fileSizeEqual = false;
            }

            return fileSizeEqual;
        }

        private static bool CompareFileBytes(string fileName1, string fileName2)
        {
            // Compare file sizes before continuing. 
            // If sizes are equal then compare bytes.
            if (CompareFileSizes(fileName1, fileName2))
            {
                int file1byte = 0;
                int file2byte = 0;

                // Open a System.IO.FileStream for each file.
                // Note: With the 'using' keyword the streams 
                // are closed automatically.
                using (FileStream fileStream1 = new FileStream(fileName1, FileMode.Open),
                                  fileStream2 = new FileStream(fileName2, FileMode.Open))
                {
                    // Read and compare a byte from each file until a
                    // non-matching set of bytes is found or the end of
                    // file is reached.
                    do
                    {
                        file1byte = fileStream1.ReadByte();
                        file2byte = fileStream2.ReadByte();
                    }
                    while ((file1byte == file2byte) && (file1byte != -1));
                }

                return ((file1byte - file2byte) == 0);
            }
            else
            {
                return false;
            }
        }

       
    }
}