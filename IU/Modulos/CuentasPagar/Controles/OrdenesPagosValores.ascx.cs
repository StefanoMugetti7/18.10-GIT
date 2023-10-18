using Bancos;
using Bancos.Entidades;
using Comunes.Entidades;
using CuentasPagar.Entidades;
using CuentasPagar.FachadaNegocio;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace IU.Modulos.CuentasPagar.Controles
{
    public partial class OrdenesPagosValores : ControlesSeguros
    {
        private CapOrdenesPagos MiOrdenPago
        {
            get { return (CapOrdenesPagos)Session[this.MiSessionPagina + "OrdenesPagosValoresMiOrdenPago"]; }
            set { Session[this.MiSessionPagina + "OrdenesPagosValoresMiOrdenPago"] = value; }
        }

        private List<TESBancosCuentas> MisBancosCuentas
        {
            get { return (List<TESBancosCuentas>)Session[this.MiSessionPagina + "OrdenesPagosValoresMisBancosCuentas"]; }
            set { Session[this.MiSessionPagina + "OrdenesPagosValoresMisBancosCuentas"] = value; }
        }

        private List<TESBancosCuentas> MisBancosCuentasTarjetas
        {
            get { return (List<TESBancosCuentas>)Session[this.MiSessionPagina + "OrdenesPagosValoresMisBancosCuentasTarjetas"]; }
            set { Session[this.MiSessionPagina + "OrdenesPagosValoresMisBancosCuentasTarjetas"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrBuscarCheque.ChequesBuscarSeleccionar += new IU.Modulos.Tesoreria.Controles.ChequesTercerosPopUp.ChequesBuscarEventHandler(ctrBuscarChequesPopUp_ChequesTercerosSeleccionar);
            //if (this.IsPostBack)
            //    this.PersistirDatosGrilla();
        }

        public void IniciarControl(CapOrdenesPagos pOrdenPago, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiOrdenPago = pOrdenPago;
            switch (pGestion)
            {
                case Gestion.Agregar:
                    this.pnlDetalle.Visible = true;
                    //EnumTGETiposFuncionalidades tipoFunc = (EnumTGETiposFuncionalidades)this.paginaSegura.paginaActual.IdTipoFuncionalidad;
                    TGETiposFuncionalidades tipoFunc = new TGETiposFuncionalidades();
                    tipoFunc.IdTipoFuncionalidad = this.paginaSegura.paginaActual.IdTipoFuncionalidad;
                    tipoFunc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    List<TGETiposValores> listaValores = TGEGeneralesF.TiposValoresObtenerLista(tipoFunc);

                    this.ddlTiposValores.DataSource = AyudaProgramacion.AcomodarIndices<TGETiposValores>(listaValores).ToList();
                    this.ddlTiposValores.DataValueField = "IdTipoValor";
                    this.ddlTiposValores.DataTextField = "TipoValor";
                    this.ddlTiposValores.DataBind();

                    if (this.ddlTiposValores.Items.Count == 0 || this.ddlTiposValores.Items.Count > 1)
                        AyudaProgramacion.AgregarItemSeleccione(this.ddlTiposValores, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                    else
                        this.ddlTiposValores_SelectedIndexChanged(null, EventArgs.Empty);

                    break;
                case Gestion.Anular:
                case Gestion.AnularConfirmar:
                case Gestion.Autorizar:
                case Gestion.Modificar:
                case Gestion.Consultar:
                    this.ddlTiposValores.Enabled = false;
                    this.pnlDetalle.Visible = false;
                    AyudaProgramacion.CargarGrillaListas<CapOrdenesPagosValores>(this.MiOrdenPago.OrdenesPagosValore, false, this.gvFormasCobros, true);
                    break;
                default:
                    break;
            }
        }
        private void HabilitarChequesTerceros()
        {

            this.btnAgregarCheque.Visible = true;
            //this.ddlBancosCuentasCheques.DataSource = this.MisBancosCuentas;
            //this.ddlBancosCuentasCheques.DataValueField = "IdBancoCuenta";
            //this.ddlBancosCuentasCheques.DataTextField = "DescripcionFilialBancoTipoCuentaNumero";
            //this.ddlBancosCuentasCheques.DataBind();
            ////this.rfvFecha.Enabled = true;
            ////this.rfvTitular.Enabled = true;
            ////this.rfvCUIT.Enabled = true;
            ////this.rfvBancos.Enabled = true;
        }
        private void HabilitarTransferencias()
        {
            this.pnlTransferencias.Visible = true;

            TESBancosCuentas bancoCuenta = new TESBancosCuentas();
            bancoCuenta.Estado.IdEstado = (int)Estados.Activo;
            bancoCuenta.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
            bancoCuenta.Moneda.IdMoneda = (int)EnumTGEMonedas.PesosArgentinos;
            bancoCuenta.Filtro = this.ddlTiposValores.SelectedValue;
            this.MisBancosCuentas = BancosF.BancosCuentasObtenerListaFiltroTransferencia(bancoCuenta);

            this.ddlBancosCuentasTransferencias.DataSource = this.MisBancosCuentas;
            this.ddlBancosCuentasTransferencias.DataValueField = "IdBancoCuenta";
            this.ddlBancosCuentasTransferencias.DataTextField = "DescripcionFilialBancoTipoCuentaNumero";
            this.ddlBancosCuentasTransferencias.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlBancosCuentasTransferencias, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            if (Convert.ToInt32(ddlTiposValores.SelectedValue) == (int)EnumTiposValores.Transferencia)
            {
                TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.CuentaBancariaOpcionalOrdenesPagoTransferencia);
                bool bvalor = false;
                bool.TryParse(valor.ParametroValor, out bvalor);
                this.rfvBancosCuentasTransferencias.Enabled = !bvalor;
                this.rfvFechaTransferencia.Enabled = !bvalor;

                if (MiOrdenPago.Entidad.IdRefEntidad > 0)
                {
                    CapOrdenesPagosValores aux = CuentasPagarF.OrdenesPagosObtenerCbu(MiOrdenPago);
                    this.txtCbu.Text = aux.CBU;
                    this.lblDatosCbu.Text = CuentasPagarF.OrdenesPagosObtenerDatosCbu(aux).DatosCBU;
                }
            }
            //else if (Convert.ToInt32(ddlTiposValores.SelectedValue) == (int)EnumTiposValores.EfectivoFondoFijo)
            //{
            //    rfvNumeroTransferencia.Enabled = false;
            //}
            ScriptManager.RegisterStartupScript(this.UpdatePanel2, this.UpdatePanel2.GetType(), "HabilitarTransferencias", "HabilitarTransferencias();", true);
        }
        private void HabilitarTarjetas()
        {
            this.pnlTarjetasCredito.Visible = true;

            TESBancosCuentas bancoCuenta = new TESBancosCuentas();
            bancoCuenta.Estado.IdEstado = (int)Estados.Activo;
            bancoCuenta.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
            bancoCuenta.Moneda.IdMoneda = (int)EnumTGEMonedas.PesosArgentinos;
            bancoCuenta.Filtro = this.ddlTiposValores.SelectedValue;
            this.MisBancosCuentasTarjetas = BancosF.BancosCuentasObtenerListaFiltroTransferencia(bancoCuenta);

            this.ddlBancosCuentasTarjetas.DataSource = this.MisBancosCuentasTarjetas;
            this.ddlBancosCuentasTarjetas.DataValueField = "IdBancoCuenta";
            this.ddlBancosCuentasTarjetas.DataTextField = "DescripcionFilialBancoTipoCuentaNumero";
            this.ddlBancosCuentasTarjetas.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlBancosCuentasTarjetas, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.rfvFechaTarjeta.Enabled = true;
            this.rfvBancosCuentasTarjetas.Enabled = true;
        }

        private void HabilitarChequesPropios()
        {
            TESBancosCuentas bancoCuenta = new TESBancosCuentas();
            bancoCuenta.Estado.IdEstado = (int)Estados.Activo;
            bancoCuenta.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
            bancoCuenta.Moneda.IdMoneda = (int)EnumTGEMonedas.PesosArgentinos;
            bancoCuenta.Filtro = this.ddlTiposValores.SelectedValue;
            this.MisBancosCuentas = BancosF.BancosCuentasObtenerListaFiltroTransferencia(bancoCuenta);
            this.ddlBancosCuentasCheques.DataSource = this.MisBancosCuentas;
            this.ddlBancosCuentasCheques.DataValueField = "IdBancoCuenta";
            this.ddlBancosCuentasCheques.DataTextField = "DescripcionFilialBancoTipoCuentaNumero";
            this.ddlBancosCuentasCheques.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlBancosCuentasCheques, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.rfvFecha.Enabled = true;
            //this.rfvChequeDiferido.Enabled = true;
            this.rfvNumeroCheque.Enabled = true;
            this.rfvBancosCuentasCheques.Enabled = true;
        }

        private void HabilitarRetencionPercepcion()
        {
            this.pnlAfipRetencionesPercepciones.Visible = true;
            this.pnlAfipRetencionesPercepciones.GroupingText = this.ddlTiposValores.SelectedItem.Text;
            this.lblTipoRecionPercepcion.Text = this.ddlTiposValores.SelectedItem.Text;
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

        public List<CapOrdenesPagosValores> ObtenerOrdenesPagosValores()
        {
            this.PersistirDatosGrilla();
            return this.MiOrdenPago.OrdenesPagosValore;
        }

        protected void ddlTiposValores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ddlTiposValores.SelectedValue))
                return;

            TGETiposValores tipoValor = new TGETiposValores();
            tipoValor.IdTipoValor = Convert.ToInt32(this.ddlTiposValores.SelectedValue);

            this.btnIngresarCobro.Visible = true;
            this.btnAgregarCheque.Visible = false;
            this.pnlChequesPropios.Visible = false;
            this.pnlTransferencias.Visible = false;
            this.pnlTarjetasCredito.Visible = false;
            this.pnlAfipRetencionesPercepciones.Visible = false;
            this.rfvFecha.Enabled = false;
            //this.rfvChequeDiferido.Enabled = false;
            this.rfvNumeroCheque.Enabled = false;
            this.rfvBancosCuentasCheques.Enabled = false;
            this.rfvBancosCuentasTransferencias.Enabled = false;
            this.rfvBancosCuentasTarjetas.Enabled = false;
            this.rfvFechaTarjeta.Enabled = false;
            this.rfvTiposRetencionesPercepciones.Enabled = false;

            if (tipoValor.IdTipoValor == (int)EnumTiposValores.Cheque)
            {
                this.pnlChequesPropios.Visible = true;
                this.HabilitarChequesPropios();
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia || tipoValor.IdTipoValor == (int)EnumTiposValores.EfectivoFondoFijo)
            {
                this.pnlTransferencias.Visible = true;
                this.HabilitarTransferencias();
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito)
            {
                this.pnlTarjetasCredito.Visible = true;
                this.HabilitarTarjetas();
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
            {
                this.btnIngresarCobro.Visible = false;
                this.HabilitarChequesTerceros();
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.AfipTiposPercepciones
                || tipoValor.IdTipoValor == (int)EnumTiposValores.AfipTiposRetenciones)
            {
                this.pnlAfipRetencionesPercepciones.Visible = true;
                this.HabilitarRetencionPercepcion();
            }
            this.UpdatePanel2.Update();
        }
        protected void btnIngresarCobro_Click(object sender, EventArgs e)
        {
            this.Page.Validate("IngresarCobro");
            if (!this.Page.IsValid)
            {
                this.UpdatePanel2.Update();
                return;
            }

            TGETiposValores tipoValor = new TGETiposValores();
            tipoValor.IdTipoValor = Convert.ToInt32(this.ddlTiposValores.SelectedValue);
            tipoValor.TipoValor = this.ddlTiposValores.SelectedItem.Text;
            if (tipoValor.IdTipoValor == (int)EnumTiposValores.Cheque)
            {
                this.Page.Validate("IngresarCheque");
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia || tipoValor.IdTipoValor == (int)EnumTiposValores.EfectivoFondoFijo)
            {
                this.Page.Validate("IngresarTransferencia");
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito)
            {
                this.Page.Validate("IngresarTarjeta");
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.AfipTiposRetenciones
                || tipoValor.IdTipoValor == (int)EnumTiposValores.AfipTiposPercepciones)
            {
                this.Page.Validate("IngresarRetencionPercepcion");
            }
            if (!this.Page.IsValid)
            {
                this.UpdatePanel2.Update();
                return;
            }
            if (txtImporte.Text == "$0,00")
            {
                this.MostrarMensaje("Debe ingresar un valor mayor a $0,00", true);
                return;
            }
            CapOrdenesPagosValores valor;
            //EFECTIVO
            if (tipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo)
            {
                if (this.MiOrdenPago.OrdenesPagosValore.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo))
                    this.MiOrdenPago.OrdenesPagosValore.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo).Importe = this.txtImporte.Decimal;
                else
                {
                    valor = new CapOrdenesPagosValores();
                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                    valor.TipoValor = tipoValor;
                    valor.Importe = this.txtImporte.Decimal;
                    this.MiOrdenPago.OrdenesPagosValore.Add(valor);
                    valor.IndiceColeccion = this.MiOrdenPago.OrdenesPagosValore.IndexOf(valor);
                }
            }//CHEQUE
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.Cheque)
            {
                valor = new CapOrdenesPagosValores();
                valor.EstadoColeccion = EstadoColecciones.Agregado;
                valor.TipoValor = tipoValor;
                this.MiOrdenPago.OrdenesPagosValore.Add(valor);
                valor.IndiceColeccion = this.MiOrdenPago.OrdenesPagosValore.IndexOf(valor);
                //Cheques Propios
                valor.Fecha = Convert.ToDateTime(this.txtFecha.Text);
                valor.FechaDiferido = this.txtFechaDiferido.Text == string.Empty ? default(Nullable<DateTime>) : Convert.ToDateTime(this.txtFechaDiferido.Text);
                valor.NumeroCheque = this.txtNumeroCheque.Text;
                valor.Importe = this.txtImporte.Decimal;
                valor.ChequePropio = true;
                valor.BancoCuenta = this.MisBancosCuentas[this.ddlBancosCuentasCheques.SelectedIndex];
                this.LimpiarControlesCheques();
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia || tipoValor.IdTipoValor == (int)EnumTiposValores.EfectivoFondoFijo)
            {
                valor = new CapOrdenesPagosValores();
                //if(this.txtCbu.Text.Length != 22)
                //{
                //    this.rfvCbu.IsValid = false;
                //    this.MostrarMensaje("El CBU ingresado no es valido",true);
                //    return;
                //}
                valor.EstadoColeccion = EstadoColecciones.Agregado;
                valor.TipoValor = tipoValor;
                this.MiOrdenPago.OrdenesPagosValore.Add(valor);
                valor.IndiceColeccion = this.MiOrdenPago.OrdenesPagosValore.IndexOf(valor);
                valor.Fecha = this.txtFechaTransferencia.Text == string.Empty ? this.MiOrdenPago.FechaPago : Convert.ToDateTime(this.txtFechaTransferencia.Text);

                if (!string.IsNullOrEmpty(this.ddlBancosCuentasTransferencias.SelectedValue))
                    valor.BancoCuenta = this.MisBancosCuentas[this.ddlBancosCuentasTransferencias.SelectedIndex];
                else
                    valor.BancoCuenta = new TESBancosCuentas();
                valor.Importe = this.txtImporte.Decimal;
                valor.NumeroCheque = this.txtNumeroTransferencia.Text;
                valor.CBU = this.txtCbu.Text;
                valor.DatosCBU = this.lblDatosCbu.Text;
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito)
            {
                if (Convert.ToInt32(txtCantidadCuotas.Text) < 1)
                {
                    MostrarMensaje("La cantidad de cuotas debe ser mayor a 0.", true);
                    return;
                }

                valor = new CapOrdenesPagosValores();
                valor.EstadoColeccion = EstadoColecciones.Agregado;
                valor.TipoValor = tipoValor;
                this.MiOrdenPago.OrdenesPagosValore.Add(valor);
                valor.IndiceColeccion = this.MiOrdenPago.OrdenesPagosValore.IndexOf(valor);
                valor.Fecha = Convert.ToDateTime(this.txtFechaTarjeta.Text);
                valor.BancoCuenta = this.MisBancosCuentasTarjetas[this.ddlBancosCuentasTarjetas.SelectedIndex];
                valor.Importe = this.txtImporte.Decimal;
                valor.CantidadCuotas = this.txtCantidadCuotas.Text == string.Empty ? 0 : Convert.ToInt32(txtCantidadCuotas.Text);
                valor.ImporteCuota = valor.Importe / Convert.ToInt32(txtCantidadCuotas.Text);
            }
            else if (tipoValor.IdTipoValor == (int)EnumTiposValores.AfipTiposPercepciones
                || tipoValor.IdTipoValor == (int)EnumTiposValores.AfipTiposRetenciones)
            {
                valor = new CapOrdenesPagosValores();
                valor.EstadoColeccion = EstadoColecciones.Agregado;
                valor.TipoValor = tipoValor;
                this.MiOrdenPago.OrdenesPagosValore.Add(valor);
                valor.IndiceColeccion = this.MiOrdenPago.OrdenesPagosValore.IndexOf(valor);
                valor.Importe = this.txtImporte.Decimal;
                valor.ListaValorSistemaDetalle.IdListaValorSistemaDetalle = Convert.ToInt32(this.ddlTiposRetencionesPercepciones.SelectedValue);
                valor.ListaValorSistemaDetalle.Descripcion = this.ddlTiposRetencionesPercepciones.SelectedItem.Text;
            }

            this.PersistirDatosGrilla();
            AyudaProgramacion.CargarGrillaListas<CapOrdenesPagosValores>(this.MiOrdenPago.OrdenesPagosValore, false, this.gvFormasCobros, true);
            this.txtImporte.Text = string.Empty;
            this.txtFechaTarjeta.Text = string.Empty;
            this.txtCantidadCuotas.Text = string.Empty;
            this.UpdatePanel2.Update();
        }

        //private void LimpiarControlesCheques3()
        //{
        //    this.txtFecha3.Text = string.Empty;
        //    this.txtFechaDiferido3.Text = string.Empty;
        //    this.txtNumeroCheque3.Text = string.Empty;
        //    this.txtTitular3.Text = string.Empty;
        //    this.txtCUIT3.Text = string.Empty;
        //}

        protected void txtCbu_TextChanged(object sender, EventArgs e)
        {
            //if(this.txtCbu.Text.Length != 22)
            //{
            //    this.lblDatosCbu.Text = "CBU Invalido.";
            //    return;
            //}
            CapOrdenesPagosValores aux = new CapOrdenesPagosValores();
            aux.CBU = this.txtCbu.Text;
            aux = CuentasPagarF.OrdenesPagosObtenerDatosCbu(aux);
            this.lblDatosCbu.Text = aux.DatosCBU;
        }

        #region Chueques Terceros Pop Up

        protected void btnBuscarCheque_Click(object sender, EventArgs e)
        {
            List<TESCheques> filtro = new List<TESCheques>();
            foreach (CapOrdenesPagosValores valores in this.MiOrdenPago.OrdenesPagosValore)
            {
                if (valores.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
                {
                    filtro.Add(valores.Cheque);
                }
            }

            this.ctrBuscarCheque.IniciarControl(filtro);
        }

        void ctrBuscarChequesPopUp_ChequesTercerosSeleccionar(global::Bancos.Entidades.TESCheques e)
        {
            //this.MapearObjetoAControlesCheques(e);
            CapOrdenesPagosValores valor = new CapOrdenesPagosValores();
            valor.Cheque = e;
            valor.Importe = e.Importe;
            valor.NumeroCheque = e.NumeroCheque;
            valor.FechaDiferido = e.FechaDiferido;
            valor.Fecha = e.Fecha;
            valor.EstadoColeccion = EstadoColecciones.Agregado;
            //tipo valor 
            TGETiposValores tipoValor = new TGETiposValores();
            tipoValor.IdTipoValor = Convert.ToInt32(this.ddlTiposValores.SelectedValue);
            tipoValor.TipoValor = this.ddlTiposValores.SelectedItem.Text;
            valor.TipoValor = tipoValor;
            this.MiOrdenPago.OrdenesPagosValore.Add(valor);
            valor.IndiceColeccion = this.MiOrdenPago.OrdenesPagosValore.IndexOf(valor);
            //CARGO EN GRILLA
            this.PersistirDatosGrilla();
            AyudaProgramacion.CargarGrillaListas<CapOrdenesPagosValores>(this.MiOrdenPago.OrdenesPagosValore, false, this.gvFormasCobros, true);
            this.txtImporte.Text = string.Empty;
            this.UpdatePanel2.Update();

        }

        //private void MapearObjetoAControlesCheques(TESCheques e)
        //{
        //    //MAP ALL DATA
        //    this.txtImporte.Text = e.Importe.ToString();
        //    this.txtFecha3.Text = e.Fecha.ToShortDateString();
        //    this.txtFechaDiferido3.Text = e.FechaDiferido.ToString();
        //    this.txtNumeroCheque3.Text = e.NumeroCheque;
        //    this.txtTitular3.Text = e.TitularCheque;
        //    this.txtCUIT3.Text = e.CUIT;

        //}


        #endregion

        protected void gvFormasCobros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Borrar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CapOrdenesPagosValores detalle = MiOrdenPago.OrdenesPagosValore[indiceColeccion];
            MiOrdenPago.OrdenesPagosValore.Remove(detalle);
            MiOrdenPago.OrdenesPagosValore = AyudaProgramacion.AcomodarIndices<CapOrdenesPagosValores>(this.MiOrdenPago.OrdenesPagosValore);
            AyudaProgramacion.CargarGrillaListas<CapOrdenesPagosValores>(MiOrdenPago.OrdenesPagosValore, false, this.gvFormasCobros, true); ;
            this.UpdatePanel2.Update();
        }

        protected void gvFormasCobros_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                CapOrdenesPagosValores detalle = (CapOrdenesPagosValores)e.Row.DataItem;

                if (this.GestionControl == Gestion.Agregar)
                    ibtnEliminar.Visible = true;

                if (
                        //    (this.MiOrdenPago.Estado.IdEstado == (int)EstadosOrdenesPago.Activo
                        //    || this.MiOrdenPago.Estado.IdEstado == (int)EstadosOrdenesPago.Autorizado)
                        //&& 
                        (this.GestionControl == Gestion.Agregar
                        || this.GestionControl == Gestion.Autorizar)
                        && detalle.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque
                    )

                {
                    TextBox txtFechaDiferido = (TextBox)e.Row.FindControl("txtFechaDiferido");
                    txtFechaDiferido.Text = detalle.FechaDiferido.HasValue ? detalle.FechaDiferido.Value.ToShortDateString() : string.Empty;

                    HtmlControl dvFechaDiferido = (HtmlControl)e.Row.FindControl("dvFechaDiferido");
                    dvFechaDiferido.Visible = true;
                    txtFechaDiferido.Enabled = true;
                    //txtFechaBanco.Text = DateTime.Now.ToShortDateString();
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotal = (Label)e.Row.FindControl("lblTotal");
                lblTotal.Text = this.MiOrdenPago.OrdenesPagosValore.Sum(x => x.Importe).ToString("C2");
                //    int cellCount = e.Row.Cells.Count;
                //    e.Row.Cells.Clear();
                //    TableCell tableCell = new TableCell();
                //    tableCell.ColumnSpan = cellCount - 1;
                //    tableCell.HorizontalAlign = HorizontalAlign.Right;
                //    tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalImporte"), MisPagosValores.Sum(x => x.Importe).ToString("C2"));
                //    e.Row.Cells.Add(tableCell);
                //    e.Row.Cells.Add(new TableCell());
            }
        }

        protected void gvFormasCobros_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvFormasCobros.PageIndex = e.NewPageIndex;
            this.gvFormasCobros.DataSource = MiOrdenPago;
            this.gvFormasCobros.DataBind();
        }

        private void LimpiarControlesCheques()
        {
            this.txtFecha.Text = string.Empty;
            this.txtFechaDiferido.Text = string.Empty;
            this.txtNumeroCheque.Text = string.Empty;
        }

        private void PersistirDatosGrilla()
        {
            foreach (GridViewRow fila in this.gvFormasCobros.Rows)
            {
                TextBox fechaDiferido = (TextBox)fila.FindControl("txtFechaDiferido");
                if (fechaDiferido.Text != string.Empty)
                {
                    this.MiOrdenPago.OrdenesPagosValore[fila.DataItemIndex].FechaDiferido = Convert.ToDateTime(fechaDiferido.Text);
                    this.MiOrdenPago.OrdenesPagosValore[fila.DataItemIndex].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiOrdenPago.OrdenesPagosValore[fila.DataItemIndex], this.GestionControl);
                }
            }
        }
    }
}