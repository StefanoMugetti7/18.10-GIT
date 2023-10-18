using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;

namespace IU.Modulos.TGE.Control
{
    public partial class TiposFuncionalidadesTiposValores : ControlesSeguros
    {
        private TGETiposFuncionalidadesTiposValores MiTipoFuncionalidadTipoValor
        {
            get { return (TGETiposFuncionalidadesTiposValores)Session[MiSessionPagina + "TiposOperacionesModulosFuncionalidadesMiTipoOperacionFuncionalidad"]; }
            set { Session[MiSessionPagina + "TiposOperacionesModulosFuncionalidadesMiTipoOperacionFuncionalidad"] = value; }
        }
        public delegate void TiposFuncionalidadesTiposValoresAceptarEventHandler(object sender, TGETiposFuncionalidadesTiposValores e);
        public event TiposFuncionalidadesTiposValoresAceptarEventHandler TiposFuncionalidadesTiposValoresAceptar;
        public delegate void TiposFuncionalidadesTiposValoresCancelarEventHandler();
        public event TiposFuncionalidadesTiposValoresCancelarEventHandler TiposFuncionalidadesTiposValoresCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }
        public void IniciarControl(TGETiposFuncionalidadesTiposValores pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiTipoFuncionalidadTipoValor = pParametro;
            this.CargarCombos();
            switch (GestionControl)
            {
                case Gestion.Agregar:
                    break;
                case Gestion.Anular:
                    this.MiTipoFuncionalidadTipoValor = TGEGeneralesF.TiposFuncionalidadesTiposValoresObtenerDatosCompletos(this.MiTipoFuncionalidadTipoValor);
                    this.MapearObjetoAControles(this.MiTipoFuncionalidadTipoValor);
                    this.ddlTipoFuncionalidad.Enabled = false;
                    this.ddlTipoValor.Enabled = false;
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            this.ddlTipoFuncionalidad.DataSource = TGEGeneralesF.TGETiposFuncionalidadesObtenerLista();
            this.ddlTipoFuncionalidad.DataValueField = "IdTipoFuncionalidad";
            this.ddlTipoFuncionalidad.DataTextField = "TipoFuncionalidad";
            this.ddlTipoFuncionalidad.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFuncionalidad, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTipoValor.DataSource = TGEGeneralesF.TiposValoresObtenerLista();
            this.ddlTipoValor.DataValueField = "IdTipoValor";
            this.ddlTipoValor.DataTextField = "TipoValor";
            this.ddlTipoValor.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoValor, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void MapearObjetoAControles(TGETiposFuncionalidadesTiposValores pParametro)
        {
            this.ddlTipoValor.SelectedValue = pParametro.IdTipoValor.ToString();
            this.ddlTipoFuncionalidad.SelectedValue = pParametro.IdTipoFuncionalidad.ToString();
        }
        private void MapearControlesAObjeto(TGETiposFuncionalidadesTiposValores pParametro)
        {
            pParametro.IdTipoValor = this.ddlTipoValor.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoValor.SelectedValue);
            pParametro.IdTipoFuncionalidad = this.ddlTipoFuncionalidad.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoFuncionalidad.SelectedValue);
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.MapearControlesAObjeto(this.MiTipoFuncionalidadTipoValor);
            this.MiTipoFuncionalidadTipoValor.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (GestionControl)
            {
                case Gestion.Agregar:

                    guardo = TGEGeneralesF.TiposFuncionalidadesTiposValoresAgregar(this.MiTipoFuncionalidadTipoValor);
                    break;
                case Gestion.Anular:
                    this.MiTipoFuncionalidadTipoValor.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    guardo = TGEGeneralesF.TiposFuncionalidadesTiposValoresEliminar(this.MiTipoFuncionalidadTipoValor);
                    break;
            }
            if (guardo)
            {
                this.btnAceptar.Visible = false;
                this.MostrarMensaje(MiTipoFuncionalidadTipoValor.CodigoMensaje, true);
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposFuncionalidadesTiposValoresListar.aspx"), true);
            }
            else
            {
                if (this.MiTipoFuncionalidadTipoValor.ConfirmarAccion) { }
                // popUpMensajes.MostrarMensaje(ObtenerMensajeSistema(MiPlazoFijoPropio.CodigoMensaje, MiPlazoFijoPropio.CodigoMensajeArgs), true);
                else
                    this.MostrarMensaje(this.MiTipoFuncionalidadTipoValor.CodigoMensaje, false, this.MiTipoFuncionalidadTipoValor.CodigoMensajeArgs);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.TiposFuncionalidadesTiposValoresCancelar?.Invoke();
        }
    }
}