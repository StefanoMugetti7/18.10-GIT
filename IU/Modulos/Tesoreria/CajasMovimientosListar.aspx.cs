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
using RestSharp.Extensions;

namespace IU.Modulos.Tesoreria
{
    public partial class CajasMovimientosListar : PaginaSegura
    {
        private DataTable MisMovimientos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "CajasMovimientosListarMisMovimientos"]; }
            set { Session[this.MiSessionPagina + "CajasMovimientosListarMisMovimientos"] = value; }
        }
        public bool ComprobantePlantillaCajas
        {
            get
            {
                if (Session[this.MiSessionPagina + "PaginaCajasComprobantePlantillaCajas"] != null)
                    return (bool)Session[this.MiSessionPagina + "PaginaCajasComprobantePlantillaCajas"];
                else
                {
                    return (bool)(Session[this.MiSessionPagina + "PaginaCajasComprobantePlantillaCajas"] = false);
                }
            }
            set { Session[this.MiSessionPagina + "PaginaCajasComprobantePlantillaCajas"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            gvDatos.PageSizeEvent += GvDatos_PageSizeEvent;
            if (!this.IsPostBack)
            {
                TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ComprobantePlantillaCajas);
                ComprobantePlantillaCajas = !string.IsNullOrEmpty(valor.ParametroValor) ? Convert.ToBoolean(valor.ParametroValor) : false;


                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtFechaDesde, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtFechaHasta, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumero, this.btnBuscar);
                this.btnAgregarMovimiento.Visible = this.ValidarPermiso("CajasMovimientosAgregar.aspx");
                this.CargarCombos();

                TESCajasMovimientos parametros = this.BusquedaParametrosObtenerValor<TESCajasMovimientos>();
                if (parametros.BusquedaParametros)
                {
                    this.txtNumero.Text = parametros.IdCajaMovimiento.ToString();
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlFilial.SelectedValue = parametros.IdFilial == 0 ? String.Empty : parametros.IdFilial.ToString();
                    this.ddlTipoOperacion.SelectedValue = parametros.TipoOperacion.IdTipoOperacion == 0 ? String.Empty : parametros.TipoOperacion.IdTipoOperacion.ToString();
                    this.txtDescripcion.Text = parametros.Descripcion.HasValue() ? parametros.Descripcion : string.Empty; 
                    // this.txt
                    this.CargarLista(parametros);
                }
            }
        }

        private void GvDatos_PageSizeEvent(int pageSize)
        {
            TESCajasMovimientos parametros = this.BusquedaParametrosObtenerValor<TESCajasMovimientos>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            CargarLista(parametros);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TESCajasMovimientos parametros = this.BusquedaParametrosObtenerValor<TESCajasMovimientos>();
            CargarLista(parametros);
        }

        protected void btnAgregarMovimiento_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);

            TESCajasMovimientos movimiento = new TESCajasMovimientos();
            movimiento.IdCajaMovimiento = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            int IdTipoOperacion = Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdTipoOperacion")).Value);
            int IdRefTipoOperacion = Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdRefTipoOperacion")).Value);

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
            movimiento.IdRefTipoOperacion = IdRefTipoOperacion;
            movimiento.TipoOperacion.IdTipoOperacion = IdTipoOperacion;
            TGEPlantillas miPlantilla = TGEGeneralesF.PlantillasObtenerDatosPorTipoOperacion(movimiento);
            miPlantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(miPlantilla);
            if (e.CommandName == Gestion.Impresion.ToString())
            {
                int idComprobante = movimiento.IdCajaMovimiento;
                if (ComprobantePlantillaCajas)
                {
   
                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.TESCajasMovimientos, "TESCajasMovimientos", movimiento, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.UpdatePanel1, string.Concat("Comprobante_", idComprobante.ToString().PadLeft(10, '0')), this.UsuarioActivo);
                }
                else if (miPlantilla.HtmlPlantilla.Trim().Length > 0)
                {
                    //Object comprobante = Enum.Parse(typeof(EnumTGEComprobantes), movimiento.GetType().Name);

                    //if (comprobante != null)
                    //{
                        //TGEPlantillas plantilla = new TGEPlantillas();
                        //plantilla = TGEGeneralesF.PlantillasObtenerDatosPorComprobante((EnumTGEComprobantes)comprobante);

                       
                        Objeto MiTipoOperacion = AyudaProgramacion.ObtenerIdTipoOperacion(movimiento);


                        byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.SinComprobante, miPlantilla.Codigo, MiTipoOperacion, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                        ExportPDF.ExportarPDF(pdf, this.UpdatePanel1, string.Concat("Comprobante_", movimiento.IdCajaMovimiento.ToString().PadLeft(10, '0')), this.UsuarioActivo);
                    //}
                }
                else
                {


                    Objeto refTipoOperacion = TesoreriasF.CajasObtenerMovimientoPendienteConfirmacion(movimiento);
                    if (Enum.IsDefined(typeof(EnumTGEComprobantes), refTipoOperacion.GetType().Name))
                    {
                        Object comprobante = Enum.Parse(typeof(EnumTGEComprobantes), refTipoOperacion.GetType().Name);
                        byte[] pdf = ReportesF.ExportPDFGenerarReportePDF((EnumTGEComprobantes)comprobante, "TESCajasMovimientos", movimiento, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                        ExportPDF.ExportarPDF(pdf, this.UpdatePanel1, string.Concat("Prestamo_", idComprobante.ToString().PadLeft(10, '0')), this.UsuarioActivo);


                    }
                }
                
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView fila = (DataRowView)e.Row.DataItem;
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ibtnConsultar.Visible = this.ValidarPermiso("CajasMovimientosConsultar.aspx");
                ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");
                if (Convert.ToInt32( fila["IdEstado"]) == (int)Estados.Activo)
                    ibtnAnular.Visible = this.ValidarPermiso("CajasMovimientosAnular.aspx");

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblImporteTotal = (Label)e.Row.FindControl("lblImporteTotal");
                lblImporteTotal.Text = this.MisMovimientos.AsEnumerable().Sum(x => x.Field<decimal>("Importe")).ToString("C2");

                Label lblCantidadRegistros = (Label)e.Row.FindControl("lblCantidadRegistros");
                lblCantidadRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisMovimientos.Rows.Count);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TESCajasMovimientos parametros = this.BusquedaParametrosObtenerValor<TESCajasMovimientos>();

            gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;

            CargarLista(parametros);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisMovimientos = this.OrdenarGrillaDatos<TESCajasMovimientos>(this.MisMovimientos, e);
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

        private void CargarCombos()
        {
            this.ddlFilial.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFilial.DataValueField = "IdFilial";
            this.ddlFilial.DataTextField = "Filial";
            this.ddlFilial.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlFilial.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();

            this.ddlTipoOperacion.DataSource = TGEGeneralesF.TiposOperacionesSeleccionarComboCajasMovimientos();
            this.ddlTipoOperacion.DataValueField = "IdTipoOperacion";
            this.ddlTipoOperacion.DataTextField = "TipoOperacion";
            this.ddlTipoOperacion.DataBind();
            if (this.ddlTipoOperacion.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoOperacion, this.ObtenerMensajeSistema("SeleccioneOpcion"));

        }

        private void CargarLista(TESCajasMovimientos pMovimiento)
        {
            pMovimiento.IdCajaMovimiento = this.txtNumero.Text == string.Empty ? 0 : Convert.ToInt32(this.txtNumero.Text);
            pMovimiento.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pMovimiento.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pMovimiento.IdFilial = this.ddlFilial.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlFilial.SelectedValue);
            pMovimiento.TipoOperacion.IdTipoOperacion = this.ddlTipoOperacion.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlTipoOperacion.SelectedValue);
            pMovimiento.Descripcion = this.txtDescripcion.Text == string.Empty ? null : this.txtDescripcion.Text;

            pMovimiento.BusquedaParametros = true;
            pMovimiento.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(UsuarioActivo.PageSize);
            gvDatos.PageSize = pMovimiento.PageSize;
            gvDatos.PageIndex = pMovimiento.PageIndex;

            this.BusquedaParametrosGuardarValor<TESCajasMovimientos>(pMovimiento);
            this.MisMovimientos = TesoreriasF.CajasMovimientosObtenerGrilla(pMovimiento);
            this.gvDatos.DataSource = this.MisMovimientos;
            this.gvDatos.VirtualItemCount = MisMovimientos.Rows.Count > 0 ? Convert.ToInt32(MisMovimientos.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + gvDatos.VirtualItemCount.ToString();
            this.gvDatos.DataBind();

            if (this.MisMovimientos.Rows.Count > 0)
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;
        }
    }
}
