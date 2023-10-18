using Afiliados;
using Afiliados.Entidades;
using Bancos;
using Bancos.Entidades;
using Cargos;
using Cargos.Entidades;
using Compras;
using Compras.Entidades;
using Comunes.Entidades;
using Contabilidad;
using Contabilidad.Entidades;
using Evol.Controls;
using Facturas;
using Facturas.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Hoteles.Entidades;
using Proveedores;
using Proveedores.Entidades;
using Reportes.Entidades;
using Reportes.FachadaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Facturas.Controles
{
    public partial class FacturasDatos : ControlesSeguros
    {
        const int CantidadItems = 2;
        private enum ModuloFactura
        {
            Ventas,
            Hotel,
            ReservasTurismo,
        }
        private ModuloFactura MiModuloFactura
        {
            get { return (ModuloFactura)Session[this.MiSessionPagina + "FacturasDatosMiModuloFactura"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMiModuloFactura"] = value; }
        }
        private int MiCantidadDecimales
        {
            get { return (int)Session[this.MiSessionPagina + "FacturasDatosCantidadDecimales"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosCantidadDecimales"] = value; }
        }
        private decimal MiDescuentoSocio
        {
            get { return (decimal)Session[this.MiSessionPagina + "FacturasDatosMiDescuentoSocio"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMiDescuentoSocio"] = value; }
        }
        private VTAFacturas MiFactura
        {
            get { return (VTAFacturas)Session[this.MiSessionPagina + "FacturasDatosMiFactura"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMiFactura"] = value; }
        }
        private VTARemitos MiRemito
        {
            get { return (VTARemitos)Session[this.MiSessionPagina + "FacturasDatosMiRemito"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMiRemito"] = value; }
        }
        private VTANotasPedidos MiNotaPedido
        {
            get { return (VTANotasPedidos)Session[this.MiSessionPagina + "FacturasDatosMiNotaPedido"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMiNotaPedido"] = value; }
        }
        private List<TGEIVA> MisIvas
        {
            get { return (List<TGEIVA>)Session[this.MiSessionPagina + "FacturasDatosMisIvas"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMisIvas"] = value; }
        }
        private List<TGEMonedas> MisMonedas
        {
            get { return (List<TGEMonedas>)Session[this.MiSessionPagina + "FacturasDatosMisMonedas"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMisMonedas"] = value; }
        }
        private List<VTATiposPuntosVentas> MisTiposPuntosVentas
        {
            get { return (List<VTATiposPuntosVentas>)Session[this.MiSessionPagina + "FacturasDatosMisTiposPuntosVentas"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMisTiposPuntosVentas"] = value; }
        }
        private List<VTAFilialesPuntosVentas> MisFilialesPuntosVentas
        {
            get { return (List<VTAFilialesPuntosVentas>)Session[this.MiSessionPagina + "FacturasDatosMisFilialesPuntosVentas"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMisFilialesPuntosVentas"] = value; }
        }
        private List<VTAFilialesPuntosVentas> MisFilialesPuntosVentasRemitos
        {
            get { return (List<VTAFilialesPuntosVentas>)Session[this.MiSessionPagina + "FacturasDatosMisFilialesPuntosVentasRemitos"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMisFilialesPuntosVentasRemitos"] = value; }
        }
        private List<TGETiposFacturas> MisTiposFacturas
        {
            get { return (List<TGETiposFacturas>)Session[this.MiSessionPagina + "FacturasDatosMisTiposFacturas"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMisTiposFacturas"] = value; }
        }
        private List<TGETiposFacturas> MisTiposFacturasRemitos
        {
            get { return (List<TGETiposFacturas>)Session[this.MiSessionPagina + "FacturasDatosMisTiposFacturasRemitos"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMisTiposFacturasRemitos"] = value; }
        }
        private List<TGEListasValoresSistemasDetalles> MisConceptosComprobantes
        {
            get { return (List<TGEListasValoresSistemasDetalles>)Session[this.MiSessionPagina + "FacturasDatosMisConceptosComprobantes"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMisConceptosComprobantes"] = value; }
        }
        private List<TGEListasValoresSistemasDetalles> MisPercepciones
        {
            get { return (List<TGEListasValoresSistemasDetalles>)Session[this.MiSessionPagina + "FacturasDatosMisPercepciones"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMisPercepciones"] = value; }
        }
        private List<CtbCentrosCostosProrrateos> MisCentrosCostos
        {
            get { return (List<CtbCentrosCostosProrrateos>)Session[this.MiSessionPagina + "FacturasDatosMisCentrosCostos"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMisCentrosCostos"] = value; }
        }
        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "FacturaModificarDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "FacturaModificarDatosMiIndiceDetalleModificar"] = value; }
        }
        //public delegate void FacturasDatosAceptarEventHandler(object sender, VTAFacturas e);
        //public event FacturasDatosAceptarEventHandler FacturaModificarDatosAceptar;
        public delegate void FacturasDatosCancelarEventHandler();
        public event FacturasDatosCancelarEventHandler FacturaModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            //this.ctrBuscarClientePopUp.AfiliadosBuscarSeleccionar += new Afiliados.Controles.ClientesBuscarPopUp.AfiliadosBuscarEventHandler(ctrBuscarClientePopUp_AfiliadosBuscarSeleccionar);
            //this.ctrBuscarProductoPopUp.ProductosBuscarSeleccionar += new BuscarProductoPopUp.ProductosBuscarEventHandler(ctrBuscarProductoPopUp_ProductosBuscarSeleccionar);
            this.ctrBuscarCliente.BuscarCliente += new Afiliados.Controles.ClientesDatosCabeceraAjax.ClientesDatosCabeceraAjaxEventHandler(this.CtrBuscarCliente_BuscarCliente);
            this.ctrBuscarRemitoPopUp.RemitosBuscarSeleccionar += new RemitosBuscarPopUp.RemitosBuscarEventHandler(this.ctrBuscarRemitoPopUp_RemitosBuscarSeleccionar);
            this.ctrBuscarPresupuestoPopUp.PresupuestosBuscarSeleccionar += this.CtrBuscarPresupuestoPopUp_PresupuestosBuscarSeleccionar;
            this.ctrBuscarNotaPedidoPopUp.NotasPedidosBuscarSeleccionar += this.ctrBuscarNotaPedidoPopUp_NotasPedidoBuscarSeleccionar;
            this.ctrDomicilios.AfiliadosModificarDatosAceptar += this.CtrDomicilios_AfiliadosModificarDatosAceptar;
            if (!this.IsPostBack)
            {
                if (this.MiFactura == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
                this.ddlCantidadCuotas.Attributes.Add("onchange", "CalcularItem();");
                //this.ddlConceptoComprobante.Attributes.Add("onchange", "HabilitarPeriodosFechas();");
            }
            else
            {
                if (this.GestionControl == Gestion.Agregar)
                {
                    //if (!string.IsNullOrWhiteSpace(this.ddlMonedas.SelectedValue))
                    //{
                    //    TGEMonedas moneda = MisMonedas.FirstOrDefault(x => x.IdMoneda == Convert.ToInt32(this.ddlMonedas.SelectedValue));
                    //    SetInitializeCulture(moneda.Moneda);
                    //}
                    this.PersistirDatosGrilla();
                    this.PersistirPercepciones();
                }
            }
        }
        public void IniciarControl(VTAFacturas pFactura, Gestion pGestion)
        {
            this.MiModuloFactura = ModuloFactura.Ventas;
            this.GestionControl = pGestion;
            this.MiFactura = pFactura;
            this.MisPercepciones = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.AFIPTiposPercepciones);
            this.MiCantidadDecimales = 2;
            this.CargarCombos();
            CtbCentrosCostosProrrateos ccp = new CtbCentrosCostosProrrateos();
            ccp.UsuarioLogueado.IdUsuarioEvento = this.UsuarioActivo.IdUsuarioEvento;
            this.MisCentrosCostos = ContabilidadF.CentrosCostosProrrateosObtenerCombo(ccp);
            this.hdfIdFilialPredeterminada.Value = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
            this.hdfIdUsuarioEvento.Value = this.UsuarioActivo.IdUsuarioEvento.ToString();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.RemitoVentaAutomatico);

                    bool bvalor = valor.ParametroValor.Trim() == string.Empty ? false : Convert.ToBoolean(valor.ParametroValor);
                    this.chkGenerarRemito.Checked = bvalor;
                    this.btnImportarRemito.Visible = !bvalor;
                    this.btnImportarPresupuesto.Visible = true;
                    this.btnImportarNotaPedido.Visible = true;
                    valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.RemitoVentaConfirmacionPopUp);
                    bvalor = valor.ParametroValor.Trim() == string.Empty ? false : Convert.ToBoolean(valor.ParametroValor);
                    if (bvalor)
                    {
                        string funcion = string.Format("ValidarShowConfirm(this,'{0}');", this.ObtenerMensajeSistema("RemitoVentaConfirmacionPopUp"));
                        this.btnAceptar.Attributes.Add("OnClick", funcion);
                    }
                    valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.FacturasPorcentajeDescuentoSocios);

                    this.MiDescuentoSocio = valor.ParametroValor.Trim() == string.Empty ? 0 : Convert.ToDecimal(valor.ParametroValor);
                    this.ddlCantidadDecimales.Enabled = true;
                    this.txtObservacion.Enabled = true;
                    this.txtObservacionComprobante.Enabled = true;
                    this.txtFechaFactura.Text = DateTime.Now.ToShortDateString();
                    this.txtFechaVencimiento.Text = DateTime.Now.AddDays(10).ToShortDateString();
                    this.MiFactura.Filial = this.UsuarioActivo.FilialPredeterminada;
                    this.txtFechaFactura.Enabled = true;
                    this.txtCuit.Enabled = true;
                    this.txtDomicilio.Enabled = true;
                    this.txtLocalidad.Enabled = true;
                    this.ddlTipoDocumentoCliente.Enabled = true;
                    this.ddlCondicionFiscalCliente.Enabled = true;
                    this.txtRazonSocialCliente.Enabled = true;
                    if (this.MisTiposPuntosVentas.Count == 0)
                    {
                        List<string> lista = new List<string>
                        {
                            this.UsuarioActivo.FilialPredeterminada.Filial,
                            this.UsuarioActivo.Usuario
                        };
                        this.MostrarMensaje("ValidarFilialPuntosVentas", true, lista);
                        this.btnAceptar.Visible = false;
                        return;
                    }
                    if (this.MisParametrosUrl.Contains("IdAfiliado"))
                    {
                        //this.hdfIdAfiliado.Value = this.MisParametrosUrl["IdAfiliado"].ToString();
                        this.ctrBuscarCliente.BuscarCliente += new Afiliados.Controles.ClientesDatosCabeceraAjax.ClientesDatosCabeceraAjaxEventHandler(CtrBuscarCliente_BuscarCliente);
                        this.MiFactura.Afiliado.IdAfiliado = Convert.ToInt32(this.MisParametrosUrl["IdAfiliado"].ToString());
                        if (this.MisParametrosUrl.Contains("IdTipoCargoAfiliadoFormaCobro"))
                            this.ctrBuscarCliente.IniciarControl(this.MiFactura.Afiliado, Gestion.Consultar);
                        else
                            this.ctrBuscarCliente.IniciarControl(this.MiFactura.Afiliado, this.GestionControl);
                        //button_Click(null, EventArgs.Empty);
                        //this.txtNumeroSocio.Text = this.MisParametrosUrl["IdAfiliado"].ToString();
                        //this.txtNumeroSocio_TextChanged(this.txtNumeroSocio, EventArgs.Empty);
                        VTAFacturas factura = new VTAFacturas();
                        factura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
                        factura.Afiliado.IdAfiliado = this.MiFactura.Afiliado.IdAfiliado;
                        factura = FacturasF.FacturasObtenerDatosPreCargados(factura);
                        if (factura.Filtro == "Precargados")
                        {
                            this.MapearObjetoControlesPrecarga(factura);
                        }
                    }
                    this.IniciarGrilla();

                    /*Reservas de Hoteles*/
                    if (this.MisParametrosUrl.Contains("IdReserva"))
                    {
                        this.MiModuloFactura = ModuloFactura.Hotel;
                        this.ddlConceptoComprobante.SelectedValue = ((int)EnumConceptosComprobantes.ProductosYServicios).ToString();
                        this.ddlConceptoComprobante_SelectedIndexChanged(null, EventArgs.Empty);
                        this.txtPeriodoFechaDesde.Text = this.MisParametrosUrl.Contains("FechaIngreso") ? Convert.ToDateTime(this.MisParametrosUrl["FechaIngreso"]).ToShortDateString() : string.Empty;
                        this.txtPeriodoFechaHasta.Text = this.MisParametrosUrl.Contains("FechaEgreso") ? Convert.ToDateTime(this.MisParametrosUrl["FechaEgreso"]).ToShortDateString() : string.Empty;
                        this.MiFactura.IdRefTabla = Convert.ToInt64(this.MisParametrosUrl["IdReserva"].ToString());
                        this.MiFactura.Tabla = typeof(HTLReservas).Name;
                        this.MiFactura.FacturasDetalles = FacturasF.FacturasDetallesObtenerDetallesPendientesPorRefTabla(this.MiFactura);
                        AyudaProgramacion.CargarGrillaListas<VTAFacturasDetalles>(pFactura.FacturasDetalles, false, this.gvItems, true);
                        this.hdfNoCalculaImporteDescuento.Value = "1";
                        this.CalcularTotal();
                        ScriptManager.RegisterStartupScript(this.upItems, this.upItems.GetType(), "CalcularItemScript", "CalcularItem();", true);
                        this.MisParametrosUrl.Remove("IdReserva");
                        this.lblCantidadAgregar.Visible = false;
                        this.txtCantidadAgregar.Visible = false;
                        //this.txtNumeroSocio.Enabled = false;
                        //this.btnBuscarSocio.Enabled = false;
                        this.btnAgregarItem.Visible = false;
                        this.upRemitos.Visible = false;
                        this.btnImportarRemito.Visible = false;
                        this.btnImportarPresupuesto.Visible = false;
                        /* Mas adelante ver esto. */
                        this.ddlMonedas.SelectedValue = 1.ToString();
                        this.ddlMonedas.Enabled = false;
                    }
                    else if (this.MisParametrosUrl.Contains("IdTipoCargoAfiliadoFormaCobro"))
                    {
                        this.MiModuloFactura = ModuloFactura.ReservasTurismo;
                        CarTiposCargosAfiliadosFormasCobros Cargo = new CarTiposCargosAfiliadosFormasCobros();

                        Cargo.IdTipoCargoAfiliadoFormaCobro = Convert.ToInt32(this.MisParametrosUrl["IdTipoCargoAfiliadoFormaCobro"].ToString());
                        Cargo = CargosF.TiposCargosAfiliadosObtenerDatosCompletos(Cargo);

                        this.hdfModulo.Value = "turismo";
                        this.ddlMonedas.DataSource = null;

                        TGEMonedasCotizaciones moneda = new TGEMonedasCotizaciones();
                        moneda.IdMoneda = Cargo.Moneda.IdMoneda;
                        moneda = TGEGeneralesF.MonedasCotizacionesObtenerCotizacionVenta(moneda);

                        this.ddlMonedas.SelectedValue = ((int)EnumTGEMonedas.PesosArgentinos).ToString();
                        //ddlMonedas_OnSelectedIndexChanged(null, EventArgs.Empty);
                        this.ddlMonedas.Enabled = false;
                        this.MiFactura.IdRefTabla = Convert.ToInt64(this.MisParametrosUrl["IdTipoCargoAfiliadoFormaCobro"].ToString());
                        this.MiFactura.Tabla = typeof(CarTiposCargosAfiliadosFormasCobros).Name;
                        this.MiFactura.MonedaCotizacionDolar = moneda.MonedaCotizacion;
                        if (this.MisParametrosUrl.Contains("FechaSalida"))
                            this.txtPeriodoFechaDesde.Text = this.MisParametrosUrl["FechaSalida"].ToString();
                        if (this.MisParametrosUrl.Contains("FechaRegreso"))
                            txtPeriodoFechaHasta.Text = this.MisParametrosUrl["FechaRegreso"].ToString();
                        this.ddlConceptoComprobante.SelectedValue = ((int)EnumConceptosComprobantes.Servicios).ToString();
                        this.ddlConceptoComprobante_SelectedIndexChanged(null, EventArgs.Empty);
                        this.MiFactura.PorcentajeTurismo = 100;
                        this.MiFactura.FacturasDetalles = FacturasF.FacturasDetallesObtenerDetallesServiciosPendientesPorRefTabla(this.MiFactura);
                        this.MiFactura.FacturasTiposPercepciones = FacturasF.FacturasPercepcionesObtenerPorIdRefTabla(this.MiFactura);
                        AyudaProgramacion.CargarGrillaListas<VTAFacturasDetalles>(pFactura.FacturasDetalles, false, this.gvItems, true);

                        this.lblCantidadAgregar.Visible = false;
                        this.txtCantidadAgregar.Visible = false;
                        this.btnAgregarItem.Visible = false;
                        this.upRemitos.Visible = false;
                        //this.btnImportarRemito.Visible = false;
                        //this.btnImportarPresupuesto.Visible = false;
                        this.btnImportarCosas.Visible = false;
                        this.ddlMonedas.Enabled = false;

                        if (this.MiFactura.FacturasTiposPercepciones.Count > 0)
                        {
                            AyudaProgramacion.CargarGrillaListas<VTAFacturasTiposPercepciones>(pFactura.FacturasTiposPercepciones, false, this.gvPercepciones, true);
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CalcularPercepcion", "CalcularPercepcion();", true);
                        }

                        this.hdfNoCalculaImporteDescuento.Value = "1";
                        this.CalcularTotal();

                        TGEMonedas mon = MisMonedas.FirstOrDefault(x => x.IdMoneda == Convert.ToInt32(this.ddlMonedas.SelectedValue));
                        if (mon != null)
                            this.SetInitializeCulture(mon.Moneda);

                        this.phTurismoPorcentajeFactura.Visible = true;

                        StringBuilder script = new StringBuilder();
                        script.AppendLine("$(document).ready(function () {");
                        script.AppendLine("CalcularItem();");
                        script.AppendLine("});");
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CalcularItemScript", script.ToString(), true);
                        //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CalcularItemScript", "CalcularItem();", true);
                        this.MisParametrosUrl.Remove("FechaSalida");
                        this.MisParametrosUrl.Remove("FechaRegreso");
                        this.MisParametrosUrl.Remove("IdTipoCargoAfiliadoFormaCobro");
                    }


                    //this.phRemitos.Visible = this.MiModuloFactura == ModuloFactura.Ventas;
                    this.ctrCamposValores.IniciarControl(this.MiFactura, new Objeto(), this.GestionControl);
                    if (this.paginaSegura.MenuPadre == EnumMenues.Afiliados)
                    {
                        this.ctrBuscarCliente.IniciarControl(this.MiFactura.Afiliado, this.GestionControl);
                    }
                    this.txtCantidadAgregar.Enabled = true;
                    this.phAplicarPorcentaje.Visible = true;
                    this.txtAplicarPorcentajeDescuento.Attributes.Add("onchange", "AplicarPorcentaje();");
                    if (this.MisParametrosUrl.Contains("UrlReferrer"))
                        this.paginaSegura.viewStatePaginaSegura = this.MisParametrosUrl["UrlReferrer"].ToString();

                    break;
                case Gestion.Anular:
                    this.MiCantidadDecimales = 4;
                    this.MiFactura = FacturasF.FacturasObtenerDatosCompletos(pFactura);
                    this.MapearObjetoControles(this.MiFactura);
                    this.btnTxtCuitBlur.Visible = false;
                    this.btnImportarCosas.Visible = false;
                    //this.ddlNumeroSocio.Enabled = false;
                    //this.txtNumeroSocio.Enabled = false;
                    //this.btnBuscarSocio.Enabled = false;
                    this.txtFechaFactura.Enabled = false;
                    this.ddlCantidadCuotas.Enabled = false;
                    this.ddlFilialPuntoVenta.Enabled = false;
                    this.ddlTipoFactura.Enabled = false;
                    this.ddlConceptoComprobante.Enabled = false;
                    this.ddlMonedas.Enabled = false;
                    this.txtObservacion.Enabled = false;
                    this.btnAgregarItem.Visible = false;
                    this.btnImportarRemito.Visible = false;
                    this.btnAceptar.Visible = true;
                    this.btnImportarPresupuesto.Visible = false;
                    this.btnImportarNotaPedido.Visible = false;
                    //this.chkGenerarRemito.Enabled = false;
                    this.chkGenerarRemito.InputAttributes.Add("disabled", "disabled");
                    this.ddlEstadosRemtios.Enabled = false;
                    this.ddlPrefijoNumeroFactura.Enabled = false;
                    this.txtPeriodoFechaDesde.Enabled = false;
                    this.txtPeriodoFechaHasta.Enabled = false;
                    this.ddlTipoRemito.Enabled = false;
                    this.ddlListaPrecio.Enabled = false;
                    break;
                case Gestion.Modificar:
                    this.MiCantidadDecimales = 4;
                    this.MiFactura = FacturasF.FacturasObtenerDatosCompletos(pFactura);
                    this.MisTiposFacturas = FacturasF.TiposFacturasSeleccionarPorCondicionFiscal(this.MiFactura);
                    if (this.MiFactura.Estado.IdEstado == (int)EstadosFacturas.FESinValidadaAfip || this.MiFactura.Estado.IdEstado == (int)EstadosFacturas.Activo)
                    {
                        this.txtFechaFactura.Enabled = true;
                        this.txtCuit.Enabled = true;
                        this.txtDomicilio.Enabled = true;
                        this.txtLocalidad.Enabled = true;
                        this.ddlTipoDocumentoCliente.Enabled = true;
                        this.ddlCondicionFiscalCliente.Enabled = true;
                        this.txtRazonSocialCliente.Enabled = true;
                    }
                    //this.ddlTipoFactura.Items.Clear();
                    //this.ddlTipoFactura.SelectedValue = null;
                    //this.ddlTipoFactura.DataSource = this.MisTiposFacturas;
                    //this.ddlTipoFactura.DataValueField = "IdTipoFactura";
                    //this.ddlTipoFactura.DataTextField = "Descripcion";
                    //this.ddlTipoFactura.DataBind();
                    //if (this.ddlTipoFactura.Items.Count != 1)
                    //    AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFactura, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                    this.ddlTipoFactura.Enabled = false;
                    this.ddlPrefijoNumeroFactura.Enabled = false;

                    this.MapearObjetoControles(this.MiFactura);
                    //this.ddlMonedas_OnSelectedIndexChanged(null, EventArgs.Empty);
                    if (this.MiFactura.Estado.IdEstado == (int)EstadosFacturas.Activo ||
                        this.MiFactura.Estado.IdEstado == (int)EstadosFacturas.CobradaParcial)
                    {
                        this.txtObservacionComprobante.Enabled = true;
                        this.txtObservacion.Enabled = true;
                    }
                    if (this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturaCreditoElectronicaMyPymesA
                        || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturaCreditoElectronicaMyPymesB
                        || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturaCreditoElectronicaMyPymesC
                        && this.MiFactura.Estado.IdEstado == (int)EstadosFacturas.FESinValidadaAfip)
                    {
                        this.txtFechaVencimiento.Enabled = true;
                        this.txtFechaFactura.Enabled = true;
                        this.ctrCamposValores.IniciarControl(this.MiFactura, new Objeto(), this.GestionControl);
                    }
                    //this.ddlNumeroSocio.Enabled = false;
                    //this.txtNumeroSocio.Enabled = false;
                    //this.btnBuscarSocio.Enabled = false;
                    //this.txtFechaFactura.Enabled = false;
                    //this.imgFechaFactura.Visible = false;
                    this.ddlCantidadCuotas.Enabled = false;
                    this.ddlFilialPuntoVenta.Enabled = false;
                    //this.ddlTipoFactura.Enabled = false;
                    this.ddlConceptoComprobante.Enabled = false;
                    this.ddlMonedas.Enabled = false;
                    //this.txtObservacion.Enabled = false;
                    //this.btnAgregarItem.Visible = false;
                    this.phAgregarItem.Visible = false;
                    this.btnImportarRemito.Visible = false;
                    //this.btnAceptar.Visible = false;
                    this.chkGenerarRemito.Enabled = false;
                    this.btnImportarPresupuesto.Visible = false;
                    this.btnImportarNotaPedido.Visible = false;
                    this.chkGenerarRemito.InputAttributes.Add("disabled", "disabled");
                    this.ddlEstadosRemtios.Enabled = false;
                    //this.cdFechaFactura.Enabled = false;
                    //this.ddlPrefijoNumeroFactura.Enabled = false;
                    this.btnImprimir.Visible = true;
                    this.btnEnviarMail.Visible = true;
                    this.txtFechaFactura.Enabled = false;
                    //txtFechaVencimiento.Enabled = false;
                    this.lblddlTipoComprobanteAsociado.Visible = false;
                    this.ddlTipoComprobanteAsociado.Visible = false;
                    this.btnAgregarComprobanteAsociado.Visible = false;
                    this.lblCantidadDecimales.Visible = false;
                    this.ddlCantidadDecimales.Visible = false;
                    //this.txtPeriodoFechaDesde.Enabled = false;
                    //this.imgPeriodoFechaDesde.Visible = false;
                    //this.txtPeriodoFechaHasta.Enabled = false;
                    //this.imgPeriodoFechaHasta.Visible = false;
                    this.ddlListaPrecio.Enabled = false;
                    this.chkFacturaContado.Enabled = false;
                    break;
                case Gestion.Consultar:
                    this.MiCantidadDecimales = 4;
                    this.MiFactura = FacturasF.FacturasObtenerDatosCompletos(pFactura);
                    this.MapearObjetoControles(this.MiFactura);
                    this.btnTxtCuitBlur.Visible = false;
                    this.btnImportarCosas.Visible = false;
                    this.chkFacturaContado.Enabled = false;
                    this.txtNumeroRecibo.Enabled = false;
                    this.ddlPrefijoNumeroFactura.Enabled = false;
                    if (this.MisParametrosUrl.Contains("IdTipoCargoAfiliadoFormaCobro"))
                    {
                        this.MiFactura.IdRefTabla = Convert.ToInt64(this.MisParametrosUrl["IdTipoCargoAfiliadoFormaCobro"].ToString());
                        this.MiFactura.Tabla = typeof(CarTiposCargosAfiliadosFormasCobros).Name;
                    }
                    //this.ddlNumeroSocio.Enabled = false;
                    //this.txtNumeroSocio.Enabled = false;
                    //this.btnBuscarSocio.Enabled = false;
                    this.txtFechaFactura.Enabled = false;
                    this.ddlCantidadCuotas.Enabled = false;
                    this.ddlFilialPuntoVenta.Enabled = false;
                    this.ddlTipoFactura.Enabled = false;
                    this.ddlConceptoComprobante.Enabled = false;
                    this.ddlMonedas.Enabled = false;
                    this.txtObservacion.Enabled = false;
                    //this.btnAgregarItem.Visible = false;
                    this.phAgregarItem.Visible = false;
                    this.btnImportarRemito.Visible = false;
                    this.btnImportarPresupuesto.Visible = false;
                    this.btnImportarNotaPedido.Visible = false;
                    this.btnAceptar.Visible = false;
                    //this.chkGenerarRemito.Enabled = false;
                    this.chkGenerarRemito.InputAttributes.Add("disabled", "disabled");
                    this.ddlEstadosRemtios.Enabled = false;
                    this.ddlPrefijoNumeroFactura.Enabled = false;
                    this.ddlTipoRemito.Enabled = false;
                    this.btnImprimir.Visible = true;
                    this.btnEnviarMail.Visible = true;
                    this.txtPeriodoFechaDesde.Enabled = false;
                    this.txtPeriodoFechaHasta.Enabled = false;
                    this.txtFechaVencimiento.Enabled = false;
                    this.lblCantidadDecimales.Visible = false;
                    this.ddlCantidadDecimales.Visible = false;
                    this.chkAcopioFinanciero.Enabled = false;
                    this.lblddlTipoComprobanteAsociado.Visible = false;
                    this.ddlTipoComprobanteAsociado.Visible = false;
                    this.btnAgregarComprobanteAsociado.Visible = false;
                    this.ddlListaPrecio.Enabled = false;
                    break;
                default:
                    break;
            }
            this.MiFactura.DirPath = this.Request.PhysicalApplicationPath;
            this.MiFactura.AppPath = this.ObtenerAppPath();
        }
        private void CargarCombos()
        {
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFactura, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.MisConceptosComprobantes = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.AFIPConceptosComprobantes);
            this.ddlConceptoComprobante.DataSource = this.MisConceptosComprobantes;
            this.ddlConceptoComprobante.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlConceptoComprobante.DataTextField = "Descripcion";
            this.ddlConceptoComprobante.DataBind();
            //this.ddlConceptoComprobante.SelectedValue = "291";
            //if (this.ddlConceptoComprobante.Items.Count != 1)
            AyudaProgramacion.AgregarItemSeleccione(this.ddlConceptoComprobante, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlCantidadCuotas.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.FacturasCantidadCuotas);
            this.ddlCantidadCuotas.DataValueField = "CodigoValor";
            this.ddlCantidadCuotas.DataTextField = "Descripcion";
            this.ddlCantidadCuotas.DataBind();
            //if (this.ddlCantidadCuotas.Items.Count != 1)
            //    AyudaProgramacion.AgregarItemSeleccione(this.ddlCantidadCuotas, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.MisIvas = TGEGeneralesF.TGEIVAAlicuotaObtenerListaActiva(this.MiFactura.TipoFactura, this.MiFactura.ComprobanteExento);

            VTAFilialesPuntosVentas filtro = new VTAFilialesPuntosVentas();
            filtro.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
            this.MisTiposPuntosVentas = FacturasF.VTAFilialesPuntosVentasObtenerPorFilial(filtro);
            if (this.MisTiposPuntosVentas.Exists(x => x.IdTipoPuntoVenta == (int)EnumAFIPTiposPuntosVentas.ComprobanteEnLinea))
            {
                this.MisTiposPuntosVentas.Remove(this.MisTiposPuntosVentas.Find(x => x.IdTipoPuntoVenta == (int)EnumAFIPTiposPuntosVentas.ComprobanteEnLinea));
                this.MisTiposPuntosVentas = AyudaProgramacion.AcomodarIndices<VTATiposPuntosVentas>(this.MisTiposPuntosVentas);
            }
            this.ddlFilialPuntoVenta.DataSource = this.MisTiposPuntosVentas;
            this.ddlFilialPuntoVenta.DataValueField = "IdTipoPuntoVenta";
            this.ddlFilialPuntoVenta.DataTextField = "Descripcion";
            this.ddlFilialPuntoVenta.DataBind();
            if (this.ddlFilialPuntoVenta.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlFilialPuntoVenta, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            //else
            //    this.ddlFilialPuntoVenta_SelectedIndexChanged(null, EventArgs.Empty);

            this.ddlCantidadDecimales.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.CantidadDecimales);
            this.ddlCantidadDecimales.DataValueField = "CodigoValor";
            this.ddlCantidadDecimales.DataTextField = "Descripcion";
            this.ddlCantidadDecimales.DataBind();

            List<AfiTiposDocumentos> tiposDocs = AfiliadosF.TipoDocumentosObtenerLista();
            tiposDocs.RemoveAll(x => x.IdTipoDocumento != (int)EnumTiposDocumentos.CUIT && x.IdTipoDocumento != (int)EnumTiposDocumentos.CUIL && x.IdTipoDocumento != (int)EnumTiposDocumentos.DNI);
            this.ddlTipoDocumentoCliente.DataSource = tiposDocs;
            this.ddlTipoDocumentoCliente.DataValueField = "IdTipoDocumento";
            this.ddlTipoDocumentoCliente.DataTextField = "TipoDocumento";
            this.ddlTipoDocumentoCliente.DataBind();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoDocumentoCliente, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            if (tiposDocs.Exists(x => x.IdTipoDocumento == (int)EnumTiposDocumentos.CUIT))
                this.ddlTipoDocumentoCliente.SelectedValue = ((int)EnumTiposDocumentos.CUIT).ToString();

            this.ddlCondicionFiscalCliente.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.CondicionesFiscales);
            this.ddlCondicionFiscalCliente.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlCondicionFiscalCliente.DataTextField = "Descripcion";
            this.ddlCondicionFiscalCliente.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCondicionFiscalCliente, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            CapProveedoresPorcentajesComisiones provComi = new CapProveedoresPorcentajesComisiones();
            provComi.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.FacturaVenta;
            this.ddlProveedores.DataSource = ProveedoresF.CapProveedoresPorcentajesComisionesObtenerProveedores(provComi);
            this.ddlProveedores.DataValueField = "IdProveedor";
            this.ddlProveedores.DataTextField = "RazonSocial";
            this.ddlProveedores.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlProveedores, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            if (this.ddlProveedores.Items.Count > 1)
                this.pnlProveedores.Visible = true;

            TGETiposOperaciones operaciones = new TGETiposOperaciones();
            operaciones.TipoFuncionalidad.IdTipoFuncionalidad = (int)EnumTGETiposFuncionalidades.RemitosVentas;
            operaciones.Estado.IdEstado = (int)Estados.Activo;
            this.ddlTipoOperacionRemito.DataSource = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(operaciones);
            this.ddlTipoOperacionRemito.DataValueField = "IdTipoOperacion";
            this.ddlTipoOperacionRemito.DataTextField = "TipoOperacion";
            this.ddlTipoOperacionRemito.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoOperacionRemito, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            TGEIVA pParamaetro = new TGEIVA();
            pParamaetro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

            this.MisMonedas = TGEGeneralesF.MonedasCotizacionesObtenerCombo();
            this.ddlMonedas.DataSource = this.MisMonedas;
            this.ddlMonedas.DataValueField = "IdMoneda";
            this.ddlMonedas.DataTextField = "MonedaDescripcionCotizacion";
            this.ddlMonedas.DataBind();
            if (this.ddlMonedas.Items.Count != 1)
                AyudaProgramacion.InsertarItemSeleccione(this.ddlMonedas, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            else
                this.ddlMonedas_OnSelectedIndexChanged(null, EventArgs.Empty);

            AyudaProgramacion.AgregarItemSeleccione(this.ddlPrefijoNumeroFactura, "0000");
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoRemito, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            AyudaProgramacion.AgregarItemSeleccione(this.ddlPrefijoNumeroRemito, "0000");

            TESBancosCuentas filtroCtas = new TESBancosCuentas();
            filtroCtas.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.ddlBancoCuenta.DataSource = BancosF.BancosCuentasObtenerLista(filtroCtas);
            this.ddlBancoCuenta.DataValueField = "IdBancoCuenta";
            this.ddlBancoCuenta.DataTextField = "Denominacion";
            this.ddlBancoCuenta.DataBind();
            this.ddlBancoCuenta.Items.Add(new ListItem(this.ObtenerMensajeSistema("SeleccioneOpcion"), "-1"));
            this.ddlBancoCuenta.SelectedValue = "-1";

            CMPListasPrecios lista = new CMPListasPrecios();
            lista.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            lista.IdFilialEvento = string.IsNullOrEmpty(this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString()) ? 0 : this.UsuarioActivo.FilialPredeterminada.IdFilial;
            this.ddlListaPrecio.DataSource = ComprasF.ObtenerListasPrecios(lista);
            this.ddlListaPrecio.DataValueField = "IdListaPrecio";
            this.ddlListaPrecio.DataTextField = "Descripcion";
            this.ddlListaPrecio.DataBind();

            AyudaProgramacion.AgregarItemSeleccione(this.ddlListaPrecio, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEstadosRemtios, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilialEntrega, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            AyudaProgramacion.AgregarItemSeleccione(this.ddlDomicilioEntrega, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void CargarComboDomicilio()
        {
            this.ddlDomicilioEntrega.Items.Clear();
            this.ddlDomicilioEntrega.SelectedIndex = -1;
            this.ddlDomicilioEntrega.SelectedValue = null;
            this.ddlDomicilioEntrega.ClearSelection();
            this.ddlDomicilioEntrega.DataSource = this.MiRemito.Afiliado.Domicilios; //AfiliadosF.AfiliadosObtenerDomicilios(pAfiliado);
            this.ddlDomicilioEntrega.DataValueField = "IdDomicilio";
            this.ddlDomicilioEntrega.DataTextField = "DomicilioCompleto";
            this.ddlDomicilioEntrega.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlDomicilioEntrega, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void ddlListaPrecio_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.MiFactura.FacturasDetalles = new List<VTAFacturasDetalles>();
            this.IniciarGrilla();
            this.upItems.Update();
        }
        protected void ddlFilialPuntoVenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlFilialPuntoVenta.SelectedValue))
            {
                //this.MiFactura.FilialPuntoVenta = this.MisFilialesPuntosVentas[this.ddlFilialPuntoVenta.SelectedIndex];
                //this.MiFactura.PrefijoNumeroFactura = this.MiFactura.FilialPuntoVenta.AfipPuntoVenta.ToString().PadLeft(4, '0');
                //this.txtPrefijoNumeroFactura.Text = this.MiFactura.PrefijoNumeroFactura;
                this.MiFactura.FilialPuntoVenta.TipoPuntoVenta = this.MisTiposPuntosVentas[this.ddlFilialPuntoVenta.SelectedIndex];

                //Cargo los comprobantes habilitados para el Cliente
                this.MiFactura.Afiliado = ctrBuscarCliente.MiAfiliado;
                this.MisTiposFacturas = FacturasF.TiposFacturasSeleccionarPorCondicionFiscal(this.MiFactura);
                this.ddlTipoFactura.Items.Clear();
                this.ddlTipoFactura.SelectedValue = null;
                this.ddlTipoFactura.DataSource = this.MisTiposFacturas;
                this.ddlTipoFactura.DataValueField = "IdTipoFactura";
                this.ddlTipoFactura.DataTextField = "Descripcion";
                this.ddlTipoFactura.DataBind();
                if (this.ddlTipoFactura.Items.Count != 1)
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFactura, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                this.ddlTipoFactura_SelectedIndexChanged(null, EventArgs.Empty);
            }
            else
            {
                this.ddlTipoFactura.Items.Clear();
                this.ddlTipoFactura.SelectedValue = null;
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFactura, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                this.MiFactura.FilialPuntoVenta = new VTAFilialesPuntosVentas();
                this.MiFactura.PrefijoNumeroFactura = string.Empty;
                this.ddlPrefijoNumeroFactura.Items.Clear();
                this.ddlPrefijoNumeroFactura.SelectedValue = null;
                AyudaProgramacion.AgregarItemSeleccione(this.ddlPrefijoNumeroFactura, "0000");
                this.txtNumeroFactura.Text = string.Empty;
            }
            this.UpdatePanel1.Update();
        }
        protected void ddlTipoFactura_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlTipoFactura.SelectedValue))
            {
                /*Parche hoteles*/
                if (this.MiFactura.Tabla == typeof(HTLReservas).Name)
                {
                    this.hdfNoCalculaImporteDescuento.Value = "1"; //this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].Signo == 1 ? "1" : "0";
                }

                this.MiFactura.TipoFactura.IdTipoFactura = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].IdTipoFactura;
                this.MiFactura.TipoFactura.CodigoValor = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].CodigoValor;
                this.MiFactura.TipoFactura.Descripcion = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].Descripcion;
                this.MiFactura.TipoFactura.Signo = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].Signo;

                this.ddlPrefijoNumeroFactura.Items.Clear();
                this.ddlPrefijoNumeroFactura.SelectedValue = null;
                this.MiFactura.FilialPuntoVenta.IdFilial = this.MiFactura.Filial.IdFilial;
                this.MiFactura.FilialPuntoVenta.IdTipoFactura = this.MiFactura.TipoFactura.IdTipoFactura;
                this.MiFactura.FilialPuntoVenta.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                this.MisFilialesPuntosVentas = FacturasF.VTAFilialesPuntosVentasObtenerListaFiltro(this.MiFactura.FilialPuntoVenta);

                if (this.MisFilialesPuntosVentas.Count == 0)
                {
                    this.ddlPrefijoNumeroFactura.Items.Clear();
                    this.ddlPrefijoNumeroFactura.SelectedValue = null;
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlPrefijoNumeroFactura, "0000");
                    this.txtNumeroFactura.Text = string.Empty;
                    this.UpdatePanel1.Update();
                    this.upItems.Update();
                    List<string> lista = new List<string>();
                    lista.Add(this.UsuarioActivo.FilialPredeterminada.Filial);
                    lista.Add(this.UsuarioActivo.ApellidoNombre);
                    this.MostrarMensaje("ValidarFilialPuntosVentas", true, lista);
                    return;
                }

                this.ddlPrefijoNumeroFactura.DataSource = this.MisFilialesPuntosVentas;
                this.ddlPrefijoNumeroFactura.DataValueField = "AfipPuntoVenta";
                this.ddlPrefijoNumeroFactura.DataTextField = "AfipPuntoVentaNumero";
                this.ddlPrefijoNumeroFactura.DataBind();

                this.ddlPrefijoNumeroFactura_SelectedIndexChanged(null, EventArgs.Empty);

                /* Parche para revisar AGULA - Boludo!*/
                if (this.GestionControl == Gestion.Modificar)
                    return;

                this.txtFechaVencimiento.Enabled = true;
                //if (this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoA
                //    || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoB
                //    || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoC
                //    || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesA
                //    || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesB
                //    || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesC
                //    )
                if (this.MiFactura.TipoFactura.Signo == -1
                    || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoElectronicaMyPymesA
                    || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoElectronicaMyPymesB
                    || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoElectronicaMyPymesC
                    )
                {
                    //Cargo los comprobantes habilitados para el Cliente
                    //this.ddlTipoFacturaAsociado.DataSource = this.MisTiposFacturas;
                    //this.ddlTipoFacturaAsociado.DataValueField = "IdTipoFactura";
                    //this.ddlTipoFacturaAsociado.DataTextField = "Descripcion";
                    //this.ddlTipoFacturaAsociado.DataBind();
                    //if (this.ddlTipoFacturaAsociado.Items.Count != 1)
                    //    AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFacturaAsociado, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                    //Cargo los comprobantes habilitados para el Cliente
                    this.MiFactura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
                    this.ddlTipoComprobanteAsociado.DataSource = FacturasF.FacturasObtenerComboAsociados(MiFactura);// FacturasF.FacturasObtenerComboAsociados(MiFactura);
                    this.ddlTipoComprobanteAsociado.DataValueField = "IdFactura";
                    this.ddlTipoComprobanteAsociado.DataTextField = "DescripcionCombo";
                    this.ddlTipoComprobanteAsociado.DataBind();
                    if (this.ddlTipoComprobanteAsociado.Items.Count != 1)
                        AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoComprobanteAsociado, ObtenerMensajeSistema("SeleccioneOpcion"));

                    this.chkGenerarRemito.Checked = false;
                    //this.chkGenerarRemito.Enabled = false;
                    this.ddlTipoOperacionRemito.SelectedValue = ((int)EnumTGETiposOperaciones.DevolucionRemitosVentas).ToString();
                    this.btnImportarRemito.Visible = false;
                    this.btnImportarPresupuesto.Visible = false;
                    this.btnImportarNotaPedido.Visible = false;
                    this.txtFechaVencimiento.Text = string.Empty;
                    this.txtFechaVencimiento.Enabled = true;
                    this.pnlComprobantesAsociados.Visible = true;
                }
                //else if (this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.CbteInternoCredito)
                //{
                //    this.LimpiarComprobantesAsociados();
                //    this.chkGenerarRemito.Checked = false;
                //    //this.chkGenerarRemito.Enabled = false;
                //    //this.phRemitos.Visible = false;
                //    this.btnImportarRemito.Visible = false;
                //    btnImportarPresupuesto.Visible = false;
                //}
                else
                {
                    this.LimpiarComprobantesAsociados();
                    if (this.MiModuloFactura == ModuloFactura.Ventas)
                    {
                        this.ddlTipoOperacionRemito.SelectedValue = ((int)EnumTGETiposOperaciones.RemitosVentas).ToString();
                        this.chkGenerarRemito.Enabled = true;
                        //this.phRemitos.Visible = true;
                        this.btnImportarRemito.Visible = true;
                        this.btnImportarPresupuesto.Visible = true;
                        this.btnImportarNotaPedido.Visible = true;
                    }
                }
                this.upRemitos.Update();
                this.upComprobantesAsociados.Update();

                this.MiFactura.TipoFactura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                this.MisIvas = TGEGeneralesF.TGEIVAAlicuotaObtenerListaActiva(this.MiFactura.TipoFactura, this.MiFactura.ComprobanteExento);
                AyudaProgramacion.CargarGrillaListas<VTAFacturasDetalles>(this.MiFactura.FacturasDetalles, false, this.gvItems, true);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "CalcularItemScript", "CalcularItem();", true);
                //this.upItems.Update();
                if (this.MiModuloFactura == ModuloFactura.Ventas)
                {
                    this.chkGenerarRemito_CheckedChanged(null, EventArgs.Empty);
                }
                this.ctrCamposValores.IniciarControl(this.MiFactura, this.MiFactura.TipoFactura, this.GestionControl);
            }
            else
            {
                this.MiFactura.TipoFactura = new TGETiposFacturas();
                this.ddlPrefijoNumeroFactura.Items.Clear();
                this.ddlPrefijoNumeroFactura.SelectedValue = null;
                AyudaProgramacion.AgregarItemSeleccione(this.ddlPrefijoNumeroFactura, "0000");
                this.txtNumeroFactura.Text = string.Empty;
            }
            this.UpdatePanel1.Update();
            this.upItems.Update();

            if (this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturasA
                || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturasB
                || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturasC
                    || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoA
                    || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoB
                || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoC
                || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturaCreditoElectronicaMyPymesA
                    || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturaCreditoElectronicaMyPymesB
                    || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturaCreditoElectronicaMyPymesC
                || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesA
                    || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesB
                    || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesC
                )
            {
                if (this.MiModuloFactura == ModuloFactura.Ventas)
                    this.AgregarPercepcion.Visible = true;
            }
            else
            {
                this.AgregarPercepcion.Visible = false;
            }
            this.upPercepciones.Update();
        }
        protected void ddlPrefijoNumeroFactura_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlPrefijoNumeroFactura.SelectedValue)
                && Convert.ToInt32(this.ddlPrefijoNumeroFactura.SelectedValue) > 0)
            {
                this.MiFactura.FilialPuntoVenta.AfipPuntoVenta = Convert.ToInt32(this.ddlPrefijoNumeroFactura.SelectedValue);
                this.MiFactura.PrefijoNumeroFactura = this.MiFactura.FilialPuntoVenta.AfipPuntoVentaNumero;
                if (FacturasF.FacturasObtenerProximoNumeroComprobanteTmp(this.MiFactura))
                {
                    this.MiFactura.PrefijoNumeroFactura = this.MiFactura.FilialPuntoVenta.AfipPuntoVentaNumero;
                    //this.txtPrefijoNumeroFactura.Text = this.MiFactura.PrefijoNumeroFactura;
                    this.txtNumeroFactura.Text = this.MiFactura.NumeroFactura;
                    this.txtNumeroFactura.Enabled = false;
                    if (Convert.ToInt32(this.ddlFilialPuntoVenta.SelectedValue) == (int)EnumAFIPTiposPuntosVentas.ComprobanteManual)
                    {
                        this.txtNumeroFactura.Enabled = true;
                    }
                    //else if (Convert.ToInt32(this.ddlFilialPuntoVenta.SelectedValue) == (int)EnumAFIPTiposPuntosVentas.WebServiceFacturaElectronica)
                    //{
                    //    this.cdFechaFactura.StartDate = ""
                    //}
                }
                else
                {
                    //this.txtPrefijoNumeroFactura.Text = string.Empty;
                    this.txtNumeroFactura.Text = string.Empty;
                    this.txtNumeroFactura.Enabled = false;
                    this.MostrarMensaje(this.MiFactura.CodigoMensaje, true, this.MiFactura.CodigoMensajeArgs);
                }
            }
        }
        protected void ddlMonedas_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.MiFactura.FacturasDetalles = new List<VTAFacturasDetalles>();
            this.IniciarGrilla();
            if (!string.IsNullOrWhiteSpace(this.ddlMonedas.SelectedValue))
            {
                TGEMonedas moneda = MisMonedas.FirstOrDefault(x => x.IdMoneda == Convert.ToInt32(this.ddlMonedas.SelectedValue));
                this.SetInitializeCulture(moneda.Moneda);
            }
            this.upItems.Update();
        }
        protected void ddlCantidadCuotas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlCantidadCuotas.SelectedValue))
            {
                this.MiFactura.FacturasDetalles.ForEach(x => x.PrecioUnitarioSinIva = x.ListaPrecioDetalle.CalcularPrecio(Convert.ToInt32(this.ddlCantidadCuotas.SelectedValue)));
                AyudaProgramacion.CargarGrillaListas<VTAFacturasDetalles>(this.MiFactura.FacturasDetalles, false, this.gvItems, true);
                ScriptManager.RegisterStartupScript(this.upItems, this.upItems.GetType(), "CalcularItemScript", "CalcularItem();", true);
                this.upItems.Update();
            }
        }
        protected void ddlConceptoComprobante_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.rfvPeriodoFechaDesde.Enabled = false;
            this.rfvPeriodoFechaHasta.Enabled = false;
            this.txtPeriodoFechaDesde.Enabled = false;
            this.txtPeriodoFechaHasta.Enabled = false;
            this.phPeriodoFecha.Visible = false;
            if (!string.IsNullOrEmpty(this.ddlConceptoComprobante.SelectedValue))
            {
                int idConcepto = Convert.ToInt32(this.ddlConceptoComprobante.SelectedValue);
                if (idConcepto == (int)EnumConceptosComprobantes.ProductosYServicios
                    || idConcepto == (int)EnumConceptosComprobantes.Servicios)
                {
                    this.rfvPeriodoFechaDesde.Enabled = true;
                    this.rfvPeriodoFechaHasta.Enabled = true;
                    this.txtPeriodoFechaDesde.Enabled = true;
                    this.txtPeriodoFechaHasta.Enabled = true;
                    this.phPeriodoFecha.Visible = true;
                    //ScriptManager.RegisterStartupScript(this.upConceptoComprobante, this.upConceptoComprobante.GetType(), "scriptInitControlFechasPeriodo", "initControlFechasPeriodo();", true);
                }
            }
            this.upConceptoComprobante.Update();
        }
        #region ChkRemito
        protected void chkGenerarRemito_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkGenerarRemito.Checked)
            {
                if (this.MiFactura.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta == 0
                    || this.MiFactura.FilialPuntoVenta.IdFilial == 0)
                {
                    this.chkGenerarRemito.Checked = false;
                    this.MostrarMensaje("ValidarDatosFacturas", true);
                    return;
                }
                btnAgregarDomicilio.Visible = true;
                this.ddlDomicilioEntrega.Enabled = true;
                this.txtRemitoObservacionComprobante.Enabled = true;
                this.txtRemitoObservacionInterna.Enabled = true;
                this.btnImportarRemito.Visible = false;
                this.ddlTipoRemito.Items.Clear();
                this.ddlTipoRemito.SelectedValue = null;
                this.ddlPrefijoNumeroRemito.Items.Clear();
                this.ddlPrefijoNumeroRemito.SelectedValue = null;

                this.MiRemito = new VTARemitos();
                this.MiRemito.FilialPuntoVenta.TipoPuntoVenta = this.MisTiposPuntosVentas[this.ddlFilialPuntoVenta.SelectedIndex];
                //Cargo los comprobantes habilitados para el Cliente
                this.MisTiposFacturasRemitos = FacturasF.TiposFacturasSeleccionarPorRemitos(this.MiRemito);
                this.ddlTipoRemito.Items.Clear();
                this.ddlTipoRemito.SelectedValue = null;
                this.ddlTipoRemito.DataSource = this.MisTiposFacturasRemitos;
                this.ddlTipoRemito.DataValueField = "IdTipoFactura";
                this.ddlTipoRemito.DataTextField = "Descripcion";
                this.ddlTipoRemito.DataBind();
                if (this.ddlTipoRemito.Items.Count != 1)
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoRemito, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                this.ddlTipoRemito_SelectedIndexChanged(null, EventArgs.Empty);

                this.ddlEstadosRemtios.Items.Clear();
                this.ddlEstadosRemtios.SelectedValue = null;
                List<TGEEstados> estados = TGEGeneralesF.TGEEstadosObtenerLista("VTARemitos");
                estados = estados.Where(x => x.IdEstado != (int)EstadosRemitos.Baja).ToList();
                this.ddlEstadosRemtios.DataSource = estados;
                this.ddlEstadosRemtios.DataValueField = "IdEstado";
                this.ddlEstadosRemtios.DataTextField = "Descripcion";
                this.ddlEstadosRemtios.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlEstadosRemtios, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                this.rfvEstadosRemitos.Enabled = true;

                this.ddlFilialEntrega.Items.Clear();
                this.ddlFilialEntrega.SelectedValue = null;
                this.ddlFilialEntrega.DataSource = TGEGeneralesF.FilialesEntregaObtenerListaActiva();
                this.ddlFilialEntrega.DataValueField = "IdFilialEntrega";
                this.ddlFilialEntrega.DataTextField = "Filial";
                this.ddlFilialEntrega.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlFilialEntrega, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                this.rfvFilialEntrega.Enabled = true;

                this.MiRemito.Afiliado = AfiliadosF.AfiliadosObtenerDatosCompletos(this.MiFactura.Afiliado);
                CargarComboDomicilio();

                if (MiRemito.Afiliado.Domicilios.Exists(x => x.Predeterminado))
                {
                    this.ddlDomicilioEntrega.SelectedValue = MiRemito.Afiliado.Domicilios.First(x => x.Predeterminado).IdDomicilio.ToString();
                }
            }
            else
            {
                this.btnImportarRemito.Visible = true;
                this.rfvEstadosRemitos.Enabled = false;
                this.rfvFilialEntrega.Enabled = false;
                this.rfvFechaEntrega.Enabled = false;
                this.txtFechaEntrega.Enabled = false;
                btnAgregarDomicilio.Visible = false;
                this.ddlDomicilioEntrega.Enabled = false;
                this.txtRemitoObservacionComprobante.Enabled = false;
                this.txtRemitoObservacionInterna.Enabled = false;
                this.upItems.Update();
            }

            this.upRemitos.Update();
        }
        protected void ddlTipoRemito_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlTipoRemito.SelectedValue))
            {
                this.MiRemito.TipoFactura.IdTipoFactura = this.MisTiposFacturasRemitos[this.ddlTipoRemito.SelectedIndex].IdTipoFactura;
                this.MiRemito.TipoFactura.CodigoValor = this.MisTiposFacturasRemitos[this.ddlTipoRemito.SelectedIndex].CodigoValor;
                this.MiRemito.TipoFactura.Descripcion = this.MisTiposFacturasRemitos[this.ddlTipoRemito.SelectedIndex].Descripcion;

                AyudaProgramacion.MatchObjectProperties(this.MiFactura.FilialPuntoVenta, this.MiRemito.FilialPuntoVenta);
                this.MiRemito.FilialPuntoVenta.IdTipoFactura = this.MiRemito.TipoFactura.IdTipoFactura;
                this.MisFilialesPuntosVentasRemitos = FacturasF.VTAFilialesPuntosVentasObtenerListaFiltro(this.MiRemito.FilialPuntoVenta);
                this.ddlPrefijoNumeroRemito.DataSource = this.MisFilialesPuntosVentasRemitos;
                this.ddlPrefijoNumeroRemito.DataValueField = "AfipPuntoVenta";
                this.ddlPrefijoNumeroRemito.DataTextField = "AfipPuntoVentaNumero";
                this.ddlPrefijoNumeroRemito.DataBind();

                this.ddlPrefijoNumeroRemito_SelectedIndexChanged(null, EventArgs.Empty);
            }
            else
            {
                this.MiRemito.TipoFactura = new TGETiposFacturas();
                this.ddlPrefijoNumeroFactura.Items.Clear();
                this.ddlPrefijoNumeroFactura.SelectedValue = null;
                AyudaProgramacion.AgregarItemSeleccione(this.ddlPrefijoNumeroFactura, "0000");
                this.txtNumeroRemito.Text = string.Empty;
            }
            this.upRemitos.Update();
        }
        protected void ddlPrefijoNumeroRemito_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlPrefijoNumeroRemito.SelectedValue)
                && Convert.ToInt32(this.ddlPrefijoNumeroRemito.SelectedValue) > 0)
            {
                this.MiRemito.FilialPuntoVenta.AfipPuntoVenta = Convert.ToInt32(this.ddlPrefijoNumeroRemito.SelectedValue);
                this.MiRemito.NumeroRemitoPrefijo = this.MiRemito.FilialPuntoVenta.AfipPuntoVentaNumero;
                this.MiRemito.Filial.IdFilial = this.MiRemito.FilialPuntoVenta.IdFilial;
                this.MiRemito.TipoFactura.IdTipoFactura = this.MiRemito.FilialPuntoVenta.IdTipoFactura;
                if (FacturasF.RemitosObtenerProximoNumeroComprobanteTmp(this.MiRemito))
                {
                    this.MiRemito.NumeroRemitoPrefijo = this.MiRemito.FilialPuntoVenta.AfipPuntoVentaNumero;
                    //this.txtPrefijoNumeroFactura.Text = this.MiFactura.PrefijoNumeroFactura;
                    this.txtNumeroRemito.Text = this.MiRemito.NumeroRemitoSuFijo;
                    this.txtNumeroRemito.Enabled = this.MiRemito.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta == (int)EnumAFIPTiposPuntosVentas.ComprobanteManual
                                                    || (this.MiRemito.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta == (int)EnumAFIPTiposPuntosVentas.WebServiceFacturaElectronica
                                                        && this.MiRemito.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.RemitoR);
                }
                else
                {
                    //this.txtPrefijoNumeroFactura.Text = string.Empty;
                    this.txtNumeroRemito.Text = string.Empty;
                    this.txtNumeroRemito.Enabled = false;
                    this.MostrarMensaje(this.MiRemito.CodigoMensaje, true, this.MiRemito.CodigoMensajeArgs);
                }
            }
            this.upRemitos.Update();
        }
        protected void ddlEstadosRemtios_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool deshabilitarControles = true;
            if (!string.IsNullOrEmpty(this.ddlEstadosRemtios.SelectedValue))
            {
                if (Convert.ToInt32(this.ddlEstadosRemtios.SelectedValue) == (int)EstadosRemitos.Entregado)
                {
                    this.txtFechaEntrega.Enabled = true;
                    this.rfvFechaEntrega.Enabled = true;
                    deshabilitarControles = false;
                }
            }

            if (deshabilitarControles)
            {
                this.txtFechaEntrega.Text = string.Empty;
                this.txtFechaEntrega.Enabled = false;
                this.MiRemito.FechaEntrega = default(DateTime?);
                this.rfvFechaEntrega.Enabled = false;
            }
        }
        protected void btnAgregarDomicilio_Click(object sender, EventArgs e)
        {
            this.ctrDomicilios.IniciarControl(new AfiDomicilios(), Gestion.Agregar);
        }
        private void CtrDomicilios_AfiliadosModificarDatosAceptar(object sender, AfiDomicilios e, Gestion pGestion)
        {
            e.IdAfiliado = this.MiRemito.Afiliado.IdAfiliado;
            e.EstadoColeccion = EstadoColecciones.Agregado;
            if (AfiliadosF.AfiliadosAgregarDomicilio(e))
            {
                this.MiRemito.Afiliado.Domicilios.Add(e);
                CargarComboDomicilio();
                ddlDomicilioEntrega.SelectedValue = e.IdDomicilio.ToString();
            }
            else
            {
                MostrarMensaje(e.CodigoMensaje, true, e.CodigoMensajeArgs);
            }
            upRemitos.Update();
        }
        #endregion
        protected void chkFacturaContado_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkFacturaContado.Checked)
            {
                this.ddlBancoCuenta.Enabled = true;
            }
            else
            {
                this.ddlBancoCuenta.Enabled = false;
                if (this.ddlBancoCuenta.Items.FindByValue("-1") != null)
                    this.ddlBancoCuenta.SelectedValue = "-1"; //SELECCIONE OPCION
            }
            this.UpdatePanel1.Update();
        }
        private void LimpiarComprobantesAsociados()
        {
            this.pnlComprobantesAsociados.Visible = false;
            this.MiFactura.FacturasAsociadas = new List<VTAFacturas>();
            AyudaProgramacion.CargarGrillaListas<VTAFacturas>(this.MiFactura.FacturasAsociadas, false, this.gvDatos, false);
        }
        #region Grilla Comprobantes
        private void IniciarGrilla()
        {
            VTAFacturasDetalles item;
            for (int i = 0; i < 2; i++)
            {
                item = new VTAFacturasDetalles();
                item.EstadoColeccion = EstadoColecciones.AgregadoPrevio;
                this.MiFactura.FacturasDetalles.Add(item);
                item.IndiceColeccion = this.MiFactura.FacturasDetalles.IndexOf(item);
                //item.IdFacturaDetalle = item.IndiceColeccion * -1;
            }
            AyudaProgramacion.CargarGrillaListas<VTAFacturasDetalles>(this.MiFactura.FacturasDetalles, false, this.gvItems, true);
        }
        private void PersistirDatosGrilla()
        {
            if (this.MiFactura.FacturasDetalles.Count == 0)
                return;

            foreach (GridViewRow fila in this.gvItems.Rows)
            {
                //string codigo = ((TextBox)fila.FindControl("txtCodigo")).Text;
                DropDownList ddlProducto = ((DropDownList)fila.FindControl("ddlProducto"));
                HiddenField hdfIdProducto = (HiddenField)fila.FindControl("hdfIdProducto");
                HiddenField hdfProductoDetalle = (HiddenField)fila.FindControl("hdfProductoDetalle");
                Label lblProductoDescripcion = (Label)fila.FindControl("lblProductoDescripcion");
                string descripcion = ((TextBox)fila.FindControl("txtDescripcion")).Text;
                decimal cantidad = ((Evol.Controls.CurrencyTextBox)fila.FindControl("txtCantidad")).Decimal;
                //decimal precioUnitario = ((Evol.Controls.CurrencyTextBox)fila.FindControl("txtPrecioUnitario")).Decimal;
                decimal importe = ((Evol.Controls.CurrencyTextBox)fila.FindControl("txtPrecioUnitario")).Decimal;                //CurrencyTextBox importe = (CurrencyTextBox)fila.FindControl("txtPrecioUnitario");
                decimal precioUnitario = ((HiddenField)fila.FindControl("hdfPreUnitario")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfPreUnitario")).Value.ToString().Replace('.', ','));
                decimal subTotal = ((HiddenField)fila.FindControl("hdfSubtotal")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfSubtotal")).Value.ToString().Replace('.', ','));
                DropDownList ddlAlicuotaIVA = ((DropDownList)fila.FindControl("ddlAlicuotaIVA"));
                decimal importeIva = ((HiddenField)fila.FindControl("hdfImporteIva")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfImporteIva")).Value.ToString().Replace('.', ','));
                decimal subTotalConIva = ((HiddenField)fila.FindControl("hdfSubtotalConIva")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfSubtotalConIva")).Value.ToString().Replace('.', ','));
                bool modificaPrecio = ((HiddenField)fila.FindControl("hdfModificaPrecio")).Value == string.Empty ? false : Convert.ToBoolean(((HiddenField)fila.FindControl("hdfModificaPrecio")).Value);
                decimal porcDesc = ((CurrencyTextBox)fila.FindControl("txtDescuentoPorcentual")).Decimal;
                decimal importeDesc = ((HiddenField)fila.FindControl("hdfDescuentoImporte")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfDescuentoImporte")).Value.Replace('.', ','));//.ToString().Replace('.',' '));
                this.MiFactura.FacturasDetalles[fila.RowIndex].ModificaPrecio = modificaPrecio;
                this.MiFactura.FacturasDetalles[fila.RowIndex].DescuentoPorcentual = porcDesc;//decimal.Parse(porcDesc.Replace(".", ","), NumberStyles.AllowDecimalPoint);
                bool hdfStockeable = ((HiddenField)fila.FindControl("hdfStockeable")).Value == string.Empty ? false : Convert.ToBoolean(((HiddenField)fila.FindControl("hdfStockeable")).Value);
                DropDownList ddlCentrosCostos = ((DropDownList)fila.FindControl("ddlCentrosCostos"));

                if (hdfIdProducto.Value != string.Empty && Convert.ToInt32(hdfIdProducto.Value) > 0)
                {
                    this.MiFactura.FacturasDetalles[fila.RowIndex].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiFactura.FacturasDetalles[fila.RowIndex], GestionControl);
                    this.MiFactura.FacturasDetalles[fila.RowIndex].Estado.IdEstado = (int)EstadosFacturasDetalles.Activo;
                    this.MiFactura.FacturasDetalles[fila.RowIndex].ListaPrecioDetalle.Producto.IdProducto = Convert.ToInt32(hdfIdProducto.Value);
                    this.MiFactura.FacturasDetalles[fila.RowIndex].ListaPrecioDetalle.Producto.Descripcion = hdfProductoDetalle.Value;
                    this.MiFactura.FacturasDetalles[fila.RowIndex].ListaPrecioDetalle.Producto.Familia.Stockeable = hdfStockeable;
                }
                this.MiFactura.FacturasDetalles[fila.RowIndex].Descripcion = descripcion;
                this.MiFactura.FacturasDetalles[fila.RowIndex].DescripcionProducto = hdfProductoDetalle.Value;
                if (cantidad > 0)
                {
                    this.MiFactura.FacturasDetalles[fila.RowIndex].Cantidad = cantidad;
                }
                this.MiFactura.FacturasDetalles[fila.RowIndex].PrecioUnitarioSinIva = Convert.ToDecimal(importe);

                if (cantidad > 0) // && precioUnitario != string.Empty)
                {

                    this.MiFactura.FacturasDetalles[fila.RowIndex].DescuentoImporte = importeDesc;
                    this.MiFactura.FacturasDetalles[fila.RowIndex].SubTotal = subTotal;

                    this.MiFactura.FacturasDetalles[fila.RowIndex].ImporteIVA = importeIva;

                    this.MiFactura.FacturasDetalles[fila.RowIndex].SubTotalConIva = subTotalConIva;
                    if (ddlAlicuotaIVA.SelectedValue != string.Empty)
                    {
                        this.MiFactura.FacturasDetalles[fila.RowIndex].IVA = this.MisIvas[ddlAlicuotaIVA.SelectedIndex];
                    }
                }
                if (ddlCentrosCostos.SelectedValue != "" && Convert.ToInt32(ddlCentrosCostos.SelectedValue) > 0)
                {
                    this.MiFactura.FacturasDetalles[fila.DataItemIndex].CentroCostoProrrateo = this.MisCentrosCostos.Find(x => x.IdCentroCostoProrrateo == Convert.ToInt32(ddlCentrosCostos.SelectedValue));
                }
                if (modificaPrecio)
                {
                    MiFactura.FacturasDetalles[fila.RowIndex].ListaPrecioDetalle.PrecioEditable = true;
                }

            }
            this.CalcularTotal();
        }
        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            this.MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            if (e.CommandName == "Borrar")
            {
                this.MiFactura.FacturasDetalles.RemoveAt(this.MiIndiceDetalleModificar);
                this.MiFactura.FacturasDetalles = AyudaProgramacion.AcomodarIndices<VTAFacturasDetalles>(this.MiFactura.FacturasDetalles);
                AyudaProgramacion.CargarGrillaListas<VTAFacturasDetalles>(this.MiFactura.FacturasDetalles, false, this.gvItems, true);
                this.CalcularTotal();
            }
            //else if (e.CommandName == "BuscarProducto")
            //{
            //    CMPListasPrecios filtro = new CMPListasPrecios();
            //    filtro.IdAfiliado = this.MiFactura.Afiliado.IdAfiliado;
            //    //this.ctrBuscarProductoPopUp.IniciarControl(filtro);
            //}
            //else if (e.CommandName == "AgregarDetalle")
            //{
            //    TextBox desc = (TextBox)this.gvItems.Rows[this.MiIndiceDetalleModificar].FindControl("txtDescripcion");
            //    desc.Visible = !desc.Visible;
            //}
        }
        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                VTAFacturasDetalles item = (VTAFacturasDetalles)e.Row.DataItem;

                DropDownList ddlAlicuotaIVA = ((DropDownList)e.Row.FindControl("ddlAlicuotaIVA"));
                DropDownList ddlCentrosCostos = ((DropDownList)e.Row.FindControl("ddlCentrosCostos"));
                ddlAlicuotaIVA.DataSource = this.MisIvas;
                ddlAlicuotaIVA.DataValueField = "IdIVAAlicuota";
                ddlAlicuotaIVA.DataTextField = "Descripcion";
                ddlAlicuotaIVA.DataBind();

                DropDownList ddlProducto = ((DropDownList)e.Row.FindControl("ddlProducto"));
                CurrencyTextBox txtDescuentoPorcentual = ((CurrencyTextBox)e.Row.FindControl("txtDescuentoPorcentual"));
                CurrencyTextBox txtCantidad = ((CurrencyTextBox)e.Row.FindControl("txtCantidad"));
                if (item.ListaPrecioDetalle.Producto.IdProducto > 0)
                    ddlProducto.Items.Add(new ListItem(item.ListaPrecioDetalle.Producto.Descripcion, item.ListaPrecioDetalle.Producto.IdProducto.ToString()));

                ddlCentrosCostos.DataSource = this.MisCentrosCostos;
                ddlCentrosCostos.DataValueField = "IdCentroCostoProrrateo";
                ddlCentrosCostos.DataTextField = "CentroCostoProrrateo";
                ddlCentrosCostos.DataBind();
                if (ddlCentrosCostos.Items.Count != 1)
                    AyudaProgramacion.AgregarItemSeleccione(ddlCentrosCostos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                if (this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.CbteInterno
                        || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.cbteInternoC
                        || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.CbteInternoCredito
                        || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturasC
                        || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoC
                        || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoC)
                {
                    if (this.MisIvas.Exists(x => x.IdIVA == (int)EnumIVA.NoCorresponde))
                    {
                        ddlAlicuotaIVA.SelectedValue = ((int)EnumIVA.NoCorresponde).ToString();
                    }
                }

                ListItem lstItem;
                if (ddlAlicuotaIVA.Items.Count == 0)
                    AyudaProgramacion.AgregarItemSeleccione(ddlAlicuotaIVA, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                else
                {
                    ddlAlicuotaIVA.SelectedValue = item.IVA.IdIVAAlicuota;
                    ddlAlicuotaIVA.Attributes.Add("onchange", "CalcularItem();");
                    ddlAlicuotaIVA.Enabled = true;
                }
                //if (item.IVA.IdIVA > 0)
                //    ddlAlicuotaIVA.SelectedValue = item.IVA.IdIVA.ToString();

                if (item.CentroCostoProrrateo.IdCentroCostoProrrateo > 0)
                    ddlCentrosCostos.SelectedValue = item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString();
                else if (ddlCentrosCostos.Items.Count != 1)
                    ddlCentrosCostos.SelectedValue = string.Empty;

                if (this.GestionControl == Gestion.Agregar)
                {
                    bool permitenegativo = false;
                    if (ddlTipoFactura.SelectedValue != string.Empty)
                    {
                        permitenegativo = MisTiposFacturas.FirstOrDefault(x => x.IdTipoFactura.ToString() == ddlTipoFactura.SelectedValue).Signo > 0 ? true : false;
                    }

                    //if (item.CentroCostoProrrateo.IdCentroCostoProrrateo > 0)
                    //    ddlCentrosCostos.SelectedValue = item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString();
                    //else if (ddlCentrosCostos.Items.Count != 1)
                    //    ddlCentrosCostos.SelectedValue = string.Empty;
                    if (item.DetalleImportado == true)
                    {
                        //ImageButton ibtnProducto = (ImageButton)e.Row.FindControl("btnBuscarProducto");
                        //ibtnProducto.Visible = false;
                        ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                        btnEliminar.Visible = true;
                        ddlProducto.Enabled = false;
                        //SKP.ASP.Controls.NumericTextBox codigo = (SKP.ASP.Controls.NumericTextBox)e.Row.FindControl("txtCodigo");
                        //codigo.Enabled = false;
                        //Label lblProductoDescripcion = (Label)e.Row.FindControl("lblProductoDescripcion");
                        //lblProductoDescripcion.Visible = true;
                        TextBox txtDescripcion = (TextBox)e.Row.FindControl("txtDescripcion");
                        txtDescripcion.Visible = true;
                        txtDescripcion.Enabled = true;
                        CurrencyTextBox cantidad = (CurrencyTextBox)e.Row.FindControl("txtCantidad");
                        cantidad.Attributes.Add("onchange", "CalcularItem();");
                        cantidad.Enabled = true;

                        CurrencyTextBox descuentoPorcentual = (CurrencyTextBox)e.Row.FindControl("txtDescuentoPorcentual");
                        descuentoPorcentual.Attributes.Add("onchange", "CalcularItem();");
                        descuentoPorcentual.Enabled = true;

                        Evol.Controls.CurrencyTextBox importe = (Evol.Controls.CurrencyTextBox)e.Row.FindControl("txtPrecioUnitario");
                        importe.AllowNegative = permitenegativo;
                        HiddenField hdfModifPrecio = (HiddenField)e.Row.FindControl("hdfModificaPrecio");
                        importe.Attributes.Add("onchange", "ModificaPrecio('" + hdfModifPrecio.ClientID + "'); CalcularItem();");
                        importe.NumberOfDecimals = this.MiCantidadDecimales;
                        string numberSymbol = importe.Prefix == string.Empty ? "N" : "C";
                        decimal precioUni = item.PrecioUnitarioSinIva.HasValue ? item.PrecioUnitarioSinIva.Value : 0;
                        importe.Text = precioUni.ToString(string.Concat(numberSymbol, this.MiCantidadDecimales.ToString()));
                        if (item.ListaPrecioDetalle.PrecioEditable)
                        {
                            importe.Enabled = true;
                        }
                        else
                        {
                            importe.Enabled = false;
                        }
                    }
                    else
                    {
                        //ImageButton ibtnProducto = (ImageButton)e.Row.FindControl("btnBuscarProducto");
                        //ibtnProducto.Visible = true;
                        ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                        btnEliminar.Visible = true;
                        ddlProducto.Enabled = true;
                        //SKP.ASP.Controls.NumericTextBox codigo = (SKP.ASP.Controls.NumericTextBox)e.Row.FindControl("txtCodigo");
                        //codigo.Enabled = true;
                        //Label lblProductoDescripcion = (Label)e.Row.FindControl("lblProductoDescripcion");
                        //lblProductoDescripcion.Visible = true;
                        TextBox txtDescripcion = (TextBox)e.Row.FindControl("txtDescripcion");
                        txtDescripcion.Visible = true;
                        txtDescripcion.Enabled = true;
                        CurrencyTextBox cantidad = (CurrencyTextBox)e.Row.FindControl("txtCantidad");
                        cantidad.Attributes.Add("onchange", "CalcularItem();");
                        cantidad.Enabled = true;
                        //CurrencyTextBox precioUnitario = (CurrencyTextBox)e.Row.FindControl("txtPrecioUnitario");
                        //precioUnitario.Attributes.Add("onchange", "CalcularItem();");
                        //precioUnitario.Enabled = true;

                        CurrencyTextBox descuentoPorcentual = (CurrencyTextBox)e.Row.FindControl("txtDescuentoPorcentual");
                        descuentoPorcentual.Attributes.Add("onchange", "CalcularItem();");
                        descuentoPorcentual.Enabled = true;

                        CurrencyTextBox importe = (CurrencyTextBox)e.Row.FindControl("txtPrecioUnitario");
                        importe.AllowNegative = permitenegativo;
                        HiddenField hdfModifPrecio = (HiddenField)e.Row.FindControl("hdfModificaPrecio");
                        importe.Attributes.Add("onchange", "ModificaPrecio('" + hdfModifPrecio.ClientID + "'); CalcularItem();");
                        importe.NumberOfDecimals = this.MiCantidadDecimales;

                        string numberSymbol = importe.Prefix == string.Empty ? "N" : "C";
                        decimal precioUni = item.PrecioUnitarioSinIva.HasValue ? item.PrecioUnitarioSinIva.Value : 0;
                        importe.Text = precioUni.ToString(string.Concat(numberSymbol, this.MiCantidadDecimales.ToString()));
                        if (item.ListaPrecioDetalle.PrecioEditable)
                        {
                            importe.Enabled = true;
                        }
                        else
                        {
                            importe.Enabled = false;
                        }
                        if (MiFactura.Tabla == typeof(HTLReservas).Name)
                        {
                            ddlProducto.Visible = false;
                            Label lblProducto = (Label)e.Row.FindControl("lblProducto");
                            lblProducto.Visible = true;
                        }
                        if (item.EsFacturaCargos)
                        {
                            //Label lblProducto = (Label)e.Row.FindControl("lblProducto");
                            //lblProducto.Visible = true;
                            //lblProducto.Enabled = true;
                            //ddlProducto.Visible = false;
                            importe.Attributes.Add("onchange", "ModificaPrecio('" + hdfModifPrecio.ClientID + "'); CalcularItem(); ValidarFacturaCargo(); ");
                            //this.btnAgregarItem.Visible = false;
                            this.btnImportarRemito.Visible = false;
                            //codigo.Enabled = false;
                            ddlProducto.Enabled = false;
                            txtDescripcion.Enabled = false;
                            importe.Enabled = true;
                            descuentoPorcentual.Enabled = false;
                            ddlAlicuotaIVA.Enabled = false;
                            //lblCantidadDecimales.Visible = false;
                            //ddlCantidadDecimales.Visible = false;
                        }
                    }
                }
                else if (this.GestionControl == Gestion.Consultar)
                {
                    //TextBox txtProductoDescripcion = (TextBox)e.Row.FindControl("txtProductoDescripcion");
                    //txtProductoDescripcion.Visible = false;
                    Label lblProductoDescripcion = (Label)e.Row.FindControl("lblProductoDescripcion");
                    lblProductoDescripcion.Visible = true;
                    TextBox txtDescripcion = (TextBox)e.Row.FindControl("txtDescripcion");
                    txtDescripcion.Visible = false;
                    Label lblProducto = (Label)e.Row.FindControl("lblProducto");
                    lblProducto.Visible = true;
                    lblProducto.Enabled = true;
                    ddlProducto.Visible = false;

                    //TextBox txtProductoDescripcionConsulta = (TextBox)e.Row.FindControl("txtProductoDescripcionConsulta");
                    //txtProductoDescripcionConsulta.Visible = true;
                    //txtProductoDescripcionConsulta.Text = txtDescripcion.Text;

                    lstItem = ddlAlicuotaIVA.Items.FindByValue(item.IVA.IdIVAAlicuota);
                    if (lstItem == null)
                        ddlAlicuotaIVA.Items.Add(new ListItem(item.IVA.Descripcion, item.IVA.IdIVAAlicuota));
                    ddlAlicuotaIVA.SelectedValue = item.IVA.IdIVAAlicuota;
                    ddlAlicuotaIVA.Enabled = false;

                    lstItem = ddlCentrosCostos.Items.FindByValue(item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString());
                    if (lstItem == null)
                    {
                        this.MisCentrosCostos.Add(item.CentroCostoProrrateo);
                        ddlCentrosCostos.Items.Add(new ListItem(item.CentroCostoProrrateo.CentroCostoProrrateo.ToString(), item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString()));
                    }
                    ddlCentrosCostos.SelectedValue = item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString();
                    ddlCentrosCostos.Enabled = false;
                }
                else if (GestionControl == Gestion.Modificar)
                {
                    Label lblProductoDescripcion = (Label)e.Row.FindControl("lblProductoDescripcion");
                    lblProductoDescripcion.Visible = true;
                    TextBox txtDescripcion = (TextBox)e.Row.FindControl("txtDescripcion");
                    txtDescripcion.Visible = false;
                    Label lblProducto = (Label)e.Row.FindControl("lblProducto");
                    lblProducto.Visible = true;
                    lblProducto.Enabled = true;
                    ddlProducto.Visible = false;

                    lstItem = ddlAlicuotaIVA.Items.FindByValue(item.IVA.IdIVAAlicuota);
                    if (lstItem == null)
                        ddlAlicuotaIVA.Items.Add(new ListItem(item.IVA.Descripcion, item.IVA.IdIVAAlicuota));
                    ddlAlicuotaIVA.SelectedValue = item.IVA.IdIVAAlicuota;
                    ddlAlicuotaIVA.Enabled = false;

                    ddlCentrosCostos.Enabled = false;
                }

                if (this.MiModuloFactura == ModuloFactura.ReservasTurismo)
                {
                    ddlProducto.Enabled = false;
                    ddlAlicuotaIVA.Enabled = false;
                    txtCantidad.Enabled = false;
                    ddlCentrosCostos.Enabled = false;
                }
            }
        }
        //void ctrBuscarProductoPopUp_ProductosBuscarSeleccionar(CMPListasPreciosDetalles e)
        //{
        //    this.MiFactura.FacturasDetalles[this.MiIndiceDetalleModificar].ListaPrecioDetalle = e;
        //    this.MiFactura.FacturasDetalles[this.MiIndiceDetalleModificar].DescripcionProducto = e.Producto.Descripcion;
        //    this.MiFactura.FacturasDetalles[this.MiIndiceDetalleModificar].PrecioUnitarioSinIva = this.MiFactura.FacturasDetalles[this.MiIndiceDetalleModificar].ListaPrecioDetalle.CalcularPrecio(Convert.ToInt32(this.ddlCantidadCuotas.SelectedValue));
        //    this.MiFactura.FacturasDetalles[this.MiIndiceDetalleModificar].Cantidad = Convert.ToDecimal(1.00); //Por defecto 1
        //    if (this.MiFactura.Afiliado.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Socios)
        //        this.MiFactura.FacturasDetalles[this.MiIndiceDetalleModificar].DescuentoPorcentual = this.MiDescuentoSocio;
        //    AyudaProgramacion.CargarGrillaListas<VTAFacturasDetalles>(this.MiFactura.FacturasDetalles, false, this.gvItems, true);
        //    TextBox txtCantidad = (TextBox)this.gvItems.Rows[this.MiIndiceDetalleModificar].FindControl("txtCantidad");
        //    txtCantidad.Focus();
        //    ScriptManager.RegisterStartupScript(this.upItems, this.upItems.GetType(), "CalcularItemScript", "CalcularItem();", true);
        //    this.upItems.Update();
        //}
        protected void btnAgregarItem_Click(object sender, EventArgs e)
        {
            this.PersistirDatosGrilla();
            this.AgregarItem();
            this.txtCantidadAgregar.Text = string.Empty;
            //VTAFacturasDetalles item;
            //item = new VTAFacturasDetalles();
            //this.MiFactura.FacturasDetalles.Add(item);
            //item.IndiceColeccion = this.MiFactura.FacturasDetalles.IndexOf(item);
            //AyudaProgramacion.CargarGrillaListas<VTAFacturasDetalles>(this.MiFactura.FacturasDetalles, false, this.gvItems, true);
            //this.gvItems.Rows[item.IndiceColeccion].FindControl("ddlProducto").Focus();
        }
        private void AgregarItem()
        {
            VTAFacturasDetalles item;
            if (this.txtCantidadAgregar.Text == string.Empty || this.txtCantidadAgregar.Text == "0")
            {
                this.txtCantidadAgregar.Text = "1";
            }
            int cantidad = Convert.ToInt32(this.txtCantidadAgregar.Text);
            for (int i = 0; i < cantidad; i++)
            {
                item = new VTAFacturasDetalles();
                this.MiFactura.FacturasDetalles.Add(item);
                item.IndiceColeccion = this.MiFactura.FacturasDetalles.IndexOf(item);
                item.DescuentoPorcentual = this.txtAplicarPorcentajeDescuento.Decimal;
                item.IdFacturaDetalle = item.IndiceColeccion * -1;
                //this.gvItems.Rows[item.IndiceColeccion].FindControl("ddlProducto").Focus();
            }
            AyudaProgramacion.CargarGrillaListas<VTAFacturasDetalles>(this.MiFactura.FacturasDetalles, false, this.gvItems, true);
            //ScriptManager.RegisterStartupScript(this.upDetalleGastos, this.upDetalleGastos.GetType(), "CalcularPrecioScript", "CalcularPrecio();", true);
        }
        protected void button_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdfIdProductoCodigo.ToString()))
            {
                List<VTAFacturasDetalles> list = MiFactura.FacturasDetalles.Where(x => x.ListaPrecioDetalle.Producto.IdProducto > 0).ToList();

                this.MiFactura.FacturasDetalles = list;

                VTAFacturasDetalles producto = new VTAFacturasDetalles();

                producto.Cantidad = this.txtCantidadCodigo.Decimal;
                producto.ListaPrecioDetalle.Producto.IdProducto = Convert.ToInt32(hdfIdProductoCodigo.Value);
                producto.DescripcionProducto = hdfProductoDetalleCodigo.Value;
                producto.ListaPrecioDetalle.Precio = hdfPrecioCodigo.Value == string.Empty ? 0 : Convert.ToDecimal(hdfPrecioCodigo.Value);
                producto.ListaPrecioDetalle.PrecioEditable = hdfModificaPrecioCodigo.Value == string.Empty ? true : Convert.ToBoolean(hdfModificaPrecioCodigo.Value);
                producto.PrecioUnitarioSinIva = hdfPreUnitarioCodigo.Value == string.Empty ? 0 : Convert.ToDecimal(hdfPreUnitarioCodigo.Value);


                if (this.MiFactura.FacturasDetalles.Exists(x => x.ListaPrecioDetalle.Producto.IdProducto == producto.ListaPrecioDetalle.Producto.IdProducto))
                {
                    this.MiFactura.FacturasDetalles.FirstOrDefault(x => x.ListaPrecioDetalle.Producto.IdProducto == producto.ListaPrecioDetalle.Producto.IdProducto).Cantidad += this.txtCantidadCodigo.Decimal;
                }
                else
                    this.MiFactura.FacturasDetalles.Add(producto);

                this.lblDescripcionProductoCodigo.Text = "Ultimo Producto Cargado: " + producto.DescripcionProducto + " - Cantidad: " + producto.Cantidad + " - Precio: $" + producto.ListaPrecioDetalle.Precio;

                //AyudaProgramacion.CargarGrillaListas<VTAFacturasDetalles>(MiFactura.FacturasDetalles, true, this.gvItems, true);
                this.gvItems.DataSource = this.MiFactura.FacturasDetalles;
                this.gvItems.DataBind();
                ScriptManager.RegisterStartupScript(this.upItems, this.upItems.GetType(), "CalcularItemScript", "CalcularItem();", true);
                this.txtCantidadCodigo.Text = "1";
                //  ScriptManager.RegisterStartupScript(this.upItems, this.upItems.GetType(), "CargarProductosCallBackScript", "CargarProductosCallBack();", true);
                this.upItems.Update();
            }
        }
        //protected void txtCodigo_TextChanged(object sender, EventArgs e)
        //{
        //    GridViewRow row = ((GridViewRow)((TextBox)sender).Parent.Parent);
        //    this.MiIndiceDetalleModificar = row.RowIndex;

        //    string contenido = ((TextBox)sender).Text;
        //    CMPListasPreciosDetalles itemFiltro = new CMPListasPreciosDetalles();
        //    itemFiltro.Producto.IdProducto = contenido == string.Empty ? 0 : Convert.ToInt32(contenido);
        //    if (itemFiltro.Producto.IdProducto > 0)
        //    {
        //        itemFiltro.Fecha = DateTime.Now;
        //        itemFiltro.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
        //        itemFiltro.ListaPrecio.IdAfiliado = this.MiFactura.Afiliado.IdAfiliado;
        //        itemFiltro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
        //        itemFiltro = ComprasF.ListasPreciosDetallesObtenerDatosPorProducto(itemFiltro);
        //        //if (itemFiltro.IdListaPrecioDetalle > 0)
        //        //    //this.ctrBuscarProductoPopUp_ProductosBuscarSeleccionar(itemFiltro);
        //        //else
        //        //    ((TextBox)sender).Text = "0";
        //    }
        //}
        #endregion
        #region Control Clientes
        void CtrBuscarCliente_BuscarCliente(AfiAfiliados e)
        {
            if (e.IdAfiliado > 0)
            {
                ListItem item;
                VTAFacturas factura = new VTAFacturas();
                factura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
                factura.Afiliado.IdAfiliado = e.IdAfiliado;
                factura = FacturasF.FacturasObtenerDatosPreCargados(factura);
                if (factura.Filtro == "Precargados")
                {
                    if (factura.RemitoVentaAutomatico)
                    {
                        this.hdfRemitosMostrar.Value = "1";
                        this.chkGenerarRemito.Checked = true;
                    }
                    this.MapearObjetoControlesPrecarga(factura);
                    if (factura.IdEstadoRemito.HasValue)
                    {
                        item = ddlEstadosRemtios.Items.FindByValue(factura.IdEstadoRemito.Value.ToString());
                        if (item != null)
                            this.ddlEstadosRemtios.SelectedValue = item.Value;

                    }
                    if (factura.IdFilialEntregaRemito.HasValue)
                    {
                        item = ddlFilialEntrega.Items.FindByValue(factura.IdFilialEntregaRemito.Value.ToString());
                        if (item != null)
                            this.ddlFilialEntrega.SelectedValue = item.Value;
                    }
                }
                else
                {
                    this.ddlFilialPuntoVenta_SelectedIndexChanged(null, EventArgs.Empty);
                }
                this.IniciarGrilla();

                item = ddlTipoDocumentoCliente.Items.FindByValue(e.TipoDocumento.IdTipoDocumento.ToString());
                if (item == null)
                    this.ddlTipoDocumentoCliente.Items.Add(new ListItem(e.TipoDocumento.TipoDocumento, e.TipoDocumento.IdTipoDocumento.ToString()));
                this.ddlTipoDocumentoCliente.SelectedValue = e.TipoDocumento.IdTipoDocumento.ToString();

                this.txtCuit.Text = e.NumeroDocumento.ToString();
                this.txtRazonSocialCliente.Text = e.RazonSocial.ToString();

                if (e.CondicionFiscal.IdCondicionFiscal > 0)
                    this.ddlCondicionFiscalCliente.SelectedValue = e.CondicionFiscal.IdCondicionFiscal.ToString();

                e.Domicilios = AfiliadosF.AfiliadosObtenerDomicilios(e);

                AfiDomicilios domicilios = new AfiDomicilios();
                if (e.Domicilios.Count > 0)
                    domicilios = e.Domicilios.Find(x => Convert.ToInt32(x.Predeterminado) == 1);

                if (domicilios != null && domicilios.IdDomicilio > 0)
                {
                    this.txtDomicilio.Text = domicilios.Calle.ToString() + " " + domicilios.Numero;
                    this.txtLocalidad.Text = domicilios.Localidad.CodigoPostalDescripcion.ToString();
                }
            }
            else
            { }
        }
        //void ctrBuscarClientePopUp_AfiliadosBuscarSeleccionar(AfiAfiliados pAfiliado)
        //{
        //    //this.MiFactura.Afiliado = pAfiliado;
        //    this.MiFactura.Afiliado = AfiliadosF.AfiliadosObtenerDatos(pAfiliado);
        //    this.MapearObjetoAControlesAfiliado(pAfiliado);
        //    this.ddlFilialPuntoVenta_SelectedIndexChanged(null, EventArgs.Empty);
        //    this.UpdatePanel2.Update();
        //}

        //protected void ddlNumeroSocio_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string txtNumeroSocio = ((TextBox)sender).Text;
        //    AfiAfiliados parametro = new AfiAfiliados();
        //    parametro.IdAfiliado = txtNumeroSocio == string.Empty ? 0 : Convert.ToInt32(txtNumeroSocio);
        //    this.MiFactura.Afiliado = AfiliadosF.AfiliadosObtenerDatos(parametro);
        //    if (this.MiFactura.Afiliado.IdAfiliado != 0)
        //    {
        //        this.MapearObjetoAControlesAfiliado(this.MiFactura.Afiliado);
        //        this.ddlFilialPuntoVenta_SelectedIndexChanged(null, EventArgs.Empty);
        //    }
        //    else
        //    {
        //        //this.txtNumeroSocio.Text = string.Empty;
        //        this.txtCUIT.Text = string.Empty;
        //        this.txtSocio.Text = string.Empty;
        //        this.txtEstado.Text = string.Empty;
        //        this.txtCondicionFiscal.Text = string.Empty;
        //        parametro.CodigoMensaje = "NumeroSocioNoExiste";
        //        this.UpdatePanel2.Update();
        //        this.MostrarMensaje(parametro.CodigoMensaje, true);
        //    }
        //}

        //protected void btnBuscarCliente_Click(object sender, EventArgs e)
        //{
        //    this.ctrBuscarClientePopUp.IniciarControl(true);
        //}
        #endregion
        protected void ddlCantidadDecimales_OnClick(object sender, EventArgs e)
        {
            this.MiCantidadDecimales = Convert.ToInt32(this.ddlCantidadDecimales.SelectedValue);
            string numberSymbol = string.Empty;
            foreach (GridViewRow fila in this.gvItems.Rows)
            {
                Evol.Controls.CurrencyTextBox PrecioUnitario = (Evol.Controls.CurrencyTextBox)fila.FindControl("txtPrecioUnitario");
                PrecioUnitario.NumberOfDecimals = this.MiCantidadDecimales;
                numberSymbol = PrecioUnitario.Prefix == string.Empty ? "N" : "C";
                PrecioUnitario.Text = PrecioUnitario.Decimal.ToString(string.Concat(numberSymbol, this.MiCantidadDecimales));
            }
            ScriptManager.RegisterStartupScript(this.upItems, this.upItems.GetType(), "CalcularItemScript", "CalcularItem();", true);
            this.upItems.Update();
        }
        //protected void popUpMensajes_popUpMensajesPostBackAceptar()
        //{
        //    if (this.FacturaModificarDatosAceptar != null)
        //        this.FacturaModificarDatosAceptar(null, this.MiFactura);
        //}
        protected void MapearControlesAObjeto(VTAFacturas pFactura)
        {
            pFactura.TipoFactura.IdTipoFactura = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].IdTipoFactura;
            pFactura.TipoFactura.CodigoValor = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].CodigoValor;
            pFactura.TipoFactura.Descripcion = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].Descripcion;
            pFactura.ConceptoComprobante.IdConceptoComprobante = this.MisConceptosComprobantes[this.ddlConceptoComprobante.SelectedIndex].IdListaValorSistemaDetalle;
            pFactura.ConceptoComprobante.CodigoValor = this.MisConceptosComprobantes[this.ddlConceptoComprobante.SelectedIndex].CodigoValor;
            pFactura.ConceptoComprobante.Descripcion = this.MisConceptosComprobantes[this.ddlConceptoComprobante.SelectedIndex].Descripcion;

            pFactura.FechaFactura = Convert.ToDateTime(this.txtFechaFactura.Text);
            pFactura.FechaVencimiento = this.txtFechaVencimiento.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaVencimiento.Text);
            pFactura.Moneda = this.MisMonedas.FirstOrDefault(x => x.IdMoneda == Convert.ToInt32(this.ddlMonedas.SelectedValue));
            TGEMonedas mon = MisMonedas.Find(x => x.IdMoneda == Convert.ToInt32(this.ddlMonedas.SelectedValue));
            pFactura.MonedaCotizacion = mon.MonedeaCotizacion.MonedaCotizacion;
            pFactura.Observacion = this.txtObservacion.Text;
            pFactura.ObservacionComprobante = this.txtObservacionComprobante.Text;
            pFactura.NumeroFactura = this.txtNumeroFactura.Text;
            pFactura.CantidadCuotas = this.ddlCantidadCuotas.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlCantidadCuotas.SelectedValue);
            pFactura.AcopioFinanciero = this.chkAcopioFinanciero.Checked;
            //this.PersistirDatosGrilla();
            pFactura.Proveedor.IdProveedor = this.ddlProveedores.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlProveedores.SelectedValue);
            pFactura.IdBancoCuenta = this.ddlBancoCuenta.SelectedValue == string.Empty ? (Nullable<int>)null : Convert.ToInt32(this.ddlBancoCuenta.SelectedValue);
            pFactura.IdListaPrecio = this.ddlListaPrecio.SelectedValue == string.Empty ? (Nullable<int>)null : Convert.ToInt32(this.ddlListaPrecio.SelectedValue);
            if (pFactura.ConceptoComprobante.IdConceptoComprobante == (int)EnumConceptosComprobantes.Servicios
                || pFactura.ConceptoComprobante.IdConceptoComprobante == (int)EnumConceptosComprobantes.ProductosYServicios)
            {
                pFactura.PeriodoFacturadoDesde = this.txtPeriodoFechaDesde.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtPeriodoFechaDesde.Text);
                pFactura.PeriodoFacturadoHasta = this.txtPeriodoFechaHasta.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtPeriodoFechaHasta.Text);
            }
            this.MiFactura.Campos = this.ctrCamposValores.ObtenerLista();

            this.MiFactura.LoteCamposValores = this.ctrCamposValores.ObtenerListaCamposValores();
            this.MiFactura.ClienteRazonSocial = this.txtRazonSocialCliente.Text;
            this.MiFactura.ClienteIdCondicionFiscal = this.ddlCondicionFiscalCliente.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlCondicionFiscalCliente.SelectedValue);
            this.MiFactura.ClienteCondicionFiscal = this.ddlCondicionFiscalCliente.Text;
            this.MiFactura.ClienteIdTipoDocumento = this.ddlTipoDocumentoCliente.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoDocumentoCliente.SelectedValue);
            this.MiFactura.ClienteCuit = this.txtCuit.Text;
            this.MiFactura.ClienteDomicilio = this.txtDomicilio.Text;
            this.MiFactura.ClienteLocalidad = this.txtLocalidad.Text;
            //string key = this.Request.Form.AllKeys.First(x => x.Contains("$ddlNumeroSocio"));
            //if (!string.IsNullOrEmpty(key))
            //    pFactura.Afiliado.IdAfiliado = this.Request.Form[key] == string.Empty ? 0 : Convert.ToInt32(this.Request.Form[key]);
        }
        private void MapearObjetoControles(VTAFacturas pFactura)
        {
            //this.MapearObjetoAControlesAfiliado(pFactura.Afiliado);
            //this.txtFilialDescripcion.Text = pFactura.Filial.Filial;
            ListItem item = this.ddlFilialPuntoVenta.Items.FindByValue(pFactura.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta.ToString());
            if (item == null)
                this.ddlFilialPuntoVenta.Items.Add(new ListItem(pFactura.FilialPuntoVenta.TipoPuntoVenta.Descripcion, pFactura.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta.ToString()));
            this.ddlFilialPuntoVenta.SelectedValue = pFactura.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta.ToString(); //Convert.ToInt32(pFactura.PrefijoNumeroFactura).ToString();
            this.txtFechaFactura.Text = pFactura.FechaFactura.ToShortDateString();
            item = this.ddlTipoFactura.Items.FindByValue(pFactura.TipoFactura.IdTipoFactura.ToString());
            if (item == null)
                this.ddlTipoFactura.Items.Add(new ListItem(pFactura.TipoFactura.Descripcion, pFactura.TipoFactura.IdTipoFactura.ToString()));
            this.ddlTipoFactura.SelectedValue = pFactura.TipoFactura.IdTipoFactura.ToString();
            //this.txtPrefijoNumeroFactura.Text = pFactura.PrefijoNumeroFactura;
            item = this.ddlPrefijoNumeroFactura.Items.FindByText(pFactura.PrefijoNumeroFactura);
            if (item == null)
                this.ddlPrefijoNumeroFactura.Items.Add(new ListItem(pFactura.PrefijoNumeroFactura, Convert.ToInt32(pFactura.PrefijoNumeroFactura).ToString()));
            this.ddlPrefijoNumeroFactura.SelectedValue = Convert.ToInt32(pFactura.PrefijoNumeroFactura).ToString();
            this.txtNumeroFactura.Text = pFactura.NumeroFactura;
            this.ddlConceptoComprobante.SelectedValue = pFactura.ConceptoComprobante.IdConceptoComprobante.ToString();
            item = this.ddlMonedas.Items.FindByValue(pFactura.Moneda.IdMoneda.ToString());
            if (item == null)
                this.ddlMonedas.Items.Add(new ListItem(pFactura.Moneda.miMonedaDescripcion, pFactura.Moneda.IdMoneda.ToString()));
            this.ddlMonedas.SelectedValue = pFactura.Moneda.IdMoneda.ToString();
            if (pFactura.Moneda.IdMoneda != (int)EnumTGEMonedas.PesosArgentinos)
                this.ddlMonedas.SelectedItem.Text = String.Concat(this.ddlMonedas.SelectedItem.Text, " (", pFactura.MonedaCotizacion.ToString("C2"), " )");
            //TGEMonedas moneda = MisMonedas.FirstOrDefault(x => x.IdMoneda == Convert.ToInt32(this.ddlMonedas.SelectedValue));
            //SetInitializeCulture(moneda.Moneda);
            this.SetInitializeCulture(pFactura.Moneda.Moneda);
            this.txtObservacion.Text = pFactura.Observacion;
            this.txtObservacionComprobante.Text = pFactura.ObservacionComprobante;
            AyudaProgramacion.CargarGrillaListas<VTAFacturasDetalles>(pFactura.FacturasDetalles, true, this.gvItems, true);
            if (pFactura.FacturasTiposPercepciones.Count > 0)
            {
                AyudaProgramacion.CargarGrillaListas<VTAFacturasTiposPercepciones>(pFactura.FacturasTiposPercepciones, true, this.gvPercepciones, true);
            }
            if (pFactura.FacturasAsociadas.Count > 0)
            {
                AyudaProgramacion.CargarGrillaListas<VTAFacturas>(pFactura.FacturasAsociadas, false, this.gvDatos, true);
                this.pnlComprobantesAsociados.Visible = true;
            }

            item = this.ddlCantidadCuotas.Items.FindByValue(pFactura.CantidadCuotas.ToString());
            if (item == null && pFactura.CantidadCuotas > 1)
                this.ddlCantidadCuotas.Items.Add(new ListItem(pFactura.CantidadCuotas.ToString(), pFactura.CantidadCuotas.ToString()));
            this.ddlCantidadCuotas.SelectedValue = pFactura.CantidadCuotas <= 1 ? (1).ToString() : pFactura.CantidadCuotas.ToString();
            this.chkAcopioFinanciero.Checked = pFactura.AcopioFinanciero.HasValue ? pFactura.AcopioFinanciero.Value : false;
            this.txtTotalConIva.Text = pFactura.ImporteTotal.ToString("C2");
            this.txtTotalSinIva.Text = pFactura.ImporteSinIVA.ToString("C2");
            this.txtTotalIva.Text = pFactura.IvaTotal.ToString("C2");

            VTARemitos remito = FacturasF.RemitosObtenerPorFactura(pFactura);
            if (remito.IdRemito > 0)
            {
                this.ddlTipoRemito.Items.Add(new ListItem(remito.TipoFactura.Descripcion, remito.TipoFactura.IdTipoFactura.ToString()));
                this.ddlPrefijoNumeroRemito.Items.Add(new ListItem(remito.NumeroRemitoPrefijo, remito.NumeroRemitoPrefijo));
                this.ddlPrefijoNumeroRemito.SelectedValue = remito.NumeroRemitoPrefijo;
                this.txtNumeroRemito.Text = remito.NumeroRemitoSuFijo;
                this.ddlEstadosRemtios.Items.Add(new ListItem(remito.Estado.Descripcion, remito.Estado.IdEstado.ToString()));
                this.ddlEstadosRemtios.SelectedValue = remito.Estado.IdEstado.ToString();
                this.txtFechaEntrega.Text = remito.FechaEntrega.HasValue ? remito.FechaEntrega.Value.ToShortDateString() : string.Empty;
                this.ddlFilialEntrega.Items.Add(new ListItem(remito.FilialEntrega.Filial, remito.FilialEntrega.IdFilialEntrega.ToString()));
                this.ddlFilialEntrega.SelectedValue = remito.FilialEntrega.IdFilialEntrega.ToString();
                item = this.ddlTipoOperacionRemito.Items.FindByText(remito.TipoOperacion.IdTipoOperacion.ToString());
                if (item == null)
                    this.ddlTipoOperacionRemito.Items.Add(new ListItem(remito.TipoOperacion.TipoOperacion, Convert.ToInt32(remito.TipoOperacion.IdTipoOperacion).ToString()));
                this.ddlTipoOperacionRemito.SelectedValue = Convert.ToInt32(remito.TipoOperacion.IdTipoOperacion).ToString();
                //this.phRemitos.Visible = true;
                if (remito.IdDomicilio.HasValue)
                {
                    item = this.ddlDomicilioEntrega.Items.FindByValue(remito.IdDomicilio.Value.ToString());
                    if (item == null)
                    {
                        this.ddlDomicilioEntrega.Items.Add(new ListItem(remito.DomicilioEntrega, remito.IdDomicilio.ToString()));
                    }
                    this.ddlDomicilioEntrega.SelectedValue = remito.IdDomicilio.Value.ToString();
                    this.ddlDomicilioEntrega.SelectedItem.Text = remito.DomicilioEntrega;
                }
                this.txtRemitoObservacionComprobante.Text = remito.ObservacionComprobante;
                this.txtRemitoObservacionInterna.Text = remito.ObservacionInterna;
            }
            else
            //this.phRemitos.Visible = false;

            if (pFactura.Proveedor.IdProveedor.HasValue)
            {
                this.pnlProveedores.Visible = true;
                item = this.ddlProveedores.Items.FindByValue(pFactura.Proveedor.IdProveedor.Value.ToString());
                if (item == null)
                    this.ddlProveedores.Items.Add(new ListItem(pFactura.Proveedor.RazonSocial, pFactura.Proveedor.IdProveedor.Value.ToString()));
                this.ddlProveedores.SelectedValue = pFactura.Proveedor.IdProveedor.Value.ToString();
            }
            this.chkFacturaContado.Checked = pFactura.FacturaContado;
            if (pFactura.IdBancoCuenta.HasValue)
            {
                item = this.ddlBancoCuenta.Items.FindByValue(pFactura.IdBancoCuenta.Value.ToString());
                if (item == null)
                    this.ddlBancoCuenta.Items.Add(new ListItem(pFactura.BancoCuenta, pFactura.IdBancoCuenta.Value.ToString()));
                this.ddlBancoCuenta.SelectedValue = pFactura.IdBancoCuenta.Value.ToString();
            }
            this.ctrAsientoMostrar.IniciarControl(pFactura);
            //if (this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoA
            //        || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoB
            //        || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoC
            //        || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesA
            //        || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesB
            //        || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesC)
            //{
            //    this.upRemitos.Visible = false;
            //}

            if (pFactura.ConceptoComprobante.IdConceptoComprobante == (int)EnumConceptosComprobantes.Servicios
                || pFactura.ConceptoComprobante.IdConceptoComprobante == (int)EnumConceptosComprobantes.ProductosYServicios)
            {
                //this.phPeriodoFecha.Visible = true;
                this.txtPeriodoFechaDesde.Text = pFactura.PeriodoFacturadoDesde.HasValue ? pFactura.PeriodoFacturadoDesde.Value.ToShortDateString() : string.Empty;
                this.txtPeriodoFechaHasta.Text = pFactura.PeriodoFacturadoHasta.HasValue ? pFactura.PeriodoFacturadoHasta.Value.ToShortDateString() : string.Empty;
            }
            this.txtFechaVencimiento.Text = pFactura.FechaVencimiento.HasValue ? pFactura.FechaVencimiento.Value.ToShortDateString() : string.Empty;

            if (pFactura.IdListaPrecio.HasValue && pFactura.IdListaPrecio.Value > 0)
            {
                item = this.ddlListaPrecio.Items.FindByValue(pFactura.IdListaPrecio.Value.ToString());
                if (item == null)
                    this.ddlListaPrecio.Items.Add(new ListItem(pFactura.ListaPrecio, pFactura.IdListaPrecio.Value.ToString()));
                this.ddlListaPrecio.SelectedValue = pFactura.IdListaPrecio.Value.ToString();
            }

            this.ctrCamposValores.IniciarControl(this.MiFactura, new Objeto(), this.GestionControl);
            this.ctrCamposValores.IniciarControl(this.MiFactura, this.MiFactura.TipoFactura, this.GestionControl);
            this.ctrBuscarCliente.IniciarControl(this.MiFactura.Afiliado, this.GestionControl);

            this.MapearCliente(this.MiFactura);
        }
        private void MapearObjetoControlesPrecarga(VTAFacturas pFactura)
        {
            ListItem item = this.ddlFilialPuntoVenta.Items.FindByValue(pFactura.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta.ToString());
            if (item == null)
                this.ddlFilialPuntoVenta.Items.Add(new ListItem(pFactura.FilialPuntoVenta.TipoPuntoVenta.Descripcion, pFactura.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta.ToString()));
            this.ddlFilialPuntoVenta.SelectedValue = pFactura.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta.ToString(); //Convert.ToInt32(pFactura.PrefijoNumeroFactura).ToString();
            this.ddlFilialPuntoVenta_SelectedIndexChanged(null, EventArgs.Empty);

            item = this.ddlTipoFactura.Items.FindByValue(pFactura.TipoFactura.IdTipoFactura.ToString());
            if (item == null)
                this.ddlTipoFactura.Items.Add(new ListItem(pFactura.TipoFactura.Descripcion, pFactura.TipoFactura.IdTipoFactura.ToString()));
            this.ddlTipoFactura.SelectedValue = pFactura.TipoFactura.IdTipoFactura.ToString();
            this.ddlTipoFactura_SelectedIndexChanged(null, EventArgs.Empty);

            item = this.ddlPrefijoNumeroFactura.Items.FindByText(pFactura.PrefijoNumeroFactura);
            if (item == null)
                this.ddlPrefijoNumeroFactura.Items.Add(new ListItem(pFactura.PrefijoNumeroFactura, Convert.ToInt32(pFactura.PrefijoNumeroFactura).ToString()));
            this.ddlPrefijoNumeroFactura.SelectedValue = Convert.ToInt32(pFactura.PrefijoNumeroFactura).ToString();
            this.ddlPrefijoNumeroFactura_SelectedIndexChanged(null, EventArgs.Empty);

            this.txtFechaFactura.Text = pFactura.FechaFactura.ToShortDateString();
            this.txtFechaVencimiento.Text = pFactura.FechaFactura.ToShortDateString();

            this.ddlConceptoComprobante.SelectedValue = pFactura.ConceptoComprobante.IdConceptoComprobante.ToString();
            this.ddlConceptoComprobante_SelectedIndexChanged(null, EventArgs.Empty);

            item = this.ddlMonedas.Items.FindByValue(pFactura.Moneda.IdMoneda.ToString());
            if (item == null)
                this.ddlMonedas.Items.Add(new ListItem(pFactura.Moneda.miMonedaDescripcion, pFactura.Moneda.IdMoneda.ToString()));
            this.ddlMonedas.SelectedValue = pFactura.Moneda.IdMoneda.ToString();
            this.ddlMonedas_OnSelectedIndexChanged(null, EventArgs.Empty);

            this.txtObservacion.Text = pFactura.Observacion;
            this.txtObservacionComprobante.Text = pFactura.ObservacionComprobante;

            if (pFactura.IdBancoCuenta.HasValue)
            {
                item = this.ddlBancoCuenta.Items.FindByValue(pFactura.IdBancoCuenta.Value.ToString());
                if (item == null)
                    this.ddlBancoCuenta.Items.Add(new ListItem(pFactura.BancoCuenta, pFactura.IdBancoCuenta.Value.ToString()));
                this.ddlBancoCuenta.SelectedValue = pFactura.IdBancoCuenta.Value.ToString();
            }

            if (pFactura.IdFilialEntregaRemito.HasValue && pFactura.IdFilialEntregaRemito > 0)
            {
                item = this.ddlFilialEntrega.Items.FindByValue(pFactura.IdFilialEntregaRemito.ToString());
                if (item == null)
                    this.ddlFilialEntrega.Items.Add(new ListItem(pFactura.FilialEntregaRemito, pFactura.IdFilialEntregaRemito.ToString()));
                this.ddlFilialEntrega.SelectedValue = pFactura.IdFilialEntregaRemito.ToString();
            }

            if (pFactura.IdEstadoRemito.HasValue && pFactura.IdEstadoRemito > 0)
            {
                item = this.ddlEstadosRemtios.Items.FindByValue(pFactura.IdEstadoRemito.ToString());
                if (item == null)
                    this.ddlEstadosRemtios.Items.Add(new ListItem(pFactura.EstadoRemito, pFactura.IdEstadoRemito.ToString()));
                this.ddlEstadosRemtios.SelectedValue = pFactura.IdEstadoRemito.ToString();
            }

            if (pFactura.IdListaPrecio.HasValue && pFactura.IdListaPrecio.Value > 0)
            {
                item = this.ddlListaPrecio.Items.FindByValue(pFactura.IdListaPrecio.Value.ToString());
                if (item == null)
                    this.ddlListaPrecio.Items.Add(new ListItem(pFactura.ListaPrecio, pFactura.IdListaPrecio.Value.ToString()));
                this.ddlListaPrecio.SelectedValue = pFactura.IdListaPrecio.Value.ToString();
            }
            this.txtFechaEntrega.Text = pFactura.FechaFactura.ToShortDateString();
        }
        protected void CalcularTotal()
        {
            decimal? totalSinIva = 0;
            decimal? totalIva = 0;
            decimal? totalConIva = 0;
            decimal totalPercepciones = 0;
            // TOTAL PERCEPCIONES
            if (this.MiFactura.FacturasTiposPercepciones.Count() != 0)
            {
                totalPercepciones = this.MiFactura.FacturasTiposPercepciones.Sum(x => x.Importe);
                this.MiFactura.ImportePercepciones = totalPercepciones;
            }

            totalSinIva = this.MiFactura.FacturasDetalles.Sum(x => x.SubTotal == null ? 0 : x.SubTotal);
            totalIva = this.MiFactura.FacturasDetalles.Sum(x => x.ImporteIVA == null ? 0 : x.ImporteIVA);
            totalConIva = totalSinIva + totalIva + totalPercepciones;

            this.MiFactura.ImporteSinIVA = totalSinIva.Value;
            this.MiFactura.IvaTotal = totalIva.Value;
            this.MiFactura.ImporteTotal = totalConIva.Value;

            this.txtTotalConIva.Text = totalConIva.Value.ToString("C2");
            this.txtTotalSinIva.Text = totalSinIva.Value.ToString("C2");
            this.txtTotalIva.Text = totalIva.Value.ToString("C2");
        }
        #region Importar Remito
        void ctrBuscarRemitoPopUp_RemitosBuscarSeleccionar(VTARemitos e)
        {
            this.chkGenerarRemito.Checked = false;
            this.chkGenerarRemito.Enabled = false;
            this.MapearRemitoAFactura(e);
            AyudaProgramacion.CargarGrillaListas<VTAFacturasDetalles>(this.MiFactura.FacturasDetalles, false, gvItems, true);
            ScriptManager.RegisterStartupScript(this.upItems, this.upItems.GetType(), "CalcularItemScript", "CalcularItem();", true);
            this.upItems.Update();
        }
        private void MapearRemitoAFactura(VTARemitos e)
        {
            List<VTAFacturasDetalles> lista = new List<VTAFacturasDetalles>();
            foreach (VTARemitosDetalles detRemito in e.RemitosDetalles)
            {
                if (lista.Exists(x => x.ListaPrecioDetalle.Producto.IdProducto == detRemito.Producto.IdProducto && detRemito.Descripcion == x.Descripcion))
                {
                    lista.Find(x => x.ListaPrecioDetalle.Producto.IdProducto == detRemito.Producto.IdProducto).Cantidad += detRemito.Cantidad;
                    lista.Find(x => x.ListaPrecioDetalle.Producto.IdProducto == detRemito.Producto.IdProducto).RemitosDetalles.Add(detRemito);

                }
                else
                {
                    VTAFacturasDetalles fDetalle = new VTAFacturasDetalles();
                    fDetalle.ListaPrecioDetalle.Producto.IdProducto = detRemito.Producto.IdProducto;//ver como se carga la lista de precio
                    //fDetalle.ListaPrecioDetalle.IdListaPrecioDetalle = detRemito.IdListaPrecioDetalle; NO MAPEAR QUE VAYA EN 0 PARA QUE BUSQUE EL ULTIMO PRECIO
                    fDetalle.ListaPrecioDetalle.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    fDetalle.ListaPrecioDetalle.ListaPrecio.IdAfiliado = this.MiFactura.Afiliado.IdAfiliado;
                    fDetalle.ListaPrecioDetalle.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    fDetalle.ListaPrecioDetalle.IdMoneda = Convert.ToInt32(ddlMonedas.SelectedValue);

                    fDetalle.ListaPrecioDetalle = ComprasF.ListasPreciosDetallesObtenerDatosPorProducto(fDetalle.ListaPrecioDetalle);
                    fDetalle.DescripcionProducto = fDetalle.ListaPrecioDetalle.Producto.Descripcion;
                    fDetalle.Descripcion = detRemito.Descripcion;
                    fDetalle.PrecioUnitarioSinIva = fDetalle.ListaPrecioDetalle.CalcularPrecio(Convert.ToInt32(this.ddlCantidadCuotas.SelectedValue));
                    fDetalle.DetalleImportado = true;
                    fDetalle.Cantidad = detRemito.Cantidad;
                    if (this.MiFactura.Afiliado.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Socios)
                        fDetalle.DescuentoPorcentual = this.MiDescuentoSocio;

                    fDetalle.RemitosDetalles.Add(detRemito);
                    lista.Add(fDetalle);
                    fDetalle.IndiceColeccion = lista.IndexOf(fDetalle);
                }
            }
            //this.MiFactura.IdRemitoImportado = e.IdRemito;
            this.MiFactura.FacturasDetalles = lista;
        }
        protected void btnImportarRemito_Click(object sender, EventArgs e)
        {
            if (ctrBuscarCliente.MiAfiliado.IdAfiliado <= 0)
            {
                this.MiFactura.CodigoMensaje = "RemitoSeleccioneCliente";
                this.MostrarMensaje(this.MiFactura.CodigoMensaje, true);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(this.ddlMonedas.SelectedValue))
                {
                    VTARemitos remito = new VTARemitos();
                    remito.Afiliado = ctrBuscarCliente.MiAfiliado;
                    remito.Moneda.IdMoneda = Convert.ToInt32(ddlMonedas.SelectedValue);
                    this.ctrBuscarRemitoPopUp.IniciarControl(remito);
                }
                else
                {
                    this.MiFactura.CodigoMensaje = "FacturasImportarPresupuesto";
                    this.MostrarMensaje(this.MiFactura.CodigoMensaje, true);
                }
            }

        }
        #endregion
        #region Importar Presupuesto
        private void CtrBuscarPresupuestoPopUp_PresupuestosBuscarSeleccionar(VTAPresupuestos e)
        {
            this.chkGenerarRemito.Checked = false;
            this.chkGenerarRemito.Enabled = false;
            this.MapearPresupuestoAFactura(e);
            AyudaProgramacion.CargarGrillaListas<VTAFacturasDetalles>(this.MiFactura.FacturasDetalles, false, gvItems, true);
            ScriptManager.RegisterStartupScript(this.upItems, this.upItems.GetType(), "CalcularItemScript", "CalcularItem();", true);
            this.upItems.Update();
        }
        private void MapearPresupuestoAFactura(VTAPresupuestos e)
        {
            List<VTAFacturasDetalles> lista = new List<VTAFacturasDetalles>();
            VTAFacturasDetalles fDetalle;
            this.MiFactura.FacturasDetalles = new List<VTAFacturasDetalles>();

            foreach (VTAPresupuestosDetalles detPresu in e.PresupuestosDetalles)
            {
                fDetalle = new VTAFacturasDetalles();
                fDetalle.ListaPrecioDetalle.Producto.IdProducto = detPresu.ListaPrecioDetalle.Producto.IdProducto;//ver como se carga la lista de precio
                fDetalle.ListaPrecioDetalle.IdListaPrecioDetalle = detPresu.ListaPrecioDetalle.IdListaPrecioDetalle;
                fDetalle.ListaPrecioDetalle.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                fDetalle.ListaPrecioDetalle.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                fDetalle.ListaPrecioDetalle = ComprasF.ListasPreciosDetallesObtenerDatosPorProducto(fDetalle.ListaPrecioDetalle);
                fDetalle.DescripcionProducto = detPresu.DescripcionProducto;
                fDetalle.Descripcion = detPresu.Descripcion;
                fDetalle.Cantidad = detPresu.Cantidad;
                fDetalle.ModificaPrecio = true;
                fDetalle.PrecioUnitarioSinIva = detPresu.PrecioUnitarioSinIva;
                fDetalle.DescuentoPorcentual = detPresu.DescuentoPorcentual;
                fDetalle.DescuentoImporte = detPresu.DescuentoImporte;
                if (this.MiFactura.Afiliado.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Socios)
                    fDetalle.DescuentoPorcentual = this.MiDescuentoSocio;

                lista.Add(fDetalle);
                fDetalle.IndiceColeccion = lista.IndexOf(fDetalle);
            }
            this.MiFactura.FacturasDetalles = lista;
        }
        protected void btnImportarPresupuesto_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.ddlMonedas.SelectedValue))
            {
                this.MiFactura.CodigoMensaje = "FacturasImportarPresupuesto";
                this.MostrarMensaje(this.MiFactura.CodigoMensaje, true);
                return;
            }

            if (ctrBuscarCliente.MiAfiliado.IdAfiliado <= 0)
            {
                this.MiFactura.CodigoMensaje = "RemitoSeleccioneCliente";
                this.MostrarMensaje(this.MiFactura.CodigoMensaje, true);
                return;
            }
            else
            {
                VTAPresupuestos remito = new VTAPresupuestos();
                remito.Afiliado = ctrBuscarCliente.MiAfiliado;
                remito.Moneda.IdMoneda = Convert.ToInt32(ddlMonedas.SelectedValue);
                this.ctrBuscarPresupuestoPopUp.IniciarControl(remito);
            }

        }
        #endregion
        #region Importar Nota Pedido
        void ctrBuscarNotaPedidoPopUp_NotasPedidoBuscarSeleccionar(VTANotasPedidos e)
        {
            MiNotaPedido = e;

            this.chkGenerarRemito.Checked = false;
            this.chkGenerarRemito.Enabled = false;
            this.MapearNotaPedidoAFactura(e);
            AyudaProgramacion.CargarGrillaListas<VTAFacturasDetalles>(this.MiFactura.FacturasDetalles, false, gvItems, true);
            ScriptManager.RegisterStartupScript(this.upItems, this.upItems.GetType(), "CalcularItemScript", "CalcularItem();", true);
            this.upItems.Update();

        }
        private void MapearNotaPedidoAFactura(VTANotasPedidos e)
        {
            List<VTAFacturasDetalles> lista = new List<VTAFacturasDetalles>();
            foreach (VTANotasPedidosDetalles detNotaPedido in e.NotasPedidosDetalles)
            {
                if (lista.Exists(x => x.ListaPrecioDetalle.Producto.IdProducto == detNotaPedido.Producto.IdProducto && detNotaPedido.Producto.Descripcion == x.ListaPrecioDetalle.Producto.Descripcion))
                {
                    lista.Find(x => x.ListaPrecioDetalle.Producto.IdProducto == detNotaPedido.Producto.IdProducto).Cantidad += detNotaPedido.Cantidad;
                    lista.Find(x => x.ListaPrecioDetalle.Producto.IdProducto == detNotaPedido.Producto.IdProducto).NotasPedidosDetalles.Add(detNotaPedido);
                }
                else
                {
                    VTAFacturasDetalles fDetalle = new VTAFacturasDetalles();
                    fDetalle.ListaPrecioDetalle.Producto.IdProducto = detNotaPedido.Producto.IdProducto;//ver como se carga la lista de precio
                    //fDetalle.ListaPrecioDetalle.IdListaPrecioDetalle = detNotaPedido.IdListaPrecioDetalle; NO MAPEAR QUE VAYA EN 0 PARA QUE BUSQUE EL ULTIMO PRECIO
                    fDetalle.ListaPrecioDetalle.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    fDetalle.ListaPrecioDetalle.ListaPrecio.IdAfiliado = this.MiFactura.Afiliado.IdAfiliado;
                    fDetalle.ListaPrecioDetalle.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    fDetalle.ListaPrecioDetalle.IdMoneda = Convert.ToInt32(ddlMonedas.SelectedValue);

                    fDetalle.ListaPrecioDetalle = ComprasF.ListasPreciosDetallesObtenerDatosPorProducto(fDetalle.ListaPrecioDetalle);
                    fDetalle.DescripcionProducto = fDetalle.ListaPrecioDetalle.Producto.Descripcion;
                    fDetalle.Descripcion = detNotaPedido.Descripcion;
                    fDetalle.PrecioUnitarioSinIva = fDetalle.ListaPrecioDetalle.CalcularPrecio(Convert.ToInt32(this.ddlCantidadCuotas.SelectedValue));
                    fDetalle.DetalleImportado = true;
                    fDetalle.Cantidad = detNotaPedido.Cantidad;
                    if (this.MiFactura.Afiliado.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Socios)
                        fDetalle.DescuentoPorcentual = this.MiDescuentoSocio;

                    fDetalle.NotasPedidosDetalles.Add(detNotaPedido);
                    lista.Add(fDetalle);
                    fDetalle.IndiceColeccion = lista.IndexOf(fDetalle);
                }
            }
            this.MiFactura.FacturasDetalles = lista;
        }
        protected void btnImportarNotaPedido_Click(object sender, EventArgs e)
        {
            if (ctrBuscarCliente.MiAfiliado.IdAfiliado <= 0)
            {
                this.MiFactura.CodigoMensaje = "RemitoSeleccioneCliente";
                this.MostrarMensaje(this.MiFactura.CodigoMensaje, true);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(this.ddlMonedas.SelectedValue))
                {
                    VTANotasPedidos notaPedido = new VTANotasPedidos();
                    notaPedido.Afiliado = ctrBuscarCliente.MiAfiliado;
                    notaPedido.Moneda.IdMoneda = Convert.ToInt32(ddlMonedas.SelectedValue);
                    this.ctrBuscarNotaPedidoPopUp.IniciarControl(notaPedido);
                }
                else
                {
                    this.MiFactura.CodigoMensaje = "FacturasImportarPresupuesto";
                    this.MostrarMensaje(this.MiFactura.CodigoMensaje, true);
                }
            }
        }
        #endregion
        #region Grilla Percepciones
        private void PersistirPercepciones()
        {
            foreach (GridViewRow fila in this.gvPercepciones.Rows)
            {
                DropDownList ddlPercepciones = ((DropDownList)fila.FindControl("ddlPercepciones"));
                decimal importePercepcion = ((TextBox)fila.FindControl("txtImportePercepcion")).Text == string.Empty ? 0 : decimal.Parse(((TextBox)fila.FindControl("txtImportePercepcion")).Text, NumberStyles.Currency);
                decimal porcentaje = ((CurrencyTextBox)fila.FindControl("txtPorcentajePercepcion")).Decimal;
                if (ddlPercepciones.SelectedValue != "")
                {
                    this.MiFactura.FacturasTiposPercepciones[fila.RowIndex].TipoPercepcion.IdTipoPercepcion = Convert.ToInt32(ddlPercepciones.SelectedValue);
                }
                if (porcentaje != 0)
                    this.MiFactura.FacturasTiposPercepciones[fila.RowIndex].Porcentaje = porcentaje;

                if (importePercepcion != 0)
                {
                    this.MiFactura.FacturasTiposPercepciones[fila.RowIndex].Importe = importePercepcion;
                }

                //((Label)fila.FindControl("lblImporteIva")).Text = (this.MiSolicitud.SolicitudPagoDetalles[fila.RowIndex].ImporteIvaTotal).ToString();
            }
            this.CalcularTotal();
        }
        protected void gvPercepciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            ///this.MiIndiceDetalleModificar = Convert.ToInt32(e.CommandArgument);
            int index = Convert.ToInt32(e.CommandArgument);
            this.MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            if (e.CommandName == "Borrar")
            {
                //ELIMINA FILA
                //int indiceColeccion = Convert.ToInt32(e.CommandArgument);
                this.MiFactura.FacturasTiposPercepciones.RemoveAt(this.MiIndiceDetalleModificar);
                this.MiFactura.FacturasTiposPercepciones = AyudaProgramacion.AcomodarIndices<VTAFacturasTiposPercepciones>(this.MiFactura.FacturasTiposPercepciones);
                AyudaProgramacion.CargarGrillaListas<VTAFacturasTiposPercepciones>(this.MiFactura.FacturasTiposPercepciones, false, this.gvPercepciones, true);
                //this.CalcularTotal();
            }

        }
        protected void gvPercepciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                VTAFacturasTiposPercepciones item = (VTAFacturasTiposPercepciones)e.Row.DataItem;
                TextBox porcentajePercepcion = (TextBox)e.Row.FindControl("txtPorcentajePercepcion");
                DropDownList ddlPercepciones = ((DropDownList)e.Row.FindControl("ddlPercepciones"));
                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");

                ddlPercepciones.Enabled = false;
                porcentajePercepcion.Enabled = false;
                btnEliminar.Visible = false;

                ddlPercepciones.DataSource = this.MisPercepciones;//TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.AFIPTiposPercepciones);
                ddlPercepciones.DataValueField = "IdListaValorSistemaDetalle";
                ddlPercepciones.DataTextField = "Descripcion";
                ddlPercepciones.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(ddlPercepciones, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                ListItem itemCombo;

                switch (GestionControl)
                {
                    case Gestion.Agregar:
                        ddlPercepciones.SelectedValue = item.TipoPercepcion.IdTipoPercepcion.ToString();
                        if (this.MiModuloFactura != ModuloFactura.ReservasTurismo)
                        {


                            ddlPercepciones.Enabled = true;
                            porcentajePercepcion.Attributes.Add("onchange", "CalcularPercepcion();");
                            porcentajePercepcion.Enabled = true;
                            btnEliminar.Visible = true;
                        }
                        break;
                    case Gestion.Consultar:
                        itemCombo = ddlPercepciones.Items.FindByValue(item.TipoPercepcion.IdTipoPercepcion.ToString());
                        if (itemCombo == null)
                            ddlPercepciones.Items.Add(new ListItem(item.TipoPercepcion.IdTipoPercepcion.ToString(), item.TipoPercepcion.Descripcion));
                        ddlPercepciones.SelectedValue = item.TipoPercepcion.IdTipoPercepcion.ToString();
                        ddlPercepciones.Enabled = false;
                        break;
                    case Gestion.Autorizar:
                        itemCombo = ddlPercepciones.Items.FindByValue(item.TipoPercepcion.IdTipoPercepcion.ToString());
                        if (itemCombo == null)
                            ddlPercepciones.Items.Add(new ListItem(item.TipoPercepcion.IdTipoPercepcion.ToString(), item.TipoPercepcion.Descripcion));
                        ddlPercepciones.SelectedValue = item.TipoPercepcion.IdTipoPercepcion.ToString();
                        ddlPercepciones.Enabled = false;
                        break;
                    default:
                        break;

                }

            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //if (this.GestionControl == Gestion.Consultar
                //    || this.GestionControl == Gestion.Autorizar
                //    || this.GestionControl == Gestion.Anular)
                //{
                Label lblImportePercepciones = (Label)e.Row.FindControl("lblImportePercepciones");
                lblImportePercepciones.Text = this.MiFactura.ImportePercepciones.ToString("C2");
                //}
                //else if (this.GestionControl == Gestion.Agregar)
                //{
                //    Label lblImportePercepciones = (Label)e.Row.FindControl("lblImportePercepciones");
                //    lblImportePercepciones.Text = this.MiSolicitud.SolicitudPagoTiposPercepciones.SuMm(x => x.Importe).ToString("C2");
                //}
            }
        }
        protected void btnAgregarPercepcion_Click(object sender, EventArgs e)
        {
            VTAFacturasTiposPercepciones item;
            item = new VTAFacturasTiposPercepciones();
            this.MiFactura.FacturasTiposPercepciones.Add(item);
            item.IndiceColeccion = this.MiFactura.FacturasTiposPercepciones.IndexOf(item);
            //this.CalcularTotal();
            AyudaProgramacion.CargarGrillaListas<VTAFacturasTiposPercepciones>(this.MiFactura.FacturasTiposPercepciones, false, this.gvPercepciones, true);
            this.gvPercepciones.Rows[item.IndiceColeccion].FindControl("ddlPercepciones").Focus();
            this.upPercepciones.Update();

        }
        #endregion
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            try
            {
                this.btnAceptar.Visible = false;
                this.Page.Validate("Aceptar");
                if (!this.Page.IsValid)
                {
                    guardo = false;
                    //this.UpdatePanel2.Update();
                    this.UpdatePanel1.Update();
                    ScriptManager.RegisterStartupScript(this.upItems, this.upItems.GetType(), "CalcularItemScript", "CalcularItem();", true);
                    this.upItems.Update();
                    this.upRemitos.Update();
                    this.btnAceptar.Visible = true;
                    return;
                }
                this.MiFactura.DirPath = this.Request.PhysicalApplicationPath;
                this.MiFactura.AppPath = this.ObtenerAppPath();
                this.MiFactura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                //if (MiFactura.TipoFactura.Signo == -1)
                //{
                this.MiFactura.ObtenerListaFacturasDetalles();
                //}
                switch (this.GestionControl)
                {
                    case Gestion.Agregar:
                        this.MapearControlesAObjeto(this.MiFactura);
                        //PersistirDatosGrilla();
                        this.MiFactura.FacturasDetalles = this.MiFactura.FacturasDetalles.Where(x => x.EstadoColeccion == EstadoColecciones.Agregado).ToList();
                        AyudaProgramacion.AcomodarIndices<VTAFacturasDetalles>(this.MiFactura.FacturasDetalles);
                        AyudaProgramacion.CargarGrillaListas(this.MiFactura.FacturasDetalles, false, this.gvItems, true);
                        this.upItems.Update();
                        this.MiFactura.UsuarioAlta.IdUsuarioAlta = this.MiFactura.UsuarioLogueado.IdUsuario;
                        this.MiFactura.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                        this.MiFactura.RemitoVentaAutomatico = this.chkGenerarRemito.Checked;
                        if (this.MiFactura.RemitoVentaAutomatico)
                        {
                            this.MiRemito.FechaAlta = DateTime.Now;
                            this.MiRemito.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                            this.MiRemito.NumeroRemitoSuFijo = this.txtNumeroRemito.Text;
                            this.MiRemito.Estado.IdEstado = Convert.ToInt32(this.ddlEstadosRemtios.SelectedValue);
                            this.MiRemito.Estado.Descripcion = this.ddlEstadosRemtios.SelectedItem.Text;
                            this.MiRemito.FilialEntrega.IdFilialEntrega = Convert.ToInt32(this.ddlFilialEntrega.SelectedValue);
                            if (this.MiRemito.Estado.IdEstado == (int)EstadosRemitos.Entregado)
                                this.MiRemito.FechaEntrega = Convert.ToDateTime(this.txtFechaEntrega.Text);
                            this.MiRemito.TipoOperacion.IdTipoOperacion = this.ddlTipoOperacionRemito.SelectedValue == string.Empty ? (int)EnumTGETiposOperaciones.RemitosVentas : Convert.ToInt32(this.ddlTipoOperacionRemito.SelectedValue);
                            this.MiRemito.ObservacionComprobante = this.txtRemitoObservacionComprobante.Text;
                            this.MiRemito.ObservacionInterna = this.txtRemitoObservacionInterna.Text;
                            this.MiRemito.DomicilioEntrega = ddlDomicilioEntrega.SelectedValue == string.Empty ? string.Empty : this.ddlDomicilioEntrega.SelectedItem.Text;
                            this.MiRemito.IdDomicilio = ddlDomicilioEntrega.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlDomicilioEntrega.SelectedValue);
                        }
                        this.MiFactura.FacturaContado = this.chkFacturaContado.Checked;
                        if (this.MiFactura.FacturaContado)
                        {
                            this.MiFactura.PrefijoNumeroRecibo = this.txtPrefijoNumeroRecibo.Text;
                            this.MiFactura.NumeroRecibo = this.txtNumeroRecibo.Text;
                        }
                        if (this.MiFactura.FacturasTiposPercepciones.Count() != 0)
                            this.MiFactura.FacturasTiposPercepciones.ForEach(x => x.EstadoColeccion = EstadoColecciones.Agregado);

                        guardo = FacturasF.FacturasAgregar(this.MiFactura, this.MiRemito);
                        if (guardo)
                        {
                            this.ctrAsientoMostrar.IniciarControl(this.MiFactura);
                            if (this.MiFactura.Estado.IdEstado == (int)EstadosFacturas.FESinValidadaAfip)
                            {
                                this.MostrarMensaje("ResultadoTransaccionFacturaElectronicaRechazo", System.Drawing.Color.BlueViolet, true, this.MiFactura.CodigoMensajeArgs);
                            }
                            else if (this.MiFactura.Estado.IdEstado == (int)EstadosFacturas.Cobrada)
                            {
                                this.btnImprimir.Visible = true;
                                this.btnEnviarMail.Visible = true;
                                this.MostrarArchivo();
                            }
                            else if (this.chkFacturaContado.Checked && !this.MiFactura.FacturaContado)
                            {
                                this.btnImprimir.Visible = true;
                                this.btnEnviarMail.Visible = true;
                                if (this.MiModuloFactura != ModuloFactura.ReservasTurismo)
                                    this.btnAgregarOC.Visible = true;

                                this.MostrarMensaje("ResultadoTransaccionFacturaContado", System.Drawing.Color.BlueViolet, true, this.MiFactura.CodigoMensajeArgs);
                                this.MostrarArchivo();
                            }
                            else if (this.MiFactura.Tabla == "HTLReservas")
                            {
                                this.btnImprimir.Visible = true;
                                this.btnAgregarOC.Visible = false;
                                this.MostrarArchivo();
                            }
                            else if (this.MiFactura.Tabla == "CarTiposCargosAfiliadosFormasCobros")
                            {
                                this.btnImprimir.Visible = true;
                                this.btnAgregarOC.Visible = false;
                                this.MostrarArchivo();
                            }
                            else
                            {
                                this.btnImprimir.Visible = true;
                                this.btnEnviarMail.Visible = true;
                                this.btnAgregarOC.Visible = true;
                                this.MostrarArchivo();
                                //this.MiFactura.CodigoMensaje = "GenerarOrdenCobroFactura";
                                //this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiFactura.CodigoMensaje), true);
                            }
                        }
                        break;
                    case Gestion.Anular:
                        this.MiFactura.Estado.IdEstado = (int)EstadosFacturas.Baja;
                        this.MiFactura.FechaAnulacion = DateTime.Now;
                        this.MiFactura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                        guardo = FacturasF.FacturasAnular(this.MiFactura);
                        if (guardo)
                        {
                            this.MostrarMensaje(this.MiFactura.CodigoMensaje, false);
                        }
                        break;
                    case Gestion.Modificar:
                        this.MapearControlesAObjeto(this.MiFactura);
                        guardo = FacturasF.FacturasModificar(this.MiFactura);
                        if (guardo)
                        {
                            this.MostrarMensaje(this.MiFactura.CodigoMensaje, false);
                        }
                        break;
                    default:
                        break;
                }
                if (!guardo)
                {
                    this.btnAceptar.Visible = true;
                    this.MostrarMensaje(this.MiFactura.CodigoMensaje, true, this.MiFactura.CodigoMensajeArgs);
                    if (this.MiFactura.dsResultado != null)
                    {
                        this.ctrPopUpGrilla.IniciarControl(this.MiFactura);
                        this.MiFactura.dsResultado = null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje(ex.Message, true);
            }
            finally
            {
                this.btnAceptar.Visible = !guardo;
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.MiFactura.IdRefTabla.HasValue)
            {
                switch (this.MiFactura.Tabla)
                {
                    case "HTLReservas":
                        this.MisParametrosUrl.Add("IdReserva", this.MiFactura.IdRefTabla);
                        break;
                    case "CarTiposCargosAfiliadosFormasCobros":
                        if (!(this.MisParametrosUrl["IdTipoCargoAfiliadoFormaCobro"].ToString() == this.MiFactura.IdRefTabla.ToString()))
                        {
                            this.MisParametrosUrl.Remove("IdTipoCargoAfiliadoFormaCobro");
                            this.MisParametrosUrl.Add("IdTipoCargoAfiliadoFormaCobro", this.MiFactura.IdRefTabla);
                        }
                        break;
                    default:
                        break;
                }
            }
            if (this.FacturaModificarDatosCancelar != null)
                this.FacturaModificarDatosCancelar();
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            this.MostrarArchivo();
        }
        protected void btnEnviarMail_Click(object sender, EventArgs e)
        {
            MailMessage mail = new MailMessage();
            if (FacturasF.FacturaArmarMail(this.MiFactura, mail))
            {
                this.popUpMail.IniciarControl(mail, this.MiFactura);
            }
        }
        protected void btnAgregarOC_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable
            {
                { "IdAfiliado", this.MiFactura.Afiliado.IdAfiliado },
                { "IdRefFacturaOCAutomatica", this.MiFactura.IdFactura },
                { "IdTipoOperacion", this.MiFactura.TipoOperacion.IdTipoOperacion }
            };
            if (!string.IsNullOrEmpty(this.paginaSegura.viewStatePaginaSegura))
                this.MisParametrosUrl.Add("UrlReferrer", this.paginaSegura.viewStatePaginaSegura.ToString());
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosFacturasAgregar.aspx"), true);
        }
        private void MostrarArchivo()
        {
            VTAFacturas factura = this.MiFactura;
            factura = FacturasF.FacturasObtenerDatosCompletos(factura);
            factura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            VTAFacturas facturaPdf = FacturasF.FacturasObtenerArchivo(factura);

            List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
            TGEArchivos archivo = new TGEArchivos();

            archivo.Archivo = facturaPdf.FacturaPDF;
            if (archivo.Archivo != null)
                listaArchivos.Add(archivo);

            VTARemitos remito = FacturasF.RemitosObtenerPorFactura(factura);
            archivo = new TGEArchivos();
            if (remito.IdRemito > 0)
            {
                remito.UsuarioLogueado = factura.UsuarioLogueado;
                remito = FacturasF.RemitosObtenerArchivo(remito);
                archivo.Archivo = remito.RemitoPDF;
                if (archivo.Archivo != null)
                    listaArchivos.Add(archivo);
            }
            archivo = new TGEArchivos();
            RepReportes reporte = new RepReportes();
            RepParametros param = new RepParametros
            {
                ValorParametro = this.MiFactura.IdFactura.ToString()
            };
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdFactura";
            reporte.Parametros.Add(param);
            if (factura.FacturaContado)
            {
                TGEPlantillas plantilla = new TGEPlantillas();
                plantilla.Codigo = "OrdenesCobros";
                plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

                reporte.StoredProcedure = plantilla.NombreSP;
                DataSet ds = ReportesF.ReportesObtenerDatos(reporte);

                archivo.Archivo = ReportesF.ExportPDFGenerarReportePDF(plantilla, ds, "IdFactura", this.MiFactura.UsuarioLogueado);
                if (archivo.Archivo != null)
                    listaArchivos.Add(archivo);
            }
            TGEEmpresas empresa = TGEGeneralesF.EmpresasSeleccionar();
            string nombreArchivo = string.Concat(empresa.CUIT, "_", factura.TipoFactura.CodigoValor, "_", factura.PrefijoNumeroFactura, "_", factura.NumeroFactura, ".pdf");
            ExportPDF.ConvertirArchivoEnPdf(this.UpdatePanel3, listaArchivos, nombreArchivo);
        }
        #region "Comprobantes Asociados"
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlTipoComprobanteAsociado.SelectedValue))
            {
                VTAFacturas filtro = new VTAFacturas();
                string[] ar = ddlTipoComprobanteAsociado.SelectedItem.ToString().Split('-');
                string prefijoNumeroFactura = ar[1].ToString().Trim();
                string numeroFactura = ar[2].ToString().Trim();
                filtro.IdFactura = ddlTipoComprobanteAsociado.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoComprobanteAsociado.SelectedValue);
                filtro.Afiliado.IdAfiliado = MiFactura.Afiliado.IdAfiliado;
                filtro.PrefijoNumeroFactura = prefijoNumeroFactura;
                filtro.NumeroFactura = numeroFactura;
                filtro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
                List<VTAFacturas> lista = FacturasF.FacturasObtenerListaComboAsociadosFiltro(filtro);
                if (lista.Count == 1)
                {
                    if (this.MiModuloFactura == ModuloFactura.Hotel)
                    {
                        this.MiFactura.FacturasDetalles = FacturasF.FacturasDetallesObtenerPorIdFacturaRefTabla(filtro);
                        AyudaProgramacion.CargarGrillaListas<VTAFacturasDetalles>(this.MiFactura.FacturasDetalles, false, this.gvItems, true);
                        ScriptManager.RegisterStartupScript(this.upItems, this.upItems.GetType(), "CalcularItemScript", "CalcularItem();", true);
                        this.upItems.Update();
                        this.CalcularTotal();
                        this.MiFactura.FacturasAsociadas.Add(lista[0]);
                        AyudaProgramacion.CargarGrillaListas<VTAFacturas>(this.MiFactura.FacturasAsociadas, false, this.gvDatos, true);
                    }
                    else
                    {
                        this.MiFactura.FacturasAsociadas.Add(lista[0]);
                        this.VerificarCargosFacturados(lista[0]);
                        AyudaProgramacion.CargarGrillaListas<VTAFacturas>(this.MiFactura.FacturasAsociadas, false, this.gvDatos, true);
                    }
                }
            }
            //else
            //    this.MostrarMensaje("ComprobanteNoEncontrado", true);
        }
        protected void VerificarCargosFacturados(VTAFacturas facturas)
        {
            VTAFacturas fact = FacturasF.FacturasValidacionesCuentasCorrientesCargosFacturados(facturas);
            if (fact.EsFacturaCargos)
            {
                this.phAgregarItem.Visible = false;
                //this.MiFactura.FacturasDetalles = new List<VTAFacturasDetalles>();
                this.MiFactura.EsFacturaCargos = true;
                //MiFactura.FacturasDetalles.AddRange(facturas.FacturasDetalles);
                //AyudaProgramacion.CargarGrillaListas<VTAFacturasDetalles>(MiFactura.FacturasDetalles, false, this.gvItems, true);
                //this.upItems.Update();
                //CalcularTotal();
            }
            else
            {
                this.phAgregarItem.Visible = true;
                this.MiFactura.EsFacturaCargos = false;
            }
            this.MiFactura.FacturasDetalles = new List<VTAFacturasDetalles>();
            this.MiFactura.FacturasDetalles.AddRange(facturas.FacturasDetalles);
            AyudaProgramacion.CargarGrillaListas<VTAFacturasDetalles>(this.MiFactura.FacturasDetalles, false, gvItems, true);
            ScriptManager.RegisterStartupScript(this.upItems, this.upItems.GetType(), "CalcularItemScript", "CalcularItem();", true);
            this.upItems.Update();
            this.CalcularTotal();
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            VTAFacturas factura = this.MiFactura.FacturasAsociadas[indiceColeccion];

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.MiFactura.FacturasAsociadas.Remove(factura);
                AyudaProgramacion.AcomodarIndices<VTAFacturas>(this.MiFactura.FacturasAsociadas);
                AyudaProgramacion.CargarGrillaListas<VTAFacturas>(this.MiFactura.FacturasAsociadas, false, this.gvDatos, true);
                this.upComprobantesAsociados.Update();

                this.MiFactura.FacturasDetalles = new List<VTAFacturasDetalles>();
                this.IniciarGrilla();
                this.upItems.Update();
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            VTAFacturas parametros = this.BusquedaParametrosObtenerValor<VTAFacturas>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<VTAFacturas>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MiFactura.FacturasAsociadas;
            this.gvDatos.DataBind();
        }
        #endregion
        #region "clientes"
        protected void btnTxtCuitBlur_Click(object sender, EventArgs e)
        {
            AfiAfiliados proveedor = new AfiAfiliados();

            List<AfiTiposDocumentos> tiposDocumentos = new List<AfiTiposDocumentos>();

            tiposDocumentos = AfiliadosF.TipoDocumentosObtenerLista();

            AfiTiposDocumentos documento = new AfiTiposDocumentos();
            documento = tiposDocumentos.Find(x => x.IdTipoDocumento == Convert.ToInt32(this.ddlTipoDocumentoCliente.SelectedValue));

            if (this.txtCuit.Decimal > 0)
            {
                proveedor.TipoDocumento.AfipCodigo = documento.AfipCodigo;
                proveedor.NumeroDocumento = Convert.ToInt64(txtCuit.Text);

                if (!AfiliadosF.AfiliadosObtenerDatosAFIP(proveedor))
                {
                    //this.txtCuit.Decimal = 0;
                    this.MostrarMensaje(proveedor.CodigoMensaje, true);
                }
            }
            this.MiFactura.ClienteRazonSocial = proveedor.RazonSocial;
            this.MiFactura.ClienteIdCondicionFiscal = proveedor.CondicionFiscal.IdCondicionFiscal;
            this.MiFactura.ClienteCondicionFiscal = proveedor.CondicionFiscal.Descripcion;
            this.MiFactura.ClienteIdTipoDocumento = documento.IdTipoDocumento.Value;
            this.MiFactura.ClienteCuit = proveedor.NumeroDocumento.ToString();
            this.MapearCliente(MiFactura);
        }
        protected void MapearCliente(VTAFacturas pFactura)
        {
            this.txtRazonSocialCliente.Text = this.MiFactura.ClienteRazonSocial.ToString();
            this.txtCuit.Text = this.MiFactura.ClienteCuit;
            this.txtDomicilio.Text = this.MiFactura.ClienteDomicilio;
            this.txtLocalidad.Text = this.MiFactura.ClienteLocalidad;

            if (this.MiFactura.ClienteIdTipoDocumento > 0)
                this.ddlTipoDocumentoCliente.SelectedValue = this.MiFactura.ClienteIdTipoDocumento.ToString();

            if (this.MiFactura.ClienteIdCondicionFiscal > 0)
                this.ddlCondicionFiscalCliente.SelectedValue = this.MiFactura.ClienteIdCondicionFiscal.ToString();
            else if (this.MiFactura.Afiliado.CondicionFiscal.IdCondicionFiscal > 0)
                this.ddlCondicionFiscalCliente.SelectedValue = this.MiFactura.Afiliado.CondicionFiscal.IdCondicionFiscal.ToString();
            else
            {
                this.ddlCondicionFiscalCliente.Items.Clear();
                this.ddlCondicionFiscalCliente.SelectedIndex = -1;
                this.ddlCondicionFiscalCliente.SelectedValue = null;
                this.ddlCondicionFiscalCliente.ClearSelection();
                this.ddlCondicionFiscalCliente.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.CondicionesFiscales);
                this.ddlCondicionFiscalCliente.DataValueField = "IdListaValorSistemaDetalle";
                this.ddlCondicionFiscalCliente.DataTextField = "Descripcion";
                this.ddlCondicionFiscalCliente.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlCondicionFiscalCliente, this.ObtenerMensajeSistema("Seleccione una opción"));
            }
        }
        protected void btnCalcularPorcentajeTurismo_Click(object sender, EventArgs e)
        {
            if (this.txtPorcentajeTurismo.Decimal > 100)
            {
                this.txtPorcentajeTurismo.Decimal = 100;
                this.MostrarMensaje(this.ObtenerMensajeSistema("ValidarPorcentaje"), true);
                return;
            }
            this.MiFactura.PorcentajeTurismo = this.txtPorcentajeTurismo.Decimal;
            this.MiFactura.FacturasDetalles = FacturasF.FacturasDetallesObtenerDetallesServiciosPendientesPorRefTabla(this.MiFactura);
            this.MiFactura.FacturasTiposPercepciones = FacturasF.FacturasPercepcionesObtenerPorIdRefTabla(this.MiFactura);
            AyudaProgramacion.CargarGrillaListas<VTAFacturasDetalles>(this.MiFactura.FacturasDetalles, false, this.gvItems, true);

            if (this.MiFactura.FacturasTiposPercepciones.Count > 0)
            {
                AyudaProgramacion.CargarGrillaListas<VTAFacturasTiposPercepciones>(this.MiFactura.FacturasTiposPercepciones, false, this.gvPercepciones, true);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CalcularPercepcion", "CalcularPercepcion();", true);
            }
            this.hdfNoCalculaImporteDescuento.Value = "1";
            this.CalcularTotal();

            StringBuilder script = new StringBuilder();
            script.AppendLine("$(document).ready(function () {");
            script.AppendLine("CalcularItem();");
            script.AppendLine("});");
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CalcularItemScript", script.ToString(), true);
        }
        #endregion
    }
}