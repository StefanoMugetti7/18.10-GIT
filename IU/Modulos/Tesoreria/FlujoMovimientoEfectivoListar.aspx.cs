using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Tesorerias.Entidades;
using System.Collections;
using Comunes.Entidades;
using Generales.Entidades;
using Tesorerias;
using Generales.FachadaNegocio;
using Seguridad.FachadaNegocio;
using Reportes.FachadaNegocio;

namespace IU.Modulos.Tesoreria
{
    public partial class FlujoMovimientoEfectivoListar : PaginaSegura
    {
        private DataTable MisMovimientos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "FlujoMovimientoEfectivoListarMisMovimientos"]; }
            set { Session[this.MiSessionPagina + "FlujoMovimientoEfectivoListarMisMovimientos"] = value; }
        }

        public bool ComprobantePlantillaCajas
        {
            get
            {
                if (Session[this.MiSessionPagina + "PaginaFlujoEfectivoComprobantePlantillaCajas"] != null)
                    return (bool)Session[this.MiSessionPagina + "PaginaFlujoEfectivoComprobantePlantillaCajas"];
                else
                {
                    return (bool)(Session[this.MiSessionPagina + "PaginaFlujoEfectivoComprobantePlantillaCajas"] = false);
                }
            }
            set { Session[this.MiSessionPagina + "PaginaFlujoEfectivoComprobantePlantillaCajas"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            
            if (!this.IsPostBack)
            {
                TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ComprobantePlantillaCajas);

                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtFechaDesde, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtFechaHasta, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDescripcion, this.btnBuscar);
                //this.btnAgregarMovimiento.Visible = this.ValidarPermiso("CajasMovimientosAgregar.aspx");
                this.CargarCombos();

                TESCajasMovimientos parametros = this.BusquedaParametrosObtenerValor<TESCajasMovimientos>();
                if (parametros.BusquedaParametros)
                {
                    this.txtDescripcion.Text = parametros.Descripcion.ToString();
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlFilial.SelectedValue = parametros.IdFilial == 0 ? String.Empty : parametros.IdFilial.ToString();

                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TESCajasMovimientos parametros = this.BusquedaParametrosObtenerValor<TESCajasMovimientos>();
            CargarLista(parametros);
        }

        //protected void btnAgregarMovimiento_Click(object sender, EventArgs e)
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosAgregar.aspx"), true);
        //}

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);

            string origen = ((GridView)sender).DataKeys[index].Values["Origen"].ToString();
            int id = Convert.ToInt32(((GridView)sender).DataKeys[index].Values["NroMovimiento"].ToString());

            if (origen == "Caja")
            {
                TESCajasMovimientos movimiento = new TESCajasMovimientos();
                movimiento.IdCajaMovimiento = id;

                this.MisParametrosUrl = new Hashtable();
                this.MisParametrosUrl.Add("IdCajaMovimiento", movimiento.IdCajaMovimiento);

                if (e.CommandName == Gestion.Consultar.ToString())
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosConsultar.aspx"), true);
                }
                else if (e.CommandName == Gestion.Anular.ToString())
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosAnular.aspx"), true);
                }
                if (e.CommandName == Gestion.Impresion.ToString())
                {
                    int idComprobante = movimiento.IdCajaMovimiento;
                    if (ComprobantePlantillaCajas)
                    {
                        byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.TESCajasMovimientos, "TESCajasMovimientos", movimiento, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                        ExportPDF.ExportarPDF(pdf, this.UpdatePanel1, string.Concat("Comprobante_", idComprobante.ToString().PadLeft(10, '0')), this.UsuarioActivo);
                    }
                    else
                    {
                        Objeto refTipoOperacion = TesoreriasF.CajasObtenerMovimientoPendienteConfirmacion(movimiento);
                        if (Enum.IsDefined(typeof(EnumTGEComprobantes), refTipoOperacion.GetType().Name))
                        {
                            Object comprobante = Enum.Parse(typeof(EnumTGEComprobantes), refTipoOperacion.GetType().Name);
                            byte[] pdf = ReportesF.ExportPDFGenerarReportePDF((EnumTGEComprobantes)comprobante, "TESCajasMovimientos", refTipoOperacion, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                            ExportPDF.ExportarPDF(pdf, this.UpdatePanel1, string.Concat("Prestamo_", idComprobante.ToString().PadLeft(10, '0')), this.UsuarioActivo);
                        }
                    }
                }
            }
            else if(origen == "Tesoreria")
            {
                this.MostrarMensaje("ErrorURLNoValida", true);
            }

            int IdAfiliado = Convert.ToInt32(((GridView)sender).DataKeys[index].Values["IdAfiliado"].ToString());
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView fila = (DataRowView)e.Row.DataItem;

                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");
                ImageButton ibtnImprimir = (ImageButton)e.Row.FindControl("btnImprimir");

                int index = Convert.ToInt32(e.Row.RowIndex);
                string origen = ((GridView)sender).DataKeys[index].Values["Origen"].ToString();

                if(origen == "Caja")
                {
                    ibtnConsultar.Visible = this.ValidarPermiso("CajasMovimientosConsultar.aspx");
                    ibtnAnular.Visible = this.ValidarPermiso("CajasMovimientosAnular.aspx");
                    ibtnAnular.Visible = this.ValidarPermiso("CajasMovimientosAnular.aspx");
                }
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TESCajasMovimientos parametros = this.BusquedaParametrosObtenerValor<TESCajasMovimientos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TESCajasMovimientos>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisMovimientos;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //this.MisMovimientos = this.OrdenarGrillaDatos<VTAFacturas>(this.MisFacturas, e);
            //this.gvDatos.DataSource = this.MisMovimientos;
            //this.gvDatos.DataBind();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisMovimientos;
            this.gvDatos.DataBind();
            ExportData exportData = new ExportData();
            Dictionary<string, string> exportColumns = new Dictionary<string, string>();
            exportColumns.Add("Fecha", "Fecha");
            exportColumns.Add("Descripcion", "Detalle");
            exportColumns.Add("Importe", "Importe");
            exportColumns.Add("SaldoActual", "Saldo");
            exportData.ExportExcel(this, this.MisMovimientos, true, "Flujo de Efectivo", "Flujo de Efectivo", exportColumns);
        }

        private void CargarCombos()
        {
            this.ddlFilial.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFilial.DataValueField = "IdFilial";
            this.ddlFilial.DataTextField = "Filial";
            this.ddlFilial.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlFilial.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
        }

        private void CargarLista(TESCajasMovimientos pMovimiento)
        {
            pMovimiento.Descripcion = this.txtDescripcion.Text;
            pMovimiento.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pMovimiento.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pMovimiento.IdFilial = this.ddlFilial.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlFilial.SelectedValue);

            pMovimiento.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<TESCajasMovimientos>(pMovimiento);
            this.MisMovimientos = TesoreriasF.CajasMovimientosObtenerGrillaFlujoEfectivo(pMovimiento);
            this.gvDatos.DataSource = this.MisMovimientos;
            this.gvDatos.PageIndex = pMovimiento.IndiceColeccion;
            this.gvDatos.DataBind();

            if (this.MisMovimientos.Rows.Count > 0)
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;
        }
    }
}
