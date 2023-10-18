using Bancos;
using Bancos.Entidades;
using Comunes.Entidades;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace IU.Modulos.Bancos
{
    public partial class PlazosFijosPropiosListar : PaginaSegura
    {
        private DataTable MisPlazosFijos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "PlazosFijosPropiosListarMisPlazosFijos"]; }
            set { Session[this.MiSessionPagina + "PlazosFijosPropiosListarMisPlazosFijos"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                TESPlazosFijos plazosFijos = new TESPlazosFijos();

                this.ddlBancos.DataSource = BancosF.CuentasObtenerListaFiltro(plazosFijos.BancoCuenta);
                this.ddlBancos.DataValueField = "IdBancoCuenta";
                this.ddlBancos.DataTextField = "Denominacion";
                this.ddlBancos.DataBind();

                AyudaProgramacion.AgregarItemSeleccione(this.ddlBancos, ObtenerMensajeSistema("SeleccioneOpcion"));
                this.btnAgregar.Visible = this.ValidarPermiso("PlazosFijosAgregar.aspx");
                this.CargarLista(plazosFijos);
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/PlazosFijosPropiosAgregar.aspx"), true);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TESPlazosFijos plazosFijos = new TESPlazosFijos();
            plazosFijos.BancoCuenta.IdBancoCuenta = this.ddlBancos.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlBancos.SelectedValue);
            this.CargarLista(plazosFijos);
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            ListaParametros parametros = new ListaParametros(this.MiSessionPagina);
            parametros.Agregar("IdPlazoFijo", indiceColeccion);//proveedor.IdProveedor);

            if (e.CommandName == Gestion.Anular.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/PlazosFijosPropiosAnular.aspx"), true);
            else if (e.CommandName == Gestion.Consultar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/PlazosFijosPropiosConsultar.aspx"), true);
            else if (e.CommandName == "Modificar")
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/PlazosFijosPropiosModificar.aspx"), true);
            else if (e.CommandName == Gestion.Pagar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/PlazosFijosPropiosPagar.aspx"), true);
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton ibtnPagar = (ImageButton)e.Row.FindControl("btnPagar");

                ibtnConsultar.Visible = this.ValidarPermiso("PlazosFijosPropiosConsultar.aspx");
                ibtnModificar.Visible = this.ValidarPermiso("PlazosFijosPropiosModificar.aspx");

                if (Convert.ToInt32(dr["IdEstado"]) == 12)
                    ibtnAnular.Visible = this.ValidarPermiso("PlazosFijosPropiosAnular.aspx");

                if (((Convert.ToDateTime(dr["FechaVencimiento"])) <= DateTime.Now.Date) && (Convert.ToInt32(dr["IdEstado"]) == 12))
                    ibtnPagar.Visible = this.ValidarPermiso("PlazosFijosPropiosPagar.aspx");

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisPlazosFijos.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TESPlazosFijos parametros = this.BusquedaParametrosObtenerValor<TESPlazosFijos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TESPlazosFijos>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisPlazosFijos;
            this.gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisPlazosFijos = this.OrdenarGrillaDatos<TESPlazosFijos>(this.MisPlazosFijos, e);
            this.gvDatos.DataSource = this.MisPlazosFijos;
            this.gvDatos.DataBind();
        }
        private void CargarLista(TESPlazosFijos pPlazosFijos)
        {
            this.MisPlazosFijos = BancosF.PlazosFijosObtenerListaFiltroDT(pPlazosFijos);
            this.gvDatos.DataSource = this.MisPlazosFijos;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(this.gvDatos);
        }
    }
}