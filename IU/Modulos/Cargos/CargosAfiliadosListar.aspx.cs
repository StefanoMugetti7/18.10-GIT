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
using System.Collections.Generic;
using Cargos.Entidades;
using Cargos;
using Comunes.Entidades;
using Afiliados.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Reportes.Entidades;
using System.Net.Mail;
using Comunes.LogicaNegocio;
using System.IO;
using Afiliados;
using Reportes.FachadaNegocio;

namespace IU.Modulos.Cargos
{
    public partial class CargosAfiliadosListar : PaginaAfiliados
    {
        private DataTable MisCargosAfiliadosMensuales
        {
            get { return (DataTable)Session[this.MiSessionPagina + "MisCargosAfiliadosMensuales"]; }
            set { Session[this.MiSessionPagina + "MisCargosAfiliadosMensuales"] = value; }
        }

        private DataTable MisCuotasPendientes
        {
            get { return (DataTable)Session[this.MiSessionPagina + "MisCuotasPendientes"]; }
            set { Session[this.MiSessionPagina + "MisCuotasPendientes"] = value; }
        }

        private DataTable MisCargosReservasTurismo
        {
            get { return (DataTable)Session[this.MiSessionPagina + "MisCargosReservasTurismo"]; }
            set { Session[this.MiSessionPagina + "MisCargosReservasTurismo"] = value; }
        }

        private List<CarTiposCargosAfiliadosFormasCobros> MisCargosAfiliadosAutomatico
        {
            get { return (List<CarTiposCargosAfiliadosFormasCobros>)Session[this.MiSessionPagina + "MisCargosAfiliadosAutomatico"]; }
            set { Session[this.MiSessionPagina + "MisCargosAfiliadosAutomatico"] = value; }
        }

        private List<CarTiposCargosAfiliadosFormasCobros> MisCargosAfiliadosAdministrables
        {
            get { return (List<CarTiposCargosAfiliadosFormasCobros>)Session[this.MiSessionPagina + "MisCargosAfiliadosAdministrables"]; }
            set { Session[this.MiSessionPagina + "MisCargosAfiliadosAdministrables"] = value; }
        }

        private List<CarTiposCargosAfiliadosFormasCobros> MisCargosAfiliadosHistoricos
        {
            get { return (List<CarTiposCargosAfiliadosFormasCobros>)Session[this.MiSessionPagina + "MisCargosAfiliadosHistoricos"]; }
            set { Session[this.MiSessionPagina + "MisCargosAfiliadosHistoricos"] = value; }
        }
              

        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);

            this.gvCargosMensuales.PageSizeEvent += this.GvCargosMensuales_PageSizeEvent;
            this.gvCuotasPendientes.PageSizeEvent += this.GvCuotasPendientes_PageSizeEvent;
            this.gvDatos.PageSizeEvent += this.GvDatos_PageSizeEvent;
            this.gvCargosAdministrables.PageSizeEvent += this.GvCargosAdministrables_PageSizeEvent;
            this.gvCargosHistoricos.PageSizeEvent += this.GvCargosHistoricos_PageSizeEvent;

            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtConepto, this.btnBuscar);
                this.btnAgregar.Visible = this.ValidarPermiso("CargosAfiliadosAgregar.aspx");
                this.CargarCombos();
                this.CargarLista(new CarTiposCargosAfiliadosFormasCobros());
                this.CargarCargosReservasTurismo(new CarTiposCargosAfiliadosFormasCobros());
             
                CarTiposCargosAfiliadosFormasCobros parametrosFiltros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>("filtroMultitab");
                if (parametrosFiltros.BusquedaParametros)
                {
                    
                    this.tcDatos.ActiveTabIndex = parametrosFiltros.HashTransaccion;
                    if (this.tcDatos.ActiveTab.ID == "tpCargosAutomaticos")
                    {
                        this.gvDatos.PageIndex = parametrosFiltros.HashTransaccion;
                    }if (this.tcDatos.ActiveTab.ID == "tpTurismo")
                    {
                        this.gvDatos.PageIndex = parametrosFiltros.HashTransaccion;
                    }if (this.tcDatos.ActiveTab.ID == "tpCargosAdministrables")
                    {
                        this.gvDatos.PageIndex = parametrosFiltros.HashTransaccion;
                    }if (this.tcDatos.ActiveTab.ID == "tpCargosMensuales")
                    {
                        this.gvDatos.PageIndex = parametrosFiltros.HashTransaccion;
                    }
                    else if (this.tcDatos.ActiveTab.ID == "tpCargosHistoricos")
                    {
                        this.gvCargosAdministrables.PageIndex = parametrosFiltros.HashTransaccion;
                    }
                }
                CarCuentasCorrientes afiFiltro = this.BusquedaParametrosObtenerValor<CarCuentasCorrientes>();
                if (afiFiltro.BusquedaParametros)
                {
                    this.ddlPeriodo.SelectedValue = afiFiltro.Periodo.ToString();
                    this.ddlFormaCobro.SelectedValue = afiFiltro.FormaCobro.IdFormaCobro.ToString();
                    this.ddlEstados.SelectedValue = afiFiltro.Estado.IdEstado.ToString();
                    this.ddlComprobantes.SelectedValue = afiFiltro.TipoOperacion.IdTipoOperacion.ToString();
                }
                this.CargarCargosMensuales(new CarCuentasCorrientes());
            }
        }
        private void GvCargosHistoricos_PageSizeEvent(int pageSize)
        {
            CarTiposCargosAfiliadosFormasCobros parametros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
            parametros.PageIndex = 0;
            this.gvDatos.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            this.CargarLista(parametros);
        }
        private void GvCargosAdministrables_PageSizeEvent(int pageSize)
        {
            CarTiposCargosAfiliadosFormasCobros parametros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
            parametros.PageIndex = 0;
            this.gvDatos.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            this.CargarLista(parametros);
        }
        private void GvDatos_PageSizeEvent(int pageSize)
        {
            CarTiposCargosAfiliadosFormasCobros parametros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
            parametros.PageIndex = 0;
            this.gvDatos.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            this.CargarLista(parametros);
        }
        private void GvCuotasPendientes_PageSizeEvent(int pageSize)
        {
            CarTiposCargosAfiliadosFormasCobros parametros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
            parametros.PageIndex = 0;
            this.gvDatos.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            this.CargarLista(parametros);
        }
        private void GvCargosMensuales_PageSizeEvent(int pageSize)
        {
            CarCuentasCorrientes parametros = this.BusquedaParametrosObtenerValor<CarCuentasCorrientes>();
            parametros.PageIndex = 0;
            this.gvDatos.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            this.CargarCargosMensuales(parametros);
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            CarTiposCargosAfiliadosFormasCobros parametrosFiltros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
            parametrosFiltros.BusquedaParametros = true;
            parametrosFiltros.IndiceColeccion = this.tcDatos.ActiveTabIndex;
            this.BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametrosFiltros);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosAgregar.aspx"), true);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.CargarCargosMensuales(new CarCuentasCorrientes());
        }
        protected void btnEnviarMail_Click(object sender, EventArgs e)
        {
            EnumTGEComprobantes comprobante = (EnumTGEComprobantes)Convert.ToInt32(this.ddlComprobantes.SelectedValue);

            RepReportes reporte = new RepReportes();
            RepParametros param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdAfiliado";
            param.ValorParametro = this.MiAfiliado.IdAfiliado;
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdEstado";
            param.ValorParametro = this.ddlEstados.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlEstados.SelectedValue);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdTipoOperacion";
            param.ValorParametro = (int)EnumTGETiposOperaciones.LevantamientoCargos;
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "Periodo";
            param.ValorParametro = this.ddlPeriodo.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlPeriodo.SelectedValue);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "PeriodoDesde";
            param.ValorParametro = this.ddlPeriodo.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlPeriodo.SelectedValue);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "PeriodoHasta";
            param.ValorParametro = Convert.ToInt32(string.Concat(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString().PadLeft(2, '0'))); // this.ddlPeriodo.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlPeriodo.SelectedValue);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdFormaCobro";
            param.ValorParametro = this.ddlFormaCobro.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFormaCobro.SelectedValue);
            reporte.Parametros.Add(param);

            MailMessage mail = new MailMessage();
            TGEArchivos archivo = new TGEArchivos();
            archivo.Archivo = AyudaProgramacionLN.StreamToByteArray(this.ctrPopUpComprobantes.GenerarReporteArchivo(reporte, comprobante));
            archivo.NombreArchivo = string.Concat(this.MiAfiliado.CUIL, ".pdf");

            mail.Attachments.Add(new Attachment(new MemoryStream(archivo.Archivo), archivo.NombreArchivo));
            if (AfiliadosF.AfiliadosArmarMailResumenCuenta(this.MiAfiliado, mail))
            {
                this.popUpMail.IniciarControl(mail, this.MiAfiliado);
            }
        }
        protected void btnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlComprobantes.SelectedValue))
            {
                //EnumTGEComprobantes comprobante = (EnumTGEComprobantes)Convert.ToInt32(this.ddlComprobantes.SelectedValue);

                RepReportes reporte = new RepReportes();
                RepParametros param = new RepParametros();
                param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
                param.Parametro = "IdAfiliado";
                param.ValorParametro = this.MiAfiliado.IdAfiliado;
                reporte.Parametros.Add(param);
                param = new RepParametros();
                param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
                param.Parametro = "IdEstado";
                param.ValorParametro = this.ddlEstados.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlEstados.SelectedValue);
                reporte.Parametros.Add(param);
                param = new RepParametros();
                param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
                param.Parametro = "IdTipoOperacion";
                param.ValorParametro = (int)EnumTGETiposOperaciones.LevantamientoCargos;
                reporte.Parametros.Add(param);
                param = new RepParametros();
                param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
                param.Parametro = "Periodo";
                param.ValorParametro = this.ddlPeriodo.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlPeriodo.SelectedValue);
                reporte.Parametros.Add(param);
                param = new RepParametros();
                param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
                param.Parametro = "PeriodoDesde";
                param.ValorParametro = this.ddlPeriodo.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlPeriodo.SelectedValue);
                reporte.Parametros.Add(param);
                param = new RepParametros();
                param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
                param.Parametro = "PeriodoHasta";
                param.ValorParametro = Convert.ToInt32(string.Concat(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString().PadLeft(2, '0'))); // this.ddlPeriodo.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlPeriodo.SelectedValue);
                reporte.Parametros.Add(param);
                param = new RepParametros();
                param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
                param.Parametro = "IdFormaCobro";
                param.ValorParametro = this.ddlFormaCobro.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFormaCobro.SelectedValue);
                reporte.Parametros.Add(param);

                TGEComprobantes comprobante = new TGEComprobantes();
                comprobante.IdComprobante = Convert.ToInt32(this.ddlComprobantes.SelectedValue);
                comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(comprobante);

                TGEPlantillas plantilla = new TGEPlantillas();
                plantilla.Codigo = comprobante.CodigoPlantilla;
                plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

                //TGEPlantillas plantilla = new TGEPlantillas();
                //plantilla.Codigo = "CargosAfiliadosDetalleDeuda";
                //plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

                reporte.StoredProcedure = string.IsNullOrWhiteSpace(plantilla.NombreSP) ?comprobante.NombreSP : plantilla.NombreSP;
                DataSet ds = ReportesF.ReportesObtenerDatos(reporte);

                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(comprobante, plantilla, ds, "IdAfiliado", AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                //byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(plantilla, ds, "IdAfiliado", AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));

                ExportPDF.ExportarPDF(pdf, this.Page, "CargosAfiliados_" + MiAfiliado.IdAfiliado.ToString() + "_" + this.UsuarioActivo.ApellidoNombre, this.UsuarioActivo);
                this.UpdatePanel1.Update();
            }
        }
        #region Cargos Mensuales
        protected void gvCargosMensuales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Modificar"
                || e.CommandName == "Auditoria"
                || e.CommandName == "Consultar"
                || e.CommandName=="DesimputarCobro"
                || e.CommandName == "RevertirCobro"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idCuentaCorriente = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            // DataRow fila = this.MisCargosAfiliadosMensuales.Rows.Find(index);
            CarTiposCargosAfiliadosFormasCobros parametrosFiltros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
            parametrosFiltros.BusquedaParametros = true;
            parametrosFiltros.IndiceColeccion = this.tcDatos.ActiveTabIndex;
            this.BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametrosFiltros, "filtroMultitab");
            switch (e.CommandName)
            {
                case "Modificar":
                    this.MisParametrosUrl = new Hashtable();
                    this.MisParametrosUrl.Add("IdCuentaCorriente", idCuentaCorriente);
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosModificarFomasCobros.aspx"), true);
                    break;
                //case "Auditoria":
                //    cargoAfiliado = CargosF.CuentasCorrientesObtenerDatosCompletos(cargoAfiliado);
                //    this.ctrAuditoria.IniciarControl(cargoAfiliado);
                //    break;
                case "Consultar" :
                    this.MisParametrosUrl = new Hashtable();
                    this.MisParametrosUrl.Add("IdCuentaCorriente", idCuentaCorriente);
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosConsultarFomasCobros.aspx"), true);
                    break;
                case "DesimputarCobro":
                    this.MisParametrosUrl = new Hashtable();
                    this.MisParametrosUrl.Add("IdCuentaCorriente", idCuentaCorriente);
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosDesimputarCobro.aspx"), true);
                    break;
                case "RevertirCobro":
                    this.MisParametrosUrl = new Hashtable();
                    this.MisParametrosUrl.Add("IdCuentaCorriente", idCuentaCorriente);
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosRevertirCobro.aspx"), true);
                    break;
                default:
                    break;
            }            
        }

        protected void gvCargosMensuales_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int index = Convert.ToInt32(e.Row.RowIndex);
                //CarCuentasCorrientes item = (CarCuentasCorrientes)e.Row.DataItem;
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                //ImageButton ibtnAuditoria = (ImageButton)e.Row.FindControl("btnAuditoria");
                //ibtnAuditoria.Visible = this.ValidarPermiso("AuditoriaDatos");
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnDesimputarCobro = (ImageButton)e.Row.FindControl("btnDesimputarCobro");
                ImageButton ibtnRevertirCobro = (ImageButton)e.Row.FindControl("btnRevertirCobro");

                ibtnConsultar.Visible = this.ValidarPermiso("CargosAfiliadosConsultarFomasCobros.aspx");
                int idEstado = Convert.ToInt32(((HiddenField)e.Row.FindControl("hdfIdEstado")).Value);
                switch (idEstado)
                {
                    case (int)EstadosCuentasCorrientes.EnviadoAlCobro:
                        ibtnModificar.Visible = this.ValidarPermiso("CargosAfiliadosModificarFomasCobrosEnviadoAlCobro.aspx");
                        break;
                    case (int)EstadosCuentasCorrientes.Activo:
                    case (int)EstadosCuentasCorrientes.Pendiente:
                    case (int)EstadosCuentasCorrientes.CobroDevuelto:
                    case (int)EstadosCuentasCorrientes.Rechazado:
                        ibtnModificar.Visible = this.ValidarPermiso("CargosAfiliadosModificarFomasCobros.aspx");
                        break;
                    case (int)EstadosCuentasCorrientes.CobroParcial:
                        ibtnModificar.Visible = this.ValidarPermiso("CargosAfiliadosModificarFomasCobros.aspx");
                        ibtnDesimputarCobro.Visible = this.ValidarPermiso("CargosAfiliadosDesimputarCobro.aspx");
                        ibtnRevertirCobro.Visible = this.ValidarPermiso("CargosAfiliadosRevertirCobro.aspx");
                        break;
                    case (int)EstadosCuentasCorrientes.Cobrado:
                        ibtnDesimputarCobro.Visible = this.ValidarPermiso("CargosAfiliadosDesimputarCobro.aspx");
                        ibtnRevertirCobro.Visible = this.ValidarPermiso("CargosAfiliadosRevertirCobro.aspx");
                        break;
                    default:
                        break;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                decimal suma;
                int Total;

                if (this.MisCargosAfiliadosMensuales.AsEnumerable().Where(x => x.Field<int>("MonedaIdMoneda") == 1).Count() > 0)
                {

                    Label lblImporte = (Label)e.Row.FindControl("lblImporte");
                    suma = Convert.ToDecimal(this.MisCargosAfiliadosMensuales.AsEnumerable().Where(x => x.Field<int>("MonedaIdMoneda") == 1).Sum(x => x.Field<decimal>("Importe")));
                    lblImporte.Text = string.Concat("$" + suma.ToString("N2"));

                    Label lblImporteCobrado = (Label)e.Row.FindControl("lblImporteCobrado");
                    suma = Convert.ToDecimal(this.MisCargosAfiliadosMensuales.AsEnumerable().Where(x => x.Field<int>("MonedaIdMoneda") == 1).Sum(x => x.Field<decimal>("ImporteCobrado")));
                    lblImporteCobrado.Text = string.Concat("$" + suma.ToString("N2"));

                    Label lblImporteEnviar = (Label)e.Row.FindControl("lblImporteEnviar");
                    suma = Convert.ToDecimal(this.MisCargosAfiliadosMensuales.AsEnumerable().Where(x => x.Field<int>("MonedaIdMoneda") == 1).Sum(x => x.Field<decimal>("ImporteEnviar")));
                    lblImporteEnviar.Text = string.Concat("$" + suma.ToString("N2"));

                    Label lblRegistros = (Label)e.Row.FindControl("lblRegistros");
                    Total = this.MisCargosAfiliadosMensuales.AsEnumerable().Where(x => x.Field<int>("MonedaIdMoneda") == 1).Count();
                    lblRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), Total);

                    lblImporte.Visible = true;
                    lblImporteCobrado.Visible = true;
                    lblImporteEnviar.Visible = true;
                    lblRegistros.Visible = true;

                }
                if (this.MisCargosAfiliadosMensuales.AsEnumerable().Where(x => x.Field<int>("MonedaIdMoneda") == 2).Count() > 0)
                {
                    Label lblImporteDolar = (Label)e.Row.FindControl("lblImporteDolar");
                    suma = Convert.ToDecimal(this.MisCargosAfiliadosMensuales.AsEnumerable().Where(x => x.Field<int>("MonedaIdMoneda") == 2).Sum(x => x.Field<decimal>("Importe")));
                    lblImporteDolar.Text = string.Concat("U$D" + suma.ToString("N2"));

                    Label lblImporteCobradoDolar = (Label)e.Row.FindControl("lblImporteCobradoDolar");
                    suma = Convert.ToDecimal(this.MisCargosAfiliadosMensuales.AsEnumerable().Where(x => x.Field<int>("MonedaIdMoneda") == 2).Sum(x => x.Field<decimal>("ImporteCobrado")));
                    lblImporteCobradoDolar.Text = string.Concat("U$D" + suma.ToString("N2"));

                    Label lblImporteEnviarDolar = (Label)e.Row.FindControl("lblImporteEnviarDolar");
                    suma = Convert.ToDecimal(this.MisCargosAfiliadosMensuales.AsEnumerable().Where(x => x.Field<int>("MonedaIdMoneda") == 2).Sum(x => x.Field<decimal>("ImporteEnviar")));
                    lblImporteEnviarDolar.Text = string.Concat("U$D" + suma.ToString("N2"));

                    Label lblRegistrosDolar = (Label)e.Row.FindControl("lblRegistrosDolar");
                    Total = this.MisCargosAfiliadosMensuales.AsEnumerable().Where(x => x.Field<int>("MonedaIdMoneda") == 2).Count();
                    lblRegistrosDolar.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), Total);

                    lblImporteDolar.Visible = true;
                    lblImporteCobradoDolar.Visible = true;
                    lblImporteEnviarDolar.Visible = true;
                    lblRegistrosDolar.Visible = true;

                }

            }
        }

        protected void gvCargosMensuales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CarCuentasCorrientes parametros = this.BusquedaParametrosObtenerValor<CarCuentasCorrientes>();
            gvCargosMensuales.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            CarTiposCargosAfiliadosFormasCobros parametro = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
            parametros.HashTransaccion = tpCargosMensuales.TabIndex;
            parametros.BusquedaParametros = true;
            BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametro);
            CargarCargosMensuales(parametros);
        }

        //protected void gvCargosMensuales_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //    this.MisCargosAfiliadosMensuales = this.OrdenarGrillaDatos<CarCuentasCorrientes>(this.MisCargosAfiliadosMensuales, e);
        //    this.gvCargosMensuales.DataSource = this.MisCargosAfiliadosMensuales;
        //    this.gvCargosMensuales.DataBind();
        //}

        #endregion
        #region Cuotas Pendientes

        protected void gvCuotasPendientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {

              

                decimal suma;

                if (this.MisCuotasPendientes.AsEnumerable().Where(x => x.Field<int>("IdMoneda") == 1).Count() > 0)
                {
                    Label lblImporte = (Label)e.Row.FindControl("lblImporte");
                    suma = Convert.ToDecimal(this.MisCuotasPendientes.AsEnumerable().Where(x => x.Field<int>("IdMoneda") == 1).Sum(x => x.Field<decimal>("ImporteCuota")));
                    lblImporte.Text = string.Concat("$" + suma.ToString("N2"));

                    Label lblImporteCobrado = (Label)e.Row.FindControl("lblImporteRestante");
                    suma = Convert.ToDecimal(this.MisCuotasPendientes.AsEnumerable().Where(x => x.Field<int>("IdMoneda") == 1).Sum(x => x.Field<decimal>("ImporteRestante")));
                    lblImporteCobrado.Text = string.Concat("$" + suma.ToString("N2"));

                    Label lblRegistros = (Label)e.Row.FindControl("lblRegistros");
                    suma = this.MisCuotasPendientes.AsEnumerable().Where(x => x.Field<int>("IdMoneda") == 1).Count();
                    lblRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), suma);

                    lblRegistros.Visible = true;
                    lblImporte.Visible = true;
                    lblImporteCobrado.Visible = true;
                }

                if (this.MisCuotasPendientes.AsEnumerable().Where(x => x.Field<int>("IdMoneda") == 2).Count() > 0)
                {
                    Label lblImporteDolar = (Label)e.Row.FindControl("lblImporteDolar");
                    suma = Convert.ToDecimal(this.MisCuotasPendientes.AsEnumerable().Where(x => x.Field<int>("IdMoneda") == 2).Sum(x => x.Field<decimal>("ImporteCuota")));
                    lblImporteDolar.Text = string.Concat("U$D" + suma.ToString("N2"));

                    Label lblImporteCobradoDolar = (Label)e.Row.FindControl("lblImporteRestanteDolar");
                    suma = Convert.ToDecimal(this.MisCuotasPendientes.AsEnumerable().Where(x => x.Field<int>("IdMoneda") == 2).Sum(x => x.Field<decimal>("ImporteRestante")));
                    lblImporteCobradoDolar.Text = string.Concat("U$D" + suma.ToString("N2"));

                    Label lblRegistrosDolar = (Label)e.Row.FindControl("lblRegistrosDolar");
                    suma = this.MisCuotasPendientes.AsEnumerable().Where(x => x.Field<int>("IdMoneda") == 2).Count();
                    lblRegistrosDolar.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), suma);

                    lblRegistrosDolar.Visible = true;
                    lblImporteDolar.Visible = true;
                    lblImporteCobradoDolar.Visible = true;
                }


            }
        }

        protected void gvCuotasPendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CarTiposCargosAfiliadosFormasCobros parametros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
            gvCuotasPendientes.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            parametros.HashTransaccion = tcCuotasPendientes.TabIndex;
            parametros.BusquedaParametros = true;
            BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametros);
            CargarLista(parametros);
        }

        #endregion
        #region Cargos Automaticos

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                   || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CarTiposCargosAfiliadosFormasCobros cargoAfiliado = this.MisCargosAfiliadosAutomatico[indiceColeccion];
            //string parametros = string.Format("?IdTipoCargoAfiliadoFormaCobro={0}", cargoAfiliado.IdTipoCargoAfiliadoFormaCobro);
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdTipoCargoAfiliadoFormaCobro", cargoAfiliado.IdTipoCargoAfiliadoFormaCobro);
            if (e.CommandName == Gestion.Modificar.ToString())
            {
                CarTiposCargosAfiliadosFormasCobros parametrosFiltros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
                parametrosFiltros.BusquedaParametros = true;
                parametrosFiltros.IndiceColeccion = this.tcDatos.ActiveTabIndex;
                this.BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametrosFiltros);
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosModificar.aspx"), true);
            }
            //else if (e.CommandName == Gestion.Consultar.ToString())
            //{
            //    CarTiposCargosAfiliadosFormasCobros parametrosFiltros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
            //    parametrosFiltros.BusquedaParametros = true;
            //    parametrosFiltros.IndiceColeccion = this.tcDatos.ActiveTabIndex;
            //    this.BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametrosFiltros);

            //    string url = "~/Modulos/Cargos/CargosAfiliadosConsultar.aspx";
            //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(url), true);
            //}
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                //ibtnModificar.Visible = this.ValidarPermiso("CargosAfiliadosModificar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisCargosAfiliadosAutomatico.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CarTiposCargosAfiliadosFormasCobros parametros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
            gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            parametros.HashTransaccion = tpCargosAutomaticos.TabIndex;
            parametros.BusquedaParametros = true;
            BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametros);
            CargarLista(parametros);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisCargosAfiliadosAutomatico = this.OrdenarGrillaDatos<CarTiposCargosAfiliadosFormasCobros>(this.MisCargosAfiliadosAutomatico, e);
            this.gvDatos.DataSource = this.MisCargosAfiliadosAutomatico;
            this.gvDatos.DataBind();
        }

        #endregion
        #region Reservas de Turismo

        protected void gvReservasTurismo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                   || e.CommandName == Gestion.Modificar.ToString()
                   || e.CommandName == Gestion.Impresion.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idTipoCargoAdministrable = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdTipoCargoAfiliadoFormaCobro", idTipoCargoAdministrable);
            if (e.CommandName == Gestion.Modificar.ToString())
            {
                CarTiposCargosAfiliadosFormasCobros parametrosFiltros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
                parametrosFiltros.BusquedaParametros = true;
                parametrosFiltros.HashTransaccion = this.tcDatos.ActiveTabIndex;
                parametrosFiltros.Filtro = this.tcDatos.ActiveTab.ID;
                this.BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametrosFiltros, "filtroMultitab");

                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                CarTiposCargosAfiliadosFormasCobros parametrosFiltros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
                parametrosFiltros.BusquedaParametros = true;
                parametrosFiltros.HashTransaccion = this.tcDatos.ActiveTabIndex;
                parametrosFiltros.Filtro = this.tcDatos.ActiveTab.ID;
                this.BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametrosFiltros, "filtroMultitab");

                string url = "~/Modulos/Cargos/CargosAfiliadosConsultar.aspx";
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(url), true);
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                CarTiposCargosAfiliadosFormasCobros cargoTurismo = new CarTiposCargosAfiliadosFormasCobros();
                cargoTurismo.IdTipoCargoAfiliadoFormaCobro = idTipoCargoAdministrable;
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.SinComprobante, "CarTiposCargosAfiliadosFormasCobrosReservaTurismo", cargoTurismo, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, UpdatePanel1, "CarTiposCargosAfiliadosFormasCobrosReservaTurismo", this.UsuarioActivo);
            }
        }

        protected void gvReservasTurismo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int idEstado = Convert.ToInt32(((HiddenField)e.Row.FindControl("hdfIdEstado")).Value);
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                if (idEstado == (int)EstadosCargos.Facturado || idEstado == (int)EstadosCargos.Activo)
                    ibtnModificar.Visible = true;
                //ibtnModificar.Visible = this.ValidarPermiso("CargosAfiliadosModificar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                
            }
        }
        
        protected void gvReservasTurismo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CarTiposCargosAfiliadosFormasCobros parametros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
            gvReservasTurismo.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            parametros.HashTransaccion = tpTurismo.TabIndex;
            parametros.BusquedaParametros = true;
            BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametros);
            CargarCargosReservasTurismo(parametros);
        }

        #endregion
        #region Carogs Administrables
        protected void gvCargosAdministrables_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                   || e.CommandName == Gestion.Modificar.ToString()
                   || e.CommandName == Gestion.Impresion.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CarTiposCargosAfiliadosFormasCobros cargoAfiliado = this.MisCargosAfiliadosAdministrables[indiceColeccion];
            //string parametros = string.Format("?IdTipoCargoAfiliadoFormaCobro={0}", cargoAfiliado.IdTipoCargoAfiliadoFormaCobro);
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdTipoCargoAfiliadoFormaCobro", cargoAfiliado.IdTipoCargoAfiliadoFormaCobro);
            if (e.CommandName == Gestion.Modificar.ToString())
            {
                CarTiposCargosAfiliadosFormasCobros parametrosFiltros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
                parametrosFiltros.BusquedaParametros = true;
                parametrosFiltros.HashTransaccion = this.tcDatos.ActiveTabIndex;
                this.BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametrosFiltros, "filtroMultitab");

                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosModificar.aspx"), true);
            }
            //else if (e.CommandName == Gestion.Consultar.ToString())
            //{
            //    CarTiposCargosAfiliadosFormasCobros parametrosFiltros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
            //    parametrosFiltros.BusquedaParametros = true;
            //    parametrosFiltros.HashTransaccion = this.tcDatos.ActiveTabIndex;
            //    this.BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametrosFiltros);

            //    string url = "~/Modulos/Cargos/CargosAfiliadosConsultar.aspx";
            //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(url), true);
            //}
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                cargoAfiliado.Filtro = cargoAfiliado.IdTipoCargoAfiliadoFormaCobro.ToString();
                TGEPlantillas miPlantilla = TGEGeneralesF.PlantillasObtenerDatosPorTipoOperacion(cargoAfiliado);
                cargoAfiliado.Filtro = String.Empty;
                if (miPlantilla.Codigo.Trim() == String.Empty)
                    miPlantilla.Codigo = "CarTiposCargosAfiliadosFormasCobros";
                //this.ctrPopUpComprobantes.CargarReporte(this.MiOrdenPago, EnumTGEComprobantes.CapOrdenesPagos);
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CarCargosAfiliadosFormasCobros, miPlantilla.Codigo, cargoAfiliado, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this.Page, "Cargo", this.UsuarioActivo);

                //this.ctrPopUpComprobantes.CargarReporte(cargoAfiliado, EnumTGEComprobantes.CarCargosAfiliadosFormasCobros);
                //this.UpdatePanel1.Update();
            }
        }

        protected void gvCargosAdministrables_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                //ibtnModificar.Visible = this.ValidarPermiso("CargosAfiliadosModificar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisCargosAfiliadosAdministrables.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvCargosAdministrables_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CarTiposCargosAfiliadosFormasCobros parametros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
            gvCargosAdministrables.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            parametros.HashTransaccion = gvCargosAdministrables.TabIndex;
            parametros.BusquedaParametros = true;
            BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametros);
            CargarLista(parametros);
        }

        protected void gvCargosAdministrables_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisCargosAfiliadosAdministrables = this.OrdenarGrillaDatos<CarTiposCargosAfiliadosFormasCobros>(this.MisCargosAfiliadosAdministrables, e);
            this.gvCargosAdministrables.DataSource = this.MisCargosAfiliadosAdministrables;
            this.gvCargosAdministrables.DataBind();
        }

        
        #endregion    
        #region Carogs Historicos
        protected void gvCargosHistoricos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (!(e.CommandName == "Modificar"
                || e.CommandName == "Auditoria"
                || e.CommandName == "Consultar"
                || e.CommandName == Gestion.Impresion.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CarTiposCargosAfiliadosFormasCobros cargoAfiliado = this.MisCargosAfiliadosHistoricos[indiceColeccion];
            //string parametros = string.Format("?IdTipoCargoAfiliadoFormaCobro={0}", cargoAfiliado.IdTipoCargoAfiliadoFormaCobro);
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdTipoCargoAfiliadoFormaCobro", cargoAfiliado.IdTipoCargoAfiliadoFormaCobro);
            if (e.CommandName == Gestion.Consultar.ToString())
            {
                CarTiposCargosAfiliadosFormasCobros parametrosFiltros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
                parametrosFiltros.BusquedaParametros = true;
                parametrosFiltros.HashTransaccion = this.tcDatos.ActiveTabIndex;
                this.BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametrosFiltros, "filtroMultitab");

                string url = "~/Modulos/Cargos/CargosAfiliadosConsultar.aspx";
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(url), true);
            }
            if (e.CommandName == Gestion.Impresion.ToString())
            {
                TGEPlantillas miPlantilla = TGEGeneralesF.PlantillasObtenerDatosPorTipoOperacion(cargoAfiliado);
                if (miPlantilla.Codigo.Trim() == String.Empty)
                    miPlantilla.Codigo = "CarTiposCargosAfiliadosFormasCobros";
                //this.ctrPopUpComprobantes.CargarReporte(this.MiOrdenPago, EnumTGEComprobantes.CapOrdenesPagos);
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CarCargosAfiliadosFormasCobros, miPlantilla.Codigo, MisCargosAfiliadosHistoricos[indiceColeccion] , AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, UpdatePanel1, "CarTiposCargosAfiliadosFormasCobros", this.UsuarioActivo);
                this.UpdatePanel1.Update();
            }
            //if ((e.CommandName == "Sort"))
            //    return;

            //int index = Convert.ToInt32(e.CommandArgument);
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            //CarTiposCargosAfiliadosFormasCobros cargoAfiliado = this.MisCargosAfiliadosAdministrables[indiceColeccion];
            //string parametros = string.Format("?IdTipoCargoAfiliadoFormaCobro={0}", cargoAfiliado.IdTipoCargoAfiliadoFormaCobro);
            //if (e.CommandName == Gestion.Modificar.ToString())
            //{
            //    CarTiposCargosAfiliadosFormasCobros parametrosFiltros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
            //    parametrosFiltros.BusquedaParametros = true;
            //    parametrosFiltros.HashTransaccion = this.tcDatos.ActiveTabIndex;
            //    this.BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametrosFiltros);

            //    string url = string.Concat("~/Modulos/Cargos/CargosAfiliadosModificar.aspx", parametros);
            //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(url), true);
            //}
        }

        protected void gvCargosHistoricos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                ibtnConsultar.Visible = this.ValidarPermiso("CargosAfiliadosConsultar.aspx");
                ibtnModificar.Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisCargosAfiliadosHistoricos.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvCargosHistoricos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CarTiposCargosAfiliadosFormasCobros parametros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
            gvCargosHistoricos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            parametros.HashTransaccion = tpCargosHistoricos.TabIndex;
            parametros.BusquedaParametros = true;
            BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametros);
            CargarLista(parametros);
        }

        protected void gvCargosHistoricos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisCargosAfiliadosHistoricos = this.OrdenarGrillaDatos<CarTiposCargosAfiliadosFormasCobros>(this.MisCargosAfiliadosHistoricos, e);
            this.gvCargosHistoricos.DataSource = this.MisCargosAfiliadosHistoricos;
            this.gvCargosHistoricos.DataBind();
        }

        #endregion
        private void CargarCombos()
        {
            CarCuentasCorrientes filtro = new CarCuentasCorrientes();
            filtro.IdAfiliado = this.MiAfiliado.IdAfiliado;
            this.ddlPeriodo.DataSource = CargosF.CuentasCorrientesObtenerPeriodos(filtro);
            this.ddlPeriodo.DataTextField = "IdListaValorDetalle";
            this.ddlPeriodo.DataValueField = "Descripcion";
            this.ddlPeriodo.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlPeriodo, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlFormaCobro.DataSource = TGEGeneralesF.FormasCobrosObtenerLista();
            this.ddlFormaCobro.DataTextField = "FormaCobro";
            this.ddlFormaCobro.DataValueField = "IdFormaCobro";
            this.ddlFormaCobro.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlFormaCobro, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosCuentasCorrientes));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlEstados, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlComprobantes.DataSource = TGEGeneralesF.ComprobantesObtenerListaCargos();
            this.ddlComprobantes.DataValueField = "IdComprobante";
            this.ddlComprobantes.DataTextField = "Nombre";
            this.ddlComprobantes.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlComprobantes, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlMoneda.DataSource = TGEGeneralesF.MonedasObtenerLista();
            this.ddlMoneda.DataValueField = "IdMoneda";
            this.ddlMoneda.DataTextField = "Descripcion";
            this.ddlMoneda.DataBind();
            if (this.ddlMoneda.Items.Count != 1)
                AyudaProgramacion.InsertarItemSeleccione(this.ddlMoneda, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void CargarLista(CarTiposCargosAfiliadosFormasCobros parametro)
        {
            parametro.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(UsuarioActivo.PageSize);
            gvCuotasPendientes.PageSize = parametro.PageSize;
            gvDatos.PageSize = parametro.PageSize;
            gvCargosAdministrables.PageSize = parametro.PageSize;
            gvCargosHistoricos.PageSize = parametro.PageSize;

            parametro.IdAfiliado = this.MiAfiliado.IdAfiliado;
            parametro.Estado.IdEstado = (int)EstadosTodos.Todos;
            parametro.TipoCargo.TipoCargoProceso.IdTipoCargoProceso = (int)EnumTiposCargosProcesos.Automatico;
            this.MisCargosAfiliadosAutomatico = CargosF.TiposCargosAfiliadosObtenerLista(parametro);
            AyudaProgramacion.CargarGrillaListas<CarTiposCargosAfiliadosFormasCobros>(this.MisCargosAfiliadosAutomatico, false, this.gvDatos, true);

            parametro.IdAfiliado = this.MiAfiliado.IdAfiliado;
            parametro.Estado.IdEstado = (int)EstadosTodos.Todos;
            parametro.TipoCargo.TipoCargoProceso.IdTipoCargoProceso = (int)EnumTiposCargosProcesos.Administrable;
            List<CarTiposCargosAfiliadosFormasCobros> lista = CargosF.TiposCargosAfiliadosObtenerLista(parametro);
            parametro.TipoCargo.TipoCargoProceso.IdTipoCargoProceso = (int)EnumTiposCargosProcesos.Bonificacion;
            lista.AddRange(CargosF.TiposCargosAfiliadosObtenerLista(parametro));
            parametro.TipoCargo.TipoCargoProceso.IdTipoCargoProceso = (int)EnumTiposCargosProcesos.Informativo;
            lista.AddRange(CargosF.TiposCargosAfiliadosObtenerLista(parametro));

            lista = lista.OrderByDescending(x => x.FechaAlta).ThenByDescending(x => x.IdTipoCargoAfiliadoFormaCobro).ToList();

            this.MisCargosAfiliadosAdministrables = lista.Where(x => x.Estado.IdEstado == (int)EstadosCargos.Activo
                || x.Estado.IdEstado == (int)EstadosCargos.PeriodicidadMensual
                || x.Estado.IdEstado == (int)EstadosCargos.Pendiente
                || x.Estado.IdEstado == (int)EstadosCargos.PeriodicidadAnual
                || x.Estado.IdEstado == (int)EstadosCargos.Facturandose).ToList();
            this.MisCargosAfiliadosHistoricos = lista.Where(p => !MisCargosAfiliadosAdministrables.Any(p2 => p2.IdTipoCargoAfiliadoFormaCobro == p.IdTipoCargoAfiliadoFormaCobro)).ToList();

            this.MisCargosAfiliadosAdministrables = AyudaProgramacion.AcomodarIndices<CarTiposCargosAfiliadosFormasCobros>(this.MisCargosAfiliadosAdministrables);
            this.MisCargosAfiliadosHistoricos = AyudaProgramacion.AcomodarIndices<CarTiposCargosAfiliadosFormasCobros>(this.MisCargosAfiliadosHistoricos);

            AyudaProgramacion.CargarGrillaListas<CarTiposCargosAfiliadosFormasCobros>(this.MisCargosAfiliadosAdministrables, false, this.gvCargosAdministrables, true);
            AyudaProgramacion.CargarGrillaListas<CarTiposCargosAfiliadosFormasCobros>(this.MisCargosAfiliadosHistoricos, false, this.gvCargosHistoricos, true);

            this.MisCuotasPendientes = CargosF.CarTiposCargosAfiliadosFormasCobrosObtenerCuotasPendientes(parametro);
            this.gvCuotasPendientes.DataSource = this.MisCuotasPendientes;
            this.gvCuotasPendientes.DataBind();
        }

        private void CargarCargosMensuales(CarCuentasCorrientes parametro)
        {
            parametro.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(UsuarioActivo.PageSize);
            gvCargosMensuales.PageSize = parametro.PageSize;

            parametro.IdAfiliado = this.MiAfiliado.IdAfiliado;
            parametro.Estado.IdEstado = this.ddlEstados.SelectedValue==string.Empty? -1 : Convert.ToInt32(this.ddlEstados.SelectedValue);
            parametro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.LevantamientoCargos;
            parametro.Periodo = this.ddlPeriodo.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlPeriodo.SelectedValue);
            parametro.FormaCobro.IdFormaCobro = this.ddlFormaCobro.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFormaCobro.SelectedValue);
            parametro.Moneda.IdMoneda = this.ddlMoneda.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlMoneda.SelectedValue);
            parametro.Concepto = txtConepto.Text;
            parametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CarCuentasCorrientes>(parametro);
            this.MisCargosAfiliadosMensuales = CargosF.CuentasCorrientesObtenerDT(parametro);
            this.gvCargosMensuales.DataSource = this.MisCargosAfiliadosMensuales;
            this.gvCargosMensuales.VirtualItemCount = MisCargosAfiliadosMensuales.Rows.Count > 0 ? Convert.ToInt32(MisCargosAfiliadosMensuales.Rows[0]["Cantidad"]) : 0;
            this.gvCargosMensuales.deTantos.Text = "de " + gvCargosMensuales.VirtualItemCount.ToString();

            this.gvCargosMensuales.DataBind();
            this.UpdatePanel1.Update();
        }

        private void CargarCargosReservasTurismo(CarTiposCargosAfiliadosFormasCobros parametro)
        {
            parametro.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(UsuarioActivo.PageSize);
            gvReservasTurismo.PageSize = parametro.PageSize;

            parametro.IdAfiliado = this.MiAfiliado.IdAfiliado;
            parametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametro);
            this.MisCargosReservasTurismo = CargosF.TiposCargosAfiliadosObtenerReservasTurismo(parametro);
            this.gvReservasTurismo.DataSource = this.MisCargosReservasTurismo;
            this.gvReservasTurismo.VirtualItemCount = MisCargosReservasTurismo.Rows.Count > 0 ? Convert.ToInt32(MisCargosReservasTurismo.Rows[0]["Cantidad"]) : 0;
            this.gvReservasTurismo.deTantos.Text = "de " + gvReservasTurismo.VirtualItemCount.ToString();

            this.gvReservasTurismo.DataBind();
            this.UpdatePanel1.Update();
        }
    }
}
