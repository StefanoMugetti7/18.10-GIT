using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Compras;
using Generales.FachadaNegocio;
using System.Data;

namespace IU.Modulos.Compras
{
    public partial class ConsultarStock : PaginaSegura
    {
        private DataTable MiStock
        {
            get { return (DataTable)Session[this.MiSessionPagina + "StockListarMiStock"]; }
            set { Session[this.MiSessionPagina + "StockListarMiStock"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.CargarCombos();
                CMPStock parametros = this.BusquedaParametrosObtenerValor<CMPStock>();
                if (parametros.BusquedaParametros)
                {
                    this.txtCodigoProducto.Text = parametros.Producto.IdProducto.ToString();
                    this.txtDescripcion.Text = parametros.Producto.Descripcion;
                    this.ddlFiliales.SelectedValue = parametros.IdFilial == 0 ? string.Empty : parametros.IdFilial.ToString();
                    //this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoProducto, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDescripcion, this.btnBuscar);
            }
        }

        private void CargarCombos()
        {
            this.ddlFiliales.DataSource = this.UsuarioActivo.Filiales; //TGEGeneralesF.FilialesObenerLista();
            this.ddlFiliales.DataValueField = "IdFilial";
            this.ddlFiliales.DataTextField = "Filial";
            this.ddlFiliales.DataBind();
            if (this.ddlFiliales.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlFiliales, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CMPStock parametros = this.BusquedaParametrosObtenerValor<CMPStock>();
            this.CargarLista(parametros);
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MiStock;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }

        #region Grilla
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiStock.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CMPStock parametros = this.BusquedaParametrosObtenerValor<CMPStock>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CMPStock>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MiStock;
            this.gvDatos.DataBind();
            //AyudaProgramacion.CargarGrillaListas<CMPStock>(this.MiStock, false, this.gvDatos, true);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MiStock = this.OrdenarGrillaDatos<CMPStock>(this.MiStock, e);
            this.gvDatos.DataSource = this.MiStock;
            this.gvDatos.DataBind();
            //AyudaProgramacion.CargarGrillaListas<CMPStock>(this.MiStock, false, this.gvDatos, true);
        }
        #endregion

        private void CargarLista(CMPStock pParametro)
        {
            pParametro.Producto.IdProducto = this.txtCodigoProducto.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCodigoProducto.Text);
            pParametro.Producto.Descripcion = this.txtDescripcion.Text;
            //pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pParametro.IdFilial = this.ddlFiliales.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFiliales.SelectedValue);
            pParametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CMPStock>(pParametro);
            this.MiStock = ComprasF.StockObtenerListaFiltro(pParametro);
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            this.gvDatos.DataSource = this.MiStock;
            this.gvDatos.DataBind();
            //AyudaProgramacion.CargarGrillaListas<CMPStock>(this.MiStock, false, this.gvDatos, true);

            if (this.MiStock.Rows.Count > 0)
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;
        }
    }
}