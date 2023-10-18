using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Compras;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Compras.Controles
{
    public partial class OrdenesComprasBuscarPopUp : ControlesSeguros
    {
        private List<CmpOrdenesCompras> MisOrdenesCompras
        {
            get { return (List<CmpOrdenesCompras>)Session[this.MiSessionPagina + "OrdenesComprasBuscarPopUpMisOrdenesCompras"]; }
            set { Session[this.MiSessionPagina + "OrdenesComprasBuscarPopUpMisOrdenesCompras"] = value; }
        }

        public delegate void OrdenesComprasBuscarEventHandler(CmpOrdenesCompras e);
        public event OrdenesComprasBuscarEventHandler OrdenesComprasBuscarSeleccionar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoOrdenCompra, this.btnBuscar);
            }
        }

        public void IniciarControl()
        {
            CargarCombos();

            this.pnlBuscar.Visible = true;
            this.txtCodigoOrdenCompra.Text = string.Empty;

            this.mpePopUp.Show();
        }

        public void IniciarControl(CmpOrdenesCompras pOrden)
        {
            this.pnlBuscar.Visible = false;
            //CmpOrdenesCompras filtro = new CmpOrdenesCompras();
            //filtro.IdOrdenCompra = pOrden.IdOrdenCompra;
            this.MisOrdenesCompras = ComprasF.OrdenCompraObtenerLista(pOrden);
            AyudaProgramacion.CargarGrillaListas<CmpOrdenesCompras>(this.MisOrdenesCompras, false, this.gvDatos, false);
            this.mpePopUp.Show();
        }

        private void CargarCombos()
        {
            this.ddlCondicionPago.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.CondicionesPagos);
            this.ddlCondicionPago.DataValueField = "IdListaValorDetalle";
            this.ddlCondicionPago.DataTextField = "Descripcion";
            this.ddlCondicionPago.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCondicionPago, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            //this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosOrdenesCompras));
            //this.ddlEstados.DataValueField = "IdEstado";
            //this.ddlEstados.DataTextField = "Descripcion";
            //this.ddlEstados.DataBind();
            //this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            //this.ddlEstados.SelectedValue = ((int)EstadosTodos.Todos).ToString();

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarLista();
            this.mpePopUp.Show();
        }

        private void CargarLista()
        {

            CmpOrdenesCompras pOrdenCompra = new CmpOrdenesCompras();
            pOrdenCompra.IdOrdenCompra = this.txtCodigoOrdenCompra.Text == String.Empty ? 0 : Convert.ToInt32(this.txtCodigoOrdenCompra.Text);

            pOrdenCompra.CondicionPago.IdCondicionPago = this.ddlCondicionPago.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlCondicionPago.SelectedValue);
            //pOrdenCompra.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            //VER QUE BUSQUE CON EL POP UP

            pOrdenCompra.Proveedor.IdProveedor = this.txtProveedor.Text == String.Empty ? 0 : Convert.ToInt32(this.txtProveedor.Text);
            pOrdenCompra.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pOrdenCompra.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pOrdenCompra.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CmpOrdenesCompras>(pOrdenCompra);
            this.MisOrdenesCompras = ComprasF.OrdenCompraObtenerListaPopUp(pOrdenCompra);
            this.gvDatos.DataSource = this.MisOrdenesCompras;
            this.gvDatos.PageIndex = pOrdenCompra.IndiceColeccion;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CmpOrdenesCompras orden = new CmpOrdenesCompras();
             orden = this.MisOrdenesCompras[indiceColeccion];
             orden = ComprasF.OrdenCompraObtenerDatosCompletos(orden);
            if (e.CommandName == Gestion.Consultar.ToString())
            {

                if (this.OrdenesComprasBuscarSeleccionar != null)
                {
                    this.OrdenesComprasBuscarSeleccionar(orden);
                    this.mpePopUp.Hide();
                }
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");

            //    //Permisos btnEliminar
            //    ibtnConsultar.Visible = this.ValidarPermiso("AfiliadosConsultar.aspx");
            //}
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisOrdenesCompras.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisOrdenesCompras;
            gvDatos.DataBind();

            this.mpePopUp.Show();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisOrdenesCompras = this.OrdenarGrillaDatos<CmpOrdenesCompras>(this.MisOrdenesCompras, e);
            this.gvDatos.DataSource = this.MisOrdenesCompras;
            this.gvDatos.DataBind();

            this.mpePopUp.Show();
        }
    }
}