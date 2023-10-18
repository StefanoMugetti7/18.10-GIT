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
    public partial class MonedasListar : PaginaSegura
    {
        private List<CtbMonedas> MisMonedas
        {
            get { return (List<CtbMonedas>)Session[this.MiSessionPagina + "MonedasListar"]; }
            set { Session[this.MiSessionPagina + "MonedasListar"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("MonedasAgregar.aspx");
                this.CargarCombos();
                CtbMonedas parametros = this.BusquedaParametrosObtenerValor<CtbMonedas>();
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoMoneda, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtMoneda, this.btnBuscar);
                if (parametros.BusquedaParametros)
                {
                    this.txtMoneda.Text = parametros.Moneda;
                    this.txtCodigoMoneda.Text = parametros.CodigoMoneda;
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CtbMonedas parametros = this.BusquedaParametrosObtenerValor<CtbMonedas>();
            this.CargarLista(parametros);
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/MonedasAgregar.aspx"), true);
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
            CtbMonedas moneda = this.MisMonedas[indiceColeccion];
            this.MisParametrosUrl = new Hashtable
            {
                { "IdMoneda", moneda.IdMoneda }
            };

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/MonedasModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/MonedasConsultar.aspx"), true);
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
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisMonedas.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CtbMonedas parametros = this.BusquedaParametrosObtenerValor<CtbMonedas>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CtbMonedas>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisMonedas;
            this.gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisMonedas = this.OrdenarGrillaDatos<CtbMonedas>(this.MisMonedas, e);
            this.gvDatos.DataSource = this.MisMonedas;
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
        private void CargarLista(CtbMonedas pMoneda)
        {
            pMoneda.Moneda = this.txtMoneda.Text.Trim();
            pMoneda.CodigoMoneda = this.txtCodigoMoneda.Text.Trim();
            pMoneda.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pMoneda.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CtbMonedas>(pMoneda);
            this.MisMonedas = ContabilidadF.MonedasObtenerListar(pMoneda);
            this.gvDatos.DataSource = this.MisMonedas;
            this.gvDatos.PageIndex = pMoneda.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}