using Comunes.Entidades;
using Facturas;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.TGE
{
    public partial class TiposOperacionesComprobantesListar : PaginaSegura
    {
        private List<TGETiposOperacionesTiposFacturas> MiTiposOperacionesComprobantes
        {
            get { return (List<TGETiposOperacionesTiposFacturas>)Session[this.MiSessionPagina + "TiposOperacionesComprobantesListarMiTiposOperacionesComprobantes"]; }
            set { Session[this.MiSessionPagina + "TiposOperacionesComprobantesListarMiTiposOperacionesComprobantes"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.CargarCombos();
                TGETiposOperacionesTiposFacturas parametros = this.BusquedaParametrosObtenerValor<TGETiposOperacionesTiposFacturas>();
                int idTipoOpe = new ListaParametros(this.MiSessionPagina).ObtenerValor("IdTipoOperacion");
                if (idTipoOpe > 0)
                {
                    parametros.IdTipoOperacion = idTipoOpe;
                    parametros.BusquedaParametros = true;
                }
                if (parametros.BusquedaParametros)
                {
                    this.ddlTipoOperacionOC.SelectedValue = parametros.IdTipoOperacion == 0 ? string.Empty : parametros.IdTipoOperacion.ToString();
                    this.ddlTipoFactura.SelectedValue = parametros.IdTipoFactura == 0 ? string.Empty : parametros.IdTipoFactura.ToString();
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado == -1 ? string.Empty : parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);

                }
            }
        }
        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEstado, this.ObtenerMensajeSistema("Todos"));
            this.ddlEstado.SelectedValue = 1.ToString();//IDActivo

            this.ddlTipoFactura.DataSource = FacturasF.TiposFacturasActivosPorIdTipoFactura(new TGETiposFacturas());
            this.ddlTipoFactura.DataValueField = "IdTipoFactura";
            this.ddlTipoFactura.DataTextField = "Descripcion";
            this.ddlTipoFactura.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFactura, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            TGETiposOperaciones operaciones = new TGETiposOperaciones();
            operaciones.TipoFuncionalidad.IdTipoFuncionalidad = this.paginaActual.IdTipoFuncionalidad;
            operaciones.Estado.IdEstado = (int)Estados.Activo;
            //MisTiposOperaciones = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(operaciones);
            this.ddlTipoOperacionOC.DataSource = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(operaciones);
            this.ddlTipoOperacionOC.DataTextField = "TipoOperacion";
            this.ddlTipoOperacionOC.DataValueField = "IdTipoOperacion";
            this.ddlTipoOperacionOC.DataBind();
            if (ddlTipoOperacionOC.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoOperacionOC, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TGETiposOperacionesTiposFacturas parametros = this.BusquedaParametrosObtenerValor<TGETiposOperacionesTiposFacturas>();
            this.CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposOperacionesComprobantesAgregar.aspx"), true);
        }

        #region Grilla
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TGETiposOperacionesTiposFacturas TiposOperacionesTiposFacturas = this.MiTiposOperacionesComprobantes.FirstOrDefault(x=>x.IdTipoOperacionTipoFactura==indiceColeccion);

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdTipoOperacionTipoFactura", TiposOperacionesTiposFacturas.IdTipoOperacionTipoFactura);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposOperacionesComprobantesModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposOperacionesComprobantesConsultar.aspx"), true);
            }

        }


        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TGETiposOperacionesTiposFacturas TiposOperacionesTiposFacturas = (TGETiposOperacionesTiposFacturas)e.Row.DataItem;
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                modificar.Visible = this.ValidarPermiso("FilialesAgregar.aspx");
                consultar.Visible = this.ValidarPermiso("FilialesConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiTiposOperacionesComprobantes.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TGETiposOperacionesTiposFacturas parametros = this.BusquedaParametrosObtenerValor<TGETiposOperacionesTiposFacturas>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TGETiposOperacionesTiposFacturas>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            AyudaProgramacion.CargarGrillaListas<TGETiposOperacionesTiposFacturas>(this.MiTiposOperacionesComprobantes, false, this.gvDatos, true);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MiTiposOperacionesComprobantes = this.OrdenarGrillaDatos<TGETiposOperacionesTiposFacturas>(this.MiTiposOperacionesComprobantes, e);
            AyudaProgramacion.CargarGrillaListas<TGETiposOperacionesTiposFacturas>(this.MiTiposOperacionesComprobantes, false, this.gvDatos, true);
        }
        #endregion

        private void CargarLista(TGETiposOperacionesTiposFacturas pParametro)
        {
            pParametro.IdTipoOperacion = this.ddlTipoOperacionOC.SelectedValue==string.Empty ? 0 : Convert.ToInt32(this.ddlTipoOperacionOC.SelectedValue);
            pParametro.IdTipoFactura = this.ddlTipoFactura.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoFactura.SelectedValue);
            pParametro.Estado.IdEstado = this.ddlEstado.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlEstado.SelectedValue);

            pParametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<TGETiposOperacionesTiposFacturas>(pParametro);
            this.MiTiposOperacionesComprobantes = TGEGeneralesF.TiposOperacionesTiposFacturasObtenerListaFiltro(pParametro);
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            this.gvDatos.DataSource = this.MiTiposOperacionesComprobantes;
            this.gvDatos.DataBind();
            this.UpdatePanel1.Update();
            AyudaProgramacion.CargarGrillaListas<TGETiposOperacionesTiposFacturas>(this.MiTiposOperacionesComprobantes, false, this.gvDatos, true);
        }
    }
}
