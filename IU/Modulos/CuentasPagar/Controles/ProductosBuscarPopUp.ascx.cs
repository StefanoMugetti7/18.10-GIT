using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Generales.FachadaNegocio;
using Compras;
using Comunes.Entidades;
using System.Data;

namespace IU.Modulos.CuentasPagar.Controles
{
    public partial class BuscarProductoPopUp : ControlesSeguros
    {
        private EnumTiposProductos MiTipoProducto
        {
            get { return (EnumTiposProductos)Session[this.MiSessionPagina + "ProductosBuscarPopUpMiTipoProducto"]; }
            set { Session[this.MiSessionPagina + "ProductosBuscarPopUpMiTipoProducto"] = value; }
        }

        private CMPProductos MiProductoFiltro
        {
            get { return (CMPProductos)Session[this.MiSessionPagina + "ProductosBuscarPopUpMiProductoFiltro"]; }
            set { Session[this.MiSessionPagina + "ProductosBuscarPopUpMiProductoFiltro"] = value; }
        }

        private List<CMPProductos> MisProductos
        {
            get { return (List<CMPProductos>)Session[this.MiSessionPagina + "ProductosBuscarPopUpMisProductos"]; }
            set { Session[this.MiSessionPagina + "ProductosBuscarPopUpMisProductos"] = value; }
        }

        private List<CMPProductos> MisProductosSeleccionados
        {
            get { return (List<CMPProductos>)Session[this.MiSessionPagina + "ProductosBuscarPopUpMisProductosSeleccionados"]; }
            set { Session[this.MiSessionPagina + "ProductosBuscarPopUpMisProductosSeleccionados"] = value; }
        }

        private DataTable TableProductos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ProductosBuscarPopUpTableProductos"]; }
            set { Session[this.MiSessionPagina + "ProductosBuscarPopUpTableProductos"] = value; }
        }
        //private List<CMPFamilias> MisFamilias
        //{
        //    get { return (List<CMPFamilias>)Session[this.MiSessionPagina + "ProductosBuscarPopUpMisFamilias"]; }
        //    set { Session[this.MiSessionPagina + "ProductosBuscarPopUpMisFamilias"] = value; }
        //}

        public delegate void ProductosBuscarEventHandler(CMPProductos e);
        public event ProductosBuscarEventHandler ProductosBuscarSeleccionar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroProducto, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDescripcion, this.btnBuscar);
            }
        }

        public void IniciarControl(EnumTiposProductos pTipoProducto, CMPProductos pFiltro)
        {
            if (this.MiProductoFiltro != null && this.MiProductoFiltro.IdFilial != pFiltro.IdFilial)
            {
                this.IniciarControl(pTipoProducto, pFiltro, new List<CMPProductos>());
            }
            else
            {
                this.MiProductoFiltro = pFiltro;
                this.MiTipoProducto = pTipoProducto;
                this.CargarCombos();
                this.pnlBuscar.Visible = true;
                this.txtNumeroProducto.Text = string.Empty;
                this.txtDescripcion.Text = string.Empty;
                this.mpePopUp.Show();
            }
        }

        //public void IniciarControl(CMPProductos pProducto, EnumTiposProductos pTipoProducto)
        //{
        //    this.MiTipoProducto = pTipoProducto;
        //    this.pnlBuscar.Visible = false;
        //    this.MiProductoFiltro = pProducto;
        //    CargarLista();
        //    this.mpePopUp.Show();
        //}

        //metodo para el buscador de Listas Precios
        public void IniciarControl(EnumTiposProductos pTipoProducto, CMPProductos pFiltro, List<CMPProductos> filtroLP)
        {
            this.MiProductoFiltro = pFiltro;
            this.MiTipoProducto = pTipoProducto;
            this.MisProductosSeleccionados = filtroLP;
            this.CargarCombos();
            this.pnlBuscar.Visible = true;
            this.txtNumeroProducto.Text = string.Empty;
            this.txtDescripcion.Text = string.Empty;

            //this.CargarLista();

            this.mpePopUp.Show();
        }

        private void CargarCombos()
        {
            CMPFamilias familia = new CMPFamilias();
            familia.Estado.IdEstado = (int)Estados.Activo;
            this.ddlFamilias.DataSource = ComprasF.FamiliasObtenerListaFiltro(familia);
            this.ddlFamilias.DataValueField = "IdFamilia";
            this.ddlFamilias.DataTextField = "Descripcion";
            this.ddlFamilias.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFamilias, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private CMPProductos ObteneTipoProducto(CMPProductos pParametro)
        {
            if (this.MiTipoProducto == EnumTiposProductos.Compras)
            {
                pParametro.Compra = true;
                pParametro.Venta = false;
            }
            else if (this.MiTipoProducto == EnumTiposProductos.Ventas)
            {
                pParametro.Venta = true;
                pParametro.Compra = false;
            }
            else if (this.MiTipoProducto == EnumTiposProductos.ComprasYVentas)
            {
                pParametro.Compra = true;
                pParametro.Venta = true;
            }

            return pParametro;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CMPProductos parametros = this.BusquedaParametrosObtenerValor<CMPProductos>();
            CargarLista();
            this.mpePopUp.Show();
        }

        protected void gvProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CMPProductos producto = new CMPProductos();
            producto.IdProducto = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdProducto"].ToString());
            producto.Descripcion = ((GridView)sender).DataKeys[index]["Descripcion"].ToString();
            producto.Familia.IdFamilia = Convert.ToInt32(((GridView)sender).DataKeys[index]["FamiliaIdFamilia"].ToString());
            producto.Familia.Descripcion = ((GridView)sender).DataKeys[index]["FamiliaDescripcion"].ToString();


            if (e.CommandName == Gestion.Consultar.ToString())
            {
                if (this.ProductosBuscarSeleccionar != null)
                {
                    this.ProductosBuscarSeleccionar(producto);
                    this.mpePopUp.Hide();
                }
            }
        }

        protected void gvProductos_RowDataBound(object sender, GridViewRowEventArgs e)
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
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), TableProductos.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //CMPProductos parametros = this.BusquedaParametrosObtenerValor<CMPProductos>();
            //parametros.IndiceColeccion = e.NewPageIndex;
            //this.BusquedaParametrosGuardarValor<CMPProductos>(parametros);

            gvProductos.PageIndex = e.NewPageIndex;
            gvProductos.DataSource = TableProductos;
            gvProductos.DataBind();

            this.mpePopUp.Show();
        }

        protected void gvProductos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisProductos = this.OrdenarGrillaDatos<CMPProductos>(this.MisProductos, e);
            this.gvProductos.DataSource = this.MisProductos;
            this.gvProductos.DataBind();

            this.mpePopUp.Show();
        }

        private void CargarLista()
        {
            this.MiProductoFiltro.IdProducto = this.txtNumeroProducto.Text == string.Empty ? 0 : Convert.ToInt32(this.txtNumeroProducto.Text);
            this.MiProductoFiltro.Descripcion = this.txtDescripcion.Text;
            this.MiProductoFiltro.Familia.IdFamilia = this.ddlFamilias.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFamilias.SelectedValue);
            this.MiProductoFiltro.Estado.IdEstado = (int)Estados.Activo;
            this.ObteneTipoProducto(this.MiProductoFiltro);
            TableProductos = ComprasF.ProductosObtenerListaFiltroDT(this.MiProductoFiltro);
            if (this.MisProductosSeleccionados != null && this.MisProductosSeleccionados.Count > 0)
                this.FiltrarItemsLista();

            this.gvProductos.Columns[3].Visible = this.MiProductoFiltro.IdFilial > 0;
            this.gvProductos.DataSource = TableProductos;
            this.gvProductos.DataBind();
        }

        private void FiltrarItemsLista()
        {
            DataRow dr;
            
            if (this.MisProductosSeleccionados != null)
            {
                foreach (CMPProductos yaIngresado in this.MisProductosSeleccionados)
                {
                    int idingreso = yaIngresado.IdProducto;
                    //MisProductos.RemoveAll(x => x.IdProducto == yaIngresado.IdProducto);
                    dr = TableProductos.AsEnumerable().FirstOrDefault(x => x.Field<Int32>("IdProducto") == yaIngresado.IdProducto);
                    if (TableProductos.AsEnumerable().ToList().Exists(x => x.Field<int>("IdProducto") == idingreso))
                    {

                        TableProductos.Rows.Remove(dr);
                        TableProductos.AcceptChanges();
                    }
                }
                //this.MisProductos = AyudaProgramacion.AcomodarIndices(this.MisProductos);
            }
        }
    }
}
