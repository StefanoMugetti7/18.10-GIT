using Cobros;
using Cobros.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Prestamos;
using Prestamos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Cobros.Controles
{
    public partial class CancelacionAnticipadaPrestamosCuotas : ControlesSeguros
    {
        private CobOrdenesCobros MiOrdenCobro
        {
            get { return (CobOrdenesCobros)Session[this.MiSessionPagina + "CancelacionAnticipadaPrestamosCuotasMiOrdenCobro"]; }
            set { Session[this.MiSessionPagina + "CancelacionAnticipadaPrestamosCuotasMiOrdenCobro"] = value; }
        }
        public delegate void OrdenesCobrosDatosCancelarEventHandler();
        public event OrdenesCobrosDatosCancelarEventHandler OrdenesCobrosModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }
        public void IniciarControl(CobOrdenesCobros pOrdenCobro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiOrdenCobro = pOrdenCobro;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiOrdenCobro.FechaEmision = DateTime.Now;
                    this.txtFecha.Text = DateTime.Now.ToShortDateString();
                    this.txtTipoOperacion.Text = this.MiOrdenCobro.TipoOperacion.TipoOperacion;
                    List<PrePrestamosCuotas> prestamosCuotas = PrePrestamosF.PrestamosCuotasObtenerCancelacionAnticipada(this.MiOrdenCobro.Afiliado);
                    CobOrdenesCobrosDetalles detalle;
                    foreach (PrePrestamosCuotas cuota in prestamosCuotas)
                    {
                        detalle = new CobOrdenesCobrosDetalles();
                        detalle.EstadoColeccion = EstadoColecciones.Agregado;
                        detalle.IdPrestamoCuota = cuota.IdPrestamoCuota;
                        detalle.Detalle = cuota.Detalle; // string.Concat("Nro Prestamo ", cuota.IdPrestamoCuota.ToString().PadLeft(10, ' '), " - Nro Cuota ", cuota.CuotaNumero.ToString().PadLeft(3, ' '), " Fecha Vto. ", cuota.CuotaFechaVencimiento.ToShortDateString());
                        detalle.Importe = cuota.ImporteAmortizacion;
                        this.MiOrdenCobro.OrdenesCobrosDetalles.Add(detalle);
                        detalle.IndiceColeccion = this.MiOrdenCobro.OrdenesCobrosDetalles.IndexOf(detalle);
                    }
                    AyudaProgramacion.CargarGrillaListas<CobOrdenesCobrosDetalles>(this.MiOrdenCobro.OrdenesCobrosDetalles, false, this.gvCuentaCorriente, true);
                    this.ddlFilialCobro.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
                    this.ctrOrdenesCobrosValores.IniciarControl(this.MiOrdenCobro, this.GestionControl);
                    break;
                case Gestion.Anular:
                    this.MiOrdenCobro = CobrosF.OrdenesCobrosObtenerDatosCompletos(this.MiOrdenCobro);
                    this.MapearObjetoAControles();
                    this.txtDetalle.Enabled = false;
                    this.ddlFilialCobro.Enabled = false;
                    this.btnAceptar.Visible = true;
                    break;
                case Gestion.Consultar:
                    this.MiOrdenCobro = CobrosF.OrdenesCobrosObtenerDatosCompletos(this.MiOrdenCobro);
                    this.MapearObjetoAControles();
                    this.txtDetalle.Enabled = false;
                    this.ddlFilialCobro.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }
        private void MapearObjetoAControles()
        {
            this.txtTipoOperacion.Text = this.MiOrdenCobro.TipoOperacion.TipoOperacion;
            this.txtFecha.Text = this.MiOrdenCobro.FechaEmision.ToShortDateString();
            this.txtOrdenCobro.Text = this.MiOrdenCobro.IdOrdenCobro.ToString();
            this.txtDetalle.Text = this.MiOrdenCobro.Detalle;
            this.ddlFilialCobro.SelectedValue = this.MiOrdenCobro.FilialCobro.IdFilialCobro.ToString();
            this.ctrOrdenesCobrosValores.IniciarControl(this.MiOrdenCobro, this.GestionControl);
            //this.ctrFechaCajaContable.IniciarControl(Gestion.Consultar, this.MiOrdenCobro.FechaConfirmacion);
            AyudaProgramacion.CargarGrillaListas<CobOrdenesCobrosDetalles>(this.MiOrdenCobro.OrdenesCobrosDetalles, false, this.gvCuentaCorriente, true);
        }
        private void MapearControlesAObjeto()
        {
            this.MiOrdenCobro.Detalle = this.txtDetalle.Text;
            this.MiOrdenCobro.FilialCobro.IdFilialCobro = Convert.ToInt32(this.ddlFilialCobro.SelectedValue);
            this.MiOrdenCobro.FilialCobro.Filial = this.ddlFilialCobro.SelectedItem.Text;
            //this.MiOrdenCobro.FechaConfirmacion = this.ctrFechaCajaContable.dFechaCajaContable;
        }
        private void CargarCombos()
        {
            this.ddlFilialCobro.DataSource = TGEGeneralesF.FilialesObenerLista();
            this.ddlFilialCobro.DataValueField = "IdFilial";
            this.ddlFilialCobro.DataTextField = "Filial";
            this.ddlFilialCobro.DataBind();
        }
        protected void gvCuentaCorriente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (this.GestionControl == Gestion.Agregar)
                {
                    CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("checkAll");
                    ibtnConsultar.Visible = true;
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                switch (this.GestionControl)
                {
                    case Gestion.Agregar:
                        CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluir");
                        ibtnConsultar.Visible = true;

                        TextBox aCobrar = (TextBox)e.Row.FindControl("txtACobrar");
                        aCobrar.Visible = true;
                        break;
                    case Gestion.Anular:
                    case Gestion.Consultar:
                        Label lblImporte = (Label)e.Row.FindControl("lblImporte");
                        lblImporte.Visible = true;
                        break;
                    default:
                        break;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                if ((this.GestionControl == Gestion.Consultar || this.GestionControl == Gestion.Anular) && (this.MiOrdenCobro.Estado.IdEstado == (int)EstadosOrdenesCobro.Activo))
                {
                    Label lblImporteTotal = (Label)e.Row.FindControl("lblTotalACobrar");
                    lblImporteTotal.Text = this.MiOrdenCobro.OrdenesCobrosDetalles.Sum(x => x.Importe).ToString("C2");
                }
            }
        }
        protected void MapearGrillaObjeto()
        {
            CobOrdenesCobrosDetalles detalle;
            CheckBox incluir;
            Evol.Controls.CurrencyTextBox aCobrar;
            foreach (GridViewRow fila in this.gvCuentaCorriente.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    detalle = this.MiOrdenCobro.OrdenesCobrosDetalles[fila.DataItemIndex];
                    incluir = (CheckBox)fila.FindControl("chkIncluir");
                    aCobrar = (Evol.Controls.CurrencyTextBox)fila.FindControl("txtACobrar");
                    if (incluir.Checked)
                    {
                        detalle.IncluirEnOP = true;
                        detalle.Importe = aCobrar.Decimal;
                    }
                }
            }
            this.MiOrdenCobro.OrdenesCobrosDetalles = this.MiOrdenCobro.OrdenesCobrosDetalles.Where(x => x.IncluirEnOP).ToList();
            AyudaProgramacion.CargarGrillaListas(this.MiOrdenCobro.OrdenesCobrosDetalles, false, this.gvCuentaCorriente, true);
            this.upArmarCobros.Update();
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MapearGrillaObjeto();
                    this.MiOrdenCobro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    this.MiOrdenCobro.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    this.MiOrdenCobro.Moneda.IdMoneda = (int)EnumTGEMonedas.PesosArgentinos;
                    this.MiOrdenCobro.ImporteTotal = this.MiOrdenCobro.OrdenesCobrosDetalles.Sum(x => x.Importe);
                    this.MiOrdenCobro = this.ctrOrdenesCobrosValores.ObtenerOrdenesCobrosValores(this.MiOrdenCobro);
                    guardo = CobrosF.OrdenesCobrosAgregar(this.MiOrdenCobro);
                    break;
                case Gestion.Anular:
                    this.MiOrdenCobro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    guardo = CobrosF.OrdenesCobrosAnular(this.MiOrdenCobro);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.btnAceptar.Visible = false;
                this.btnImprimir.Visible = true;
                this.ctrPopUpComprobantes.CargarReporte(this.MiOrdenCobro, EnumTGEComprobantes.CobOrdenesCobros, true);
            }
            else
            {
                this.MostrarMensaje(this.MiOrdenCobro.CodigoMensaje, true, this.MiOrdenCobro.CodigoMensajeArgs);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.OrdenesCobrosModificarDatosCancelar != null)
            {
                this.MiOrdenCobro = new CobOrdenesCobros();
                this.OrdenesCobrosModificarDatosCancelar();
            }
        }
        protected void btnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            this.ctrPopUpComprobantes.CargarReporte(this.MiOrdenCobro, EnumTGEComprobantes.CobOrdenesCobros);
        }
    }
}