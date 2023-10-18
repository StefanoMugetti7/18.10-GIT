using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Contabilidad;

namespace IU.Modulos.Contabilidad.Controles
{
    public partial class MonedasDatos : ControlesSeguros
    {
        private CtbMonedas MiMoneda
        {
            get { return (CtbMonedas)Session[this.MiSessionPagina + "MiMoneda"]; }
            set { Session[this.MiSessionPagina + "MiMoneda"] = value; }
        }
        public delegate void MonedaDatosAceptarEventHandler(object sender, CtbMonedas e);
        public event MonedaDatosAceptarEventHandler MonedaDatosAceptar;
        public delegate void MonedaDatosCancelarEventHandler();
        public event MonedaDatosCancelarEventHandler MonedaDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(this.popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
                if (this.MiMoneda == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }
        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Moneda
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(CtbMonedas pMoneda, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiMoneda = pMoneda;
                    this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();
                    break;
                case Gestion.Modificar:
                    this.MiMoneda = ContabilidadF.MonedasObtenerDatosCompletos(pMoneda);
                    this.MapearObjetoAControles(this.MiMoneda);
                    break;
                case Gestion.Consultar:
                    this.MiMoneda = ContabilidadF.MonedasObtenerDatosCompletos((pMoneda));
                    this.MapearObjetoAControles(this.MiMoneda);
                    this.txtMoneda.Enabled = false;
                    this.txtCodigoMoneda.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }
        private void MapearObjetoAControles(CtbMonedas pMoneda)
        {
            this.txtMoneda.Text = pMoneda.Moneda;
            this.txtCodigoMoneda.Text = pMoneda.CodigoMoneda;
            this.ddlEstado.SelectedValue = pMoneda.Estado.IdEstado.ToString();
        }
        private void MapearControlesAObjeto(CtbMonedas pMoneda)
        {
            pMoneda.Moneda = this.txtMoneda.Text;
            pMoneda.CodigoMoneda = this.txtCodigoMoneda.Text;
            pMoneda.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
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
            this.MapearControlesAObjeto(this.MiMoneda);
            this.MiMoneda.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = ContabilidadF.MonedasAgregar(this.MiMoneda);
                    break;
                case Gestion.Modificar:
                    guardo = ContabilidadF.MonedasModificar(this.MiMoneda);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiMoneda.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiMoneda.CodigoMensaje, true, this.MiMoneda.CodigoMensajeArgs);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.MonedaDatosCancelar != null)
                this.MonedaDatosCancelar();
        }
        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.MonedaDatosAceptar != null)
                this.MonedaDatosAceptar(null, this.MiMoneda);
        }
    }
}