using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bancos.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Tesorerias.Entidades;
using Bancos;
using System.Web.UI.HtmlControls;
using Evol.Controls;
using Ahorros.Entidades;
using Ahorros;

namespace IU.Modulos.Tesoreria.Controles
{
    public partial class CajasIngresosValores : ControlesSeguros
    {
        private TESCajasMovimientos  MiCajaMovimiento
        {
            get { return (TESCajasMovimientos)Session[this.MiSessionPagina + "CajasIngresosValoresMiCajaMovimiento"]; }
            set { Session[this.MiSessionPagina + "CajasIngresosValoresMiCajaMovimiento"] = value; }
        }

        private TGETiposOperaciones MiTipoOperacion
        {
            get { return (TGETiposOperaciones)Session[this.MiSessionPagina + "CajasIngresosValoreMiTipoOperacion"]; }
            set { Session[this.MiSessionPagina + "CajasIngresosValoreMiTipoOperacion"] = value; }
        }

        private List<TESBancosCuentas> MisBancosCuentasTransferencias
        {
            get { return (List<TESBancosCuentas>)Session[this.MiSessionPagina + "CajasIngresosValoreMisBancosCuentasTransferencias"]; }
            set { Session[this.MiSessionPagina + "CajasIngresosValoreMisBancosCuentasTransferencias"] = value; }
        }

        private List<AhoCuentas> MisCajasAhorros
        {
            get { return (List<AhoCuentas>)Session[this.MiSessionPagina + "CajasIngresosValoreMisCajasAhorros"]; }
            set { Session[this.MiSessionPagina + "CajasIngresosValoreMisCajasAhorros"] = value; }
        }

        private bool MiHabilitar
        {
            get { return (bool)Session[this.MiSessionPagina + "CajasIngresosValoreMiHabilitar"]; }
            set { Session[this.MiSessionPagina + "CajasIngresosValoreMiHabilitar"] = value; }
        }

        private decimal ValidarImporteEfectivo
        {
            get { return (decimal)Session[this.MiSessionPagina + "OrdenesDeCobroDatosValidarImporteEfectivo"]; }
            set { Session[this.MiSessionPagina + "OrdenesDeCobroDatosValidarImporteEfectivo"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrBuscarCheque.ChequesBuscarSeleccionar += new IU.Modulos.Tesoreria.Controles.ChequesTercerosPopUp.ChequesBuscarEventHandler(ctrBuscarChequesPopUp_ChequesTercerosSeleccionar);
            if (this.IsPostBack)
            {
                TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.AlertaParaIngresoEfectivo);
                decimal dValor = 0;
                decimal.TryParse(valor.ParametroValor, out dValor);
                ValidarImporteEfectivo = dValor;
            }
        }

        public void IniciarControl(TESCajasMovimientos pCajaMovimiento, Gestion pGestion, TGETiposOperaciones pTipoOperacion, TGESectores pSector, bool pHabilitar)
        {
            this.gvFormasCobros.Columns[1].Visible = false;
            this.gvFormasCobros.Columns[2].Visible = false;
            switch (pGestion)
            {
                case Gestion.Consultar:
                    this.gvFormasCobros.Columns[1].Visible = true;
                    this.gvFormasCobros.Columns[2].Visible = true;
                    this.btnIngresarCobro.Visible = false;
                    this.txtImporte.Visible = false;
                    this.ddlTiposValores.Visible = false;
                    this.lblFormaCobro.Visible = false;
                    this.lblImporte.Visible = false;
                    break;
                default:
                    break;
            }
            this.GestionControl = pGestion;
            this.MiCajaMovimiento = pCajaMovimiento;
            this.MiTipoOperacion = pTipoOperacion;                      
            this.MiHabilitar = pHabilitar;
            this.pnlFormasCobros.Visible = this.MiHabilitar;
            this.pnlDetalle.Visible = true;

            this.gvFormasCobros.DataSource = null;
            this.gvFormasCobros.DataBind();
            this.gvCheques.DataSource = null;
            this.gvCheques.DataBind();
            this.gvTarjetas.DataSource = null;
            this.gvTarjetas.DataBind();
            this.gvTransferencias.DataSource = null;
            this.gvTransferencias.DataBind();
            this.gvChequesTerceros.DataSource = null;
            this.gvChequesTerceros.DataBind();
            
            
            AyudaProgramacion.CargarGrillaListas<TESCajasMovimientosValores>(this.MiCajaMovimiento.CajasMovimientosValores, false, this.gvFormasCobros, true);
            TESCajasMovimientosValores valor;
            //CHEQUES
            if (this.MiCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque))
            {
                valor = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque);
                AyudaProgramacion.CargarGrillaListas<TESCheques>(valor.Cheques, false, this.gvCheques, false);
                this.pnlDetalleValores.Visible = true; 
            }
            if (this.MiCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero))
            {
                valor = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero);
                AyudaProgramacion.CargarGrillaListas<TESCheques>(valor.Cheques, false, this.gvChequesTerceros, false);
                this.pnlDetalleChequesTerceros.Visible = true;
            }
            if (this.MiCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia))
            {
                valor = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia);
                AyudaProgramacion.CargarGrillaListas<TESBancosCuentasMovimientos>(valor.BancosCuentasMovimientos, false, this.gvTransferencias, false);
                this.pnlDetalleTransferencia.Visible = true;
            }
            if (pTipoOperacion.TipoMovimiento.IdTipoMovimiento==(int)EnumTGETiposMovimientos.Debito
                && this.MiCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito))
            {
                valor = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito);
                AyudaProgramacion.CargarGrillaListas<TESBancosCuentasMovimientos>(valor.BancosCuentasMovimientos, false, this.gvTarjetasPropias, false);
                this.pnlDetalleTarjetasPropias.Visible = true;
            }

            if (pHabilitar)
            {
                List<TGETiposValores> listaValores = TGEGeneralesF.TiposValoresObtenerLista();

                switch (pTipoOperacion.TipoMovimiento.IdTipoMovimiento)
                {
                    case (int)EnumTGETiposMovimientos.Credito:
                        if (listaValores.Exists(x => x.IdTipoValor == (int)EnumTiposValores.Cheque))
                            listaValores.Remove(listaValores.Find(x => x.IdTipoValor == (int)EnumTiposValores.Cheque));
                        break;
                    case (int)EnumTGETiposMovimientos.Debito:
                        if (listaValores.Exists(x => x.IdTipoValor == (int)EnumTiposValores.TarjetaCredito))
                            listaValores.Remove(listaValores.Find(x => x.IdTipoValor == (int)EnumTiposValores.TarjetaCredito));
                        break;
                    default:
                        break;
                }
                this.ddlTiposValores.DataSource = AyudaProgramacion.AcomodarIndices<TGETiposValores>(listaValores).ToList();
                this.ddlTiposValores.DataValueField = "IdTipoValor";
                this.ddlTiposValores.DataTextField = "TipoValor";
                this.ddlTiposValores.DataBind();

                if (this.ddlTiposValores.Items.Count == 0 || this.ddlTiposValores.Items.Count > 1)
                {
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlTiposValores, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                }                
                    this.ddlTiposValores_SelectedIndexChanged(null, EventArgs.Empty);

            }
        }

        public void ActualizarUpdatePanel()
        {
            this.UpdatePanel2.Update();
           
        }

        private void HabilitarChequesTerceros()
        {
            this.pnlChequesTerceros.Visible = true;

            TESBancos banco = new TESBancos();
            banco.Estado.IdEstado = (int)Estados.Activo;
            this.ddlBancos.DataSource = BancosF.BancosObtenerListaFiltro(banco);
            this.ddlBancos.DataValueField = "IdBanco";
            this.ddlBancos.DataTextField = "Descripcion";
            this.ddlBancos.DataBind();

            this.rfvFecha.Enabled = true;
            this.rfvChequeDiferido.Enabled = true;
            this.rfvTitular.Enabled = true;
            this.rfvCUIT.Enabled = true;
            this.rfvBancos.Enabled = true;
            this.rfvNumeroCheque.Enabled = true;
        }

        private void HabilitarTransferencias()
        {
            this.pnlTransferencias.Visible = true;

            TESBancosCuentas bancoCuenta = new TESBancosCuentas();
            bancoCuenta.Estado.IdEstado = (int)Estados.Activo;
            bancoCuenta.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
            bancoCuenta.Moneda = MiCajaMovimiento.CajaMoneda.Moneda;
            bancoCuenta.Filtro = ddlTiposValores.SelectedValue;
            this.MisBancosCuentasTransferencias = BancosF.BancosCuentasObtenerListaFiltroTransferencia(bancoCuenta);
            this.ddlBancosCuentasTransferencias.DataSource = this.MisBancosCuentasTransferencias;
            this.ddlBancosCuentasTransferencias.DataValueField = "IdBancoCuenta";
            this.ddlBancosCuentasTransferencias.DataTextField = "DescripcionFilialBancoTipoCuentaNumero";
            this.ddlBancosCuentasTransferencias.DataBind();

            this.rfvFechaTransferencia.Enabled = true;
            //this.rfvChequeDiferido.Enabled = true;
            this.rfvBancosCuentasTransferencias.Enabled = true;

            this.ctrCamposValoresTransferencias.BorrarControlesParametros();
            this.ctrCamposValoresTransferencias.IniciarControl(new TESBancosCuentasMovimientos(), MiCajaMovimiento.TipoOperacion, this.GestionControl);
        }

        private void HabilitarChequesPropios()
        {
            TESBancosCuentas bancoCuenta = new TESBancosCuentas();
            bancoCuenta.Estado.IdEstado = (int)Estados.Activo;
            bancoCuenta.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
            bancoCuenta.Filtro = this.ddlTiposValores.SelectedValue;
            this.ddlBancosCuentasCheques.DataSource = BancosF.BancosCuentasObtenerListaFiltro(bancoCuenta);
            this.ddlBancosCuentasCheques.DataValueField = "IdBancoCuenta";
            this.ddlBancosCuentasCheques.DataTextField = "DescripcionFilialBancoTipoCuentaNumero";
            this.ddlBancosCuentasCheques.DataBind();

            this.rfvFecha.Enabled = true;
            this.rfvChequeDiferido.Enabled = true;
            this.rfvTitular.Enabled = true;
            this.rfvCUIT.Enabled = true;
            this.rfvBancosCuentasCheques.Enabled = true;
        }

        private void HabilitarTarjetas()
        {
            this.pnlTarjetasCreditos.Visible = true;

            //CARGAR COMBO TARJETAS CREDITO
            this.ddlTarjetas.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TarjetasCredito);
            this.ddlTarjetas.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTarjetas.DataTextField = "Descripcion";
            this.ddlTarjetas.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTarjetas, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.txtFechaTransaccion.Text = DateTime.Now.ToShortDateString();
            this.rfvTarjetas.Enabled = true;
            this.rfvVencimientoMM.Enabled = true;
            this.rfvVencimientoAA.Enabled = true;
            this.rfvPosnet.Enabled = true;
            this.rfvCuotas.Enabled = true;
            this.rfvNumeroLote.Enabled = true;
        }

        private void HabilitarCajaAhorro()
        {
            AhoCuentas cuenta = new AhoCuentas();
            cuenta.Estado.IdEstado = (int)EstadosAhorrosCuentas.CuentaAbierta;
            cuenta.Afiliado.IdAfiliado = this.MiCajaMovimiento.Afiliado.IdAfiliado;
            cuenta.Moneda.IdMoneda = this.MiCajaMovimiento.CajaMoneda.Moneda.IdMoneda;
            this.MisCajasAhorros = AhorroF.CuentasObtenerListaFiltro(cuenta);
            this.ddlCuentaBancariaCajaAhorro.DataSource = MisCajasAhorros;
            this.ddlCuentaBancariaCajaAhorro.DataValueField = "IdCuenta";
            this.ddlCuentaBancariaCajaAhorro.DataTextField = "CuentaDatos";
            this.ddlCuentaBancariaCajaAhorro.DataBind();
            //if (MisCajasAhorros.Count!=1)
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCuentaBancariaCajaAhorro, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        public TESCajasMovimientos ObtenerCajaMovimiento()
        {
            this.PersistirDatosGrilla();
            return this.MiCajaMovimiento;
        }

        protected void ddlTiposValores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ddlTiposValores.SelectedValue))
            {
                pnlCheques.Visible = false;
                pnlChequesPropios.Visible = false;
                pnlChequesTerceros.Visible = false;
      
                pnlDetalleChequesTerceros.Visible = false;
                pnlDetalleIngresos.Visible = true;
                pnlDetalleTarjetas.Visible = false;
                pnlDetalleTarjetasPropias.Visible = false;
                pnlDetalleTransferencia.Visible = false;
                pnlDetalleValores.Visible = false;
            
                pnlTarjetasCreditos.Visible = false;
                pnlTransferencias.Visible = false;
                pnlCajaAhorro.Visible = false;
                return;
            }
            this.rfvBancos.Enabled = false;
            this.rfvBancosCuentasCheques.Enabled = false;
            this.rfvBancosCuentasTransferencias.Enabled = false;
            this.rfvChequeDiferido.Enabled = false;
            this.rfvCUIT.Enabled = false;
            this.rfvCuotas.Enabled = false;
            this.rfvFecha.Enabled = false;
            this.rfvFechaTransferencia.Enabled = false;
            this.rfvNumeroCheque.Enabled = false;
            this.rfvNumeroLote.Enabled = false;
            this.rfvPosnet.Enabled = false;
            this.rfvTarjetas.Enabled = false;
            this.rfvTitular.Enabled = false;
            this.rfvVencimientoAA.Enabled = false;
            this.rfvVencimientoMM.Enabled = false;

            TGETiposValores tipoValor = new TGETiposValores();
            tipoValor.IdTipoValor = Convert.ToInt32(this.ddlTiposValores.SelectedValue);

            this.btnIngresarCobro.Visible = true;
            this.lblImporte.Visible = true;
            this.txtImporte.Visible = true;
            this.rfvImporte.Enabled = true;

            this.btnAgregarCheque.Visible = false;
            this.pnlCheques.Visible = false;
            this.pnlChequesTerceros.Visible = false;
            this.pnlChequesPropios.Visible = false;
            this.pnlTransferencias.Visible = false;
            this.pnlTarjetasCreditos.Visible = false;
            this.ddlBancos.Enabled = true;
            this.pnlCajaAhorro.Visible = false;

            switch (this.MiTipoOperacion.TipoMovimiento.IdTipoMovimiento)
            {
                case (int)EnumTGETiposMovimientos.Credito:
                    if (tipoValor.IdTipoValor == (int)EnumTiposValores.Cheque)
                        //|| tipoValor.IdTipoValor == (int)EnumTiposValores.ChequeBancoSol)
                    {
                        this.pnlCheques.Visible = true;
                        this.HabilitarChequesPropios();
                        //if (tipoValor.IdTipoValor == (int)EnumTiposValores.ChequeBancoSol)
                        //{
                        //    this.ddlBancos.Enabled = false;
                        //    this.ddlBancos.SelectedValue = ((int)EnumTesBancos.BancoSol).ToString();
                        //}
                    }
                    else if (tipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
                    {
                        this.pnlCheques.Visible = true;
                        this.pnlChequesTerceros.Visible = true;
                        this.HabilitarChequesTerceros();
                    }
                    else if (tipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia || tipoValor.IdTipoValor == (int)EnumTiposValores.EfectivoFondoFijo)
                    {
                        this.pnlTransferencias.Visible = true;
                        this.HabilitarTransferencias();
                    }
                    else if (tipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito)
                    {
                        this.pnlTarjetasCreditos.Visible = true;
                        this.HabilitarTarjetas();
                    }
                    else if (tipoValor.IdTipoValor == (int)EnumTiposValores.CajaAhorro)
                    {
                        this.pnlCajaAhorro.Visible = true;
                        this.HabilitarCajaAhorro();
                    }
                    break;
                case (int)EnumTGETiposMovimientos.Debito:
                    if (tipoValor.IdTipoValor == (int)EnumTiposValores.Cheque)
                    {
                        this.pnlCheques.Visible = true;
                        this.pnlChequesPropios.Visible = true;
                        this.HabilitarChequesPropios();
                    }
                    else if (tipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
                    {
                        this.btnIngresarCobro.Visible = false;
                        this.lblImporte.Visible = false;
                        this.txtImporte.Visible = false;
                        this.rfvImporte.Enabled = false;
                        this.btnAgregarCheque.Visible = true;
                    }
                    else if (tipoValor.IdTipoValor == (int)EnumTiposValores.EfectivoFondoFijo || tipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia)
                    {
                        this.pnlTransferencias.Visible = true;
                        this.HabilitarTransferencias();
                    }
                    else if (tipoValor.IdTipoValor == (int)EnumTiposValores.CajaAhorro)
                    {
                        this.pnlCajaAhorro.Visible = true;
                        this.HabilitarCajaAhorro();
                    }
                    break;
                default:
                    break;
            }
            this.ctrCamposValores.BorrarControlesParametros();
            this.ctrCamposValores.IniciarControl(new TESCajasMovimientosValores(), tipoValor, this.GestionControl, tipoValor.IdTipoValor);
            this.UpdatePanel2.Update();
        }

        protected void btnIngresarCobro_Click(object sender, EventArgs e)
        {
            this.Page.Validate("IngresarCobro");
            TGETiposValores tipoValor = new TGETiposValores();
            tipoValor.IdTipoValor = Convert.ToInt32(this.ddlTiposValores.SelectedValue);
            tipoValor.TipoValor = this.ddlTiposValores.SelectedItem.Text;
            if (tipoValor.IdTipoValor == (int)EnumTiposValores.Cheque
                || tipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
            {
                this.Page.Validate("IngresarCheque");
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito)
            {
                this.Page.Validate("IngresarTarjeta");
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia || tipoValor.IdTipoValor == (int)EnumTiposValores.EfectivoFondoFijo)
            {
                this.Page.Validate("IngresarTransferencia");
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.CajaAhorro)
            {
                this.Page.Validate("IngresarCajaAhorro");
            }
            if (!this.Page.IsValid)
                return;

            TESCajasMovimientosValores valor;
            //EFECTIVO
            if (tipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo)
            {
                if (this.MiCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo))
                {
                    valor = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo);
                    valor.Importe = ((CurrencyTextBox)this.txtImporte).Decimal;                    
                }
                else
                {
                    valor = new TESCajasMovimientosValores();
                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                    valor.TipoValor = tipoValor;
                    valor.Importe = this.txtImporte.Decimal;
                    valor.LoteCamposValores = this.ctrCamposValores.ObtenerListaCamposValores();
                    this.MiCajaMovimiento.CajasMovimientosValores.Add(valor);
                    valor.IndiceColeccion = this.MiCajaMovimiento.CajasMovimientosValores.IndexOf(valor);
                }
                valor.LoteCamposValores = this.ctrCamposValores.ObtenerListaCamposValores();
                if (ValidarImporteEfectivo > 0 && txtImporte.Decimal > ValidarImporteEfectivo)
                {
                    List<string> param = new List<string>();
                    param.Add(string.Concat(this.MiCajaMovimiento.CajaMoneda.Moneda.Moneda, " ", ValidarImporteEfectivo.ToString("N2")));
                    MostrarMensaje("ValidarAlertaIngresoEfectivo", true, param);
                }
            }//CHEQUE
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.Cheque)
            {
                if (!this.MiCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque))
                {
                    valor = new TESCajasMovimientosValores();
                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                    valor.TipoValor = tipoValor;
                    this.MiCajaMovimiento.CajasMovimientosValores.Add(valor);
                    valor.IndiceColeccion = this.MiCajaMovimiento.CajasMovimientosValores.IndexOf(valor);
                }
                valor = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque);
                valor.LoteCamposValores = this.ctrCamposValores.ObtenerListaCamposValores();
                TESCheques cheque = new TESCheques();
                cheque.EstadoColeccion = EstadoColecciones.Agregado;
                cheque.Estado.IdEstado = (int)EstadosCheques.Entregado;
                cheque.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                cheque.Fecha = this.txtFecha.Text == string.Empty ? default(DateTime) : Convert.ToDateTime(this.txtFecha.Text);
                cheque.FechaDiferido = this.txtFechaDiferido.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaDiferido.Text);
                cheque.Concepto = this.txtConcepto.Text;
                cheque.NumeroCheque = this.txtNumeroCheque.Text;
                cheque.Banco.IdBanco = this.ddlBancos.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlBancos.SelectedValue);
                cheque.Banco.Descripcion = this.ddlBancos.SelectedValue == string.Empty ? string.Empty : this.ddlBancos.SelectedItem.Text;
                cheque.TitularCheque = this.txtTitular.Text;
                cheque.CUIT = this.txtCUIT.Text;
                cheque.Importe = ((CurrencyTextBox)this.txtImporte).Decimal;
                cheque.IdBancoCuenta = this.ddlBancosCuentasCheques.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlBancosCuentasCheques.SelectedValue);
                cheque.ChequePropio = true;
                cheque.TipoOperacion = this.MiTipoOperacion;

                valor.Cheques.Add(cheque);
                cheque.IndiceColeccion = valor.Cheques.IndexOf(cheque);

                valor.Importe = valor.Cheques.Sum(x => x.Importe);
                AyudaProgramacion.CargarGrillaListas<TESCheques>(valor.Cheques, false, this.gvCheques, false);
                this.pnlDetalleValores.Visible = true;
                this.LimpiarControlesCheques();
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
            {
                if (!this.MiCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero))
                {
                    valor = new TESCajasMovimientosValores();
                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                    valor.TipoValor = tipoValor;
                    this.MiCajaMovimiento.CajasMovimientosValores.Add(valor);
                    valor.IndiceColeccion = this.MiCajaMovimiento.CajasMovimientosValores.IndexOf(valor);
                }

                valor = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero);
                valor.LoteCamposValores = this.ctrCamposValores.ObtenerListaCamposValores();
                TESCheques cheque = new TESCheques();
                cheque.EstadoColeccion = EstadoColecciones.Agregado;
                cheque.Estado.IdEstado = (int)EstadosCheques.EnCaja;
                cheque.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                cheque.Fecha = this.txtFecha.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(this.txtFecha.Text);
                cheque.FechaDiferido = this.txtFechaDiferido.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaDiferido.Text);
                cheque.Concepto = this.txtConcepto.Text;
                cheque.NumeroCheque = this.txtNumeroCheque.Text;
                cheque.Banco.IdBanco = this.ddlBancos.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlBancos.SelectedValue);
                cheque.Banco.Descripcion = this.ddlBancos.SelectedValue == string.Empty ? string.Empty : this.ddlBancos.SelectedItem.Text;
                cheque.TitularCheque = this.txtTitular.Text;
                cheque.CUIT = this.txtCUIT.Text;
                cheque.Importe = ((CurrencyTextBox)this.txtImporte).Decimal;
                cheque.IdBancoCuenta = this.ddlBancosCuentasCheques.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlBancosCuentasCheques.SelectedValue);
                cheque.ChequePropio = false;
                cheque.TipoOperacion = this.MiTipoOperacion;

                valor.Cheques.Add(cheque);
                cheque.IndiceColeccion = valor.Cheques.IndexOf(cheque);

                valor.Importe = valor.Cheques.Sum(x => x.Importe);
                //this.PersistirDatosGrilla();
                AyudaProgramacion.CargarGrillaListas<TESCheques>(valor.Cheques, false, this.gvChequesTerceros, false);
                this.pnlDetalleChequesTerceros.Visible = true;
                this.LimpiarControlesCheques();
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia)
            {
                if (!this.MiCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia))
                {
                    valor = new TESCajasMovimientosValores();
                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                    valor.TipoValor = tipoValor;
                    this.MiCajaMovimiento.CajasMovimientosValores.Add(valor);
                    valor.IndiceColeccion = this.MiCajaMovimiento.CajasMovimientosValores.IndexOf(valor);
                }

                valor = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia);
                valor.LoteCamposValores = this.ctrCamposValores.ObtenerListaCamposValores();
                TESBancosCuentasMovimientos bancoMovimiento = new TESBancosCuentasMovimientos();
                bancoMovimiento.BancoCuenta = this.MisBancosCuentasTransferencias[this.ddlBancosCuentasTransferencias.SelectedIndex];
                bancoMovimiento.Importe = ((CurrencyTextBox)this.txtImporte).Decimal;
                bancoMovimiento.TipoOperacion = this.MiTipoOperacion;
                bancoMovimiento.FechaAlta = DateTime.Now;
                bancoMovimiento.FechaMovimiento = this.txtFechaTransferencia.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(this.txtFechaTransferencia.Text); ;
                bancoMovimiento.FechaConfirmacionBanco = this.txtFechaTransferencia.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(this.txtFechaTransferencia.Text);
                bancoMovimiento.FechaConciliacion = DateTime.Now;
                bancoMovimiento.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.Confirmado;
                bancoMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
                bancoMovimiento.Detalle = this.MiCajaMovimiento.Afiliado.TipoDocumento.TipoDocumento + " " + this.MiCajaMovimiento.Afiliado.NumeroDocumento.ToString() + " " + this.MiCajaMovimiento.Afiliado.ApellidoNombre;
                bancoMovimiento.LoteCamposValores = this.ctrCamposValoresTransferencias.ObtenerListaCamposValores();
                foreach (TGECampos c in ctrCamposValoresTransferencias.ObtenerLista())
                    bancoMovimiento.Detalle = string.Concat(bancoMovimiento.Detalle, " - ", c.Titulo, ": ", c.CampoValor.ListaValor.Length > 0 ? c.CampoValor.ListaValor : c.CampoValor.Valor);
                valor.BancosCuentasMovimientos.Add(bancoMovimiento);
                valor.Importe = valor.BancosCuentasMovimientos.Sum(x => x.Importe);
                AyudaProgramacion.CargarGrillaListas<TESBancosCuentasMovimientos>(valor.BancosCuentasMovimientos, false, this.gvTransferencias, false);

                this.pnlDetalleTransferencia.Visible = true;
                this.LimpiarControlesCheques();
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.EfectivoFondoFijo)
            {
                if (!this.MiCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.EfectivoFondoFijo))
                {
                    valor = new TESCajasMovimientosValores();
                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                    valor.TipoValor = tipoValor;
                    this.MiCajaMovimiento.CajasMovimientosValores.Add(valor);
                    valor.IndiceColeccion = this.MiCajaMovimiento.CajasMovimientosValores.IndexOf(valor);
                }

                valor = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.EfectivoFondoFijo);

                TESBancosCuentasMovimientos bancoMovimiento = new TESBancosCuentasMovimientos();
                bancoMovimiento.BancoCuenta = this.MisBancosCuentasTransferencias[this.ddlBancosCuentasTransferencias.SelectedIndex];
                bancoMovimiento.Importe = ((CurrencyTextBox)this.txtImporte).Decimal;
                bancoMovimiento.TipoOperacion = this.MiTipoOperacion;
                bancoMovimiento.FechaAlta = DateTime.Now;
                bancoMovimiento.FechaMovimiento = this.txtFechaTransferencia.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(this.txtFechaTransferencia.Text); ;
                bancoMovimiento.FechaConfirmacionBanco = this.txtFechaTransferencia.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(this.txtFechaTransferencia.Text);
                bancoMovimiento.FechaConciliacion = DateTime.Now;
                bancoMovimiento.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.Confirmado;
                bancoMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
                bancoMovimiento.Detalle = this.MiCajaMovimiento.Afiliado.TipoDocumento.TipoDocumento + " " + this.MiCajaMovimiento.Afiliado.NumeroDocumento.ToString() + " " + this.MiCajaMovimiento.Afiliado.ApellidoNombre;

                valor.BancosCuentasMovimientos.Add(bancoMovimiento);
                valor.Importe = valor.BancosCuentasMovimientos.Sum(x => x.Importe);
                AyudaProgramacion.CargarGrillaListas<TESBancosCuentasMovimientos>(valor.BancosCuentasMovimientos, false, this.gvFondoFijo, false);

                this.pnlDetalleFondoFijo.Visible = true;
                this.LimpiarControlesCheques();
            }
            //TARJETAS CREDITOS TRANSACCIONES
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito)
            {
                if (!this.MiCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito))
                {
                    valor = new TESCajasMovimientosValores();
                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                    valor.TipoValor = tipoValor;
                    this.MiCajaMovimiento.CajasMovimientosValores.Add(valor);
                    valor.IndiceColeccion = this.MiCajaMovimiento.CajasMovimientosValores.IndexOf(valor);
                }

                valor = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito);
                
                TESTarjetasTransacciones tarjeta = new TESTarjetasTransacciones();
                tarjeta.EstadoColeccion = EstadoColecciones.Agregado;
                tarjeta.Estado.IdEstado = (int)Estados.Activo;
                tarjeta.Importe = ((CurrencyTextBox)this.txtImporte).Decimal;
                tarjeta.Fecha = DateTime.Now;
                tarjeta.FechaTransaccion = this.txtFechaTransaccion.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(this.txtFechaTransaccion.Text);
                tarjeta.IdTarjetaCredito = this.ddlTarjetas.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTarjetas.SelectedValue);
                tarjeta.Titular = this.txtTitularTarjeta.Text;
                tarjeta.NumeroTarjetaCredito = this.txtNumeroTarjeta.Text;
                tarjeta.Importe = ((CurrencyTextBox)this.txtImporte).Decimal;
                tarjeta.VencimientoAnio = txtVencimientoAA.Text == string.Empty ? 0 : Convert.ToInt32(this.txtVencimientoAA.Text);
                tarjeta.VencimientoMes = txtVencimientoMM.Text == string.Empty ? 0 : Convert.ToInt32(this.txtVencimientoMM.Text);
                tarjeta.NumeroTransaccionPosnet = this.txtPosnet.Text;
                tarjeta.CantidadCuotas = txtCuotas.Text == string.Empty ? 1 : Convert.ToInt32(this.txtCuotas.Text);
                tarjeta.Observaciones = this.txtObservaciones.Text;
                tarjeta.NumeroLote = this.txtNumeroLote.Text;
                tarjeta.TarjetaDescripcion = this.ddlTarjetas.SelectedItem.Text;
                valor.TarjetasTransacciones.Add(tarjeta);
                tarjeta.IndiceColeccion = valor.TarjetasTransacciones.IndexOf(tarjeta);
                

                valor.Importe = valor.TarjetasTransacciones.Sum(x => x.Importe);
                //this.PersistirDatosGrilla();
                AyudaProgramacion.CargarGrillaListas<TESTarjetasTransacciones>(valor.TarjetasTransacciones, false, this.gvTarjetas, false);
                this.pnlDetalleTarjetas.Visible = true;
                this.LimpiarControlesTarjetas();
            }
            //CAJA AHORRO
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.CajaAhorro)
            {

                if (MiCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.CajaAhorro))
                {
                    valor = MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.CajaAhorro);
                    valor.Importe = ((CurrencyTextBox)this.txtImporte).Decimal;
                }
                else
                {
                    valor = new TESCajasMovimientosValores();
                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                    valor.TipoValor = tipoValor;
                    valor.TipoValor.TipoValor = MisCajasAhorros[ddlCuentaBancariaCajaAhorro.SelectedIndex].CuentaDatos;
                    valor.Importe = ((CurrencyTextBox)txtImporte).Decimal;
                    valor.IdCuenta = MisCajasAhorros[ddlCuentaBancariaCajaAhorro.SelectedIndex].IdCuenta;
                    MiCajaMovimiento.CajasMovimientosValores.Add(valor);
                    valor.IndiceColeccion = MiCajaMovimiento.CajasMovimientosValores.IndexOf(valor);
                    pnlCajaAhorro.Visible = true;
                }
                valor.LoteCamposValores = this.ctrCamposValores.ObtenerListaCamposValores();
            }
            AyudaProgramacion.CargarGrillaListas<TESCajasMovimientosValores>(this.MiCajaMovimiento.CajasMovimientosValores, false, this.gvFormasCobros, true);
            this.txtImporte.Text = string.Empty;
            //this.ddlTiposValores.SelectedValue = string.Empty;
            this.UpdatePanel2.Update();
        }

        #region Cheques Terceros Pop Up

        protected void btnBuscarCheque_Click(object sender, EventArgs e)
        {
            List<TESCheques> filtro = new List<TESCheques>();
            if (this.MiCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero))
                filtro.AddRange(this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero).Cheques);

            this.ctrBuscarCheque.IniciarControl(filtro);
        }

        void ctrBuscarChequesPopUp_ChequesTercerosSeleccionar(global::Bancos.Entidades.TESCheques e)
        {
            TESCajasMovimientosValores valor;
            TGETiposValores tipoValor = new TGETiposValores();
            tipoValor.IdTipoValor = Convert.ToInt32(this.ddlTiposValores.SelectedValue);
            tipoValor.TipoValor = this.ddlTiposValores.SelectedItem.Text;

            if (!this.MiCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero))
            {
                valor = new TESCajasMovimientosValores();
                valor.EstadoColeccion = EstadoColecciones.Agregado;
                valor.TipoValor = tipoValor;
                this.MiCajaMovimiento.CajasMovimientosValores.Add(valor);
                valor.IndiceColeccion = this.MiCajaMovimiento.CajasMovimientosValores.IndexOf(valor);
            }

            valor = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero);
            e.EstadoColeccion = EstadoColecciones.Modificado;
            e.Estado.IdEstado = (int)EstadosCheques.Entregado;
            e.Estado.Descripcion = EstadosCheques.Entregado.ToString();
            e.TipoOperacion = this.MiTipoOperacion;
            e.IdRefTipoOperacion = this.MiCajaMovimiento.IdRefTipoOperacion;
            valor.Cheques.Add(e);
            e.IndiceColeccion = valor.Cheques.IndexOf(e);
            valor.Importe = valor.Cheques.Sum(x => x.Importe);
            AyudaProgramacion.CargarGrillaListas<TESCheques>(valor.Cheques, false, this.gvChequesTerceros, false);
            this.pnlDetalleChequesTerceros.Visible = true;
            AyudaProgramacion.CargarGrillaListas<TESCajasMovimientosValores>(this.MiCajaMovimiento.CajasMovimientosValores, false, this.gvFormasCobros, true);
            this.UpdatePanel2.Update();
        }

        #endregion

        #region "Grilla Cheques"

        protected void gvCheques_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Borrar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TESCajasMovimientosValores detalle = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque);
            TESCheques cheque = detalle.Cheques[indiceColeccion];
            detalle.Cheques.Remove(cheque);
            detalle.Cheques = AyudaProgramacion.AcomodarIndices<TESCheques>(detalle.Cheques);
            detalle.Importe = detalle.Cheques.Sum(x => x.Importe);
            AyudaProgramacion.CargarGrillaListas<TESCheques>(detalle.Cheques, false, this.gvCheques, false); ;
            if (detalle.Cheques.Count == 0)
            {
                this.pnlDetalleValores.Visible = false;
                this.MiCajaMovimiento.CajasMovimientosValores.Remove(detalle);
                this.MiCajaMovimiento.CajasMovimientosValores = AyudaProgramacion.AcomodarIndices<TESCajasMovimientosValores>(this.MiCajaMovimiento.CajasMovimientosValores);
            }
            AyudaProgramacion.CargarGrillaListas<TESCajasMovimientosValores>(this.MiCajaMovimiento.CajasMovimientosValores, false, this.gvFormasCobros, true);
            this.UpdatePanel2.Update();
        }

        protected void gvCheques_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                TESCheques detalle = (TESCheques)e.Row.DataItem;

                if (this.GestionControl == Gestion.Consultar
                    || !this.MiHabilitar)
                    ibtnEliminar.Visible = false;


                TextBox txtFechaDiferido = (TextBox)e.Row.FindControl("txtFechaDiferido");
                txtFechaDiferido.Text = detalle.FechaDiferido.HasValue ? detalle.FechaDiferido.Value.ToShortDateString() : string.Empty;
                txtFechaDiferido.Enabled = !detalle.FechaDiferido.HasValue;

                //NumericTextBox txtNumeroCheque = (NumericTextBox)e.Row.FindControl("txtNumeroCheque");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (this.MiCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque ))
                {
                    TESCajasMovimientosValores item = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque);
                    if (item.Cheques.Count > 0)
                    {
                        decimal totalCheques = item.Cheques.Sum(x => x.Importe);
                        Label lblImporteTotal = (Label)e.Row.FindControl("lblImporteTotal");
                        lblImporteTotal.Text = totalCheques.ToString("C2");
                    }
                }
            }
        }

        protected void gvCheques_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvCheques.PageIndex = e.NewPageIndex;
            this.gvCheques.DataSource = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque).Cheques;
            this.gvCheques.DataBind();
        }

        #endregion

        #region "Grilla Cheques Terceros"

        protected void gvChequesTerceros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Borrar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TESCajasMovimientosValores detalle = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero);
            TESCheques cheque = detalle.Cheques[indiceColeccion];
            detalle.Cheques.Remove(cheque);
            detalle.Cheques = AyudaProgramacion.AcomodarIndices<TESCheques>(detalle.Cheques);
            detalle.Importe = detalle.Cheques.Sum(x => x.Importe);
            AyudaProgramacion.CargarGrillaListas<TESCheques>(detalle.Cheques, false, this.gvChequesTerceros, false); ;
            if (detalle.Cheques.Count == 0)
            {
                this.pnlDetalleChequesTerceros.Visible = false;
                this.MiCajaMovimiento.CajasMovimientosValores.Remove(detalle);
                this.MiCajaMovimiento.CajasMovimientosValores = AyudaProgramacion.AcomodarIndices<TESCajasMovimientosValores>(this.MiCajaMovimiento.CajasMovimientosValores);
            }
            AyudaProgramacion.CargarGrillaListas<TESCajasMovimientosValores>(this.MiCajaMovimiento.CajasMovimientosValores, false, this.gvFormasCobros, true);
            this.UpdatePanel2.Update();
        }

        protected void gvChequesTerceros_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                TESCheques detalle = (TESCheques)e.Row.DataItem;

                if (this.GestionControl == Gestion.Consultar
                    || !this.MiHabilitar)
                    ibtnEliminar.Visible = false;


                //TextBox txtFechaDiferido = (TextBox)e.Row.FindControl("txtFechaDiferido");
                //txtFechaDiferido.Text = detalle.FechaDiferido.HasValue ? detalle.FechaDiferido.Value.ToShortDateString() : string.Empty;

                //HtmlControl dvFechaDiferido = (HtmlControl)e.Row.FindControl("dvFechaDiferido");
                //dvFechaDiferido.Visible = !detalle.FechaDiferido.HasValue;
                //txtFechaDiferido.Enabled = !detalle.FechaDiferido.HasValue;

                //NumericTextBox txtNumeroCheque = (NumericTextBox)e.Row.FindControl("txtNumeroCheque");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (this.MiCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero))
                {
                    TESCajasMovimientosValores item = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero);
                    if (item.Cheques.Count > 0)
                    {
                        decimal totalCheques = item.Cheques.Sum(x => x.Importe);
                        int cellCount = e.Row.Cells.Count;
                        e.Row.Cells.Clear();
                        TableCell tableCell = new TableCell();
                        tableCell.ColumnSpan = cellCount - 1;
                        tableCell.HorizontalAlign = HorizontalAlign.Right;
                        tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalImporte"), totalCheques.ToString("C2"));
                        e.Row.Cells.Add(tableCell);
                        e.Row.Cells.Add(new TableCell());
                    }
                }
            }
        }

        protected void gvChequesTerceros_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvChequesTerceros.PageIndex = e.NewPageIndex;
            this.gvChequesTerceros.DataSource = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero).Cheques;
            this.gvChequesTerceros.DataBind();
        }

        #endregion

        #region Grilla Transferencias

        protected void gvTransferencias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Borrar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TESCajasMovimientosValores detalle = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia);
            TESBancosCuentasMovimientos item = detalle.BancosCuentasMovimientos[indiceColeccion];
            detalle.BancosCuentasMovimientos.Remove(item);
            detalle.Importe = detalle.BancosCuentasMovimientos.Sum(x => x.Importe);
            detalle.BancosCuentasMovimientos = AyudaProgramacion.AcomodarIndices<TESBancosCuentasMovimientos>(detalle.BancosCuentasMovimientos);
            AyudaProgramacion.CargarGrillaListas<TESBancosCuentasMovimientos>(detalle.BancosCuentasMovimientos, false, this.gvTransferencias, false); ;
            if (detalle.BancosCuentasMovimientos.Count == 0)
            {
                this.pnlTransferencias.Visible = false;
                this.MiCajaMovimiento.CajasMovimientosValores.Remove(detalle);
                this.MiCajaMovimiento.CajasMovimientosValores = AyudaProgramacion.AcomodarIndices<TESCajasMovimientosValores>(this.MiCajaMovimiento.CajasMovimientosValores);
            }
            AyudaProgramacion.CargarGrillaListas<TESCajasMovimientosValores>(this.MiCajaMovimiento.CajasMovimientosValores, false, this.gvFormasCobros, true);
            this.UpdatePanel2.Update();
        }

        protected void gvTransferencias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");

                if (this.GestionControl == Gestion.Consultar
                    || !this.MiHabilitar)
                    ibtnEliminar.Visible = false;

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (this.MiCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia))
                {
                    TESCajasMovimientosValores item = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia);
                    if (item.BancosCuentasMovimientos.Count > 0)
                    {
                        decimal totalCheques = item.BancosCuentasMovimientos.Sum(x => x.Importe);
                        int cellCount = e.Row.Cells.Count;
                        e.Row.Cells.Clear();
                        TableCell tableCell = new TableCell();
                        tableCell.ColumnSpan = cellCount - 1;
                        tableCell.HorizontalAlign = HorizontalAlign.Right;
                        tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalImporte"), totalCheques.ToString("C2"));
                        e.Row.Cells.Add(tableCell);
                        e.Row.Cells.Add(new TableCell());
                    }
                }
            }
        }

        protected void gvTransferencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvTransferencias.PageIndex = e.NewPageIndex;
            this.gvTransferencias.DataSource = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia).BancosCuentasMovimientos;
            this.gvTransferencias.DataBind();
        }

        #endregion

        #region Grilla FondoFijo

        protected void gvFondoFijo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Borrar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TESCajasMovimientosValores detalle = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.EfectivoFondoFijo);
            TESBancosCuentasMovimientos item = detalle.BancosCuentasMovimientos[indiceColeccion];
            detalle.BancosCuentasMovimientos.Remove(item);
            detalle.Importe = detalle.BancosCuentasMovimientos.Sum(x => x.Importe);
            detalle.BancosCuentasMovimientos = AyudaProgramacion.AcomodarIndices<TESBancosCuentasMovimientos>(detalle.BancosCuentasMovimientos);
            AyudaProgramacion.CargarGrillaListas<TESBancosCuentasMovimientos>(detalle.BancosCuentasMovimientos, false, this.gvFondoFijo, false); ;
            if (detalle.BancosCuentasMovimientos.Count == 0)
            {
                this.pnlDetalleFondoFijo.Visible = false;
                this.MiCajaMovimiento.CajasMovimientosValores.Remove(detalle);
                this.MiCajaMovimiento.CajasMovimientosValores = AyudaProgramacion.AcomodarIndices<TESCajasMovimientosValores>(this.MiCajaMovimiento.CajasMovimientosValores);
            }
            AyudaProgramacion.CargarGrillaListas<TESCajasMovimientosValores>(this.MiCajaMovimiento.CajasMovimientosValores, false, this.gvFormasCobros, true);
            this.UpdatePanel2.Update();
        }

        protected void gvFondoFijo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");

                if (this.GestionControl == Gestion.Consultar
                    || !this.MiHabilitar)
                    ibtnEliminar.Visible = false;

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (this.MiCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.EfectivoFondoFijo))
                {
                    TESCajasMovimientosValores item = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.EfectivoFondoFijo);
                    if (item.BancosCuentasMovimientos.Count > 0)
                    {
                        decimal totalCheques = item.BancosCuentasMovimientos.Sum(x => x.Importe);
                        int cellCount = e.Row.Cells.Count;
                        e.Row.Cells.Clear();
                        TableCell tableCell = new TableCell();
                        tableCell.ColumnSpan = cellCount - 1;
                        tableCell.HorizontalAlign = HorizontalAlign.Right;
                        tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalImporte"), totalCheques.ToString("C2"));
                        e.Row.Cells.Add(tableCell);
                        e.Row.Cells.Add(new TableCell());
                    }
                }
            }
        }

        protected void gvFondoFijo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvTransferencias.PageIndex = e.NewPageIndex;
            this.gvTransferencias.DataSource = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.EfectivoFondoFijo).BancosCuentasMovimientos;
            this.gvTransferencias.DataBind();
        }

        #endregion

        #region Grilla Tarjetas Propias

        protected void gvTarjetasPropias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Borrar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TESCajasMovimientosValores detalle = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito);
            TESBancosCuentasMovimientos item = detalle.BancosCuentasMovimientos[indiceColeccion];
            detalle.BancosCuentasMovimientos.Remove(item);
            detalle.Importe = detalle.BancosCuentasMovimientos.Sum(x => x.Importe);
            detalle.BancosCuentasMovimientos = AyudaProgramacion.AcomodarIndices<TESBancosCuentasMovimientos>(detalle.BancosCuentasMovimientos);
            AyudaProgramacion.CargarGrillaListas<TESBancosCuentasMovimientos>(detalle.BancosCuentasMovimientos, false, this.gvTarjetasPropias, false); ;
            if (detalle.BancosCuentasMovimientos.Count == 0)
            {
                this.pnlTransferencias.Visible = false;
                this.MiCajaMovimiento.CajasMovimientosValores.Remove(detalle);
                this.MiCajaMovimiento.CajasMovimientosValores = AyudaProgramacion.AcomodarIndices<TESCajasMovimientosValores>(this.MiCajaMovimiento.CajasMovimientosValores);
            }
            AyudaProgramacion.CargarGrillaListas<TESCajasMovimientosValores>(this.MiCajaMovimiento.CajasMovimientosValores, false, this.gvFormasCobros, true);
            this.UpdatePanel2.Update();
        }

        protected void gvTarjetasPropias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");

                if (this.GestionControl == Gestion.Consultar
                    || !this.MiHabilitar)
                    ibtnEliminar.Visible = false;

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (this.MiCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito))
                {
                    TESCajasMovimientosValores item = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito);
                    if (item.BancosCuentasMovimientos.Count > 0)
                    {
                        decimal totalCheques = item.BancosCuentasMovimientos.Sum(x => x.Importe);
                        int cellCount = e.Row.Cells.Count;
                        e.Row.Cells.Clear();
                        TableCell tableCell = new TableCell();
                        tableCell.ColumnSpan = cellCount - 1;
                        tableCell.HorizontalAlign = HorizontalAlign.Right;
                        tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalImporte"), totalCheques.ToString("C2"));
                        e.Row.Cells.Add(tableCell);
                        e.Row.Cells.Add(new TableCell());
                    }
                }
            }
        }

        protected void gvTarjetasPropias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvTarjetasPropias.PageIndex = e.NewPageIndex;
            this.gvTarjetasPropias.DataSource = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito).BancosCuentasMovimientos;
            this.gvTarjetasPropias.DataBind();
        }

        #endregion

        #region Grilla Tarjetas Credito

        protected void gvTarjetas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Borrar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TESCajasMovimientosValores detalle = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito);
            TESTarjetasTransacciones tarjeta = detalle.TarjetasTransacciones[indiceColeccion];
            detalle.TarjetasTransacciones.Remove(tarjeta);
            detalle.TarjetasTransacciones = AyudaProgramacion.AcomodarIndices<TESTarjetasTransacciones>(detalle.TarjetasTransacciones);
            AyudaProgramacion.CargarGrillaListas<TESTarjetasTransacciones>(detalle.TarjetasTransacciones, false, this.gvTarjetas, false); ;
            if (detalle.TarjetasTransacciones.Count == 0)
            {
                this.pnlDetalleValores.Visible = false;
                this.MiCajaMovimiento.CajasMovimientosValores.Remove(detalle);
                this.MiCajaMovimiento.CajasMovimientosValores = AyudaProgramacion.AcomodarIndices<TESCajasMovimientosValores>(this.MiCajaMovimiento.CajasMovimientosValores);
            }
            AyudaProgramacion.CargarGrillaListas<TESCajasMovimientosValores>(this.MiCajaMovimiento.CajasMovimientosValores, false, this.gvFormasCobros, true);
            this.UpdatePanel2.Update();
        }

        protected void gvTarjetas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                TESTarjetasTransacciones detalle = (TESTarjetasTransacciones)e.Row.DataItem;

                if (this.GestionControl == Gestion.Consultar
                    || !this.MiHabilitar)
                    ibtnEliminar.Visible = false;

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (this.MiCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito))
                {
                    TESCajasMovimientosValores item = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito);
                    if (item.TarjetasTransacciones.Count > 0)
                    {
                        decimal totalTarjeta = item.TarjetasTransacciones.Sum(x => x.Importe);
                        int cellCount = e.Row.Cells.Count;
                        e.Row.Cells.Clear();
                        TableCell tableCell = new TableCell();
                        tableCell.ColumnSpan = cellCount - 1;
                        tableCell.HorizontalAlign = HorizontalAlign.Right;
                        tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalImporte"), totalTarjeta.ToString("C2"));
                        e.Row.Cells.Add(tableCell);
                        e.Row.Cells.Add(new TableCell());
                    }
                }
            }
        }

        protected void gvTarjetas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvTarjetas.PageIndex = e.NewPageIndex;
            this.gvTarjetas.DataSource = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito).TarjetasTransacciones;
            this.gvTarjetas.DataBind();
        }

        #endregion

        protected void gvFormasCobros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Borrar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TESCajasMovimientosValores detalle = MiCajaMovimiento.CajasMovimientosValores[indiceColeccion];
            MiCajaMovimiento.CajasMovimientosValores.Remove(detalle);
            MiCajaMovimiento.CajasMovimientosValores = AyudaProgramacion.AcomodarIndices<TESCajasMovimientosValores>(this.MiCajaMovimiento.CajasMovimientosValores);
            AyudaProgramacion.CargarGrillaListas<TESCajasMovimientosValores>(MiCajaMovimiento.CajasMovimientosValores, false, this.gvFormasCobros, true); ;
            this.UpdatePanel2.Update();
        }

        protected void gvFormasCobros_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                TESCajasMovimientosValores detalle = (TESCajasMovimientosValores)e.Row.DataItem;
                if (detalle.TipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo
                    || detalle.TipoValor.IdTipoValor == (int)EnumTiposValores.CajaAhorro)
                    ibtnEliminar.Visible = true;

                if (this.GestionControl == Gestion.Consultar
                    || !this.MiHabilitar)
                    ibtnEliminar.Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.Footer
                && MiCajaMovimiento.CajasMovimientosValores.Count > 0)
            {
                Label lblImporteTotal = (Label)e.Row.FindControl("lblImporteTotal");
                lblImporteTotal.Text = this.MiCajaMovimiento.CajasMovimientosValores.Sum(x => x.Importe).ToString("C2");
            }
        }

        protected void gvFormasCobros_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvFormasCobros.PageIndex = e.NewPageIndex;
            this.gvFormasCobros.DataSource = MiCajaMovimiento.CajasMovimientosValores;
            this.gvFormasCobros.DataBind();
        }

        private void LimpiarControlesCheques()
        {
            this.txtFecha.Text = string.Empty;
            this.txtFechaDiferido.Text = string.Empty;
            this.txtConcepto.Text = string.Empty;
            this.txtNumeroCheque.Text = string.Empty;
            this.txtTitular.Text = string.Empty;
            this.txtCUIT.Text = string.Empty;
        }

        private void LimpiarControlesTarjetas()
        {
            this.txtNumeroTarjeta.Text = String.Empty;
            this.txtTitularTarjeta.Text = String.Empty;
            this.txtVencimientoAA.Text = String.Empty;
            this.txtVencimientoMM.Text = String.Empty;
            //this.txtPosnet.Text = String.Empty;
            this.txtCuotas.Text = String.Empty;
            this.txtObservaciones.Text = String.Empty;
            //this.txtNumeroLote.Text = String.Empty;
            this.txtFechaTransferencia.Text = DateTime.Now.ToShortDateString();
        }

        private void PersistirDatosGrilla()
        {
            if (this.MiCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque))
            {
                TESCajasMovimientosValores valor = this.MiCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque );

                foreach (GridViewRow fila in this.gvCheques.Rows)
                {
                    string fechaDiferido = ((TextBox)fila.FindControl("txtFechaDiferido")).Text;
                    string numeroCheque = ((TextBox)fila.FindControl("txtNumeroCheque")).Text;
                    if (fechaDiferido != string.Empty)
                    {
                        valor.Cheques[fila.DataItemIndex].FechaDiferido = Convert.ToDateTime(fechaDiferido);
                    }
                    if (numeroCheque != string.Empty)
                    {
                        valor.Cheques[fila.DataItemIndex].NumeroCheque = numeroCheque;
                    }
                }
            }
        }

    }
}
