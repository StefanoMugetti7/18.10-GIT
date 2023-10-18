using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Generales.Entidades;
using System.Data;
using ProcesosDatos;
using Auditoria;
using Auditoria.Entidades;
using Comunes.Entidades;
using System.Threading;
using System.Web.Caching;

namespace IU.Modulos.ProcesosDatos
{
    public partial class BackupsListar : PaginaSegura
    {
        private string SessionId
        {
            get { return (string)Session["BackupsListarSessionId"]; }
            set { Session["BackupsListarSessionId"] = value; }
        }

        protected Objeto MiProcesoProcesamiento
        {
            get { return (Objeto)Session[this.MiSessionPagina + "BackupsListarSessionIdMiProcesoProcesamiento"]; }
            set { Session[this.MiSessionPagina + "BackupsListarSessionIdMiProcesoProcesamiento"] = value; }
        }

        private DataTable MisBackups
        {
            get { return (DataTable)Session[this.MiSessionPagina + "BackupsListarMisBackups"]; }
            set { Session[this.MiSessionPagina + "BackupsListarMisBackups"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.MiProcesoProcesamiento = new Objeto();
                this.CargarLista();
                this.DescargarArchivosRegisterPostBack();
            }
            else
            {
                this.DescargarArchivosRegisterPostBack();
            }
        }

        private void CargarLista()
        {
            this.MisBackups = ProcesosDatosF.BackupsObtenerGrilla();
            this.gvArchivos.DataSource = this.MisBackups;
            this.gvArchivos.DataBind();
            this.DescargarArchivosRegisterPostBack();
        }

        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            this.btnProcesar.Visible = false;

           this.MiProcesoProcesamiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

            HttpRuntime.Cache.Insert(Session.SessionID + "CacheProcesoProcesando", "#PROCESANDO", null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            Thread workerThread = new Thread(new ParameterizedThreadStart(EjecutarProceso));
            workerThread.CurrentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            workerThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            this.MiProcesoProcesamiento.Filtro = Session.SessionID;
            workerThread.Start(this.MiProcesoProcesamiento);
        }

        private void EjecutarProceso(object pProcesoProcesamiento)
        {
            bool resultado = false;
            Objeto objProc = (Objeto)pProcesoProcesamiento;
            this.SessionId = this.MiProcesoProcesamiento.Filtro;
            objProc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

            global::ProcesosDatos.LogicaNegocio.BackupsLN backupsLN = new global::ProcesosDatos.LogicaNegocio.BackupsLN();
            backupsLN.ProcesoDatosEjecutarSPMensajesCallback += BackupsLN_ProcesoDatosEjecutarSPMensajesCallback;
            resultado = backupsLN.Backup(objProc);
            
            string CacheProcesoProcesando;
            if (resultado)
                CacheProcesoProcesando = "#FINALIZADO";
            else
                CacheProcesoProcesando = "#ERROR";
            HttpRuntime.Cache.Insert(this.SessionId + "CacheProcesoProcesando", CacheProcesoProcesando, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            HttpRuntime.Cache.Insert(this.SessionId + "Resultado", resultado, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            HttpRuntime.Cache.Insert(this.SessionId + "objProcesoProcesamiento", objProc, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
        }

        private void BackupsLN_ProcesoDatosEjecutarSPMensajesCallback(List<string> e)
        {
            HttpRuntime.Cache.Insert(Session.SessionID + "CacheMensajes", e, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
        }

        protected void btnFinalizarProceso_Click(object sender, EventArgs e)
        {
            HttpRuntime.Cache.Remove(Session.SessionID + "CacheMensajes");
            HttpRuntime.Cache.Remove(Session.SessionID + "CacheProcesoProcesando");
            object proc = HttpRuntime.Cache.Get(Session.SessionID + "Resultado");
            bool resultado = false;
            if (proc != null)
                Boolean.TryParse(proc.ToString(), out resultado);
            HttpRuntime.Cache.Remove(Session.SessionID + "Resultado");

            Objeto sisProcProcesa = (Objeto)HttpRuntime.Cache.Get(Session.SessionID + "objProcesoProcesamiento");
            HttpRuntime.Cache.Remove(Session.SessionID + "objProcesoProcesamiento");

            if (resultado)
            {
                //this.ctrMensajesPostBack.MostrarMensaje(this.ObtenerMensajeSistema(this.MiProcesoProcesamiento.CodigoMensaje));
                this.btnProcesar.Visible = false;
                this.MostrarMensaje(sisProcProcesa.CodigoMensaje, false, sisProcProcesa.CodigoMensajeArgs);
                this.CargarLista();
                this.upArchivos.Update();
            }
            else
            {
                this.btnProcesar.Visible = true;
                if (sisProcProcesa == null)
                {
                    sisProcProcesa = new Objeto();
                    sisProcProcesa.CodigoMensaje = "El proceso no se pudo ejecutar. Vuelvalo a intentar.";
                }
                this.MostrarMensaje(this.ObtenerMensajeSistema(sisProcProcesa.CodigoMensaje), true, sisProcProcesa.CodigoMensajeArgs);
            }
        }

        protected void gvArchivos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!( e.CommandName == "Descargar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int Id = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            if (e.CommandName == "Descargar")
            {
                //HistorialCambios cambio = new HistorialCambios();
                //cambio.CampoCambiado = "Descarga de Backups";

                //AuditoriaF.AuditoriaAgregar(
                
                TGEArchivos archivo = new TGEArchivos();
                DataRow row = this.MisBackups.AsEnumerable().First(x => x.Field<int>("Id") == Id);
                archivo.RutaFisica = row["RutaFisica"].ToString();
                string nombre = row["NombreArchivo"].ToString();
                archivo.NombreArchivo = nombre.Substring(0, nombre.Length-4);
                archivo.TipoArchivo = nombre.Substring(nombre.Length - 3, 3);

                this.Response.Clear();
                this.Response.Buffer = true;
                this.Response.ContentType = "application/octet-stream";
                this.Response.AddHeader("content-disposition", String.Format("attachment; filename=\"{0}\"", string.Concat(archivo.NombreArchivo, ".", archivo.TipoArchivo)));
                //this.Response.TransmitFile(archivo.RutaFisica);
                this.Response.WriteFile(archivo.RutaFisica);
                this.Response.End();
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        private void DescargarArchivosRegisterPostBack()
        {
            foreach (GridViewRow fila in this.gvArchivos.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    ImageButton ibtnDescargar = (ImageButton)fila.FindControl("btnDescargar");
                    this.toolkitScriptManager.RegisterPostBackControl(ibtnDescargar);
                }
            }
        }
    }
}