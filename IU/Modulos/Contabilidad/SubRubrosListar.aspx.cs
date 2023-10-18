using Comunes.Entidades;
using Contabilidad;
using Contabilidad.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace IU.Modulos.Contabilidad
{
    public partial class SubRubrosListar : PaginaSegura
    {
        private List<CtbSubRubros> MisSubRubros
        {
            get { return (List<CtbSubRubros>)Session[this.MiSessionPagina + "SubRubrosListar"]; }
            set { Session[this.MiSessionPagina + "SubRubrosListar"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("SubRubrosAgregar.aspx");
                this.CargarCombos();
                CtbSubRubros parametros = this.BusquedaParametrosObtenerValor<CtbSubRubros>();
                if (parametros.BusquedaParametros)
                {
                    this.txtSubRubro.Text = parametros.SubRubro;
                    this.txtCodigoSubRubro.Text = parametros.CodigoSubRubro;
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CtbSubRubros parametros = this.BusquedaParametrosObtenerValor<CtbSubRubros>();
            this.CargarLista(parametros);
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/SubRubrosAgregar.aspx"), true);
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CtbSubRubros subRubro = this.MisSubRubros[indiceColeccion];
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdSubRubro", subRubro.IdSubRubro);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/SubRubrosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/SubRubrosConsultar.aspx"), true);
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton movimientos = (ImageButton)e.Row.FindControl("btnMovimientos");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisSubRubros.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CtbSubRubros parametros = this.BusquedaParametrosObtenerValor<CtbSubRubros>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CtbSubRubros>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisSubRubros;
            this.gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisSubRubros = this.OrdenarGrillaDatos<CtbSubRubros>(this.MisSubRubros, e);
            this.gvDatos.DataSource = this.MisSubRubros;
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
        }
        private void CargarLista(CtbSubRubros pSubRubros)
        {
            pSubRubros.SubRubro = this.txtSubRubro.Text.Trim();
            pSubRubros.CodigoSubRubro = this.txtCodigoSubRubro.Text.Trim();
            pSubRubros.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pSubRubros.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CtbSubRubros>(pSubRubros);
            this.MisSubRubros = ContabilidadF.SubRubrosObtenerListar(pSubRubros);
            this.gvDatos.DataSource = this.MisSubRubros;
            this.gvDatos.PageIndex = pSubRubros.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}