using Compras;
using Compras.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;

namespace IU.Modulos.Compras
{
    public partial class ProductosListar : PaginaSegura
    {
        private DataTable MisProductos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ProductosListarMisProductos"]; }
            set { Session[this.MiSessionPagina + "ProductosListarMisProductos"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.gvDatos.PageSizeEvent += GvDatos_PageSizeEvent;
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("ProductosAgregar.aspx");
                this.CargarCombos();
                CMPProductos parametros = this.BusquedaParametrosObtenerValor<CMPProductos>();
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDescripcion, this.btnBuscar);
                if (parametros.BusquedaParametros)
                {
                    this.txtDescripcion.Text = parametros.Descripcion;
                    this.ddlFamilias.SelectedValue = parametros.Familia.IdFamilia == 0 ? string.Empty : parametros.Familia.IdFamilia.ToString();
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }
        private void GvDatos_PageSizeEvent(int pageSize)
        {
            CMPProductos parametros = this.BusquedaParametrosObtenerValor<CMPProductos>();
            parametros.PageIndex = 0;
            this.gvDatos.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            this.CargarLista(parametros);
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/ProductosAgregar.aspx"), true);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CMPProductos parametros = this.BusquedaParametrosObtenerValor<CMPProductos>();
            parametros.PageIndex = 0;
            this.gvDatos.PageIndex = 0;
            this.CargarLista(parametros);
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idproducto = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable
            {
                { "IdProducto", idproducto }
            };

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/ProductosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/ProductosConsultar.aspx"), true);
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                modificar.Visible = this.ValidarPermiso("ProductosModificar.aspx");
                consultar.Visible = this.ValidarPermiso("ProductosConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisProductos.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CMPProductos parametros = BusquedaParametrosObtenerValor<CMPProductos>();
            this.gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            this.CargarLista(parametros);
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisProductos = this.OrdenarGrillaDatos<CMPProductos>(this.MisProductos, e);
            this.gvDatos.DataSource = this.MisProductos;
            this.gvDatos.DataBind();
        }
        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlEstado.SelectedValue = "-1";

            this.ddlFamilias.DataSource = ComprasF.FamiliasObtenerListaFiltro(new CMPFamilias());
            this.ddlFamilias.DataValueField = "IdFamilia";
            this.ddlFamilias.DataTextField = "Descripcion";
            this.ddlFamilias.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFamilias, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void CargarLista(CMPProductos pParametro)
        {
            pParametro.Descripcion = this.txtDescripcion.Text;
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pParametro.Familia.IdFamilia = this.ddlFamilias.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFamilias.SelectedValue);
            pParametro.BusquedaParametros = true;
            pParametro.PageSize = UsuarioActivo.PageSize > 0 ? UsuarioActivo.PageSize : 25;
            gvDatos.PageSize = UsuarioActivo.PageSize > 0 ? UsuarioActivo.PageSize : 25;
            pParametro.UsuarioLogueado.IdUsuarioEvento = this.UsuarioActivo.IdUsuarioEvento;
            this.BusquedaParametrosGuardarValor<CMPProductos>(pParametro);
            this.MisProductos = ComprasF.ProductosObtenerGrilla(pParametro);
            this.gvDatos.DataSource = this.MisProductos;
            this.gvDatos.VirtualItemCount = MisProductos.Rows.Count > 0 ? Convert.ToInt32(MisProductos.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + gvDatos.VirtualItemCount.ToString();
            this.gvDatos.DataBind();
        }
    }
}
