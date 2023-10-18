using Comunes.Entidades;
using Contabilidad;
using Contabilidad.Entidades;
using Generales.FachadaNegocio;
using System;

namespace IU.Modulos.Contabilidad.Controles
{
    public partial class CapitulosDatos : ControlesSeguros
    {
        private CtbCapitulos MiCapitulo
        {
            get { return (CtbCapitulos)Session[this.MiSessionPagina + "MiCapitulo"]; }
            set { Session[this.MiSessionPagina + "MiCapitulo"] = value; }
        }
        public delegate void CapituloDatosAceptarEventHandler(object sender, CtbCapitulos e);
        public event CapituloDatosAceptarEventHandler CapituloDatosAceptar;
        public delegate void CapituloDatosCancelarEventHandler();
        public event CapituloDatosCancelarEventHandler CapituloDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(this.popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
                if (this.MiCapitulo == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }
        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Solicitud Material
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(CtbCapitulos pCapitulo, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiCapitulo = pCapitulo;
                    this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();
                    break;
                case Gestion.Modificar:
                    this.MiCapitulo = ContabilidadF.CapitulosObtenerDatosCompletos(pCapitulo);
                    this.MapearObjetoAControles(this.MiCapitulo);
                    break;
                case Gestion.Consultar:
                    this.MiCapitulo = ContabilidadF.CapitulosObtenerDatosCompletos(pCapitulo);
                    this.MapearObjetoAControles(this.MiCapitulo);
                    this.txtCapitulo.Enabled = false;
                    this.txtCodigoCapitulo.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }
        private void MapearObjetoAControles(CtbCapitulos pCapitulo)
        {
            this.txtCapitulo.Text = pCapitulo.Capitulo;
            this.txtCodigoCapitulo.Text = pCapitulo.CodigoCapitulo;
            this.ddlEstado.SelectedValue = pCapitulo.Estado.IdEstado.ToString();
        }
        private void MapearControlesAObjeto(CtbCapitulos pCapitulo)
        {
            pCapitulo.Capitulo = this.txtCapitulo.Text;
            pCapitulo.CodigoCapitulo = this.txtCodigoCapitulo.Text;
            pCapitulo.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
        }
        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiCapitulo);
            this.MiCapitulo.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = ContabilidadF.CapitulosAgregar(this.MiCapitulo);
                    break;
                case Gestion.Modificar:
                    guardo = ContabilidadF.CapitulosModificar(this.MiCapitulo);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiCapitulo.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiCapitulo.CodigoMensaje, true, this.MiCapitulo.CodigoMensajeArgs);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.CapituloDatosCancelar != null)
                this.CapituloDatosCancelar();
        }
        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.CapituloDatosAceptar != null)
                this.CapituloDatosAceptar(null, this.MiCapitulo);
        }
    }
}