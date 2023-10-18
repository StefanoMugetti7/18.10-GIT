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


namespace IU.Modulos.Facturas.Controles
{
    public partial class BuscarProductoPopUp : ControlesSeguros
    {
        private List<CMPListasPreciosDetalles> MisListasPreciosDetalles
        {
            get { return (List<CMPListasPreciosDetalles>)Session[this.MiSessionPagina + "ProductosBuscarPopUpMisListasPreciosDetalles"]; }
            set { Session[this.MiSessionPagina + "ProductosBuscarPopUpMisListasPreciosDetalles"] = value; }
        }

        private CMPListasPrecios MiFiltroListaPrecio
        {
            get { return (CMPListasPrecios)Session[this.MiSessionPagina + "ProductosBuscarPopUpMiFiltroListaPrecio"]; }
            set { Session[this.MiSessionPagina + "ProductosBuscarPopUpMiFiltroListaPrecio"] = value; }
        }

        //private List<CMPFamilias> MisFamilias
        //{
        //    get { return (List<CMPFamilias>)Session[this.MiSessionPagina + "ProductosBuscarPopUpMisFamilias"]; }
        //    set { Session[this.MiSessionPagina + "ProductosBuscarPopUpMisFamilias"] = value; }
        //}

        public delegate void ProductosBuscarEventHandler(CMPListasPreciosDetalles e);
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

        public void IniciarControl(CMPListasPrecios pFiltro)
        {
            this.MiFiltroListaPrecio = pFiltro;
            this.CargarCombos();
            this.pnlBuscar.Visible = true;
            this.txtNumeroProducto.Text = string.Empty;
            this.txtDescripcion.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalBuscarProducto();", true);
        }

        public void IniciarControl(CMPProductos pProducto, CMPListasPrecios pFiltro)
        {
            this.pnlBuscar.Visible = false;
            CMPListasPreciosDetalles filtro = new CMPListasPreciosDetalles();
            filtro.Producto = pProducto;
            this.MisListasPreciosDetalles = ComprasF.ListasPreciosDetallesObtenerListaFiltro(filtro);
            AyudaProgramacion.CargarGrillaListas<CMPListasPreciosDetalles>(this.MisListasPreciosDetalles, false, this.gvProductos, false);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalBuscarProducto();", true);
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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //CMPProductos parametros = this.BusquedaParametrosObtenerValor<CMPProductos>();
            CargarLista();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModal", "ShowModaBuscarProducto();", true);
        }

        protected void gvProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CMPListasPreciosDetalles producto = this.MisListasPreciosDetalles[indiceColeccion];

            if (e.CommandName == Gestion.Consultar.ToString())
            {
                if (this.ProductosBuscarSeleccionar != null)
                {
                    this.ProductosBuscarSeleccionar(producto);
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
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisListasPreciosDetalles.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //CMPProductos parametros = this.BusquedaParametrosObtenerValor<CMPProductos>();
            //parametros.IndiceColeccion = e.NewPageIndex;
            //this.BusquedaParametrosGuardarValor<CMPProductos>(parametros);

            gvProductos.PageIndex = e.NewPageIndex;
            gvProductos.DataSource = this.MisListasPreciosDetalles;
            gvProductos.DataBind();

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModal", "ShowModalBuscarProducto();", true);
        }

        protected void gvProductos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisListasPreciosDetalles = this.OrdenarGrillaDatos<CMPListasPreciosDetalles>(this.MisListasPreciosDetalles, e);
            this.gvProductos.DataSource = this.MisListasPreciosDetalles;
            this.gvProductos.DataBind();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModal", "ShowModalBuscarProducto();", true);
        }

        private void CargarLista()
        {
            CMPListasPreciosDetalles filtro = new CMPListasPreciosDetalles();
            filtro.Producto.IdProducto = this.txtNumeroProducto.Text == string.Empty ? 0 : Convert.ToInt32(this.txtNumeroProducto.Text);
            filtro.Producto.Descripcion = this.txtDescripcion.Text;
            filtro.Producto.Familia.IdFamilia = this.ddlFamilias.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFamilias.SelectedValue);
            filtro.Estado.IdEstado = (int)Estados.Activo;
            filtro.Fecha = MiFiltroListaPrecio.FechaAcopio.HasValue ? MiFiltroListaPrecio.FechaAcopio : DateTime.Now;
            filtro.NoIncluidoEnAcopio = MiFiltroListaPrecio.NoIncluidoEnAcopio;
            filtro.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
            filtro.ListaPrecio.IdAfiliado = this.MiFiltroListaPrecio.IdAfiliado;
            filtro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.MisListasPreciosDetalles = ComprasF.ListasPreciosDetallesObtenerListaFiltro(filtro);
            this.gvProductos.PageIndex = filtro.IndiceColeccion;
            AyudaProgramacion.CargarGrillaListas<CMPListasPreciosDetalles>(this.MisListasPreciosDetalles, false, this.gvProductos, true);
        }
    }
}
