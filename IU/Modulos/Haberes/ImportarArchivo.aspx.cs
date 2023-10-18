using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Haberes.Entidades;
using Haberes;
using Generales.FachadaNegocio;
using Generales.Entidades;
using System.Collections.Generic;
using System.Threading;
using System.Web.Caching;

namespace IU.Modulos.Cardex
{
    public partial class ImportarArchivo : PaginaSegura
    {
        private HabArchivosCabeceras MiArchivoCabecera
        {
            get { return (HabArchivosCabeceras)Session[this.MiSessionPagina + "ImportarArchivoMiArchivoCabecera"]; }
            set { Session[this.MiSessionPagina + "ImportarArchivoMiArchivoCabecera"] = value; }
        }

        private string SessionId
        {
            get { return (string)Session["ImportarArchivoSessionId"]; }
            set { Session["ImportarArchivoSessionId"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                //string funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarImportarArchivo"));
                //this.btnAceptar.Attributes.Add("OnClick", funcion);
                this.CargarCombos();
                this.MiArchivoCabecera = new HabArchivosCabeceras();
                this.ctrArchivos.IniciarControl(this.MiArchivoCabecera, global::Comunes.Entidades.Gestion.Agregar);
            }
        }

        protected void pbProgreso_RunTask(object sender, EO.Web.ProgressTaskEventArgs e)
        {
            //this.btnAceptar.Enabled = false;
            //this.lblMensaje.Text = string.Empty;
            this.MapearControlesAObjeto(this.MiArchivoCabecera);
            this.MiArchivoCabecera.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            if (!HaberesF.HaberesImportarPdfIaf(this.MiArchivoCabecera))
            {
                //this.lblMensaje.Text = obj.CodigoMensaje;
                e.UpdateProgress(0, "<BR/><span style='color: red;'>El proceso no pudo completarse.</span><BR/ ><BR/ >" + this.MiArchivoCabecera.CodigoMensaje);
            }
            else
            {
                this.MiArchivoCabecera = new HabArchivosCabeceras();
                this.ctrArchivos.IniciarControl(this.MiArchivoCabecera, global::Comunes.Entidades.Gestion.Agregar);
                this.ctrArchivos.ActualizarControl();
            }
            //this.btnAceptar.Enabled = true;
        }

        private void CargarCombos()
        {
            this.ddlAnios.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.Anios);
            this.ddlAnios.DataValueField = "CodigoValor";
            this.ddlAnios.DataTextField = "Descripcion";
            this.ddlAnios.DataBind();
            ListItem item = this.ddlAnios.Items.FindByValue(DateTime.Now.Year.ToString());
            if (item != null)
                this.ddlAnios.SelectedValue = DateTime.Now.Year.ToString();

            this.ddlMeses.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.Meses);
            this.ddlMeses.DataValueField = "CodigoValor";
            this.ddlMeses.DataTextField = "Descripcion";
            this.ddlMeses.DataBind();

            item = this.ddlMeses.Items.FindByValue(DateTime.Now.Month.ToString());
            if (item != null)
                this.ddlMeses.SelectedValue = DateTime.Now.Month.ToString();

            this.ddlTipo.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.RemesasTipos);
            this.ddlTipo.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTipo.DataTextField = "Descripcion";
            this.ddlTipo.DataBind();
        }

        private void MapearControlesAObjeto(HabArchivosCabeceras pArchivoCabecera)
        {
            pArchivoCabecera.Anio = Convert.ToInt32(this.ddlAnios.SelectedValue);
            pArchivoCabecera.Mes = Convert.ToInt32(this.ddlMeses.SelectedValue);
            pArchivoCabecera.RemesaTipo.IdRemesaTipo = Convert.ToInt32(this.ddlTipo.SelectedValue);
            pArchivoCabecera.Archivos = ctrArchivos.ObtenerLista();
            pArchivoCabecera.AppPath = this.Request.PhysicalApplicationPath;

        }

        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            //procesando = true;
            //this.MisMensajes = new List<string>();
            this.btnProcesar.Visible = false;
            this.MapearControlesAObjeto(this.MiArchivoCabecera);

            if (this.MiArchivoCabecera.Archivos.Count == 0)
            {
                this.btnProcesar.Visible = true;
                this.MostrarMensaje("ValidarSubirArchivo", true);
                this.MiArchivoCabecera.CodigoMensaje = "ValidarSubirArchivo";
                HttpRuntime.Cache.Insert(Session.SessionID + "CacheProcesoProcesando", "#ERROR", null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
                HttpRuntime.Cache.Insert(Session.SessionID + "objProcesoProcesamiento", this.MiArchivoCabecera, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
                //ScriptManager.RegisterStartupMiArchivoCabeceraScript(this, this.GetType(), "fnErrorValidacionesScript", " fnErrorValidaciones();", true);
                return;
            }

            this.MiArchivoCabecera.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "wsEjecutarProcesoScript", " fnEjecutarProceso();", true);

            HttpRuntime.Cache.Insert(Session.SessionID + "CacheProcesoProcesando", "#PROCESANDO", null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            Thread workerThread = new Thread(new ParameterizedThreadStart(EjecutarProceso));
            workerThread.CurrentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            workerThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            this.MiArchivoCabecera.Filtro = Session.SessionID;
            workerThread.Start(this.MiArchivoCabecera);
        }

        private void EjecutarProceso(object pProcesoProcesamiento)
        {
            bool resultado = false;
            HabArchivosCabeceras objProc = (HabArchivosCabeceras)pProcesoProcesamiento;
            this.SessionId = this.MiArchivoCabecera.Filtro;

            global::Haberes.LogicaNegocio.ImportarArchivosLN importarArchivosLN = new global::Haberes.LogicaNegocio.ImportarArchivosLN();
            importarArchivosLN.ImportarArchivosEjecutarSPMensajesCallback += ImportarArchivosLN_ImportarArchivosEjecutarSPMensajesCallback;
            resultado = importarArchivosLN.ImportarPdfIaf(objProc);
            importarArchivosLN = null;
            
            string CacheProcesoProcesando;
            if (resultado)
                CacheProcesoProcesando = "#FINALIZADO";
            else
                CacheProcesoProcesando = "#ERROR";
            HttpRuntime.Cache.Insert(this.SessionId + "CacheProcesoProcesando", CacheProcesoProcesando, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            HttpRuntime.Cache.Insert(this.SessionId + "Resultado", resultado, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            HttpRuntime.Cache.Insert(this.SessionId + "objProcesoProcesamiento", objProc, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
        }

        private void ImportarArchivosLN_ImportarArchivosEjecutarSPMensajesCallback(List<string> e)
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

            HabArchivosCabeceras sisProcProcesa = (HabArchivosCabeceras)HttpRuntime.Cache.Get(Session.SessionID + "objProcesoProcesamiento");
            HttpRuntime.Cache.Remove(Session.SessionID + "objProcesoProcesamiento");

            if (resultado)
            {
                //this.ctrMensajesPostBack.MostrarMensaje(this.ObtenerMensajeSistema(this.MiProcesoProcesamiento.CodigoMensaje));
                this.btnProcesar.Visible = false;
                this.MostrarMensaje(sisProcProcesa.CodigoMensaje, false);
            }
            else
            {
                this.btnProcesar.Visible = true;
                if (sisProcProcesa == null)
                {
                    sisProcProcesa = new HabArchivosCabeceras();
                    sisProcProcesa.CodigoMensaje = "El proceso no se pudo ejecutar. Vuelvalo a intentar.";
                }
                this.MostrarMensaje(this.ObtenerMensajeSistema(sisProcProcesa.CodigoMensaje), true, sisProcProcesa.CodigoMensajeArgs);

            }
        }

    }
}
