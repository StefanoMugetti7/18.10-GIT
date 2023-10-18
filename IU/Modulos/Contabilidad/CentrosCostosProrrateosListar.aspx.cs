using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Contabilidad;
using System.Collections;

namespace IU.Modulos.Contabilidad
{
    public partial class CentrosCostosProrrateosListar : PaginaSegura
    {
        private List<CtbCentrosCostosProrrateos> MisCentrosCostos
        {
            get { return (List<CtbCentrosCostosProrrateos>)Session[this.MiSessionPagina + "CentrosCostosProrrateosListarMisCentrosCostos"]; }
            set { Session[this.MiSessionPagina + "CentrosCostosProrrateosListarMisCentrosCostos"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.CargarCombos();
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("CentrosCostosProrrateosAgregar.aspx");
                CtbCentrosCostosProrrateos parametros = this.BusquedaParametrosObtenerValor<CtbCentrosCostosProrrateos>();
                if (parametros.BusquedaParametros)
                {
                    this.txtCentrosCostosProrrateo.Text = parametros.CentroCostoProrrateo;
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CtbCentrosCostosProrrateos parametros = this.BusquedaParametrosObtenerValor<CtbCentrosCostosProrrateos>();
            this.CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CentrosCostosProrrateosAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CtbCentrosCostosProrrateos centroCostoProrrateo = this.MisCentrosCostos[indiceColeccion];
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdCentroCostoProrrateo", centroCostoProrrateo.IdCentroCostoProrrateo);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CentrosCostosProrrateosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CentrosCostosProrrateosConsultar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                consultar.Visible = this.ValidarPermiso("CentrosCostosProrrateosConsultar.aspx");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                modificar.Visible = this.ValidarPermiso("CentrosCostosProrrateosModificar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisCentrosCostos.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CtbMonedas parametros = this.BusquedaParametrosObtenerValor<CtbMonedas>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CtbMonedas>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisCentrosCostos;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisCentrosCostos = this.OrdenarGrillaDatos<CtbCentrosCostosProrrateos>(this.MisCentrosCostos, e);
            this.gvDatos.DataSource = this.MisCentrosCostos;
            this.gvDatos.DataBind();
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstado.SelectedValue = ((int)EstadosTodos.Todos).ToString();
        }

        private void CargarLista(CtbCentrosCostosProrrateos pConceptosContables)
        {
            pConceptosContables.CentroCostoProrrateo = this.txtCentrosCostosProrrateo.Text.Trim();
            pConceptosContables.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pConceptosContables.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CtbCentrosCostosProrrateos>(pConceptosContables);
            this.MisCentrosCostos = ContabilidadF.CentrosCostosProrrateosObtenerListarFiltro(pConceptosContables);
            this.gvDatos.DataSource = this.MisCentrosCostos;
            this.gvDatos.PageIndex = pConceptosContables.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}