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
using Ahorros.Entidades;
using System.Collections.Generic;
using Ahorros;
using Comunes.Entidades;
using Generales.FachadaNegocio;

namespace IU.Modulos.Ahorros
{
    public partial class PlazosListar : PaginaSegura
    {
        private List<AhoPlazos> MisPlazos
        {
            get { return (List<AhoPlazos>)Session[this.MiSessionPagina + "PlazosListarMisPlazos"]; }
            set { Session[this.MiSessionPagina + "PlazosListarMisPlazos"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("PlazosAgregar.aspx");
                this.CargarCombos();
                AhoPlazos parametros = this.BusquedaParametrosObtenerValor<AhoPlazos>();
                if (parametros.BusquedaParametros)
                {
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            AhoPlazos parametros = this.BusquedaParametrosObtenerValor<AhoPlazos>();
            this.CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosAgregar.aspx"), true);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                   || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            AhoPlazos plazo = this.MisPlazos[indiceColeccion];
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", e.CommandName);
            this.MisParametrosUrl.Add("IdPlazo", plazo.IdPlazos);
            //string parametros = string.Format("?IdPlazo={0}", plazo.IdPlazos);
            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosConsultar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AhoPlazos plazo = (AhoPlazos)e.Row.DataItem;
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                if (plazo.FechaDesde.Date >= DateTime.Now.Date)
                    ibtnModificar.Visible = false;

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisPlazos.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AhoPlazos parametros = this.BusquedaParametrosObtenerValor<AhoPlazos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<AhoPlazos>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisPlazos;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisPlazos = this.OrdenarGrillaDatos<AhoPlazos>(this.MisPlazos, e);
            this.gvDatos.DataSource = this.MisPlazos;
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

        private void CargarLista(AhoPlazos pPlazos)
        {
            pPlazos.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<AhoPlazos>(pPlazos);
            this.MisPlazos = AhorroF.PlazosObtenerListaFiltro(pPlazos);
            this.gvDatos.DataSource = this.MisPlazos;
            this.gvDatos.PageIndex = pPlazos.IndiceColeccion;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);
        }
    }
}
