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
using Cargos;
using Cargos.Entidades;
using System.Collections.Generic;
using Facturas.Entidades;
using Reportes.Entidades;
using Generales.Entidades;
using System.Net.Mail;
using Comunes.LogicaNegocio;
using System.IO;
using Afiliados;
using Facturas;
using Comunes.Entidades;
using Seguridad.FachadaNegocio;
using Afiliados.Entidades;
using Afiliados.Entidades.Entidades;
using Reportes.FachadaNegocio;
using Generales.FachadaNegocio;
using Cobros.Entidades;

namespace IU.Modulos.Afiliados
{
    public partial class PosicionGlobal : PaginaAfiliados
    {
      
        private DataTable MiCuentaCorrienteCargos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "PosicionGlobalMiCuentaCorriente"]; }
            set { Session[this.MiSessionPagina + "PosicionGlobalMiCuentaCorriente"] = value; }
        }
        private DataTable MiCuentaCorrienteCargosDolar
        {
            get { return (DataTable)Session[this.MiSessionPagina + "PosicionGlobalMiCuentaCorrienteDolar"]; }
            set { Session[this.MiSessionPagina + "PosicionGlobalMiCuentaCorrienteDolar"] = value; }
        }


        private DataTable MiCuentaCorriente
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ClientesModificarDatosMiCuentaCorriente"]; }
            set { Session[this.MiSessionPagina + "ClientesModificarDatosMiCuentaCorriente"] = value; }
        }

        private DataTable MiCuentaCorrienteDolar
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ClientesModificarDatosMiCuentaCorrienteDolar"]; }
            set { Session[this.MiSessionPagina + "ClientesModificarDatosMiCuentaCorrienteDolar"] = value; }
        }

        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            if (!this.IsPostBack)
            {
                CargarCombos();
                   CarCuentasCorrientes cuenta = new CarCuentasCorrientes();
                cuenta.IdAfiliado = this.MiAfiliado.IdAfiliado;
                cuenta.Moneda.IdMoneda = (int)EnumTGEMonedas.PesosArgentinos;
                this.MiCuentaCorrienteCargos = CargosF.CuentasCorrientesObtenerDTGrilla(cuenta);
                if (this.MiCuentaCorrienteCargos.Rows.Count > 0)
                    btnExportarCargosExcel.Visible = true;
                else
                    btnExportarCargosExcel.Visible = false;

                this.gvDatosCargos.DataSource = this.MiCuentaCorrienteCargos;
                this.gvDatosCargos.DataBind();
                this.UpdatePanel1.Update();

                cuenta.Moneda.IdMoneda = (int)EnumTGEMonedas.DolarEEUU;
                this.MiCuentaCorrienteCargosDolar = CargosF.CuentasCorrientesObtenerDTGrilla(cuenta);
                if (this.MiCuentaCorrienteCargosDolar.Rows.Count > 0)
                {
                    btnExportarCargosExcelDolar.Visible = true;
                    tpCuentaCorrienteCargosDolar.Visible = true;
                }
                else
                {
                    btnExportarCargosExcelDolar.Visible = false;
                    tpCuentaCorrienteCargosDolar.Visible = false;
                }
                this.gvDatosCargosDolar.DataSource = this.MiCuentaCorrienteCargosDolar;
                this.gvDatosCargosDolar.DataBind();
                this.UpdatePanel1.Update();


                CuentasCorrientesFiltro cuentaFiltro = new CuentasCorrientesFiltro();
                cuentaFiltro.IdAfiliado = MiAfiliado.IdAfiliado;
                cuentaFiltro.IdMoneda = (int)EnumTGEMonedas.PesosArgentinos;
                this.MiCuentaCorriente = FacturasF.CuentasCorrientesSeleccionarPorClienteTable(cuentaFiltro);
                VTACuentasCorrientes parametrosCC = this.BusquedaParametrosObtenerValor<VTACuentasCorrientes>();
                this.gvDatos.PageIndex = parametrosCC.IndiceColeccion;
                this.gvDatos.DataSource = this.MiCuentaCorriente;
                this.gvDatos.DataBind();

                cuentaFiltro.IdMoneda = (int)EnumTGEMonedas.DolarEEUU;
                this.MiCuentaCorrienteDolar = FacturasF.CuentasCorrientesSeleccionarPorClienteTable(cuentaFiltro);
                this.gvDatosDolar.PageIndex = parametrosCC.IndiceColeccion;
                this.gvDatosDolar.DataSource = this.MiCuentaCorrienteDolar;
                this.gvDatosDolar.DataBind();

                this.upCtaCte.Update();
                this.upCtaCteDolar.Update();
                //AyudaProgramacion.CargarGrillaListas<VTACuentasCorrientes>(this.MiCuentaCorriente, false, this.gvDatos, true);
                if (this.MiCuentaCorriente.Rows.Count > 0)
                    btnExportarExcel.Visible = true;
                else
                    btnExportarExcel.Visible = false;
                if (this.MiCuentaCorrienteDolar.Rows.Count > 0)
                    btnExportarExcelDolar.Visible = true;
                else
                    btnExportarExcelDolar.Visible = false;

                this.btnAgregarComprobante.Visible = this.ValidarPermiso("FacturasAgregar.aspx");
                this.btnAgregarOC.Visible = this.ValidarPermiso("OrdenesCobrosFacturasAgregar.aspx");
                this.btnAgregarComprobanteDolar.Visible = this.ValidarPermiso("FacturasAgregar.aspx");
                this.btnAgregarOCDolar.Visible = this.ValidarPermiso("OrdenesCobrosFacturasAgregar.aspx");
                this.btnFiltrar_Click(this.btnBuscarCtaCte, EventArgs.Empty);

                AfiClientes parametros = this.BusquedaParametrosObtenerValor<AfiClientes>();
                if (parametros.BusquedaParametros)
                {
                    this.tcDatos.ActiveTabIndex = parametros.HashTransaccion;
                }
            }
            else {
                AfiClientes  parametros = this.BusquedaParametrosObtenerValor<AfiClientes>();
                parametros.HashTransaccion = this.tcDatos.ActiveTabIndex;
                parametros.BusquedaParametros = true;
                this.BusquedaParametrosGuardarValor<AfiClientes>(parametros);
            }
        }

        protected void gvDatosCargos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CarCuentasCorrientes parametros = this.BusquedaParametrosObtenerValor<CarCuentasCorrientes>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CarCuentasCorrientes>(parametros);
            this.gvDatosCargos.PageIndex = e.NewPageIndex;
            this.gvDatosCargos.DataSource = this.MiCuentaCorrienteCargos;
            this.gvDatosCargos.DataBind();
            this.UpdatePanel1.Update();

        }

        protected void gvDatosCargosDolar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CarCuentasCorrientes parametros = this.BusquedaParametrosObtenerValor<CarCuentasCorrientes>();
            parametros.HashTransaccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CarCuentasCorrientes>(parametros);
            this.gvDatosCargosDolar.PageIndex = e.NewPageIndex;
            this.gvDatosCargosDolar.DataSource = this.MiCuentaCorrienteCargosDolar;
            this.gvDatosCargosDolar.DataBind();
            this.UpdatePanel1.Update();

        }

        protected void btnExportarCargosExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatosCargos.AllowPaging = false;
            this.gvDatosCargos.DataSource = this.MiCuentaCorrienteCargos;
            this.gvDatosCargos.DataBind();
          //  GridViewExportUtil.Export("DatosCargos.xls", this.gvDatosCargos);
            ExportData exportData = new ExportData();
            exportData.ExportExcel(this.Page, this.MiCuentaCorrienteCargos);
        }

        protected void btnExportarCargosExcelDolar_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatosCargosDolar.AllowPaging = false;
            this.gvDatosCargosDolar.DataSource = this.MiCuentaCorrienteCargosDolar;
            this.gvDatosCargosDolar.DataBind();
          //  GridViewExportUtil.Export("DatosCargosDolar.xls", this.gvDatosCargosDolar);
            ExportData exportData = new ExportData();
            exportData.ExportExcel(this.Page, this.MiCuentaCorrienteCargosDolar);
        }

        private void CargarCombos()
        {
           
            RepReportes reporte = new RepReportes();
            reporte.StoredProcedure = "ReportesSoloDeuda";
            this.ddlTipoReporte.DataSource = ReportesF.ReportesObtenerDatos(reporte);
            this.ddlTipoReporte.DataValueField = "SoloDeuda";
            this.ddlTipoReporte.DataTextField = "Descripcion";
            this.ddlTipoReporte.DataBind();
            reporte = new RepReportes();
            reporte.StoredProcedure = "ReportesParametrosTiposFacturas";
            this.ddlTipoComprobante.DataSource = ReportesF.ReportesObtenerDatos(reporte);
            this.ddlTipoComprobante.DataValueField = "IdTipoFactura";
            this.ddlTipoComprobante.DataTextField = "Descripcion";
            this.ddlTipoComprobante.DataBind();

            reporte = new RepReportes();
            reporte.StoredProcedure = "ReportesSoloDeuda";
            this.ddlTipoReporteDolar.DataSource = ReportesF.ReportesObtenerDatos(reporte);
            this.ddlTipoReporteDolar.DataValueField = "SoloDeuda";
            this.ddlTipoReporteDolar.DataTextField = "Descripcion";
            this.ddlTipoReporteDolar.DataBind();
            reporte = new RepReportes();
            reporte.StoredProcedure = "ReportesParametrosTiposFacturas";
            this.ddlTipoComprobanteDolar.DataSource = ReportesF.ReportesObtenerDatos(reporte);
            this.ddlTipoComprobanteDolar.DataValueField = "IdTipoFactura";
            this.ddlTipoComprobanteDolar.DataTextField = "Descripcion";
            this.ddlTipoComprobanteDolar.DataBind();
        }

        #region Cuenta Corriente Grilla

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            VTACuentasCorrientes parametros = this.BusquedaParametrosObtenerValor<VTACuentasCorrientes>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<VTACuentasCorrientes>(parametros);
            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MiCuentaCorriente;
            this.gvDatos.DataBind();
            //AyudaProgramacion.CargarGrillaListas(this.MiCuentaCorriente, false, this.gvDatos, true);
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MiCuentaCorriente;
            this.gvDatos.DataBind();
           //GridViewExportUtil.Export("Datos.xls", this.gvDatos);
            ExportData exportData = new ExportData();
            exportData.ExportExcel(this.Page, this.MiCuentaCorriente);
        }

      

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            this.MiAfiliado.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            this.MiAfiliado.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            //this.MiCuentaCorriente = FacturasF.CuentasCorrientesSeleccionarPorCliente(this.MiAfiliado); ;
            //AyudaProgramacion.CargarGrillaListas<VTACuentasCorrientes>(this.MiCuentaCorriente, false, this.gvDatos, true);
            this.MiCuentaCorriente = FacturasF.CuentasCorrientesSeleccionarPorClienteTable(this.MiAfiliado);
            this.gvDatos.DataSource = this.MiCuentaCorriente;
            this.gvDatos.DataBind();
            this.upCtaCte.Update();

        }

        protected void btnAgregarComprobante_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturasAgregar.aspx"), true);
        }

        protected void btnAgregarOC_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosFacturasAgregar.aspx"), true);
        }

        protected void btnAgregarRemito_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/RemitosAgregar.aspx"), true);
        }

        protected void btnAgregarPresupuesto_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/PresupuestosAgregar.aspx"), true);
        }

        protected void btnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            RepReportes reporte = new RepReportes();
            RepParametros param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdAfiliado";
            param.ValorParametro = this.MiAfiliado.IdAfiliado;
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
            param.Parametro = "FechaDesde";
            param.ValorParametro = this.txtFechaDesde.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaDesde.Text);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
            param.Parametro = "FechaHasta";
            param.ValorParametro = this.txtFechaHasta.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaHasta.Text);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "SoloDeuda";
            param.ValorParametro = this.ddlTipoReporte.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlTipoReporte.SelectedValue);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdTipoFactura";
            param.ValorParametro = this.ddlTipoComprobante.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlTipoComprobante.SelectedValue);
            reporte.Parametros.Add(param);

            this.ctrPopUpComprobantes.CargarReporte(reporte, EnumTGEComprobantes.VTAResumenCuenta);
            this.upImprimir.Update();

        }

        protected void btnEnviarMail_Click(object sender, EventArgs e)
        {
            RepReportes reporte = new RepReportes();
            RepParametros param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdAfiliado";
            param.ValorParametro = this.MiAfiliado.IdAfiliado;
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
            param.Parametro = "FechaHasta";
            param.ValorParametro = this.txtFechaDesde.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(this.txtFechaDesde.Text);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "SoloDeuda";
            param.ValorParametro = this.ddlTipoReporte.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlTipoReporte.SelectedValue);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdTipoFactura";
            param.ValorParametro = this.ddlTipoComprobante.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlTipoComprobante.SelectedValue);
            reporte.Parametros.Add(param);

            MailMessage mail = new MailMessage();
            TGEArchivos archivo = new TGEArchivos();
            archivo.Archivo = AyudaProgramacionLN.StreamToByteArray(this.ctrPopUpComprobantes.GenerarReporteArchivo(reporte, EnumTGEComprobantes.VTAResumenCuenta));
            archivo.NombreArchivo = string.Concat(this.MiAfiliado.CUIL, ".pdf");

            mail.Attachments.Add(new Attachment(new MemoryStream(archivo.Archivo), archivo.NombreArchivo));
            if (AfiliadosF.AfiliadosArmarMailResumenCuenta(this.MiAfiliado, mail))
            {
                this.popUpMail.IniciarControl(mail, this.MiAfiliado);
            }
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);

            int IdTipoOperacion = Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdTipoOperacion")).Value);
            int IdRefTipoOperacion = Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdRefTipoOperacion")).Value);
            int IdEstado = Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdEstado")).Value);

            if (IdRefTipoOperacion == 0)
                return;

            this.MisParametrosUrl = new Hashtable();
            //Filtro para Obtener URL y NombreParametro
            Menues filtroMenu = new Menues();
            filtroMenu.IdTipoOperacion = IdTipoOperacion;
            //Control de Tipo de Menues (SOLO CONSULTA)
            if (e.CommandName == Gestion.Consultar.ToString())
            {
                filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Consulta;
            }
            else if (e.CommandName == Gestion.Anular.ToString())
            {
                if (IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas)
                {
                    if (IdEstado == (int)EstadosFacturas.Cobrada)
                        filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_AnularConfirmar;
                }
                else
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Anular;
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                if (IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas)
                {
                    CobOrdenesCobros ordenCobro = new CobOrdenesCobros();
                    ordenCobro.IdOrdenCobro = IdRefTipoOperacion;
                    ordenCobro.Estado.IdEstado = IdEstado;
                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CobOrdenesCobros, "OrdenesCobros", ordenCobro, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.upCtaCte, "OrdenesCobros", this.UsuarioActivo);
                }
                else {
                    VTAFacturas factura = new VTAFacturas();
                    factura.IdFactura = IdRefTipoOperacion;
                    factura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    factura = FacturasF.FacturasObtenerDatosCompletos(factura);
                    VTAFacturas facturaPdf = FacturasF.FacturasObtenerArchivo(factura);
                    List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
                    TGEArchivos archivo = new TGEArchivos();
                    archivo.Archivo = facturaPdf.FacturaPDF;
                    if (archivo.Archivo != null)
                        listaArchivos.Add(archivo);

                    VTARemitos remito = FacturasF.RemitosObtenerPorFactura(factura);
                    if (remito.IdRemito > 0)
                    {
                        archivo = new TGEArchivos();
                        remito.UsuarioLogueado = factura.UsuarioLogueado;
                        remito = FacturasF.RemitosObtenerArchivo(remito);
                        archivo.Archivo = remito.RemitoPDF;
                        if (archivo.Archivo != null)
                            listaArchivos.Add(archivo);
                    }
                    TGEEmpresas empresa = TGEGeneralesF.EmpresasSeleccionar();
                    string nombreArchivo = string.Concat(empresa.CUIT, "_", factura.TipoFactura.CodigoValor, "_", factura.PrefijoNumeroFactura, "_", factura.NumeroFactura, ".pdf");
                    ExportPDF.ConvertirArchivoEnPdf(this.upCtaCte, listaArchivos, nombreArchivo);
                }
            }
                //else
                //{
                //    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Anular;
                //}

                //Guardo Menu devuelto de la DB
                filtroMenu = SeguridadF.MenuesObtenerPorOperacionTipoMenu(filtroMenu);
            this.MisParametrosUrl.Add(filtroMenu.NombreParametro, IdRefTipoOperacion);
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
            //Si devuelve una URL Redirecciona si no muestra mensaje error
            if (filtroMenu.URL.Length != 0)
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(string.Concat("~/", filtroMenu.URL)), true);
            }
            else
            {
                this.MiAfiliado.CodigoMensaje = "ErrorURLNoValida";
                this.MostrarMensaje(this.MiAfiliado.CodigoMensaje, true);
            }

        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                //consultar.Visible = this.ValidarPermiso("LibroMayorListar.aspx");
                ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");
                ibtnAnular.Visible = ValidarPermiso("FacturasAnular.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

            }
        }
        #endregion

        #region Cuenta Corriente Grilla Dolar
        protected void btnExportarExcelDolar_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatosDolar.AllowPaging = false;
            this.gvDatosDolar.DataSource = this.MiCuentaCorrienteDolar;
            this.gvDatosDolar.DataBind();
           // GridViewExportUtil.Export("Datos.xls", this.gvDatosDolar);
            ExportData exportData = new ExportData();
            exportData.ExportExcel(this.Page, this.MiCuentaCorrienteDolar);
        }

        protected void btnFiltrarDolar_Click(object sender, EventArgs e)
        {

            //this.MiCuentaCorriente = FacturasF.CuentasCorrientesSeleccionarPorCliente(this.MiAfiliado); ;
            //AyudaProgramacion.CargarGrillaListas<VTACuentasCorrientes>(this.MiCuentaCorriente, false, this.gvDatos, true);
            CuentasCorrientesFiltro cuenta = new CuentasCorrientesFiltro();
            cuenta.FechaDesde = this.txtFechaDesdeDolar.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesdeDolar.Text);
            cuenta.FechaHasta = this.txtFechaHastaDolar.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHastaDolar.Text);
            cuenta.IdAfiliado = MiAfiliado.IdAfiliado;
            cuenta.IdMoneda = (int)EnumTGEMonedas.DolarEEUU;
            this.MiCuentaCorrienteDolar = FacturasF.CuentasCorrientesSeleccionarPorClienteTable(cuenta);
            this.gvDatosDolar.DataSource = this.MiCuentaCorrienteDolar;
            this.gvDatosDolar.DataBind();
            this.upCtaCteDolar.Update();

        }

        protected void btnAgregarComprobanteDolar_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturasAgregar.aspx"), true);
        }

        protected void btnAgregarOCDolar_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosFacturasAgregar.aspx"), true);
        }

        protected void btnAgregarRemitoDolar_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/RemitosAgregar.aspx"), true);
        }

        protected void btnAgregarPresupuestoDolar_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/PresupuestosAgregar.aspx"), true);
        }

        protected void btnImprimirDolar_Click(object sender, ImageClickEventArgs e)
        {
            RepReportes reporte = new RepReportes();
            RepParametros param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdAfiliado";
            param.ValorParametro = this.MiAfiliado.IdAfiliado;
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
            param.Parametro = "FechaDesde";
            param.ValorParametro = this.txtFechaDesdeDolar.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaDesdeDolar.Text);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
            param.Parametro = "FechaHasta";
            param.ValorParametro = this.txtFechaHastaDolar.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaHastaDolar.Text);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "SoloDeuda";
            param.ValorParametro = this.ddlTipoReporteDolar.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlTipoReporteDolar.SelectedValue);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdTipoFactura";
            param.ValorParametro = this.ddlTipoComprobanteDolar.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlTipoComprobanteDolar.SelectedValue);
            reporte.Parametros.Add(param);

            this.ctrPopUpComprobantes.CargarReporte(reporte, EnumTGEComprobantes.VTAResumenCuenta);
            this.upImprimir.Update();

        }

        protected void btnEnviarMailDolar_Click(object sender, EventArgs e)
        {
            RepReportes reporte = new RepReportes();
            RepParametros param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdAfiliado";
            param.ValorParametro = this.MiAfiliado.IdAfiliado;
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
            param.Parametro = "FechaHasta";
            param.ValorParametro = this.txtFechaDesdeDolar.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(this.txtFechaDesdeDolar.Text);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "SoloDeuda";
            param.ValorParametro = this.ddlTipoReporteDolar.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlTipoReporteDolar.SelectedValue);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdTipoFactura";
            param.ValorParametro = this.ddlTipoComprobanteDolar.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlTipoComprobanteDolar.SelectedValue);
            reporte.Parametros.Add(param);

            MailMessage mail = new MailMessage();
            TGEArchivos archivo = new TGEArchivos();
            archivo.Archivo = AyudaProgramacionLN.StreamToByteArray(this.ctrPopUpComprobantes.GenerarReporteArchivo(reporte, EnumTGEComprobantes.VTAResumenCuenta));
            archivo.NombreArchivo = string.Concat(this.MiAfiliado.CUIL, ".pdf");

            mail.Attachments.Add(new Attachment(new MemoryStream(archivo.Archivo), archivo.NombreArchivo));
            if (AfiliadosF.AfiliadosArmarMailResumenCuenta(this.MiAfiliado, mail))
            {
                this.popUpMail.IniciarControl(mail, this.MiAfiliado);
            }
        }

        protected void gvDatosDolar_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);

            int IdTipoOperacion = Convert.ToInt32(((HiddenField)this.gvDatosDolar.Rows[index].FindControl("hdfIdTipoOperacionDolar")).Value);
            int IdRefTipoOperacion = Convert.ToInt32(((HiddenField)this.gvDatosDolar.Rows[index].FindControl("hdfIdRefTipoOperacionDolar")).Value);

            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            parametros.HashTransaccion = tpCuentaCorrienteDolar.TabIndex;
            parametros.IdAfiliado = MiAfiliado.IdAfiliado;
            parametros.BusquedaParametros = true;

            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            //if (e.CommandName == Gestion.Impresion.ToString())
            //{
            //    VTAFacturas factura = new VTAFacturas();
            //    //factura.IdFactura = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdFactura"].ToString());

            //    factura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            //    factura = FacturasF.FacturasObtenerDatosCompletos(factura);
            //    VTAFacturas facturaPdf = FacturasF.FacturasObtenerArchivo(factura);
            //    List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
            //    TGEArchivos archivo = new TGEArchivos();
            //    archivo.Archivo = facturaPdf.FacturaPDF;
            //    if (archivo.Archivo != null)
            //        listaArchivos.Add(archivo);

            //    VTARemitos remito = FacturasF.RemitosObtenerPorFactura(factura);
            //    if (remito.IdRemito > 0)
            //    {
            //        archivo = new TGEArchivos();
            //        remito.UsuarioLogueado = factura.UsuarioLogueado;
            //        remito = FacturasF.RemitosObtenerArchivo(remito);
            //        archivo.Archivo = remito.RemitoPDF;
            //        if (archivo.Archivo != null)
            //            listaArchivos.Add(archivo);
            //    }
            //    TGEEmpresas empresa = TGEGeneralesF.EmpresasSeleccionar();
            //    string nombreArchivo = string.Concat(empresa.CUIT, "_", factura.TipoFactura.CodigoValor, "_", factura.PrefijoNumeroFactura, "_", factura.NumeroFactura, ".pdf");
            //    ExportPDF.ConvertirArchivoEnPdf(this.UpdatePanel1, listaArchivos, nombreArchivo);
            //}
            //else
            //{
            this.MisParametrosUrl = new Hashtable();
            //Filtro para Obtener URL y NombreParametro
            Menues filtroMenu = new Menues();
            filtroMenu.IdTipoOperacion = IdTipoOperacion;
            //Control de Tipo de Menues (SOLO CONSULTA)
            if (e.CommandName == Gestion.Consultar.ToString())
                filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Consulta;

            //Control de Tipo de Menues (Anular)
            if (e.CommandName == Gestion.Anular.ToString())
                filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Anular;

            //Guardo Menu devuelto de la DB
            filtroMenu = SeguridadF.MenuesObtenerPorOperacionTipoMenu(filtroMenu);
            this.MisParametrosUrl.Add(filtroMenu.NombreParametro, IdRefTipoOperacion);
            this.MisParametrosUrl.Add("hdfIdRefTipoOperacion", IdRefTipoOperacion);
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
            //Si devuelve una URL Redirecciona si no muestra mensaje error
            if (filtroMenu.URL.Length != 0)
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(string.Concat("~/", filtroMenu.URL)), true);
            }
            else
            {
                this.MiAfiliado.CodigoMensaje = "ErrorURLNoValida";
                this.MostrarMensaje(this.MiAfiliado.CodigoMensaje, true);
            }
            //}


        }

        protected void gvDatosDolar_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                //consultar.Visible = this.ValidarPermiso("LibroMayorListar.aspx");
                ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnularDolar");

                ibtnAnular.Visible = true;

                DataRowView dr = (DataRowView)e.Row.DataItem;

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

            }
        }

        protected void gvDatosDolar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            parametros.IndiceColeccion = e.NewPageIndex;
        
            parametros.IdAfiliado = MiAfiliado.IdAfiliado;
            parametros.BusquedaParametros = true;
            BusquedaParametrosGuardarValor<AfiAfiliados>(parametros);

            this.gvDatosDolar.PageIndex = e.NewPageIndex;
            this.gvDatosDolar.DataSource = this.MiCuentaCorrienteDolar;
            this.gvDatosDolar.DataBind();

            //VTAFacturas parametrosFacturas = this.BusquedaParametrosObtenerValor<VTAFacturas>();
            //parametros.IndiceColeccion = e.NewPageIndex;
            //this.BusquedaParametrosGuardarValor<VTAFacturas>(parametrosFacturas);

            //gvDatos.PageIndex = e.NewPageIndex;
            //gvDatos.DataSource = this.MisFacturas;
            //gvDatos.DataBind();

            //CobOrdenesCobros parametrosCob = this.BusquedaParametrosObtenerValor<CobOrdenesCobros>();
            //parametros.IndiceColeccion = e.NewPageIndex;
            //this.BusquedaParametrosGuardarValor<CobOrdenesCobros>(parametrosCob);

            //gvDatos.PageIndex = e.NewPageIndex;
            //gvDatos.DataSource = this.MisOrdenesCobros;
            //gvDatos.DataBind();
            //AyudaProgramacion.CargarGrillaListas(this.MiCuentaCorriente, false, this.gvDatos, true);
        }
        #endregion

    }
}
