using Afiliados;
using Afiliados.Entidades;
using Cargos;
using Cargos.Entidades;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using CuentasPagar.Entidades;
using CuentasPagar.FachadaNegocio;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Prestamos;
using Prestamos.Entidades;
using Reportes.FachadaNegocio;
using Seguridad.FachadaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tesorerias.Entidades;

namespace IU.Modulos.Prestamos.Controles
{
    public partial class PrestamoAfiliadoModificarDatos : ControlesSeguros
    {
        private bool _volverSiguiente = false;
        //private bool _prestamoIPS = false;
        private bool _prestamoIPS
        {
            get { return Session[this.MiSessionPagina + "Prestamo_prestamoIPS"] == null ? false : (bool)Session[this.MiSessionPagina + "Prestamo_prestamoIPS"]; }
            set { Session[this.MiSessionPagina + "Prestamo_prestamoIPS"] = value; }
        }

        public enum TipoAutorizar
        {
            SinPrivilegio = 0,
            PreAutorizar = 1,
            Autorizar = 2,
        }

        public PrePrestamos MiPrePrestamos
        {
            get { return (PrePrestamos)Session[this.MiSessionPagina + "PrestamoMiPrePrestamos"]; }
            set { Session[this.MiSessionPagina + "PrestamoMiPrePrestamos"] = value; }
        }

        private List<CarCuentasCorrientes> MisCargosPendientes
        {
            get { return (List<CarCuentasCorrientes>)Session[this.MiSessionPagina + "PrestamoMisCargosPendientes"]; }
            set { Session[this.MiSessionPagina + "PrestamoMisCargosPendientes"] = value; }
        }

        private List<PrePrestamos> MisPrestamosCancelacionesPendientes
        {
            get { return (List<PrePrestamos>)Session[this.MiSessionPagina + "PrestamoMisPrestamosCancelacionesPendientes"]; }
            set { Session[this.MiSessionPagina + "PrestamoMisPrestamosCancelacionesPendientes"] = value; }
        }

        private List<CapSolicitudPago> MisSolicitudesPendientes
        {
            get { return (List<CapSolicitudPago>)Session[this.MiSessionPagina + "PrestamoMisSolicitudesPendientes"]; }
            set { Session[this.MiSessionPagina + "PrestamoMisSolicitudesPendientes"] = value; }
        }

        private List<TGEFormasCobrosAfiliados> MisFormasCobros
        {
            get { return (List<TGEFormasCobrosAfiliados>)Session[this.MiSessionPagina + "PrestamoAfiliadoModificarDatosMisFormasCobros"]; }
            set { Session[this.MiSessionPagina + "PrestamoAfiliadoModificarDatosMisFormasCobros"] = value; }
        }

        private List<PrePrestamosPlanes> MisPlanes
        {
            get { return (List<PrePrestamosPlanes>)Session[this.MiSessionPagina + "PrestamoAfiliadoModificarDatosMisPlanes"]; }
            set { Session[this.MiSessionPagina + "PrestamoAfiliadoModificarDatosMisPlanes"] = value; }
        }

        private List<TGEMonedas> MisMonedas
        {
            get { return (List<TGEMonedas>)Session[this.MiSessionPagina + "PrestamoAfiliadoModificarDatosMisMonedas"]; }
            set { Session[this.MiSessionPagina + "PrestamoAfiliadoModificarDatosMisMonedas"] = value; }
        }

        private TipoAutorizar MiTipoAutorizar
        {
            get { return (TipoAutorizar)Session[this.MiSessionPagina + "PrestamoMiTipoAutorizar"]; }
            set { Session[this.MiSessionPagina + "PrestamoMiTipoAutorizar"] = value; }
        }

        private int MiIndiceGvCancelaciones
        {
            get { return (int)Session[this.MiSessionPagina + "PrestamoMiIndiceGvCancelaciones"]; }
            set { Session[this.MiSessionPagina + "PrestamoMiIndiceGvCancelaciones"] = value; }
        }

        public delegate void PrestamosAfiliadosDatosAceptarEventHandler(object sender, PrePrestamos e);
        public event PrestamosAfiliadosDatosAceptarEventHandler PrestamosAfiliadosModificarDatosAceptar;

        public delegate void PrestamosAfiliadosDatosCancelarEventHandler();
        public event PrestamosAfiliadosDatosCancelarEventHandler PrestamosAfiliadosModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            this.ctrPrestamosCuotasPopUp.PrestamosCuotasSeleccionar += new PrestamosCuotasPopUp.PrestamosCuotasEventHandler(ctrPrestamosCuotasPopUp_PrestamosCuotasSeleccionar);
            if (!this.IsPostBack)
            {
                //ver de setear el TAB 
                if (this.MisParametrosUrl.ContainsKey("tcDatosTabIndex"))
                {
                    int tabIndex = 0;
                    if (int.TryParse(this.MisParametrosUrl["tcDatosTabIndex"].ToString(), out tabIndex))
                    {
                        if (this.tcDatos.Visible == true && this.tcDatos.Tabs.Count > tabIndex)
                            this.tcDatos.ActiveTabIndex = tabIndex;
                    }
                }
            }
            else
            {
                if (this.MiPrePrestamos == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosListar.aspx"), true);
                }
                if (this.GestionControl == Gestion.Modificar
                    && (this.MiPrePrestamos.Estado.IdEstado == (int)EstadosPrestamos.Activo
                        || this.MiPrePrestamos.Estado.IdEstado == (int)EstadosPrestamos.PreAutorizado)
                    )
                {
                    this.txtMonto.TextChanged += new EventHandler(txtMonto_TextChanged);
                    this.txtMontoAutorizado.TextChanged += new EventHandler(txtMontoAutorizado_TextChanged);
                    this.txtCantidadCuotas.TextChanged += new EventHandler(txtCantidadCuotas_TextChanged);
                }
            }
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Solicitud Material
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(PrePrestamos pPrestamos, Gestion pGestion, TipoAutorizar pTipoAutorizacion, EnumTGETiposOperaciones tiposOperaciones)
        {
            this.MisFormasCobros = null;
            this.MisPlanes = new List<PrePrestamosPlanes>();
            this.GestionControl = pGestion;
            this.MiPrePrestamos = pPrestamos;
            this.MiPrePrestamos.TipoOperacion.IdTipoOperacion = (int)tiposOperaciones;
            this.MiTipoAutorizar = pTipoAutorizacion;
            dvImporteCuota.Visible = true;
            dvTasaIntereses.Visible = false;
            dvImporteGastos.Visible = false;
            dvMoneda.Visible = false;
            dvTipoValor.Visible = false;
            dvImporteExcedido.Visible = false;
            dvImporteSolicitudesPagos.Visible = false;

            if (!this._volverSiguiente)
                this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.btnCancelar.Visible = false;
                    this.btnVolver.Visible = true;
                    this.btnAceptar.Text = this.ObtenerMensajeSistema("btnSiguiente");
                    this.txtFechaAlta.Text = DateTime.Today.ToShortDateString();
                    this.txtFechaAlta.Enabled = true;
                    this.ddlPeriodoVencimiento.Enabled = true;
                    this.txtMonto.Text = (0).ToString("C2");
                    this.txtImportePrestamo.Text = (0).ToString("C2");
                    this.txtImporteCancelaciones.Enabled = true;
                    this.ddlEstado.Enabled = false;
                    this.txtImporteCuota.Enabled = true;
                    this.txtImporteCuota.Text = (0).ToString("C2");
                    //this.ddlTipoOperacion.Enabled = true;
                    ListItem item = ddlTipoOperacion.Items.FindByValue(((int)tiposOperaciones).ToString());
                    if (item != null)
                        this.ddlTipoOperacion.SelectedValue = item.Value;

                    item = ddlTipoValor.Items.FindByValue(((int)EnumTiposValores.Efectivo).ToString());
                    if (item == null)
                    {
                        this.MostrarMensaje("No esta configurada la opcion de Pago Tipo de Valor Efectivo.", true);
                        this.btnAceptar.Visible = false;
                    }
                    else
                        ddlTipoValor.SelectedValue = item.Value;

                    //this.ddlFormasCobros.Enabled = false;
                    if (this.ddlFormasCobros.Items.Count == 0)
                    {
                        this.MostrarMensaje("ValidarFormasCobro", true);
                        this.btnAceptar.Visible = false;
                    }
                    if (this.MiPrePrestamos.PrestamosCuotas.Count > 0)
                    {
                        this.MiPrePrestamos.PrestamosCuotas.Clear();
                        this.MapearObjetoAControles(this.MiPrePrestamos);

                        //this.DeshabilitarControles(false);
                        this.txtMonto.Enabled = true;
                        if (this.MiPrePrestamos.PrestamoPlan.PrestamoPlanTasa.ImporteDesde.HasValue && this.MiPrePrestamos.PrestamoPlan.PrestamoPlanTasa.ImporteHasta.HasValue && this.MiPrePrestamos.PrestamoPlan.PrestamoPlanTasa.ImporteDesde.Value == this.MiPrePrestamos.PrestamoPlan.PrestamoPlanTasa.ImporteHasta.Value)
                            this.txtMonto.Enabled = false;

                        //this.ddlTipoValor.Enabled = true;
                        this.ddlFilialPago.Enabled = true;
                    }

                    //if ((this._prestamoIPS || this.Request.UrlReferrer.AbsolutePath.Contains("PrePrestamosIpsCadAutorizacionesListar"))
                    //    && this.MisParametrosUrl.Contains("IdPrestamoIpsCadAutorizacion"))
                    //{
                    //    this._prestamoIPS = true;
                    //    //ListItem item = ddlTipoOperacion.Items.FindByValue(((int)tiposOperaciones).ToString());
                    //    //if (item != null)
                    //    //    this.ddlTipoOperacion.SelectedValue = item.Value;

                    //    this.MiPrePrestamos.IdPrestamoIpsCadAutorizacion = Convert.ToInt32(this.MisParametrosUrl["IdPrestamoIpsCadAutorizacion"]);
                    //    this.MiPrePrestamos.IpsCADNumero = Convert.ToInt64(this.MisParametrosUrl["IpsCADNumero"]);
                    //    this.MiPrePrestamos.IpsCADSelloTiempo = Convert.FromBase64String(this.MisParametrosUrl["IpsCADSelloTiempo"].ToString());
                    //}
                    //else
                    //{
                    //    this.MiPrePrestamos.IdPrestamoIpsCadAutorizacion = 0;
                    //    this.MiPrePrestamos.IpsCADNumero = 0;
                    //    this.MiPrePrestamos.IpsCADSelloTiempo = null;
                    //}
                    this._volverSiguiente = false;

                    this.ctrCamposValores.IniciarControl(this.MiPrePrestamos, new Objeto(), this.GestionControl);
                    this.ctrComentarios.IniciarControl(this.MiPrePrestamos, this.GestionControl);
                    this.ctrArchivos.IniciarControl(this.MiPrePrestamos, this.GestionControl);

                    this.ddlTipoOperacion_SelectedIndexChanged(null, EventArgs.Empty);

                    break;
                default:
                    break;
            }

        }

        public void IniciarControlVendedor(PrePrestamos pPrestamos, Gestion pGestion, TipoAutorizar pTipoAutorizacion)
        {
            IniciarControl(pPrestamos, pGestion, pTipoAutorizacion);
            switch (GestionControl)
            {
                case Gestion.Agregar:
                    MiPrePrestamos = PrePrestamosF.PrestamosObtenerDatosPreCargados(MiPrePrestamos);
                    MiPrePrestamos.Estado.IdEstado = (int)EstadosPrestamos.Pendiente;
                    MapearObjetoAControles(MiPrePrestamos);
                    ddlTipoOperacion_SelectedIndexChanged(ddlTipoOperacion, EventArgs.Empty);
                    break;
                default:
                    break;
            }
        }

        public void IniciarControl(PrePrestamos pPrestamos, Gestion pGestion, TipoAutorizar pTipoAutorizacion)
        {
            this.MisFormasCobros = null;
            this.MisPlanes = new List<PrePrestamosPlanes>();
            this.GestionControl = pGestion;
            this.MiPrePrestamos = pPrestamos;
            this.MiPrePrestamos.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            this.MiTipoAutorizar = pTipoAutorizacion;
            //this.ddlFilialPago.SelectedValue == string.Empty? 0 :Convert.ToInt32(this.ddlFilialPago.SelectedValue);
            if (!this._volverSiguiente)
                this.CargarCombos();

            this.ddlFilialPago.SelectedValue = UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
            bool validarFirmarDocumento = false;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    /*Agregar Prestamos desde Vendedores*/
                    //  MiPrePrestamos = PrePrestamosF.PrestamosObtenerDatosPreCargados(this.MiPrePrestamos);
                    MiPrePrestamos.Estado.IdEstado = (int)EstadosPrestamos.Activo;
                    this.btnCancelar.Visible = false;
                    this.btnVolver.Visible = true;
                    this.btnAceptar.Text = this.ObtenerMensajeSistema("btnSiguiente");
                    this.txtFechaAlta.Text = DateTime.Today.ToShortDateString();
                    this.txtFechaAlta.Enabled = true;
                    this.ddlPeriodoVencimiento.Enabled = true;
                    this.txtMonto.Text = (0).ToString("C2");
                    ddlMoneda.Enabled = true;
                    //this.txtImporteExedido.Text = pPrestamos.Afiliado.ImporteExcedido.ToString("C2");
                    //this.MiPrePrestamos.ImporteExcedido = pPrestamos.Afiliado.ImporteExcedido;
                    //this.MiPrePrestamos.Cancelaciones = PrePrestamosF.PrestamosObtenerPorAfiliadoCancelacion(this.MiPrePrestamos.Afiliado);
                    //AyudaProgramacion.CargarGrillaListas<PrePrestamos>(this.MiPrePrestamos.Cancelaciones, false, this.gvCancelaciones, true);
                    //if (this.MiPrePrestamos.Cancelaciones.Count > 0)
                    //{
                    //    this.txtImporteCancelaciones.Text = this.MiPrePrestamos.ImporteCancelaciones.ToString("N2");
                    //    this.tcDatos.ActiveTab = this.tpCancelaciones;
                    //}
                    if (this.ddlFormasCobros.Items.Count == 0)
                    {
                        this.MostrarMensaje("ValidarFormasCobro", true);
                        this.btnAceptar.Visible = false;
                    }
                    if (this.MiPrePrestamos.PrestamosCuotas.Count > 0)
                    {
                        this.MiPrePrestamos.PrestamosCuotas.Clear();
                        SetInitializeCulture(MiPrePrestamos.Moneda.Moneda);
                        //this.MiPrePrestamos.ImporteExcedido = this.MisCargosPendientes.Where(x => x.Incluir).Sum(x => x.Importe);
                        //this.MiPrePrestamos.ImporteSolicitudesPagos = this.MisSolicitudesPendientes.Where(x => x.IncluirEnOP).Sum(x => x.ImporteTotal);

                        this.DeshabilitarControles(false);
                        this.txtMonto.Enabled = true;
                        if (this.MiPrePrestamos.PrestamoPlan.PrestamoPlanTasa.ImporteDesde.HasValue && this.MiPrePrestamos.PrestamoPlan.PrestamoPlanTasa.ImporteHasta.HasValue && this.MiPrePrestamos.PrestamoPlan.PrestamoPlanTasa.ImporteDesde.Value == this.MiPrePrestamos.PrestamoPlan.PrestamoPlanTasa.ImporteHasta.Value)
                            this.txtMonto.Enabled = false;

                        //this.ddlTipoValor.Enabled = true;
                        //this.ddlFilialPago.Enabled = true;
                    }
                    this.ddlEstado.Enabled = false;
                    this.ddlTipoOperacion.Enabled = true;
                    this.ddlTiposCargos.Enabled = true;
                    this.ddlFormasCobros.Enabled = true;
                    this.ddlPlan.Enabled = true;
                    this.ddlFilialPago.Enabled = true;
                    this.ddlTipoValor.Enabled = true;
                    if ((this._prestamoIPS || this.Request.UrlReferrer.AbsolutePath.Contains("PrePrestamosIpsCadAutorizacionesListar"))
                        && this.MisParametrosUrl.Contains("IdPrestamoIpsCadAutorizacion"))
                    {
                        this._prestamoIPS = true;
                        ListItem item = ddlTipoOperacion.Items.FindByValue(((int)EnumTGETiposOperaciones.PrestamosLargoPlazo).ToString());
                        if (item != null)
                            this.ddlTipoOperacion.SelectedValue = item.Value;

                        this.MiPrePrestamos.IdPrestamoIpsCadAutorizacion = Convert.ToInt32(this.MisParametrosUrl["IdPrestamoIpsCadAutorizacion"]);
                        this.MiPrePrestamos.IpsCADNumero = Convert.ToInt64(this.MisParametrosUrl["IpsCADNumero"]);
                        this.MiPrePrestamos.IpsCADSelloTiempo = Convert.FromBase64String(this.MisParametrosUrl["IpsCADSelloTiempo"].ToString());
                    }
                    else
                    {
                        this.MiPrePrestamos.IdPrestamoIpsCadAutorizacion = 0;
                        this.MiPrePrestamos.IpsCADNumero = 0;
                        this.MiPrePrestamos.IpsCADSelloTiempo = null;
                    }

                    this.ctrCamposValores.IniciarControl(this.MiPrePrestamos, new Objeto(), this.GestionControl);
                    this.ctrComentarios.IniciarControl(this.MiPrePrestamos, this.GestionControl);
                    this.ctrArchivos.IniciarControl(this.MiPrePrestamos, this.GestionControl);
                    //this.ctrPrestamosCheques.IniciarControl(this.MiPrePrestamos, this.GestionControl);


                    if (this._volverSiguiente)
                    {
                        this.MapearObjetoAControles(this.MiPrePrestamos);
                        //if (MiPrePrestamos.Cancelaciones.Count > 0)
                        //{
                        //    AyudaProgramacion.CargarGrillaListas<PrePrestamos>(MiPrePrestamos.Cancelaciones, false, gvCancelaciones, true);
                        //}
                    }

                    this.ddlTipoOperacion_SelectedIndexChanged(null, EventArgs.Empty);//ESTO ESTABA ARRIBA DEL IF(VOLVERSIGUIENTE)


                    break;
                case Gestion.ConfirmarAgregar:
                    this.btnCancelar.Visible = false;
                    this.btnVolver.Visible = true;
                    //this.MiPrePrestamos.Estado.IdEstado = (int)EstadosPrestamos.Activo;
                    //PrePrestamosF.PrestamosAgregarPrevio(this.MiPrePrestamos);
                    this.MapearObjetoAControles(this.MiPrePrestamos);
                    SetInitializeCulture(MiPrePrestamos.Moneda.Moneda);
                    this.btnAceptar.Text = "Aceptar";// this.ObtenerMensajeSistema("btnConfirmar");
                    this.DeshabilitarControles(true);
                    this.txtMonto.Enabled = false;
                    this.ddlTipoValor.Enabled = false;
                    this.ddlFilialPago.Enabled = false;
                    this.ddlPeriodoVencimiento.Enabled = false;
                    this.txtImporteCuota.Enabled = false;
                    this.txtImporteCancelaciones.Enabled = false;
                    if (MiPrePrestamos.ConfirmaMensajes.Exists(x => x.TipoMensaje == TipoError.Alerta))
                    {
                        ConfirmarMensajes msg = MiPrePrestamos.ConfirmaMensajes.FirstOrDefault(x => x.TipoMensaje == TipoError.Alerta);
                        MostrarMensaje(msg.CodigoMensaje, true, new List<string>() { msg.Mensaje });
                    }
                    break;
                case Gestion.Anular:
                    this.MiPrePrestamos = PrePrestamosF.PrestamosObtenerDatosCompletos(pPrestamos);
                    this.MiPrePrestamos.Estado.IdEstado = (int)EstadosPrestamos.Anulado;
                    this.MapearObjetoAControles(this.MiPrePrestamos);
                    SetInitializeCulture(MiPrePrestamos.Moneda.Moneda);
                    this.DeshabilitarControles(true);
                    this.txtMonto.Enabled = false;
                    this.ddlTipoValor.Enabled = false;
                    this.ddlFilialPago.Enabled = false;
                    this.btnImprimir.Visible = true;
                    break;
                case Gestion.AnularConfirmar:
                    this.MiPrePrestamos = PrePrestamosF.PrestamosObtenerDatosCompletos(pPrestamos);
                    this.MiPrePrestamos.Estado.IdEstado = (int)EstadosPrestamos.AnuladoPostConfirmado;
                    this.MapearObjetoAControles(this.MiPrePrestamos);
                    SetInitializeCulture(MiPrePrestamos.Moneda.Moneda);
                    this.DeshabilitarControles(true);
                    this.txtMonto.Enabled = false;
                    this.ddlTipoValor.Enabled = false;
                    this.ddlFilialPago.Enabled = false;
                    string funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarAnularPrestamo"));
                    this.btnAceptar.Attributes.Add("OnClick", funcion);
                    this.btnImprimir.Visible = true;
                    break;
                case Gestion.Modificar:
                    validarFirmarDocumento = true;
                    this.MiPrePrestamos = PrePrestamosF.PrestamosObtenerDatosCompletos(pPrestamos);
                    this.MapearObjetoAControles(this.MiPrePrestamos);
                    SetInitializeCulture(MiPrePrestamos.Moneda.Moneda);
                    this.btnImprimir.Visible = true;
                    switch (this.MiPrePrestamos.Estado.IdEstado)
                    {
                        case (int)EstadosPrestamos.Activo:
                            this.HabilitarMontoAutorizado(true);
                            this.MostrarOcultarAutorizado(false);
                            this.HabilitarControlesModificacion(true);
                            if (MiPrePrestamos.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.CompraDeCheque)
                            { this.txtMonto.Enabled = false; }
                            else
                            {
                                this.txtMonto.AutoPostBack = true;
                                this.txtMonto.TextChanged += new EventHandler(txtMonto_TextChanged);
                                this.txtCantidadCuotas.AutoPostBack = true;
                                this.txtCantidadCuotas.TextChanged += new EventHandler(txtCantidadCuotas_TextChanged);
                            }
                            CargarCancelaciones();
                            break;
                        case (int)EstadosPrestamos.Pendiente:
                            this.HabilitarMontoAutorizado(true);
                            this.MostrarOcultarAutorizado(false);
                            this.HabilitarControlesModificacion(true);
                            if (MiPrePrestamos.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.CompraDeCheque)
                            { this.txtMonto.Enabled = false; }
                            else
                            {
                                this.txtMonto.AutoPostBack = true;
                                this.txtMonto.TextChanged += new EventHandler(txtMonto_TextChanged);
                                this.txtCantidadCuotas.AutoPostBack = true;
                                this.txtCantidadCuotas.TextChanged += new EventHandler(txtCantidadCuotas_TextChanged);
                            }
                            CargarCancelaciones();
                            break;
                        case (int)EstadosPrestamos.PreAutorizado:
                            if (pTipoAutorizacion == TipoAutorizar.PreAutorizar
                                || pTipoAutorizacion == TipoAutorizar.Autorizar)
                            {
                                this.HabilitarMontoAutorizado(true);
                                this.MostrarOcultarAutorizado(true);
                                this.txtMonto.Enabled = false;
                            }
                            this.HabilitarControlesModificacion(true);
                            this.txtMontoAutorizado.AutoPostBack = true;
                            this.txtMontoAutorizado.TextChanged += new EventHandler(txtMontoAutorizado_TextChanged);
                            CargarCancelaciones();
                            break;
                        case (int)EstadosPrestamos.Autorizado:
                            this.ddlFormasCobros.Enabled = true;
                            this.ddlTipoValor.Enabled = true;
                            this.ddlFilialPago.Enabled = true;
                            this.ddlPeriodoVencimiento.Enabled = true;
                            this.ddlPlan.Enabled = false;
                            this.txtCantidadCuotas.Enabled = false;
                            this.txtMonto.Enabled = false;
                            break;
                        case (int)EstadosPrestamos.Confirmado:
                            this.HabilitarControlesModificacion(false);
                            this.DeshabilitarControles(true);
                            this.ddlFormasCobros.Enabled = true;
                            this.ddlPeriodoVencimiento.Enabled = !this.MiPrePrestamos.PrestamosCuotas.Exists(pc => pc.IdCuentaCorriente.HasValue);
                            break;
                        default:
                            this.DeshabilitarControles(true);
                            break;
                    }
                    break;
                case Gestion.Consultar:
                    validarFirmarDocumento = true;
                    this.MiPrePrestamos = PrePrestamosF.PrestamosObtenerDatosCompletos(pPrestamos);
                    this.MapearObjetoAControles(this.MiPrePrestamos);
                    SetInitializeCulture(MiPrePrestamos.Moneda.Moneda);
                    this.DeshabilitarControles(true);
                    this.btnAceptar.Visible = false;
                    this.txtMonto.Enabled = false;
                    this.ddlTipoValor.Enabled = false;
                    this.ddlFilialPago.Enabled = false;
                    this.btnImprimir.Visible = true;
                    break;
                case Gestion.AnularCancelar:
                    this.MiPrePrestamos = PrePrestamosF.PrestamosObtenerDatosCompletos(pPrestamos);
                    this.MapearObjetoAControles(this.MiPrePrestamos);
                    SetInitializeCulture(MiPrePrestamos.Moneda.Moneda);
                    this.DeshabilitarControles(true);
                    this.btnAceptar.Visible = true;
                    this.txtMonto.Enabled = false;
                    this.ddlTipoValor.Enabled = false;
                    this.ddlFilialPago.Enabled = false;
                    this.txtImporteCancelacion.Enabled = false;
                    this.pnlCancelacion.Visible = true;
                    this.btnImprimir.Visible = true;
                    break;
                case Gestion.Cancelar:
                    this.MiPrePrestamos = PrePrestamosF.PrestamosObtenerDatosCancelacion(pPrestamos);
                    //this.MiPrePrestamos.PrestamosCuotas.ForEach(x=>x.Incluir=true);
                    this.MapearObjetoAControles(this.MiPrePrestamos);
                    SetInitializeCulture(MiPrePrestamos.Moneda.Moneda);
                    this.DeshabilitarControles(true);
                    this.txtImporteCancelacion.Enabled = true;
                    this.txtMonto.Enabled = false;
                    this.ddlTipoValor.Enabled = false;
                    this.ddlFilialPago.Enabled = false;
                    this.pnlCancelacion.Visible = true;
                    EnumTGETiposFuncionalidades tipoFunc = (EnumTGETiposFuncionalidades)this.paginaSegura.paginaActual.IdTipoFuncionalidad;
                    this.ddlTipoValorCancelacion.DataSource = TGEGeneralesF.TiposValoresObtenerLista(tipoFunc);
                    this.ddlTipoValorCancelacion.DataValueField = "IdTipoValor";
                    this.ddlTipoValorCancelacion.DataTextField = "TipoValor";
                    this.ddlTipoValorCancelacion.DataBind();
                    this.ddlTipoValorCancelacion.SelectedValue = ((int)EnumTiposValores.Efectivo).ToString();
                    this.ddlFilialCancelacion.DataSource = TGEGeneralesF.FilialesObenerLista();
                    this.ddlFilialCancelacion.DataValueField = "IdFilial";
                    this.ddlFilialCancelacion.DataTextField = "Filial";
                    this.ddlFilialCancelacion.DataBind();
                    this.ddlFilialCancelacion.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
                    int index = AyudaProgramacion.GridviewIndexColumn(gvDatos, "Incluir");
                    if (index >= 0)
                        this.gvDatos.Columns[index].Visible = true;
                    this.btnImprimir.Visible = true;
                    break;
                case Gestion.Aplicar:
                    this.MiPrePrestamos = PrePrestamosF.PrestamosObtenerDatosCompletos(pPrestamos);
                    //this.MiPrePrestamos.PrestamosCuotas.ForEach(x=>x.Incluir=true);
                    this.MapearObjetoAControles(this.MiPrePrestamos);
                    SetInitializeCulture(MiPrePrestamos.Moneda.Moneda);
                    this.DeshabilitarControles(true);
                    this.txtMonto.Enabled = false;
                    this.ddlTipoValor.Enabled = false;
                    this.ddlFilialPago.Enabled = false;
                    this.btnImprimir.Visible = true;
                    break;
                default:
                    break;
            }

            switch (this.MiPrePrestamos.Estado.IdEstado)
            {
                case (int)EstadosPrestamos.Anulado:
                case (int)EstadosPrestamos.Finalizado:
                case (int)EstadosPrestamos.Autorizado:
                case (int)EstadosPrestamos.Confirmado:
                    this.MostrarOcultarAutorizado(true);
                    this.MostrarOcultarUsuarioPreAutorizo(true);
                    this.MostrarOcultarUsuarioAutorizo(true);
                    break;
                case (int)EstadosPrestamos.PreAutorizado:
                    this.MostrarOcultarAutorizado(true);
                    this.MostrarOcultarUsuarioPreAutorizo(true);
                    break;
                case (int)EstadosPrestamos.Cancelado:
                case (int)EstadosPrestamos.PendienteCancelacion:
                    this.MostrarOcultarAutorizado(true);
                    this.MostrarOcultarUsuarioPreAutorizo(true);
                    this.MostrarOcultarUsuarioAutorizo(true);
                    this.pnlCancelacion.Visible = true;
                    this.ddlFilialCancelacion.Enabled = false;
                    break;
                default:
                    break;
            }

            if (this.MiPrePrestamos.ImporteExcedido > 0)
            {
                AyudaProgramacion.FormatoResaltado(this.txtImporteExedido);
            }
            if (validarFirmarDocumento &&
                !(this.MiPrePrestamos.Estado.IdEstado == (int)EstadosPrestamos.Anulado ||
                this.MiPrePrestamos.Estado.IdEstado == (int)EstadosPrestamos.Finalizado ||
                this.MiPrePrestamos.Estado.IdEstado == (int)EstadosPrestamos.Cancelado)
                )
            {
                TGEFirmarDocumentos firmarDoc = new TGEFirmarDocumentos();
                firmarDoc.IdRefTabla = this.MiPrePrestamos.IdPrestamo;
                firmarDoc.Tabla = "PrePrestamos";
                firmarDoc.IdTipoOperacion = this.MiPrePrestamos.TipoOperacion.IdTipoOperacion;
                this.btnFirmarDocumento.Visible = TGEGeneralesF.FirmarDocumentosValidar(firmarDoc);
                this.btnWhatsAppFirmarDocumento.Visible = this.btnFirmarDocumento.Visible;
                this.copyClipboard.Visible = this.btnFirmarDocumento.Visible;
                if (this.btnFirmarDocumento.Visible)
                {
                    firmarDoc = new TGEFirmarDocumentos();
                    firmarDoc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    firmarDoc.IdRefTabla = this.MiPrePrestamos.IdPrestamo;
                    firmarDoc.Tabla = "PrePrestamos";
                    firmarDoc.IdTipoOperacion = this.MiPrePrestamos.TipoOperacion.IdTipoOperacion;
                    PropertyInfo prop = MiPrePrestamos.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() == 1).ToList()[0];
                    firmarDoc.Key = prop.Name;
                    firmarDoc.CodigoPlantilla = "PrestamosSolicitudOtorgamiento";
                    hfLinkFirmarDocumento.Value = TGEGeneralesF.FirmarDocumentosArmarLinkFirmarDocumento(firmarDoc).Link;
                    this.MiPrePrestamos.LinkFirmarDocumento = hfLinkFirmarDocumento.Value;
                }
                this.btnFirmarDocumentoBaja.Visible = !this.btnFirmarDocumento.Visible;
                if (this.btnFirmarDocumentoBaja.Visible)
                {
                    string funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarFirmaDigitalManuscritaBaja"));
                    this.btnFirmarDocumentoBaja.Attributes.Add("OnClick", funcion);
                }
            }

        }

        private void HabilitarControlesModificacion(bool pEstado)
        {
            this.ddlFormasCobros.Enabled = pEstado;
            this.ddlPlan.Enabled = pEstado;
            this.txtCantidadCuotas.Enabled = pEstado;
            this.ddlTipoValor.Enabled = pEstado;
            this.ddlFilialPago.Enabled = pEstado;
            this.ddlPeriodoVencimiento.Enabled = pEstado;
        }

        private void DeshabilitarControles(bool pEstado)
        {
            this.txtFechaAlta.Enabled = !pEstado;
            this.ddlEstado.Enabled = !pEstado;
            // this.txtNumeroIdentificacion.Enabled = !pEstado;
            this.ddlPlan.Enabled = !pEstado;
            //this.txtTasaInteres.Enabled = !pEstado;
            this.txtCantidadCuotas.Enabled = !pEstado;
            this.ddlTipoOperacion.Enabled = !pEstado;
            this.ddlTiposCargos.Enabled = !pEstado;
            this.ddlFormasCobros.Enabled = !pEstado;
            this.ddlMoneda.Enabled = !pEstado;
            this.txtMonto.Enabled = !pEstado;
            if (!pEstado)
                if (this.MiPrePrestamos.PrestamoPlan.PrestamoPlanTasa.ImporteDesde.HasValue && this.MiPrePrestamos.PrestamoPlan.PrestamoPlanTasa.ImporteHasta.HasValue && this.MiPrePrestamos.PrestamoPlan.PrestamoPlanTasa.ImporteDesde.Value == this.MiPrePrestamos.PrestamoPlan.PrestamoPlanTasa.ImporteHasta.Value)
                    this.txtMonto.Enabled = false;
            //this.ddlTipoValor.Enabled = false;
        }

        private void MostrarOcultarUsuarioAutorizo(bool mostrar)
        {
            this.lblAutorizar.Visible = mostrar;
            this.txtAutorizo.Visible = mostrar;
        }

        private void MostrarOcultarUsuarioPreAutorizo(bool mostrar)
        {
            this.lblPreAutorizar.Visible = mostrar;
            this.txtPreAutorizar.Visible = mostrar;
        }

        private void MostrarOcultarAutorizado(bool mostrar)
        {
            this.pnlAutorizar.Visible = mostrar;
        }

        private void HabilitarMontoAutorizado(bool mostrar)
        {
            if (MiPrePrestamos.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.CompraDeCheque)
                mostrar = false;
            this.rfvMontoAutorizado.Enabled = mostrar;
            this.txtFechaValidezAutorizado.Enabled = mostrar;
            this.rfvFechaValidezAutorizado.Enabled = mostrar;
        }

        private void CargarCombos()
        {
            TGETiposOperaciones operaciones = new TGETiposOperaciones();
            operaciones.TipoFuncionalidad.IdTipoFuncionalidad = this.paginaSegura.paginaActual.IdTipoFuncionalidad;
            operaciones.Estado.IdEstado = (int)Estados.Activo;
            List<TGETiposOperaciones> listaop = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(operaciones);
            if (MiPrePrestamos.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosManual)
                listaop = listaop.Where(x => x.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosManual).ToList();
            else
                listaop = listaop.Where(x => x.IdTipoOperacion != (int)EnumTGETiposOperaciones.PrestamosManual).ToList();

            this.ddlTipoOperacion.DataSource = listaop;
            this.ddlTipoOperacion.DataValueField = "IdTipoOperacion";
            this.ddlTipoOperacion.DataTextField = "TipoOperacion";
            this.ddlTipoOperacion.DataBind();
            if (this.ddlTipoOperacion.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoOperacion, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            if (GestionControl == Gestion.Agregar)
                if (this.ddlTipoOperacion.Items.Count == 1)
                    this.ddlTipoOperacion_SelectedIndexChanged(null, EventArgs.Empty);

            AyudaProgramacion.AgregarItemSeleccione(this.ddlTiposCargos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFormasCobros, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            AyudaProgramacion.AgregarItemSeleccione(this.ddlPlan, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            MisMonedas = TGEGeneralesF.MonedasCotizacionesObtenerVentaCombo();
            this.ddlMoneda.DataSource = MisMonedas;
            this.ddlMoneda.DataValueField = "IdMoneda";
            this.ddlMoneda.DataTextField = "MonedaDescripcionCotizacion";
            this.ddlMoneda.DataBind();
            if (this.ddlMoneda.Items.Count != 1)
                AyudaProgramacion.InsertarItemSeleccione(this.ddlMoneda, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            EnumTGETiposFuncionalidades tipoFunc = (EnumTGETiposFuncionalidades)this.paginaSegura.paginaActual.IdTipoFuncionalidad;
            this.ddlTipoValor.DataSource = TGEGeneralesF.TiposValoresObtenerLista(tipoFunc);
            this.ddlTipoValor.DataValueField = "IdTipoValor";
            this.ddlTipoValor.DataTextField = "TipoValor";
            this.ddlTipoValor.DataBind();
            if (this.ddlTipoValor.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoValor, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosPrestamos));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.SelectedValue = ((int)EstadosPrestamos.Activo).ToString();

            this.ddlFilialPago.DataSource = TGEGeneralesF.FilialesPagosObtenerListaActiva();
            this.ddlFilialPago.DataValueField = "IdFilialPago";
            this.ddlFilialPago.DataTextField = "Filial";
            this.ddlFilialPago.DataBind();
            if (this.ddlFilialPago.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlFilialPago, this.ObtenerMensajeSistema("SeleccioneOpcion"));


            AyudaProgramacion.AgregarItemSeleccione(this.ddlPeriodoVencimiento, this.ObtenerMensajeSistema("SeleccioneOpcion"));



        }

        private void MapearControlesAObjeto(PrePrestamos pPrestamos)
        {
            pPrestamos.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pPrestamos.FechaPrestamo = Convert.ToDateTime(this.txtFechaAlta.Text);
            //pPrestamos.NroDeIdentificacion = Convert.ToInt32(this.txtNumeroIdentificacion.Text);
            //Falta la tasa
            pPrestamos.CantidadCuotas = this.txtCantidadCuotas.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCantidadCuotas.Text);
            pPrestamos.ImporteSolicitado = this.txtMonto.Decimal; //Convert.ToDecimal(this.txtMonto.Text);
            pPrestamos.ImporteAutorizado = this.txtMontoAutorizado.Decimal;//this.txtMontoAutorizado.Text==string.Empty? 0 : Convert.ToDecimal(this.txtMontoAutorizado.Text);
            pPrestamos.TipoOperacion.IdTipoOperacion = Convert.ToInt32(this.ddlTipoOperacion.SelectedValue);
            pPrestamos.TipoCargo.IdTipoCargo = Convert.ToInt32(this.ddlTiposCargos.SelectedValue);
            pPrestamos.TipoCargo.TipoCargo = this.ddlTiposCargos.SelectedItem.Text;
            pPrestamos.FormaCobroAfiliado.IdFormaCobroAfiliado = Convert.ToInt32(this.ddlFormasCobros.SelectedValue);
            pPrestamos.FormaCobroAfiliado.FormaCobro.FormaCobro = this.ddlFormasCobros.SelectedItem.Text;
            pPrestamos.FormaCobroAfiliado.FormaCobro.IdFormaCobro = this.MisFormasCobros.Find(x => x.IdFormaCobroAfiliado == Convert.ToInt32(this.ddlFormasCobros.SelectedValue)).FormaCobro.IdFormaCobro;
            pPrestamos.PrestamoPlan.IdPrestamoPlan = Convert.ToInt32(this.ddlPlan.SelectedValue);
            pPrestamos.Moneda.IdMoneda = Convert.ToInt32(this.ddlMoneda.SelectedValue);
            pPrestamos.TipoValor.IdTipoValor = this.ddlTipoValor.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoValor.SelectedValue);
            pPrestamos.FilialPago.IdFilialPago = this.ddlFilialPago.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFilialPago.SelectedValue);
            pPrestamos.FilialPago.Filial = this.ddlFilialPago.SelectedValue == string.Empty ? string.Empty : this.ddlFilialPago.SelectedItem.Text;
            pPrestamos.PeriodoPrimerVencimiento = this.ddlPeriodoVencimiento.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlPeriodoVencimiento.SelectedValue);
            pPrestamos.PorcentajeCapitalSocial = this.txtPorcentajeCapitalSocial.Decimal == 0 ? default(decimal?) : this.txtPorcentajeCapitalSocial.Decimal;
            pPrestamos.ImporteCapitalSocial = this.txtImporteCapitalSocial.Decimal == 0 ? default(decimal?) : this.txtImporteCapitalSocial.Decimal;
            pPrestamos.Comentarios = ctrComentarios.ObtenerLista();
            pPrestamos.Archivos = ctrArchivos.ObtenerLista();
            pPrestamos.Campos = this.ctrCamposValores.ObtenerLista();
            pPrestamos.LoteCamposValores = this.ctrCamposValores.ObtenerListaCamposValores();
            pPrestamos.ImporteCuota = this.txtImporteCuota.Decimal;
            //pPrestamos.Coeficiente = this.txtValorUnidad.Text == string.Empty ? 0 : Convert.ToDecimal(this.txtValorUnidad.Text);
            pPrestamos.TipoUnidad.IdTiposUnidades = this.hdfIdTipoUnidad.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdTipoUnidad.Value);
            TGEMonedas moneda = MisMonedas.FirstOrDefault(x => x.IdMoneda == Convert.ToInt32(this.ddlMoneda.SelectedValue));
            if (moneda.IdMoneda > 0)
            {
                pPrestamos.Moneda = moneda;
                pPrestamos.MonedaCotizacion = moneda.MonedeaCotizacion.MonedaCotizacion;
            }
        }

        private void MapearObjetoAControles(PrePrestamos pPrestamos)
        {
            this.ddlEstado.SelectedValue = pPrestamos.Estado.IdEstado.ToString();
            this.txtFechaAlta.Text = pPrestamos.FechaPrestamo.ToShortDateString();
            this.txtNumeroIdentificacion.Text = pPrestamos.NroDeIdentificacion.ToString();
            this.txtCantidadCuotas.Text = pPrestamos.CantidadCuotas.ToString();
            this.txtMonto.Text = pPrestamos.ImporteSolicitado.ToString("C2");
            this.txtMontoAutorizado.Text = pPrestamos.ImporteAutorizado.ToString("C2");
            this.txtImportePrestamo.Text = pPrestamos.ImportePrestamo.ToString("C2");
            this.txtImporteCuota.Text = pPrestamos.ImporteCuota.ToString("C2");

            this.txtFechaValidezAutorizado.Text = pPrestamos.UsuarioAutorizar.IdUsuarioAutorizar > 0 ? pPrestamos.FechaValidezAutorizado.ToShortDateString() : DateTime.Now.ToShortDateString();
            this.txtPreAutorizar.Text = pPrestamos.UsuarioPreAutorizar.ApellidoNombre;
            this.txtAutorizo.Text = pPrestamos.UsuarioAutorizar.ApellidoNombre;
            ListItem item = this.ddlTipoOperacion.Items.FindByValue(pPrestamos.TipoOperacion.IdTipoOperacion.ToString());
            if (item == null)
                this.ddlTipoOperacion.Items.Add(new ListItem(pPrestamos.TipoOperacion.TipoOperacion, pPrestamos.TipoOperacion.IdTipoOperacion.ToString()));
            this.ddlTipoOperacion.SelectedValue = pPrestamos.TipoOperacion.IdTipoOperacion.ToString();
            this.txtImporteGastos.Text = pPrestamos.ImporteGastos.ToString("C2");
            if (pPrestamos.PorcentajeCapitalSocial.HasValue && pPrestamos.PorcentajeCapitalSocial.Value > 0)
            {
                this.txtPorcentajeCapitalSocial.Text = pPrestamos.PorcentajeCapitalSocial.HasValue ? pPrestamos.PorcentajeCapitalSocial.Value.ToString("N4") : string.Empty;
                this.txtImporteCapitalSocial.Text = pPrestamos.ImporteCapitalSocial.HasValue ? pPrestamos.ImporteCapitalSocial.Value.ToString("C2") : string.Empty;
                this.phCapitalSocial.Visible = true;
            }
            item = this.ddlTiposCargos.Items.FindByValue(pPrestamos.TipoCargo.IdTipoCargo.ToString());
            if (item == null)
                this.ddlTiposCargos.Items.Add(new ListItem(pPrestamos.TipoCargo.TipoCargo, pPrestamos.TipoCargo.IdTipoCargo.ToString()));

            this.ddlTiposCargos.SelectedValue = pPrestamos.TipoCargo.IdTipoCargo.ToString();

            if (this.MisFormasCobros == null)
            {
                this.MisFormasCobros = new List<TGEFormasCobrosAfiliados>();
                TGEFormasCobrosAfiliados formasCobroAfi = new TGEFormasCobrosAfiliados();
                formasCobroAfi.IdAfiliado = this.MiPrePrestamos.Afiliado.IdAfiliado;
                formasCobroAfi.IdTipoCargo = Convert.ToInt32(this.ddlTiposCargos.SelectedValue);
                formasCobroAfi.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
                this.MisFormasCobros = TGEGeneralesF.FormasCobrosAfiliadosObtenerPorAfiliadoTipoCargo(formasCobroAfi);

                if (!this.MisFormasCobros.Exists(x => x.IdFormaCobroAfiliado == this.MiPrePrestamos.FormaCobroAfiliado.IdFormaCobroAfiliado)
                    && this.MiPrePrestamos.FormaCobroAfiliado.IdFormaCobroAfiliado > 0)
                {
                    this.MisFormasCobros.Add(this.MiPrePrestamos.FormaCobroAfiliado);
                    this.MisFormasCobros = AyudaProgramacion.AcomodarIndices<TGEFormasCobrosAfiliados>(this.MisFormasCobros);
                }

                this.ddlFormasCobros.Items.Clear();
                this.ddlFormasCobros.SelectedIndex = -1;
                this.ddlFormasCobros.SelectedValue = null;
                this.ddlFormasCobros.ClearSelection();

                this.ddlPlan.Items.Clear();
                this.ddlPlan.SelectedIndex = -1;
                this.ddlPlan.SelectedValue = null;
                this.ddlPlan.ClearSelection();

                if (this.MisFormasCobros.Count > 0)
                {
                    this.ddlFormasCobros.DataSource = this.MisFormasCobros;
                    this.ddlFormasCobros.DataValueField = "IdFormaCobroAfiliado";
                    this.ddlFormasCobros.DataTextField = "FormaCobroDescripcion";
                    this.ddlFormasCobros.DataBind();
                    if (this.MiPrePrestamos.FormaCobroAfiliado.IdFormaCobroAfiliado > 0)
                        this.ddlFormasCobros.SelectedValue = this.MiPrePrestamos.FormaCobroAfiliado.IdFormaCobroAfiliado.ToString();

                    PrePrestamosPlanes prestamoPlan = new PrePrestamosPlanes();
                    prestamoPlan.Estado.IdEstado = (int)Estados.Activo;
                    prestamoPlan.FormaCobro.IdFormaCobro = this.MisFormasCobros[this.ddlFormasCobros.SelectedIndex].FormaCobro.IdFormaCobro;
                    prestamoPlan.TipoOperacion.IdTipoOperacion = this.ddlTipoOperacion.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoOperacion.SelectedValue);
                    prestamoPlan.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
                    this.MisPlanes = PrePrestamosF.PrestamosPlanesObtenerLista(prestamoPlan);
                    this.ddlPlan.DataSource = this.MisPlanes;
                    this.ddlPlan.DataValueField = "IdPrestamoPlan";
                    this.ddlPlan.DataTextField = "Descripcion";
                    this.ddlPlan.DataBind();
                }
                else
                {
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlFormasCobros, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlPlan, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                }
            }
            item = this.ddlFormasCobros.Items.FindByValue(pPrestamos.FormaCobroAfiliado.IdFormaCobroAfiliado.ToString());
            if (item == null && pPrestamos.FormaCobroAfiliado.IdFormaCobroAfiliado > 0)
                this.ddlFormasCobros.Items.Add(new ListItem(pPrestamos.FormaCobroAfiliado.FormaCobroDescripcion, pPrestamos.FormaCobroAfiliado.IdFormaCobroAfiliado.ToString()));
            if (pPrestamos.FormaCobroAfiliado.IdFormaCobroAfiliado > 0)
                this.ddlFormasCobros.SelectedValue = pPrestamos.FormaCobroAfiliado.IdFormaCobroAfiliado.ToString();

            if (MisMonedas.Exists(x => x.IdMoneda == pPrestamos.Moneda.IdMoneda))
                MisMonedas.First(x => x.IdMoneda == pPrestamos.Moneda.IdMoneda).MonedeaCotizacion.MonedaCotizacion = pPrestamos.MonedaCotizacion;
            else if (pPrestamos.Moneda.IdMoneda > 0)
                MisMonedas.Add(pPrestamos.Moneda);
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
            this.ddlMoneda.SelectedValue = pPrestamos.Moneda.IdMoneda > 0 ? pPrestamos.Moneda.IdMoneda.ToString() : string.Empty;

            //Por si el Plan esta dado de baja.
            item = this.ddlPlan.Items.FindByValue(pPrestamos.PrestamoPlan.IdPrestamoPlan.ToString());
            if (item == null && pPrestamos.PrestamoPlan.IdPrestamoPlan > 0)
                this.ddlPlan.Items.Add(new ListItem(pPrestamos.PrestamoPlan.Descripcion, pPrestamos.PrestamoPlan.IdPrestamoPlan.ToString()));
            if (pPrestamos.PrestamoPlan.IdPrestamoPlan > 0)
                this.ddlPlan.SelectedValue = pPrestamos.PrestamoPlan.IdPrestamoPlan.ToString();

            this.txtTasaInteres.Text = pPrestamos.PrestamoPlan.PrestamoPlanTasa.Tasa.ToString();
            this.ddlMoneda.SelectedValue = pPrestamos.Moneda.IdMoneda.ToString();

            item = this.ddlTipoValor.Items.FindByValue(pPrestamos.TipoValor.IdTipoValor.ToString());
            if (item == null && pPrestamos.TipoValor.IdTipoValor > 0)
                this.ddlTipoValor.Items.Add(new ListItem(pPrestamos.TipoValor.TipoValor, pPrestamos.TipoValor.IdTipoValor.ToString()));
            this.ddlTipoValor.SelectedValue = pPrestamos.TipoValor.IdTipoValor > 0 ? pPrestamos.TipoValor.IdTipoValor.ToString() : string.Empty;

            this.ddlFilialPago.SelectedValue = pPrestamos.FilialPago.IdFilialPago.ToString();

            this.txtImporteCancelacion.Text = pPrestamos.ImporteCancelacion.ToString("C2");
            if (pPrestamos.FilialCancelacion.IdFilialCancelacion > 0)
            {
                item = this.ddlFilialCancelacion.Items.FindByValue(pPrestamos.FilialCancelacion.IdFilialCancelacion.ToString());
                if (item == null)
                    this.ddlFilialCancelacion.Items.Add(new ListItem(pPrestamos.FilialCancelacion.Filial, pPrestamos.FilialCancelacion.IdFilialCancelacion.ToString()));
                this.ddlFilialCancelacion.SelectedValue = pPrestamos.FilialCancelacion.IdFilialCancelacion.ToString();
            }
            if (pPrestamos.TipoValorCancelacion.IdTipoValorCancelacion > 0)
            {
                item = this.ddlTipoValorCancelacion.Items.FindByValue(pPrestamos.TipoValorCancelacion.IdTipoValorCancelacion.ToString());
                if (item == null)
                    this.ddlTipoValorCancelacion.Items.Add(new ListItem(pPrestamos.TipoValorCancelacion.TipoValor, pPrestamos.TipoValorCancelacion.IdTipoValorCancelacion.ToString()));
                this.ddlTipoValorCancelacion.SelectedValue = pPrestamos.TipoValorCancelacion.IdTipoValorCancelacion.ToString();
            }


            AyudaProgramacion.CargarGrillaListas<CarCuentasCorrientes>(this.MiPrePrestamos.CargosExcedidos, false, this.gvCuentaCorriente, true);
            this.tpCargosPendientes.Visible = this.MiPrePrestamos.CargosExcedidos.Count > 0;
            this.txtImporteExedido.Text = pPrestamos.ImporteCargosPendientes.ToString("C2");

            AyudaProgramacion.CargarGrillaListas<CapSolicitudPago>(this.MiPrePrestamos.SolicitudesPagos, false, this.gvSolicitudesPagos, true);
            this.tpSolicitudesPagos.Visible = this.MiPrePrestamos.SolicitudesPagos.Count > 0;
            this.txtImporteSolicitudesPagos.Text = pPrestamos.ImporteSolicitudesPagos.ToString("C2");

            AyudaProgramacion.CargarGrillaListas<PrePrestamos>(this.MiPrePrestamos.Cancelaciones, false, this.gvCancelaciones, true);
            this.tpCancelaciones.Visible = this.MiPrePrestamos.Cancelaciones.Count > 0;
            this.txtImporteCancelaciones.Text = this.MiPrePrestamos.ImporteCancelaciones.ToString("C2");

            if (pPrestamos.PeriodoPrimerVencimiento.HasValue)
            {
                item = this.ddlPeriodoVencimiento.Items.FindByValue(pPrestamos.PeriodoPrimerVencimiento.Value.ToString());
                if (item == null)
                    this.ddlPeriodoVencimiento.Items.Add(new ListItem(pPrestamos.PeriodoPrimerVencimiento.Value.ToString(), pPrestamos.PeriodoPrimerVencimiento.Value.ToString()));
            }
            this.ddlPeriodoVencimiento.SelectedValue = pPrestamos.PeriodoPrimerVencimiento.HasValue ? pPrestamos.PeriodoPrimerVencimiento.Value.ToString() : string.Empty;

            this.MostrarDetalleFormacobro();
            this.MostrarDetalleFormacobroPlan();

            this.ctrComentarios.IniciarControl(pPrestamos, this.GestionControl);
            this.ctrArchivos.IniciarControl(pPrestamos, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pPrestamos);

            if (MiPrePrestamos.PrestamoPlan.TipoUnidad.IdTiposUnidades > 0)
            {
                dvUnidades.Visible = true;
                txtTipoUnidad.Text = MiPrePrestamos.PrestamoPlan.TipoUnidad.Descripcion;
                hdfIdTipoUnidad.Value = MiPrePrestamos.PrestamoPlan.TipoUnidad.IdTiposUnidades.ToString();
                txtValorUnidad.Decimal = MiPrePrestamos.Coeficiente.HasValue ? MiPrePrestamos.Coeficiente.Value : 0;
                txtTotalUnidades.Decimal = MiPrePrestamos.UnidadesPrestamos.HasValue ? MiPrePrestamos.UnidadesPrestamos.Value : 0;
            }
            else
                dvUnidades.Visible = false;

            if (this.GestionControl == Gestion.ConfirmarAgregar)
            {
                this.ctrCamposValores.IniciarControl(pPrestamos.Campos, Gestion.Consultar, true);
            }
            else
            {
                Gestion gestion = this.GestionControl;// pPrestamos.Estado.IdEstado == (int)EstadosPrestamos.Autorizado ? Gestion.Consultar : this.GestionControl;
                this.ctrCamposValores.IniciarControl(pPrestamos, new Objeto(), gestion);
                if (pPrestamos.FormaCobroAfiliado.FormaCobro.IdFormaCobro > 0)
                    this.ctrCamposValores.IniciarControl(pPrestamos, pPrestamos.FormaCobroAfiliado.FormaCobro, gestion);
                if (pPrestamos.TipoOperacion.IdTipoOperacion > 0)
                    this.ctrCamposValores.IniciarControl(pPrestamos, pPrestamos.TipoOperacion, gestion);
                if (pPrestamos.PrestamoPlan.IdPrestamoPlan > 0)
                    this.ctrCamposValores.IniciarControl(pPrestamos, pPrestamos.PrestamoPlan, gestion);
                this.ctrCamposValores.IniciarControl(pPrestamos, pPrestamos.Estado, gestion);
                if (pPrestamos.TipoValor.IdTipoValor > 0)
                    this.ctrCamposValores.IniciarControl(pPrestamos, pPrestamos.TipoValor, gestion);
            }
            AyudaProgramacion.CargarGrillaListas<PrePrestamosCuotas>(pPrestamos.PrestamosCuotas.Where(x => x.Estado.IdEstado != (int)EstadosCuotas.Baja).ToList(), false, this.gvDatos, true);

            int index;
            if (pPrestamos.PrestamosCuotas.Exists(x => x.ImporteGastoCuota > 0))
            {
                index = AyudaProgramacion.GridviewIndexColumn(gvDatos, "Gasto");
                if (index >= 0)
                    this.gvDatos.Columns[index].Visible = true;
            }
            if (pPrestamos.PrestamosCuotas.Exists(x => x.ImporteSeguroCuota > 0))
            {
                index = AyudaProgramacion.GridviewIndexColumn(gvDatos, "Seguro");
                if (index >= 0)
                    this.gvDatos.Columns[index].Visible = true;
            }
            if (pPrestamos.PrestamosCuotas.Exists(x => x.ImporteIvaCuota > 0))
            {
                index = AyudaProgramacion.GridviewIndexColumn(gvDatos, "IVA");
                if (index >= 0)
                    this.gvDatos.Columns[index].Visible = true;
            }


            if (pPrestamos.TipoUnidad.IdTiposUnidades > 0)
            {
                index = AyudaProgramacion.GridviewIndexColumn(gvDatos, "Unidades");
                if (index >= 0)
                    this.gvDatos.Columns[index].Visible = true;
                index = AyudaProgramacion.GridviewIndexColumn(gvDatos, "U.Amortizacion");
                if (index >= 0)
                    this.gvDatos.Columns[index].Visible = true;
                index = AyudaProgramacion.GridviewIndexColumn(gvDatos, "U.Interes");
                if (index >= 0)
                    this.gvDatos.Columns[index].Visible = true;
                index = AyudaProgramacion.GridviewIndexColumn(gvDatos, "U.Saldo");
                if (index >= 0)
                    this.gvDatos.Columns[index].Visible = true;
                index = AyudaProgramacion.GridviewIndexColumn(gvDatos, "Valor Unidad");
                if (index >= 0)
                    this.gvDatos.Columns[index].Visible = true;
                index = AyudaProgramacion.GridviewIndexColumn(gvDatos, "Dif.Aumento");
                if (index >= 0)
                    this.gvDatos.Columns[index].Visible = true;
            }
            if (this.MiPrePrestamos.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.CompraDeCheque)
                this.ctrPrestamosCheques.IniciarControl(pPrestamos, this.GestionControl);

            DataTable dtDoc = PrePrestamosF.PrestamosObtenerDocumentosAsociados(this.MiPrePrestamos);
            if (dtDoc.Rows.Count > 0)
            {
                this.gvDocumentosAsociados.DataSource = dtDoc;
                this.gvDocumentosAsociados.DataBind();
                this.tpDocumentosAsociados.Visible = true;
            }
            else
                this.tpDocumentosAsociados.Visible = false;

            dtDoc = PrePrestamosF.PrestamosObtenerCobrosPorPrestamo(this.MiPrePrestamos);
            if (dtDoc.Rows.Count > 0)
            {
                this.gvDetalleCobros.DataSource = dtDoc;
                this.gvDetalleCobros.DataBind();
                this.tpDetalleCobros.Visible = true;
            }
            else
                this.tpDetalleCobros.Visible = false;
        }

        private void MostrarDetalleFormacobro()
        {
            List<TGECampos> lista = TGEGeneralesF.CamposObtenerListaFiltro(this.MiPrePrestamos.FormaCobroAfiliado, this.MiPrePrestamos.FormaCobroAfiliado.FormaCobro);
            lista = lista.Where(x => x.CampoValor.IdCampoValor > 0).ToList();
            this.gvDetalleFormaCobro.DataSource = AyudaProgramacion.PivotList(lista);
            this.gvDetalleFormaCobro.DataBind();
        }

        private void MostrarDetalleFormacobroPlan()
        {
            if (!(string.IsNullOrEmpty(this.ddlTiposCargos.SelectedValue) || string.IsNullOrEmpty(this.ddlFormasCobros.SelectedValue) || string.IsNullOrEmpty(this.ddlPlan.SelectedValue)))
            {
                TGEFormasCobrosCodigosConceptosTiposCargosCategorias formaCobroCodigoConcepto = new TGEFormasCobrosCodigosConceptosTiposCargosCategorias();
                formaCobroCodigoConcepto.IdPrestamoPlan = Convert.ToInt32(this.ddlPlan.SelectedValue);
                formaCobroCodigoConcepto.IdTipoCargo = Convert.ToInt32(this.ddlTiposCargos.SelectedValue);
                formaCobroCodigoConcepto.FormaCobro.IdFormaCobro = this.MisFormasCobros[this.ddlFormasCobros.SelectedIndex].FormaCobro.IdFormaCobro;
                DataTable dtFormaCobro = TGEGeneralesF.FormasCobrosCodigosConceptosTiposCargosCategoriasObtenerListaFiltro(formaCobroCodigoConcepto);
                this.gvFormasCobrosCodigosConceptos.DataSource = dtFormaCobro;
                this.gvFormasCobrosCodigosConceptos.DataBind();
            }
        }

        private bool RecalcularPrestamo()
        {
            this.MapearControlesAObjeto(this.MiPrePrestamos);
            bool resultado = PrePrestamosF.PrestamosAgregarPrevio(this.MiPrePrestamos);
            if (resultado)
            {
                this.MapearObjetoAControles(this.MiPrePrestamos);
                this.upImprtePrestamo.Update();
                this.upDetalleCuotas.Update();
            }
            return resultado;
        }



        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiPrePrestamos);
            this.MiPrePrestamos.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiPrePrestamos.ImporteAutorizado = 0;
                    this.MiPrePrestamos.PeriodoPrimerVencimiento = this.MiPrePrestamos.PeriodoPrimerVencimiento.HasValue ? this.MiPrePrestamos.PeriodoPrimerVencimiento : AyudaProgramacion.ObtenerPeriodo(this.MiPrePrestamos.FechaPrestamo);
                    this.ObtenerCancelacionesDescontar();
                    this.ObtenerCargosDescontar();
                    this.ObtenerSolicitudesPagosDescontar();
                    if (this.MiPrePrestamos.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.CompraDeCheque)
                        MiPrePrestamos.PrestamosCheques = this.ctrPrestamosCheques.ObtenerLista();
                    else
                        MiPrePrestamos.PrestamosCheques.Clear();

                    this.MiPrePrestamos.EstadoColeccion = EstadoColecciones.SinCambio;
                    this.MiPrePrestamos.ImporteAutorizado = this.MiPrePrestamos.ImporteSolicitado;
                    if (PrePrestamosF.PrestamosAgregarPrevio(this.MiPrePrestamos))
                    {
                        this._volverSiguiente = true;
                        this.IniciarControl(this.MiPrePrestamos, Gestion.ConfirmarAgregar, TipoAutorizar.SinPrivilegio);
                    }
                    else
                        this.MostrarMensaje(this.MiPrePrestamos.CodigoMensaje, true, this.MiPrePrestamos.CodigoMensajeArgs);
                    return;
                //break;
                case Gestion.ConfirmarAgregar:
                    //this.MiPrePrestamos.ImporteAutorizado = 0;
                    this.MiPrePrestamos.EstadoColeccion = EstadoColecciones.Agregado;
                    this.MiPrePrestamos.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    //this.MiPrePrestamos.ImporteAutorizado = this.MiPrePrestamos.ImporteSolicitado;
                    guardo = PrePrestamosF.PrestamosAgregar(this.MiPrePrestamos);
                    if (guardo)
                    {
                        this.GestionControl = Gestion.Consultar;
                        this.btnImprimir.Visible = true;
                        this.btnFirmarDocumento.Visible = true;
                        this.btnWhatsAppFirmarDocumento.Visible = true;
                        if (this.MisParametrosUrl.Contains("IdPrestamoIpsCadAutorizacion"))
                        {
                            this.MisParametrosUrl.Remove("IdPrestamoIpsCadAutorizacion");
                            this.MisParametrosUrl.Remove("IpsCADNumero");
                            this.MisParametrosUrl.Remove("IpsCADSelloTiempo");
                            this._prestamoIPS = false;
                        }
                    }
                    break;
                case Gestion.Anular:
                    this.MiPrePrestamos.Estado.IdEstado = (int)EstadosPrestamos.Anulado;
                    guardo = PrePrestamosF.PrestamosModificar(this.MiPrePrestamos);
                    break;
                case Gestion.AnularConfirmar:
                    this.MiPrePrestamos.Estado.IdEstado = (int)EstadosPrestamos.AnuladoPostConfirmado;
                    guardo = PrePrestamosF.PrestamosAnularConfirmado(this.MiPrePrestamos);
                    break;
                case Gestion.Cancelar:
                    this.MiPrePrestamos.Estado.IdEstado = (int)EstadosPrestamos.PendienteCancelacion;
                    this.MiPrePrestamos.UsuarioCancelacion.IdUsuarioCancelacion = this.UsuarioActivo.IdUsuario;
                    this.MiPrePrestamos.FilialCancelacion.IdFilialCancelacion = Convert.ToInt32(this.ddlFilialCancelacion.SelectedValue);
                    this.MiPrePrestamos.TipoValorCancelacion.IdTipoValorCancelacion = Convert.ToInt32(this.ddlTipoValorCancelacion.SelectedValue);
                    if (this.MiPrePrestamos.ImporteCancelacion != ((Evol.Controls.CurrencyTextBox)this.txtImporteCancelacion).Decimal)
                    {
                        this.MiPrePrestamos.ImporteCancelacion = ((Evol.Controls.CurrencyTextBox)this.txtImporteCancelacion).Decimal;
                        this.MiPrePrestamos.ModificaImporteCancelacion = true;
                    }
                    //bool incluir;
                    //foreach (GridViewRow fila in this.gvDatos.Rows)
                    //{
                    //    if (fila.RowType == DataControlRowType.DataRow)
                    //    {
                    //        incluir = ((CheckBox)fila.FindControl("gvDatosChkIncluir")).Checked;
                    //        this.MiPrePrestamos.PrestamosCuotas[fila.DataItemIndex].Incluir = incluir;
                    //        this.MiPrePrestamos.PrestamosCuotas[fila.DataItemIndex].EstadoColeccion = EstadoColecciones.Modificado;
                    //    }
                    //}
                    this.MiPrePrestamos.CantidadCuotasCanceladas = this.MiPrePrestamos.PrestamosCuotas.Count(x => x.Incluir);
                    guardo = PrePrestamosF.PrestamosModificar(this.MiPrePrestamos);
                    break;
                case Gestion.AnularCancelar:
                    this.MiPrePrestamos.Estado.IdEstado = (int)EstadosPrestamos.Confirmado;
                    this.MiPrePrestamos.UsuarioCancelacion.IdUsuarioCancelacion = 0;
                    foreach (PrePrestamosCuotas cuota in this.MiPrePrestamos.PrestamosCuotas)
                    {
                        if (cuota.IdRefTipoOperacionCobro.HasValue)
                        {
                            cuota.IdRefTipoOperacionCobro = null;
                            cuota.IdTipoOperacionCobro = null;
                            cuota.EstadoColeccion = EstadoColecciones.Modificado;
                        }
                    }
                    guardo = PrePrestamosF.PrestamosModificar(this.MiPrePrestamos);
                    break;
                case Gestion.Modificar:
                    //switch (this.MiPrePrestamos.Estado.IdEstado)
                    //{
                    //    case (int)EstadosPrestamos.Activo:
                    //    case (int)EstadosPrestamos.PreAutorizado:
                    //        //this.MiPrePrestamos.ImporteAutorizado = this.MiPrePrestamos.ImporteSolicitado;
                    //        break;
                    //    case (int)EstadosPrestamos.Autorizado:
                    //        break;
                    //    default:
                    //        break;
                    //}
                    ObtenerCancelacionesDescontar();
                    switch (this.MiTipoAutorizar)
                    {
                        case TipoAutorizar.PreAutorizar:
                            //if (this.MiPrePrestamos.ImporteAutorizado < 0)
                            //{
                            //    this.MostrarMensaje("ValidarImportePreAutorizado", true);
                            //    return;
                            //}
                            this.MiPrePrestamos.FechaPreAutorizado = DateTime.Now;
                            this.MiPrePrestamos.Estado.IdEstado = (int)EstadosPrestamos.PreAutorizado;
                            this.MiPrePrestamos.UsuarioPreAutorizar.IdUsuarioPreAutorizar = this.UsuarioActivo.IdUsuario;
                            ObtenerImporteCancelacion(this.MiPrePrestamos);
                            break;
                        case TipoAutorizar.Autorizar:
                            //if (this.MiPrePrestamos.ImporteAutorizado < 0)
                            //{
                            //    this.MostrarMensaje("ValidarImporteAutorizado", true);
                            //    return;
                            //}
                            this.MiPrePrestamos.FechaAutorizado = DateTime.Now;
                            this.MiPrePrestamos.FechaValidezAutorizado = Convert.ToDateTime(this.txtFechaValidezAutorizado.Text);
                            this.MiPrePrestamos.Estado.IdEstado = (int)EstadosPrestamos.Autorizado;
                            this.MiPrePrestamos.UsuarioAutorizar.IdUsuarioAutorizar = this.UsuarioActivo.IdUsuario;
                            break;
                        default:
                            break;
                    }
                    guardo = PrePrestamosF.PrestamosModificar(this.MiPrePrestamos);
                    break;
                case Gestion.Aplicar:
                    guardo = PrePrestamosF.PrestamosAplicarCheque(this.MiPrePrestamos);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.btnAceptar.Visible = false;
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiPrePrestamos.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiPrePrestamos.CodigoMensaje, true, this.MiPrePrestamos.CodigoMensajeArgs);
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                case Gestion.Modificar:
                case Gestion.Anular:
                case Gestion.Consultar:
                case Gestion.Cancelar:
                default:
                    if (this.PrestamosAfiliadosModificarDatosCancelar != null)
                        this.PrestamosAfiliadosModificarDatosCancelar();
                    break;
                case Gestion.ConfirmarAgregar:
                    this._volverSiguiente = true;
                    if (MiPrePrestamos.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosManual)
                        this.IniciarControl(this.MiPrePrestamos, Gestion.Agregar, TipoAutorizar.SinPrivilegio, EnumTGETiposOperaciones.PrestamosManual);
                    else
                        this.IniciarControl(this.MiPrePrestamos, Gestion.Agregar, TipoAutorizar.SinPrivilegio);
                    this.ctrCamposValores.IniciarControl(this.MiPrePrestamos.Campos, Gestion.Agregar, true);
                    break;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.PrestamosAfiliadosModificarDatosCancelar != null)
                this.PrestamosAfiliadosModificarDatosCancelar();
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            EnumTGEComprobantes enumTGEComprobantes;
            if (this.MiPrePrestamos.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosBancoDelSol)
                enumTGEComprobantes = EnumTGEComprobantes.PrePrestamosBancoSol;
            else
                enumTGEComprobantes = EnumTGEComprobantes.PrePrestamos;

            TESCajasMovimientos movimiento = new TESCajasMovimientos();
            movimiento.IdRefTipoOperacion = MiPrePrestamos.IdPrestamo;
            movimiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            movimiento.TipoOperacion.IdTipoOperacion = MiPrePrestamos.TipoOperacion.IdTipoOperacion;
            TGEPlantillas miPlantilla = TGEGeneralesF.PlantillasObtenerDatosPorTipoOperacion(movimiento);

            byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(enumTGEComprobantes, miPlantilla.Codigo, this.MiPrePrestamos, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            ExportPDF.ExportarPDF(pdf, this.UpdatePanel2, string.Concat("Prestamo_", MiPrePrestamos.IdPrestamo.ToString().PadLeft(10, '0')), this.UsuarioActivo);

            //TGEPlantillas plantilla = new TGEPlantillas();
            //plantilla.Codigo = "PrestamosSolicitudOtorgamiento";
            //plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

            //if (plantilla.HtmlPlantilla.Trim().Length > 0)
            //{
            //    TGEComprobantes comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(EnumTGEComprobantes.PrePrestamos);
            //    DataSet ds = ExportPDF.ObtenerDatosReporteComprobante(this.MiPrePrestamos, comprobante);
            //    ExportPDF.ConvertirHtmlEnPdf(this.UpdatePanel1, plantilla, ds, this.UsuarioActivo);
            //}
            //else
            //{
            //    if (this.MiPrePrestamos.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosBancoDelSol)
            //        this.ctrPopUpComprobantes.CargarReporte(this.MiPrePrestamos, EnumTGEComprobantes.PrePrestamosBancoSol);
            //    else
            //        this.ctrPopUpComprobantes.CargarReporte(this.MiPrePrestamos, EnumTGEComprobantes.PrePrestamos);
            //}
        }

        protected void btnFirmarDocumento_Click(object sender, EventArgs e)
        {
            TGEParametrosValores paramVal = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.FirmaDigitalHabilitar);
            bool firmaDigital = paramVal.ParametroValor == string.Empty ? false : Convert.ToBoolean(paramVal.ParametroValor);
            if (!firmaDigital)
            {
                this.MostrarMensaje(this.ObtenerMensajeSistema("ValidarFirmaDigitalManuscritaHabilitar"), true);
                return;
            }
            MailMessage mail = new MailMessage();
            this.MiPrePrestamos.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            if (PrePrestamosF.PrestamosArmarMailLinkFirmarDocumento(this.MiPrePrestamos, mail))
            {
                this.popUpMail.IniciarControl(mail, this.MiPrePrestamos);
            }
            else
            {
                this.MostrarMensaje(MiPrePrestamos.CodigoMensaje, true, MiPrePrestamos.CodigoMensajeArgs);
                return;
            }
        }

        protected void btnWhatsAppFirmarDocumento_Click(object sender, EventArgs e)
        {
            TGEParametrosValores paramVal = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.FirmaDigitalHabilitar);
            bool firmaDigital = paramVal.ParametroValor == string.Empty ? false : Convert.ToBoolean(paramVal.ParametroValor);
            if (!firmaDigital)
            {
                this.MostrarMensaje(this.ObtenerMensajeSistema("ValidarFirmaDigitalManuscritaHabilitar"), true);
                return;
            }
            //TGEFirmarDocumentos firmarDoc = new TGEFirmarDocumentos();
            //firmarDoc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            //firmarDoc.IdRefTabla = this.MiPrePrestamos.IdPrestamo;
            //firmarDoc.Tabla = "PrePrestamos";
            //firmarDoc.IdTipoOperacion = this.MiPrePrestamos.TipoOperacion.IdTipoOperacion;
            //PropertyInfo prop = MiPrePrestamos.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() == 1).ToList()[0];
            //firmarDoc.Key = prop.Name;
            //firmarDoc.CodigoPlantilla = "PrestamosSolicitudOtorgamiento";

            string text = string.Concat("Estimado ", this.MiPrePrestamos.Afiliado.ApellidoNombre, " haga clic en el siguiente link para firmar el documento ", hfLinkFirmarDocumento.Value);
            AfiTelefonos cel = AfiliadosF.AfiliadosObtenerTelefonoCelular(this.MiPrePrestamos.Afiliado);
            string numero = /*cel.Prefijo.ToString() + */cel.Numero.ToString();
            string urlwa = string.Format("https://api.whatsapp.com/send?phone={0}&text={1}", numero, HttpUtility.UrlEncode(text));
            ScriptManager.RegisterStartupScript(this.UpdatePanel2, this.UpdatePanel2.GetType(), "scriptWa", string.Format("EnviarWhatsApp('{0}');", urlwa), true);
        }

        protected void btnFirmarDocumentoBaja_Click(object sender, EventArgs e)
        {
            TGEFirmarDocumentos firmarDoc = new TGEFirmarDocumentos();
            firmarDoc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            firmarDoc.IdRefTabla = this.MiPrePrestamos.IdPrestamo;
            firmarDoc.Tabla = "PrePrestamos";
            firmarDoc.IdTipoOperacion = this.MiPrePrestamos.TipoOperacion.IdTipoOperacion;
            firmarDoc.Estado.IdEstado = (int)Estados.Baja;
            bool resultado = TGEGeneralesF.FirmarDocumentosModificar(firmarDoc);
            if (resultado)
            {
                this.btnFirmarDocumentoBaja.Visible = false;
                this.btnFirmarDocumento.Visible = true;
                this.btnWhatsAppFirmarDocumento.Visible = true;
                this.MostrarMensaje(firmarDoc.CodigoMensaje, false);
            }
            else
            {
                this.MostrarMensaje(firmarDoc.CodigoMensaje, true, firmarDoc.CodigoMensajeArgs);
            }
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.MiPrePrestamos.ConfirmarAccion)
            {
                this.MiPrePrestamos.ConfirmarExcedido = true;
                this.btnAceptar_Click(null, EventArgs.Empty);
            }
            //else
            //{
            //    if (this.PrestamosAfiliadosModificarDatosAceptar != null)
            //        this.PrestamosAfiliadosModificarDatosAceptar(this, this.MiPrePrestamos);
            //}
        }

        protected void txtFechaAlta_TextChanged(object sender, EventArgs e)
        {
            this.ddlPlan_SelectedIndexChanged(null, EventArgs.Empty);
        }

        protected void txtMonto_TextChanged(object sender, EventArgs e)
        {
            this.txtMontoAutorizado.Decimal = this.txtMonto.Decimal;
            if (!this.RecalcularPrestamo())
            {
                //this.MiPrePrestamos.ImporteSolicitado = monto;
                //this.MiPrePrestamos.ImporteAutorizado = this.MiPrePrestamos.ImporteAutorizado > 0 ? monto : 0;
                this.MostrarMensaje(this.MiPrePrestamos.CodigoMensaje, true, this.MiPrePrestamos.CodigoMensajeArgs);
            }
        }

        protected void txtMontoAutorizado_TextChanged(object sender, EventArgs e)
        {
            if (!this.RecalcularPrestamo())
            {
                //this.MiPrePrestamos.ImporteSolicitado = monto;
                //this.MiPrePrestamos.ImporteAutorizado = this.MiPrePrestamos.ImporteAutorizado > 0 ? monto : 0;
                this.MostrarMensaje(this.MiPrePrestamos.CodigoMensaje, true, this.MiPrePrestamos.CodigoMensajeArgs);
            }
        }

        protected void txtCantidadCuotas_TextChanged(object sender, EventArgs e)
        {
            //int cantidadCuotas = this.MiPrePrestamos.CantidadCuotas;
            if (!this.RecalcularPrestamo())
            {
                //this.txtCantidadCuotas.Text = cantidadCuotas.ToString();
                //this.MiPrePrestamos.CantidadCuotas = cantidadCuotas;
                this.MostrarMensaje(this.MiPrePrestamos.CodigoMensaje, true, this.MiPrePrestamos.CodigoMensajeArgs);
            }
        }

        protected void ddlTipoOperacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ctrPrestamosCheques.Visible = false;
            //this.ctrPrestamosCheques.LimpiarDatos();
            this.txtMonto.Enabled = true;
            this.dvImporteCuota.Visible = false;
            if (!string.IsNullOrEmpty(this.ddlTipoOperacion.SelectedValue))
            {
                if (!this._volverSiguiente)
                {
                    this.ddlTiposCargos.DataSource = null;
                    this.ddlTiposCargos.DataBind();
                    CarTiposCargos cargo = new CarTiposCargos();
                    cargo.TipoOperacion.IdTipoOperacion = Convert.ToInt32(this.ddlTipoOperacion.SelectedValue);
                    cargo.Estado.IdEstado = (int)Estados.Activo;
                    this.ddlTiposCargos.DataSource = CargosF.TiposCargosObtenerListaFiltro(cargo);
                    this.ddlTiposCargos.DataValueField = "IdTipoCargo";
                    this.ddlTiposCargos.DataTextField = "TipoCargo";
                    this.ddlTiposCargos.DataTextField = "TipoCargo";
                    this.ddlTiposCargos.DataBind();
                }
                if (this.ddlTiposCargos.Items.Count != 1)
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlTiposCargos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                //this.ddlTiposCargos_SelectedIndexChanged(null, EventArgs.Empty);

                if (this._volverSiguiente)
                {
                    var item = this.ddlTiposCargos.Items.FindByValue(MiPrePrestamos.TipoCargo.IdTipoCargo.ToString());
                    if (item == null)
                        this.ddlTiposCargos.Items.Add(new ListItem(MiPrePrestamos.TipoCargo.TipoCargo, MiPrePrestamos.TipoCargo.IdTipoCargo.ToString()));

                    this.ddlTiposCargos.SelectedValue = MiPrePrestamos.TipoCargo.IdTipoCargo.ToString();
                    //this.MapearObjetoAControles(MiPrePrestamos);
                }

                this.MiPrePrestamos.TipoOperacion.IdTipoOperacion = Convert.ToInt32(this.ddlTipoOperacion.SelectedValue);

                if (!this._volverSiguiente)
                {
                    this.MiPrePrestamos.CargosExcedidos = new List<CarCuentasCorrientes>();
                    this.MisSolicitudesPendientes = new List<CapSolicitudPago>();
                    this.MiPrePrestamos.Cancelaciones = new List<PrePrestamos>();
                    this.MiPrePrestamos.SolicitudesPagos = new List<CapSolicitudPago>();
                    this.MisCargosPendientes = new List<CarCuentasCorrientes>();
                    this.MisPrestamosCancelacionesPendientes = new List<PrePrestamos>();
                }

                if (this.MiPrePrestamos.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosCortoPlazo)
                {
                    this.MiPrePrestamos.ImporteExcedido = 0;

                    this.txtImporteExedido.Text = (0).ToString("C2");

                    this.MiPrePrestamos.ImporteCancelaciones = 0;
                    this.txtImporteCancelaciones.Text = (0).ToString("C2");

                    this.txtImporteSolicitudesPagos.Text = (0).ToString("C2");
                    this.MiPrePrestamos.ImporteSolicitudesPagos = 0;

                }
                else if (this.MiPrePrestamos.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.CompraDeCheque)
                {
                    this.txtMonto.Enabled = false;
                    this.ctrPrestamosCheques.IniciarControl(MiPrePrestamos, GestionControl);
                }
                else if (this.MiPrePrestamos.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosManual)
                {
                    this.dvImporteCuota.Visible = true;
                }
                //else if (this.MiPrePrestamos.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosBancoDelSol)
                //{
                //    this.MiPrePrestamos.ImporteExcedido = 0;
                //    this.MiPrePrestamos.CargosExcedidos = new List<CarCuentasCorrientes>();
                //    this.MisCargosPendientes = new List<CarCuentasCorrientes>();
                //    this.MiPrePrestamos.Cancelaciones = new List<PrePrestamos>();
                //    this.txtImporteExedido.Text = "0";
                //    this.txtImporteSolicitudesPagos.Text = "0";
                //    this.MiPrePrestamos.ImporteSolicitudesPagos = 0;
                //    this.MisSolicitudesPendientes = new List<CapSolicitudPago>();
                //    this.MiPrePrestamos.SolicitudesPagos = new List<CapSolicitudPago>();
                //}
                else
                {
                    //PRESTAMOS A LARGO PLAZO
                    //CANCELACIONES
                    this.MisPrestamosCancelacionesPendientes = PrePrestamosF.PrestamosObtenerPorAfiliadoCancelacion(this.MiPrePrestamos);
                    PrePrestamos preExiste;
                    foreach (PrePrestamos can in this.MisPrestamosCancelacionesPendientes)
                    {
                        if (this.MiPrePrestamos.Cancelaciones.Exists(x => x.IdPrestamo == can.IdPrestamo))
                        {
                            preExiste = this.MiPrePrestamos.Cancelaciones.Find(x => x.IdPrestamo == can.IdPrestamo);
                            AyudaProgramacionLN.MatchObjectProperties(preExiste, can);
                            can.PrestamosCuotas = preExiste.PrestamosCuotas;
                        }
                    }
                    this.MiPrePrestamos.Cancelaciones = new List<PrePrestamos>();
                    this.MiPrePrestamos.Cancelaciones = this.MisPrestamosCancelacionesPendientes;
                    //this.MiPrePrestamos.ImporteCancelaciones = this.MisPrestamosCancelacionesPendientes.Where(x => x.Incluir).Sum(x => x.ImporteCancelacion);
                    this.txtImporteCancelaciones.Text = this.MisPrestamosCancelacionesPendientes.Where(x => x.Incluir).Sum(x => x.ImporteCancelacion).ToString("C2");// this.MiPrePrestamos.ImporteCancelaciones.ToString("C2");
                    this.tpCancelaciones.Visible = this.MisPrestamosCancelacionesPendientes.Count > 0;

                    //CARGOS PENDIENTES
                    CarCuentasCorrientes filtro = new CarCuentasCorrientes();
                    filtro.IdAfiliado = this.MiPrePrestamos.Afiliado.IdAfiliado;
                    this.MisCargosPendientes = PrePrestamosF.PrestamosObtenerCargosPendientes(filtro);
                    foreach (CarCuentasCorrientes cargoCuenta in this.MisCargosPendientes)
                        if (!cargoCuenta.Incluir)
                            cargoCuenta.Incluir = this.MiPrePrestamos.CargosExcedidos.Exists(x => x.IdCuentaCorriente == cargoCuenta.IdCuentaCorriente && x.Incluir == true);

                    this.MiPrePrestamos.CargosExcedidos = new List<CarCuentasCorrientes>();
                    this.MiPrePrestamos.CargosExcedidos = this.MisCargosPendientes.Where(x => x.Incluir).ToList();
                    this.MiPrePrestamos.ImporteExcedido = this.MisCargosPendientes.Where(x => x.Incluir).Sum(x => x.Importe);
                    this.txtImporteExedido.Text = this.MiPrePrestamos.ImporteExcedido.ToString("C2");
                    this.tpCargosPendientes.Visible = this.MisCargosPendientes.Count > 0;

                    //SOLICITUDES PAGOS PENDIENTES
                    CapSolicitudPago spFiltro = new CapSolicitudPago();
                    spFiltro.Afiliado.IdAfiliado = this.MiPrePrestamos.Afiliado.IdAfiliado;
                    spFiltro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.SolicitudesPagosTerceros;
                    this.MisSolicitudesPendientes = CuentasPagarF.SolicitudPagoObtenerTercerosPendienteCobro(spFiltro);
                    foreach (CapSolicitudPago sp in this.MisSolicitudesPendientes)
                        if (!sp.IncluirEnOP)
                            sp.IncluirEnOP = this.MiPrePrestamos.SolicitudesPagos.Exists(x => x.IdSolicitudPago == sp.IdSolicitudPago && x.IncluirEnOP == true);

                    this.MiPrePrestamos.SolicitudesPagos = new List<CapSolicitudPago>();
                    this.MiPrePrestamos.SolicitudesPagos = this.MisSolicitudesPendientes.Where(x => x.IncluirEnOP).ToList();
                    this.MiPrePrestamos.ImporteSolicitudesPagos = this.MisSolicitudesPendientes.Where(x => x.IncluirEnOP).Sum(x => x.ImporteTotal);
                    this.txtImporteSolicitudesPagos.Text = this.MiPrePrestamos.ImporteSolicitudesPagos.ToString("C2");
                    this.tpSolicitudesPagos.Visible = this.MiPrePrestamos.SolicitudesPagos.Count > 0;
                }

                AyudaProgramacion.CargarGrillaListas<CarCuentasCorrientes>(this.MisCargosPendientes, false, this.gvCuentaCorriente, true);
                AyudaProgramacion.CargarGrillaListas<CapSolicitudPago>(this.MisSolicitudesPendientes, false, this.gvSolicitudesPagos, true);
                this.UpdatePanel2.Update();

                this.ctrCamposValores.IniciarControl(this.MiPrePrestamos, this.MiPrePrestamos.TipoOperacion, this.GestionControl);
                this.UpdatePanel3.Update();
            }
            else
            {
                this.ddlTiposCargos.Items.Clear();
                //AyudaProgramacion.AgregarItemSeleccione(this.ddlTiposCargos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
            if (this.ddlTiposCargos.Items.Count == 0)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTiposCargos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            //if (!this._volverSiguiente)
            this.ddlTiposCargos_SelectedIndexChanged(null, EventArgs.Empty);
        }

        protected void ddlTiposCargos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlTiposCargos.SelectedValue))
            {
                MiPrePrestamos.TipoCargo.IdTipoCargo = Convert.ToInt32(this.ddlTiposCargos.SelectedValue);

                if (this._volverSiguiente)
                {
                    foreach (PrePrestamos can in MisPrestamosCancelacionesPendientes)
                    {
                        if (MiPrePrestamos.Cancelaciones.Exists(x => x.IdPrestamo == can.IdPrestamo))
                        {
                            if (gvCancelaciones.Rows.Count > 0)
                            {
                                PrePrestamos canMemoria = MiPrePrestamos.Cancelaciones.FirstOrDefault(x => x.IdPrestamo == can.IdPrestamo);
                                can.ImporteCancelacion = canMemoria.ImporteCancelacion;
                                Label lblCantidadCuotasCanceladas = (Label)this.gvCancelaciones.Rows[this.MiIndiceGvCancelaciones].FindControl("lblCantidadCuotasCanceladas");
                                lblCantidadCuotasCanceladas.Text = this.MiPrePrestamos.Cancelaciones[this.MiIndiceGvCancelaciones].PrestamosCuotas.Count(x => x.Incluir).ToString();
                            }
                        }

                    }
                }
                else
                {
                    this.MisPrestamosCancelacionesPendientes = PrePrestamosF.PrestamosObtenerPorAfiliadoCancelacion(this.MiPrePrestamos);
                }
                this.MiPrePrestamos.Cancelaciones = this.MisPrestamosCancelacionesPendientes;
                AyudaProgramacion.CargarGrillaListas<PrePrestamos>(this.MiPrePrestamos.Cancelaciones, false, this.gvCancelaciones, true);
                this.UpdatePanel2.Update();
                this.ddlFormasCobros.DataSource = null;
                this.ddlFormasCobros.DataBind();
                TGEFormasCobrosAfiliados formasCobroAfi = new TGEFormasCobrosAfiliados();
                formasCobroAfi.IdAfiliado = this.MiPrePrestamos.Afiliado.IdAfiliado;
                formasCobroAfi.IdTipoCargo = Convert.ToInt32(this.ddlTiposCargos.SelectedValue);
                formasCobroAfi.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
                this.MisFormasCobros = TGEGeneralesF.FormasCobrosAfiliadosObtenerPorAfiliadoTipoCargo(formasCobroAfi);
                this.ddlFormasCobros.DataSource = this.MisFormasCobros;
                this.ddlFormasCobros.DataValueField = "IdFormaCobroAfiliado";
                this.ddlFormasCobros.DataTextField = "FormaCobroDescripcion";
                this.ddlFormasCobros.DataBind();

                if (this.MiPrePrestamos.Cancelaciones.Count > 0)
                {
                    this.tpCancelaciones.Visible = true;
                }

                ListItem item = this.ddlFormasCobros.Items.FindByValue(this.MiPrePrestamos.FormaCobroAfiliado.IdFormaCobroAfiliado.ToString());
                if (item != null)
                {
                    this.ddlFormasCobros.SelectedValue = item.Value;
                }
            }
            else
            {
                this.ddlFormasCobros.Items.Clear();
            }

            if (this.ddlFormasCobros.Items.Count == 0)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlFormasCobros, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            if (this._prestamoIPS)
            {
                if (this.MisFormasCobros.Exists(x => x.FormaCobroIdFormaCobro == (int)EnumTGEFormasCobros.IPS))
                {
                    this.ddlFormasCobros.SelectedValue = this.MisFormasCobros.Find(x => x.FormaCobroIdFormaCobro == (int)EnumTGEFormasCobros.IPS).IdFormaCobroAfiliado.ToString();
                }
            }
            ddlFormasCobros_SelectedIndexChanged(null, EventArgs.Empty);
        }

        protected void ddlFormasCobros_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.MiPrePrestamos.Estado.IdEstado != (int)EstadosPrestamos.Confirmado)
            {
                this.ddlPeriodoVencimiento.Items.Clear();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlPeriodoVencimiento, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                if (!string.IsNullOrEmpty(this.ddlFormasCobros.SelectedValue))
                {
                    this.MiPrePrestamos.FormaCobroAfiliado = this.MisFormasCobros[this.ddlFormasCobros.SelectedIndex];
                    List<int> items = PrePrestamosF.PrestamosObtenerPeriodoPrimerVencimiento(this.MiPrePrestamos.FormaCobroAfiliado);
                    if (items.Count > 0)
                    {
                        this.ddlPeriodoVencimiento.Items.Clear();
                        foreach (int i in items)
                            this.ddlPeriodoVencimiento.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }
                    else
                        AyudaProgramacion.AgregarItemSeleccione(this.ddlPeriodoVencimiento, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                    MostrarDetalleFormacobro();
                    ddlMoneda_SelectedIndexChanged(null, EventArgs.Empty);
                    //IPS NUMERO CAD
                    //if ( this._prestamoIPS)
                    if (!string.IsNullOrWhiteSpace(ddlFormasCobros.SelectedValue))
                    {
                        if (this.MisFormasCobros.FirstOrDefault(x => x.IdFormaCobroAfiliado == Convert.ToInt32(ddlFormasCobros.SelectedValue)).FormaCobro.IdFormaCobro == (int)EnumTGEFormasCobros.IPS)
                        {
                            TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.PrestamosPorIPSSinNumeroCAD);
                            bool bvalor = !string.IsNullOrEmpty(valor.ParametroValor) ? Convert.ToBoolean(valor.ParametroValor) : false;

                            if (this.MiPrePrestamos.IpsCADNumero == 0 && !bvalor)
                            {
                                this.MostrarMensaje("ValidarIPSNumeroCADCero", true, new List<string>() { this.MisFormasCobros[this.ddlFormasCobros.SelectedIndex].FormaCobro.FormaCobro });
                            }
                            //this._prestamoIPS = false;
                            List<TGECampos> camposValores = this.ctrCamposValores.ObtenerLista();
                            if (camposValores.Exists(x => x.Nombre == "IPSCad"))
                            {
                                TGECampos campo = camposValores.Find(x => x.Nombre == "IPSCad");
                                TextBox txtNumeroCad = (TextBox)this.paginaSegura.BuscarControlRecursivo(this.ctrCamposValores, campo.IdCampo.ToString());
                                if (txtNumeroCad != null)
                                {
                                    txtNumeroCad.Text = this.MiPrePrestamos.IpsCADNumero.ToString();
                                    txtNumeroCad.Enabled = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    ListItem item = ddlPlan.Items.FindByValue(string.Empty);
                    if (item == null)
                        AyudaProgramacion.AgregarItemSeleccione(this.ddlPlan, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                }

                ddlPlan_SelectedIndexChanged(null, EventArgs.Empty);
                //this.upPlan.Update();
            }
        }

        protected void ddlMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlMoneda.SelectedValue))
            {
                if (!string.IsNullOrEmpty(ddlTipoOperacion.SelectedValue)
                && !string.IsNullOrEmpty(ddlFormasCobros.SelectedValue)
                && !string.IsNullOrEmpty(ddlMoneda.SelectedValue))
                {
                    this.MiPrePrestamos.Moneda = this.MisMonedas.First(x => x.IdMoneda == Convert.ToInt32(ddlMoneda.SelectedValue));
                    SetInitializeCulture(MiPrePrestamos.Moneda.Moneda);
                    PrePrestamosPlanes planFiltro = new PrePrestamosPlanes();
                    planFiltro.Estado.IdEstado = (int)Estados.Activo;
                    planFiltro.FormaCobro.IdFormaCobro = this.MisFormasCobros[this.ddlFormasCobros.SelectedIndex].FormaCobro.IdFormaCobro;
                    planFiltro.TipoOperacion.IdTipoOperacion = this.ddlTipoOperacion.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoOperacion.SelectedValue);
                    planFiltro.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    planFiltro.Moneda.IdMoneda = Convert.ToInt32(ddlMoneda.SelectedValue);
                    planFiltro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
                    planFiltro.IdTipoCargo = this.ddlTiposCargos.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlTiposCargos.SelectedValue);
                    ListItem itemPlanAnt = this.ddlPlan.Items.FindByValue(this.ddlPlan.SelectedValue);

                    this.ddlPlan.Items.Clear();
                    this.ddlPlan.SelectedIndex = -1;
                    this.ddlPlan.SelectedValue = null;
                    this.ddlPlan.ClearSelection();

                    this.MisPlanes = PrePrestamosF.PrestamosPlanesObtenerLista(planFiltro);
                    this.ddlPlan.DataSource = this.MisPlanes;
                    this.ddlPlan.DataValueField = "IdPrestamoPlan";
                    this.ddlPlan.DataTextField = "Descripcion";
                    this.ddlPlan.DataBind();
                    if (this.MisPlanes.Count != 1)
                        AyudaProgramacion.AgregarItemSeleccione(this.ddlPlan, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                    if (itemPlanAnt != null)
                    {
                        ListItem itemPlanExiste = this.ddlPlan.Items.FindByValue(itemPlanAnt.Value);
                        if (itemPlanExiste != null)
                            this.ddlPlan.SelectedValue = itemPlanAnt.Value;
                    }
                    this.ctrCamposValores.IniciarControl(this.MiPrePrestamos, planFiltro.FormaCobro, this.GestionControl);
                }
            }
            ddlPlan_SelectedIndexChanged(null, EventArgs.Empty);
        }

        protected void ddlPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlPlan.SelectedValue))
            {
                ListItem item;
                //this.MiPrePrestamos.PrestamoPlan.IdPrestamoPlan = Convert.ToInt32(this.ddlPlan.SelectedValue);
                //this.MiPrePrestamos.PrestamoPlan.Descripcion = this.ddlPlan.SelectedItem.Text;
                //this.MiPrePrestamos.PrestamoPlan.PrestamoPlanTasa = PrePrestamosF.PrestamosPlanesTasasObtenerTasaActiva(this.MiPrePrestamos.PrestamoPlan);
                PrePrestamosPlanes plan = this.MisPlanes[this.ddlPlan.SelectedIndex];
                plan.FechaAlta = this.txtFechaAlta.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(this.txtFechaAlta.Text);
                PrePrestamosPlanesTasas planTasa = PrePrestamosF.PrestamosPlanesTasasObtenerTasaActiva(this.MisPlanes[this.ddlPlan.SelectedIndex]);
                if (planTasa.IdPrestamoPlanTasa == 0)
                {
                    this.txtTasaInteres.Text = string.Empty;
                    this.txtCantidadCuotas.Text = string.Empty;
                    this.txtPorcentajeCapitalSocial.Text = string.Empty;
                    item = this.ddlPlan.Items.FindByValue(string.Empty);
                    if (item == null)
                        AyudaProgramacion.AgregarItemSeleccione(this.ddlPlan, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                    this.ddlPlan.SelectedValue = string.Empty;
                    List<string> lista = new List<string>();
                    lista.Add(plan.Descripcion);
                    lista.Add(this.txtFechaAlta.Text);
                    this.MostrarMensaje("ValidarPlanTasaFechaPrestamo", true, lista);
                    //this.upPlan.Update();
                    if (this.txtPorcentajeCapitalSocial.Decimal > 0)
                    {
                        this.txtPorcentajeCapitalSocial.Decimal = 0;
                        this.txtImporteCapitalSocial.Decimal = 0;
                        this.phCapitalSocial.Visible = false;
                        this.upCapitalSocial.Update();
                    }
                    return;
                }
                if (plan.TipoUnidad.IdTiposUnidades > 0)
                {
                    dvTipoUnidad.Visible = true;
                    dvValorUnidad.Visible = true;
                    txtTipoUnidad.Text = plan.TipoUnidad.Descripcion;
                    hdfIdTipoUnidad.Value = plan.TipoUnidad.IdTiposUnidades.ToString();
                }
                this.MisPlanes[this.ddlPlan.SelectedIndex].PrestamoPlanTasa = planTasa;
                this.MiPrePrestamos.PrestamoPlan = this.MisPlanes[this.ddlPlan.SelectedIndex];
                this.txtTasaInteres.Text = this.MiPrePrestamos.PrestamoPlan.PrestamoPlanTasa.Tasa.ToString();

                decimal porcAnterior = this.txtPorcentajeCapitalSocial.Decimal;
                this.txtPorcentajeCapitalSocial.Text = this.MisPlanes[this.ddlPlan.SelectedIndex].PorcentajeCapitalSocial.HasValue ?
                    this.MisPlanes[this.ddlPlan.SelectedIndex].PorcentajeCapitalSocial.Value.ToString("N4") : string.Empty;
                this.phCapitalSocial.Visible = this.txtPorcentajeCapitalSocial.Decimal > 0;
                if (porcAnterior != this.txtPorcentajeCapitalSocial.Decimal)
                    this.upCapitalSocial.Update();

                if (planTasa.CantidadCuotas == planTasa.CantidadCuotasHasta)
                {
                    this.txtCantidadCuotas.Text = planTasa.CantidadCuotas.ToString();
                    this.txtCantidadCuotas.Enabled = false;
                }
                else
                {
                    this.txtCantidadCuotas.Text = this.txtCantidadCuotas.Text == string.Empty ? this.MiPrePrestamos.PrestamoPlan.PrestamoPlanTasa.CantidadCuotas.ToString() : this.txtCantidadCuotas.Text;
                    this.txtCantidadCuotas.Enabled = true;
                }

                if (MiPrePrestamos.TipoOperacion.IdTipoOperacion != (int)EnumTGETiposOperaciones.CompraDeCheque)
                {
                    this.txtMonto.Enabled = false;
                    if (planTasa.ImporteDesde.HasValue && planTasa.ImporteHasta.HasValue && planTasa.ImporteDesde.Value != planTasa.ImporteHasta.Value)
                    {
                        this.txtMonto.Enabled = true;
                        if (this.txtMonto.Decimal > 0 && (this.txtMonto.Decimal < planTasa.ImporteDesde.Value || this.txtMonto.Decimal > planTasa.ImporteHasta.Value))
                        {
                            this.txtMonto.Decimal = planTasa.ImporteDesde.Value;
                            this.txtMontoAutorizado.Decimal = this.txtMontoAutorizado.Decimal > 0 ? planTasa.ImporteDesde.Value : 0;
                        }
                    }
                    else if (planTasa.ImporteDesde.HasValue)
                    {
                        this.txtMonto.Decimal = planTasa.ImporteDesde.Value;
                        this.txtMontoAutorizado.Decimal = this.txtMontoAutorizado.Decimal > 0 ? planTasa.ImporteDesde.Value : 0;
                    }
                    else
                        this.txtMonto.Enabled = true;
                }

                if (this.MisFormasCobros[this.ddlFormasCobros.SelectedIndex].FormaCobro.IdFormaCobro == (int)EnumTGEFormasCobros.IPS)
                {
                    //PrePrestamosIpsCadAutorizaciones ipsAutorizar = new PrePrestamosIpsCadAutorizaciones();
                    //ipsAutorizar.IdPrestamoIpsCadAutorizacion = this.MiPrePrestamos.IdPrestamoIpsCadAutorizacion;
                    //ipsAutorizar = PrePrestamosF.PrePrestamosIpsCadAutorizacionesObtenerDatosCompletos(ipsAutorizar);

                    PrePrestamosIpsPlanes ipsPlan = new PrePrestamosIpsPlanes();
                    ipsPlan.IdPlan = this.MisPlanes[this.ddlPlan.SelectedIndex].IdPrestamoPlan;
                    ipsPlan.IdPrestamoIpsCadAutorizacion = this.MiPrePrestamos.IdPrestamoIpsCadAutorizacion;
                    ipsPlan = PrePrestamosF.PrePrestamosIpsPlanesObtenerDatosCompletos(ipsPlan);
                    if (ipsPlan.IdPrestamoIpsPlan == 0)
                    {
                        this.MostrarMensaje("ValidarIpsPlanes", true, new List<string> { this.MiPrePrestamos.IpsCADNumero.ToString() });
                        item = this.ddlPlan.Items.FindByValue(string.Empty);
                        if (item == null)
                            AyudaProgramacion.AgregarItemSeleccione(this.ddlPlan, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                        this.ddlPlan.SelectedValue = string.Empty;
                    }
                    else
                    {
                        this.txtCantidadCuotas.Text = ipsPlan.CantidadCuotas.ToString();
                        this.txtMonto.Decimal = ipsPlan.ImporteTotal;
                        this.txtMontoAutorizado.Decimal = this.txtMontoAutorizado.Decimal > 0 ? ipsPlan.ImporteTotal : 0;
                        this.txtMonto.Enabled = false;
                        this.txtCantidadCuotas.Enabled = false;
                    }
                }

                this.upMonto.Update();
                if (this.GestionControl == Gestion.Modificar)
                {
                    if (!this.RecalcularPrestamo())
                    {
                        this.MostrarMensaje(this.MiPrePrestamos.CodigoMensaje, true, this.MiPrePrestamos.CodigoMensajeArgs);
                    }
                }
                this.MostrarDetalleFormacobroPlan();
                //this.UpdatePanel1.Update();
            }
            else
            {
                this.txtTasaInteres.Text = string.Empty;
                this.txtCantidadCuotas.Text = string.Empty;
                if (this.txtPorcentajeCapitalSocial.Decimal > 0)
                {
                    this.txtPorcentajeCapitalSocial.Decimal = 0;
                    this.txtImporteCapitalSocial.Decimal = 0;
                    this.phCapitalSocial.Visible = false;
                    this.upCapitalSocial.Update();
                }
            }
            this.ctrCamposValores.IniciarControl(this.MiPrePrestamos, this.MiPrePrestamos.PrestamoPlan, this.GestionControl);
            this.UpdatePanel3.Update();
            if (this.ddlPlan.Items.Count == 0)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlPlan, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void ddlTipoValor_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.MiPrePrestamos.TipoValor.IdTipoValor = this.ddlTipoValor.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoValor.SelectedValue);
            this.ctrCamposValores.IniciarControl(this.MiPrePrestamos, this.MiPrePrestamos.TipoValor, this.GestionControl);
            this.UpdatePanel3.Update();
        }

        protected void ddlPeriodoVencimiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlPeriodoVencimiento.SelectedValue))
            {
                this.MiPrePrestamos.PeriodoPrimerVencimiento = Convert.ToInt32(this.ddlPeriodoVencimiento.SelectedValue);

                if (this.MiPrePrestamos.PrestamosCuotas.Count > 0)
                {
                    DateTime fechaPeriodo = new DateTime(Convert.ToInt32(this.MiPrePrestamos.PeriodoPrimerVencimiento.Value.ToString().Substring(0, 4)), Convert.ToInt32(this.MiPrePrestamos.PeriodoPrimerVencimiento.Value.ToString().Substring(4, 2)), 1);
                    foreach (PrePrestamosCuotas pc in this.MiPrePrestamos.PrestamosCuotas)
                    {
                        pc.CuotaFechaVencimiento = fechaPeriodo.AddMonths(Convert.ToInt32(pc.CuotaNumero)).AddDays(-1);
                        pc.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(pc, this.GestionControl);
                    }
                    AyudaProgramacion.CargarGrillaListas<PrePrestamosCuotas>(this.MiPrePrestamos.PrestamosCuotas, false, this.gvDatos, true);
                    this.upDetalleCuotas.Update();
                }
            }
        }

        #region Detalle Cuotas

        protected void gvDatosChkIncluir_CheckedChanged(object sender, EventArgs e)
        {
            bool incluir;
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                incluir = ((CheckBox)fila.FindControl("gvDatosChkIncluir")).Checked;
                this.MiPrePrestamos.PrestamosCuotas[fila.DataItemIndex].Incluir = incluir;
            }
            PrePrestamosF.PrestamosCalcularImporteCancelar(this.MiPrePrestamos, true);
            this.txtImporteCancelacion.Text = this.MiPrePrestamos.ImporteCancelacion.ToString("C2");
            this.upCancelacion.Update();
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if ((e.CommandName == "Sort"))
            //    return;

            //int index = Convert.ToInt32(e.CommandArgument);
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            //PrePrestamos prestamoAfiliado = this.MisPrestamosAfiliados[indiceColeccion];
            //string parametros = string.Format("?IdPrestamo={0}", prestamoAfiliado.IdPrestamo);
            //if (e.CommandName == Gestion.Modificar.ToString())
            //{
            //    string url = string.Concat("~/Modulos/Prestamos/PrestamosAfiliadosModificar.aspx", parametros);
            //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(url), true);
            //}
            //else if (e.CommandName == Gestion.Consultar.ToString())
            //{
            //    string url = string.Concat("~/Modulos/Prestamos/PrestamosAfiliadosConsultar.aspx", parametros);
            //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(url), true);
            //}
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (this.GestionControl == Gestion.Cancelar)
                {
                    PrePrestamosCuotas cuota = (PrePrestamosCuotas)e.Row.DataItem;
                    if (cuota.Estado.IdEstado == (int)EstadosCuotas.Activa)
                    {
                        CheckBox chk = ((CheckBox)e.Row.FindControl("gvDatosChkIncluir"));
                        //chk.Enabled = true;
                    }
                }
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MiPrePrestamos.PrestamosCuotas;
            this.gvDatos.DataBind();
            this.upDetalleCuotas.Update();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MiPrePrestamos.PrestamosCuotas = this.OrdenarGrillaDatos<PrePrestamosCuotas>(this.MiPrePrestamos.PrestamosCuotas, e);
            this.gvDatos.DataSource = this.MiPrePrestamos.PrestamosCuotas;
            this.gvDatos.DataBind();
            this.upDetalleCuotas.Update();
        }

        #endregion

        #region Cancelaciones de Prestamos
        private void CargarCancelaciones()
        {
            if (MiPrePrestamos.TipoOperacion.IdTipoOperacion != (int)EnumTGETiposOperaciones.PrestamosCortoPlazo
                && MiPrePrestamos.TipoOperacion.IdTipoOperacion != (int)EnumTGETiposOperaciones.CompraDeCheque
                && MiPrePrestamos.TipoOperacion.IdTipoOperacion != (int)EnumTGETiposOperaciones.PrestamosManual)
            {
                MisPrestamosCancelacionesPendientes = PrePrestamosF.PrestamosObtenerPorAfiliadoCancelacion(MiPrePrestamos);
                PrePrestamos preExiste;
                foreach (PrePrestamos can in MisPrestamosCancelacionesPendientes)
                {
                    if (MiPrePrestamos.Cancelaciones.Exists(x => x.IdPrestamo == can.IdPrestamo))
                    {
                        preExiste = MiPrePrestamos.Cancelaciones.Find(x => x.IdPrestamo == can.IdPrestamo);
                        AyudaProgramacionLN.MatchObjectProperties(preExiste, can);
                        can.PrestamosCuotas = preExiste.PrestamosCuotas;
                    }
                }
                var aux = MisPrestamosCancelacionesPendientes.Find(x => x.Incluir == true);
                if (MisPrestamosCancelacionesPendientes.Count > 0 && aux != null)
                {

                    MiPrePrestamos.Cancelaciones = new List<PrePrestamos>();
                    MiPrePrestamos.Cancelaciones = MisPrestamosCancelacionesPendientes;
                    //this.MiPrePrestamos.ImporteCancelaciones = this.MisPrestamosCancelacionesPendientes.Where(x => x.Incluir).Sum(x => x.ImporteCancelacion);
                    txtImporteCancelaciones.Text = MisPrestamosCancelacionesPendientes.Where(x => x.Incluir).Sum(x => x.ImporteCancelacion).ToString("C2");// this.MiPrePrestamos.ImporteCancelaciones.ToString("C2");
                }
                tpCancelaciones.Visible = MisPrestamosCancelacionesPendientes.Count > 0;
                AyudaProgramacion.CargarGrillaListas<PrePrestamos>(MiPrePrestamos.Cancelaciones, false, gvCancelaciones, true);

            }
        }

        private bool ObtenerImporteCancelacion(PrePrestamos pParametro)
        {
            var aux = pParametro.Cancelaciones.Where(x => x.Incluir).ToList();
            if (aux.Count > 0)
            {
                pParametro.ImporteCancelaciones = pParametro.Cancelaciones.Sum(x => x.ImporteCancelacion);
                return true;
            }
            return false;
        }

        private void ObtenerCancelacionesDescontar()
        {
            if (this.MisPrestamosCancelacionesPendientes != null
                && this.MisPrestamosCancelacionesPendientes.Count > 0)
            {
                this.MiPrePrestamos.Cancelaciones = this.MiPrePrestamos.Cancelaciones.Where(x => x.Incluir).ToList();
                //PrePrestamos cancela;
                //CheckBox incluir;
                //decimal importe;
                //foreach (GridViewRow fila in this.gvCancelaciones.Rows)
                //{
                //    if (fila.RowType == DataControlRowType.DataRow)
                //    {
                //        cancela = this.MisPrestamosCancelacionesPendientes[fila.DataItemIndex];
                //        cancela.Incluir = false;
                //        importe = ((CurrencyTextBox)fila.FindControl("txtImporteCancelacion")).Decimal;
                //        if (importe != cancela.ImporteCancelacion)
                //        {
                //            cancela.ModificaImporteCancelacion = true;
                //            cancela.ImporteCancelacion = importe;
                //        }
                //        incluir = (CheckBox)fila.FindControl("chkIncluir");
                //        if (incluir.Checked)
                //        {
                //            cancela.Incluir = true;
                //            cancela.EstadoColeccion = EstadoColecciones.Agregado;
                //            cancela.CantidadCuotasCanceladas = cancela.PrestamosCuotas.Count(x => x.Incluir);
                //            this.MiPrePrestamos.Cancelaciones.Add(cancela);
                //        }
                //    }
                //}
                this.MiPrePrestamos.ImporteCancelaciones = this.MiPrePrestamos.Cancelaciones.Sum(x => x.ImporteCancelacion);
            }
        }

        protected void chkIncluir_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = ((CheckBox)sender);
            GridViewRow Row = ((GridViewRow)((Control)sender).Parent.Parent);
            this.MiIndiceGvCancelaciones = Row.DataItemIndex;

            if (chk.Checked)
            {
                PrePrestamos cancelacion = this.MiPrePrestamos.Cancelaciones[this.MiIndiceGvCancelaciones];
                cancelacion.FechaCancelacion = this.txtFechaAlta.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(this.txtFechaAlta.Text);
                cancelacion = PrePrestamosF.PrestamosObtenerDatosCompletos(cancelacion);

                foreach (PrePrestamosCuotas cuota in cancelacion.PrestamosCuotas)
                {
                    if (cuota.Estado.IdEstado == (int)EstadosCuentasCorrientes.EnviadoAlCobro
                            || cuota.Estado.IdEstado == (int)EstadosCuentasCorrientes.CobroDevuelto
                            || cuota.Estado.IdEstado == (int)EstadosCuentasCorrientes.CobroParcial
                            || cuota.Estado.IdEstado == (int)EstadosCuentasCorrientes.Rechazado)
                    {
                        cuota.Estado.IdEstado = (int)EstadosCuotas.Activa;
                    }

                }

                if (this.GestionControl == Gestion.Consultar)
                    cancelacion.PrestamosCuotas.RemoveAll(x => x.Incluir == false);


                this.ctrPrestamosCuotasPopUp.IniciarControl(cancelacion, this.GestionControl, MiPrePrestamos);
                //if (cancelacion.ObtenerCuotasPendientes(false).Count > 1)
                //{
                //    ((CheckBox)sender).Checked = cancelacion.PrestamosCuotas.Exists(x => x.Incluir);
                //    this.ctrPrestamosCuotasPopUp.IniciarControl(cancelacion, this.GestionControl);
                //}
                //else
                //{
                //    cancelacion.PrestamosCuotas.Find(x => x.Estado.IdEstado == (int)EstadosCuotas.Activa || x.Estado.IdEstado == (int)EstadosCuotas.CobradaParcial).Incluir = ((CheckBox)sender).Checked;
                //    this.ctrPrestamosCuotasPopUp_PrestamosCuotasSeleccionar(cancelacion);                   
                //}
            }
            else
            {
                this.MiPrePrestamos.Cancelaciones[this.MiIndiceGvCancelaciones].PrestamosCuotas.ForEach(x => x.Incluir = false);
                this.MiPrePrestamos.Cancelaciones[this.MiIndiceGvCancelaciones].ImporteCancelacion = 0;
                TextBox importeCancelacion = (TextBox)this.gvCancelaciones.Rows[this.MiIndiceGvCancelaciones].FindControl("txtImporteCancelacion");
                importeCancelacion.Text = this.MiPrePrestamos.Cancelaciones[this.MiIndiceGvCancelaciones].ImporteCancelacion.ToString("C2");
                ScriptManager.RegisterStartupScript(this.upCancelaciones, this.upCancelaciones.GetType(), "CalcularCancelacionesScript", "CalcularCancelaciones();", true);
                /*Para poder enviar las cancelaciones que ya estaban y en LN filtrar la actualizacion del prestamo*/
                MiPrePrestamos.Cancelaciones[MiIndiceGvCancelaciones].IncluirOriginal = false;
                this.upCancelaciones.Update();
            }
        }

        void ctrPrestamosCuotasPopUp_PrestamosCuotasSeleccionar(PrePrestamos e)
        {
            this.MiPrePrestamos.Cancelaciones[this.MiIndiceGvCancelaciones] = e;
            bool incluir = this.MiPrePrestamos.Cancelaciones[this.MiIndiceGvCancelaciones].PrestamosCuotas.Exists(x => x.Incluir);
            this.MiPrePrestamos.Cancelaciones[this.MiIndiceGvCancelaciones].Incluir = incluir;
            this.MiPrePrestamos.Cancelaciones[this.MiIndiceGvCancelaciones].IncluirOriginal = incluir;
            PrePrestamosF.PrestamosCalcularImporteCancelar(this.MiPrePrestamos.Cancelaciones[this.MiIndiceGvCancelaciones], true);
            this.MiPrePrestamos.Cancelaciones[this.MiIndiceGvCancelaciones].CantidadCuotasCanceladas = this.MiPrePrestamos.Cancelaciones[this.MiIndiceGvCancelaciones].PrestamosCuotas.Count(x => x.Incluir);

            ((CheckBox)this.gvCancelaciones.Rows[this.MiIndiceGvCancelaciones].FindControl("chkIncluir")).Checked = incluir;
            TextBox importeCancelacion = (TextBox)this.gvCancelaciones.Rows[this.MiIndiceGvCancelaciones].FindControl("txtImporteCancelacion");
            importeCancelacion.Text = this.MiPrePrestamos.Cancelaciones[this.MiIndiceGvCancelaciones].ImporteCancelacion.ToString("C2");
            //importeCancelacion.Enabled = incluir;

            Label lblCantidadCuotasCanceladas = (Label)this.gvCancelaciones.Rows[this.MiIndiceGvCancelaciones].FindControl("lblCantidadCuotasCanceladas");
            lblCantidadCuotasCanceladas.Text = this.MiPrePrestamos.Cancelaciones[this.MiIndiceGvCancelaciones].PrestamosCuotas.Count(x => x.Incluir).ToString();

            Label lblImporteTotal = (Label)this.gvCancelaciones.FooterRow.FindControl("lblImporteTotal");
            lblImporteTotal.Text = this.MiPrePrestamos.Cancelaciones.Where(x => x.Incluir).Sum(x => x.ImporteCancelacion).ToString("C2");

            //this.upCancelaciones.Update();

            ScriptManager.RegisterStartupScript(this.upCancelaciones, this.upCancelaciones.GetType(), "CalcularCancelacionesScript", "CalcularCancelaciones();", true);
            this.upCancelaciones.Update();
        }

        protected void gvCancelaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                PrePrestamos cancelacion = (PrePrestamos)e.Row.DataItem;
                if (this.GestionControl == Gestion.Agregar)
                {
                    CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluir");
                    ibtnConsultar.Visible = true;
                    ibtnConsultar.Attributes.Add("onchange", "CalcularCancelaciones();");
                    TextBox txtImporteCancelacion = (TextBox)e.Row.FindControl("txtImporteCancelacion");
                    txtImporteCancelacion.Attributes.Add("onchange", "CalcularCancelaciones();");
                }
                else if (this.GestionControl == Gestion.ConfirmarAgregar)
                {
                    CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluir");
                    ibtnConsultar.Visible = true;
                    ibtnConsultar.Enabled = false;
                }
                else if (this.GestionControl == Gestion.Consultar)
                {
                    CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluir");
                    ibtnConsultar.Visible = true;
                    ibtnConsultar.Enabled = true;
                    ibtnConsultar.Checked = false;
                    ibtnConsultar.Text = "Detalle";
                }
                else if (GestionControl == Gestion.Modificar && MiPrePrestamos.Estado.IdEstado != (int)EstadosPrestamos.Autorizado)
                {
                    CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluir");
                    ibtnConsultar.Visible = true;
                    ibtnConsultar.Attributes.Add("onchange", "CalcularCancelaciones();");
                    TextBox txtImporteCancelacion = (TextBox)e.Row.FindControl("txtImporteCancelacion");
                    txtImporteCancelacion.Attributes.Add("onchange", "CalcularCancelaciones();");
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblImporteTotal = (Label)e.Row.FindControl("lblImporteTotal");
                if (this.GestionControl == Gestion.Agregar || this.GestionControl == Gestion.ConfirmarAgregar)
                    lblImporteTotal.Text = this.MiPrePrestamos.Cancelaciones.Where(x => x.Incluir).Sum(x => x.ImporteCancelacion).ToString("C2");
                else
                    lblImporteTotal.Text = this.MiPrePrestamos.ImporteCancelaciones.ToString("C2");
            }
            //if(e.Row.RowType == DataControlRowType.Header)
            //{

            //}
        }

        protected void gvCancelaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //AhoCuentasMovimientos parametros = this.BusquedaParametrosObtenerValor<AhoCuentasMovimientos>();
            //parametros.IndiceColeccion = e.NewPageIndex;
            //this.BusquedaParametrosGuardarValor<AhoCuentasMovimientos>(parametros);

            this.gvCancelaciones.PageIndex = e.NewPageIndex;
            this.gvCancelaciones.DataSource = this.MiPrePrestamos.Cancelaciones;
            this.gvCancelaciones.DataBind();
        }

        #endregion

        #region Cargos Pendientes

        private void ObtenerCargosDescontar()
        {
            if (this.MisCargosPendientes.Count > 0)
            {
                this.MiPrePrestamos.CargosExcedidos = new List<CarCuentasCorrientes>();
                CarCuentasCorrientes cuentaCte;
                CheckBox incluir;
                foreach (GridViewRow fila in this.gvCuentaCorriente.Rows)
                {
                    if (fila.RowType == DataControlRowType.DataRow)
                    {
                        cuentaCte = this.MisCargosPendientes[fila.DataItemIndex];
                        cuentaCte.Incluir = false;
                        incluir = (CheckBox)fila.FindControl("chkIncluir");
                        if (incluir.Checked)
                        {
                            cuentaCte.Incluir = true;
                            cuentaCte.EstadoColeccion = EstadoColecciones.Agregado;
                            this.MiPrePrestamos.CargosExcedidos.Add(cuentaCte);
                        }
                    }
                }
                this.MiPrePrestamos.ImporteExcedido = this.MiPrePrestamos.CargosExcedidos.Sum(x => x.Importe);
            }
        }

        protected void gvCuentaCorriente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CarCuentasCorrientes cargo = (CarCuentasCorrientes)e.Row.DataItem;
                if (this.GestionControl == Gestion.Agregar)
                {
                    CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluir");
                    ibtnConsultar.Visible = true;
                    ibtnConsultar.Attributes.Add("onchange", "CalcularItem();");

                    if (cargo.Periodo < AyudaProgramacion.ObtenerPeriodo(DateTime.Now)
                    || cargo.TipoCargo.IdTipoCargo == (int)EnumTiposCargos.DescuentosPendientes)
                    {
                        //ibtnConsultar.Enabled = false;
                    }
                }
                else if (this.GestionControl == Gestion.ConfirmarAgregar)
                {
                    CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluir");
                    ibtnConsultar.Visible = true;
                    ibtnConsultar.Enabled = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblImporteTotal = (Label)e.Row.FindControl("lblImporteTotal");
                lblImporteTotal.Text = this.MiPrePrestamos.ImporteExcedido.ToString("C2");
            }
        }

        protected void gvCuentaCorriente_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //AhoCuentasMovimientos parametros = this.BusquedaParametrosObtenerValor<AhoCuentasMovimientos>();
            //parametros.IndiceColeccion = e.NewPageIndex;
            //this.BusquedaParametrosGuardarValor<AhoCuentasMovimientos>(parametros);

            this.gvCuentaCorriente.PageIndex = e.NewPageIndex;
            this.gvCuentaCorriente.DataSource = this.MiPrePrestamos.CargosExcedidos;
            this.gvCuentaCorriente.DataBind();
        }

        #endregion

        #region Solicitudes Pagos

        private void ObtenerSolicitudesPagosDescontar()
        {
            if (this.MisSolicitudesPendientes.Count > 0)
            {
                this.MiPrePrestamos.SolicitudesPagos = new List<CapSolicitudPago>();
                CapSolicitudPago solPago;
                CheckBox incluir;
                foreach (GridViewRow fila in this.gvSolicitudesPagos.Rows)
                {
                    if (fila.RowType == DataControlRowType.DataRow)
                    {
                        solPago = this.MisSolicitudesPendientes[fila.DataItemIndex];
                        solPago.IncluirEnOP = false;
                        incluir = (CheckBox)fila.FindControl("chkIncluir");
                        if (incluir.Checked)
                        {
                            solPago.IncluirEnOP = true;
                            solPago.EstadoColeccion = EstadoColecciones.Agregado;
                            this.MiPrePrestamos.SolicitudesPagos.Add(solPago);
                        }
                    }
                }
                this.MiPrePrestamos.ImporteSolicitudesPagos = this.MiPrePrestamos.SolicitudesPagos.Sum(x => x.ImporteTotal);
            }
        }

        protected void gvSolicitudesPagos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CapSolicitudPago solPago = (CapSolicitudPago)e.Row.DataItem;
                if (this.GestionControl == Gestion.Agregar)
                {
                    CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluir");
                    ibtnConsultar.Visible = true;
                    ibtnConsultar.Attributes.Add("onchange", "CalcularSolicitudPago();");

                }
                else if (this.GestionControl == Gestion.ConfirmarAgregar)
                {
                    CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluir");
                    ibtnConsultar.Visible = true;
                    ibtnConsultar.Enabled = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblImporteTotal = (Label)e.Row.FindControl("lblImporteTotal");
                lblImporteTotal.Text = this.MiPrePrestamos.ImporteSolicitudesPagos.ToString("C2");
            }
        }

        protected void gvSolicitudesPagos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //AhoCuentasMovimientos parametros = this.BusquedaParametrosObtenerValor<AhoCuentasMovimientos>();
            //parametros.IndiceColeccion = e.NewPageIndex;
            //this.BusquedaParametrosGuardarValor<AhoCuentasMovimientos>(parametros);

            this.gvSolicitudesPagos.PageIndex = e.NewPageIndex;
            this.gvSolicitudesPagos.DataSource = this.MiPrePrestamos.SolicitudesPagos;
            this.gvSolicitudesPagos.DataBind();
        }

        #endregion

        #region Documentos Asociados

        protected void gvDocumentosAsociados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);

            int IdTipoOperacion = Convert.ToInt32(((HiddenField)this.gvDocumentosAsociados.Rows[index].FindControl("hdfIdTipoOperacion")).Value);
            int IdRefTipoOperacion = Convert.ToInt32(((HiddenField)this.gvDocumentosAsociados.Rows[index].FindControl("hdfIdRefTipoOperacion")).Value);
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable();
            //Filtro para Obtener URL y NombreParametro
            Menues filtroMenu = new Menues();
            filtroMenu.IdTipoOperacion = IdTipoOperacion;
            //Control de Tipo de Menues (SOLO CONSULTA)
            if (e.CommandName == Gestion.Consultar.ToString())
                filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Consulta;

            //Guardo Menu devuelto de la DB
            filtroMenu = SeguridadF.MenuesObtenerPorOperacionTipoMenu(filtroMenu);
            this.MisParametrosUrl.Add(filtroMenu.NombreParametro, IdRefTipoOperacion);
            this.MisParametrosUrl.Add("IdAfiliado", this.MiPrePrestamos.Afiliado.IdAfiliado);
            this.MisParametrosUrl.Add("IdPrestamo", this.MiPrePrestamos.IdPrestamo);
            this.MisParametrosUrl.Add("tcDatosTabIndex", this.tcDatos.ActiveTabIndex);
            //Si devuelve una URL Redirecciona si no muestra mensaje error
            if (filtroMenu.URL.Length != 0)
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(string.Concat("~/", filtroMenu.URL)), true);
            }
            else
            {
                this.MostrarMensaje("ErrorURLNoValida", true);
            }

        }

        protected void gvDocumentosAsociados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                //consultar.Visible = this.ValidarPermiso("LibroMayorListar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

            }
        }

        #endregion
    }
}