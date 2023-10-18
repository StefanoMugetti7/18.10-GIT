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
using CuentasPagar.Entidades;
using System.Data;

namespace IU.Modulos.Contabilidad
{
    public partial class BienesUsosListar : PaginaSegura
    {
        private DataTable MisBienesUsos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "MisBienesUsos"]; }
            set { Session[this.MiSessionPagina + "MisBienesUsos"] = value; }
        }

        private CapSolicitudPagoDetalles MiSolicitudPagoDetalle
        {
            get { return (CapSolicitudPagoDetalles)Session[this.MiSessionPagina + "MiSolicitudPagoDetalle"]; }
            set { Session[this.MiSessionPagina + "MiSolicitudPagoDetalle"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("BienesUsosAgregar.aspx");
                //this.txtFechaActivacionDesde.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                //this.txtFechaActivacionHasta.Text = DateTime.Now.ToShortDateString();
                this.CargarCombos();
                CtbBienesUsos parametros = this.BusquedaParametrosObtenerValor<CtbBienesUsos>();
                if (parametros.BusquedaParametros)
                {
                    this.txtDescripcion.Text = parametros.Descripcion;
                    this.txtFechaActivacionDesde.Text = parametros.FechaActivacionDesde.HasValue ? parametros.FechaActivacionDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaActivacionHasta.Text = parametros.FechaActivacionHasta.HasValue ? parametros.FechaActivacionHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlEstado.SelectedValue  =parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CtbBienesUsos parametros = this.BusquedaParametrosObtenerValor<CtbBienesUsos>();
            this.CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/BienesUsosAgregar.aspx"), true);
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
            int idBienUso = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdBienUso", idBienUso);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/BienesUsosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/BienesUsosConsultar.aspx"), true);
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
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisBienesUsos.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CtbBienesUsos parametros = this.BusquedaParametrosObtenerValor<CtbBienesUsos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CtbBienesUsos>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisBienesUsos;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisBienesUsos = this.OrdenarGrillaDatos<CtbBienesUsos>(this.MisBienesUsos, e);
            this.gvDatos.DataSource = this.MisBienesUsos;
            this.gvDatos.DataBind();
        }

        protected void buscarSolicitudPago_SolicitudPagoDetalleBuscarSeleccionar(CapSolicitudPagoDetalles pSolicitudPagoDetalle)
        {
            this.MiSolicitudPagoDetalle = pSolicitudPagoDetalle;
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosBienesUsos));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstado.SelectedValue = ((int)EstadosTodos.Todos).ToString();
        }

        private void CargarLista(CtbBienesUsos pBienesUsos)
        {
            pBienesUsos.Descripcion = this.txtDescripcion.Text.Trim();
            pBienesUsos.FechaActivacionDesde = this.txtFechaActivacionDesde.Text==string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaActivacionDesde.Text);
            pBienesUsos.FechaActivacionHasta = this.txtFechaActivacionHasta.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaActivacionHasta.Text);
            pBienesUsos.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pBienesUsos.SolicitudPagoDetalles = this.MiSolicitudPagoDetalle;
            pBienesUsos.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CtbBienesUsos>(pBienesUsos);
            this.MisBienesUsos = ContabilidadF.BienesUsosObtenerListaGrilla(pBienesUsos);
            this.gvDatos.DataSource = this.MisBienesUsos;
            this.gvDatos.PageIndex = pBienesUsos.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}