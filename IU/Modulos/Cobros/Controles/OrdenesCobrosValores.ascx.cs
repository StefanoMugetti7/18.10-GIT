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
using Cobros.Entidades;
using System.Web.UI.HtmlControls;
using Prestamos.Entidades;
using Prestamos;
using ProcesosDatos;
using Cargos.Entidades;
using Facturas.Entidades;
using Facturas;
using Cargos;
using Ahorros.Entidades;
using Ahorros;

namespace IU.Modulos.Cobros.Controles
{
    public partial class OrdenesCobrosValores : ControlesSeguros
    {
        private CobOrdenesCobros MiOrdenCobro
        {
            get { return (CobOrdenesCobros)Session[this.MiSessionPagina + "OrdenesCobrosValoresMiOrdenCobro"]; }
            set { Session[this.MiSessionPagina + "OrdenesCobrosValoresMiOrdenCobro"] = value; }
        }

        private PrePrestamos MiPrestamo
        {
            get { return (PrePrestamos)Session[this.MiSessionPagina + "OrdenesDeCobroDatosMiPrestamo"]; }
            set { Session[this.MiSessionPagina + "OrdenesDeCobroDatosMiPrestamo"] = value; }
        }

        private List<VTARemitos> MisRemitos
        {
            get { return (List<VTARemitos>)Session[this.MiSessionPagina + "OrdenesDeCobroDatosMisRemitos"]; }
            set { Session[this.MiSessionPagina + "OrdenesDeCobroDatosMisRemitos"] = value; }
        }

        private List<VTARemitos> MisRemitosSeleccionados
        {
            get { return (List<VTARemitos>)Session[this.MiSessionPagina + "OrdenesCobrosValoresMisRemitosSeleccionados"]; }
            set { Session[this.MiSessionPagina + "OrdenesCobrosValoresMisRemitosSeleccionados"] = value; }
        }

        public CarTiposCargosAfiliadosFormasCobros MiCargoAfiliado
        {
            get { return (CarTiposCargosAfiliadosFormasCobros)Session[this.MiSessionPagina + "OrdenesDeCobroDatoMiCargoAfiliado"]; }
            set { Session[this.MiSessionPagina + "OrdenesDeCobroDatoMiCargoAfiliado"] = value; }
        }

        private List<TGETiposOperaciones> MisTiposOperacionesPrestamos
        {
            get { return (List<TGETiposOperaciones>)Session[this.MiSessionPagina + "OrdenesDeCobroDatosMisTiposOperacionesPrestamos"]; }
            set { Session[this.MiSessionPagina + "OrdenesDeCobroDatosMisTiposOperacionesPrestamos"] = value; }
        }

        private List<PrePrestamosPlanes> MisPrestamosPlanes
        {
            get { return (List<PrePrestamosPlanes>)Session[this.MiSessionPagina + "OrdenesDeCobroDatosMisPrestamosPlanes"]; }
            set { Session[this.MiSessionPagina + "OrdenesDeCobroDatosMisPrestamosPlanes"] = value; }
        }

        private List<TGEFormasCobrosAfiliados> MisFormasCobrosAfiliados
        {
            get { return (List<TGEFormasCobrosAfiliados>)Session[this.MiSessionPagina + "OrdenesCobrosFacturasDatosMisFormasCobrosAfiliados"]; }
            set { Session[this.MiSessionPagina + "OrdenesCobrosFacturasDatosMisFormasCobrosAfiliados"] = value; }
        }

        private List<TESBancosCuentas> MisBancosCuentas
        {
            get { return (List<TESBancosCuentas>)Session[this.MiSessionPagina + "OrdenesCobrosValoresMisBancosCuentas"]; }
            set { Session[this.MiSessionPagina + "OrdenesCobrosValoresMisBancosCuentas"] = value; }
        }

        private List<AhoCuentas> MisCajasAhorros
        {
            get { return (List<AhoCuentas>)Session[this.MiSessionPagina + "OrdenesCobrosValoresMisCajasAhorros"]; }
            set { Session[this.MiSessionPagina + "OrdenesCobrosValoresMisCajasAhorros"] = value; }
        }

        private List<TGETiposValores> MisTiposValores
        {
            get { return (List<TGETiposValores>)Session[this.MiSessionPagina + "OrdenesCobrosValoresMisTiposValores"]; }
            set { Session[this.MiSessionPagina + "OrdenesCobrosValoresMisTiposValores"] = value; }
        }

        private decimal ValidarImporteEfectivo
        {
            get { return (decimal)Session[this.MiSessionPagina + "OrdenesDeCobroDatosValidarImporteEfectivo"]; }
            set { Session[this.MiSessionPagina + "OrdenesDeCobroDatosValidarImporteEfectivo"] = value; }
        }

        public delegate void OrdenesCobrosValoresIngresarEventHandler(List<CobOrdenesCobrosValores> e);
        public event OrdenesCobrosValoresIngresarEventHandler OrdenesCobrosIngresarValor;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtImporte, this.btnIngresarCobro);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtFechaTransaccion, this.btnIngresarCobro);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtFechaTransferencia, this.btnIngresarCobro);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtFechaDiferido, this.btnIngresarCobro);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroCheque, this.btnIngresarCobro);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtTitular, this.btnIngresarCobro);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtConcepto, this.btnIngresarCobro);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCUIT, this.btnIngresarCobro);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroTarjeta, this.btnIngresarCobro);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtTitularTarjeta, this.btnIngresarCobro);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtVencimientoAA, this.btnIngresarCobro);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtVencimientoMM, this.btnIngresarCobro);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtPosnet, this.btnIngresarCobro);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCantidadCuotas, this.btnIngresarCobro);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroLote, this.btnIngresarCobro);
                TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.AlertaParaIngresoEfectivo);
                decimal dValor = 0;
                decimal.TryParse(valor.ParametroValor, out dValor);
                ValidarImporteEfectivo = dValor;
                this.PersistirDatosGrilla();
            }
        }

        public void IniciarControl(CobOrdenesCobros pOrdenCobro, Gestion pGestion)
        {
            this.IniciarControl(pOrdenCobro, pGestion, false);
        }
        public void IniciarControl(CobOrdenesCobros pOrdenCobro, Gestion pGestion, bool pUpdate)
        {
            this.GestionControl = pGestion;
            this.MiOrdenCobro = pOrdenCobro;
            switch (pGestion)
            {
                case Gestion.Agregar:
                    this.pnlDetalle.Visible = true;
                    this.MiOrdenCobro.CargoDescuentoAfiliado = false;
                    this.MiOrdenCobro.OrdenesCobrosValores = new List<CobOrdenesCobrosValores>();
                    this.MiPrestamo = null;
                    this.MiCargoAfiliado = null;
                    this.MisRemitos = null;
                    this.MisRemitosSeleccionados = null;
                    //EnumTGETiposFuncionalidades tipoFunc = (EnumTGETiposFuncionalidades)this.paginaSegura.paginaActual.IdTipoFuncionalidad;
                    TGETiposFuncionalidades tipoFunc = new TGETiposFuncionalidades();
                    tipoFunc.IdTipoFuncionalidad = this.paginaSegura.paginaActual.IdTipoFuncionalidad;
                    tipoFunc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    this.MisTiposValores = TGEGeneralesF.TiposValoresObtenerLista(tipoFunc);
                    if (this.MisTiposValores.Exists(x => x.IdTipoValor == (int)EnumTiposValores.Cheque))
                    {
                        this.MisTiposValores.Remove(this.MisTiposValores.Find(x => x.IdTipoValor == (int)EnumTiposValores.Cheque));
                    }
                    //if (!this.MiOrdenCobro.TipoOperacion.Contabiliza)
                    //{
                    //    if (this.MisTiposValores.Exists(x => x.IdTipoValor == (int)EnumTiposValores.Cargos))
                    //    {
                    //        this.MisTiposValores.Remove(this.MisTiposValores.Find(x => x.IdTipoValor == (int)EnumTiposValores.Cargos));
                    //    }
                    //    if (this.MisTiposValores.Exists(x => x.IdTipoValor == (int)EnumTiposValores.Prestamos))
                    //    {
                    //        this.MisTiposValores.Remove(this.MisTiposValores.Find(x => x.IdTipoValor == (int)EnumTiposValores.Prestamos));
                    //    }
                    //}
                    this.MisTiposValores = AyudaProgramacion.AcomodarIndices<TGETiposValores>(this.MisTiposValores).ToList();
                    this.CargarComboValores();
                    AyudaProgramacion.CargarGrillaListas<CobOrdenesCobrosValores>(this.MiOrdenCobro.OrdenesCobrosValores, false, this.gvFormasCobros, false);
                    if (pUpdate) this.upCobrosValores.Update();
                    break;
                case Gestion.Anular:
                case Gestion.AnularConfirmar:
                case Gestion.Autorizar:
                case Gestion.Modificar:
                case Gestion.Consultar:
                    this.ddlTiposValores.Enabled = false;
                    this.pnlDetalle.Visible = false;
                    AyudaProgramacion.CargarGrillaListas<CobOrdenesCobrosValores>(this.MiOrdenCobro.OrdenesCobrosValores, false, this.gvFormasCobros, true);
                    break;
                default:
                    break;
                  

            }
        }
        public void ActualizarUpdatePanel()
        {
            this.UpdatePanel3.Update();

        }
        private void CargarComboValores()
        {
            this.ddlTiposValores.Items.Clear();
            this.ddlTiposValores.SelectedIndex = -1;
            this.ddlTiposValores.SelectedValue = null;
            this.ddlTiposValores.ClearSelection();

            this.ddlTiposValores.DataSource = this.MisTiposValores;
            this.ddlTiposValores.DataValueField = "IdTipoValor";
            this.ddlTiposValores.DataTextField = "TipoValor";
            this.ddlTiposValores.DataBind();
            if (this.ddlTiposValores.Items.Count != 1)
            {
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTiposValores, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
            
            this.ddlTiposValores_SelectedIndexChanged(null, EventArgs.Empty);
        }

        private void HabilitarChequesTerceros()
        {
            TESBancos banco = new TESBancos();
            banco.Estado.IdEstado = (int)Estados.Activo;
            this.ddlBancos.DataSource = BancosF.BancosObtenerListaFiltro(banco);
            this.ddlBancos.DataValueField = "IdBanco";
            this.ddlBancos.DataTextField = "Descripcion";
            this.ddlBancos.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlBancos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void HabilitarTransferencias()
        {
            TESBancosCuentas bancoCuenta = new TESBancosCuentas();
            bancoCuenta.Estado.IdEstado = (int)Estados.Activo;
            bancoCuenta.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
            bancoCuenta.Moneda = MiOrdenCobro.Moneda;
            bancoCuenta.Filtro = ddlTiposValores.SelectedValue;
            this.MisBancosCuentas = BancosF.BancosCuentasObtenerListaFiltroTransferencia(bancoCuenta);
            this.ddlBancosCuentasTransferencias.DataSource = this.MisBancosCuentas;
            this.ddlBancosCuentasTransferencias.DataValueField = "IdBancoCuenta";
            this.ddlBancosCuentasTransferencias.DataTextField = "DescripcionFilialBancoTipoCuentaNumero";
            this.ddlBancosCuentasTransferencias.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlBancosCuentasTransferencias, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void HabilitarCajaAhorro()
        {
            AhoCuentas cuenta = new AhoCuentas();
            cuenta.Estado.IdEstado = (int)EstadosAhorrosCuentas.CuentaAbierta;
            cuenta.Afiliado.IdAfiliado = MiOrdenCobro.Afiliado.IdAfiliado;
            cuenta.Moneda.IdMoneda = MiOrdenCobro.Moneda.IdMoneda;
            cuenta.Filtro = "OrdenesCobrosValores";
            this.MisCajasAhorros = AhorroF.CuentasObtenerListaFiltro(cuenta);
            this.ddlCuentaBancariaCajaAhorro.DataSource = this.MisCajasAhorros;
            this.ddlCuentaBancariaCajaAhorro.DataValueField = "IdCuenta";
            this.ddlCuentaBancariaCajaAhorro.DataTextField = "CuentaDatos";
            this.ddlCuentaBancariaCajaAhorro.DataBind();
            //if (MisCajasAhorros.Count!=1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlCuentaBancariaCajaAhorro, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void HabilitarTarjetas()
        {
            this.ddlTarjetas.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TarjetasCredito);
            this.ddlTarjetas.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTarjetas.DataTextField = "Descripcion";
            this.ddlTarjetas.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTarjetas, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.txtFechaTransaccion.Text = DateTime.Now.ToShortDateString();
        }

        public CobOrdenesCobros ObtenerOrdenesCobrosValores(CobOrdenesCobros pOrdenCobro)
        {
            this.PersistirDatosGrilla();
            pOrdenCobro.OrdenesCobrosValores = this.MiOrdenCobro.OrdenesCobrosValores;
            pOrdenCobro.CargoDescuentoAfiliado = this.MiOrdenCobro.CargoDescuentoAfiliado;
            pOrdenCobro.CuotasDescuentoAfiliado = this.MiOrdenCobro.CuotasDescuentoAfiliado;
            pOrdenCobro.FormaCobroAfiliado.IdFormaCobroAfiliado = this.MiOrdenCobro.FormaCobroAfiliado.IdFormaCobroAfiliado;
            pOrdenCobro.Prestamo = this.MiPrestamo;
            if (this.MisRemitosSeleccionados!=null)
            {
                pOrdenCobro.Remitos = new List<VTARemitos>();
                pOrdenCobro.Remitos.AddRange(this.MisRemitosSeleccionados);
            }
            return pOrdenCobro;
        }

        protected void ddlTiposValores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ddlTiposValores.SelectedValue))
            {
                pnlCheques.Visible = false;
                pnlRemitos.Visible = false;

                //pnlCheques.Visible = false;
                pnlDetalleIngresos.Visible = true;
                pnlTarjetasCreditos.Visible = false;
                pnlDescuentoCargos.Visible = false;
                pnlPrestamos.Visible = false;
                pnlDescuentoCargos.Visible = false;
                //pnlTarjetasCreditos.Visible = false;
                pnlTransferencias.Visible = false;
                pnlCajaAhorro.Visible = false;
                return;
            }
            TGETiposValores tipoValor = new TGETiposValores();
            tipoValor.IdTipoValor = Convert.ToInt32(this.ddlTiposValores.SelectedValue);

            this.btnIngresarCobro.Visible = true;
            this.pnlCheques.Visible = false;
            this.pnlTransferencias.Visible = false;
            this.pnlCajaAhorro.Visible = false;
            this.pnlTarjetasCreditos.Visible = false;
            this.pnlDescuentoCargos.Visible = false;
            this.pnlPrestamos.Visible = false;
            this.pnlRemitos.Visible = false;
            string idControlFecha=string.Empty;
            if (tipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia
                || tipoValor.IdTipoValor == (int)EnumTiposValores.EfectivoFondoFijo)
            {
                this.pnlTransferencias.Visible = true;
                this.HabilitarTransferencias();
                idControlFecha = this.txtFechaTransferencia.ID;
                //this.btnIngresarCobro.ValidationGroup = "IngresarCobro,IngresarTransferencia";
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.CajaAhorro)
            {
                this.pnlCajaAhorro.Visible = true;
                this.HabilitarCajaAhorro();
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
            {
                this.pnlCheques.Visible = true;
                this.HabilitarChequesTerceros();
                idControlFecha = this.txtFecha.ID;
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito)
            {
                this.pnlTarjetasCreditos.Visible = true;
                this.HabilitarTarjetas();
                idControlFecha = this.txtFechaTransaccion.ID;
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.Cargos)
            {
                this.pnlDescuentoCargos.Visible = true;
                this.HabilitarCargo();
                this.HabilitarRemitos();
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.Prestamos)
            {
                this.pnlPrestamos.Visible = true;
                this.HabilitarPrestamo();
                this.HabilitarRemitos();
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.AfipTiposPercepciones
               || tipoValor.IdTipoValor == (int)EnumTiposValores.AfipTiposRetenciones)
            {
                this.pnlAfipRetencionesPercepciones.Visible = true;
                this.HabilitarRetencionPercepcion();
            }

            this.CamposValoresTipoValor.IniciarControl( new CobOrdenesCobrosValores() , tipoValor, this.GestionControl);
            ScriptManager.RegisterStartupScript(this.upCobrosValores, this.upCobrosValores.GetType(), "BuscarFechaCajaContScript", "BuscarFechaCajaCont('"+this.ID+"_"+idControlFecha+"');", true);
            this.upCobrosValores.Update();
        }

        protected void btnIngresarCobro_Click(object sender, EventArgs e)
        {
            this.Page.Validate("IngresarCobro");
            if (!this.Page.IsValid)
            {
                this.upCobrosValores.Update();
                return;
            }

            if (this.txtImporte.Decimal <= 0)
            {
                this.MostrarMensaje("ValidarImporteMayorCero", true);
                this.upCobrosValores.Update();
                return;
            }
            TGETiposValores tipoValor = new TGETiposValores();
            tipoValor.IdTipoValor = Convert.ToInt32(this.ddlTiposValores.SelectedValue);
            tipoValor.TipoValor = this.ddlTiposValores.SelectedItem.Text;
            if (tipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia
                || tipoValor.IdTipoValor == (int)EnumTiposValores.EfectivoFondoFijo)
            {
                this.Page.Validate("IngresarTransferencia");
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.CajaAhorro)
            {
                this.Page.Validate("IngresarCajaAhorro");
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
            {
                this.Page.Validate("IngresarCheque");
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito)
            {
                this.Page.Validate("IngresarTarjeta");
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.Cargos)
            {
                this.Page.Validate("IngresarDescuentoCargos");
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.Prestamos)
            {
                this.Page.Validate("IngresarPrestamo");
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.AfipTiposRetenciones
               || tipoValor.IdTipoValor == (int)EnumTiposValores.AfipTiposPercepciones)
            {
                this.Page.Validate("IngresarRetencionPercepcion");
            }

            if (!this.Page.IsValid)
            {
                this.upCobrosValores.Update();
                return;
            }
            CobOrdenesCobrosValores valor = new CobOrdenesCobrosValores();
            //EFECTIVO
            if (tipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo)
            {   
                if (this.MiOrdenCobro.OrdenesCobrosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo))
                    this.MiOrdenCobro.OrdenesCobrosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo).Importe = this.txtImporte.Decimal;
                else
                {
                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                    valor.TipoValor = tipoValor;
                    valor.Importe = this.txtImporte.Decimal;
                    this.MiOrdenCobro.OrdenesCobrosValores.Add(valor);
                    valor.IndiceColeccion = this.MiOrdenCobro.OrdenesCobrosValores.IndexOf(valor);
                }
                if (ValidarImporteEfectivo > 0 && txtImporte.Decimal > ValidarImporteEfectivo)
                {
                    List<string> param = new List<string>();
                    param.Add(string.Concat(this.MiOrdenCobro.Moneda.Moneda, " ", ValidarImporteEfectivo.ToString("N2")));
                    MostrarMensaje("ValidarAlertaIngresoEfectivo", true, param);
                }
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia
                || tipoValor.IdTipoValor == (int)EnumTiposValores.EfectivoFondoFijo)
            {
                valor.EstadoColeccion = EstadoColecciones.Agregado;
                valor.TipoValor = tipoValor;
                this.MiOrdenCobro.OrdenesCobrosValores.Add(valor);
                valor.IndiceColeccion = this.MiOrdenCobro.OrdenesCobrosValores.IndexOf(valor);

                valor.Fecha = Convert.ToDateTime(this.txtFechaTransferencia.Text);
                valor.BancoCuenta = this.MisBancosCuentas[this.ddlBancosCuentasTransferencias.SelectedIndex];
                valor.Detalle = valor.BancoCuenta.DescripcionFilialBancoTipoCuentaNumero;
                valor.Importe = this.txtImporte.Decimal;
            }
            //CAJA AHORRO
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.CajaAhorro)
            {
                valor.EstadoColeccion = EstadoColecciones.Agregado;
                valor.TipoValor = tipoValor;
                this.MiOrdenCobro.OrdenesCobrosValores.Add(valor);
                valor.IndiceColeccion = this.MiOrdenCobro.OrdenesCobrosValores.IndexOf(valor);
                valor.IdCuenta = this.MisCajasAhorros[this.ddlCuentaBancariaCajaAhorro.SelectedIndex].IdCuenta; 
                //valor.Fecha = Convert.ToDateTime(this.txtFechaCajaAhorro.Text);
                //valor.BancoCuenta = this.MisBancosCuentas[this.ddlBancosCuentasTransferencias.SelectedIndex];
                valor.Detalle = this.MisCajasAhorros[this.ddlCuentaBancariaCajaAhorro.SelectedIndex].CuentaDatos;
                valor.Importe = this.txtImporte.Decimal;
            }
            //CHEQUE TERCEROS
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
            {
                valor.EstadoColeccion = EstadoColecciones.Agregado;
                valor.TipoValor = tipoValor;
                this.MiOrdenCobro.OrdenesCobrosValores.Add(valor);
                valor.IndiceColeccion = this.MiOrdenCobro.OrdenesCobrosValores.IndexOf(valor);

                valor.Fecha = this.txtFecha.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(this.txtFecha.Text);
                valor.FechaDiferido = this.txtFechaDiferido.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaDiferido.Text);
                valor.Importe = this.txtImporte.Decimal;
                valor.NumeroCheque = this.txtNumeroCheque.Text;
                valor.BancoCuenta.Banco.IdBanco = this.ddlBancos.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlBancos.SelectedValue);
                valor.BancoCuenta.Banco.Descripcion = this.ddlBancos.SelectedValue == string.Empty ? string.Empty : this.ddlBancos.SelectedItem.Text;
                valor.Titular = this.txtTitular.Text;

                TESCheques cheque = new TESCheques();
                cheque.EstadoColeccion = EstadoColecciones.Agregado;
                cheque.Estado.IdEstado = (int)EstadosCheques.Activo;
                cheque.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                cheque.Fecha = valor.Fecha.Value;
                cheque.FechaDiferido = valor.FechaDiferido;
                cheque.Concepto = this.txtConcepto.Text;
                cheque.NumeroCheque = valor.NumeroCheque;
                cheque.Banco.IdBanco = this.ddlBancos.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlBancos.SelectedValue);
                cheque.Banco.Descripcion = this.ddlBancos.SelectedValue == string.Empty ? string.Empty : this.ddlBancos.SelectedItem.Text;
                cheque.TitularCheque = this.txtTitular.Text;
                cheque.CUIT = this.txtCUIT.Text;
                cheque.Importe = valor.Importe;
                valor.Cheque = cheque;

                valor.Detalle = cheque.BancoTitularCuit;
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito)
            {
                valor.EstadoColeccion = EstadoColecciones.Agregado;
                valor.TipoValor = tipoValor;
                this.MiOrdenCobro.OrdenesCobrosValores.Add(valor);
                valor.IndiceColeccion = this.MiOrdenCobro.OrdenesCobrosValores.IndexOf(valor);
                valor.Fecha = this.txtFechaTransaccion.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(this.txtFechaTransaccion.Text);
                valor.FechaDiferido = valor.Fecha;
                valor.Importe = this.txtImporte.Decimal;
                valor.IdTarjetaCredito = this.ddlTarjetas.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTarjetas.SelectedValue);
                valor.NumeroTarjetaCredito = this.txtNumeroTarjeta.Text;
                valor.Titular = this.txtTitularTarjeta.Text;
                valor.VencimientoAnio = this.txtVencimientoAA.Text == string.Empty ? 0 : Convert.ToInt32(this.txtVencimientoAA.Text);
                valor.VencimientoMes = this.txtVencimientoMM.Text == string.Empty ? 0 : Convert.ToInt32(this.txtVencimientoMM.Text);
                valor.NumeroTransaccionPosnet = this.txtPosnet.Text;
                valor.CantidadCuotas = this.txtCantidadCuotas.Text == string.Empty ? 1 : Convert.ToInt32(this.txtCuotas.Text);
                valor.NumeroLote = this.txtNumeroLote.Text;
                valor.Detalle = string.Concat("Nro. ", valor.NumeroTarjetaCredito, " Cantidad Cuotas ", valor.CantidadCuotas.ToString());
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.Cargos)
            {
                //this.ddlTiposValores.Items.Remove(this.ddlTiposValores.Items.FindByValue(((int)EnumTiposValores.Cargos).ToString()));
                //this.ddlTiposValores.Items.Remove(this.ddlTiposValores.Items.FindByValue(((int)EnumTiposValores.Prestamos).ToString()));
                this.btnIngresarCobro.Visible = false;
                this.MiOrdenCobro.OrdenesCobrosValores = new List<CobOrdenesCobrosValores>();

                //Genero los Cargos a Descontar al Socio
                MiCargoAfiliado = new CarTiposCargosAfiliadosFormasCobros();
                MiCargoAfiliado.IdAfiliado = this.MiOrdenCobro.Afiliado.IdAfiliado;
                MiCargoAfiliado.TipoCargo.IdTipoCargo = (int)EnumTiposCargos.OrdenesCobrosFacturas;
                MiCargoAfiliado.FormaCobroAfiliado = this.MisFormasCobrosAfiliados[this.ddlFormasCobros.SelectedIndex];//.FormaCobroAfiliado;
                MiCargoAfiliado.FechaAltaEvento = DateTime.Now;
                MiCargoAfiliado.FechaAlta = new DateTime(Convert.ToInt32(this.ddlPrimerVtoDtoCargo.SelectedValue.Substring(0, 4)), Convert.ToInt32(this.ddlPrimerVtoDtoCargo.SelectedValue.Substring(4, 2)), 1);
                MiCargoAfiliado.Estado.IdEstado = (int)EstadosCargos.Activo;
                MiCargoAfiliado.UsuarioAlta.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                MiCargoAfiliado.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                MiCargoAfiliado.CantidadCuotas = this.txtCantidadCuotasDtoCargo.Text == string.Empty ? 1 : Convert.ToInt32(this.txtCantidadCuotasDtoCargo.Text);

                this.MiOrdenCobro.CargoDescuentoAfiliado = true;
                this.MiOrdenCobro.FormaCobroAfiliado.IdFormaCobroAfiliado = this.MisFormasCobrosAfiliados[this.ddlFormasCobros.SelectedIndex].IdFormaCobroAfiliado;
                this.MiOrdenCobro.CuotasDescuentoAfiliado = Convert.ToInt32(this.txtCantidadCuotasDtoCargo.Decimal);
                decimal importeInteres = 0;
                if (this.MiOrdenCobro.CuotasDescuentoAfiliado.Value > 1)
                {
                    TGEParametrosValores paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.TasaDeInteres);
                    importeInteres = Math.Round(this.MiOrdenCobro.CuotasDescuentoAfiliado.Value * Convert.ToDecimal(paramValor.ParametroValor) / 100 * this.txtImporte.Decimal, 2);
                }
                decimal importeNuevo = this.txtImporte.Decimal + importeInteres;

                MiCargoAfiliado.CantidadCuotas = this.MiOrdenCobro.CuotasDescuentoAfiliado.Value;
                MiCargoAfiliado.ImporteCuota = Math.Round(importeNuevo / MiCargoAfiliado.CantidadCuotas, 2);
                MiCargoAfiliado.ImporteTotal = importeNuevo;
                MiCargoAfiliado.ImporteInteres = importeInteres;

                valor.EstadoColeccion = EstadoColecciones.Agregado;
                valor.TipoValor = tipoValor;
                valor.Detalle = string.Concat(this.MiOrdenCobro.CuotasDescuentoAfiliado, " cuotas de ", MiCargoAfiliado.ImporteCuota.ToString("C2"), " a partir de ", this.ddlPrimerVtoDtoCargo.SelectedValue);
                this.MiOrdenCobro.OrdenesCobrosValores.Add(valor);
                valor.IndiceColeccion = this.MiOrdenCobro.OrdenesCobrosValores.IndexOf(valor);
                valor.Importe = this.txtImporte.Decimal;
                this.pnlDescuentoCargos.Visible = false;
                this.btnIngresarCobro.Visible = false;
                this.ddlTiposValores.Enabled = false;
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.Prestamos)
            {
                this.btnIngresarCobro.Visible = true;
                this.MiOrdenCobro.OrdenesCobrosValores = new List<CobOrdenesCobrosValores>();

                this.MiPrestamo.FechaPrestamo = DateTime.Now;
                this.MiPrestamo.FechaPreAutorizado = DateTime.Now;
                this.MiPrestamo.FechaAutorizado = DateTime.Now;
                this.MiPrestamo.Estado.IdEstado = (int)EstadosPrestamos.Autorizado;
                this.MiPrestamo.TipoValor.IdTipoValor = (int)EnumTiposValores.Efectivo;
                this.MiPrestamo.PeriodoPrimerVencimiento = Convert.ToInt32(this.ddlPeriodoPrimerVtoPrestamo.SelectedValue);
                //SOLO PRESTAMOS EN PESOS PARA LAS OCF
                this.MiPrestamo.Moneda.IdMoneda = (int)EnumTGEMonedas.PesosArgentinos; //PREGUNTAR
                this.MiPrestamo.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial; //PREGUNTAR
                this.CalcularPrestamo();
                this.MiPrestamo.Campos = this.ctrCamposValores.ObtenerLista();
                this.MiPrestamo.EstadoColeccion = EstadoColecciones.Agregado;

                valor.EstadoColeccion = EstadoColecciones.Agregado;
                valor.TipoValor = tipoValor;
                valor.Detalle = string.Concat(this.MiPrestamo.CantidadCuotas, " cuotas de ", this.MiPrestamo.ImporteCuota.ToString("C2"), " a partir de ", this.ddlPeriodoPrimerVtoPrestamo.SelectedValue);
                this.MiOrdenCobro.OrdenesCobrosValores.Add(valor);
                valor.IndiceColeccion = this.MiOrdenCobro.OrdenesCobrosValores.IndexOf(valor);
                valor.Importe = this.txtImporte.Decimal;
                this.pnlPrestamos.Visible = false;
                this.btnIngresarCobro.Visible = false;
                this.ddlTiposValores.Enabled = false;
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.AfipTiposPercepciones
                || tipoValor.IdTipoValor == (int)EnumTiposValores.AfipTiposRetenciones)
            {
                valor.EstadoColeccion = EstadoColecciones.Agregado;
                valor.TipoValor = tipoValor;
                this.MiOrdenCobro.OrdenesCobrosValores.Add(valor);
                valor.IndiceColeccion = this.MiOrdenCobro.OrdenesCobrosValores.IndexOf(valor);
                valor.Importe = this.txtImporte.Decimal;
                valor.ListaValorSistemaDetalle.IdListaValorSistemaDetalle = ddlTiposRetencionesPercepciones.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTiposRetencionesPercepciones.SelectedValue);
                valor.ListaValorSistemaDetalle.Descripcion = this.ddlTiposRetencionesPercepciones.SelectedItem.Text;
            }
            else {
                valor.EstadoColeccion = EstadoColecciones.Agregado;
                valor.TipoValor = tipoValor;
                this.MiOrdenCobro.OrdenesCobrosValores.Add(valor);
                valor.IndiceColeccion = this.MiOrdenCobro.OrdenesCobrosValores.IndexOf(valor);
                valor.Importe = this.txtImporte.Decimal;
            }
            valor.Campos = this.CamposValoresTipoValor.ObtenerLista();
            valor.Campos.ForEach(x => valor.Detalle += string.Concat(x.Titulo, ": ", x.CampoValor.ListaValor==string.Empty ? x.CampoValor.Valor : x.CampoValor.ListaValor, " "));
            this.LimpiarControles();
            this.PersistirDatosGrilla();
            AyudaProgramacion.CargarGrillaListas<CobOrdenesCobrosValores>(this.MiOrdenCobro.OrdenesCobrosValores, false, this.gvFormasCobros, true);
            this.upCobrosValores.Update();

            if (this.OrdenesCobrosIngresarValor != null)
            {
                this.OrdenesCobrosIngresarValor(this.MiOrdenCobro.OrdenesCobrosValores);
            }
        }

        protected void gvFormasCobros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Borrar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CobOrdenesCobrosValores detalle = MiOrdenCobro.OrdenesCobrosValores[indiceColeccion];

            if (detalle.TipoValor.IdTipoValor == (int)EnumTiposValores.Cargos)
            {
                this.MiOrdenCobro.CuotasDescuentoAfiliado = null;
                this.MiOrdenCobro.CargoDescuentoAfiliado = false;
                this.MiOrdenCobro.FormaCobroAfiliado.IdFormaCobroAfiliado = 0;
                this.MiCargoAfiliado = null;
                this.btnIngresarCobro.Visible = true;
                this.ddlTiposValores.Enabled = true;
                this.txtCantidadCuotasDtoCargo.Text = string.Empty;
                this.txtImporte.Text = string.Empty;
                if (this.ddlTiposValores.SelectedValue == ((int)EnumTiposValores.Cargos).ToString())
                    this.ddlTiposValores_SelectedIndexChanged(null, EventArgs.Empty);
            }
            else if (detalle.TipoValor.IdTipoValor == (int)EnumTiposValores.Prestamos)
            {
                this.MiPrestamo = null;
                this.btnIngresarCobro.Visible = true;
                this.ddlTiposValores.Enabled = true;
                this.txtCantidadCuotas.Text = string.Empty;
                this.txtImporteCuotas.Text = string.Empty;
                this.txtImporte.Text = string.Empty;
                if (this.ddlTiposValores.SelectedValue == ((int)EnumTiposValores.Prestamos).ToString())
                    this.ddlTiposValores_SelectedIndexChanged(null, EventArgs.Empty);
            }

            MiOrdenCobro.OrdenesCobrosValores.Remove(detalle);
            MiOrdenCobro.OrdenesCobrosValores = AyudaProgramacion.AcomodarIndices<CobOrdenesCobrosValores>(this.MiOrdenCobro.OrdenesCobrosValores);
            AyudaProgramacion.CargarGrillaListas<CobOrdenesCobrosValores>(MiOrdenCobro.OrdenesCobrosValores, false, this.gvFormasCobros, true);
            this.upCobrosValores.Update();
        }

        protected void gvFormasCobros_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                CobOrdenesCobrosValores detalle = (CobOrdenesCobrosValores)e.Row.DataItem;

                if (this.GestionControl == Gestion.Agregar)
                {
                    this.gvFormasCobros.Columns[6].Visible = true;
                    ibtnEliminar.Visible = true;
                }

                if (
                        (this.GestionControl == Gestion.Agregar
                        || this.GestionControl == Gestion.Autorizar)
                        && detalle.TipoValor.IdTipoValor==(int)EnumTiposValores.ChequeTercero
                    )

                {
                    TextBox txtFechaDiferido = (TextBox)e.Row.FindControl("txtFechaDiferido");
                    txtFechaDiferido.Text = detalle.FechaDiferido.HasValue ? detalle.FechaDiferido.Value.ToShortDateString() : string.Empty;

                    //HtmlControl dvFechaDiferido = (HtmlControl)e.Row.FindControl("dvFechaDiferido");
                    //dvFechaDiferido.Visible = true;
                    txtFechaDiferido.Enabled = true;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotal = (Label)e.Row.FindControl("lblTotal");
                lblTotal.Text = this.MiOrdenCobro.OrdenesCobrosValores.Sum(x => x.Importe).ToString("C2");
            }
        }

        protected void gvFormasCobros_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvFormasCobros.PageIndex = e.NewPageIndex;
            this.gvFormasCobros.DataSource = MiOrdenCobro;
            this.gvFormasCobros.DataBind();
        }

        private void LimpiarControles()
        {
            this.txtImporte.Text =string.Empty;
            this.txtFecha.Text = string.Empty;
            this.txtFechaDiferido.Text = string.Empty;
            this.txtNumeroCheque.Text = string.Empty;
            this.txtConcepto.Text = string.Empty;
            this.txtCUIT.Text = string.Empty;
            this.txtNumeroLote.Text = string.Empty;
            this.txtNumeroTarjeta.Text = string.Empty;
            this.txtObservaciones.Text = string.Empty;
            this.txtPosnet.Text = string.Empty;
            this.txtTitular.Text = string.Empty;
            this.txtTitularTarjeta.Text = string.Empty;
            this.txtVencimientoAA.Text = string.Empty;
            this.txtVencimientoMM.Text = string.Empty;
        }

        private void PersistirDatosGrilla()
        {
            if (this.MiOrdenCobro != null && this.MiOrdenCobro.OrdenesCobrosValores.Count > 0)
            {
                foreach (GridViewRow fila in this.gvFormasCobros.Rows)
                {
                    TextBox fechaDiferido = (TextBox)fila.FindControl("txtFechaDiferido");

                    if (fechaDiferido != null && fechaDiferido.Text != string.Empty)
                    {
                        this.MiOrdenCobro.OrdenesCobrosValores[fila.DataItemIndex].FechaDiferido = Convert.ToDateTime(fechaDiferido.Text);
                        this.MiOrdenCobro.OrdenesCobrosValores[fila.DataItemIndex].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiOrdenCobro.OrdenesCobrosValores[fila.DataItemIndex], this.GestionControl);
                    }
                }
            }
        }

        private void HabilitarRetencionPercepcion()
        {
            this.pnlAfipRetencionesPercepciones.Visible = true;
            this.pnlAfipRetencionesPercepciones.GroupingText = this.ddlTiposValores.SelectedItem.Text;
            this.lblTipoRetencionPercepcion.Text = this.ddlTiposValores.SelectedItem.Text;
            List<TGEListasValoresSistemasDetalles> lista = new List<TGEListasValoresSistemasDetalles>();
            if (Convert.ToInt32(this.ddlTiposValores.SelectedValue) == (int)EnumTiposValores.AfipTiposPercepciones)
            {
                lista = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.AFIPTiposPercepciones);
            }
            else if (Convert.ToInt32(this.ddlTiposValores.SelectedValue) == (int)EnumTiposValores.AfipTiposRetenciones)
            {
                lista = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.AFIPTiposRetenciones);
            }
            this.ddlTiposRetencionesPercepciones.DataSource = lista;
            this.ddlTiposRetencionesPercepciones.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTiposRetencionesPercepciones.DataTextField = "Descripcion";
            this.ddlTiposRetencionesPercepciones.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTiposRetencionesPercepciones, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.rfvTiposRetencionesPercepciones.Enabled = true;
        }

        #region Descuento Cargos

        protected void HabilitarCargo()
        {
            List<int> items = ProcesosDatosF.ProcesosProcesamientoObtenerProximosPeriodosCargos();
            this.ddlPrimerVtoDtoCargo.Items.Clear();
            foreach (int i in items)
                this.ddlPrimerVtoDtoCargo.Items.Add(new ListItem(i.ToString(), i.ToString()));

            TGEFormasCobrosAfiliados filtro = new TGEFormasCobrosAfiliados();
            filtro.IdAfiliado = this.MiOrdenCobro.Afiliado.IdAfiliado;
            filtro.Estado.IdEstado = (int)Estados.Activo;
            this.MisFormasCobrosAfiliados = TGEGeneralesF.FormasCobrosAfiliadosObtenerListaFiltro(filtro);

            ddlFormasCobros.DataSource = this.MisFormasCobrosAfiliados;//TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.AFIPTiposPercepciones);
            ddlFormasCobros.DataValueField = "IdFormaCobroAfiliado";
            ddlFormasCobros.DataTextField = "FormaCobroDescripcion";
            ddlFormasCobros.DataBind();
            if (ddlFormasCobros.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(ddlFormasCobros, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        #endregion

        #region Prestamos

        private void HabilitarPrestamo()
        {
            this.MiPrestamo = new PrePrestamos();
            AyudaProgramacion.MatchObjectProperties(this.MiOrdenCobro.Afiliado, this.MiPrestamo.Afiliado);

            List<int> items = ProcesosDatosF.ProcesosProcesamientoObtenerProximosPeriodosCargos();
            this.ddlPeriodoPrimerVtoPrestamo.Items.Clear();
            foreach (int i in items)
                this.ddlPeriodoPrimerVtoPrestamo.Items.Add(new ListItem(i.ToString(), i.ToString()));

            this.MisTiposOperacionesPrestamos = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(EnumTGETiposFuncionalidades.Cobros);
            
            this.ddlTipoOperacionPrestamo.DataSource = this.MisTiposOperacionesPrestamos;
            this.ddlTipoOperacionPrestamo.DataValueField = "IdTipoOperacion";
            this.ddlTipoOperacionPrestamo.DataTextField = "TipoOperacion";
            this.ddlTipoOperacionPrestamo.DataBind();
            if (this.ddlTipoOperacionPrestamo.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoOperacionPrestamo, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlTipoOperacionPrestamo_SelectedIndexChanged(null, EventArgs.Empty);

            this.ctrCamposValores.IniciarControl(this.MiPrestamo, new Objeto(), this.GestionControl);
        }

        protected void ddlTipoOperacionPrestamo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlFormasCobrosPrestamos.Items.Clear();
            ddlFormasCobrosPrestamos.SelectedIndex = -1;
            ddlFormasCobrosPrestamos.SelectedValue = null;
            ddlFormasCobrosPrestamos.ClearSelection();

            if (!string.IsNullOrEmpty(ddlTipoOperacionPrestamo.SelectedValue))
            {
                //this.MiPrestamo.TipoOperacion.IdTipoOperacion = Convert.ToInt32(ddlTipoOperacionPrestamo.SelectedValue);
                this.MiPrestamo.TipoOperacion = this.MisTiposOperacionesPrestamos[this.ddlTipoOperacionPrestamo.SelectedIndex];

                CarTiposCargos cargo = new CarTiposCargos();
                cargo.TipoOperacion.IdTipoOperacion = this.MiPrestamo.TipoOperacion.IdTipoOperacion;
                cargo.Estado.IdEstado = (int)Estados.Activo;
                List<CarTiposCargos> lista = CargosF.TiposCargosObtenerListaFiltro(cargo);
                if (lista.Count == 0)
                {
                    this.ddlTipoOperacionPrestamo.SelectedValue = string.Empty;
                    this.MostrarMensaje("TipoCargoNoDefinidoTipoOperacion", true);
                    return;
                }
                this.MiPrestamo.TipoCargo = lista.FirstOrDefault();

                TGEFormasCobrosAfiliados filtro = new TGEFormasCobrosAfiliados();
                filtro.IdAfiliado = this.MiOrdenCobro.Afiliado.IdAfiliado;
                filtro.Estado.IdEstado = (int)Estados.Activo;
                this.MisFormasCobrosAfiliados = TGEGeneralesF.FormasCobrosAfiliadosObtenerListaFiltro(filtro);

                this.ddlFormasCobrosPrestamos.DataSource = this.MisFormasCobrosAfiliados;
                this.ddlFormasCobrosPrestamos.DataValueField = "IdFormaCobroAfiliado";
                this.ddlFormasCobrosPrestamos.DataTextField = "FormaCobroDescripcion";
                this.ddlFormasCobrosPrestamos.DataBind();

                this.ctrCamposValores.IniciarControl(this.MiPrestamo, this.MiPrestamo.TipoOperacion, this.GestionControl);
            }

            if (this.ddlFormasCobrosPrestamos.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlFormasCobrosPrestamos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            ddlFormasCobrosPrestamos_SelectedIndexChanged(null, EventArgs.Empty);
        }

        protected void ddlFormasCobrosPrestamos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPlanesPrestamos.Items.Clear();
            ddlPlanesPrestamos.SelectedIndex = -1;
            ddlPlanesPrestamos.SelectedValue = null;
            ddlPlanesPrestamos.ClearSelection();

            ListItem itemPlanAnt=new ListItem();
            if (!string.IsNullOrEmpty(ddlFormasCobrosPrestamos.SelectedValue) &&
                !string.IsNullOrEmpty(ddlTipoOperacionPrestamo.SelectedValue))
            {
                this.MiPrestamo.FormaCobroAfiliado = this.MisFormasCobrosAfiliados[ddlFormasCobrosPrestamos.SelectedIndex];

                PrePrestamosPlanes planFiltro = new PrePrestamosPlanes();
                planFiltro.Estado.IdEstado = (int)Estados.Activo;
                planFiltro.FormaCobro.IdFormaCobro = this.MisFormasCobrosAfiliados[ddlFormasCobrosPrestamos.SelectedIndex].FormaCobro.IdFormaCobro;
                planFiltro.TipoOperacion.IdTipoOperacion = this.MisTiposOperacionesPrestamos[ddlTipoOperacionPrestamo.SelectedIndex].IdTipoOperacion;

                itemPlanAnt = ddlPlanesPrestamos.Items.FindByValue(ddlPlanesPrestamos.SelectedValue);

                this.MisPrestamosPlanes = PrePrestamosF.PrestamosPlanesObtenerLista(planFiltro);
                ddlPlanesPrestamos.DataSource = this.MisPrestamosPlanes;
                ddlPlanesPrestamos.DataValueField = "IdPrestamoPlan";
                ddlPlanesPrestamos.DataTextField = "Descripcion";
                ddlPlanesPrestamos.DataBind();   
            }
            if (this.ddlPlanesPrestamos.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlPlanesPrestamos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            if (itemPlanAnt!=null && itemPlanAnt.Value != string.Empty)
            {
                ListItem item = this.ddlPlanesPrestamos.Items.FindByValue(itemPlanAnt.Value);
                if (item != null)
                    this.ddlPlanesPrestamos.SelectedValue = item.Value;
            }

            ddlPlanesPrestamos_SelectedIndexChanged(sender, e);
        }

        protected void ddlPlanesPrestamos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlPlanesPrestamos.SelectedValue)) 
            {
                this.MisPrestamosPlanes[ddlPlanesPrestamos.SelectedIndex].FechaAlta = DateTime.Now;
                this.MisPrestamosPlanes[ddlPlanesPrestamos.SelectedIndex].PrestamoPlanTasa = PrePrestamosF.PrestamosPlanesTasasObtenerTasaActiva(this.MisPrestamosPlanes[ddlPlanesPrestamos.SelectedIndex]);
                this.MiPrestamo.PrestamoPlan = this.MisPrestamosPlanes[ddlPlanesPrestamos.SelectedIndex];
            }
            this.txtCantidadCuotas_TextChanged(null, EventArgs.Empty);
        }

        protected void txtCantidadCuotas_TextChanged(object sender, EventArgs e)
        {
            this.MiPrestamo.CantidadCuotas = Convert.ToInt32( this.txtCantidadCuotas.Decimal);
            this.CalcularPrestamo();
        }

        private void CalcularPrestamo()
        {
            if (this.MiPrestamo.TipoOperacion.IdTipoOperacion > 0
                && this.MiPrestamo.FormaCobroAfiliado.IdFormaCobroAfiliado > 0
                && this.MiPrestamo.PrestamoPlan.IdPrestamoPlan > 0
                && this.txtCantidadCuotas.Decimal > 0)
            {
                this.MiPrestamo.PrestamosCuotas.Clear();
                this.MiPrestamo.PeriodoPrimerVencimiento = Convert.ToInt32(this.ddlPeriodoPrimerVtoPrestamo.SelectedValue);
                this.MiPrestamo.ImporteSolicitado = this.txtImporte.Decimal;
                this.MiPrestamo.ImporteAutorizado = this.txtImporte.Decimal;
                if (PrePrestamosF.PrestamosAgregarPrevio(this.MiPrestamo))
                {
                    this.txtImporteCuotas.Text = this.MiPrestamo.ImporteCuota.ToString("C2");
                }
                else
                {
                    this.MostrarMensaje(this.MiPrestamo.CodigoMensaje, true, this.MiPrestamo.CodigoMensajeArgs);
                }
            }
            else
                this.txtImporteCuotas.Text = (0).ToString("C2");

        }

        #endregion

        #region Remitos
        private void HabilitarRemitos()
        {
            TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.CobroConPrestamosAsociarRemitos);
            bool mostrar = !string.IsNullOrEmpty(valor.ParametroValor) ? Convert.ToBoolean(valor.ParametroValor) : false;
            if (!mostrar)
                return;

            this.pnlRemitos.Visible = true;
            if (this.MisRemitos == null)
            {
                VTARemitos remito = new VTARemitos();
                remito.Afiliado.IdAfiliado = this.MiOrdenCobro.Afiliado.IdAfiliado;
                this.MisRemitos = FacturasF.RemitosObtenerListaOrdenesCobrosPendientes(remito);
                this.MisRemitosSeleccionados = new List<VTARemitos>();
            }
            this.ddlRemitos.DataSource = this.MisRemitos;
            this.ddlRemitos.DataValueField = "IdRemito";
            this.ddlRemitos.DataTextField = "DescripcionCombo";
            this.ddlRemitos.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlRemitos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.gvRemitosDatos.DataSource = this.MisRemitosSeleccionados;
            this.gvRemitosDatos.DataBind();
        }

        protected void ddlRemitos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ddlRemitos.SelectedValue))
                return;

            VTARemitos remito = this.MisRemitos[this.ddlRemitos.SelectedIndex];
            remito.EstadoColeccion = EstadoColecciones.Agregado;
            this.MisRemitosSeleccionados.Add(remito);
            this.gvRemitosDatos.DataSource = this.MisRemitosSeleccionados;
            this.gvRemitosDatos.DataBind();

            this.MisRemitos.RemoveAt(this.ddlRemitos.SelectedIndex);
            this.MisRemitos = AyudaProgramacion.AcomodarIndices<VTARemitos>(this.MisRemitos);
            this.ddlRemitos.DataSource = this.MisRemitos;
            this.ddlRemitos.DataValueField = "IdRemito";
            this.ddlRemitos.DataTextField = "DescripcionCombo";
            this.ddlRemitos.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlRemitos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void gvRemitosDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            VTARemitos remito = this.MisRemitosSeleccionados[indiceColeccion];
            remito.EstadoColeccion = EstadoColecciones.SinCambio;
            if (e.CommandName == Gestion.Anular.ToString())
            {
                this.MisRemitos.Add(remito);
                this.MisRemitos = AyudaProgramacion.AcomodarIndices<VTARemitos>(this.MisRemitos);
                this.ddlRemitos.DataSource = this.MisRemitos;
                this.ddlRemitos.DataValueField = "IdRemito";
                this.ddlRemitos.DataTextField = "DescripcionCombo";
                this.ddlRemitos.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlRemitos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                this.MisRemitosSeleccionados.RemoveAt(indiceColeccion);
                this.MisRemitosSeleccionados = AyudaProgramacion.AcomodarIndices<VTARemitos>(this.MisRemitosSeleccionados);
                this.gvRemitosDatos.DataSource = this.MisRemitosSeleccionados;
                this.gvRemitosDatos.DataBind();

            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                //this.ctrPopUpComprobantes.CargarReporte(remito, EnumTGEComprobantes.VTARemitos);
                VTARemitos remitoPDF = new VTARemitos();
                remitoPDF.IdRemito = remito.IdRemito;
                remitoPDF = FacturasF.RemitosObtenerArchivo(remitoPDF);

                TGEArchivos archivo = new TGEArchivos();
                archivo.Archivo = remitoPDF.RemitoPDF;
                archivo.NombreArchivo = remito.NumeroRemitoCompleto;
                archivo.TipoArchivo = "application/pdf";
                ExportPDF.ConvertirArchivoEnPdf(this.upRemitos, archivo);
            }
        }

        #endregion
    }
}