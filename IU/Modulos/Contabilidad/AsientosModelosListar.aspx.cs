using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using Contabilidad;
using System.Collections;

namespace IU.Modulos.Contabilidad
{
    public partial class AsientosModelosListar : PaginaSegura
    {
        private List<CtbAsientosModelos> MisAsientosModelos
        {
            get { return (List<CtbAsientosModelos>)Session[this.MiSessionPagina + "AsientosModelos"]; }
            set { Session[this.MiSessionPagina + "AsientosModelos"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDetalle, this.btnBuscar);
                this.btnAgregar.Visible = this.ValidarPermiso("AsientosModelosAgregar.aspx");
                this.CargarCombos();
                CtbAsientosModelos parametros = this.BusquedaParametrosObtenerValor<CtbAsientosModelos>();
                if (parametros.BusquedaParametros)
                {
                    this.txtDetalle.Text = parametros.Detalle;
                    this.ddlTipoAsiento.SelectedValue = parametros.TipoAsiento.IdTipoAsiento.ToString();
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.ddlEjercicioContable.SelectedValue = parametros.EjercicioContable.IdEjercicioContable.Value.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CtbAsientosModelos parametros = this.BusquedaParametrosObtenerValor<CtbAsientosModelos>();
            this.CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosModelosAgregar.aspx"), true);
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
            CtbAsientosModelos asientoModelo = this.MisAsientosModelos[indiceColeccion];
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAsientoModelo", asientoModelo.IdAsientoModelo);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosModelosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosModelosConsultar.aspx"), true);
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
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisAsientosModelos.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CtbAsientosModelos parametros = this.BusquedaParametrosObtenerValor<CtbAsientosModelos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CtbAsientosModelos>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisAsientosModelos;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisAsientosModelos = this.OrdenarGrillaDatos<CtbAsientosModelos>(this.MisAsientosModelos, e);
            this.gvDatos.DataSource = this.MisAsientosModelos;
            this.gvDatos.DataBind();
        }

        private void CargarCombos()
        {
            CtbEjerciciosContables filtroAsiento = new CtbEjerciciosContables();
            filtroAsiento.Estado.IdEstado = (int)EstadosTodos.Todos;
            List<CtbEjerciciosContables> lista = ContabilidadF.EjerciciosContablesObtenerListaFiltro(filtroAsiento);
            this.ddlEjercicioContable.DataSource = lista;
            this.ddlEjercicioContable.DataValueField = "IdEjercicioContable";
            this.ddlEjercicioContable.DataTextField = "Descripcion";
            this.ddlEjercicioContable.DataBind();

            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlEstado.SelectedValue = "-1";
            this.ddlTipoAsiento.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EnumTiposAsientos));
            this.ddlTipoAsiento.DataValueField = "IdEstado";
            this.ddlTipoAsiento.DataTextField = "Descripcion";
            this.ddlTipoAsiento.DataBind();
            this.ddlTipoAsiento.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlTipoAsiento.SelectedValue = "-1";
        }

        private void CargarLista(CtbAsientosModelos pAseintoModelo)
        {
            pAseintoModelo.Detalle = this.txtDetalle.Text.Trim();
            pAseintoModelo.TipoAsiento.IdTipoAsiento = Convert.ToInt32(this.ddlTipoAsiento.SelectedValue);
            pAseintoModelo.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pAseintoModelo.EjercicioContable.IdEjercicioContable = Convert.ToInt32(this.ddlEjercicioContable.SelectedValue);
            pAseintoModelo.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CtbAsientosModelos>(pAseintoModelo);
            this.MisAsientosModelos = ContabilidadF.AsientosModelosObtenerListaFiltro(pAseintoModelo);
            this.gvDatos.DataSource = this.MisAsientosModelos;
            this.gvDatos.PageIndex = pAseintoModelo.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}