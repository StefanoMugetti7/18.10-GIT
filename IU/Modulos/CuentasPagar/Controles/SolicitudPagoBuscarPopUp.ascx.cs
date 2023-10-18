using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Proveedores.Entidades;
using CuentasPagar.Entidades;
using Generales.Entidades;
using CuentasPagar.FachadaNegocio;
using Comunes.Entidades;

namespace IU.Modulos.CuentasPagar.Controles
{
    public partial class SolicitudPagoBuscarPopUp : ControlesSeguros
    {
        private CapSolicitudPago MiSolicitud
        {
            get { return (CapSolicitudPago)Session[this.MiSessionPagina + "SolicitudPagoBuscarPopUpMiSolicitud"]; }
            set { Session[this.MiSessionPagina + "SolicitudPagoBuscarPopUpMiSolicitud"] = value; }
        }

        private List<CapSolicitudPago> MisSolicitudes
        {
            get { return (List<CapSolicitudPago>)Session[this.MiSessionPagina + "SolicitudPagoBuscarPopUpMisSolicitudes"]; }
            set { Session[this.MiSessionPagina + "SolicitudPagoBuscarPopUpMisSolicitudes"] = value; }
        }

        private List<CapSolicitudPago> MisSolicitudesSeleccionadas
        {
            get { return (List<CapSolicitudPago>)Session[this.MiSessionPagina + "SolicitudPagoBuscarPopUpMisSolicitudesSeleccionadas"]; }
            set { Session[this.MiSessionPagina + "SolicitudPagoBuscarPopUpMisSolicitudesSeleccionadas"] = value; }
        }

        public delegate void ControlBuscarEventHandler(CapSolicitudPago e);
        public event ControlBuscarEventHandler ControlBuscarSeleccionar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }

        public void IniciarControl(CapSolicitudPago pParametro)
        {
            this.MiSolicitud = pParametro;
            this.CargarLista();
            //this.mpePopUp.Show();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalBuscarSolicitudPago();", true);

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarLista();
            //this.mpePopUp.Show();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalBuscarSolicitudPago();", true);

        }

        private void CargarLista()
        {
            this.MiSolicitud.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.MiSolicitud.IdSolicitudPago = this.txtNumeroSolicitud.Text == String.Empty ? 0 : Convert.ToInt32(this.txtNumeroSolicitud.Text);
            this.MiSolicitud.NumeroFactura = this.txtNumeroFactura.Text == string.Empty ? "" : this.txtNumeroFactura.Text;
            //pSolicitud.PrefijoNumeroFactura = this.txtPrefijoNumeroFactura.Text == String.Empty ? "" : this.txtPrefijoNumeroFactura.Text;
            this.MiSolicitud.TipoSolicitudPago.IdTipoSolicitudPago = this.ddlTipoSolicitud.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlTipoSolicitud.SelectedValue);

            this.MiSolicitud.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            this.MiSolicitud.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);

            this.MisSolicitudes = CuentasPagarF.SolicitudPagoObtenerListaFiltro(this.MiSolicitud);
            
            AyudaProgramacion.CargarGrillaListas<CapSolicitudPago>(this.MisSolicitudes, false, this.gvDatos, false);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CapSolicitudPago solPago = this.MisSolicitudes[indiceColeccion];

            if (e.CommandName == Gestion.Consultar.ToString())
            {
                if (this.ControlBuscarSeleccionar != null)
                {
                    this.ControlBuscarSeleccionar(solPago);
                    //this.mpePopUp.Hide();
                }
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");

            //    //Permisos btnEliminar
            //    ibtnConsultar.Visible = this.ValidarPermiso("ProductosConsultar.aspx");
            //}
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisSolicitudes.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //CMPProductos parametros = this.BusquedaParametrosObtenerValor<CMPProductos>();
            //parametros.IndiceColeccion = e.NewPageIndex;
            //this.BusquedaParametrosGuardarValor<CMPProductos>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisSolicitudes;
            gvDatos.DataBind();

            //this.mpePopUp.Show();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisSolicitudes = this.OrdenarGrillaDatos<CapSolicitudPago>(this.MisSolicitudes, e);
            this.gvDatos.DataSource = this.MisSolicitudes;
            this.gvDatos.DataBind();

            //this.mpePopUp.Show();
        }
    }
}