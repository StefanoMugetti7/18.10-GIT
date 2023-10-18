using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using Cargos.Entidades;
using System.Collections.Generic;
using Cargos;

namespace IU.Modulos.Cargos
{
    public partial class TiposCargosListar : PaginaSegura
    {
        private List<CarTiposCargos> MisTiposCargos
        {
            get { return (List<CarTiposCargos>)Session[this.MiSessionPagina + "CargosListarMiTiposCargos"]; }
            set { Session[this.MiSessionPagina + "CargosListarMiTiposCargos"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtTipoCargo, this.btnBuscar);
                this.btnAgregar.Visible = this.ValidarPermiso("TiposCargosAgregar.aspx");
                this.CargarCombos();
                CarTiposCargos parametros = this.BusquedaParametrosObtenerValor<CarTiposCargos>();
                if (parametros.BusquedaParametros)
                {
                    this.txtTipoCargo.Text = parametros.TipoCargo;
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CarTiposCargos parametros = this.BusquedaParametrosObtenerValor<CarTiposCargos>();
            this.CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/TiposCargosAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CarTiposCargos tipoCargo = this.MisTiposCargos[indiceColeccion];
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", e.CommandName);
            this.MisParametrosUrl.Add("IdCargo", tipoCargo.IdTipoCargo);
            if (e.CommandName == Gestion.Modificar.ToString())
            {
                string url = "~/Modulos/Cargos/TiposCargosModificar.aspx";
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(url), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                string url = "~/Modulos/Cargos/TiposCargosConsultar.aspx";
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(url), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisTiposCargos.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CarTiposCargos parametros = this.BusquedaParametrosObtenerValor<CarTiposCargos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CarTiposCargos>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisTiposCargos;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisTiposCargos = this.OrdenarGrillaDatos<CarTiposCargos>(this.MisTiposCargos, e);
            this.gvDatos.DataSource = this.MisTiposCargos;
            this.gvDatos.DataBind();
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();
        }

        private void CargarLista(CarTiposCargos pTipoCargo)
        {
            pTipoCargo.TipoCargo = this.txtTipoCargo.Text.Trim();
            pTipoCargo.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pTipoCargo.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CarTiposCargos>(pTipoCargo);
            this.MisTiposCargos = CargosF.TiposCargosObtenerListaFiltro(pTipoCargo);
            this.gvDatos.DataSource = this.MisTiposCargos;
            this.gvDatos.PageIndex = pTipoCargo.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}
