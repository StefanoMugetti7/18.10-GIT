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
    public partial class RubrosListar : PaginaSegura
    {
        private List<CtbRubros> MisRubros
        {
            get { return (List<CtbRubros>)Session[this.MiSessionPagina + "RubrosListar"]; }
            set { Session[this.MiSessionPagina + "RubrosListar"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("RubrosAgregar.aspx");
                this.CargarCombos();
                CtbRubros parametros = this.BusquedaParametrosObtenerValor<CtbRubros>();
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtRubro, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoRubro, this.btnBuscar);
                if (parametros.BusquedaParametros)
                {
                    this.txtRubro.Text = parametros.Rubro;
                    this.txtCodigoRubro.Text = parametros.CodigoRubro;
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CtbRubros parametros = this.BusquedaParametrosObtenerValor<CtbRubros>();
            this.CargarLista(parametros);
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/RubrosAgregar.aspx"), true);
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
            CtbRubros rubro = this.MisRubros[indiceColeccion];
            this.MisParametrosUrl = new Hashtable
            {
                { "IdRubro", rubro.IdRubro }
            };

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/RubrosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/RubrosConsultar.aspx"), true);
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
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisRubros.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CtbRubros parametros = this.BusquedaParametrosObtenerValor<CtbRubros>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CtbRubros>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisRubros;
            this.gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisRubros = this.OrdenarGrillaDatos<CtbRubros>(this.MisRubros, e);
            this.gvDatos.DataSource = this.MisRubros;
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
        private void CargarLista(CtbRubros pRubro)
        {
            pRubro.Rubro = this.txtRubro.Text.Trim();
            pRubro.CodigoRubro = this.txtCodigoRubro.Text.Trim();
            pRubro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pRubro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CtbRubros>(pRubro);
            this.MisRubros = ContabilidadF.RubrosObtenerListar(pRubro);
            this.gvDatos.DataSource = this.MisRubros;
            this.gvDatos.PageIndex = pRubro.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}