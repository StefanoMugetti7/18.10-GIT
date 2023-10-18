using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Compras;
using Comunes.Entidades;

namespace IU.Modulos.Compras.Controles
{
    public partial class CotizacionesBuscarPopUp : ControlesSeguros
    {
        private List<CmpCotizacionesDetalles> MisCotizacionesDetalles
        {
            get { return (List<CmpCotizacionesDetalles>)Session[this.MiSessionPagina + "CotizacionesBuscarPopUpMisCotizaciones"]; }
            set { Session[this.MiSessionPagina + "CotizacionesBuscarPopUpMisCotizaciones"] = value; }
        }

        public delegate void CotizacionesBuscarEventHandler(CmpSolicitudesComprasDetalles e);
        public event CotizacionesBuscarEventHandler CotizacionesBuscarSeleccionar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroProducto, this.btnBuscar);
                
            }
        }

        public void IniciarControl()
        {
            this.pnlBuscaCotizacion.Visible = true;
            this.txtNumeroProducto.Text = string.Empty;
            this.mpePopUp.Show();
        }

        public void IniciarControl(CmpSolicitudesComprasDetalles pSolicitud)
        {
            this.txtNumeroProducto.Text = (pSolicitud.Producto.IdProducto).ToString();
            this.txtNumeroProducto.Enabled = false;
           
            this.MisCotizacionesDetalles = ComprasF.CotizacionesObtenerListaFiltroPorProducto(pSolicitud.Producto);
            //AyudaProgramacion.CargarGrillaListas<CmpCotizacionesDetalles>(this.MisCotizacionesDetalles, false, this.gvCotizacion, false);
            this.gvCotizacion.DataSource = this.MisCotizacionesDetalles;
            this.gvCotizacion.DataBind();
            this.mpePopUp.Show();
            
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //CMPProductos parametros = this.BusquedaParametrosObtenerValor<CMPProductos>();
            CargarLista();
            this.mpePopUp.Show();
        }

        protected void gvCotizacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CmpSolicitudesComprasDetalles sol = new CmpSolicitudesComprasDetalles();
            sol.Producto = this.MisCotizacionesDetalles[indiceColeccion].Producto;
            sol.PrecioUnitario = this.MisCotizacionesDetalles[indiceColeccion].PrecioUnitario;
            sol.IdCotizacionDetalle = this.MisCotizacionesDetalles[indiceColeccion].IdCotizacionDetalle;
            if (e.CommandName == Gestion.Consultar.ToString())
            {
                if (this.CotizacionesBuscarSeleccionar != null)
                {
                    this.CotizacionesBuscarSeleccionar(sol);
                    this.mpePopUp.Hide();
                }
            }
        }

        protected void gvCotizacion_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisCotizacionesDetalles.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvCotizacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            gvCotizacion.PageIndex = e.NewPageIndex;
            gvCotizacion.DataSource = this.MisCotizacionesDetalles;
            gvCotizacion.DataBind();

            this.mpePopUp.Show();
        }

        protected void gvCotizacion_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisCotizacionesDetalles = this.OrdenarGrillaDatos<CmpCotizacionesDetalles>(this.MisCotizacionesDetalles, e);
            this.gvCotizacion.DataSource = this.MisCotizacionesDetalles;
            this.gvCotizacion.DataBind();

            this.mpePopUp.Show();
        }

        private void CargarLista()
        {

            CMPProductos pProducto = new CMPProductos();
            pProducto.IdProducto = this.txtNumeroProducto.Text == string.Empty ? -1 : Convert.ToInt32(this.txtNumeroProducto.Text);
            this.MisCotizacionesDetalles = ComprasF.CotizacionesObtenerListaFiltroPorProducto(pProducto);
            this.gvCotizacion.DataSource = this.MisCotizacionesDetalles;
            this.gvCotizacion.DataBind();


        }
    }
}