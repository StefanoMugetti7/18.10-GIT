using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using System.Collections;
using Comunes.Entidades;
using Contabilidad;
using Generales.FachadaNegocio;

namespace IU.Modulos.Contabilidad
{
    public partial class CapitulosListar : PaginaSegura
    {
        private List<CtbCapitulos> MisCapitulos
        {
            get { return (List<CtbCapitulos>)Session[this.MiSessionPagina + "CapitulosListar"]; }
            set { Session[this.MiSessionPagina + "CapitulosListar"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("CapitulosAgregar.aspx");
                this.CargarCombos();
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoCapitulo, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCapitulo, this.btnBuscar);
                CtbCapitulos parametros = this.BusquedaParametrosObtenerValor<CtbCapitulos>();
                if (parametros.BusquedaParametros)
                {
                    this.txtCapitulo.Text = parametros.Capitulo;
                    this.txtCodigoCapitulo.Text = parametros.CodigoCapitulo;
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CtbCapitulos parametros = this.BusquedaParametrosObtenerValor<CtbCapitulos>();
            this.CargarLista(parametros);
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CapitulosAgregar.aspx"), true);
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
            CtbCapitulos capitulo = this.MisCapitulos[indiceColeccion];
            this.MisParametrosUrl = new Hashtable
            {
                { "IdCapitulo", capitulo.IdCapitulo }
            };

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CapitulosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CapitulosConsultar.aspx"), true);
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CtbCapitulos capitulo = (CtbCapitulos)e.Row.DataItem;

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
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisCapitulos.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CtbCapitulos parametros = this.BusquedaParametrosObtenerValor<CtbCapitulos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CtbCapitulos>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisCapitulos;
            this.gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisCapitulos = this.OrdenarGrillaDatos<CtbCapitulos>(this.MisCapitulos, e);
            this.gvDatos.DataSource = this.MisCapitulos;
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
        private void CargarLista(CtbCapitulos pCapitulos)
        {
            pCapitulos.Capitulo = this.txtCapitulo.Text.Trim();
            pCapitulos.CodigoCapitulo = this.txtCodigoCapitulo.Text.Trim();
            pCapitulos.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pCapitulos.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CtbCapitulos>(pCapitulos);
            this.MisCapitulos = ContabilidadF.CapitulosObtenerListar(pCapitulos);
            this.gvDatos.DataSource = this.MisCapitulos;
            this.gvDatos.PageIndex = pCapitulos.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}