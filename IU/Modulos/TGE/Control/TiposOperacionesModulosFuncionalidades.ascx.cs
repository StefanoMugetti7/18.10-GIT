using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;

namespace IU.Modulos.TGE.Control
{
    public partial class TiposOperacionesModulosFuncionalidades : ControlesSeguros
    {
        private TGETiposOperacionesModulosFuncionalidades MiTipoOperacionFuncionalidad
        {
            get { return (TGETiposOperacionesModulosFuncionalidades)Session[MiSessionPagina + "TiposOperacionesModulosFuncionalidadesMiTipoOperacionFuncionalidad"]; }
            set { Session[MiSessionPagina + "TiposOperacionesModulosFuncionalidadesMiTipoOperacionFuncionalidad"] = value; }
        }
        public delegate void TiposOperacionesModulosFuncionalidadesAceptarEventHandler(object sender, TGETiposOperacionesModulosFuncionalidades e);
        public event TiposOperacionesModulosFuncionalidadesAceptarEventHandler TiposOperacionesModulosFuncionalidadesAceptar;
        public delegate void TiposOperacionesModulosFuncionalidadesCancelarEventHandler();
        public event TiposOperacionesModulosFuncionalidadesCancelarEventHandler TiposOperacionesModulosFuncionalidadesCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!IsPostBack)
            {
            }
        }
        public void IniciarControl(TGETiposOperacionesModulosFuncionalidades pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiTipoOperacionFuncionalidad = pParametro;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    break;
                case Gestion.Anular:
                    this.MiTipoOperacionFuncionalidad = TGEGeneralesF.TiposOperacionesModulosFuncionalidadesEliminarObtenerDatosCompletos(this.MiTipoOperacionFuncionalidad);
                    this.MapearObjetoAControles(this.MiTipoOperacionFuncionalidad);
                    this.ddlModuloSistema.Enabled = false;
                    this.ddlTipoFuncionalidad.Enabled = false;
                    this.ddlTipoOperacion.Enabled = false;
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

            this.ddlTipoOperacion.DataSource = TGEGeneralesF.TGETiposOperacionesObtenerLista();
            this.ddlTipoOperacion.DataValueField = "IdTipoOperacion";
            this.ddlTipoOperacion.DataTextField = "TipoOperacion";
            this.ddlTipoOperacion.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoOperacion, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlModuloSistema.DataSource = TGEGeneralesF.ModulosSistemaObtenerLista();
            this.ddlModuloSistema.DataValueField = "IdModuloSistema";
            this.ddlModuloSistema.DataTextField = "ModuloSistema";
            this.ddlModuloSistema.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlModuloSistema, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void MapearObjetoAControles(TGETiposOperacionesModulosFuncionalidades pParametro)
        {
            this.ddlTipoOperacion.SelectedValue = pParametro.IdTipoOperacion.ToString();
            this.ddlTipoFuncionalidad.SelectedValue = pParametro.IdTipoFuncionalidad.ToString();
            this.ddlModuloSistema.SelectedValue = pParametro.IdModuloSistema.ToString();
        }
        private void MapearControlesAObjeto(TGETiposOperacionesModulosFuncionalidades pParametro)
        {
            pParametro.IdTipoOperacion = ddlTipoOperacion.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlTipoOperacion.SelectedValue);
            pParametro.IdTipoFuncionalidad = ddlTipoFuncionalidad.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlTipoFuncionalidad.SelectedValue);
            pParametro.IdModuloSistema = ddlModuloSistema.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlModuloSistema.SelectedValue);
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            //if (!Page.IsValid)
            //    return;

            this.MapearControlesAObjeto(this.MiTipoOperacionFuncionalidad);
            if ((this.MiTipoOperacionFuncionalidad.IdModuloSistema > 0 && this.MiTipoOperacionFuncionalidad.IdTipoFuncionalidad > 0) ||
              (this.MiTipoOperacionFuncionalidad.IdTipoFuncionalidad > 0 && this.MiTipoOperacionFuncionalidad.IdTipoOperacion > 0) ||
              (this.MiTipoOperacionFuncionalidad.IdTipoOperacion > 0 && this.MiTipoOperacionFuncionalidad.IdModuloSistema > 0))
            {
                // MiPlazoFijoPropio.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
                switch (this.GestionControl)
                {
                    case Gestion.Agregar:
                        guardo = TGEGeneralesF.TiposOperacionesModulosFuncionalidadesAgregar(this.MiTipoOperacionFuncionalidad);
                        break;
                    case Gestion.Anular:
                        this.MiTipoOperacionFuncionalidad.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                        guardo = TGEGeneralesF.TiposOperacionesModulosFuncionalidadesEliminar(this.MiTipoOperacionFuncionalidad);
                        break;
                }
                if (guardo)
                {
                    this.btnAceptar.Visible = false;
                    this.MostrarMensaje(this.MiTipoOperacionFuncionalidad.CodigoMensaje, false);
                }
                else
                {
                    if (this.MiTipoOperacionFuncionalidad.ConfirmarAccion) { }
                    // popUpMensajes.MostrarMensaje(ObtenerMensajeSistema(MiPlazoFijoPropio.CodigoMensaje, MiPlazoFijoPropio.CodigoMensajeArgs), true);
                    else
                        this.MostrarMensaje(MiTipoOperacionFuncionalidad.CodigoMensaje, false, MiTipoOperacionFuncionalidad.CodigoMensajeArgs);
                }
            }
            else
            {
                guardo = false;
                this.MostrarMensaje("Debe Completar Al Menos 2 Campos", false);
                return;
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.TiposOperacionesModulosFuncionalidadesCancelar?.Invoke();
        }
    }
}