using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Cargos.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Cargos;
using Generales.Entidades;
using System.Collections.Generic;
using Afiliados.Entidades;
using System.Globalization;
using Reportes.FachadaNegocio;

namespace IU.Modulos.Cargos.Controles
{
    public partial class CargoAfiliadoModificarDatos : ControlesSeguros
    {
        private CarTiposCargosAfiliadosFormasCobros MiCarCargosAfilados
        {
            get { return (CarTiposCargosAfiliadosFormasCobros)Session[this.MiSessionPagina + "MiCarCargosAfilados"]; }
            set { Session[this.MiSessionPagina + "MiCarCargosAfilados"] = value; }
        }

        private List<CarTiposCargosAfiliadosFormasCobros> MisCarCargosABonificar
        {
            get { return (List<CarTiposCargosAfiliadosFormasCobros>)Session[this.MiSessionPagina + "MisCarCargosABonificar"]; }
            set { Session[this.MiSessionPagina + "MisCarCargosABonificar"] = value; }
        }

        private List<TGEFormasCobrosAfiliados> MisFormasCobros
        {
            get { return (List<TGEFormasCobrosAfiliados>)Session[this.MiSessionPagina + "CargoAfiliadoModificarDatosMisFormasCobros"]; }
            set { Session[this.MiSessionPagina + "CargoAfiliadoModificarDatosMisFormasCobros"] = value; }
        }

        private List<TGEMonedas> MisMonedas
        {
            get { return (List<TGEMonedas>)Session[this.MiSessionPagina + "CargoAfiliadoModificarDatosMisMonedas"]; }
            set { Session[this.MiSessionPagina + "CargoAfiliadoModificarDatosMisMonedas"] = value; }
        }
        private bool AceptarContinuar
        {
            get { return (bool)Session[this.MiSessionPagina + "CargoAfiliadoModificarDatosAceptarContinuar"]; }
            set { Session[this.MiSessionPagina + "CargoAfiliadoModificarDatosAceptarContinuar"] = value; }
        }
        //bool AceptarContinuar = false;

        public delegate void CargoAfiliadoAceptarEventHandler(object sender, CarTiposCargosAfiliadosFormasCobros e);
        public event CargoAfiliadoAceptarEventHandler CargoAfiliadoModificarDatosAceptar;
        public delegate void CargoAfiliadoCancelarEventHandler();
        public event CargoAfiliadoCancelarEventHandler CargoAfiliadoModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
                AceptarContinuar = false;
                this.txtImporteCargo.Attributes.Add("onchange", "txtControlChange();");
                this.txtCantidadCuotas.Attributes.Add("onchange", "txtControlChange();");
                this.txtTasaInteres.Attributes.Add("onchange", "txtControlChange();");
                this.txtPorcentaje.Attributes.Add("onchange", "CalcularBonificacion();");
                this.ddlCargoReferencia.Attributes.Add("onchange", "CalcularBonificacion();");
                if (this.MiCarCargosAfilados == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/ControlSesion.aspx"), true);
                }
            }
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(CarTiposCargosAfiliadosFormasCobros pCargoAfiliado, Gestion pGestion)
        {
            MisMonedas = new List<TGEMonedas>();
            this.GestionControl = pGestion;
            this.MiCarCargosAfilados = pCargoAfiliado;
            hdfIdAfiliado.Value = pCargoAfiliado.IdAfiliado.ToString();
            this.CargarComboEstados();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.CargarCombos();
                    //this.ddlEstados.SelectedValue = ((int)EstadosCargos.Activo).ToString();
                    string parametro = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.EstadoCargosAdministrablespordefecto).ParametroValor;

                    ListItem item = ddlEstados.Items.FindByValue(parametro);
                    if (item != null)
                        ddlEstados.SelectedValue = parametro;

                    this.ddlEstados.Items.Remove(this.ddlEstados.Items.FindByValue(((int)EstadosCargos.Baja).ToString()));
                    this.txtFechaAlta.Text = DateTime.Today.ToShortDateString();
                    this.txtFechaAlta.Enabled = true;
                    ddlMoneda.Enabled = true;
                    this.ctrComentarios.IniciarControl(this.MiCarCargosAfilados, this.GestionControl);
                    this.ctrArchivos.IniciarControl(this.MiCarCargosAfilados, this.GestionControl);
                    break;
                case Gestion.Modificar:
                    this.ddlTipoCargo.Enabled = false;
                    this.ddlFormaCobro.Enabled = true;
                    this.ddlEstados.Enabled = true;
                    this.btnImprimir.Visible = true;
                    this.MiCarCargosAfilados = CargosF.TiposCargosAfiliadosObtenerDatosCompletos(pCargoAfiliado);
                    this.CargarComboFormasCobros();
                    hdfIdTipoCargoAfiliadoFormaCobro.Value = MiCarCargosAfilados.IdTipoCargoAfiliadoFormaCobro.ToString();
                    hdfIdTipoCargo.Value = MiCarCargosAfilados.TipoCargo.IdTipoCargo.ToString();

                    //this.ddlEstado.Items.Remove(this.ddlEstado.Items.FindByValue(((int)EstadosCargos.Activo).ToString()));
                    //this.ddlEstado.Items.Remove(this.ddlEstado.Items.FindByValue(((int)EstadosCargos.PeriodicidadMensual).ToString()));
                    this.MapearObjetoAControles(this.MiCarCargosAfilados);
                    SetInitializeCulture(MiCarCargosAfilados.Moneda.Moneda);
                    if (this.MiCarCargosAfilados.TipoCargo.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.Administrable
                        || this.MiCarCargosAfilados.TipoCargo.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.Bonificacion)
                    {
                        if (this.MiCarCargosAfilados.CantidadCuotas == 1
                        && (this.MiCarCargosAfilados.Estado.IdEstado == (int)EstadosCargos.PeriodicidadMensual
                            || this.MiCarCargosAfilados.Estado.IdEstado == (int)EstadosCargos.PeriodicidadAnual))
                        {
                            this.ddlEstados.Items.Remove(this.ddlEstados.Items.FindByValue(((int)EstadosCargos.Activo).ToString()));
                            this.ddlEstados.Items.Remove(this.ddlEstados.Items.FindByValue(((int)EstadosCargos.Pendiente).ToString()));
                            this.txtCantidadCuotas.Enabled = false;

                            if (this.MiCarCargosAfilados.Estado.IdEstado == (int)EstadosCargos.PeriodicidadMensual)
                                this.ddlEstados.Items.Remove(this.ddlEstados.Items.FindByValue(((int)EstadosCargos.PeriodicidadAnual).ToString()));
                            else if (this.MiCarCargosAfilados.Estado.IdEstado == (int)EstadosCargos.PeriodicidadAnual)
                                this.ddlEstados.Items.Remove(this.ddlEstados.Items.FindByValue(((int)EstadosCargos.PeriodicidadMensual).ToString()));
                        }
                        if (this.MiCarCargosAfilados.Estado.IdEstado == (int)EstadosCargos.Activo)
                        {
                            this.ddlEstados.Items.Remove(this.ddlEstados.Items.FindByValue(((int)EstadosCargos.Pendiente).ToString()));
                        }

                        if (!this.MiCarCargosAfilados.CargoFacturado)
                        {
                            if (this.MiCarCargosAfilados.TipoCargo.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.Administrable)
                            {
                                this.txtImporteCargo.Enabled = true;
                                this.txtCantidadCuotas.Enabled = this.MiCarCargosAfilados.Estado.IdEstado == (int)EstadosCargos.Activo;
                                this.txtTasaInteres.Enabled = true;
                                this.txtFechaAlta.Enabled = true;
                            }
                            else if (this.MiCarCargosAfilados.TipoCargo.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.Bonificacion)
                            {
                                this.ddlCargoReferencia.Enabled = true;
                            }
                            else if (this.MiCarCargosAfilados.TipoCargo.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.AdministrablePorcentaje)
                            {
                                txtPorcentaje.Enabled = true;
                                rfvPorcentaje.Enabled = true;
                            }
                        }
                    }
                    else if (this.MiCarCargosAfilados.TipoCargo.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.Turismo)
                    {
                        dvImporteCargo.Visible = false;
                        dvPorcentaje.Visible = false;
                        dvImporteInteres.Visible = false;

                        if (this.MiCarCargosAfilados.Estado.IdEstado == (int)EstadosCargos.Activo)
                        {
                            this.ddlEstados.Items.Remove(this.ddlEstados.Items.FindByValue(((int)EstadosCargos.Pendiente).ToString()));
                            this.ddlEstados.Items.Remove(this.ddlEstados.Items.FindByValue(((int)EstadosCargos.PeriodicidadMensual).ToString()));
                            this.ddlEstados.Items.Remove(this.ddlEstados.Items.FindByValue(((int)EstadosCargos.PeriodicidadAnual).ToString()));
                            this.ddlEstados.Items.Remove(this.ddlEstados.Items.FindByValue(((int)EstadosCargos.Facturado).ToString()));
                        }
                        if (this.MiCarCargosAfilados.Estado.IdEstado == (int)EstadosCargos.Facturado)
                        {
                            this.ddlEstados.Items.Remove(this.ddlEstados.Items.FindByValue(((int)EstadosCargos.Pendiente).ToString()));
                            this.ddlEstados.Items.Remove(this.ddlEstados.Items.FindByValue(((int)EstadosCargos.PeriodicidadMensual).ToString()));
                            this.ddlEstados.Items.Remove(this.ddlEstados.Items.FindByValue(((int)EstadosCargos.PeriodicidadAnual).ToString()));
                            this.ddlEstados.Items.Remove(this.ddlEstados.Items.FindByValue(((int)EstadosCargos.Activo).ToString()));
                        }
                    }
                    //this.ddlTipoCargo_SelectedIndexChanged(null, EventArgs.Empty);
                    break;
                case Gestion.Consultar:
                    this.btnAceptar.Visible = false;
                    this.btnAceptarContinuar.Visible = false;
                    this.ddlTipoCargo.Enabled = false;
                    this.btnImprimir.Visible = true;
                    this.ddlFormaCobro.Enabled = false;
                    this.ddlEstados.Enabled = false;
                    this.MiCarCargosAfilados = CargosF.TiposCargosAfiliadosObtenerDatosCompletos(pCargoAfiliado);
                    this.CargarComboFormasCobros();
                    this.ddlEstados.Items.Remove(this.ddlEstados.Items.FindByValue(((int)EstadosCargos.Activo).ToString()));
                    this.ddlEstados.Items.Remove(this.ddlEstados.Items.FindByValue(((int)EstadosCargos.PeriodicidadMensual).ToString()));
                    this.ddlEstados.Items.Remove(this.ddlEstados.Items.FindByValue(((int)EstadosCargos.PeriodicidadAnual).ToString()));
                    this.MapearObjetoAControles(this.MiCarCargosAfilados);
                    hdfIdTipoCargoAfiliadoFormaCobro.Value = MiCarCargosAfilados.IdTipoCargoAfiliadoFormaCobro.ToString();
                    hdfIdTipoCargo.Value = MiCarCargosAfilados.TipoCargo.IdTipoCargo.ToString();
                    SetInitializeCulture(MiCarCargosAfilados.Moneda.Moneda);
                    if (this.MiCarCargosAfilados.TipoCargo.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.Turismo)
                    {
                        dvImporteCargo.Visible = false;
                        dvPorcentaje.Visible = false;
                        dvImporteInteres.Visible = false;
                    }
                    break;
                default:
                    break;
            }
        }

        protected void btnAceptarContinuar_Click(object sender, EventArgs e)
        {
            this.AceptarContinuar = true;
            this.btnAceptar_Click(sender, e);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.btnAceptar.Visible = false;
            this.btnAceptarContinuar.Visible = false;

            this.MapearControlesAObjeto(this.MiCarCargosAfilados);
            this.MiCarCargosAfilados.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiCarCargosAfilados.UsuarioAlta.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    guardo = CargosF.TiposCargosAfiliadosAgregar(this.MiCarCargosAfilados);

                    if (guardo && this.MiCarCargosAfilados.Estado.IdEstado == (int)EstadosCargos.Pendiente)
                        this.MiCarCargosAfilados.CodigoMensaje = "ResultadoCargoPendiente";

                    break;
                case Gestion.Modificar:
                    guardo = CargosF.TiposCargosAfiliadosModificar(this.MiCarCargosAfilados);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                
                if (this.AceptarContinuar)
                {
                    this.btnAceptar.Visible = true;
                    this.btnAceptarContinuar.Visible = true;
                    this.btnImprimir.Visible = true;

                    if (GestionControl == Gestion.Agregar)
                    {
                        AceptarContinuar = true;
                        this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiCarCargosAfilados.CodigoMensaje), false);
                    }
                    else if (MiCarCargosAfilados.Estado.IdEstado == (int)EstadosCargos.Baja)
                    {
                        MostrarMensaje(this.ObtenerMensajeSistema(this.MiCarCargosAfilados.CodigoMensaje), false);
                        this.btnImprimir.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DeshabilitarControlesScript", "$(document).ready(function () { deshabilitarControles('deshabilitarControles') }); ;", true);

                        btnAceptar.Visible = false;
                        btnAceptarContinuar.Visible = false;
                    }
                    else
                    {
                        MostrarMensaje(this.ObtenerMensajeSistema(this.MiCarCargosAfilados.CodigoMensaje), false);
                        this.IniciarControl(this.MiCarCargosAfilados, Gestion.Modificar);
                        this.upTipoCargo.Update();
                    }
                }
                else
                {
                    MostrarMensaje(this.ObtenerMensajeSistema(this.MiCarCargosAfilados.CodigoMensaje), false);
                    this.btnImprimir.Visible = true;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DeshabilitarControlesScript", "$(document).ready(function () { deshabilitarControles('deshabilitarControles') }); ;", true);

                    btnAceptar.Visible = false;
                    btnAceptarContinuar.Visible = false;
                }

            }
            else
            {
                this.btnAceptar.Visible = true;
                this.btnAceptarContinuar.Visible = true;
                this.MostrarMensaje(this.MiCarCargosAfilados.CodigoMensaje, true, this.MiCarCargosAfilados.CodigoMensajeArgs);
            }
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            MiCarCargosAfilados.Filtro = MiCarCargosAfilados.IdTipoCargoAfiliadoFormaCobro.ToString();
            TGEPlantillas miPlantilla = TGEGeneralesF.PlantillasObtenerDatosPorTipoOperacion(MiCarCargosAfilados);
            MiCarCargosAfilados.Filtro = String.Empty;
            if (miPlantilla.Codigo.Trim() == String.Empty)
                miPlantilla.Codigo = "CarTiposCargosAfiliadosFormasCobros";
            //this.ctrPopUpComprobantes.CargarReporte(this.MiOrdenPago, EnumTGEComprobantes.CapOrdenesPagos);
            byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CarCargosAfiliadosFormasCobros, miPlantilla.Codigo, this.MiCarCargosAfilados, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            ExportPDF.ExportarPDF(pdf, this.Page, "Cargo", this.UsuarioActivo);
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.CargoAfiliadoModificarDatosCancelar != null)
                this.CargoAfiliadoModificarDatosCancelar();
        }

        protected void ddlTipoCargo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pnlCargoReferencia.Visible = false;
            this.rfvCargoReferencia.Enabled = false;
            dvImporteCargo.Visible = true;
            dvPorcentaje.Visible = true;
            dvImporteInteres.Visible = true;
            rfvPorcentaje.Enabled = false;
            ddlEstados.Enabled = true;
            if (!string.IsNullOrEmpty(this.ddlTipoCargo.SelectedValue))
            {
                this.MiCarCargosAfilados.TipoCargo.IdTipoCargo = Convert.ToInt32(this.ddlTipoCargo.SelectedValue);
                this.MiCarCargosAfilados.TipoCargo = CargosF.TiposCargosObtenerDatosCompletos(this.MiCarCargosAfilados.TipoCargo);

                if (this.MiCarCargosAfilados.TipoCargo.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.Administrable)
                {
                    this.txtImporteCargo.Enabled = this.MiCarCargosAfilados.TipoCargo.CargoIrregular;
                    this.txtTasaInteres.Enabled = this.MiCarCargosAfilados.TipoCargo.CargoIrregular;
                    this.txtTasaInteres.Decimal = Convert.ToDecimal(TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.TasaDeInteres).ParametroValor);
                    hdfTasaInteres.Value = txtTasaInteres.Decimal.ToString().Replace(",", ".");
                    this.txtPorcentaje.Enabled = false;
                    this.txtPorcentaje.Text = string.Empty;
                }
                else if (this.MiCarCargosAfilados.TipoCargo.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.Turismo)
                {
                    this.txtTasaInteres.Decimal = 0;
                    hdfTasaInteres.Value = "0";
                    this.txtTasaInteres.Enabled = false;
                    dvImporteCargo.Visible = false;
                    dvPorcentaje.Visible = false;
                    dvImporteInteres.Visible = false;
                    ddlEstados.SelectedValue = ((int)EstadosCargos.Activo).ToString();
                    ddlEstados.Enabled = false;
                }
                else if (this.MiCarCargosAfilados.TipoCargo.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.Informativo)
                {
                    this.txtTasaInteres.Decimal = 0;
                    hdfTasaInteres.Value = "0";
                    this.txtTasaInteres.Enabled = false;
                    dvImporteCargo.Visible = false;
                    dvPorcentaje.Visible = false;
                    dvImporteInteres.Visible = false;
                    ddlEstados.SelectedValue = ((int)EstadosCargos.Activo).ToString();
                    ddlEstados.Enabled = false;
                }
                else if (this.MiCarCargosAfilados.TipoCargo.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.Bonificacion)
                {
                    this.pnlCargoReferencia.Visible = true;
                    this.rfvCargoReferencia.Enabled = true;
                    this.txtImporteCargo.Decimal = 0;
                    hdfImporteCargo.Value = "0";
                    this.txtImporteCargo.Enabled = true;
                    this.txtPorcentaje.Decimal = MiCarCargosAfilados.TipoCargo.Porcentaje;
                    this.hdfPorcentaje.Value = MiCarCargosAfilados.TipoCargo.Porcentaje.ToString().Replace(",", ".");
                    this.txtPorcentaje.Enabled = true;
                    this.txtTasaInteres.Decimal = 0;
                    hdfTasaInteres.Value = "0";
                    this.txtTasaInteres.Enabled = false;
                    
                    CarTiposCargosAfiliadosFormasCobros afiFiltro = new CarTiposCargosAfiliadosFormasCobros();
                    afiFiltro.IdAfiliado = this.MiCarCargosAfilados.IdAfiliado;
                    afiFiltro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    afiFiltro.TipoCargo = this.MiCarCargosAfilados.TipoCargo;
                    //afiFiltro.TablaReferenciaRegistro = "";
                    this.MisCarCargosABonificar = CargosF.TiposCargosAfiliadosObtenerABonificar(afiFiltro);
                    this.ddlCargoReferencia.DataSource = this.MisCarCargosABonificar;
                    this.ddlCargoReferencia.DataValueField = "Filtro";
                    this.ddlCargoReferencia.DataTextField = "Detalle";
                    this.ddlCargoReferencia.DataBind();
                    AyudaProgramacion.InsertarItemSeleccione(this.ddlCargoReferencia, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                }
                else if (this.MiCarCargosAfilados.TipoCargo.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.AdministrablePorcentaje)
                {
                    txtImporteCargo.Enabled = false;
                    txtTasaInteres.Decimal = 0;
                    hdfTasaInteres.Value = "0";
                    txtPorcentaje.Enabled = true;
                    rfvPorcentaje.Enabled = true;
                    txtPorcentaje.Enabled = true;
                }

                this.txtCantidadCuotas.Enabled = this.MiCarCargosAfilados.TipoCargo.PermiteCuotas;
                this.txtCantidadCuotas.Text = (1).ToString();
                hdfCantidadCuotas.Value = (1).ToString();
                this.txtImporteCargo.Text = this.MiCarCargosAfilados.TipoCargo.Importe.ToString("C2");
                hdfImporteCargo.Value = MiCarCargosAfilados.TipoCargo.Importe.ToString().Replace(",", ".");
                txtImporteCuota.Text = MiCarCargosAfilados.TipoCargo.Importe.ToString("C2");
                                
                decimal tasa = (this.txtTasaInteres.Text == string.Empty ? 0 : this.txtTasaInteres.Decimal);
                this.MiCarCargosAfilados.ImporteInteres = Math.Round((this.MiCarCargosAfilados.TipoCargo.Importe * 1 * tasa / 100),2); 
                this.txtImporteInteres.Text = this.MiCarCargosAfilados.ImporteInteres.Value.ToString("C2");
                this.MiCarCargosAfilados.ImporteTotal= this.MiCarCargosAfilados.TipoCargo.Importe + this.MiCarCargosAfilados.ImporteInteres.Value;
                this.MiCarCargosAfilados.ImporteCuota = this.MiCarCargosAfilados.ImporteTotal;
                this.txtImporteTotal.Text = this.MiCarCargosAfilados.ImporteTotal.ToString("C2");
                
                this.CargarComboFormasCobros();

                this.MiCarCargosAfilados.FechaAlta = Convert.ToDateTime(this.txtFechaAlta.Text);
                this.ctrCamposValores.IniciarControl(this.MiCarCargosAfilados, this.MiCarCargosAfilados.TipoCargo, this.GestionControl);
                this.pnlCamposValores.Visible = this.ctrCamposValores.MostrarControl;
            }
            else
            {
                this.ddlFormaCobro.Items.Clear();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlFormaCobro, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                this.txtImporteCargo.Text = string.Empty;
                this.txtPorcentaje.Enabled = false;
                this.txtPorcentaje.Text = string.Empty;
            }

            this.ddlFormaCobro_SelectedIndexChanged(null, EventArgs.Empty);
        }

        protected void ddlFormaCobro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlFormaCobro.SelectedValue))
            {
                this.MiCarCargosAfilados.FormaCobroAfiliado.FormaCobro.IdFormaCobro = this.MisFormasCobros[this.ddlFormaCobro.SelectedIndex].FormaCobro.IdFormaCobro;
                this.ctrCamposValores.IniciarControl(this.MiCarCargosAfilados, this.MiCarCargosAfilados.FormaCobroAfiliado.FormaCobro, this.GestionControl);
                this.pnlCamposValores.Visible = this.ctrCamposValores.MostrarControl;
                this.MostrarDetalleFormacobro();
            }
        }

        protected void ddlMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlMoneda.SelectedValue))
            {
                this.MiCarCargosAfilados.Moneda = this.MisMonedas.First(x => x.IdMoneda == Convert.ToInt32(ddlMoneda.SelectedValue));
                SetInitializeCulture(MiCarCargosAfilados.Moneda.Moneda);
            }
        }


        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.GestionControl == Gestion.Agregar && AceptarContinuar)
            {

                this.MisParametrosUrl = new Hashtable();
                this.MisParametrosUrl.Add("IdTipoCargoAfiliadoFormaCobro", MiCarCargosAfilados.IdTipoCargoAfiliadoFormaCobro);
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosModificar.aspx"), true);

            }
            else if (MiCarCargosAfilados.Estado.IdEstado == (int)EstadosCargos.Baja)
            {
                this.MisParametrosUrl = new Hashtable();
                this.MisParametrosUrl.Add("IdTipoCargoAfiliadoFormaCobro", MiCarCargosAfilados.IdTipoCargoAfiliadoFormaCobro);
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosConsultar.aspx"), true);
            }

            if (this.CargoAfiliadoModificarDatosAceptar != null)
                this.CargoAfiliadoModificarDatosAceptar(null, this.MiCarCargosAfilados);
        }

        private void CargarCombos()
        {
            CarTiposCargosAfiliadosFormasCobros afiFiltro = new CarTiposCargosAfiliadosFormasCobros();
            afiFiltro.IdAfiliado = this.MiCarCargosAfilados.IdAfiliado;
            afiFiltro.Estado.IdEstado = (int)Estados.Activo;
            afiFiltro.Filtro = string.Concat((int)EnumTiposCargosProcesos.Administrable, ",", (int)EnumTiposCargosProcesos.Turismo, ",",
              (int)EnumTiposCargosProcesos.Bonificacion, ",", (int)EnumTiposCargosProcesos.AdministrablePorcentaje,
                ",", (int)EnumTiposCargosProcesos.Informativo);
            afiFiltro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            List<CarTiposCargos> lista = CargosF.TiposCargosObtenerListaActiva(afiFiltro);
            this.ddlTipoCargo.DataSource = lista;
            //afiFiltro.TipoCargo.TipoCargoProceso.IdTipoCargoProceso = (int)EnumTiposCargosProcesos.Administrable;
            //List<CarTiposCargos> lista = CargosF.TiposCargosObtenerListaActiva(afiFiltro);
            //afiFiltro.TipoCargo.TipoCargoProceso.IdTipoCargoProceso = (int)EnumTiposCargosProcesos.Turismo;
            //lista.AddRange(CargosF.TiposCargosObtenerListaActiva(afiFiltro));
            //afiFiltro.TipoCargo.TipoCargoProceso.IdTipoCargoProceso = (int)EnumTiposCargosProcesos.Bonificacion;
            //lista.AddRange( CargosF.TiposCargosObtenerListaActiva(afiFiltro));
            //afiFiltro.TipoCargo.TipoCargoProceso.IdTipoCargoProceso = (int)EnumTiposCargosProcesos.AdministrablePorcentaje;
            //lista.AddRange(CargosF.TiposCargosObtenerListaActiva(afiFiltro));
            //this.ddlTipoCargo.DataSource = AyudaProgramacion.AcomodarIndices<CarTiposCargos>( lista).OrderBy(X=>X.TipoCargo).ToList();
            this.ddlTipoCargo.DataValueField = "IdTipoCargo";
            this.ddlTipoCargo.DataTextField = "TipoCargo";
            this.ddlTipoCargo.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoCargo, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            AyudaProgramacion.AgregarItemSeleccione(this.ddlFormaCobro, this.ObtenerMensajeSistema("SeleccioneOpcion"));


            MisMonedas = TGEGeneralesF.MonedasCotizacionesObtenerVentaCombo();
            this.ddlMoneda.DataSource = MisMonedas;
            this.ddlMoneda.DataValueField = "IdMoneda";
            this.ddlMoneda.DataTextField = "MonedaDescripcionCotizacion";
            this.ddlMoneda.DataBind();
            //if (this.ddlMoneda.Items.Count != 1)
            //    AyudaProgramacion.InsertarItemSeleccione(this.ddlMoneda, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            ddlMoneda_SelectedIndexChanged(null, EventArgs.Empty);
        }

        private void CargarComboEstados()
        {
            List<TGEEstados> MisEstados = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosCargos));
            this.ddlEstados.DataSource = MisEstados;
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.SelectedValue = ((int)EstadosCargos.Activo).ToString();
            this.ddlEstados.Items.Remove(this.ddlEstados.Items.FindByValue(((int)EstadosCargos.Facturado).ToString()));
            this.ddlEstados.Items.Remove(this.ddlEstados.Items.FindByValue(((int)EstadosCargos.Facturandose).ToString()));
         
        }

        private void CargarComboFormasCobros()
        {
            this.ddlFormaCobro.SelectedIndex = -1;
            this.ddlFormaCobro.SelectedValue = null;
            this.ddlFormaCobro.Items.Clear();
            this.ddlFormaCobro.ClearSelection();

            TGEFormasCobrosAfiliados formasCobroAfi = new TGEFormasCobrosAfiliados();
            formasCobroAfi.IdAfiliado = this.MiCarCargosAfilados.IdAfiliado;
            formasCobroAfi.IdTipoCargo = this.MiCarCargosAfilados.TipoCargo.IdTipoCargo;
            this.MisFormasCobros = TGEGeneralesF.FormasCobrosAfiliadosObtenerPorAfiliadoTipoCargo(formasCobroAfi);
            
            if (this.MisFormasCobros.Count > 0)
            {
                //formasCobroAfi.IdTipoCargo = this.MiCarCargosAfilados.TipoCargo.IdTipoCargo;
                this.ddlFormaCobro.DataSource = this.MisFormasCobros;
                this.ddlFormaCobro.DataValueField = "IdFormaCobroAfiliado";
                this.ddlFormaCobro.DataTextField = "FormaCobroDescripcion";
                this.ddlFormaCobro.DataBind();
            }
            else
            {
                this.MostrarMensaje("ValidarFormasCobroAfiliados", true);
                AyudaProgramacion.AgregarItemSeleccione(this.ddlFormaCobro, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
        }

        private void MapearControlesAObjeto(CarTiposCargosAfiliadosFormasCobros pCargosAfiliados)
        {
            pCargosAfiliados.FechaAlta = Convert.ToDateTime(this.txtFechaAlta.Text);
            pCargosAfiliados.TipoCargo.IdTipoCargo = Convert.ToInt32(this.ddlTipoCargo.SelectedValue);
            pCargosAfiliados.TipoCargo.TipoCargo = this.ddlTipoCargo.SelectedItem.Text;
            pCargosAfiliados.FormaCobroAfiliado.IdFormaCobroAfiliado = Convert.ToInt32(this.ddlFormaCobro.SelectedValue);
            pCargosAfiliados.FormaCobroAfiliado.FormaCobro.FormaCobro = this.ddlFormaCobro.SelectedItem.Text;
            pCargosAfiliados.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pCargosAfiliados.Estado.Descripcion = this.ddlEstados.SelectedItem.Text;
            pCargosAfiliados.ImporteCargo = Convert.ToDecimal( hdfImporteCargo.Value.Replace(".",",")); // decimal.Parse(this.txtImporteCargo.Text, NumberStyles.Currency);
            pCargosAfiliados.CantidadCuotas = Convert.ToInt32(this.txtCantidadCuotas.Text);
            pCargosAfiliados.TasaInteres = this.txtTasaInteres.Decimal;
            pCargosAfiliados.Porcentaje = this.hdfPorcentaje.Value == string.Empty ? 0 : this.txtPorcentaje.Decimal;// Convert.ToDecimal(this.hdfPorcentaje.Value);// this.txtPorcentaje.Decimal;
            pCargosAfiliados.Moneda.IdMoneda = Convert.ToInt32(this.ddlMoneda.SelectedValue);

            /* Comentado el 11/11/2022 porque se cambian los valores por javascript desde los campos dinamicos */
            //if ((pCargosAfiliados.TipoCargo.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.Administrable
            //    || pCargosAfiliados.TipoCargo.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.AdministrableCargoFijo
            //    || pCargosAfiliados.TipoCargo.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.Turismo)
            //    && !pCargosAfiliados.CargoFacturado)
            //{
                pCargosAfiliados.ImporteInteres = Math.Round((pCargosAfiliados.ImporteCargo.Value * pCargosAfiliados.CantidadCuotas * pCargosAfiliados.TasaInteres.Value / 100), 2);
                pCargosAfiliados.ImporteTotal = pCargosAfiliados.ImporteCargo.Value + pCargosAfiliados.ImporteInteres.Value;
                pCargosAfiliados.ImporteCuota = Math.Round((pCargosAfiliados.ImporteTotal / pCargosAfiliados.CantidadCuotas), 2);
            //}
            
            if (pCargosAfiliados.TipoCargo.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.Bonificacion)
            {
                if (!pCargosAfiliados.CargoFacturado)
                {
                    pCargosAfiliados.ImporteInteres = 0;
                    pCargosAfiliados.TasaInteres = 0;
                    pCargosAfiliados.ImporteTotal = this.txtImporteTotal.Decimal;// + pCargosAfiliados.ImporteInteres.Value;
                    pCargosAfiliados.ImporteCuota = this.txtImporteCuota.Decimal;//Math.Round((pCargosAfiliados.ImporteTotal / pCargosAfiliados.CantidadCuotas), 2);
                }
                if (!string.IsNullOrEmpty(this.ddlCargoReferencia.SelectedValue))
                {
                    CarTiposCargosAfiliadosFormasCobros bonificar = this.MisCarCargosABonificar.FirstOrDefault(x => x.Filtro == this.ddlCargoReferencia.SelectedValue);
                    pCargosAfiliados.ListaValorDetalle.IdListaValorDetalle = bonificar.IdReferenciaRegistro;
                    pCargosAfiliados.ListaValorDetalle.Descripcion = bonificar.Detalle;
                    pCargosAfiliados.Detalle = bonificar.Detalle;
                    pCargosAfiliados.IdReferenciaRegistro = pCargosAfiliados.ListaValorDetalle.IdListaValorDetalle;
                    pCargosAfiliados.TablaReferenciaRegistro = bonificar.TablaReferenciaRegistro;
                    pCargosAfiliados.IdAfiliadoRef = bonificar.IdAfiliadoRef;
                }
            }
            pCargosAfiliados.Campos = this.ctrCamposValores.ObtenerLista();
            pCargosAfiliados.LoteCamposValores = this.ctrCamposValores.ObtenerListaCamposValores();

            pCargosAfiliados.Comentarios = ctrComentarios.ObtenerLista();
            pCargosAfiliados.Archivos = ctrArchivos.ObtenerLista();

            TGEMonedas moneda = MisMonedas.FirstOrDefault(x => x.IdMoneda == Convert.ToInt32(this.ddlMoneda.SelectedValue));
            if (moneda.IdMoneda > 0)
            {
                pCargosAfiliados.Moneda = moneda;
                pCargosAfiliados.MonedaCotizacion = moneda.MonedeaCotizacion.MonedaCotizacion;
            }
        }

        private void MapearObjetoAControles(CarTiposCargosAfiliadosFormasCobros pCargosAfiliados)
        {
            ListItem item = this.ddlFormaCobro.Items.FindByValue(pCargosAfiliados.FormaCobroAfiliado.IdFormaCobroAfiliado.ToString());
            if (item==null)
                this.ddlFormaCobro.Items.Add(new ListItem(pCargosAfiliados.FormaCobroAfiliado.FormaCobroDescripcion, pCargosAfiliados.FormaCobroAfiliado.IdFormaCobroAfiliado.ToString()));
            this.ddlFormaCobro.SelectedValue = pCargosAfiliados.FormaCobroAfiliado.IdFormaCobroAfiliado.ToString();

            item = this.ddlTipoCargo.Items.FindByValue(pCargosAfiliados.TipoCargo.IdTipoCargo.ToString());
            if (item==null)
                this.ddlTipoCargo.Items.Add(new ListItem(pCargosAfiliados.TipoCargo.TipoCargo, pCargosAfiliados.TipoCargo.IdTipoCargo.ToString()));
            this.ddlTipoCargo.SelectedValue = pCargosAfiliados.TipoCargo.IdTipoCargo.ToString();

            this.txtFechaAlta.Text = pCargosAfiliados.FechaAlta.ToShortDateString();
            this.txtImporteCuota.Text = pCargosAfiliados.ImporteCuota.ToString("C2");
            this.txtCantidadCuotas.Text = pCargosAfiliados.CantidadCuotas.ToString();
            this.txtImporteTotal.Text = pCargosAfiliados.ImporteTotal.ToString("C2");
            this.txtTasaInteres.Text = pCargosAfiliados.TasaInteres.HasValue ? pCargosAfiliados.TasaInteres.Value.ToString("N2") : (0).ToString("N2");
            this.txtImporteInteres.Text = pCargosAfiliados.ImporteInteres.HasValue ? pCargosAfiliados.ImporteInteres.Value.ToString("C2") : (0).ToString("C2");
            this.txtImporteCargo.Text = pCargosAfiliados.ImporteCargo.HasValue? pCargosAfiliados.ImporteCargo.Value.ToString("C2") : (0).ToString("C2");
            this.txtPorcentaje.Text = pCargosAfiliados.Porcentaje.HasValue ? pCargosAfiliados.Porcentaje.Value.ToString("N2") : (0).ToString("N2");

            hdfImporteCargo.Value = pCargosAfiliados.ImporteCargo.ToString();
            hdfCantidadCuotas.Value = pCargosAfiliados.CantidadCuotas.ToString();
            hdfTasaInteres.Value = pCargosAfiliados.TasaInteres.HasValue ? pCargosAfiliados.TasaInteres.Value.ToString() : (0).ToString();
            hdfPorcentaje.Value = pCargosAfiliados.Porcentaje.HasValue ? pCargosAfiliados.Porcentaje.Value.ToString() : (0).ToString();

            item = this.ddlEstados.Items.FindByValue(pCargosAfiliados.Estado.IdEstado.ToString());
            if (item == null)
                this.ddlEstados.Items.Add(new ListItem(pCargosAfiliados.Estado.Descripcion, pCargosAfiliados.Estado.IdEstado.ToString()));
            this.ddlEstados.SelectedValue = pCargosAfiliados.Estado.IdEstado.ToString();

            this.ctrCamposValores.BorrarControlesParametros();/*No eliminar por turismo*/
            this.ctrCamposValores.IniciarControl(this.MiCarCargosAfilados, this.MiCarCargosAfilados.TipoCargo, this.GestionControl);
            this.ctrCamposValores.IniciarControl(this.MiCarCargosAfilados, this.MiCarCargosAfilados.FormaCobroAfiliado.FormaCobro, this.GestionControl);
            this.pnlCamposValores.Visible = this.ctrCamposValores.MostrarControl;

            if (pCargosAfiliados.TipoCargo.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.Bonificacion)
            {
                this.pnlCargoReferencia.Visible = true;
                CarTiposCargosAfiliadosFormasCobros afiFiltro = new CarTiposCargosAfiliadosFormasCobros();
                afiFiltro.IdAfiliado = pCargosAfiliados.IdAfiliado;
                afiFiltro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                this.MisCarCargosABonificar = CargosF.TiposCargosAfiliadosObtenerABonificar(afiFiltro);
                this.ddlCargoReferencia.DataSource = this.MisCarCargosABonificar;
                this.ddlCargoReferencia.DataValueField = "Filtro";
                this.ddlCargoReferencia.DataTextField = "Detalle";
                this.ddlCargoReferencia.DataBind();
                string filtro = string.Concat(pCargosAfiliados.IdReferenciaRegistro.ToString(), "|", pCargosAfiliados.IdAfiliadoRef.HasValue ? pCargosAfiliados.IdAfiliadoRef.Value : 0);
                item = this.ddlCargoReferencia.Items.FindByValue(filtro);
                if (item==null)
                    this.ddlCargoReferencia.Items.Add(new ListItem(pCargosAfiliados.ListaValorDetalle.Descripcion, filtro));
                this.ddlCargoReferencia.SelectedValue = filtro;
            }
            this.ctrComentarios.IniciarControl(pCargosAfiliados, this.GestionControl);
            this.ctrArchivos.IniciarControl(pCargosAfiliados, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pCargosAfiliados);
            MostrarDetalleFormacobro();

            if (MisMonedas.Exists(x => x.IdMoneda == pCargosAfiliados.Moneda.IdMoneda))
                MisMonedas.First(x => x.IdMoneda == pCargosAfiliados.Moneda.IdMoneda).MonedeaCotizacion.MonedaCotizacion = pCargosAfiliados.MonedaCotizacion;
            else if (pCargosAfiliados.Moneda.IdMoneda > 0)
                MisMonedas.Add(pCargosAfiliados.Moneda);
            this.ddlMoneda.Items.Clear();
            this.ddlMoneda.SelectedIndex = -1;
            this.ddlMoneda.SelectedValue = null;
            this.ddlMoneda.ClearSelection();
            this.ddlMoneda.DataSource = MisMonedas;
            this.ddlMoneda.DataValueField = "IdMoneda";
            this.ddlMoneda.DataTextField = "MonedaDescripcionCotizacion";
            this.ddlMoneda.DataBind();
            if (this.ddlMoneda.Items.Count != 1)
                AyudaProgramacion.InsertarItemSeleccione(this.ddlMoneda, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlMoneda.SelectedValue = pCargosAfiliados.Moneda.IdMoneda > 0 ? pCargosAfiliados.Moneda.IdMoneda.ToString() : string.Empty;
        }

        private void MostrarDetalleFormacobro()
        {
            List<TGECampos> lista = TGEGeneralesF.CamposObtenerListaFiltro(this.MiCarCargosAfilados.FormaCobroAfiliado, this.MiCarCargosAfilados.FormaCobroAfiliado.FormaCobro);
            lista = lista.Where(x => x.CampoValor.IdCampoValor > 0).ToList();
            this.gvDetalleFormaCobro.DataSource = AyudaProgramacion.PivotList(lista);
            this.gvDetalleFormaCobro.DataBind();


                if (!(string.IsNullOrEmpty(this.ddlTipoCargo.SelectedValue) || string.IsNullOrEmpty(this.ddlFormaCobro.SelectedValue)))
                {
                    TGEFormasCobrosCodigosConceptosTiposCargosCategorias formaCobroCodigoConcepto = new TGEFormasCobrosCodigosConceptosTiposCargosCategorias();
                    formaCobroCodigoConcepto.IdTipoCargo = Convert.ToInt32(this.ddlTipoCargo.SelectedValue);
                    formaCobroCodigoConcepto.FormaCobro.IdFormaCobro = Convert.ToInt32(this.ddlFormaCobro.SelectedValue);
                    DataTable dtFormaCobro = TGEGeneralesF.FormasCobrosCodigosConceptosTiposCargosCategoriasObtenerListaFiltro(formaCobroCodigoConcepto);
                    this.gvFormasCobrosCodigosConceptos.DataSource = dtFormaCobro;
                    this.gvFormasCobrosCodigosConceptos.DataBind();
                }
        }

        //protected void ddlEstados_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrWhiteSpace(ddlEstados.SelectedValue))
        //    {
        //        this.MiCarCargosAfilados.Estado.IdEstado = Convert.ToInt32(ddlEstados.SelectedValue);
        //        this.ctrCamposValores.IniciarControl(this.MiCarCargosAfilados, this.MiCarCargosAfilados.Estado, this.GestionControl);
        //    }
        //}
    }
}
