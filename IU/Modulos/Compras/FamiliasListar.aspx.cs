using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Comunes.Entidades;
using System.Collections;
using Generales.FachadaNegocio;
using Compras;


namespace IU.Modulos.Compras
{
    public partial class FamiliasListar : PaginaSegura
    {
        private List<CMPFamilias> MisFamilias
        {
            get { return (List<CMPFamilias>)Session[this.MiSessionPagina + "FamiliasListarMisFamilias"]; }
            set { Session[this.MiSessionPagina + "FamiliasListarMisFamilias"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("FamiliasAgregar.aspx");
                this.CargarCombos();
                CMPFamilias parametros = this.BusquedaParametrosObtenerValor<CMPFamilias>();
                if (parametros.BusquedaParametros)
                {
                    this.txtDescripcion.Text = parametros.Descripcion;
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/FamiliasAgregar.aspx"), true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CMPFamilias parametros = this.BusquedaParametrosObtenerValor<CMPFamilias>();
            this.CargarLista(parametros);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName==Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CMPFamilias familia = this.MisFamilias[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdFamilia", familia.IdFamilia);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/FamiliasModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/FamiliasConsultar.aspx"), true);
            }
             
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CMPFamilias bancoCuenta = (CMPFamilias)e.Row.DataItem;

                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                modificar.Visible = this.ValidarPermiso("FamiliasModificar.aspx");
                consultar.Visible = this.ValidarPermiso("FamiliasConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisFamilias.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CMPFamilias parametros = this.BusquedaParametrosObtenerValor<CMPFamilias>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CMPFamilias>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            AyudaProgramacion.CargarGrillaListas<CMPFamilias>(this.MisFamilias, false, this.gvDatos, true);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisFamilias = this.OrdenarGrillaDatos<CMPFamilias>(this.MisFamilias, e);
            AyudaProgramacion.CargarGrillaListas<CMPFamilias>(this.MisFamilias, false, this.gvDatos, true);
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlEstado.SelectedValue = "-1";
           
        }

        private void CargarLista(CMPFamilias pParametro)
        {
            pParametro.Descripcion = this.txtDescripcion.Text;
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pParametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CMPFamilias>(pParametro);
            this.MisFamilias = ComprasF.FamiliasObtenerListaFiltro(pParametro);
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            AyudaProgramacion.CargarGrillaListas<CMPFamilias>(this.MisFamilias, false, this.gvDatos, true);
        }
    }
}
