using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;

namespace IU.Modulos.TGE.Control
{
    public partial class TiposOperacionesDatos : ControlesSeguros
    {
        TGETiposOperaciones MisTiposOperaciones
        {
            get { return (TGETiposOperaciones)Session[this.MiSessionPagina + "TiposOperacionesDatosMisTiposOperaciones"]; }
            set { Session[this.MiSessionPagina + "TiposOperacionesDatosMisTiposOperaciones"] = value; }
        }

        public delegate void TiposOperacionesDatosAceptarEventHandler(object sender, TGETiposOperaciones e);
        public event TiposOperacionesDatosAceptarEventHandler TiposOperacionesModificarDatosAceptar;

        public delegate void TiposOperacionesDatosCancelarEventHandler();
        public event TiposOperacionesDatosCancelarEventHandler TiposOperacionesModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (this.IsPostBack)
            {
                if (this.MisTiposOperaciones == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }
        /// <summary>
        /// Inicializa el control de Alta y Modificación de una operacion
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(TGETiposOperaciones pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            this.MisTiposOperaciones = pParametro;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    break;
                case Gestion.Modificar:
                    this.MisTiposOperaciones = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MisTiposOperaciones);
                    break;
                case Gestion.Consultar:
                    this.MisTiposOperaciones = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MisTiposOperaciones);
                    this.txtTipoOperacion.Enabled = false;
                    this.ddlEstados.Enabled = false;
                    this.ddlModulosSistema.Enabled = false;
                    this.ddlTipoMovimiento.Enabled = false;
                    this.txtCodigoOperacion.Enabled = false;
                    this.chkContabiliza.Enabled = false;
                    this.chkLibroIVA.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            this.ddlTipoMovimiento.DataSource = TGEGeneralesF.TiposMovimientosListar();
            this.ddlTipoMovimiento.DataValueField = "IdTipoMovimiento";
            this.ddlTipoMovimiento.DataTextField = "TipoMovimiento";
            this.ddlTipoMovimiento.DataBind();

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();

            this.ddlModulosSistema.DataSource = TGEGeneralesF.ModulosSistemaObtenerLista();
            this.ddlModulosSistema.DataValueField = "IdModuloSistema";
            this.ddlModulosSistema.DataTextField = "ModuloSistema";
            this.ddlModulosSistema.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlModulosSistema, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MisTiposOperaciones);
            this.MisTiposOperaciones.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MisTiposOperaciones.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    guardo = TGEGeneralesF.TiposOperacionesAgregar(this.MisTiposOperaciones);
                    break;
                case Gestion.Modificar:
                    guardo = TGEGeneralesF.TiposOperacionesModificar(this.MisTiposOperaciones);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MisTiposOperaciones.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MisTiposOperaciones.CodigoMensaje, true, this.MisTiposOperaciones.CodigoMensajeArgs);
            }
        }
        private void MapearControlesAObjeto(TGETiposOperaciones pParametro)
        {
            pParametro.TipoOperacion = this.txtTipoOperacion.Text;
            pParametro.TipoMovimiento.IdTipoMovimiento = Convert.ToInt32(this.ddlTipoMovimiento.SelectedValue);
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pParametro.CodigoOperacion = this.txtCodigoOperacion.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCodigoOperacion.Text);
            pParametro.IdModuloSistema = this.ddlModulosSistema.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlModulosSistema.SelectedValue);
            pParametro.Contabiliza = this.chkContabiliza.Checked;
            pParametro.LibroIVA = this.chkLibroIVA.Checked;
        }
        private void MapearObjetoAControles(TGETiposOperaciones pParametro)
        {
            this.txtTipoOperacion.Text = pParametro.TipoOperacion;
            this.ddlTipoMovimiento.SelectedValue = pParametro.TipoMovimiento.IdTipoMovimiento.ToString();
            this.ddlEstados.SelectedValue = pParametro.Estado.IdEstado.ToString();
            this.txtCodigoOperacion.Text = pParametro.CodigoOperacion.ToString();
            this.ddlModulosSistema.SelectedValue = pParametro.IdModuloSistema == 0 ? string.Empty : pParametro.IdModuloSistema.ToString();
            this.chkContabiliza.Checked = pParametro.Contabiliza;
            this.chkLibroIVA.Checked = pParametro.LibroIVA;
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.TiposOperacionesModificarDatosCancelar != null)
                this.TiposOperacionesModificarDatosCancelar();
        }
        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.TiposOperacionesModificarDatosAceptar != null)
                this.TiposOperacionesModificarDatosAceptar(null, this.MisTiposOperaciones);
        }
    }
}