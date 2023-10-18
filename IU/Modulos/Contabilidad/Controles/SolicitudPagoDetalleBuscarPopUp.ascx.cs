using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuentasPagar.Entidades;
using CuentasPagar.FachadaNegocio;
using Comunes.Entidades;
using System.Data;

namespace IU.Modulos.Contabilidad.Controles
{
    public partial class SolicitudPagoDetalleBuscarPopUp : ControlesSeguros
    {
        private DataTable MisSolicitudesPagoDetalles
        {
            get { return (DataTable)Session[this.MiSessionPagina + "SolicitudPagoDetallesBuscarPopUpMisSolicitudesPagoDetalles"]; }
            set { Session[this.MiSessionPagina + "SolicitudPagoDetallesBuscarPopUpMisSolicitudesPagoDetalles"] = value; }
        }

        private int MiPaginaGrilla
        {
            get { return (int)Session[this.MiSessionPagina + "PaginaGrillaBuscarPopUpMisSolicitudPagoDetalle"]; }
            set { Session[this.MiSessionPagina + "PaginaGrillaBuscarPopUpMisSolicitudPagoDetalle"] = value; }
        }

        public delegate void SolicitudPagoDetalleBuscarPopUpEventHandler(CapSolicitudPagoDetalles e);
        public event SolicitudPagoDetalleBuscarPopUpEventHandler SolicitudPagoDetalleBuscarSeleccionarPopUp;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }

        public void IniciarControl(bool pLimpiarDatos)
        {
            if (pLimpiarDatos)
            {
                this.MisSolicitudesPagoDetalles = new DataTable();
                this.MiPaginaGrilla = 0;
                this.gvDatos.DataSource = this.MisSolicitudesPagoDetalles;
                this.gvDatos.DataBind();
            }
            this.mpePopUp.Show();
        }

        //public void IniciarControl(bool pLimpiarDatos, List<CapSolicitudPagoDetalles> pSolicitudPagoDetalle)
        //{
        //    if (pLimpiarDatos)
        //    {
        //        this.MisSolicitudesPagoDetalles = new List<CapSolicitudPagoDetalles>();
        //        this.MiPaginaGrilla = 0;
        //        this.gvDatos.DataSource = this.MisSolicitudesPagoDetalles;
        //        this.gvDatos.DataBind();
        //    }
        //    this.MisSolicitudesPagoDetalles = pSolicitudPagoDetalle;
        //    this.gvDatos.DataSource = this.MisSolicitudesPagoDetalles;
        //    this.gvDatos.DataBind();
        //    this.mpePopUp.Show();
        //}

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.CargarLista();
            this.mpePopUp.Show();
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()))
                return;

            CapSolicitudPagoDetalles solicitudPagoDetalle = new CapSolicitudPagoDetalles();
            solicitudPagoDetalle.IdSolicitudPagoDetalle = Convert.ToInt32(this.gvDatos.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
            
            solicitudPagoDetalle = CuentasPagarF.SolicitudPagoDetallesObtenerDatosCompletos(solicitudPagoDetalle);

            if (this.SolicitudPagoDetalleBuscarSeleccionarPopUp != null)
                this.SolicitudPagoDetalleBuscarSeleccionarPopUp(solicitudPagoDetalle);
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisSolicitudesPagoDetalles.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
            this.mpePopUp.Show();
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDatos.PageIndex = e.NewPageIndex;
            this.MiPaginaGrilla = e.NewPageIndex;
            gvDatos.DataSource = this.MisSolicitudesPagoDetalles;
            gvDatos.DataBind();
            this.mpePopUp.Show();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //this.MisSolicitudesPagoDetalles = this.OrdenarGrillaDatos<DataTable>(this.MisSolicitudesPagoDetalles, e);
            //this.gvDatos.DataSource = this.MisSolicitudesPagoDetalles;
            //this.gvDatos.DataBind();
            //this.mpePopUp.Show();
        }

        private void CargarLista()
        {
            CapSolicitudPagoDeatallesSolicitudPago solicitudPagoDetalle = new CapSolicitudPagoDeatallesSolicitudPago();
            solicitudPagoDetalle.SolicitudPago.Observacion = this.txtRazonSocial.Text.Trim();
            solicitudPagoDetalle.SolicitudPago.PrefijoNumeroFactura = this.txtPreNumeroFactura.Text.Trim();
            solicitudPagoDetalle.SolicitudPago.NumeroFactura = this.txtNumeroFactura.Text.Trim();
            solicitudPagoDetalle.SolicitudPago.FechaDesde = this.txtFechaDesde.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaDesde.Text);
            solicitudPagoDetalle.SolicitudPago.FechaHasta = this.txtFechaHasta.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaHasta.Text);
            solicitudPagoDetalle.Descripcion = this.txtDescripcion.Text.Trim();

            this.MisSolicitudesPagoDetalles = CuentasPagarF.SolicitudPagoDetalleObtenerItemsBienesUsoGrilla(solicitudPagoDetalle);
            this.gvDatos.DataSource = this.MisSolicitudesPagoDetalles;
            this.gvDatos.DataBind();
        }
    }
}