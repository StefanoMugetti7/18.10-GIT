using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Reportes.FachadaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tesorerias;
using Tesorerias.Entidades;

namespace IU.Modulos.Tesoreria.Controles
{
    public partial class CompraVentaMonedaDatos : ControlesSeguros
    {
        private TESCajasMovimientos MiMovimiento
        {
            get { return this.PropiedadObtenerValor<TESCajasMovimientos>("CajasMovimientosDatosMiMovimiento"); }
            set { this.PropiedadGuardarValor("CajasMovimientosDatosMiMovimiento", value); }
        }
        private List<TGEMonedas> MisMonedas
        {
            get { return (List<TGEMonedas>)Session[this.MiSessionPagina + "PrestamoAfiliadoModificarDatosMisMonedas"]; }
            set { Session[this.MiSessionPagina + "PrestamoAfiliadoModificarDatosMisMonedas"] = value; }
        }
        private TESCajas MiCaja
        {
            get { return this.PropiedadObtenerValor<TESCajas>("CajasMovimientosDatosMiCaja"); }
            set { this.PropiedadGuardarValor("CajasMovimientosDatosMiCaja", value); }
        }
        private List<TGETiposOperaciones> MisTiposOperaciones
        {
            get { return this.PropiedadObtenerValor<List<TGETiposOperaciones>>("CajasMovimientosDatosMisTiposOperaciones"); }
            set { this.PropiedadGuardarValor("CajasMovimientosDatosMisTiposOperaciones", value); }
        }
        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.txtImporte.Attributes.Add("onchange", "CalcularTotal(); ");
            }
        }

        public void IniciarControl(TESCajasMovimientos pCajaMovimiento, TESCajas pCaja, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiMovimiento = pCajaMovimiento;
            this.MiCaja = pCaja;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    string funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarAccion"));
                    this.MiMovimiento.Fecha = pCaja.FechaAbrir;
                    this.btnAceptar.Attributes.Add("OnClick", funcion);
                    this.ctrFechaCajaContable.IniciarControl(this.GestionControl, pCaja.FechaAbrir, pCaja.FechaAbrir);
                    this.txtDescripcion.Enabled = true;
                    this.ddlTipoOperacion.Enabled = true;
                    this.ddlMoneda.Enabled = true;
                    this.txtImporteTotal.Enabled = false;
                    this.btnAceptar.Visible = true;
                    break;
                case Gestion.Anular:

                    break;
                case Gestion.Consultar:

                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            AyudaProgramacion.InsertarItemSeleccione(this.ddlMoneda, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            TGETiposOperaciones operaciones = new TGETiposOperaciones();
            operaciones.TipoFuncionalidad.IdTipoFuncionalidad = this.paginaSegura.paginaActual.IdTipoFuncionalidad;
            operaciones.Estado.IdEstado = (int)Estados.Activo;
            this.MisTiposOperaciones = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(operaciones);
            this.ddlTipoOperacion.DataSource = this.MisTiposOperaciones;
            this.ddlTipoOperacion.DataValueField = "IdTipoOperacion";
            this.ddlTipoOperacion.DataTextField = "TipoOperacion";
            this.ddlTipoOperacion.DataBind();
            if (ddlTipoOperacion.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoOperacion, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            else
                ddlTipoOperacion_SelectedIndexChanged(ddlTipoOperacion, EventArgs.Empty);

        }

        protected void ddlTipoOperacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ddlTipoOperacion.SelectedValue))
            {
                TGETiposOperaciones op = MisTiposOperaciones.FirstOrDefault(x => x.IdTipoOperacion == Convert.ToInt32(ddlTipoOperacion.SelectedValue));
                if(op.TipoMovimiento.IdTipoMovimiento==(int)EnumTGETiposMovimientos.Credito)
                    MisMonedas = TGEGeneralesF.MonedasCotizacionesObtenerCombo();
                else
                    MisMonedas = TGEGeneralesF.MonedasCotizacionesObtenerVentaCombo();

                if (this.MisMonedas.Exists(x => x.IdMoneda == (int)EnumTGEMonedas.PesosArgentinos))
                {
                    this.MisMonedas.Remove(this.MisMonedas.Find(x => x.IdMoneda == (int)EnumTGEMonedas.PesosArgentinos));
                    this.MisMonedas = AyudaProgramacion.AcomodarIndices<TGEMonedas>(this.MisMonedas);
                }
                this.ddlMoneda.DataSource = MisMonedas;
                this.ddlMoneda.DataValueField = "IdMoneda";
                this.ddlMoneda.DataTextField = "MonedaDescripcionCotizacion";
                this.ddlMoneda.DataBind();
                if (this.ddlMoneda.Items.Count != 1)
                    AyudaProgramacion.InsertarItemSeleccione(this.ddlMoneda, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                else
                    this.ddlMoneda_SelectedIndexChanged(null, EventArgs.Empty);
            }
        }

        protected void ddlMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlMoneda.SelectedValue))
            {               
                    List<TESCajasMonedas> monedas = this.MiCaja.CajasMonedas;
                    var idMoneda = Convert.ToInt32(this.ddlMoneda.SelectedValue);
                    TESCajasMonedas moneda = monedas.Find(delegate (TESCajasMonedas m) { return m.Moneda.IdMoneda == idMoneda; });
                    SetInitializeCulture(moneda.Moneda.Moneda);
                
                hdfCotizacion.Value = MisMonedas.FirstOrDefault(x => x.IdMoneda == 2).MonedeaCotizacion.MonedaCotizacion.ToString("N2");
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("Aceptar");
            if (!this.Page.IsValid)
            {
                this.upTipoOperacionConceptos.Update();
                return;
            }
            this.btnAceptar.Visible = false;

            this.MiCaja.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            MiMovimiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    #region Agregar
                    var cajaMoneda = this.MiCaja.CajasMonedas.Find(x => x.Moneda.IdMoneda == Convert.ToInt32(this.ddlMoneda.SelectedValue));
                    MiMovimiento.TipoOperacion = this.MisTiposOperaciones[this.ddlTipoOperacion.SelectedIndex];
                    MiMovimiento.Fecha = this.MiCaja.FechaAbrir;// DateTime.Now;
                    MiMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
                    MiMovimiento.Descripcion = this.txtDescripcion.Text.Trim();
                    MiMovimiento.Importe = txtImporte.Decimal;
                    MiMovimiento.CajaMoneda = cajaMoneda;
                    MiMovimiento.MonedaCotizacion = MisMonedas.FirstOrDefault(x => x.IdMoneda == 2).MonedeaCotizacion.MonedaCotizacion;
                    MiMovimiento.Afiliado.IdAfiliado = ddlApellido.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlApellido.SelectedValue);


                    if (MiMovimiento.Importe <= 0)
                    {
                        this.MostrarMensaje("ValidarCantidadItems", true);
                        this.btnAceptar.Visible = true;
                        return;
                    }

                    MiMovimiento.CajasMovimientosValores = new List<TESCajasMovimientosValores>();
                    TESCajasMovimientosValores valor = new TESCajasMovimientosValores();
                    valor.Importe = txtImporte.Decimal; 
                    valor.TipoValor.IdTipoValor = (int)EnumTiposValores.Efectivo;
                    MiMovimiento.CajasMovimientosValores.Add(valor);
                        this.MiMovimiento = TesoreriasF.CajasMovimientosObtenerMovimientoValoresXML(this.MiMovimiento);
                        this.MiMovimiento.IdFilial = this.MiCaja.Tesoreria.Filial.IdFilial;
                        this.MiMovimiento.Fecha = this.ctrFechaCajaContable.dFechaCajaContable.Value;

                    if (!TesoreriasF.CajasConfirmarMovimientoXml(this.MiMovimiento))
                    {
                        this.btnAceptar.Visible = true;
                        this.MostrarMensaje(this.MiMovimiento.CodigoMensaje, true, this.MiMovimiento.CodigoMensajeArgs);
                        if (this.MiMovimiento.dsResultado != null)
                        {
                     
                            this.MiMovimiento.dsResultado = null;
                        }
                    }
                    else
                    {
                        this.MostrarMensaje(this.MiMovimiento.CodigoMensaje, false);
                        //this.IniciarControl(this.MiMovimiento, this.MiCaja, Gestion.Consultar);
                        this.btnImprimir.Visible = true;
                        this.upTipoOperacionConceptos.Update();
             
                    }
                    #endregion
                    break;
                case Gestion.Anular:
                   
                    break;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosAutomaticos.aspx"), true);
        }

        protected void btnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ComprobantePlantillaCajas);
            bool bvalor = !string.IsNullOrEmpty(valor.ParametroValor) ? Convert.ToBoolean(valor.ParametroValor) : false;
            int idComprobante;
            if (this.MiMovimiento.IdCajaMovimiento > 0 && bvalor)
            {
                idComprobante = this.MiMovimiento.IdCajaMovimiento;
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.TESCajasMovimientos, "TESCajasMovimientos", this.MiMovimiento, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this.upTipoOperacionConceptos, string.Concat("Comprobante_", idComprobante.ToString().PadLeft(10, '0')), this.UsuarioActivo);
            }
            else
            {
                Type t2 = MiMovimiento.GetType();
                PropertyInfo prop = t2.GetProperty("TipoOperacion");
                TGETiposOperaciones operacion = (TGETiposOperaciones)prop.GetValue(this.MiMovimiento, null);
                prop = this.MiMovimiento.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() == 1).ToList()[0];
                idComprobante = Convert.ToInt32(prop.GetValue(this.MiMovimiento, null));

                Object comprobante = Enum.Parse(typeof(EnumTGEComprobantes), this.MiMovimiento.GetType().Name);

                if (comprobante != null)
                {
                    TGEPlantillas plantilla = new TGEPlantillas();
                    plantilla = TGEGeneralesF.PlantillasObtenerDatosPorComprobante((EnumTGEComprobantes)comprobante);

                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF((EnumTGEComprobantes)comprobante, plantilla.Codigo, this.MiMovimiento, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.upTipoOperacionConceptos, string.Concat("Comprobante_", idComprobante.ToString().PadLeft(10, '0')), this.UsuarioActivo);
                }
            }

            //this.ctrPopUpComprobantes.CargarReporte(this.MiMovimiento, EnumTGEComprobantes.TESCajasMovimientos);
        }
                
    }
}