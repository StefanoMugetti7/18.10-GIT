using Comunes.Entidades;
using Contabilidad.Entidades;
using CuentasPagar.Entidades;
using CuentasPagar.FachadaNegocio;
using Elecciones.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Proveedores.Entidades;
using Reportes.FachadaNegocio;
using System;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Compras.Controles
{
    public partial class AnticipoReservaDatos : ControlesSeguros
    {
        private CapSolicitudPago MiSolicitud
        {
            get { return (CapSolicitudPago)Session[this.MiSessionPagina + "SolicitudDatosMiSolicitud"]; }
            set { Session[this.MiSessionPagina + "SolicitudDatosMiSolicitud"] = value; }
        }
        public delegate void SolicitudPagoDatosAceptarEventHandler(object sender, CapSolicitudPago e);
        public event SolicitudPagoDatosAceptarEventHandler SolicitudPagoModificarDatosAceptar;
        public delegate void SolicitudPagoDatosCancelarEventHandler();
        public event SolicitudPagoDatosCancelarEventHandler SolicitudPagoModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (this.MiSolicitud == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }
        public void IniciarControl(CapSolicitudPago pSolicitud, Gestion pGestion)
        {
            CapProveedores prov = new CapProveedores();
            this.GestionControl = pGestion;
            this.MiSolicitud = pSolicitud;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Modificar:
                    this.MiSolicitud = CuentasPagarF.SolicitudPagoObtenerDatosCompletos(pSolicitud);
                    prov.IdProveedor = this.MiSolicitud.Entidad.IdRefEntidad;
                    this.btnImprimir.Visible = true;
                    this.MapearObjetoAControles(this.MiSolicitud);
                    this.txtFechaContable.Enabled = false;
                    this.IniciarGrilla();
                    break;
            }
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CapSolicitudPagos, "SolicitudPagoCompras", this.MiSolicitud, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            ExportPDF.ExportarPDF(pdf, this.Page, "SolicitudPagoCompras", this.UsuarioActivo);
        }
        private void CargarCombos()
        {
            this.ddlFilialPago.DataSource = TGEGeneralesF.FilialesPagosObtenerListaActiva();
            this.ddlFilialPago.DataValueField = "IdFilialPago";
            this.ddlFilialPago.DataTextField = "Filial";
            this.ddlFilialPago.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilialPago, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private decimal ImporteTotal(CapSolicitudPago pParametro)
        {
            decimal acumulador = 0;
            foreach (CapSolicitudPago item in pParametro.AnticiposReimputar)
            {
                acumulador += item.ImporteTotal;
            }
            return acumulador;
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.PersistirDatosGrilla();
            this.MapearControlesAObjeto(this.MiSolicitud);
            if (this.MiSolicitud.ImporteTotal <= 0)
            {
                this.MostrarMensaje(this.ObtenerMensajeSistema("ImporteCero"), true);
                return;
            }
            if (this.MiSolicitud.ImporteTotal != this.ImporteTotal(this.MiSolicitud))
            {
                this.MostrarMensaje(this.ObtenerMensajeSistema("ErrorDiferenciaImporte"), true);
                return;
            }
            this.MiSolicitud.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Modificar:
                    guardo = CuentasPagarF.SolicitudPagoReimputarAnticipo(this.MiSolicitud);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.btnImprimir.Visible = true;
                this.btnAceptar.Visible = false;
                string mensajeResult = this.ObtenerMensajeSistema(this.MiSolicitud.CodigoMensaje);
                this.MostrarMensaje(mensajeResult, false);
                this.ctrAsientoMostrar.IniciarControl(this.MiSolicitud);
            }
            else
            {
                this.MostrarMensaje(this.MiSolicitud.CodigoMensaje, true, this.MiSolicitud.CodigoMensajeArgs);
                if (this.MiSolicitud.dsResultado != null)
                {
                    this.MiSolicitud.dsResultado = null;
                }
            }
        }
        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            this.SolicitudPagoModificarDatosAceptar?.Invoke(null, this.MiSolicitud);
        }
        #region "Datos Solicitud"
        protected void MapearControlesAObjeto(CapSolicitudPago pSolicitud)
        {
            pSolicitud.TipoSolicitudPago.IdTipoSolicitudPago = (int)EnumTiposSolicitudPago.Compras;
            pSolicitud.IdUsuarioAlta = this.UsuarioActivo.IdUsuarioEvento;
            pSolicitud.FechaContable = this.txtFechaContable.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaContable.Text);
            pSolicitud.Observacion = this.txtObservacion.Text;
            pSolicitud.Entidad.IdEntidad = (int)EnumTGEEntidades.Proveedores;
            pSolicitud.Entidad.IdRefEntidad = this.ctrBuscarProveedor.MiProveedor.IdProveedor.Value;
            pSolicitud.ImporteTotal = decimal.Parse(this.txtTotalConIva.Text, NumberStyles.Currency);
            pSolicitud.ImporteSinIVA = decimal.Parse(this.txtTotalConIva.Text, NumberStyles.Currency);
            pSolicitud.IvaTotal = 0;
            pSolicitud.FilialPago.IdFilialPago = this.ddlFilialPago.SelectedValue == string.Empty ? default(int) : Convert.ToInt32(this.ddlFilialPago.SelectedValue);

            pSolicitud.Comentarios = ctrComentarios.ObtenerLista();
            pSolicitud.Archivos = ctrArchivos.ObtenerLista();
        }
        private void MapearObjetoAControles(CapSolicitudPago pSolicitud)
        {
            this.txtEstado.Text = this.MiSolicitud.Estado.Descripcion;
            this.txtFechaContable.Text = pSolicitud.FechaContable.HasValue ? pSolicitud.FechaContable.Value.ToShortDateString() : string.Empty;
            this.txtObservacion.Text = pSolicitud.Observacion;
            this.ddlFilialPago.SelectedValue = (pSolicitud.FilialPago.IdFilialPago).ToString();
            this.txtTotalConIva.Text = pSolicitud.ImporteTotal.ToString("C2");

            CapSolicitudPago spAsiento = new CapSolicitudPago();
            spAsiento.IdSolicitudPago = pSolicitud.IdSolicitudPago;
            this.ctrAsientoMostrar.IniciarControl(spAsiento);

            CapProveedores prov = new CapProveedores();
            this.MiSolicitud = CuentasPagarF.SolicitudPagoObtenerDatosCompletos(pSolicitud);
            prov.IdProveedor = this.MiSolicitud.Entidad.IdRefEntidad;

            this.ctrComentarios.IniciarControl(pSolicitud, this.GestionControl);
            this.ctrArchivos.IniciarControl(pSolicitud, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pSolicitud);
            this.ctrBuscarProveedor.IniciarControl(prov, this.GestionControl);
        }
        #endregion
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.SolicitudPagoModificarDatosCancelar?.Invoke();
        }
        protected void btnAgregarItem_Click(object sender, EventArgs e)
        {
            this.PersistirDatosGrilla();
            this.AgregarItem();
            this.txtCantidadAgregar.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this.upExcepciones, this.upExcepciones.GetType(), "CalcularItem", "CalcularItem();", true);
        }
        private void PersistirDatosGrilla()
        {
            if (this.MiSolicitud.AnticiposReimputar.Count == 0)
                return;

            foreach (GridViewRow fila in this.gvItems.Rows)
            {
                HiddenField hdfIdReserva = ((HiddenField)fila.FindControl("hdfIdReservaDestino"));
                HiddenField hdfReserva = ((HiddenField)fila.FindControl("hdfReservaDestino"));
                decimal importe = Convert.ToInt32(((Evol.Controls.CurrencyTextBox)fila.FindControl("txtImporteReimputar")).Decimal);

                if (!(string.IsNullOrEmpty(hdfIdReserva.Value)))
                {
                    this.MiSolicitud.AnticiposReimputar[fila.RowIndex].IdRefTabla = Convert.ToInt32(hdfIdReserva.Value);
                    this.MiSolicitud.AnticiposReimputar[fila.RowIndex].Tabla = hdfReserva.Value;
                    this.MiSolicitud.AnticiposReimputar[fila.RowIndex].Estado.IdEstado = (int)Estados.Activo;
                    this.MiSolicitud.AnticiposReimputar[fila.RowIndex].ImporteTotal = importe;
                }
            }
        }
        private void AgregarItem()
        {
            CapSolicitudPago item;
            if (this.txtCantidadAgregar.Text == string.Empty || this.txtCantidadAgregar.Text == "0")
            {
                this.txtCantidadAgregar.Text = "1";
            }
            int cantidad = Convert.ToInt32(this.txtCantidadAgregar.Text);
            for (int i = 0; i < cantidad; i++)
            {
                item = new CapSolicitudPago
                {
                    ImporteTotal = 0
                };
                this.MiSolicitud.AnticiposReimputar.Add(item);
                item.IndiceColeccion = this.MiSolicitud.AnticiposReimputar.IndexOf(item);
                item.IdSolicitudPago = item.IndiceColeccion * -1;
            }
            AyudaProgramacion.CargarGrillaListas<CapSolicitudPago>(this.MiSolicitud.AnticiposReimputar, false, this.gvItems, true);
        }
        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Anular.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            //int MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            if (e.CommandName == Gestion.Anular.ToString())
            {
                this.PersistirDatosGrilla();
                this.MiSolicitud.AnticiposReimputar.RemoveAt(index);
                this.MiSolicitud.AnticiposReimputar = AyudaProgramacion.AcomodarIndices<CapSolicitudPago>(this.MiSolicitud.AnticiposReimputar);
                AyudaProgramacion.CargarGrillaListas<CapSolicitudPago>(this.MiSolicitud.AnticiposReimputar, false, this.gvItems, true);
                ScriptManager.RegisterStartupScript(this.upExcepciones, this.upExcepciones.GetType(), "CalcularItem", "CalcularItem();", true);
            }
        }
        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CapSolicitudPago item = (CapSolicitudPago)e.Row.DataItem;
                TextBox importe = (TextBox)e.Row.FindControl("txtImporteReimputar");
                DropDownList ddlReserva = ((DropDownList)e.Row.FindControl("ddlReservaDestino"));
                importe.Attributes.Add("onchange", "CalcularItem();");
                importe.Text = item.ImporteTotal.ToString("C2");

                if(item.IdRefTabla > 0)
                {
                    ddlReserva.Items.Add(new ListItem(item.Tabla, item.IdRefTabla.ToString()));
                }
            }
        }
        private void IniciarGrilla()
        {
            CapSolicitudPago item;
            for (int i = 0; i < 2; i++)
            {
                item = new CapSolicitudPago
                {
                    EstadoColeccion = EstadoColecciones.AgregadoPrevio,
                    ImporteTotal = 0
                };
                this.MiSolicitud.AnticiposReimputar.Add(item);
                item.IndiceColeccion = this.MiSolicitud.AnticiposReimputar.IndexOf(item);
                item.IdRefTabla = item.IndiceColeccion * -1;
            }
            AyudaProgramacion.CargarGrillaListas<CapSolicitudPago>(this.MiSolicitud.AnticiposReimputar, false, this.gvItems, true);
        }
    }
}