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
using Cobros.Entidades;
using System.Collections.Generic;
using Generales.FachadaNegocio;
using Cobros;
using Comunes.Entidades;
using Generales.Entidades;

namespace IU.Modulos.Cobros
{
    public partial class OrdenesCobrosListar : PaginaSegura
    {
        private List<CobOrdenesCobros> MisOrdenesCobros
        {
            get { return (List<CobOrdenesCobros>)Session[this.MiSessionPagina + "OrdenesCobrosListarMisOrdenesCobros"]; }
            set { Session[this.MiSessionPagina + "OrdenesCobrosListarMisOrdenesCobros"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.CargarCombos();
                CobOrdenesCobros parametros = this.BusquedaParametrosObtenerValor<CobOrdenesCobros>();
                if (parametros.BusquedaParametros)
                {
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlFiliales.SelectedValue = parametros.Filial.IdFilial.ToString();
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CobOrdenesCobros parametros = this.BusquedaParametrosObtenerValor<CobOrdenesCobros>();
            this.CargarLista(parametros);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CobOrdenesCobros ordenCobro = this.MisOrdenesCobros[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdOrdenCobro", ordenCobro.IdOrdenCobro);

            if (e.CommandName == Gestion.Anular.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosAnular.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                this.ctrPopUpComprobantes.CargarReporte(ordenCobro, EnumTGEComprobantes.CobOrdenesCobros);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CobOrdenesCobros bancoCuenta = (CobOrdenesCobros)e.Row.DataItem;

                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                consultar.Visible = this.ValidarPermiso("OrdenesCobrosConsultar.aspx");

                ImageButton anular = (ImageButton)e.Row.FindControl("btnAnular");
                if (bancoCuenta.Estado.IdEstado==(int)EstadosOrdenesCobro.Activo)
                    anular.Visible = this.ValidarPermiso("OrdenesCobrosAnular.aspx");

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisOrdenesCobros.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CobOrdenesCobros parametros = this.BusquedaParametrosObtenerValor<CobOrdenesCobros>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CobOrdenesCobros>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisOrdenesCobros;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisOrdenesCobros = this.OrdenarGrillaDatos<CobOrdenesCobros>(this.MisOrdenesCobros, e);
            this.gvDatos.DataSource = this.MisOrdenesCobros;
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

            this.ddlFiliales.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFiliales.DataValueField = "IdFilial";
            this.ddlFiliales.DataTextField = "Filial";
            this.ddlFiliales.DataBind();
            this.ddlFiliales.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
        }

        private void CargarLista(CobOrdenesCobros pParametro)
        {
            pParametro.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pParametro.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pParametro.Filial.IdFilial = this.ddlFiliales.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFiliales.SelectedValue);
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pParametro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.OrdenesCobrosVarios;
            pParametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CobOrdenesCobros>(pParametro);
            this.MisOrdenesCobros = CobrosF.OrdenesCobrosListaFiltro(pParametro);
            this.gvDatos.DataSource = this.MisOrdenesCobros;
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}