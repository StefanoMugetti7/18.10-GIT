using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Proveedores.Entidades;
using Generales.FachadaNegocio;
using Proveedores;
using Comunes.Entidades;

namespace IU.Modulos.CuentasPagar.Controles
{
    public partial class ProveedoresBuscarPopUp : ControlesSeguros
    {
        private List<CapProveedores> MisProveedores
        {
            get { return (List<CapProveedores>)Session[this.MiSessionPagina + "ProveedoresBuscarPopUpMisProveedores"]; }
            set { Session[this.MiSessionPagina + "ProveedoresBuscarPopUpMisProveedores"] = value; }
        }

        public delegate void ProveedoresBuscarEventHandler(CapProveedores e);
        public event ProveedoresBuscarEventHandler ProveedoresBuscar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroProveedor, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtRazonSocial, this.btnBuscar);
            }
        }

        public void IniciarControl()
        {
            CargarCombos();

            this.pnlBuscar.Visible = true;
            this.txtNumeroProveedor.Text = string.Empty;
            this.txtRazonSocial.Text = string.Empty;
            this.mpePopUp.Show();
        }

        public void IniciarControl(CapProveedores pProveedor)
        {
            this.pnlBuscar.Visible = false;
            CapProveedores filtro = new CapProveedores();
            filtro.IdProveedor = pProveedor.IdProveedor;
            filtro.RazonSocial = pProveedor.RazonSocial;
            this.MisProveedores = ProveedoresF.ProveedoresObtenerListaFiltro(filtro);
            AyudaProgramacion.CargarGrillaListas<CapProveedores>(this.MisProveedores, false, this.gvDatos, false);
            this.mpePopUp.Show();
        }

        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEstados, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            //this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("SeleccioneOpción"), "-1"));
            //this.ddlEstados.SelectedValue = "-1";
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarLista();
            this.mpePopUp.Show();
        }

        private void CargarLista()
        {
            CapProveedores pProveedor = new CapProveedores();
            pProveedor.IdProveedor = this.txtNumeroProveedor.Text == string.Empty ? -1 : Convert.ToInt32(this.txtNumeroProveedor.Text);
            pProveedor.RazonSocial = this.txtRazonSocial.Text;
            pProveedor.CUIT = this.txtCuit.Text;
            pProveedor.Estado.IdEstado = this.ddlEstados.SelectedValue== String.Empty ? (int)EstadosTodos.Todos : Convert.ToInt32(this.ddlEstados.SelectedValue);

            this.MisProveedores = ProveedoresF.ProveedoresObtenerListaFiltro(pProveedor);
            this.gvDatos.DataSource = this.MisProveedores;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CapProveedores proveedor = this.MisProveedores[indiceColeccion];

            if (this.ProveedoresBuscar != null)
            {
                this.ProveedoresBuscar(proveedor);
                this.mpePopUp.Hide();
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
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisProveedores.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisProveedores;
            gvDatos.DataBind();

            this.mpePopUp.Show();
        }
        
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisProveedores = this.OrdenarGrillaDatos<CapProveedores>(this.MisProveedores, e);
            this.gvDatos.DataSource = this.MisProveedores;
            this.gvDatos.DataBind();

            this.mpePopUp.Show();
        }

    }
}