using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;

namespace IU.Modulos.TGE.Control
{
    public partial class CondicionesFiscalesTiposFacturasDatos : ControlesSeguros
    {
        private TGECondicionesFiscalesTiposFacturas MiCondicionFiscalTipoFactura
        {
            get { return (TGECondicionesFiscalesTiposFacturas)Session[MiSessionPagina + "TGECondicionesFiscalesTiposFacturasMiCondicionFiscalTipoFactura"]; }
            set { Session[MiSessionPagina + "TGECondicionesFiscalesTiposFacturasMiCondicionFiscalTipoFactura"] = value; }
        }
        public delegate void CondicionesFiscalesTiposFacturasAceptarEventHandler(object sender, TGECondicionesFiscalesTiposFacturas e);
        public event CondicionesFiscalesTiposFacturasAceptarEventHandler CondicionesFiscalesTiposFacturasAceptar;
        public delegate void CondicionesFiscalesTiposFacturasCancelarEventHandler();
        public event CondicionesFiscalesTiposFacturasCancelarEventHandler CondicionesFiscalesTiposFacturasCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
            }
        }
        public void IniciarControl(TGECondicionesFiscalesTiposFacturas pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiCondicionFiscalTipoFactura = pParametro;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    break;
                case Gestion.Anular:
                    this.MiCondicionFiscalTipoFactura = TGEGeneralesF.CondicionesFiscalesTiposFacturasDatosEliminarObtenerDatosCompletos(this.MiCondicionFiscalTipoFactura);
                    this.MapearObjetoAControles(this.MiCondicionFiscalTipoFactura);
                    this.ddlTipoFactura.Enabled = false;
                    this.ddlCondicionFiscal.Enabled = false;
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            this.ddlCondicionFiscal.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.CondicionesFiscales);
            this.ddlCondicionFiscal.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlCondicionFiscal.DataTextField = "Descripcion";
            this.ddlCondicionFiscal.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCondicionFiscal, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTipoFactura.DataSource = TGEGeneralesF.TiposFacturasObtenerLista();
            this.ddlTipoFactura.DataValueField = "IdTipoFactura";
            this.ddlTipoFactura.DataTextField = "TipoFactura";
            this.ddlTipoFactura.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFactura, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void MapearObjetoAControles(TGECondicionesFiscalesTiposFacturas pParametro)
        {
            this.ddlCondicionFiscal.SelectedValue = pParametro.IdCondicionFiscal.ToString();
            this.ddlTipoFactura.SelectedValue = pParametro.IdTipoFactura.ToString();
        }
        private void MapearControlesAObjeto(TGECondicionesFiscalesTiposFacturas pParametro)
        {
            pParametro.IdCondicionFiscal = this.ddlCondicionFiscal.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlCondicionFiscal.SelectedValue);
            pParametro.IdTipoFactura = this.ddlTipoFactura.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoFactura.SelectedValue);
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            //if (!Page.IsValid)
            //    return;
            this.MapearControlesAObjeto(this.MiCondicionFiscalTipoFactura);
            this.MiCondicionFiscalTipoFactura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = TGEGeneralesF.CondicionesFiscalesTiposFacturasDatosAgregar(this.MiCondicionFiscalTipoFactura);
                    break;
                case Gestion.Anular:
                    this.MiCondicionFiscalTipoFactura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    guardo = TGEGeneralesF.CondicionesFiscalesTiposFacturasDatosEliminar(this.MiCondicionFiscalTipoFactura);
                    break;
            }
            if (guardo)
            {
                this.btnAceptar.Visible = false;
                this.MostrarMensaje(this.MiCondicionFiscalTipoFactura.CodigoMensaje, false);
            }
            else
            {
                if (!this.MiCondicionFiscalTipoFactura.ConfirmarAccion) { }
                this.MostrarMensaje(this.MiCondicionFiscalTipoFactura.CodigoMensaje, false, this.MiCondicionFiscalTipoFactura.CodigoMensajeArgs);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.CondicionesFiscalesTiposFacturasCancelar?.Invoke();
        }
    }
}