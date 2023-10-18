using Afiliados.Entidades;
using Afiliados;
using Compras;
using Compras.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Reportes.FachadaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Compras
{
    public partial class StockMovimientosListar : PaginaSegura
    {
        private List<CmpStockMovimientos> MisMovimientos
        {
            get { return (List<CmpStockMovimientos>)Session[this.MiSessionPagina + "StockMovimientosListarMisMovimientos"]; }
            set { Session[this.MiSessionPagina + "StockMovimientosListarMisMovimientos"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroMovimiento, this.btnBuscar);
                this.btnAgregar.Visible = this.ValidarPermiso("StockMovimientosAgregar.aspx");
                this.CargarCombos();

                CmpStockMovimientos parametros = this.BusquedaParametrosObtenerValor<CmpStockMovimientos>();
                if (parametros.BusquedaParametros)
                {
                    this.txtNumeroMovimiento.Text = parametros.IdStockMovimiento == 0 ? String.Empty : parametros.IdStockMovimiento.ToString();
                    //this.ddlTipoOperacion.SelectedValue = parametros.TipoOperacion.IdTipoOperacion.ToString();
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CmpStockMovimientos parametros = this.BusquedaParametrosObtenerValor<CmpStockMovimientos>();
            CargarLista(parametros);
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/StockMovimientosAgregar.aspx"), true);
        }
        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosStock));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstados.SelectedValue = ((int)EstadosTodos.Todos).ToString();
            //this.ddlFilial.DataSource = this.UsuarioActivo.Filiales;
            //this.ddlFilial.DataValueField = "IdFilial";
            //this.ddlFilial.DataTextField = "Filial";
            //this.ddlFilial.DataBind();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            //this.ddlFilial.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
        }
        #region "Grilla"
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Anular.ToString()
                || e.CommandName == Gestion.ConfirmarAgregar.ToString()
                || e.CommandName == Gestion.Impresion.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CmpStockMovimientos pStockMov = this.MisMovimientos[indiceColeccion];
            this.MisParametrosUrl = new Hashtable
            {
                { "IdStockMovimiento", pStockMov.IdStockMovimiento }
            };

            if (e.CommandName == Gestion.ConfirmarAgregar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/StockMovimientosConfirmar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Anular.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/StockMovimientosAnular.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/StockMovimientosConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                //this.ctrPopUpComprobantes.CargarReporte(pStockMov, EnumTGEComprobantes.StockMovimiento);
                pStockMov = ComprasF.StockMovimientosObtenerDatosCompletos(pStockMov);
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.StockMovimiento, "CMPStockMovimientos", pStockMov, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, UpdatePanel1, "CMPStockMovimientos", this.UsuarioActivo);
                //byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(aux, plantilla.Codigo, afiliadoImprimir, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                //ExportPDF.ExportarPDF(pdf, this.UpdatePanel1, string.Concat("Stock_", pStockMov.IdStockMovimiento.ToString().PadLeft(10, '0')), this.UsuarioActivo);
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CmpStockMovimientos stockMov = (CmpStockMovimientos)e.Row.DataItem;

                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton anular = (ImageButton)e.Row.FindControl("btnAnular");
                ImageButton autorizar = (ImageButton)e.Row.FindControl("btnAutorizar");
                ImageButton imprimir = (ImageButton)e.Row.FindControl("btnImprimir");
                ibtnConsultar.Visible = this.ValidarPermiso("StockMovimientosConsultar.aspx");

                switch (stockMov.Estado.IdEstado)
                {
                    case (int)EstadosStock.Activo:
                        anular.Visible = this.ValidarPermiso("StockMovimientosAnular.aspx");
                        imprimir.Visible = true;
                        break;
                    case (int)EstadosStock.PendienteConfirmacion:
                        if (stockMov.IdFilialDestino == this.UsuarioActivo.FilialPredeterminada.IdFilial)
                            autorizar.Visible = this.ValidarPermiso("StockMovimientosConfirmar.aspx");
                        if (stockMov.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.StockTransferencia)
                            anular.Visible = this.ValidarPermiso("StockMovimientosAnular.aspx");
                        imprimir.Visible = false;
                        break;
                    case (int)EstadosStock.Confirmado:
                        anular.Visible = this.ValidarPermiso("StockMovimientosAnular.aspx");
                        imprimir.Visible = true;
                        break;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblCantidadRegistros = (Label)e.Row.FindControl("lblCantidadRegistros");
                lblCantidadRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisMovimientos.Count);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CmpStockMovimientos parametros = this.BusquedaParametrosObtenerValor<CmpStockMovimientos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CmpStockMovimientos>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisMovimientos;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisMovimientos = this.OrdenarGrillaDatos<CmpStockMovimientos>(this.MisMovimientos, e);
            this.gvDatos.DataSource = this.MisMovimientos;
            this.gvDatos.DataBind();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisMovimientos;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }
        #endregion

        private void CargarLista(CmpStockMovimientos pStockMov)
        {
            //VER PARAMETROS QUE RECIBE EL STORE
            pStockMov.IdStockMovimiento = this.txtNumeroMovimiento.Text == String.Empty ? 0 : Convert.ToInt32(this.txtNumeroMovimiento.Text);

            pStockMov.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pStockMov.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pStockMov.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pStockMov.BusquedaParametros = true;
            pStockMov.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.BusquedaParametrosGuardarValor<CmpStockMovimientos>(pStockMov);
            this.MisMovimientos = ComprasF.StockMovimientosListaFiltro(pStockMov);
            this.gvDatos.DataSource = this.MisMovimientos;
            this.gvDatos.PageIndex = pStockMov.IndiceColeccion;
            this.gvDatos.DataBind();

            if (this.MisMovimientos.Count > 0)
                this.btnExportarExcel.Visible = true;
            else
                this.btnExportarExcel.Visible = false;
        }
    }
}