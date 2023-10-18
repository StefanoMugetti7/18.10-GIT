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
using Generales.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Reportes.Entidades;
using Reportes.FachadaNegocio;

namespace IU.Modulos.Ahorros
{
    public partial class CuentasMovimientosListar : PaginaAfiliados
    {
        private AhoCuentas MiCuenta
        {
            get { return (AhoCuentas)Session[this.MiSessionPagina + "CuentasListarMiCuenta"]; }
            set { Session[this.MiSessionPagina + "CuentasListarMiCuenta"] = value; }
        }

        private DataTable MisCuentasMovimientos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "CuentasListarMisCuentasMovimientos"]; }
            set { Session[this.MiSessionPagina + "CuentasListarMisCuentasMovimientos"] = value; }
        }

        private DataTable MisCuentasMovimientosPendientes
        {
            get { return (DataTable)Session[this.MiSessionPagina + "CuentasListarMisCuentasMovimientosPendientes"]; }
            set { Session[this.MiSessionPagina + "CuentasListarMisCuentasMovimientosPendientes"] = value; }
        }

        private DataTable MisCuentasMovimientosExcel
        {
            get { return (DataTable)Session[this.MiSessionPagina + "CuentasListarMisCuentasMovimientosExcel"]; }
            set { Session[this.MiSessionPagina + "CuentasListarMisCuentasMovimientosExcel"] = value; }
        }

        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdCuenta"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasListar.aspx"), true);

                this.MiCuenta = new AhoCuentas();
                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdCuenta"]);
                this.MiCuenta.IdCuenta = parametro;

                this.MiCuenta = AhorroF.CuentasObtenerDatosCompletos(this.MiCuenta);

                this.txtFechaDesde.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                this.txtFechaHasta.Text = DateTime.Now.ToShortDateString();

                this.CargarLista(this.MiCuenta);
                this.CargarTextBox(this.MiCuenta);
            }
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Impresion.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            AhoCuentasMovimientos movimiento = new AhoCuentasMovimientos();
            movimiento.IdCuentaMovimiento = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdCuentaMovimiento"].ToString());
            //movimiento = AhorroF.MovimientosObtenerDatosCompletos(movimiento);
            byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.AhoCuentasMovimientos, "AhorroCuentasMovimientos", movimiento, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            ExportPDF.ExportarPDF(pdf, this, "AhorroCuentasMovimientos", this.UsuarioActivo);
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                DataRowView dr = (DataRowView)e.Row.DataItem;
                if (Convert.ToInt32(dr["TipoOperacionTipoMovimientoIdTipoMovimiento"]) == (int)EnumTGETiposMovimientos.Debito)
                {
                    e.Row.Cells[4].ForeColor = System.Drawing.Color.Red;
                }
                if (Convert.ToInt32(dr["SaldoActual"]) < 0)
                    e.Row.Cells[5].ForeColor = System.Drawing.Color.Red;

                ImageButton ibtnImprimir = (ImageButton)e.Row.FindControl("btnImprimir");
                ibtnImprimir.Visible = true;
                //switch (movimiento.TipoOperacion.IdTipoOperacion)
                //{
                //    case (int)EnumTGETiposOperaciones.AhorroDepositos:
                //    case (int)EnumTGETiposOperaciones.AhorroExtracciones:
                //    case (int)EnumTGETiposOperaciones.DepositoEfectivo:
                //    case (int)EnumTGETiposOperaciones.ExtraccionEfectivo:
                //    case (int)EnumTGETiposOperaciones.ExtraccionCheque:
                //    case (int)EnumTGETiposOperaciones.DepositoPlazoFijo:
                //    case (int)EnumTGETiposOperaciones.ExtraccionPlazoFijo:
                //        ImageButton ibtnImprimir = (ImageButton)e.Row.FindControl("btnImprimir");
                //        ibtnImprimir.Visible = true;
                //        break;
                //    default:
                //        break;
                //}
            }
            //if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    int cellCount = e.Row.Cells.Count;
            //    e.Row.Cells.Clear();
            //    TableCell tableCell = new TableCell();
            //    tableCell.ColumnSpan = cellCount;
            //    tableCell.HorizontalAlign = HorizontalAlign.Right;
            //    tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisCuentasMovimientos.Count);
            //    e.Row.Cells.Add(tableCell);
            //}
        }

        protected void gvMovimientosPendientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Impresion.ToString()
                || e.CommandName == Gestion.Anular.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            AhoCuentasMovimientos movimiento = new AhoCuentasMovimientos();
            movimiento.IdCuentaMovimiento = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdCuentaMovimiento"].ToString());
            //movimiento = AhorroF.MovimientosObtenerDatosCompletos(movimiento);
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdCuentaMovimiento", movimiento.IdCuentaMovimiento);

            if (e.CommandName == Gestion.Anular.ToString())
            {
                movimiento = AhorroF.MovimientosObtenerDatosCompletos(movimiento);

                if (AhorroF.MovimientosAnular(movimiento))
                {
                    ImageButton ibtnAnular = (ImageButton)this.gvMovimientosPendientes.Rows[index].FindControl("btnAnular");
                    ibtnAnular.Visible = false;
                    this.MostrarMensaje(movimiento.CodigoMensaje, false);
                }
                else
                    this.MostrarMensaje(movimiento.CodigoMensaje, true, movimiento.CodigoMensajeArgs);

            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.AhoCuentasMovimientos, "AhorroCuentasMovimientos", movimiento, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this, "AhorroCuentasMovimientos", this.UsuarioActivo);
            }
        }

        protected void gvMovimientosPendientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                DataRowView dr = (DataRowView)e.Row.DataItem;
                //if (Convert.ToInt32(dr["EstadoIdEstado"]) == (int)EstadosOrdenesCobro.Activo)
                if (Convert.ToInt32(dr["TipoOperacionTipoMovimientoIdTipoMovimiento"]) == (int)EnumTGETiposMovimientos.Debito)
                {
                    //e.Row.Cells[4].ForeColor = System.Drawing.Color.Red;
                    //if (movimiento.SaldoActual < 0)
                    //    e.Row.Cells[5].ForeColor = System.Drawing.Color.Red;
                }

                // Muestro la opción de Anular
                if (Convert.ToInt32(dr["EstadoIdEstado"]) == (int)EstadosAhorrosCuentasMovimientos.PendienteConfirmacion
                    && Convert.ToInt32(dr["FilialIdFilial"]) == this.UsuarioActivo.FilialPredeterminada.IdFilial)
                {
                    ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");
                    
                    if (Convert.ToInt32(dr["TipoOperacionIdTipoOperacion"])==(int)EnumTGETiposOperaciones.AhorroDepositos)
                        ibtnAnular.Visible = this.ValidarPermiso("CuentasMovimientosDepositar.aspx");
                    else if (Convert.ToInt32(dr["TipoOperacionIdTipoOperacion"]) == (int)EnumTGETiposOperaciones.AhorroExtracciones)
                        ibtnAnular.Visible = this.ValidarPermiso("CuentasMovimientosExtraer.aspx");

                    string funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarAnular"));
                    ibtnAnular.Attributes.Add("OnClick", funcion);
                }

                // Muestro la opción de Imprimir
                switch (Convert.ToInt32(dr["TipoOperacionIdTipoOperacion"]))
                {
                    case (int)EnumTGETiposOperaciones.AhorroDepositos:
                    case (int)EnumTGETiposOperaciones.AhorroExtracciones:
                    case (int)EnumTGETiposOperaciones.DepositoEfectivo:
                    case (int)EnumTGETiposOperaciones.ExtraccionEfectivo:
                    case (int)EnumTGETiposOperaciones.ExtraccionCheque:
                    case (int)EnumTGETiposOperaciones.DepositoPlazoFijo:
                    case (int)EnumTGETiposOperaciones.ExtraccionPlazoFijo:
                        ImageButton ibtnImprimir = (ImageButton)e.Row.FindControl("btnImprimir");
                        ibtnImprimir.Visible = true;
                        break;
                    default:
                        break;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisCuentasMovimientosPendientes.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //AhoCuentasMovimientos parametros = this.BusquedaParametrosObtenerValor<AhoCuentasMovimientos>();
            //parametros.IndiceColeccion = e.NewPageIndex;
            //this.BusquedaParametrosGuardarValor<AhoCuentasMovimientos>(parametros);
            //DataView aux = new DataView(this.MisCuentasMovimientos);
            ////aux.Sort = "EstadoIdEstado = " + ((int)EstadosAhorrosCuentasMovimientos.Confirmado).ToString();
            //this.gvDatos.PageIndex = e.NewPageIndex;
            //this.gvDatos.DataSource = aux.ToTable();
            //this.gvDatos.DataBind();
            AhoCuentas parametros = this.BusquedaParametrosObtenerValor<AhoCuentas>();
            gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;

            CargarLista(parametros);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisCuentasMovimientos = this.OrdenarGrillaDatos<DataTable>(this.MisCuentasMovimientos, e);
            this.gvDatos.DataSource = this.MisCuentasMovimientos;
            this.gvDatos.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.CargarLista(this.MiCuenta);
        }

        protected void btnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            RepReportes reporte = new RepReportes();
            RepParametros param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdCuenta";
            param.ValorParametro = this.MiCuenta.IdCuenta.ToString();
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
            param.Parametro = "FechaDesde";
            param.ValorParametro = Convert.ToDateTime(this.txtFechaDesde.Text);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
            param.Parametro = "FechaHasta";
            param.ValorParametro = Convert.ToDateTime(this.txtFechaHasta.Text);
            reporte.Parametros.Add(param);
        
            TGEPlantillas plantilla = new TGEPlantillas();
            plantilla.Codigo = "AhorroCuentas";
            plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);
            reporte.StoredProcedure = plantilla.NombreSP;
            DataSet ds = ReportesF.ReportesObtenerDatos(reporte);
            byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(plantilla, ds, "IdCuenta", AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            ExportPDF.ExportarPDF(pdf, this, "AhorroCuentas", this.UsuarioActivo);
            // this.ctrPopUpComprobantes.CargarReporte(reporte, EnumTGEComprobantes.AhoCuentasDetalleMovimientos);
            //AhoCuentas filtro = new AhoCuentas();
            //filtro.IdCuenta = this.MiCuenta.IdCuenta;
            //filtro.FechaDesde = Convert.ToDateTime(this.txtFechaDesde.Text);
            //filtro.FechaHasta = Convert.ToDateTime(this.txtFechaHasta.Text);


            //byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.AhoCuentasMovimientos, "AhorroCuentasMovimientos", filtro, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            //ExportPDF.ExportarPDF(pdf, this, "AhorroCuentasMovimientos", this.UsuarioActivo);

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasListar.aspx"), true);
        }

        private void CargarLista(AhoCuentas pCuentas)
        {
            pCuentas.IdCuenta = MiCuenta.IdCuenta;
            pCuentas.FechaDesde = Convert.ToDateTime(this.txtFechaDesde.Text);
            pCuentas.FechaHasta = Convert.ToDateTime(this.txtFechaHasta.Text);
            pCuentas.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(UsuarioActivo.PageSize);
            gvDatos.PageSize = pCuentas.PageSize;

            this.MisCuentasMovimientos = this.ObtenerMovimientosConfirmados(pCuentas);
            this.gvDatos.DataSource = this.MisCuentasMovimientos;
            this.gvDatos.VirtualItemCount = MisCuentasMovimientos.Rows.Count > 0 ? Convert.ToInt32(MisCuentasMovimientos.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + gvDatos.VirtualItemCount.ToString();
            this.gvDatos.DataBind();

            if (this.gvDatos.Rows.Count > 0)
            {
                btnExportarExcel.Visible = true;
            }
            else
            {
                btnExportarExcel.Visible = false;
            }

            this.MisCuentasMovimientosPendientes = this.ObtenerMovimientosPendientes(pCuentas);
            this.gvMovimientosPendientes.DataSource = this.MisCuentasMovimientosPendientes;
            this.gvMovimientosPendientes.DataBind();


            this.gvChequesRechazados.DataSource = this.ObtenerMovimientosRechazados(pCuentas);
            this.gvChequesRechazados.DataBind();

        
        }
        private DataTable ObtenerMovimientosConfirmados(AhoCuentas pCuentas)
        {
            DataTable data = AhorroF.MovimientosObtenerListaPorCuentaDT(pCuentas);
            DataView aux = new DataView(data);
            aux.RowFilter = "EstadoIdEstado = " + ((int)EstadosAhorrosCuentasMovimientos.Confirmado).ToString();
            return aux.ToTable();
        }

        private DataTable ObtenerMovimientosRechazados(AhoCuentas pCuentas)
        {
            DataTable data = AhorroF.MovimientosObtenerListaPorCuentaDT(pCuentas);
            DataView aux = new DataView(data);
            aux.RowFilter = "EstadoIdEstado = " + ((int)EstadosAhorrosCuentasMovimientos.Rechazado).ToString();
            return aux.ToTable();
        }
        private DataTable ObtenerMovimientosPendientes(AhoCuentas pCuentas)
        {
            DataTable data = AhorroF.MovimientosObtenerListaPorCuentaDT(pCuentas);
            DataView aux = new DataView(data);
            aux.RowFilter = "EstadoIdEstado = " + ((int)EstadosAhorrosCuentasMovimientos.PendienteConfirmacion).ToString() + "OR EstadoIdEstado = " + ((int)EstadosAhorrosCuentasMovimientos.PendienteAcreditacionBancos).ToString();
            return aux.ToTable();
        }

        private void CargarTextBox(AhoCuentas pCuentas)
        {
            this.txtTipoCuenta.Text = pCuentas.CuentaTipo.CuentaTipo;
            this.txtNumeroCuenta.Text = pCuentas.NumeroCuenta.ToString().Trim();
            this.txtSaldoActual.Text = pCuentas.SaldoActual.ToString().Trim();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            AhoCuentas cuenta= new AhoCuentas();
            cuenta.IdCuenta = this.MiCuenta.IdCuenta;
            cuenta.FechaDesde =  Convert.ToDateTime(this.txtFechaDesde.Text);
            cuenta.FechaHasta =  Convert.ToDateTime(this.txtFechaHasta.Text);
            cuenta.BusquedaParametros = true;
            cuenta.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            cuenta.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(UsuarioActivo.PageSize);
            cuenta.PageSize = int.MaxValue;
            gvDatos.PageIndex = 0; // cuenta.PageIndex;

            this.MisCuentasMovimientosExcel = this.ObtenerMovimientosConfirmados(cuenta);
            //GridViewExportUtil.Export("Datos.xls", this.gvDatos);
            ExportData exportData = new ExportData();
            Dictionary<string, string> exportColumns = new Dictionary<string, string>();
            exportColumns.Add("FechaConfirmacion", "Fecha");
            if (this.MisCuentasMovimientosExcel.Columns.Contains("NumeroSocioIntegrante"))
                exportColumns.Add("NumeroSocioIntegrante", "Numero de Socio Integrante");
            exportColumns.Add("TipoOperacionTipoOperacion", "Tipo de Operacion");
            exportColumns.Add("ImporteSigno", "Importe");
            exportColumns.Add("SaldoActual", "Saldo Actual");
            
            if(this.MisCuentasMovimientosExcel.Columns.Contains("DetalleIntegrante"))
            exportColumns.Add("DetalleIntegrante", "Detalle Integrante");
                        
            exportData.ExportExcel(this, this.MisCuentasMovimientosExcel, true, "Cuentas Movimientos", "Cuentas Movimientos", exportColumns);
        }
    }
}
