using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using Cargos.Entidades;
using Cargos;
using Cobros.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Contabilidad;
using Cobros;
using Generales.FachadaNegocio;

namespace IU.Modulos.Cobros.Controles
{
    public partial class OrdenesCobrosDatos : ControlesSeguros
    {
        private CobOrdenesCobros MiOrdenCobro
        {
            get { return (CobOrdenesCobros)Session[this.MiSessionPagina + "OrdenesCobrosDatosMiOrdenCobro"]; }
            set { Session[this.MiSessionPagina + "OrdenesCobrosDatosMiOrdenCobro"] = value; }
        }

        //public delegate void OrdenesCobrosDatosAceptarEventHandler(CobOrdenesCobros e);
        //public event OrdenesCobrosDatosAceptarEventHandler OrdenesCobrosModificarDatosAceptar;
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
                    this.CargarComboConceptos();
                    this.ddlFilialCobro.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
                    this.MiOrdenCobro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.OrdenesCobrosVarios;
                    break;
                case Gestion.Anular:
                    this.MiOrdenCobro = CobrosF.OrdenesCobrosObtenerDatosCompletos(this.MiOrdenCobro);
                    this.MapearObjetoAControles();
                    this.txtDetalle.Enabled = false;
                    this.ddlFilialCobro.Enabled = false;
                    this.pnlCobrosManuales.Visible = false;
                    break;
                case Gestion.Consultar:
                    this.MiOrdenCobro = CobrosF.OrdenesCobrosObtenerDatosCompletos(this.MiOrdenCobro);
                    this.MapearObjetoAControles();
                    this.txtDetalle.Enabled = false;
                    this.ddlFilialCobro.Enabled = false;
                    this.btnAceptar.Visible = false;
                    this.pnlCobrosManuales.Visible = false;
                    //this.pnlDetalleCobros.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void MapearObjetoAControles()
        {
            this.txtFecha.Text = this.MiOrdenCobro.FechaEmision.ToShortDateString();
            this.txtOrdenCobro.Text = this.MiOrdenCobro.IdOrdenCobro.ToString();
            this.txtDetalle.Text = this.MiOrdenCobro.Detalle;
            this.ddlFilialCobro.SelectedValue = this.MiOrdenCobro.FilialCobro.IdFilialCobro.ToString();
            this.txtDetalle.Text = this.MiOrdenCobro.Detalle;
            AyudaProgramacion.CargarGrillaListas<CobOrdenesCobrosDetalles>(this.MiOrdenCobro.OrdenesCobrosDetalles, false, this.gvOrdenesCobrosDetalles, true);
            this.ctrAsientoMostrar.IniciarControl(this.MiOrdenCobro);
        }

        private void MapearControlesAObjeto()
        {
            this.MiOrdenCobro.Detalle = this.txtDetalle.Text;
            this.MiOrdenCobro.FilialCobro.IdFilialCobro = Convert.ToInt32(this.ddlFilialCobro.SelectedValue);
            this.MiOrdenCobro.FilialCobro.Filial = this.ddlFilialCobro.SelectedItem.Text;
        }

        private void CargarCombos()
        {
            this.ddlFilialCobro.DataSource = TGEGeneralesF.FilialesObenerLista();
            this.ddlFilialCobro.DataValueField = "IdFilial";
            this.ddlFilialCobro.DataTextField = "Filial";
            this.ddlFilialCobro.DataBind();
        }

        private void CargarComboConceptos()
        {
            TGETiposOperaciones opeFiltro = new TGETiposOperaciones();// funcion = new TGETiposFuncionalidades();
            opeFiltro.IdTipoOperacion = (int)EnumTGETiposOperaciones.OrdenesCobrosVarios; //(int)EnumTGETiposFuncionalidades.Cobros;
            opeFiltro.Estado.IdEstado = (int)Estados.Activo;
            this.ddlConceptosContables.DataSource = ContabilidadF.ConceptosContablesObtenerListaFiltro(opeFiltro);
            this.ddlConceptosContables.DataValueField = "IdConceptoContable";
            this.ddlConceptosContables.DataTextField = "ConceptoContable";
            this.ddlConceptosContables.DataBind();
        }

        protected void gvOrdenesCobrosDetalles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Borrar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CobOrdenesCobrosDetalles detalle = this.MiOrdenCobro.OrdenesCobrosDetalles[indiceColeccion];
            this.MiOrdenCobro.OrdenesCobrosDetalles.Remove(detalle);
            
            AyudaProgramacion.CargarGrillaListas<CobOrdenesCobrosDetalles>(this.MiOrdenCobro.OrdenesCobrosDetalles, false, this.gvOrdenesCobrosDetalles, true);
            this.upArmarCobros.Update();
        }

        protected void gvOrdenesCobrosDetalles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                switch (this.GestionControl)
                {
                    case Gestion.Agregar:
                        ImageButton ibtnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                        ibtnEliminar.Visible = true;
                        break;
                    default:
                        break;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer
                && this.MiOrdenCobro.OrdenesCobrosDetalles.Count > 0)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount - 1;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalImporte"), this.MiOrdenCobro.OrdenesCobrosDetalles.Sum(x => x.Importe).ToString("C2"));
                e.Row.Cells.Add(tableCell);
                e.Row.Cells.Add(new TableCell());
            }
        }

        protected void gvOrdenesCobrosDetalles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //AhoCuentasMovimientos parametros = this.BusquedaParametrosObtenerValor<AhoCuentasMovimientos>();
            //parametros.IndiceColeccion = e.NewPageIndex;
            //this.BusquedaParametrosGuardarValor<AhoCuentasMovimientos>(parametros);

            this.gvOrdenesCobrosDetalles.PageIndex = e.NewPageIndex;
            this.gvOrdenesCobrosDetalles.DataSource = this.MiOrdenCobro.OrdenesCobrosDetalles;
            this.gvOrdenesCobrosDetalles.DataBind();
        }

        protected void btnIngresarConcepto_Click(object sender, EventArgs e)
        {
            this.Page.Validate("IngresarConcepto");
            if (!this.Page.IsValid)
                return;

            CobOrdenesCobrosDetalles detalle = new CobOrdenesCobrosDetalles();

            if (this.MiOrdenCobro.OrdenesCobrosDetalles.Exists(
                x => x.CuentaCorriente.IdCuentaCorriente == 0
                    && x.ConceptoContable.IdConceptoContable == Convert.ToInt32(this.ddlConceptosContables.SelectedValue))
                )
            {
                detalle = this.MiOrdenCobro.OrdenesCobrosDetalles.Find(
                    x => x.CuentaCorriente.IdCuentaCorriente == 0
                        && x.ConceptoContable.IdConceptoContable == Convert.ToInt32(this.ddlConceptosContables.SelectedValue));
            }
            else
            {
                detalle = new CobOrdenesCobrosDetalles();
                detalle.EstadoColeccion = EstadoColecciones.Agregado;
                detalle.ConceptoContable.ConceptoContable = this.ddlConceptosContables.SelectedItem.Text;
                detalle.ConceptoContable.IdConceptoContable = Convert.ToInt32(this.ddlConceptosContables.SelectedValue);
                detalle.Detalle = detalle.ConceptoContable.ConceptoContable;
                this.MiOrdenCobro.OrdenesCobrosDetalles.Add(detalle);
                detalle.IndiceColeccion = this.MiOrdenCobro.OrdenesCobrosDetalles.IndexOf(detalle);
            }

            detalle.Importe = Convert.ToDecimal(this.txtImporte.Text);
            this.txtImporte.Text = string.Empty;
            AyudaProgramacion.CargarGrillaListas<CobOrdenesCobrosDetalles>(this.MiOrdenCobro.OrdenesCobrosDetalles, false, this.gvOrdenesCobrosDetalles, true);
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
                    this.MiOrdenCobro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    this.MiOrdenCobro.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    this.MiOrdenCobro.Moneda.IdMoneda = (int)EnumTGEMonedas.PesosArgentinos;
                    this.MiOrdenCobro.ImporteTotal = this.MiOrdenCobro.OrdenesCobrosDetalles.Sum(x => x.Importe);
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
                if(this.MiOrdenCobro.Estado.IdEstado==(int)EstadosOrdenesCobro.Cobrado)
                    this.ctrAsientoMostrar.IniciarControl(this.MiOrdenCobro);
            }
            else
            {
                this.MostrarMensaje(this.MiOrdenCobro.CodigoMensaje, true, this.MiOrdenCobro.CodigoMensajeArgs);
                if (this.MiOrdenCobro.dsResultado != null)
                {
                    this.ctrPopUpGrilla.IniciarControl(this.MiOrdenCobro);
                    this.MiOrdenCobro.dsResultado = null;
                }
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