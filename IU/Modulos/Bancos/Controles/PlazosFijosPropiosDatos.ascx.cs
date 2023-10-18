using Bancos;
using Bancos.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace IU.Modulos.Bancos.Controles
{
    public partial class PlazosFijosPropiosDatos : ControlesSeguros
    {
        private TESPlazosFijos MiPlazoFijoPropio
        {
            get { return (TESPlazosFijos)Session[MiSessionPagina + "PlazosFijosMiPlazoFijoPropio"]; }
            set { Session[MiSessionPagina + "PlazosFijosMiPlazoFijoPropio"] = value; }
        }
        private List<TGEMonedas> MisMonedas
        {
            get { return (List<TGEMonedas>)Session[MiSessionPagina + "PlazosFijosDatosMisMonedas"]; }
            set { Session[MiSessionPagina + "PlazosFijosDatosMisMonedas"] = value; }
        }
        public delegate void PlazosFijosPropiosDatosAceptarEventHandler(object sender, TESPlazosFijos e);
        public event PlazosFijosPropiosDatosAceptarEventHandler PlazosFijosPropiosDatosAceptar;
        public delegate void PlazosFijosPropiosDatosCancelarEventHandler();
        public event PlazosFijosPropiosDatosCancelarEventHandler PlazosFijosPropiosDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.txtImporte.Attributes.Add("onblur", "CalcularTotal();");
                this.txtImporteInteres.Attributes.Add("onblur", "CalcularTotal();");
            }
        }
        public void IniciarControl(TESPlazosFijos pPlazoFijo, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiPlazoFijoPropio = pPlazoFijo;
            switch (GestionControl)
            {
                case Gestion.Agregar:
                    this.CargarCombos();
                    this.txtFechaInicioVigencia.Text = DateTime.Today.ToShortDateString();
                    this.txtFechaInicioVigencia.Enabled = true;
                    this.txtSaldoActual.Enabled = false;
                    this.txtFechaVencimiento.Enabled = true;
                    this.txtFechaInicioVigencia.Enabled = true;
                    this.txtImporteInteres.Enabled = true;
                    break;
                case Gestion.Consultar:
                    this.MiPlazoFijoPropio = BancosF.PlazosFijosObtenerDatosCompletos(pPlazoFijo);
                    this.CargarCombos();
                    this.MapearObjetoAControles(this.MiPlazoFijoPropio);
                    this.ddlCuenta_SelectedIndexChanged(this.MiPlazoFijoPropio, EventArgs.Empty);
                    this.ddlCuenta.Enabled = false;
                    this.txtFechaVencimiento.Enabled = false;
                    this.txtDescripcion.Enabled = false;
                    this.txtFechaInicioVigencia.Enabled = false;
                    this.txtImporte.Enabled = false;
                    this.ddlTipoRenovacion.Enabled = false;
                    this.btnAceptar.Visible = false;
                    this.txtSaldoActual.Enabled = false;
                    this.txtImporteInteres.Enabled = false;
                    ScriptManager.RegisterStartupScript(this.upSaldoActual, this.upSaldoActual.GetType(), "CalcularTotalScript", "CalcularTotal();", true);
                    break;
                case Gestion.Anular:
                    this.MiPlazoFijoPropio = BancosF.PlazosFijosObtenerDatosCompletos(pPlazoFijo);
                    this.CargarCombos();
                    this.txtDescripcion.Enabled = false;
                    this.MapearObjetoAControles(this.MiPlazoFijoPropio);
                    this.ddlCuenta_SelectedIndexChanged(this.MiPlazoFijoPropio, EventArgs.Empty);
                    this.ddlCuenta.Enabled = false;
                    this.txtFechaVencimiento.Enabled = false;
                    this.txtFechaInicioVigencia.Enabled = false;
                    this.txtImporte.Enabled = false;
                    this.ddlTipoRenovacion.Enabled = false;
                    this.btnAceptar.Visible = true;
                    this.txtSaldoActual.Enabled = false;
                    this.txtImporteInteres.Enabled = false;
                    ScriptManager.RegisterStartupScript(this.upSaldoActual, this.upSaldoActual.GetType(), "CalcularTotalScript", "CalcularTotal();", true);
                    break;
                case Gestion.Pagar:
                    this.MiPlazoFijoPropio = BancosF.PlazosFijosObtenerDatosCompletos(pPlazoFijo);
                    this.CargarCombos();
                    this.txtDescripcion.Enabled = false;
                    this.MapearObjetoAControles(this.MiPlazoFijoPropio);
                    this.ddlCuenta_SelectedIndexChanged(this.MiPlazoFijoPropio, EventArgs.Empty);
                    this.ddlCuenta.Enabled = false;
                    this.txtFechaVencimiento.Enabled = false;
                    this.txtFechaInicioVigencia.Enabled = false;
                    this.txtImporte.Enabled = false;
                    this.ddlTipoRenovacion.Enabled = false;
                    this.btnAceptar.Visible = true;
                    this.txtSaldoActual.Enabled = false;
                    this.txtImporteInteres.Enabled = false;
                    ScriptManager.RegisterStartupScript(this.upSaldoActual, this.upSaldoActual.GetType(), "CalcularTotalScript", "CalcularTotal();", true);
                    break;
                case Gestion.Modificar:
                    this.MiPlazoFijoPropio = BancosF.PlazosFijosObtenerDatosCompletos(pPlazoFijo);
                    this.CargarCombos();
                    this.MapearObjetoAControles(this.MiPlazoFijoPropio);
                    this.ddlCuenta_SelectedIndexChanged(this.MiPlazoFijoPropio, EventArgs.Empty);
                    this.ddlCuenta.Enabled = false;
                    this.txtImporte.Enabled = false;
                    this.txtSaldoActual.Enabled = false;
                    this.txtFechaInicioVigencia.Enabled = false;
                    this.txtImporteInteres.Enabled = false;
                    this.ddlTipoRenovacion.Enabled = true;
                    this.btnAceptar.Visible = true;
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            this.MisMonedas = TGEGeneralesF.MonedasCotizacionesObtenerCombo();

            this.ddlCuenta.DataSource = BancosF.CuentasObtenerListaFiltro(this.MiPlazoFijoPropio.BancoCuenta);
            this.ddlCuenta.DataValueField = "IdBancoCuenta";
            this.ddlCuenta.DataTextField = "CuentaDatos";
            this.ddlCuenta.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCuenta, ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void MapearObjetoAControles(TESPlazosFijos pPlazosFijos)
        {
            this.ddlCuenta.SelectedValue = pPlazosFijos.BancoCuenta.IdBancoCuenta.ToString();
            this.txtImporte.Text = pPlazosFijos.ImporteCapital.ToString("C2");
            this.txtDescripcion.Text = pPlazosFijos.Descripcion;
            this.txtImporteInteres.Text = pPlazosFijos.ImporteInteres.ToString("C2");
            this.txtFechaInicioVigencia.Text = pPlazosFijos.FechaInicioVigencia.ToShortDateString();
            this.txtFechaVencimiento.Text = pPlazosFijos.FechaVencimiento.ToShortDateString();
            this.txtSaldoActual.Text = pPlazosFijos.BancoCuenta.SaldoActual.ToString("C2");

            this.ctrArchivos.IniciarControl(pPlazosFijos, this.GestionControl);
        }
        private void MapearControlesAObjeto(TESPlazosFijos pPlazosFijos)
        {
            pPlazosFijos.BancoCuenta.IdBancoCuenta = this.ddlCuenta.SelectedValue == "" ? 0 : Convert.ToInt32(this.ddlCuenta.SelectedValue);
            pPlazosFijos.Descripcion = this.txtDescripcion.Text;
            pPlazosFijos.ImporteCapital = this.txtImporte.Decimal;
            pPlazosFijos.ImporteInteres = this.txtImporteInteres.Decimal;
            pPlazosFijos.FechaInicioVigencia = Convert.ToDateTime(this.txtFechaInicioVigencia.Text);
            pPlazosFijos.FechaVencimiento = Convert.ToDateTime(this.txtFechaVencimiento.Text);
            TGEMonedas mon = MisMonedas.Find(x => x.IdMoneda == this.MiPlazoFijoPropio.BancoCuenta.Moneda.IdMoneda);
            pPlazosFijos.BancoCuenta.Moneda.IdMoneda = mon.IdMoneda;
            pPlazosFijos.MonedaCotizacion = mon.MonedeaCotizacion.MonedaCotizacion;
            pPlazosFijos.Archivos = this.ctrArchivos.ObtenerLista();
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;

            this.MiPlazoFijoPropio.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (GestionControl)
            {
                case Gestion.Agregar:
                    this.MapearControlesAObjeto(this.MiPlazoFijoPropio);
                    this.MiPlazoFijoPropio.FechaAlta = DateTime.Now;
                    this.MiPlazoFijoPropio.UsuarioAlta.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    this.MiPlazoFijoPropio.BancoCuenta.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    guardo = BancosF.PlazosFijosAgregar(this.MiPlazoFijoPropio);
                    break;
                case Gestion.Anular:
                    this.MiPlazoFijoPropio.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    guardo = BancosF.PlazosFijosBorrar(this.MiPlazoFijoPropio);
                    break;
                case Gestion.Pagar:
                    this.MiPlazoFijoPropio.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    guardo = BancosF.PlazosFijosPagar(this.MiPlazoFijoPropio);
                    break;
                case Gestion.Modificar:
                    this.MapearControlesAObjeto(this.MiPlazoFijoPropio);
                    this.MiPlazoFijoPropio.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    guardo = BancosF.PlazosFijosModificar(this.MiPlazoFijoPropio);
                    break;
            }
            if (guardo)
            {
                this.btnAceptar.Visible = false;
                this.GestionControl = Gestion.Consultar;
                this.MostrarMensaje(this.MiPlazoFijoPropio.CodigoMensaje, false);
            }
            else
            {
                this.MostrarMensaje(this.MiPlazoFijoPropio.CodigoMensaje, true, MiPlazoFijoPropio.CodigoMensajeArgs);
                if (this.MiPlazoFijoPropio.dsResultado != null)
                {
                    this.ctrPopUpGrilla.IniciarControl(this.MiPlazoFijoPropio);
                    this.MiPlazoFijoPropio.dsResultado = null;
                }
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.PlazosFijosPropiosDatosCancelar?.Invoke();
        }
        protected void ddlCuenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlCuenta.SelectedValue))
            {
                this.MiPlazoFijoPropio.BancoCuenta.IdBancoCuenta = Convert.ToInt32(this.ddlCuenta.SelectedValue);
                this.MiPlazoFijoPropio.BancoCuenta = BancosF.BancosCuentasObtenerDatosCompletos(this.MiPlazoFijoPropio.BancoCuenta);
                this.SetInitializeCulture(this.MiPlazoFijoPropio.BancoCuenta.Moneda.Moneda);
                this.txtSaldoActual.Text = this.MiPlazoFijoPropio.BancoCuenta.SaldoActual.ToString("C2");
                this.upSaldoActual.Update();
                this.upTasaInteres.Update();
            }
            else
            {
                this.txtSaldoActual.Text = "0";
                this.upTasaInteres.Update();
            }
        }
    }
}