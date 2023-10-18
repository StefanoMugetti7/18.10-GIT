using Comunes.Entidades;
using Contabilidad;
using Contabilidad.Entidades;
using CuentasPagar.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace IU.Modulos.Contabilidad.Controles
{
    public partial class BienesUsosDatos : ControlesSeguros
    {
        private CtbBienesUsos MiBienUso
        {
            get { return (CtbBienesUsos)Session[this.MiSessionPagina + "MiBienUso"]; }
            set { Session[this.MiSessionPagina + "MiBienUso"] = value; }
        }
        public delegate void BienUsoDatosAceptarEventHandler(object sender, CtbBienesUsos e);
        public event BienUsoDatosAceptarEventHandler BienUsoDatosAceptar;
        public delegate void BienUsoDatosCancelarEventHandler();
        public event BienUsoDatosCancelarEventHandler BienUsoDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(this.popUpMensajes_popUpMensajesPostBackAceptar);
            this.puSolicitudPagoDetalle.SolicitudPagoDetalleBuscarSeleccionarPopUp += new SolicitudPagoDetalleBuscarPopUp.SolicitudPagoDetalleBuscarPopUpEventHandler(this.puSolicitudPagoDetalle_SolicitudPagoDetalleBuscarSeleccionarPopUp);
            this.ctrCuentasContables.CuentasContablesBuscarSeleccionar += new Contabilidad.Controles.CuentasContablesBuscar.CuentasContablesBuscarEventHandler(this.ctrCuentasContables_CuentasContablesBuscarSeleccionar);
            this.ctrCuentasContables.CuentasContablesBuscarIniciar += new CuentasContablesBuscar.CuentasContablesBuscarIniciarEventHandler(this.ctrCuentasContables_CuentasContablesBuscarIniciar);
            if (!this.IsPostBack)
            {
                if (this.MiBienUso == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }
        void ctrCuentasContables_CuentasContablesBuscarIniciar(CtbEjerciciosContables ejercicio)
        {
            AyudaProgramacion.MatchObjectProperties(ContabilidadF.EjerciciosContablesObtenerUltimoActivo(), ejercicio);
        }
        void ctrCuentasContables_CuentasContablesBuscarSeleccionar(global::Contabilidad.Entidades.CtbCuentasContables e, int indiceColeccion)
        {
            this.MiBienUso.CuentaContable = e;
            this.upCuentasContables.Update();
        }
        void puSolicitudPagoDetalle_SolicitudPagoDetalleBuscarSeleccionarPopUp(CapSolicitudPagoDetalles e)
        {
            this.MiBienUso.SolicitudPagoDetalles = e;
            this.MiBienUso.Descripcion = e.Producto.Descripcion;
            this.MiBienUso.Importe = e.PrecioUnitarioSinIva + e.PrecioNoGravado;
            this.MiBienUso.Cantidad = Convert.ToInt32(e.Cantidad);
            this.MiBienUso.Estado.IdEstado = (int)EstadosBienesUsos.Activado;
            this.MapearObjetoAControles(this.MiBienUso);
            this.upBinesUso.Update();
        }
        protected void btnBuscarSolicitudPagoDetalle_Click(object sender, EventArgs e)
        {
            this.puSolicitudPagoDetalle.IniciarControl(false);
        }
        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Solicitud Material
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(CtbBienesUsos pBienUso, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiBienUso = pBienUso;
                    this.btnBuscarSolicitudPagoDetalle.Visible = true;
                    this.txtFechaActivacion.Text = DateTime.Now.ToShortDateString();
                    this.MiBienUso.FechaActivacion = DateTime.Now;
                    this.ddlEstado.SelectedValue = ((int)EstadosBienesUsos.Activado).ToString();
                    this.MiBienUso.Estado.IdEstado = (int)EstadosBienesUsos.Activado;
                    this.txtVidaTranscurrida.Text = "0";
                    this.txtAmortAcumulada.Text = "0";
                    this.ctrCamposValores.IniciarControl(pBienUso, new Objeto(), this.GestionControl);
                    break;
                case Gestion.Modificar:
                    this.MiBienUso = ContabilidadF.BienesUsosObtenerDatosCompletos(pBienUso);
                    this.HabilitarControles(true);
                    this.MapearObjetoAControles(this.MiBienUso);
                    break;
                case Gestion.Consultar:
                    this.MiBienUso = ContabilidadF.BienesUsosObtenerDatosCompletos(pBienUso);
                    this.MapearObjetoAControles(this.MiBienUso);
                    this.HabilitarControles(false);
                    this.txtDescripcion.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            //int index = Convert.ToInt32(e.CommandArgument);
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotalAmortizacion = (Label)e.Row.FindControl("lblTotalAmortizacion");
                lblTotalAmortizacion.Text = this.MiBienUso.BienesUsosDetalles.Sum(x => x.ImporteAmortizado.HasValue ? x.ImporteAmortizado.Value : 0).ToString("C2");
                //int cellCount = e.Row.Cells.Count;
                //e.Row.Cells.Clear();
                //TableCell tableCell = new TableCell();
                //tableCell.ColumnSpan = cellCount;
                //tableCell.HorizontalAlign = HorizontalAlign.Right;
                //tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiBienUso.BienesUsosDetalles.Count);
                //e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CtbBienesUsosDetalles parametros = this.BusquedaParametrosObtenerValor<CtbBienesUsosDetalles>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CtbBienesUsosDetalles>(parametros);
            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MiBienUso.BienesUsosDetalles;
            this.gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MiBienUso.BienesUsosDetalles = this.OrdenarGrillaDatos<CtbBienesUsosDetalles>(this.MiBienUso.BienesUsosDetalles, e);
            this.gvDatos.DataSource = this.MiBienUso.BienesUsosDetalles;
            this.gvDatos.DataBind();
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiBienUso);
            this.MiBienUso.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = ContabilidadF.BienesUsosAgregar(this.MiBienUso);
                    break;
                case Gestion.Modificar:
                    guardo = ContabilidadF.BienesUsosModificar(this.MiBienUso);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiBienUso.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiBienUso.CodigoMensaje, true, this.MiBienUso.CodigoMensajeArgs);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.BienUsoDatosCancelar != null)
                this.BienUsoDatosCancelar();
        }
        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.BienUsoDatosAceptar != null)
                this.BienUsoDatosAceptar(null, this.MiBienUso);
        }
        private void HabilitarControles(bool habilitar)
        {
            this.txtCantidad.Enabled = habilitar;
            this.txtImporte.Enabled = habilitar;
            this.ddlClasificador.Enabled = habilitar;
            this.txtVidaUtil.Enabled = habilitar;
            this.txtFechaActivacion.Enabled = habilitar;
        }
        private void MapearObjetoAControles(CtbBienesUsos pBienUso)
        {
            this.txtDescripcion.Text = pBienUso.Descripcion;
            this.txtCantidad.Text = pBienUso.Cantidad.ToString();
            this.ddlClasificador.SelectedValue = pBienUso.Clasificador.IdClasificador == 0 ? string.Empty : pBienUso.Clasificador.IdClasificador.ToString();
            this.txtImporte.Text = pBienUso.Importe.Value.ToString("C2");
            this.txtVidaUtil.Text = pBienUso.VidaUtil.ToString();
            this.txtFechaActivacion.Text = pBienUso.FechaActivacion.ToShortDateString();
            this.txtVidaTranscurrida.Text = pBienUso.VidaTranscurrida.ToString().Trim();
            this.txtVidaRestante.Text = pBienUso.VidaRestante.ToString().Trim();
            this.txtAmortAcumulada.Text = pBienUso.AmortAcumulada.ToString().Trim();
            this.ddlEstado.SelectedValue = pBienUso.Estado.IdEstado.ToString();
            this.ctrCuentasContables.MapearObjetoControles(pBienUso.CuentaContable, this.GestionControl, 0);
            ListItem item = this.ddlFilial.Items.FindByValue(pBienUso.Filial.IdFilial.ToString());
            if (item != null)
                this.ddlFilial.SelectedValue = pBienUso.Filial.IdFilial.ToString();
            AyudaProgramacion.CargarGrillaListas<CtbBienesUsosDetalles>(pBienUso.BienesUsosDetalles, false, this.gvDatos, true);
            this.ctrCamposValores.IniciarControl(pBienUso, new Objeto(), this.GestionControl);
        }
        private void MapearControlesAObjeto(CtbBienesUsos pBienUso)
        {
            pBienUso.Descripcion = this.txtDescripcion.Text;
            pBienUso.Cantidad = Convert.ToInt32(this.txtCantidad.Text);
            pBienUso.Clasificador.IdClasificador = Convert.ToInt32(this.ddlClasificador.SelectedValue);
            pBienUso.Clasificador.Descripcion = this.ddlClasificador.SelectedItem.Text;
            pBienUso.Importe = this.txtImporte.Decimal;
            pBienUso.VidaUtil = Convert.ToInt32(this.txtVidaUtil.Text);
            pBienUso.FechaActivacion = Convert.ToDateTime(this.txtFechaActivacion.Text);
            pBienUso.VidaTranscurrida = Convert.ToInt32(this.txtVidaTranscurrida.Text);
            pBienUso.VidaRestante = Convert.ToInt32(this.txtVidaRestante.Text);
            pBienUso.AmortAcumulada = this.txtAmortAcumulada.Decimal;
            pBienUso.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pBienUso.Estado.Descripcion = this.ddlEstado.SelectedItem.Text;
            pBienUso.Filial.IdFilial = this.ddlFilial.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFilial.SelectedValue);
            pBienUso.Filial.Filial = this.ddlFilial.SelectedValue == string.Empty ? string.Empty : this.ddlFilial.SelectedItem.Text;
            pBienUso.Campos = this.ctrCamposValores.ObtenerLista();
        }
        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosBienesUsos));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();

            this.ddlClasificador.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.CtbClasificadores);
            this.ddlClasificador.DataValueField = "IdListaValorDetalle";
            this.ddlClasificador.DataTextField = "Descripcion";
            this.ddlClasificador.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlClasificador, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlFilial.DataSource = TGEGeneralesF.FilialesPagosObtenerListaActiva();
            this.ddlFilial.DataValueField = "IdFilial";
            this.ddlFilial.DataTextField = "Filial";
            this.ddlFilial.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
    }
}