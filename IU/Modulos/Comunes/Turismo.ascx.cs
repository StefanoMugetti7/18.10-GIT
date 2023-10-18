using Afiliados.Entidades;
using Ahorros;
using Ahorros.Entidades;
using Cargos.Entidades;
using Compras;
using Compras.Entidades;
using Comunes.Entidades;
using CuentasPagar.Entidades;
using CuentasPagar.FachadaNegocio;
using Facturas;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Proveedores.Entidades;
using Reportes.Entidades;
using Reportes.FachadaNegocio;
using Seguridad.FachadaNegocio;
using Servicio.AccesoDatos;
using Servicio.Encriptacion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Turismo;
using Turismo.Entidades;

namespace IU.Modulos.Comunes
{
    [Serializable]
    public partial class Turismo : ControlesSeguros
    {
        #region VARIABLES DE SESION
        private DataSet MiTurismoDetalles
        {
            get { return (DataSet)Session[this.MiSessionPagina + "TurismoMiTurismoDetalles"]; }
            set { Session[this.MiSessionPagina + "TurismoMiTurismoDetalles"] = value; }
        }
        private DataTable MisTiposValoresImportes
        {
            get { return (DataTable)Session[this.MiSessionPagina + "TurismoMisTiposValoresImportes"]; }
            set { Session[this.MiSessionPagina + "TurismoMisTiposValoresImportes"] = value; }
        }
        private bool PrimeraVez
        {
            get { return (bool)Session[this.MiSessionPagina + "TurismoMisResetImportesBool"]; }
            set { Session[this.MiSessionPagina + "TurismoMisResetImportesBool"] = value; }
        }
        private string ResetImportes
        {
            get { return (string)Session[this.MiSessionPagina + "TurismoMisResetImportes"]; }
            set { Session[this.MiSessionPagina + "TurismoMisResetImportes"] = value; }
        }
        private enum TipoControl
        {
            ReservaTurismo,
            ReservaPaquetes,
        }
        private TipoControl MiControl
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "ControlTurismoTipoControl"] == null ? new TipoControl() : (TipoControl)Session[this.MiSessionPagina + this.ClientID + "ControlTurismoTipoControl"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismoTipoControl"] = value; }
        }
        private CarTiposCargosAfiliadosFormasCobros MiTablaValor
        {
            get
            {
                return (Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiTablaValor"] == null ? new CarTiposCargosAfiliadosFormasCobros() : (CarTiposCargosAfiliadosFormasCobros)Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiTablaValor"]);
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiTablaValor"] = value; }
        }
        private DataTable MiReservaPagar
        {
            get
            {
                return (Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiReservaPagar"] == null ? new DataTable() : (DataTable)Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiReservaPagar"]);
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiReservaPagar"] = value; }
        }
        private bool MiArmarParametros
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiArmarParametros"] == null ? false : (bool)Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiArmarParametros"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiArmarParametros"] = value; }
        }
        private bool MiArmarParametrosTurismo
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiArmarParametrosTurismo"] == null ? false : (bool)Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiArmarParametrosTurismo"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiArmarParametrosTurismo"] = value; }
        }
        private bool HabilitarControles
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "ControlTurismoHabilitarControles"] == null ? false : (bool)Session[this.MiSessionPagina + this.ClientID + "ControlTurismoHabilitarControles"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismoHabilitarControles"] = value; }
        }
        //private TGECampos MiCampo
        //{
        //    get
        //    {
        //        return (Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiCampo"] == null ?
        //            (TGECampos)(Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiCampo"] = new TGECampos()) : (TGECampos)Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiCampo"]);
        //    }
        //    set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiCampo"] = value; }
        //}
        private List<TGECampos> MisCamposServicios
        {
            get
            {
                return (Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMisCamposServicios"] == null ?
                    (List<TGECampos>)(Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMisCamposServicios"] = new List<TGECampos>()) : (List<TGECampos>)Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMisCamposServicios"]);
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMisCamposServicios"] = value; }
        }
        private List<TGECampos> MisCamposTurismo
        {
            get
            {
                return (Session[this.MiSessionPagina + this.ClientID + "ControlMisCamposTurismo"] == null ?
                    (List<TGECampos>)(Session[this.MiSessionPagina + this.ClientID + "ControlMisCamposTurismo"] = new List<TGECampos>()) : (List<TGECampos>)Session[this.MiSessionPagina + this.ClientID + "ControlMisCamposTurismo"]);
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlMisCamposTurismo"] = value; }
        }
        private DataTable MiCuentaCorriente
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ReservasDatosMiCuentaCorriente"]; }
            set { Session[this.MiSessionPagina + "ReservasDatosMiCuentaCorriente"] = value; }
        }
        public bool MostrarControl
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMostrarControl"] == null ? false : (bool)Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMostrarControl"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMostrarControl"] = value; }
        }
        #endregion
        #region CSS
        const string _cssLabel = "col-sm-3 col-form-label";
        const string _cssLabelCol6 = "col-lg-12 col-form-label";
        const string _cssRow = "row";
        const string _cssCol3 = "col-sm-9";
        const string _cssCol2 = "col-sm-12";
        const string _cssContainer = "col-12 col-md-8 col-lg-4";
        const string preid = "EvolCvId";
        public string cssLabel
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "ControlTurismoccsLabel"] == null ? _cssLabel : (string)Session[this.MiSessionPagina + this.ClientID + "ControlTurismoccsLabel"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismoccsLabel"] = value; }
        }
        public string cssLabelCol6
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "ControlTurismocssLabelCol6"] == null ? _cssLabelCol6 : (string)Session[this.MiSessionPagina + this.ClientID + "ControlTurismoccsLabelCol6"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismocssLabelCol6"] = value; }
        }
        public string cssRow
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "ControlTurismocssRow"] == null ? _cssRow : (string)Session[this.MiSessionPagina + this.ClientID + "ControlTurismocssRow"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismocssRow"] = value; }
        }
        public string cssCol
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "ControlTurismocssCol"] == null ? _cssCol3 : (string)Session[this.MiSessionPagina + this.ClientID + "ControlTurismocssCol"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismocssCol"] = value; }
        }
        public string ccsContainer
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "ControlTurismocssContainer"] == null ? _cssContainer : (string)Session[this.MiSessionPagina + this.ClientID + "ControlTurismocssContainer"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismocssContainer"] = value; }
        }
        #endregion
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.IsPostBack)
            {
                this.MiArmarParametrosTurismo = false;
                MisCamposTurismo = new List<TGECampos>();
            }

            if (this.MiArmarParametros)
            {
                this.ObtenerValoresParametros(this.MisCamposServicios);
                this.ArmarTablaParametros(this.pnlCamposDinamicosTurismoServicios, this.MisCamposServicios, this.HabilitarControles);
            }
            if (this.MiArmarParametrosTurismo)
            {
                this.ObtenerValoresParametros(this.MisCamposTurismo);
                this.ArmarTablaParametros(this.pnlCamposDinamicosTurismo, this.MisCamposTurismo, this.HabilitarControles);
                this.pnlPrinc.Update();
            }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            string path = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Modulos\\Comunes\\Turismo.js");
            string readText = File.ReadAllText(path);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scriptTurismo", readText, true);

            if (!this.IsPostBack)
            {

            }
        }
        public void IniciarControl(Objeto pTablaValor, TGECampos pParametro, bool pHabilitar, Gestion pGestion)
        {
            this.MiControl = TipoControl.ReservaTurismo;
            if (!string.IsNullOrEmpty(pParametro.NombreCampoDependiente))
            {
                MiControl = TipoControl.ReservaPaquetes;
            }

            this.MiTablaValor = (CarTiposCargosAfiliadosFormasCobros)pTablaValor;
            this.GestionControl = pGestion;
            this.HabilitarControles = true;
            this.MiArmarParametros = false;
            this.MiArmarParametrosTurismo = false;
            this.MisCamposServicios = new List<TGECampos>();
            this.CargarCombos();
            this.hdfIdTurismo.Value = Encriptar.EncriptarTexto(pParametro.CampoValor.Valor);
            this.btnAgregarOC.Visible = this.ValidarPermiso("OrdenesCobrosAfiliadosAgregar.aspx");
            this.btnAgregarComprobante.Visible = this.ValidarPermiso("FacturasAgregar.aspx");
            if (string.IsNullOrEmpty(pParametro.CampoValor.Valor))
            {
                TurTurismo tur = new TurTurismo();
                tur.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);

                if (pTablaValor.GetType().GetProperties().ToList().Exists(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).ToList().Count > 0))
                {
                    PropertyInfo prop = pTablaValor.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() == 1).ToList()[0];
                    tur.IdRefTablaValor = Convert.ToInt32(prop.GetValue(pTablaValor, null));
                }
                this.txtNroReserva.Text = TurismoF.TurismoObtenerProximoNumeroReserva(tur).NumeroReserva.ToString();
            }
            else
            {
                this.SetInitializeCulture(this.MiTablaValor.Moneda.Moneda);
                this.MapearDatos(pParametro.CampoValor.Valor);
            }

            if (this.GestionControl == Gestion.Modificar)
            {
                this.btnAgregarPago.Visible = true;
                this.ddlProveedoresPagos.Visible = true;
                this.lblProveedoresPagos.Visible = true;
                //BtnAgregarFactura.Visible = true;
                this.lblImporteAnticipo.Visible = true;
                this.txtImporteAnticipo.Visible = true;

                Control ctr = BuscarControlRecursivo(this.Page, "ddlEstados");
                if (ctr != null && ctr is DropDownList)
                {
                    DropDownList ddlEstdo = (DropDownList)ctr;
                    ddlEstdo.AutoPostBack = true;
                    ddlEstdo.SelectedIndexChanged += this.DdlEstdo_SelectedIndexChanged;
                }
            }
            if (pParametro.CampoValor.Valor.Contains("IdPaqueteDetalle"))
            {
                this.btnAgregar.Visible = false;
                this.txtCostoServicio.Visible = false;
                this.ddlServicio.Visible = false;
                this.lblCostoServicio.Visible = false;
                this.lblImporteServicio.Visible = false;
                this.lblServicio.Visible = false;
                this.lblProveedor.Visible = false;
                this.ddlEntidad.Visible = false;
                this.txtImporteServicio.Visible = false;
                this.btnAgregarPago.Visible = false;
                this.BtnAgregarFactura.Visible = true;
                this.btnAgregarOC.Visible = this.ValidarPermiso("OrdenesCobrosAfiliadosAgregar.aspx");
                this.btnAgregarComprobante.Visible = this.ValidarPermiso("FacturasAgregar.aspx");
                this.txtFechaSalida.Enabled = false;
                this.txtFechaRegreso.Enabled = false;
                this.Div1.Visible = false;
                this.Div2.Visible = false;
            }

            CtrTurismo turCampos = new CtrTurismo();
            this.MisCamposTurismo = TGEGeneralesF.CamposObtenerListaFiltro(turCampos, new Objeto());
            this.ArmarTablaParametros(this.pnlCamposDinamicosTurismo, this.MisCamposTurismo, this.HabilitarControles);
            this.MiArmarParametrosTurismo = this.MisCamposTurismo.Count > 0;
            PaginaAfiliados paginaAfiliados = new PaginaAfiliados();
            AfiAfiliados afi = paginaAfiliados.Obtener(this.MiSessionPagina);

            if (afi.IdAfiliado > 0)
            {
                string idTipoCargoAfiliadoFormaCobro = Request.Form["ctl00$ctl00$ContentPlaceHolder1$cphPrincipal$ModificarDatos$hdfIdTipoCargoAfiliadoFormaCobro"];
                afi.IdRefTabla = this.MiTablaValor.IdTipoCargoAfiliadoFormaCobro;
                afi.Tabla = typeof(CarTiposCargosAfiliadosFormasCobros).Name;
                this.MiCuentaCorriente = FacturasF.CuentasCorrientesSeleccionarPorClienteIdRefTablaTable(afi);
                this.gvCuentaCorriente.DataSource = this.MiCuentaCorriente;
                this.gvCuentaCorriente.DataBind();
                this.MisTiposValoresImportes = FacturasF.CuentasCorrientesSeleccionarTiposValoresImportesPorClienteIdRefTablaTable(afi);
                this.gvValores.DataSource = this.MisTiposValoresImportes;
                this.gvValores.DataBind();
            }
            CapOrdenesPagos pOrdenPago = new CapOrdenesPagos();
            pOrdenPago.IdRefTipoOperacion = MiTablaValor.IdTipoCargoAfiliadoFormaCobro;
            pOrdenPago.Filtro = typeof(CarTiposCargosAfiliadosFormasCobros).Name;
            this.gvPagos.DataSource = CuentasPagarF.OrdenesPagosObtenerListaFiltroGrillaTurismo(pOrdenPago);
            this.gvPagos.DataBind();
            this.gvPagosValores.DataSource = CuentasPagarF.OrdenesPagosObtenerListaFiltroPagoTurismo(pOrdenPago);
            this.gvPagosValores.DataBind();

            if (this.MiTablaValor.Estado.IdEstado == (int)EstadosCargos.Baja)
                this.HabilitarReintegro(true);
            else
                this.HabilitarReintegro(false);

            if (string.IsNullOrEmpty(pParametro.NombreCampoDependiente))
            {
                this.BtnAgregarFactura.Visible = true;
            }
            this.pnlPrinc.Update();
        }
        private void DdlEstdo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(((DropDownList)sender).SelectedValue))
            {
                if (Convert.ToInt32(((DropDownList)sender).SelectedValue) == (int)EstadosCargos.Baja)
                    HabilitarReintegro(true);
                else
                    HabilitarReintegro(false);
            }
        }
        private void HabilitarReintegro(bool estado)
        {
            this.dvImporteReintegrar.Visible = estado;
            this.dvCuentaAhorro.Visible = estado;
            this.dvCostoCancelacion.Visible = estado;
            if (estado)
            {
                AhoCuentas cuenta = new AhoCuentas();
                cuenta.CuentaTipo.IdCuentaTipo = (int)EnumAhorrosCuentasTipos.CajaAhorro;
                cuenta.Estado.IdEstado = (int)EstadosAhorrosCuentas.CuentaAbierta;
                cuenta.Moneda.IdMoneda = (int)EnumTGEMonedas.PesosArgentinos;
                string idAfiliado = Request.Form["ctl00$ctl00$ContentPlaceHolder1$cphPrincipal$ModificarDatos$hdfIdAfiliado"];
                cuenta.Afiliado.IdAfiliado = idAfiliado == string.Empty ? 0 : Convert.ToInt32(idAfiliado);

                this.ddlCuentasAhorros.Items.Clear();
                this.ddlCuentasAhorros.SelectedIndex = -1;
                this.ddlCuentasAhorros.SelectedValue = null;
                this.ddlCuentasAhorros.ClearSelection();

                this.ddlCuentasAhorros.DataSource = AhorroF.CuentasObtenerListaFiltro(cuenta);
                this.ddlCuentasAhorros.DataValueField = "IdCuenta";
                this.ddlCuentasAhorros.DataTextField = "CuentaDatos";
                this.ddlCuentasAhorros.DataBind();
                if (this.ddlCuentasAhorros.Items.Count != 1)
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlCuentasAhorros, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
        }
        public void IniciarControl(Objeto pTablaValor, TGECampos pParametro, bool pHabilitar, Gestion pGestion, int IdPaquete)
        {
            TurPaquetes aux = new TurPaquetes()
            {
                IdPaquete = IdPaquete,
            };
            this.PrimeraVez = true;
            this.IniciarControl(pTablaValor, aux, pGestion, IdPaquete);
        }
        public void IniciarControl(Objeto pTablaValor, TurPaquetes pParametro, Gestion pGestion, int? IdPaquete)
        {
            this.MiControl = TipoControl.ReservaPaquetes;
            this.GestionControl = pGestion;
            this.CargarCombos();
            this.HabilitarControles = true;
            this.MiArmarParametros = false;
            this.MiArmarParametrosTurismo = false;
            this.MisCamposServicios = new List<TGECampos>();
            this.MiTablaValor = new CarTiposCargosAfiliadosFormasCobros();
            TurPaquetesDetalles turCampos = new TurPaquetesDetalles();
            this.MisCamposTurismo = TGEGeneralesF.CamposObtenerListaFiltro(turCampos, new Objeto());
            this.ArmarTablaParametros(pnlCamposDinamicosTurismoServicios, this.MisCamposTurismo, this.HabilitarControles);
            this.MiArmarParametrosTurismo = this.MisCamposTurismo.Count > 0;
            switch (this.GestionControl)
            {
                case Gestion.Consultar:
                    pParametro = TurismoF.PaquetesObtenerDatosCompletos(pParametro);
                    SetInitializeCulture(pParametro.Moneda.Moneda);
                    #region OCULTO CONTROLES 
                    this.btnAgregar.Visible = false;
                    this.txtCostoServicio.Visible = false;
                    this.ddlServicio.Visible = false;
                    this.lblCostoServicio.Visible = false;
                    this.lblImporteServicio.Visible = false;
                    this.lblServicio.Visible = false;
                    this.lblProveedor.Visible = false;
                    this.ddlEntidad.Visible = false;
                    this.txtImporteServicio.Visible = false;
                    this.BtnAgregarFactura.Visible = true;
                    this.btnAgregarOC.Visible = this.ValidarPermiso("OrdenesCobrosAfiliadosAgregar.aspx");
                    this.btnAgregarComprobante.Visible = this.ValidarPermiso("FacturasAgregar.aspx");
                    this.txtFechaSalida.Enabled = false;
                    this.txtFechaRegreso.Enabled = false;
                    this.Div1.Visible = false;
                    this.Div2.Visible = false;
                    string xml;
                    xml = this.ObtenerListaCamposValores(pParametro.Detalles, this.MisCamposTurismo).InnerXml;
                    this.MapearDatos(xml);
                    this.hdfIdTurismo.Value = Encriptar.EncriptarTexto(xml);
                    TurTurismo tur = new TurTurismo();
                    tur.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);

                    if (pTablaValor.GetType().GetProperties().ToList().Exists(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).ToList().Count > 0))
                    {
                        PropertyInfo prop = pTablaValor.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() == 1).ToList()[0];
                        tur.IdRefTablaValor = Convert.ToInt32(prop.GetValue(pTablaValor, null));
                    }
                    txtNroReserva.Text = TurismoF.TurismoObtenerProximoNumeroReserva(tur).NumeroReserva.ToString();
                    #endregion
                    break;
                default:
                    break;
            }
            PaginaAfiliados paginaAfiliados = new PaginaAfiliados();
            AfiAfiliados afi = paginaAfiliados.Obtener(MiSessionPagina);
            if (afi.IdAfiliado > 0)
            {
                string idTipoCargoAfiliadoFormaCobro = Request.Form["ctl00$ctl00$ContentPlaceHolder1$cphPrincipal$ModificarDatos$hdfIdTipoCargoAfiliadoFormaCobro"];
                afi.IdRefTabla = MiTablaValor.IdTipoCargoAfiliadoFormaCobro;
                afi.Tabla = typeof(CarTiposCargosAfiliadosFormasCobros).Name;
                this.MiCuentaCorriente = FacturasF.CuentasCorrientesSeleccionarPorClienteIdRefTablaTable(afi);
                this.gvCuentaCorriente.DataSource = this.MiCuentaCorriente;
                this.gvCuentaCorriente.DataBind();
                this.MisTiposValoresImportes = FacturasF.CuentasCorrientesSeleccionarTiposValoresImportesPorClienteIdRefTablaTable(afi);
                this.gvValores.DataSource = this.MisTiposValoresImportes;
                this.gvValores.DataBind();
            }

            CapOrdenesPagos pOrdenPago = new CapOrdenesPagos();
            pOrdenPago.IdRefTipoOperacion = MiTablaValor.IdTipoCargoAfiliadoFormaCobro;
            pOrdenPago.Filtro = typeof(CarTiposCargosAfiliadosFormasCobros).Name;
            this.gvPagos.DataSource = CuentasPagarF.OrdenesPagosObtenerListaFiltroGrillaTurismo(pOrdenPago);
            this.gvPagos.DataBind();
            this.gvPagosValores.DataSource = CuentasPagarF.OrdenesPagosObtenerListaFiltroPagoTurismo(pOrdenPago);
            this.gvPagosValores.DataBind();

            this.pnlPrinc.Update();
        }
        private void CargarCombos()
        {
            this.ddlServicio.Items.Clear();
            this.ddlServicio.SelectedIndex = -1;
            this.ddlServicio.SelectedValue = null;
            this.ddlServicio.ClearSelection();

            CMPProductos pParametro = new CMPProductos();
            this.ddlServicio.DataSource = ComprasF.ObtenerProductosServiciosTurismo(pParametro); //TurismoF.TurismoObtenerTiposServicios(new TurTurismoDetalle());
            this.ddlServicio.DataValueField = "IdProducto";
            this.ddlServicio.DataTextField = "Descripcion";
            this.ddlServicio.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlServicio, ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            TurTurismoDetalle reserva = new TurTurismoDetalle();
            string xml = Encriptar.DesencriptarTexto(hdfIdTurismo.Value);
            List<TurTurismoDetalle> MisSericios = ObtenerListaServiciosDesdeXML(xml);
            reserva.IdTurismoDetalle = (MisSericios.Count > 0 ? MisSericios.Max(x => x.IdTurismoDetalle) : 0) + 1;
            reserva.IdProveedor = Convert.ToInt32(this.hdfIdProveedor.Value);
            reserva.Proveedor = this.hdfProveedor.Value;
            reserva.IdTipoServicio = Convert.ToInt32(this.ddlServicio.SelectedValue);
            reserva.TipoServicio = this.ddlServicio.SelectedItem.Text;
            reserva.Costo = this.txtCostoServicio.Decimal;
            reserva.Importe = this.txtImporteServicio.Decimal;
            int indice = 0;
            if (!string.IsNullOrEmpty(this.hdfIdTurismoDetalle.Value)
                && MisSericios.Count > 0)
            {
                reserva.IdTurismoDetalle = Convert.ToInt32(this.hdfIdTurismoDetalle.Value);
                indice = MisSericios.IndexOf(MisSericios.Find(x => x.IdTurismoDetalle == Convert.ToInt32(this.hdfIdTurismoDetalle.Value)));
                MisSericios.Remove(MisSericios.Find(x => x.IdTurismoDetalle == Convert.ToInt32(this.hdfIdTurismoDetalle.Value)));
            }
            reserva.ServiciosCampos = this.MisCamposServicios;
            MisSericios.Insert(indice, reserva);
            xml = this.ObtenerListaCamposValores(MisSericios, this.MisCamposTurismo).InnerXml;
            this.hdfIdTurismo.Value = Encriptar.EncriptarTexto(xml);
            this.MapearDatos(xml);
            this.LimpiarControles();
            decimal total = MisSericios.Sum(x => x.Importe);// + txtImpuestoPais.Decimal + txtPercepcionRG3819.Decimal + txtPercepcionRG4815.Decimal;
            this.hdfTotalServicios.Value = total.ToString("N2").Replace(".", "").Replace(",", ".");
            string stotal = total.ToString().Replace(",", ".");
            string js = string.Format("CalcularTotalServicios({0});", stotal);
            ScriptManager.RegisterStartupScript(this.pnlPrinc, this.pnlPrinc.GetType(), "CalcularTotalServiciosScript", js, true);
            this.ddlEntidad.Items.Clear();
            this.hdfIdProveedor.Value = "";
            this.hdfProveedor.Value = "";
            this.ddlServicio.SelectedValue = "";
            this.pnlCamposDinamicosTurismoServicios.Controls.Clear();
            this.ddlEntidad.Enabled = true;
            this.ddlServicio.Enabled = true;
        }
        protected void LimpiarControles()
        {
            this.txtCostoServicio.Decimal = 0;
            this.txtImporteServicio.Decimal = 0;
        }
        protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.hdfIdTurismoDetalle.Value = "";
            this.pnlCamposDinamicosTurismoServicios.Controls.Clear();
            this.MisCamposServicios = new List<TGECampos>();
            this.HabilitarControles = true;
            if (!string.IsNullOrEmpty(this.ddlServicio.SelectedValue))
            {
                CMPProductos turParam = new CMPProductos();
                turParam.IdProducto = Convert.ToInt32(this.ddlServicio.SelectedValue);
                this.MisCamposServicios = TGEGeneralesF.CamposObtenerListaFiltro(new CtrTurismo(), turParam);
                this.ArmarTablaParametros(this.pnlCamposDinamicosTurismoServicios, this.MisCamposServicios, this.HabilitarControles);
                this.MiArmarParametros = this.MisCamposServicios.Count > 0;
            }
            this.MostrarControl = this.MisCamposServicios.Count > 0;
            this.pnlCamposDinamicosTurismoServicios.Visible = this.MisCamposServicios.Count > 0;
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Anular.ToString()
                || e.CommandName == Gestion.Modificar.ToString()
                || e.CommandName == Gestion.Impresion.ToString()))
                return;

            this.hdfIdTurismoDetalle.Value = string.Empty;
            int index = Convert.ToInt32(e.CommandArgument);
            int idTurismoDetalle = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            string xml = Encriptar.DesencriptarTexto(hdfIdTurismo.Value);
            List<TurTurismoDetalle> MisSericios = ObtenerListaServiciosDesdeXML(xml);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.hdfIdTurismoDetalle.Value = idTurismoDetalle.ToString();
                TurTurismoDetalle miServicio = new TurTurismoDetalle();
                miServicio = MisSericios.Find(x => x.IdTurismoDetalle == idTurismoDetalle);
                if (miServicio == null)
                    return;

                this.ddlServicio.SelectedValue = miServicio.IdTipoServicio.ToString();
                this.txtImporteServicio.Text = miServicio.Importe.ToString();
                this.txtCostoServicio.Text = miServicio.Costo.ToString();
                this.hdfIdProveedor.Value = miServicio.IdProveedor.ToString(); ;
                this.hdfProveedor.Value = miServicio.Proveedor.ToString();
                this.ddlEntidad.Items.Add(new ListItem(miServicio.Proveedor, miServicio.IdProveedor.ToString()));
                this.ddlEntidad.SelectedValue = miServicio.IdProveedor.ToString();
                this.ddlEntidad.Enabled = false;
                this.ddlServicio.Enabled = false;
                this.pnlCamposDinamicosTurismoServicios.Controls.Clear();
                this.MisCamposServicios = new List<TGECampos>();
                this.HabilitarControles = true;
                CMPProductos turParam = new CMPProductos();
                turParam.IdProducto = Convert.ToInt32(this.ddlServicio.SelectedValue);
                this.MisCamposServicios = TGEGeneralesF.CamposObtenerListaFiltro(new CtrTurismo(), turParam);
                foreach (TGECampos c in this.MisCamposServicios)
                {
                    TGECampos val = miServicio.ServiciosCampos.FirstOrDefault(x => x.IdCampo == c.IdCampo);
                    if (val != null)
                        c.CampoValor = val.CampoValor;
                }
                this.ArmarTablaParametros(pnlCamposDinamicosTurismoServicios, this.MisCamposServicios, this.HabilitarControles);
                this.MiArmarParametros = this.MisCamposServicios.Count > 0;
                this.MostrarControl = this.MisCamposServicios.Count > 0;
                this.pnlCamposDinamicosTurismoServicios.Visible = this.MisCamposServicios.Count > 0;
            }
            else if (e.CommandName == Gestion.Anular.ToString())
            {
                MisSericios.Remove(MisSericios.Find(x => x.IdTurismoDetalle == idTurismoDetalle));
                xml = this.ObtenerListaCamposValores(MisSericios, this.MisCamposTurismo).InnerXml;
                this.hdfIdTurismo.Value = Encriptar.EncriptarTexto(xml);
                MapearDatos(xml);
                decimal total = MisSericios.Sum(x => x.Importe);
                ScriptManager.RegisterStartupScript(this.pnlPrinc, this.pnlPrinc.GetType(), "CalcularTotalServiciosScript", "CalcularTotalServicios(" + total + ");", true);
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                string idTipoCargoAfiliadoFormaCobro = Request.Form["ctl00$ctl00$ContentPlaceHolder1$cphPrincipal$ModificarDatos$hdfIdTipoCargoAfiliadoFormaCobro"]; //si tiene implica que ya se dio de alta el cargo y podria imprimir.
                TurTurismoDetalle tur = MisSericios.Find(x => x.IdTurismoDetalle == idTurismoDetalle);
                if (tur != null && !string.IsNullOrEmpty(idTipoCargoAfiliadoFormaCobro))
                {
                    RepReportes reporte = new RepReportes();
                    RepParametros param = new RepParametros();
                    param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.TextBox;
                    param.Parametro = "IdTurismoDetalle";
                    param.ValorParametro = tur.IdTurismoDetalle.ToString();

                    RepParametros param2 = new RepParametros();
                    param2.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.TextBox;
                    param2.Parametro = "IdTipoCargoAfiliadoFormaCobro";
                    param2.ValorParametro = idTipoCargoAfiliadoFormaCobro;

                    TGEPlantillas plantilla = new TGEPlantillas();
                    plantilla.Codigo = "CarTiposCargosAfiliadosFormasCobrosReservaPaquetesTurismo";
                    plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

                    reporte.StoredProcedure = plantilla.NombreSP;
                    reporte.Parametros.Add(param2);
                    reporte.Parametros.Add(param);

                    DataSet ds = ReportesF.ReportesObtenerDatos(reporte);

                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(plantilla, ds, "IdTurismoDetalle", AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.Page, "CarTiposCargosAfiliadosFormasCobrosReservaPaquetesTurismo" + tur.IdTurismoDetalle.ToString() + "_" + this.UsuarioActivo.ApellidoNombre, this.UsuarioActivo);
                }
            }
        }
        protected void gvDatos_RowCreated(object sender, GridViewRowEventArgs e)
        { }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton imprimir = (ImageButton)e.Row.FindControl("btnImprimir");
                ImageButton anular = (ImageButton)e.Row.FindControl("btnBaja");
                if (this.GestionControl == Gestion.Consultar)
                    modificar.Visible = false;
                else
                {
                    if (MiControl != TipoControl.ReservaPaquetes)
                        modificar.Visible = true;

                }
                if (MiControl == TipoControl.ReservaPaquetes)
                    imprimir.Visible = true;

                string idTurismoDetalle = gvDatos.DataKeys[e.Row.DataItemIndex].Value.ToString();

                if (this.MiTurismoDetalles.Tables[1].AsEnumerable()
                            .Where(row => row.Field<int>("IdTurismoDetalle") == Convert.ToInt32(idTurismoDetalle)).Count() > 0)
                {
                    Literal detalles = e.Row.FindControl("ltlDetalleCampos") as Literal;
                    DataTable tblFiltered = this.MiTurismoDetalles.Tables[1].AsEnumerable()
                            .Where(row => row.Field<int>("IdTurismoDetalle") == Convert.ToInt32(idTurismoDetalle))
                            .CopyToDataTable();
                    string codigo = string.Empty;
                    codigo = string.Concat(codigo, "<table>");
                    foreach (DataRow r in tblFiltered.Rows)
                    {
                        codigo = string.Concat(codigo, string.Format("<tr><td>{0}</td><td>{1}</td></tr>", r["Titulo"].ToString(), r["Valor"].ToString()));
                    }
                    codigo = string.Concat(codigo, "</table>");
                    detalles.Text = codigo;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lbl = (Label)e.Row.FindControl("lblCostoTotal");
                lbl.Text = this.MiTurismoDetalles.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("Costo")).ToString("C2");
                lbl = (Label)e.Row.FindControl("lblImporteTotal");
                lbl.Text = this.MiTurismoDetalles.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("Importe")).ToString("C2");
            }
        }
        protected void gvPagos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int IdTipoOperacion = Convert.ToInt32(((HiddenField)this.gvPagos.Rows[index].FindControl("hdfIdTipoOperacion")).Value);
            int IdRefTipoOperacion = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdSolicitudPago"].ToString());

            if (IdRefTipoOperacion <= 0)
                IdRefTipoOperacion = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdOrdenPago"].ToString());

            this.MisParametrosUrl = new Hashtable();
            Menues filtroMenu = new Menues
            {
                IdTipoOperacion = IdTipoOperacion
            };

            if (e.CommandName == Gestion.Impresion.ToString())
            {
                if (IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesPagos
                    || IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesPagosInterno)
                {
                    CapOrdenesPagos op = new CapOrdenesPagos();
                    op.IdOrdenPago = IdRefTipoOperacion;
                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CapOrdenesPagos, "OrdenesPagos", op, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.Page, "OrdenesPagos", this.UsuarioActivo);
                }
                else
                {
                    CapSolicitudPago sp = new CapSolicitudPago();
                    sp.IdSolicitudPago = IdRefTipoOperacion;
                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CapSolicitudPagos, "SolicitudPagoCompras", sp, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.Page, "SolicitudPagoCompras", this.UsuarioActivo);
                }
            }
            else
            {
                if (e.CommandName == Gestion.Consultar.ToString())
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Consulta;
                else if (e.CommandName == Gestion.Anular.ToString())
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Anular;
                else if (e.CommandName == Gestion.Autorizar.ToString())
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Autorizar;

                //Guardo Menu devuelto de la DB
                filtroMenu = SeguridadF.MenuesObtenerPorOperacionTipoMenu(filtroMenu);
                this.MisParametrosUrl.Add(filtroMenu.NombreParametro, IdRefTipoOperacion);
                string IdTipoCargoAfiliadoFormaCobro = Request.Form["ctl00$ctl00$ContentPlaceHolder1$cphPrincipal$ModificarDatos$hdfIdTipoCargoAfiliadoFormaCobro"];
                this.MisParametrosUrl.Add("IdTipoCargoAfiliadoFormaCobro", IdTipoCargoAfiliadoFormaCobro);
                //Si devuelve una URL Redirecciona si no muestra mensaje error
                if (filtroMenu.URL.Length != 0)
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(string.Concat("~/", filtroMenu.URL)), true);
                else
                    this.MostrarMensaje("La URL no es valida.", true);
            }
        }
        protected void gvPagos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");
                ImageButton ibtnAutorizar = (ImageButton)e.Row.FindControl("btnAutorizar");
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                ibtnConsultar.Visible = this.ValidarPermiso("OrdenesPagosConsultar.aspx");

                DataRowView dr = (DataRowView)e.Row.DataItem;
                switch (Convert.ToInt32(dr["EstadoIdEstado"]))
                {
                    case (int)EstadosOrdenesPago.Activo:
                        ibtnAnular.Visible = this.ValidarPermiso("OrdenesPagosAnular.aspx");
                        ibtnAutorizar.Visible = this.ValidarPermiso("OrdenesPagosAutorizar.aspx");
                        break;
                    case (int)EstadosOrdenesPago.Autorizado:
                        ibtnAnular.Visible = this.ValidarPermiso("OrdenesPagosAnular.aspx");
                        break;
                    case (int)EstadosOrdenesPago.Pagado:
                        ibtnAnular.Visible = this.ValidarPermiso("OrdenesPagosAnularPagada.aspx");
                        ibtnModificar.Visible = this.ValidarPermiso("OrdenesPagosModificar.aspx");
                        break;
                    default:
                        break;
                }
            }
        }
        private void MapearDatos(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                this.gvDatos.DataSource = null;
                this.gvDatos.DataBind();
                AyudaProgramacion.FixGridView(gvDatos);
                return;
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNode nodo = doc.SelectSingleNode("TurismoDetalles");
            if (nodo is null)
                return;

            txtNroReserva.Text = nodo.Attributes["NroReserva"].Value != null ? nodo.Attributes["NroReserva"].Value : string.Empty; ;
            txtDetalle.Text = nodo.Attributes["Detalle"] != null ? nodo.Attributes["Detalle"].Value : string.Empty;
            txtFechaSalida.Text = nodo.Attributes["FechaSalida"] != null ? nodo.Attributes["FechaSalida"].Value : string.Empty;
            txtFechaRegreso.Text = nodo.Attributes["FechaRegreso"] != null ? nodo.Attributes["FechaRegreso"].Value : string.Empty;
            txtImpuestoPais.Decimal = nodo.Attributes["ImpuestoPais"] != null ? !string.IsNullOrEmpty(nodo.Attributes["ImpuestoPais"].Value) ? Convert.ToDecimal(nodo.Attributes["ImpuestoPais"].Value.Replace(".", ",")) : 0 : 0;
            txtPercepcionRG3819.Decimal = nodo.Attributes["PercepcionRG3819"] != null ? !string.IsNullOrEmpty(nodo.Attributes["PercepcionRG3819"].Value) ? Convert.ToDecimal(nodo.Attributes["PercepcionRG3819"].Value.Replace(".", ",")) : 0 : 0;
            txtPercepcionRG5272.Decimal = nodo.Attributes["PercepcionRG5272"] != null ? !string.IsNullOrEmpty(nodo.Attributes["PercepcionRG5272"].Value) ? Convert.ToDecimal(nodo.Attributes["PercepcionRG5272"].Value.Replace(".", ",")) : 0 : 0;
            txtPercepcionRG4815.Decimal = nodo.Attributes["PercepcionRG4815"] != null ? !string.IsNullOrEmpty(nodo.Attributes["PercepcionRG4815"].Value) ? Convert.ToDecimal(nodo.Attributes["PercepcionRG4815"].Value.Replace(".", ",")) : 0 : 0;
            txtImporteReintegrar.Decimal = nodo.Attributes["ImporteReintegrar"] != null ? !string.IsNullOrEmpty(nodo.Attributes["ImporteReintegrar"].Value) ? Convert.ToDecimal(nodo.Attributes["ImporteReintegrar"].Value.Replace(".", ",")) : 0 : 0;
            ddlCuentasAhorros.SelectedValue = nodo.Attributes["IdCuenta"] != null ? !string.IsNullOrEmpty(nodo.Attributes["IdCuenta"].Value) ? nodo.Attributes["IdCuenta"].Value : string.Empty : string.Empty;
            txtCostoCancelar.Decimal = nodo.Attributes["CostoCancelar"] != null ? !string.IsNullOrEmpty(nodo.Attributes["CostoCancelar"].Value) ? Convert.ToDecimal(nodo.Attributes["CostoCancelar"].Value.Replace(".", ",")) : 0 : 0;

            MiTurismoDetalles = new DataSet();
            TGECampos MiCampo = new TGECampos();
            MiCampo.CampoValor.Valor = xml;

            MiTurismoDetalles = TurismoF.TurismoObtenerReservasDetallesDesdeXML(MiCampo);

            this.gvDatos.DataSource = MiTurismoDetalles.Tables[0];
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);
            string value = MiTurismoDetalles.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("Importe")).ToString("N2").Replace(".", "").Replace(",", ".");

            if (ResetImportes != value || PrimeraVez)
            {
                this.PrimeraVez = false;
                hdfTotalServicios.Value = value;
                this.ResetImportes = value;
                string stotal = hdfTotalServicios.Value.Replace(",", ".");
                string js = string.Format("CalcularTotalServicios({0});", stotal);
                ScriptManager.RegisterStartupScript(this.pnlPrinc, this.pnlPrinc.GetType(), "CalcularTotalServiciosScript", js, true);
            }
            if (nodo.HasChildNodes)
            {
                List<CapProveedores> proveedores = new List<CapProveedores>();
                CapProveedores prov;
                foreach (XmlNode item in nodo.ChildNodes)
                {
                    if (!proveedores.Exists(x => x.IdProveedor == Convert.ToInt32(item.Attributes["IdProveedor"].Value)))
                    {
                        prov = new CapProveedores();
                        prov.IdProveedor = Convert.ToInt32(item.Attributes["IdProveedor"].Value);
                        prov.RazonSocial = item.Attributes["Proveedor"].Value;
                        proveedores.Add(prov);
                    }
                }
                ddlProveedoresPagos.Items.Clear();
                ddlProveedoresPagos.SelectedIndex = -1;
                ddlProveedoresPagos.SelectedValue = null;
                ddlProveedoresPagos.ClearSelection();

                ddlProveedoresPagos.DataSource = proveedores;
                ddlProveedoresPagos.DataValueField = "IdProveedor";
                ddlProveedoresPagos.DataTextField = "RazonSocial";
                ddlProveedoresPagos.DataBind();
                AyudaProgramacion.InsertarItemSeleccione(ddlProveedoresPagos, ObtenerMensajeSistema("SeleccioneOpcion"));

                CapOrdenesPagos pOrdenPago = new CapOrdenesPagos();
                pOrdenPago.IdRefTipoOperacion = MiTablaValor.IdTipoCargoAfiliadoFormaCobro;
                pOrdenPago.Filtro = typeof(CarTiposCargosAfiliadosFormasCobros).Name;
                this.gvPagos.DataSource = CuentasPagarF.OrdenesPagosObtenerListaFiltroGrillaTurismo(pOrdenPago);
                this.gvPagos.DataBind();
                this.gvPagosValores.DataSource = CuentasPagarF.OrdenesPagosObtenerListaFiltroPagoTurismo(pOrdenPago);
                this.gvPagosValores.DataBind();
                this.pnlPrinc.Update();
            }
        }
        private List<TurTurismoDetalle> ObtenerListaServiciosDesdeXML(string xml)
        {
            List<TurTurismoDetalle> lista = new List<TurTurismoDetalle>();
            if (string.IsNullOrEmpty(xml))
                return lista;

            TurTurismoDetalle servicio;
            TGECampos campo;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNode nodo = doc.SelectSingleNode("TurismoDetalles");
            if (nodo is null)
                return lista;

            XmlNodeList nodos = doc.SelectNodes("TurismoDetalles/TurismoDetalle");
            foreach (XmlNode item in nodos)
            {
                servicio = new TurTurismoDetalle();
                servicio.IdTurismoDetalle = item.Attributes.GetNamedItem("IdTurismoDetalle") == null ? Convert.ToInt32(item.Attributes["IdPaqueteDetalle"].Value) : Convert.ToInt32(item.Attributes["IdTurismoDetalle"].Value);
                servicio.IdTipoServicio = item.Attributes.GetNamedItem("IdTipoServicio") == null ? Convert.ToInt32(item.Attributes["IdProducto"].Value) : Convert.ToInt32(item.Attributes["IdTipoServicio"].Value);
                servicio.TipoServicio = item.Attributes.GetNamedItem("TipoServicio") == null ? item.Attributes["ProductoDescripcion"].Value : item.Attributes["TipoServicio"].Value;
                servicio.IdProveedor = Convert.ToInt32(item.Attributes["IdProveedor"].Value);
                servicio.Proveedor = item.Attributes["Proveedor"].Value;
                servicio.Costo = Convert.ToDecimal(item.Attributes["Costo"].Value.Replace(".", ","));
                servicio.Importe = Convert.ToDecimal(item.Attributes["Importe"].Value.Replace(".", ","));
                if (item.HasChildNodes)
                {
                    foreach (XmlNode cv in item.ChildNodes)
                    {
                        campo = new TGECampos();
                        campo.IdCampo = Convert.ToInt32(cv.Attributes["IdCampo"].Value);
                        campo.Nombre = cv.Attributes["Nombre"].Value;
                        campo.CampoValor.Valor = cv.Attributes["Valor"].Value;
                        campo.CampoValor.ListaValor = cv.Attributes["ListaValor"].Value;
                        servicio.ServiciosCampos.Add(campo);
                    }
                }
                lista.Add(servicio);
            }
            return lista;
        }
        private XmlDocument ObtenerListaCamposValores(List<TurPaquetesDetalles> listaServicios, List<TGECampos> listaCamposTurismo)
        {
            XmlNode itemNodo;
            XmlNode itemCampos;
            XmlAttribute itemAttribute;
            XmlDocument loteCampos = new XmlDocument();
            XmlNode nodos = loteCampos.CreateElement("TurismoDetalles");
            itemAttribute = loteCampos.CreateAttribute("NroReserva");
            itemAttribute.Value = string.IsNullOrEmpty(txtNroReserva.Text) ? "0" : txtNroReserva.Text;
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("Detalle");
            itemAttribute.Value = txtDetalle.Text;
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("FechaSalida");
            itemAttribute.Value = txtFechaSalida.Text;
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("FechaRegreso");
            itemAttribute.Value = txtFechaRegreso.Text;
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("ImpuestoPais");
            itemAttribute.Value = txtImpuestoPais.Decimal.ToString("N2").Replace(".", "").Replace(",", ".");
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("PercepcionRG3819");
            itemAttribute.Value = txtPercepcionRG3819.Decimal.ToString("N2").Replace(".", "").Replace(",", ".");
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("PercepcionRG4815");
            itemAttribute.Value = txtPercepcionRG4815.Decimal.ToString("N2").Replace(".", "").Replace(",", ".");
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("PercepcionRG4815");
            itemAttribute.Value = txtPercepcionRG4815.Decimal.ToString("N2").Replace(".", "").Replace(",", ".");
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("PercepcionRG5272");
            itemAttribute.Value = txtPercepcionRG5272.Decimal.ToString("N2").Replace(".", "").Replace(",", ".");
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("TotalServicios");
            itemAttribute.Value = listaServicios.Sum(x => x.Importe).ToString("N2").Replace(".", "").Replace(",", "."); ;
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("ImporteReintegrar");
            itemAttribute.Value = txtImporteReintegrar.Decimal.ToString("N2").Replace(".", "").Replace(",", ".");
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("IdCuenta");
            itemAttribute.Value = ddlCuentasAhorros.SelectedValue;
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("CostoCancelar");
            itemAttribute.Value = txtCostoCancelar.Decimal.ToString("N2").Replace(".", "").Replace(",", ".");
            nodos.Attributes.Append(itemAttribute);

            loteCampos.AppendChild(nodos);

            if (listaServicios.Count > 0)
            {
                itemAttribute = loteCampos.CreateAttribute("TotalServicios");
                itemAttribute.Value = listaServicios.Sum(x => x.Importe).ToString("N2").Replace(".", "").Replace(",", "."); ;
                nodos.Attributes.Append(itemAttribute);
            }
            loteCampos.AppendChild(nodos);

            foreach (TurPaquetesDetalles item in listaServicios)
            {
                itemNodo = loteCampos.CreateElement("TurismoDetalle");

                itemAttribute = loteCampos.CreateAttribute("IdPaqueteDetalle");
                itemAttribute.Value = item.IdPaqueteDetalle.ToString();
                itemNodo.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("IdProducto");
                itemAttribute.Value = item.Producto.IdProducto.ToString();
                itemNodo.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("ProductoDescripcion");
                itemAttribute.Value = item.Producto.Descripcion;
                itemNodo.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("IdProveedor");
                itemAttribute.Value = item.IdProveedor.ToString();
                itemNodo.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("Proveedor");
                itemAttribute.Value = item.Proveedor.ToString();
                itemNodo.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("Costo");
                itemAttribute.Value = item.Costo.ToString("N2").Replace(".", "").Replace(",", ".");
                itemNodo.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("Importe");
                itemAttribute.Value = item.Importe.ToString("N2").Replace(".", "").Replace(",", ".");
                itemNodo.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("EstadoColeccion");
                itemAttribute.Value = item.EstadoColeccion.ToString();
                itemNodo.Attributes.Append(itemAttribute);

                foreach (TGECampos servicio in item.Campos)
                {
                    itemCampos = loteCampos.CreateElement("ServiciosCampos");

                    itemAttribute = loteCampos.CreateAttribute("IdPaqueteDetalle");
                    itemAttribute.Value = item.IdPaqueteDetalle.ToString();
                    itemCampos.Attributes.Append(itemAttribute);

                    itemAttribute = loteCampos.CreateAttribute("IdCampo");
                    itemAttribute.Value = servicio.IdCampo.ToString();
                    itemCampos.Attributes.Append(itemAttribute);

                    itemAttribute = loteCampos.CreateAttribute("Nombre");
                    itemAttribute.Value = servicio.Nombre;
                    itemCampos.Attributes.Append(itemAttribute);

                    itemAttribute = loteCampos.CreateAttribute("Valor");
                    itemAttribute.Value = servicio.CampoValor.Valor;
                    itemCampos.Attributes.Append(itemAttribute);

                    itemAttribute = loteCampos.CreateAttribute("ListaValor");
                    itemAttribute.Value = servicio.CampoValor.ListaValor;
                    itemCampos.Attributes.Append(itemAttribute);
                    itemNodo.AppendChild(itemCampos);
                }
                nodos.AppendChild(itemNodo);
            }

            XmlNode nodoTur = loteCampos.CreateElement("TurismoCampos");
            foreach (TGECampos item in listaCamposTurismo)
            {
                itemCampos = loteCampos.CreateElement("TurismoCampo");

                itemAttribute = loteCampos.CreateAttribute("IdCampo");
                itemAttribute.Value = item.IdCampo.ToString();
                itemCampos.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("Nombre");
                itemAttribute.Value = item.Nombre;
                itemCampos.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("Valor");
                itemAttribute.Value = item.CampoValor.Valor;
                itemCampos.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("ListaValor");
                itemAttribute.Value = item.CampoValor.ListaValor;
                itemCampos.Attributes.Append(itemAttribute);

                nodoTur.AppendChild(itemCampos);
            }
            return loteCampos;
        }
        private XmlDocument ObtenerListaCamposValores(List<TurTurismoDetalle> listaServicios, List<TGECampos> listaCamposTurismo)
        {
            XmlNode itemNodo;
            XmlNode itemCampos;
            XmlAttribute itemAttribute;
            XmlDocument loteCampos = new XmlDocument();
            XmlNode nodos = loteCampos.CreateElement("TurismoDetalles");
            itemAttribute = loteCampos.CreateAttribute("NroReserva");
            itemAttribute.Value = txtNroReserva.Text;
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("Detalle");
            itemAttribute.Value = txtDetalle.Text;
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("FechaSalida");
            itemAttribute.Value = txtFechaSalida.Text;
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("FechaRegreso");
            itemAttribute.Value = txtFechaRegreso.Text;
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("ImpuestoPais");
            itemAttribute.Value = txtImpuestoPais.Decimal.ToString("N2").Replace(".", "").Replace(",", ".");
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("PercepcionRG3819");
            itemAttribute.Value = txtPercepcionRG3819.Decimal.ToString("N2").Replace(".", "").Replace(",", ".");
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("PercepcionRG4815");
            itemAttribute.Value = txtPercepcionRG4815.Decimal.ToString("N2").Replace(".", "").Replace(",", ".");
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("PercepcionRG4815");
            itemAttribute.Value = txtPercepcionRG4815.Decimal.ToString("N2").Replace(".", "").Replace(",", ".");
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("PercepcionRG5272");
            itemAttribute.Value = txtPercepcionRG5272.Decimal.ToString("N2").Replace(".", "").Replace(",", ".");
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("TotalServicios");
            itemAttribute.Value = listaServicios.Sum(x => x.Importe).ToString("N2").Replace(".", "").Replace(",", "."); ;
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("ImporteReintegrar");
            itemAttribute.Value = txtImporteReintegrar.Decimal.ToString("N2").Replace(".", "").Replace(",", ".");
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("IdCuenta");
            itemAttribute.Value = ddlCuentasAhorros.SelectedValue;
            nodos.Attributes.Append(itemAttribute);

            itemAttribute = loteCampos.CreateAttribute("CostoCancelar");
            itemAttribute.Value = txtCostoCancelar.Decimal.ToString("N2").Replace(".", "").Replace(",", ".");
            nodos.Attributes.Append(itemAttribute);

            loteCampos.AppendChild(nodos);

            foreach (TurTurismoDetalle item in listaServicios)
            {
                itemNodo = loteCampos.CreateElement("TurismoDetalle");

                itemAttribute = loteCampos.CreateAttribute("IdTurismoDetalle");
                itemAttribute.Value = item.IdTurismoDetalle.ToString();
                itemNodo.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("IdTipoServicio");
                itemAttribute.Value = item.IdTipoServicio.ToString();
                itemNodo.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("TipoServicio");
                itemAttribute.Value = item.TipoServicio.ToString();
                itemNodo.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("IdProveedor");
                itemAttribute.Value = item.IdProveedor.ToString();
                itemNodo.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("Proveedor");
                itemAttribute.Value = item.Proveedor.ToString();
                itemNodo.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("Costo");
                itemAttribute.Value = item.Costo.ToString("N2").Replace(".", "").Replace(",", ".");
                itemNodo.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("Importe");
                itemAttribute.Value = item.Importe.ToString("N2").Replace(".", "").Replace(",", ".");
                itemNodo.Attributes.Append(itemAttribute);

                foreach (TGECampos servicio in item.ServiciosCampos)
                {
                    itemCampos = loteCampos.CreateElement("ServiciosCampos");

                    itemAttribute = loteCampos.CreateAttribute("IdTurismoDetalle");
                    itemAttribute.Value = item.IdTurismoDetalle.ToString();
                    itemCampos.Attributes.Append(itemAttribute);

                    itemAttribute = loteCampos.CreateAttribute("IdCampo");
                    itemAttribute.Value = servicio.IdCampo.ToString();
                    itemCampos.Attributes.Append(itemAttribute);

                    itemAttribute = loteCampos.CreateAttribute("Nombre");
                    itemAttribute.Value = servicio.Nombre;
                    itemCampos.Attributes.Append(itemAttribute);

                    itemAttribute = loteCampos.CreateAttribute("Valor");
                    itemAttribute.Value = servicio.CampoValor.Valor;
                    itemCampos.Attributes.Append(itemAttribute);

                    itemAttribute = loteCampos.CreateAttribute("ListaValor");
                    itemAttribute.Value = servicio.CampoValor.ListaValor;
                    itemCampos.Attributes.Append(itemAttribute);
                    itemNodo.AppendChild(itemCampos);
                }
                nodos.AppendChild(itemNodo);
            }
            XmlNode nodoTur = loteCampos.CreateElement("TurismoCampos");
            foreach (TGECampos item in listaCamposTurismo)
            {
                itemCampos = loteCampos.CreateElement("TurismoCampo");

                itemAttribute = loteCampos.CreateAttribute("IdCampo");
                itemAttribute.Value = item.IdCampo.ToString();
                itemCampos.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("Nombre");
                itemAttribute.Value = item.Nombre;
                itemCampos.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("Valor");
                itemAttribute.Value = item.CampoValor.Valor;
                itemCampos.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("ListaValor");
                itemAttribute.Value = item.CampoValor.ListaValor;
                itemCampos.Attributes.Append(itemAttribute);

                nodoTur.AppendChild(itemCampos);
            }
            return loteCampos;
        }
        protected void ddlProveedoresPagos_SelectedIndexChanged(object sender, EventArgs e)
        {
            MiReservaPagar = new DataTable();
            if (!string.IsNullOrEmpty(ddlProveedoresPagos.SelectedValue))
            {
                string idTipoCargoAfiliadoFormaCobro = Request.Form["ctl00$ctl00$ContentPlaceHolder1$cphPrincipal$ModificarDatos$hdfIdTipoCargoAfiliadoFormaCobro"];

                CapProveedores prov = new CapProveedores();
                prov.IdProveedor = Convert.ToInt32(ddlProveedoresPagos.SelectedValue);
                prov.Filtro = idTipoCargoAfiliadoFormaCobro;

                MiReservaPagar = CuentasPagarF.SolicitudPagoObtenerAnticiposReservasTurismoPendientes(prov);

                if (MiReservaPagar.Rows.Count > 0)
                    txtImporteAnticipo.Decimal = Convert.ToDecimal(MiReservaPagar.Rows[0]["Importe"]);
                else
                    txtImporteAnticipo.Decimal = 0;
            }
            else
                txtImporteAnticipo.Decimal = 0;
            ScriptManager.RegisterStartupScript(this.pnlPrinc, this.pnlPrinc.GetType(), "SetMultitabActive", "SetMultitabProfileActive();", true);
        }
        protected void btnAgregarPago_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.pnlPrinc, this.pnlPrinc.GetType(), "SetMultitabActive", "SetMultitabProfileActive();", true);
            if (string.IsNullOrEmpty(ddlProveedoresPagos.SelectedValue))
                return;

            if (MiReservaPagar.Rows.Count == 0)
            {
                this.MostrarMensaje("Los anticipos ya han sido generados", true);
                return;
            }

            if (txtImporteAnticipo.Decimal > Convert.ToDecimal(MiReservaPagar.Rows[0]["Importe"]))
            {
                this.MostrarMensaje(string.Format("El importe no puede ser mayor a {0}", MiReservaPagar.Rows[0]["Importe"]), true);
                return;
            }

            MiReservaPagar.Rows[0]["Incluir"] = true;
            MiReservaPagar.Rows[0]["Importe"] = txtImporteAnticipo.Decimal;
            int IdTipoCargoAfiliadoFormaCobro = (int)MiReservaPagar.Rows[0]["IdTipoCargoAfiliadoFormaCobro"];
            Objeto resultado = new Objeto();
            resultado.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            if (CuentasPagarF.SolicitudPagoAgregarAnticiposTurismo(resultado, MiReservaPagar))
            {
                MiReservaPagar = new DataTable();
                this.MostrarMensaje(resultado.CodigoMensaje, false);
                CapOrdenesPagos pOrdenPago = new CapOrdenesPagos();
                pOrdenPago.IdRefTipoOperacion = IdTipoCargoAfiliadoFormaCobro;
                pOrdenPago.Filtro = typeof(CarTiposCargosAfiliadosFormasCobros).Name;
                this.gvPagos.DataSource = CuentasPagarF.OrdenesPagosObtenerListaFiltroGrillaTurismo(pOrdenPago);
                this.gvPagos.DataBind();
                this.gvPagosValores.DataSource = CuentasPagarF.OrdenesPagosObtenerListaFiltroPagoTurismo(pOrdenPago);
                this.gvPagosValores.DataBind();
                txtImporteAnticipo.Text = "";
                ddlProveedoresPagos.SelectedValue = "";
            }
            else
            {
                this.MostrarMensaje(resultado.CodigoMensaje, true);
            }
        }
        protected void btnAgregarComprobante_Click(object sender, EventArgs e)
        {
            string idTipoCargoAfiliadoFormaCobro = Request.Form["ctl00$ctl00$ContentPlaceHolder1$cphPrincipal$ModificarDatos$hdfIdTipoCargoAfiliadoFormaCobro"];

            PaginaAfiliados paginaAfiliados = new PaginaAfiliados();
            AfiAfiliados afi = paginaAfiliados.Obtener(MiSessionPagina);
            this.MisParametrosUrl = new Hashtable
            {
                { "IdAfiliado", afi.IdAfiliado },
                { "IdTipoCargoAfiliadoFormaCobro", idTipoCargoAfiliadoFormaCobro },
                { "FechaSalida", txtFechaSalida.Text },
                { "FechaRegreso", txtFechaRegreso.Text }
            };
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturasAgregar.aspx"), true);
        }
        protected void btnAgregarOC_Click(object sender, EventArgs e)
        {
            string idTipoCargoAfiliadoFormaCobro = Request.Form["ctl00$ctl00$ContentPlaceHolder1$cphPrincipal$ModificarDatos$hdfIdTipoCargoAfiliadoFormaCobro"];
            string idTipoCargo = Request.Form["ctl00$ctl00$ContentPlaceHolder1$cphPrincipal$ModificarDatos$hdfIdTipoCargo"];
            PaginaAfiliados paginaAfiliados = new PaginaAfiliados();
            AfiAfiliados afi = paginaAfiliados.Obtener(MiSessionPagina);
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", afi.IdAfiliado);
            this.MisParametrosUrl.Add("IdTipoCargoAfiliadoFormaCobro", idTipoCargoAfiliadoFormaCobro);
            this.MisParametrosUrl.Add("IdTipoCargo", idTipoCargo);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosAfiliadosAgregar.aspx"), true);
        }
        #region "Armar Controles Parametros"
        private void ObtenerValoresParametros(List<TGECampos> pCampos)
        {
            CultureInfo culture = CultureInfo.CurrentUICulture;
            List<string> keys = this.Request.Form.AllKeys.Where(x => !string.IsNullOrEmpty(x) && x.Contains("$" + this.ID + "$")).ToList();
            string k, l;
            foreach (TGECampos parametro in pCampos)
            {
                switch (parametro.CampoTipo.IdCampoTipo)
                {
                    case (int)EnumCamposTipos.DropDownListSPAutoComplete:
                        k = keys.Find(x => x.EndsWith(preid + "select2HdfValue" + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                            parametro.CampoValor.Valor = this.Request.Form[k];
                        k = keys.Find(z => z.EndsWith(preid + "select2HdfText" + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                            parametro.CampoValor.ListaValor = this.Request.Form[k];
                        break;
                    case (int)EnumCamposTipos.ComboBoxSP:
                        k = keys.Find(x => x.EndsWith(preid + "ListComboBox" + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                            parametro.CampoValor.Valor = this.Request.Form[k];
                        break;
                    case (int)EnumCamposTipos.CheckBox:
                        k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                            parametro.CampoValor.Valor = string.IsNullOrEmpty(this.Request.Form[k]) ? false.ToString() : this.Request.Form[k] == "on" ? true.ToString() : false.ToString();
                        else
                            parametro.CampoValor.Valor = false.ToString();
                        break;
                    case (int)EnumCamposTipos.DropDownList:
                    case (int)EnumCamposTipos.DropDownListSP:
                    case (int)EnumCamposTipos.DropDownListQuery:
                        k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                        {
                            parametro.CampoValor.Valor = this.Request.Form[k];
                            if (parametro.CampoValor.Valor.Trim() == string.Empty)
                                parametro.CampoValor.ListaValor = string.Empty;
                            else
                            {
                                l = keys.Find(z => z.EndsWith(preid + "HdfText" + parametro.IdCampo.ToString()));
                                parametro.CampoValor.ListaValor = string.IsNullOrEmpty(l) ? string.Empty : this.Request.Form[l];
                            }
                        }
                        break;
                    case (int)EnumCamposTipos.DropDownListMultiple:
                        k = keys.Find(x => x.EndsWith(preid + "select2HdfValue" + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                        {
                            parametro.CampoValor.Valor = this.Request.Form[k];
                        }
                        else
                        {
                            parametro.CampoValor.Valor = "";
                        }
                        break;
                    case (int)EnumCamposTipos.TextBox:
                    case (int)EnumCamposTipos.DateTime:
                    case (int)EnumCamposTipos.IntegerTextBox:
                        k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                            parametro.CampoValor.Valor = this.Request.Form[k];

                        break;
                    case (int)EnumCamposTipos.CurrencyTextBox:
                    case (int)EnumCamposTipos.NumericTextBox:
                    case (int)EnumCamposTipos.PercentTextBox:
                        k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                            parametro.CampoValor.Valor = decimal.Parse(this.Request.Form[k].Replace(culture.NumberFormat.CurrencySymbol, string.Empty)).ToString();
                        break;
                    case (int)EnumCamposTipos.GrillaDinamicaAB:
                        k = keys.Find(x => x.EndsWith(preid + "select2HdfValue" + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                        {
                            string value = this.Request.Form[k];
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                List<string> values = parametro.CampoValor.Valor.Split(';').ToList();
                                if (!values.Exists(x => x == value))
                                {
                                    if (parametro.CampoValor.Valor.Trim().Length > 0)
                                        parametro.CampoValor.Valor += ";" + value;
                                    else
                                        parametro.CampoValor.Valor = value;
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
                parametro.CampoValor.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(parametro.CampoValor, this.GestionControl);
                if (parametro.CampoValor.IdCampoValor == 0 && parametro.CampoValor.Valor.Trim() == string.Empty)
                    parametro.CampoValor.EstadoColeccion = EstadoColecciones.SinCambio;
                else if (parametro.CampoValor.IdCampoValor == 0)
                    parametro.CampoValor.EstadoColeccion = EstadoColecciones.Agregado;
                else
                    parametro.CampoValor.EstadoColeccion = EstadoColecciones.Modificado;
            }
        }
        private void ArmarTablaParametros(PlaceHolder phCamposDinamicos, List<TGECampos> pCampos, bool pHabilitar)
        {
            //this.tablaParametros = new Table();
            //this.pnlCamposDinamicos.Controls.Clear();
            List<Control> controlesEliminar = new List<Control>();
            foreach (Control ctr in phCamposDinamicos.Controls)// this.pnlCamposDinamicosTurismoServicios.Controls)
            {
                if (ctr is PlaceHolder)
                {
                    if (!pCampos.Exists(x => "panel" + x.IdCampo.ToString() == ctr.ID))
                        controlesEliminar.Add(ctr);
                }
            }
            foreach (Control ctr in controlesEliminar)
                phCamposDinamicos.Controls.Remove(ctr);
            pCampos = pCampos.OrderBy(x => x.Orden).ToList();
            List<TGEListasValoresDetalles> listaValoresDetalles;
            Control ctrExiste;

            //Panel pnlRow = new Panel();
            //pnlRow.CssClass = "form-group row";
            foreach (TGECampos parametro in pCampos)
            {
                ctrExiste = phCamposDinamicos.FindControl("panel" + parametro.IdCampo.ToString());
                if (ctrExiste != null)
                {
                    foreach (Control c in ctrExiste.Controls)
                        if (c is WebControl)
                        {
                            if (c is PlaceHolder)
                                foreach (Control d in c.Controls)
                                    if (d is WebControl)
                                        ((WebControl)d).Enabled = pHabilitar;
                        }
                    continue;
                }
                if (parametro.SaltoLinea)
                {
                    Panel pnlWrap = new Panel();
                    pnlWrap.CssClass = "w-100";
                    phCamposDinamicos.Controls.Add(pnlWrap);
                }
                listaValoresDetalles = new List<TGEListasValoresDetalles>();
                switch (parametro.CampoTipo.IdCampoTipo)
                {
                    case (int)EnumCamposTipos.DropDownList:
                        listaValoresDetalles = TGEGeneralesF.ListasValoresObtenerListaDetalle(parametro.ListaValor);
                        phCamposDinamicos.Controls.Add(AddListBoxRow(parametro, listaValoresDetalles, pHabilitar));
                        break;
                    case (int)EnumCamposTipos.DropDownListSP:
                        listaValoresDetalles = TGEGeneralesF.ListasValoresDetallesObtenerGenerica(parametro, this.MiTablaValor);
                        phCamposDinamicos.Controls.Add(AddListBoxRow(parametro, listaValoresDetalles, pHabilitar));
                        break;
                    case (int)EnumCamposTipos.DropDownListSPAutoComplete:
                        phCamposDinamicos.Controls.Add(AddListBoxAutocompleteRow(parametro, pHabilitar));
                        break;
                    case (int)EnumCamposTipos.DropDownListQuery:
                        listaValoresDetalles = TGEGeneralesF.ListasValoresDetallesObtenerConsultaDinamica(parametro, this.MiTablaValor);
                        phCamposDinamicos.Controls.Add(AddListBoxRow(parametro, listaValoresDetalles, pHabilitar));
                        break;
                    //case (int)EnumCamposTipos.DropDownListMultiple:
                    //    listaValoresDetalles = TGEGeneralesF.ListasValoresDetallesObtenerGenerica(parametro, this.MiTablaValor);
                    //    pnlRow.Controls.Add(AddListBoxRowMultiple(parametro, listaValoresDetalles, pHabilitar));
                    //    break;
                    //case (int)EnumCamposTipos.ComboBoxSP:
                    //    listaValoresDetalles = TGEGeneralesF.ListasValoresDetallesObtenerGenerica(parametro, this.MiTablaValor);
                    //    pnlRow.Controls.Add(AddListComboBox(parametro, listaValoresDetalles, pHabilitar));
                    //    break;
                    case (int)EnumCamposTipos.TextBox:
                        PlaceHolder textboxEnPlaceHolder = AddTextBoxRow(parametro, pHabilitar);
                        phCamposDinamicos.Controls.Add(textboxEnPlaceHolder);
                        phCamposDinamicos.Controls.Add(AddLabelRow(parametro, textboxEnPlaceHolder));
                        break;
                    //case (int)EnumCamposTipos.IntegerTextBox:
                    //    pnlRow.Controls.Add(AddNumericInputBoxRow(parametro, pHabilitar));
                    //    break;
                    //case (int)EnumCamposTipos.CurrencyTextBox:
                    //case (int)EnumCamposTipos.NumericTextBox:
                    //case (int)EnumCamposTipos.PercentTextBox:

                    //case (int)EnumCamposTipos.EmailTextBox:
                    //case (int)EnumCamposTipos.PasswordTextBox:
                    //    pnlRow.Controls.Add(AddCurrencyInputBoxRow(parametro, pHabilitar));
                    //    break;
                    //case (int)EnumCamposTipos.CheckBox:
                    //    pnlRow.Controls.Add(AddCheckBoxRow(parametro, pHabilitar));
                    //    break;
                    case (int)EnumCamposTipos.DateTime:
                        phCamposDinamicos.Controls.Add(AddDateTimeRow(parametro, pHabilitar));
                        break;
                    case (int)EnumCamposTipos.GrillaDinamicaAB:
                        if (GestionControl != Gestion.Listar)
                            phCamposDinamicos.Controls.Add(AddGrillaDinamicaAB(parametro, pHabilitar));
                        break;
                    default:
                        break;
                }
            }
            //pnlCamposDinamicosTurismoServicios.Controls.Add(pnlRow);
        }
        private PlaceHolder AddListBoxRow(TGECampos pCampo, List<TGEListasValoresDetalles> dsDatos, bool pHabilitar)
        {
            pHabilitar = pCampo.Deshabilitado ? false : pHabilitar;
            PlaceHolder panel = new PlaceHolder();
            panel.ID = "panel" + pCampo.IdCampo.ToString();

            Label lblParametro = new Label();
            lblParametro.CssClass = cssLabel;
            lblParametro.Text = pCampo.Titulo;

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol;

            DropDownList ddlListaOpciones = new DropDownList();
            ddlListaOpciones.CssClass = "form-control select2";

            ddlListaOpciones.DataValueField = "IdListaValorDetalle";
            ddlListaOpciones.DataTextField = "Descripcion";
            ddlListaOpciones.DataSource = dsDatos;
            ddlListaOpciones.DataBind();
            ListItem item = new ListItem(this.ObtenerMensajeSistema("SeleccioneOpcion"), string.Empty);
            ddlListaOpciones.Items.Add(item);
            ddlListaOpciones.ID = preid + pCampo.IdCampo.ToString();
            item = ddlListaOpciones.Items.FindByValue(pCampo.CampoValor.Valor);
            if (item != null)
                ddlListaOpciones.SelectedValue = pCampo.CampoValor.Valor;
            else
                ddlListaOpciones.SelectedValue = string.Empty;
            ddlListaOpciones.Enabled = pHabilitar;

            HiddenField hdfText = new HiddenField();
            hdfText.ID = preid + "HdfText" + pCampo.IdCampo.ToString();
            hdfText.Value = pCampo.CampoValor.ListaValor;

            pnlRow.Controls.Add(ddlListaOpciones);
            pnlRow.Controls.Add(hdfText);

            if (pCampo.Requerido && pHabilitar)
            {
                RequiredFieldValidator validador = new RequiredFieldValidator();
                validador.ControlToValidate = ddlListaOpciones.ID;
                validador.ErrorMessage = "*";
                validador.CssClass = "Validador";
                validador.ValidationGroup = "TurismoCamposValores";
                pnlRow.Controls.Add(validador);
            }
            panel.Controls.Add(pnlRow);
            if (pHabilitar)
            {
                StringBuilder script = new StringBuilder();
                script.AppendLine(" $(document).ready(function () {");
                //start function
                script.AppendLine(" function InitControl|CTRLID|() {");
                script.AppendFormat("var control = $('select[name$={0}]');", ddlListaOpciones.ID);
                //Select Change
                script.AppendLine("control.change(function() { ");
                script.AppendFormat("var hdfText = $('input[name$={0}]');", hdfText.ID);
                script.AppendFormat("hdfText.val( $('select[name$={0}] option:selected').text() );", ddlListaOpciones.ID);

                if (!string.IsNullOrWhiteSpace(pCampo.EventoJavaScript))
                {
                    script.AppendLine(pCampo.EventoJavaScript);
                }
                script.AppendLine("});");
                //end Select Change

                script.AppendLine("};");
                //end function
                script.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitControl|CTRLID|);");
                script.AppendLine("InitControl|CTRLID|();");
                script.AppendLine("});");

                script.Replace("|CTRLID|", ddlListaOpciones.ID);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script" + ddlListaOpciones.ID.ToString(), script.ToString(), true);
            }

            Panel divRow = new Panel();
            divRow.CssClass = cssRow;
            divRow.Controls.Add(lblParametro);
            divRow.Controls.Add(pnlRow);

            Panel divContainer = new Panel();
            divContainer.ID = string.Concat("dv", preid, pCampo.IdCampo.ToString());
            divContainer.CssClass = ccsContainer;
            divContainer.Controls.Add(divRow);
            panel.Controls.Add(divContainer);
            return panel;
        }
        private PlaceHolder AddListBoxAutocompleteRow(TGECampos pCampo, bool pHabilitar)
        {
            pHabilitar = pCampo.Deshabilitado ? false : pHabilitar;
            PlaceHolder panel = new PlaceHolder();
            panel.ID = "panel" + pCampo.IdCampo.ToString();

            Label lblParametro = new Label();
            lblParametro.CssClass = cssLabel;
            lblParametro.Text = pCampo.Titulo;
            //panel.Controls.Add(lblParametro);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol;
            DropDownList ddlListaOpciones = new DropDownList();
            ddlListaOpciones.ID = preid + pCampo.IdCampo.ToString();
            ddlListaOpciones.CssClass = "form-control select2";
            ddlListaOpciones.Enabled = pHabilitar;

            if (pCampo.CampoValor.Valor.Trim().Length > 0)
            {
                TGECamposValores parametro = new TGECamposValores();
                parametro.Valor = pCampo.CampoValor.Valor.Trim();
                parametro.IdRefTablaValor = 0;
                DataSet ds = BaseDatos.ObtenerBaseDatos().ObtenerDataSet(pCampo.StoredProcedure, parametro);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    pCampo.CampoValor.ListaValor = ds.Tables[0].Rows[0]["Descripcion"].ToString();

                ListItem item = new ListItem(pCampo.CampoValor.ListaValor, pCampo.CampoValor.Valor);
                ddlListaOpciones.Items.Add(item);
            }

            HiddenField hdfValue = new HiddenField();
            hdfValue.ID = preid + "select2HdfValue" + pCampo.IdCampo.ToString();
            hdfValue.Value = pCampo.CampoValor.Valor;
            HiddenField hdfText = new HiddenField();
            hdfText.ID = preid + "select2HdfText" + pCampo.IdCampo.ToString();
            hdfText.Value = pCampo.CampoValor.ListaValor;
            pnlRow.Controls.Add(ddlListaOpciones);
            pnlRow.Controls.Add(hdfValue);
            pnlRow.Controls.Add(hdfText);

            if (pCampo.Requerido && pHabilitar)
            {
                RequiredFieldValidator validador = new RequiredFieldValidator();
                validador.ControlToValidate = ddlListaOpciones.ID;
                validador.ErrorMessage = "*";
                validador.CssClass = "Validador";
                validador.ValidationGroup = "TurismoCamposValores";
                pnlRow.Controls.Add(validador);
            }


            if (pHabilitar)
            {
                StringBuilder script = new StringBuilder();
                script.AppendLine(" $(document).ready(function () {");
                //start function
                script.AppendLine(" function InitControlSelect2|CTRLID|() {");
                script.AppendFormat("var control = $('select[name$={0}]');", ddlListaOpciones.ID);

                //select2 start
                script.AppendLine("control.select2({");
                script.AppendFormat("placeholder: '{0}',", pCampo.Titulo);
                script.AppendLine("minimumInputLength: 4,");
                script.AppendLine("theme: 'bootstrap4',");
                script.AppendLine("language: 'es',");
                script.AppendLine("allowClear: true,");
                script.AppendLine("ajax: {");
                script.AppendLine("type: 'POST',");
                script.AppendLine("contentType: 'application/json; charset=utf-8',");
                script.AppendLine("dataType: 'json',");
                script.AppendFormat("url: '{0}',", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica2"));
                script.AppendLine("delay: 250,");
                script.AppendLine("data: function (params) {");
                script.AppendLine("return JSON.stringify( {");
                script.AppendLine("value: control.val(), // search term");
                script.AppendLine("filtro: params.term, // search term");
                script.AppendFormat("sp: '{0}',", pCampo.StoredProcedure);
                script.AppendFormat("idRefTablaValor: '{0}',", 0);// this.MiIdRefTablaValor);
                script.AppendFormat("idRefValor: '{0}'", 0); // MiIdRefValor.HasValue ? MiIdRefValor.Value.ToString() : string.Empty);
                script.AppendLine(" });");
                script.AppendLine("},");
                script.AppendLine("processResults: function (data, params) {");
                script.AppendLine("return {");
                script.AppendLine("results: data.d,");
                script.AppendLine("};");
                script.AppendLine(" cache: true");
                script.AppendLine("},");
                script.AppendLine("}");
                script.AppendLine("});");
                //end select2
                //select2 ON select
                script.AppendLine("control.on('select2:select', function(e) { ");
                script.AppendLine("var newOption = new Option(e.params.data.text, e.params.data.id, false, true);");
                script.AppendFormat("$('select[id$={0}]').append(newOption).trigger('change');", ddlListaOpciones.ID);
                script.AppendFormat("var hdfValue = $('input[name$={0}]');", hdfValue.ID);
                script.AppendFormat("var hdfText = $('input[name$={0}]');", hdfText.ID);
                script.AppendLine("hdfValue.val(e.params.data.id);");
                script.AppendLine("hdfText.val(e.params.data.text);");
                if (!string.IsNullOrWhiteSpace(pCampo.EventoJavaScript))
                {
                    script.AppendLine("ControlSelect2|CTRLID|On(e.params.data.id);");
                }
                script.AppendLine("});");
                //end select2 ON

                //select2 ON unselect
                script.AppendLine("control.on('select2:unselect', function(e) { ");
                script.AppendFormat("var hdfValue = $('input[name$={0}]');", hdfValue.ID);
                script.AppendFormat("var hdfText = $('input[name$={0}]');", hdfText.ID);
                script.AppendLine("hdfValue.val('');");
                script.AppendLine("hdfText.val('');");
                script.AppendLine("});");
                //end select2 ON unselect

                script.AppendLine("};");
                //end function
                //start scriptEvent
                script.AppendLine("function ControlSelect2|CTRLID|On(e) {");
                script.AppendFormat("{0}", pCampo.EventoJavaScript);
                script.AppendLine("}");
                //end scriptEvent

                script.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitControlSelect2|CTRLID|);");
                script.AppendLine("InitControlSelect2|CTRLID|();");
                script.AppendLine("});");

                script.Replace("|CTRLID|", ddlListaOpciones.ID);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script" + ddlListaOpciones.ID.ToString(), script.ToString(), true);
            }

            Panel divRow = new Panel();
            divRow.CssClass = cssRow;
            divRow.Controls.Add(lblParametro);
            divRow.Controls.Add(pnlRow);

            Panel divContainer = new Panel();
            divContainer.ID = string.Concat("dv", preid, pCampo.IdCampo.ToString());
            divContainer.CssClass = ccsContainer;
            divContainer.Controls.Add(divRow);
            panel.Controls.Add(divContainer);
            return panel;
        }
        private PlaceHolder AddLabelRow(TGECampos pCampo, PlaceHolder txtPlaceHolder)
        {
            PlaceHolder placeHolder = new PlaceHolder();
            if (!string.IsNullOrWhiteSpace(pCampo.StoredProcedureLeyenda))
            {
                placeHolder.ID = "panel" + "Leyenda" + pCampo.IdCampo.ToString();

                Panel divContainer = new Panel();
                divContainer.ID = string.Concat("dv", preid, "Leyenda", pCampo.IdCampo.ToString());
                divContainer.CssClass = ccsContainer;
                placeHolder.Controls.Add(divContainer);

                Panel divRowLeyenda = new Panel();
                divRowLeyenda.CssClass = cssRow;

                TextBox Text = (TextBox)BuscarControlRecursivo(txtPlaceHolder, preid + pCampo.IdCampo.ToString());
                Text.AutoPostBack = true;
                pCampo.Filtro = Text.Text;

                Label lblLeyenda = new Label();
                lblLeyenda.ID = preid + "Leyenda" + pCampo.IdCampo.ToString();
                lblLeyenda.CssClass = cssLabelCol6;

                divRowLeyenda.Controls.Add(lblLeyenda);
                if (pCampo.Filtro.Length > 0)
                {
                    TGEListasValoresDetalles valorDetalle = TGEGeneralesF.ListasValoresDetallesObtenerLeyenda(pCampo, this.MiTablaValor);

                    if (!string.IsNullOrEmpty(valorDetalle.Descripcion))
                        lblLeyenda.Text = valorDetalle.Descripcion;
                    else
                        lblLeyenda.Text = string.Empty;
                }
                divContainer.Controls.Add(divRowLeyenda);
            }
            return placeHolder;
        }
        private PlaceHolder AddTextBoxRow(TGECampos pCampo, bool pHabilitar)
        {
            pHabilitar = pCampo.Deshabilitado ? false : pHabilitar;
            PlaceHolder placeHolder = new PlaceHolder();
            placeHolder.ID = "panel" + pCampo.IdCampo.ToString();

            Label lblParametro = new Label();
            lblParametro.CssClass = cssLabel;
            lblParametro.Text = pCampo.Titulo;

            Panel pnlInputCtrl = new Panel();
            pnlInputCtrl.CssClass = cssCol;

            TextBox Text = new TextBox();
            Text.CssClass = "form-control";
            Text.ID = preid + pCampo.IdCampo.ToString();
            Text.Text = pCampo.CampoValor.Valor;
            Text.Enabled = pHabilitar;
            Text.MaxLength = pCampo.TamanioMaximo;
            pnlInputCtrl.Controls.Add(Text);

            if (!string.IsNullOrWhiteSpace(pCampo.StoredProcedure))
            {
                List<TGEListasValoresDetalles> listaValoresDetalles = TGEGeneralesF.ListasValoresDetallesObtenerGenerica(pCampo, this.MiTablaValor);
                if (listaValoresDetalles.Count > 0)
                    Text.Text = listaValoresDetalles[0].Descripcion;
            }

            if (pCampo.Requerido && pHabilitar)
            {
                RequiredFieldValidator validador = new RequiredFieldValidator();
                validador.ControlToValidate = Text.ID;
                validador.ErrorMessage = "*";
                validador.CssClass = "Validador";
                validador.ValidationGroup = "TurismoCamposValores";
                pnlInputCtrl.Controls.Add(validador);
            }

            Panel divRow = new Panel();
            divRow.CssClass = cssRow;
            divRow.Controls.Add(lblParametro);
            divRow.Controls.Add(pnlInputCtrl);

            Panel divContainer = new Panel();
            divContainer.ID = string.Concat("dv", preid, pCampo.IdCampo.ToString());
            divContainer.CssClass = ccsContainer;
            divContainer.Controls.Add(divRow);
            placeHolder.Controls.Add(divContainer);

            if (!string.IsNullOrWhiteSpace(pCampo.EventoJavaScript))
            {
                StringBuilder script = new StringBuilder();
                script.AppendLine(" $(document).ready(function () {");
                //start scriptEvent
                script.AppendLine("function ControlTextBox|CTRLID|() {");
                script.AppendFormat("{0}", pCampo.EventoJavaScript);
                script.AppendLine("}");
                //end scriptEvent
                script.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ControlTextBox|CTRLID|);");
                script.AppendLine("ControlTextBox|CTRLID|();");
                script.AppendLine("});");
                script.Replace("|CTRLID|", Text.ID);
                ScriptManager.RegisterClientScriptBlock(this.pnlPrinc, this.pnlPrinc.GetType(), "Script" + Text.ID, script.ToString(), true);
            }
            return placeHolder;
        }
        private PlaceHolder AddDateTimeRow(TGECampos pCampo, bool pHabilitar)
        {
            pHabilitar = pCampo.Deshabilitado ? false : pHabilitar;
            PlaceHolder panel = new PlaceHolder();
            panel.ID = "panel" + pCampo.IdCampo.ToString();

            Label lblParametro = new Label();
            lblParametro.CssClass = cssLabel;
            lblParametro.Text = pCampo.Titulo;

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol;
            TextBox Text = new TextBox();
            Text.CssClass = "form-control datepicker";
            Text.ID = preid + pCampo.IdCampo.ToString();
            Text.Text = pCampo.CampoValor.Valor;
            Text.Enabled = pHabilitar;
            pnlRow.Controls.Add(Text);

            if (pCampo.Requerido && pHabilitar)
            {
                RequiredFieldValidator validador = new RequiredFieldValidator();
                validador.ControlToValidate = Text.ID;
                validador.ErrorMessage = "*";
                validador.CssClass = "Validador";
                validador.ValidationGroup = "TurismoCamposValores";
                pnlRow.Controls.Add(validador);
            }
            Panel divRow = new Panel();
            divRow.CssClass = cssRow;
            divRow.Controls.Add(lblParametro);
            divRow.Controls.Add(pnlRow);

            Panel divContainer = new Panel();
            divContainer.ID = string.Concat("dv", preid, pCampo.IdCampo.ToString());
            divContainer.CssClass = ccsContainer;
            divContainer.Controls.Add(divRow);
            panel.Controls.Add(divContainer);

            if (!string.IsNullOrWhiteSpace(pCampo.EventoJavaScript))
            {
                StringBuilder script = new StringBuilder();
                script.AppendLine(" $(document).ready(function () {");
                //start scriptEvent
                script.AppendLine("function ControlTextBox|CTRLID|() {");
                script.AppendFormat("{0}", pCampo.EventoJavaScript);
                script.AppendLine("}");
                //end scriptEvent
                script.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ControlTextBox|CTRLID|);");
                script.AppendLine("ControlTextBox|CTRLID|();");
                script.AppendLine("});");
                script.Replace("|CTRLID|", Text.ID);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script" + Text.ID, script.ToString(), true);
            }
            return panel;
        }
        private PlaceHolder AddGrillaDinamicaAB(TGECampos pCampo, bool pHabilitar)
        {
            pHabilitar = pCampo.Deshabilitado ? false : pHabilitar;
            PlaceHolder panel = new PlaceHolder();
            panel.ID = "panel" + pCampo.IdCampo.ToString();

            Panel divGroupCtrl = new Panel();
            divGroupCtrl.ID = string.Concat("dv", preid, pCampo.IdCampo.ToString());
            divGroupCtrl.CssClass = "col-12";

            Panel divGroupRow = new Panel();
            divGroupRow.CssClass = cssRow;

            if (pHabilitar)
            {
                Label lblParametro = new Label();
                lblParametro.CssClass = cssLabel;
                lblParametro.Text = pCampo.Titulo;

                Panel pnlCol = new Panel();
                pnlCol.CssClass = cssCol;
                DropDownList ddlListaOpciones = new DropDownList();
                ddlListaOpciones.ID = preid + pCampo.IdCampo.ToString();
                ddlListaOpciones.CssClass = "form-control select2";
                ddlListaOpciones.Enabled = pHabilitar;

                HiddenField hdfValue = new HiddenField();
                hdfValue.ID = preid + "select2HdfValue" + pCampo.IdCampo.ToString();

                pnlCol.Controls.Add(ddlListaOpciones);
                pnlCol.Controls.Add(hdfValue);

                Panel divRow = new Panel();
                divRow.CssClass = cssRow;
                divRow.Controls.Add(lblParametro);
                divRow.Controls.Add(pnlCol);
                Panel divContainer = new Panel();
                divContainer.CssClass = ccsContainer;
                divContainer.Controls.Add(divRow);

                Button btn = new Button();
                btn.ID = preid + "btnGridAB" + pCampo.IdCampo.ToString();
                btn.CssClass = "botonesEvol";
                btn.Text = "Agregar";
                btn.ValidationGroup = "AgregarGrillaAB" + pCampo.IdCampo.ToString();
                btn.Click += BtnAgregarGrillaAB_Click;

                Panel pnlColBtn = new Panel();
                pnlColBtn.CssClass = cssCol;
                pnlColBtn.Controls.Add(btn);
                pnlCol.Controls.Add(pnlColBtn);

                Panel divRowBtn = new Panel();
                divRowBtn.CssClass = cssRow;
                divRowBtn.Controls.Add(pnlColBtn);
                Panel divContainerBtn = new Panel();
                divContainerBtn.CssClass = ccsContainer;
                divContainerBtn.Controls.Add(divRowBtn);

                if (pCampo.Requerido)
                {
                    RequiredFieldValidator validador = new RequiredFieldValidator();
                    validador.ControlToValidate = ddlListaOpciones.ID;
                    validador.ErrorMessage = "*";
                    validador.CssClass = "Validador";
                    validador.ValidationGroup = "AgregarGrillaAB" + pCampo.IdCampo.ToString();
                    pnlCol.Controls.Add(validador);
                }

                #region Javascript
                StringBuilder script = new StringBuilder();
                script.AppendLine(" $(document).ready(function () {");
                //start function
                script.AppendLine(" function InitControlSelect2|CTRLID|() {");
                script.AppendFormat("var control = $('select[name$={0}]');", ddlListaOpciones.ID);

                //select2 start
                script.AppendLine("control.select2({");
                script.AppendFormat("placeholder: '{0}',", pCampo.Titulo);
                script.AppendLine("minimumInputLength: 4,");
                script.AppendLine("theme: 'bootstrap4',");
                script.AppendLine("language: 'es',");
                script.AppendLine("allowClear: true,");
                script.AppendLine("ajax: {");
                script.AppendLine("type: 'POST',");
                script.AppendLine("contentType: 'application/json; charset=utf-8',");
                script.AppendLine("dataType: 'json',");
                script.AppendFormat("url: '{0}',", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                script.AppendLine("delay: 250,");
                script.AppendLine("data: function (params) {");
                script.AppendLine("return JSON.stringify( {");
                script.AppendLine("value: control.val(), // search term");
                script.AppendLine("filtro: params.term, // search term");
                script.AppendFormat("sp: '{0}',", pCampo.StoredProcedure);
                script.AppendFormat("idRefTablaValor: '{0}',", 0); // this.MiIdRefTablaValor);
                script.AppendLine(" });");
                script.AppendLine("},");
                script.AppendLine("processResults: function (data, params) {");
                script.AppendLine("return {");
                script.AppendLine("results: data.d,");
                script.AppendLine("};");
                script.AppendLine(" cache: true");
                script.AppendLine("},");
                script.AppendLine("}");
                script.AppendLine("});");
                //end select2
                //select2 ON select
                script.AppendLine("control.on('select2:select', function(e) { ");
                script.AppendLine("var newOption = new Option(e.params.data.text, e.params.data.id, false, true);");
                script.AppendFormat("$('select[id$={0}]').append(newOption).trigger('change');", ddlListaOpciones.ID);
                script.AppendFormat("var hdfValue = $('input[name$={0}]');", hdfValue.ID);
                script.AppendLine("hdfValue.val(e.params.data.id);");
                script.AppendLine("});");
                script.AppendLine("control.on('select2:unselect', function(e) { ");
                script.AppendFormat("var hdfValue = $('input[name$={0}]');", hdfValue.ID);
                script.AppendLine("hdfValue.val('');");
                script.AppendLine("});");
                script.AppendLine("};");
                //end function
                script.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitControlSelect2|CTRLID|);");
                script.AppendLine("InitControlSelect2|CTRLID|();");
                script.AppendLine("});");

                script.Replace("|CTRLID|", ddlListaOpciones.ID);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script" + ddlListaOpciones.ID.ToString(), script.ToString(), true);
                #endregion

                divGroupRow.Controls.Add(divContainer);
                divGroupRow.Controls.Add(divContainerBtn);
            }

            HiddenField Text = new HiddenField();
            Text.ID = preid + "txtgvValues" + pCampo.IdCampo.ToString();
            Text.Value = pCampo.CampoValor.Valor;
            divGroupRow.Controls.Add(Text);

            Panel divGrid = new Panel();
            divGrid.CssClass = "container-fluid table-responsive";
            GridView gv = new GridView();
            gv.ID = preid + "GridAB" + pCampo.IdCampo.ToString();
            gv.AutoGenerateColumns = false;
            gv.ShowHeader = true;
            gv.ShowHeaderWhenEmpty = true;
            gv.ShowFooter = true;
            gv.SkinID = "GrillaResponsive";
            DataTable dt = BaseDatos.ObtenerBaseDatos().ObtenerLista(pCampo.ConsultaDinamica, pCampo);
            BoundField bfield;
            foreach (DataColumn col in dt.Columns)
            {
                bfield = new BoundField();
                bfield.DataField = col.ColumnName;
                bfield.HeaderText = col.ColumnName;

                if (col.DataType == typeof(decimal))
                {
                    bfield.DataFormatString = "{0:N2}";
                    bfield.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    bfield.FooterText = dt.AsEnumerable().Take(1000).Sum(r => r.Field<decimal?>(col.ColumnName) ?? 0).ToString("N2");
                    bfield.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                }
                else if (col.DataType == typeof(DateTime))
                    bfield.DataFormatString = "{0:MM/dd/yyyy}";

                gv.Columns.Add(bfield);
            }
            if (pHabilitar)
            {
                gv.RowDataBound += gvAB_RowDataBound;
                gv.RowCommand += gvAB_RowCommand;
                TemplateField tf = new TemplateField();
                tf.HeaderText = "Acciones";
                gv.Columns.Add(tf);
            }
            gv.DataSource = dt;
            gv.DataBind();
            gv.UseAccessibleHeader = true;
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
            divGrid.Controls.Add(gv);

            divGroupRow.Controls.Add(Text);
            divGroupRow.Controls.Add(divGrid);
            divGroupCtrl.Controls.Add(divGroupRow);
            panel.Controls.Add(divGroupCtrl);

            if (!string.IsNullOrWhiteSpace(pCampo.EventoJavaScript))
            {
                StringBuilder scriptGV = new StringBuilder();
                scriptGV.AppendLine("function ControlGrillaAB|CTRLID|() {");
                scriptGV.AppendFormat("{0}", pCampo.EventoJavaScript);
                scriptGV.AppendLine("}");
                scriptGV.Replace("|CTRLID|", gv.ID);
                ScriptManager.RegisterClientScriptBlock(this.pnlCamposDinamicosTurismoServicios, this.pnlCamposDinamicosTurismoServicios.GetType(), "Script" + gv.ID, scriptGV.ToString(), true);
            }
            return panel;
        }
        private void gvAB_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;
                int ultimaColumna = e.Row.Cells.Count - 1;
                ImageButton ibtn = new ImageButton
                {
                    CommandName = "Borrar",
                    CommandArgument = dr[0].ToString(),
                    ImageUrl = "~/Imagenes/Baja.png"
                };
                e.Row.Cells[ultimaColumna].Controls.Add(ibtn);
            }
        }
        private void gvAB_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                GridView gv = (GridView)sender;
                string id = gv.ID;
                string val = e.CommandArgument.ToString();
                TGECampos campo = this.MisCamposTurismo.First(x => x.IdCampo.ToString() == id.Replace(preid + "GridAB", string.Empty));
                List<string> values = campo.CampoValor.Valor.Split(';').ToList();
                if (values.Exists(x => x == val))
                {
                    values.Remove(val);
                    campo.CampoValor.Valor = string.Join(";", values);
                    gv.DataSource = BaseDatos.ObtenerBaseDatos().ObtenerLista(campo.ConsultaDinamica, campo);
                    gv.DataBind();

                    Control CtrlPanel = BuscarControlRecursivo(((GridView)sender).Parent.Parent.Parent.Parent, "panel" + campo.IdCampo);
                    if (CtrlPanel != null)
                    {
                        Control ctrHdv = BuscarControlRecursivo(CtrlPanel, preid + "txtgvValues" + campo.IdCampo);
                        if (ctrHdv != null)
                            ((HiddenField)ctrHdv).Value = campo.CampoValor.Valor;
                    }
                }
                StringBuilder scriptGV = new StringBuilder();
                scriptGV.AppendLine("ControlGrillaAB|CTRLID|();");
                scriptGV.Replace("|CTRLID|", gv.ID);
                ScriptManager.RegisterClientScriptBlock(this.pnlCamposDinamicosTurismoServicios, this.pnlCamposDinamicosTurismoServicios.GetType(), "ScriptBorrar" + gv.ID, scriptGV.ToString(), true);
            }
        }
        private void BtnAgregarGrillaAB_Click(object sender, EventArgs e)
        {
            string idCampo = ((Button)sender).ID.Replace(preid + "btnGridAB", string.Empty);
            string idGV = ((Button)sender).ID.Replace("btnGridAB", "GridAB");
            Control CtrlPanel = BuscarControlRecursivo(((Button)sender).Parent.Parent.Parent.Parent.Parent.Parent, "panel" + idCampo);

            if (MisCamposTurismo != null && MisCamposTurismo.Count > 0)
            {
                TGECampos campo = this.MisCamposTurismo.First(x => x.IdCampo.ToString() == idCampo);
                if (CtrlPanel != null && campo != null)
                {
                    PlaceHolder pnl = (PlaceHolder)CtrlPanel;
                    Control ctrHdv = BuscarControlRecursivo(pnl, preid + "select2HdfValue" + idCampo);
                    if (ctrHdv != null)
                    {
                        ((HiddenField)ctrHdv).Value = string.Empty;
                    }
                    ctrHdv = BuscarControlRecursivo(pnl, preid + "txtgvValues" + idCampo);
                    if (ctrHdv != null && campo != null)
                    {
                        ((HiddenField)ctrHdv).Value = campo.CampoValor.Valor;
                    }
                }
                StringBuilder scriptGV = new StringBuilder();
                scriptGV.AppendLine("ControlGrillaAB|CTRLID|();");
                scriptGV.Replace("|CTRLID|", idGV);
                ScriptManager.RegisterClientScriptBlock(this.pnlCamposDinamicosTurismoServicios, this.pnlCamposDinamicosTurismoServicios.GetType(), "ScriptAgregar" + idGV, scriptGV.ToString(), true);
            }
        }
        protected void gvCuentaCorriente_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;
            PaginaAfiliados paginaAfiliados = new PaginaAfiliados();
            AfiAfiliados afi = paginaAfiliados.Obtener(MiSessionPagina);
            CarTiposCargosAfiliadosFormasCobros parametros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
            parametros.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametros);
            int index = Convert.ToInt32(e.CommandArgument);
            int IdTipoOperacion = Convert.ToInt32(((HiddenField)this.gvCuentaCorriente.Rows[index].FindControl("hdfIdTipoOperacion")).Value);
            int IdRefTipoOperacion = Convert.ToInt32(((HiddenField)this.gvCuentaCorriente.Rows[index].FindControl("hdfIdRefTipoOperacion")).Value);
            this.MisParametrosUrl = new Hashtable();
            Menues filtroMenu = new Menues
            {
                IdTipoOperacion = IdTipoOperacion
            };
            if (e.CommandName == Gestion.Consultar.ToString())
                filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Consulta;
            string idTipoCargoAfiliadoFormaCobro = Request.Form["ctl00$ctl00$ContentPlaceHolder1$cphPrincipal$ModificarDatos$hdfIdTipoCargoAfiliadoFormaCobro"];
            filtroMenu = SeguridadF.MenuesObtenerPorOperacionTipoMenu(filtroMenu);
            this.MisParametrosUrl.Add(filtroMenu.NombreParametro, IdRefTipoOperacion);
            this.MisParametrosUrl.Add("IdAfiliado", afi.IdAfiliado);
            this.MisParametrosUrl.Add("IdTipoOperacion", IdTipoOperacion);
            this.MisParametrosUrl.Add("IdTipoCargoAfiliadoFormaCobro", idTipoCargoAfiliadoFormaCobro);
            if (filtroMenu.URL.Length != 0)
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(string.Concat("~/", filtroMenu.URL)), true);
            }
        }
        protected void gvCuentaCorriente_RowDataBound(object sender, GridViewRowEventArgs e)
        { }
        protected void gvTiposValores_RowDataBound(object sender, GridViewRowEventArgs e)
        { }
        protected void gvPagosValores_RowDataBound(object sender, GridViewRowEventArgs e)
        { }
        #endregion
    }
}