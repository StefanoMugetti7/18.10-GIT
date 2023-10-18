using Comunes.Entidades;
using Facturas;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.TGE.Control
{
    public partial class TiposOperacionesComprobantesDatos : ControlesSeguros
    {
        TGETiposOperacionesTiposFacturas TiposOperacionesTiposFacturas
        {
            get { return (TGETiposOperacionesTiposFacturas)Session[this.MiSessionPagina + "TiposComprobantesTiposFacturas"]; }
            set { Session[this.MiSessionPagina + "TiposComprobantesTiposFacturas"] = value; }
        }

        public delegate void TGETiposOperacionesTiposFacturasDatosAceptarEventHandler(object sender, TGETiposOperacionesTiposFacturas e);
        public event TGETiposOperacionesTiposFacturasDatosAceptarEventHandler TiposOperacionesTiposFacturasModificarDatosAceptar;

        public delegate void TGETiposOperacionesTiposFacturasDatosCancelarEventHandler();
        public event TGETiposOperacionesTiposFacturasDatosCancelarEventHandler TiposOperacionesTiposFacturasModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (this.IsPostBack)
            {
                if (this.TiposOperacionesTiposFacturas == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una operacion
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(TGETiposOperacionesTiposFacturas pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            this.TiposOperacionesTiposFacturas = pParametro;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    //this.ctrComentarios .IniciarControl(this.TiposOperacionesTiposFacturas, this.GestionControl);
                    this.ddlEstado.SelectedValue = 1.ToString(); //Activo
                    break;
                case Gestion.Modificar:
                    this.TiposOperacionesTiposFacturas = TGEGeneralesF.TiposOperacionesTiposFacturasObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.TiposOperacionesTiposFacturas);
                    break;
                case Gestion.Consultar:
                    this.TiposOperacionesTiposFacturas = TGEGeneralesF.TiposOperacionesTiposFacturasObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.TiposOperacionesTiposFacturas);
                    this.ddlEstado.Enabled = false;
                    this.ddlTipoFactura.Enabled = false;
                    this.ddlTipoOperacionOC.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.ddlSigno.Enabled = false;
                    this.chkMostrarIVA.Enabled = false;
                    //this.txtAfipPuntoVenta.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }
        protected global::IU.Modulos.Comunes.popUpMensajesPostBack popUpMensajes;
        private void MapearObjetoAControles(TGETiposOperacionesTiposFacturas pParametro)
        {
            this.chkMostrarIVA.Checked = pParametro.MostrarIVA;
            this.ddlTipoFactura.SelectedValue = Convert.ToString(pParametro.IdTipoFactura);
            this.ddlTipoOperacionOC.SelectedValue = pParametro.IdTipoOperacion.ToString();
            this.ddlSigno.SelectedValue = Convert.ToString(pParametro.Signo);
            this.ddlEstado.SelectedValue = Convert.ToString(pParametro.Estado.IdEstado);
            //this.txtAfipPuntoVenta.Text = pParametro.AfipPuntoVenta.ToString();

            // this.ctrComentarios.IniciarControl(pParametro, this.GestionControl);
            //this.ctrAuditoria.IniciarControl(pParametro);
        }
        protected global::System.Web.UI.WebControls.Button btnAceptar;
        private void MapearControlesAObjeto(TGETiposOperacionesTiposFacturas pParametro)
        {
            pParametro.MostrarIVA = this.chkMostrarIVA.Checked;
            pParametro.IdTipoFactura = Convert.ToInt32(this.ddlTipoFactura.SelectedValue);
            pParametro.IdTipoOperacion = Convert.ToInt32(this.ddlTipoOperacionOC.SelectedValue);
            pParametro.Signo = Convert.ToInt32(this.ddlSigno.SelectedValue);
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            //pParametro.AfipPuntoVenta = this.txtAfipPuntoVenta.Text == string.Empty ? 0 : Convert.ToInt32(this.txtAfipPuntoVenta.Text);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.TiposOperacionesTiposFacturas);
            this.TiposOperacionesTiposFacturas.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    //this.MiFilial.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    guardo = TGEGeneralesF.TiposOperacionesTiposFacturasAgregar(this.TiposOperacionesTiposFacturas);
                    break;
                case Gestion.Modificar:
                    guardo = TGEGeneralesF.TiposOperacionesTiposFacturasModificar(this.TiposOperacionesTiposFacturas);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.TiposOperacionesTiposFacturas.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.TiposOperacionesTiposFacturas.CodigoMensaje, true, this.TiposOperacionesTiposFacturas.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.TiposOperacionesTiposFacturasModificarDatosCancelar != null)
                this.TiposOperacionesTiposFacturasModificarDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.TiposOperacionesTiposFacturasModificarDatosAceptar != null)
                this.TiposOperacionesTiposFacturasModificarDatosAceptar(null, this.TiposOperacionesTiposFacturas);
        }
        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion" ;
            this.ddlEstado.DataBind();

            this.ddlTipoFactura.DataSource = FacturasF.TiposFacturasActivosPorIdTipoFactura(new TGETiposFacturas());
            this.ddlTipoFactura.DataValueField = "IdTipoFactura";
            this.ddlTipoFactura.DataTextField = "Descripcion";
            this.ddlTipoFactura.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFactura, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            TGETiposOperaciones operaciones = new TGETiposOperaciones();
            operaciones.TipoFuncionalidad.IdTipoFuncionalidad = this.paginaSegura.paginaActual.IdTipoFuncionalidad;
            operaciones.Estado.IdEstado = (int)Estados.Activo;
            //MisTiposOperaciones = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(operaciones);
            this.ddlTipoOperacionOC.DataSource = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(operaciones);
            this.ddlTipoOperacionOC.DataTextField = "TipoOperacion";
            this.ddlTipoOperacionOC.DataValueField = "IdTipoOperacion";
            this.ddlTipoOperacionOC.DataBind();
            if (ddlTipoOperacionOC.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoOperacionOC, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
    }
}

