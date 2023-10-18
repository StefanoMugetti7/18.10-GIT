using Bancos;
using Bancos.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Tesorerias;
using Tesorerias.Entidades;

namespace IU.Modulos.Tesoreria
{
    public partial class TesoreriasMovimientosAgregar : PaginaTesoreria
    {
        private List<TGETiposOperaciones> MisTiposOperaciones
        {
            get { return this.PropiedadObtenerValor<List<TGETiposOperaciones>>("TesoreriasMovimientosAgregarMisTiposOperaciones"); }
            set { this.PropiedadGuardarValor("TesoreriasMovimientosAgregarMisTiposOperaciones", value); }
        }

        private List<TESCajas> MisCajas
        {
            get { return this.PropiedadObtenerValor<List<TESCajas>>("TesoreriasMovimientosAgregarMisCajas"); }
            set { this.PropiedadGuardarValor("TesoreriasMovimientosAgregarMisCajas", value); }
        }

        private List<TGEMonedas> MisMonedas
        {
            get { return (List<TGEMonedas>)Session[this.MiSessionPagina + "TesoreriasMovimientosAgregarMisMonedas"]; }
            set { Session[this.MiSessionPagina + "TesoreriasMovimientosAgregarMisMonedas"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
                string funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarAccion"));
                this.btnAceptar.Attributes.Add("OnClick", funcion);
                this.CargarCombos();
                this.DeshabilitarControles();
                TESTesoreriasMovimientos aux = new TESTesoreriasMovimientos();
                if (this.MisParametrosUrl.ContainsKey("IdTesoreriaMovimiento"))
                {
                    aux.IdTesoreriaMovimiento = Convert.ToInt32(this.MisParametrosUrl["IdTesoreriaMovimiento"].ToString());
                    aux = TesoreriasF.TesoreriaMovimientosObtenerDatosCompletos(aux);
                    this.ConsultarMovimiento(aux);
                    this.ctrArchivos.IniciarControl(aux, Gestion.Consultar);

                    this.MisParametrosUrl.Remove("IdTesoreriaMovimiento");
                }
                else
                {
                    this.ctrArchivos.IniciarControl(aux, Gestion.Agregar);
                }

            }
        }

        void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/TesoreriasMovimientos.aspx"), true);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Validate("Aceptar");
            if (!this.IsValid)
                return;

            if (this.txtImporte.Decimal <= 0)
            {
                this.MostrarMensaje(this.ObtenerMensajeSistema("ValidarImporteMayorCero"), true);
                return;
            }

            TESTesoreriasMovimientos movimiento = new TESTesoreriasMovimientos();
            TGEMonedas mon = MisMonedas.Find(x => x.IdMoneda == Convert.ToInt32(this.ddlMoneda.SelectedValue));
            movimiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            var tesoreriaMoneda = this.MiTesoreria.TesoreriasMonedas.Find(x => x.Moneda.IdMoneda == mon.IdMoneda);
            movimiento.TipoOperacion = this.MisTiposOperaciones[this.ddlTipoOperacion.SelectedIndex];
            movimiento.TipoValor.IdTipoValor = (int)EnumTiposValores.Efectivo;
            movimiento.Fecha = this.MiTesoreria.FechaAbrir;
            movimiento.Importe = this.txtImporte.Decimal;
            //movimiento.Descripcion = this.txtDescripcion.Text + movimiento.UsuarioLogueado.ApellidoNombre;
            movimiento.Descripcion = this.txtDescripcion.Text + (this.txtDescripcion.Text.Trim().Length > 0 ? " - " : string.Empty) + movimiento.UsuarioLogueado.ApellidoNombre;
            movimiento.EstadoColeccion = EstadoColecciones.Agregado;
            movimiento.MonedaCotizacion = mon.MonedeaCotizacion.MonedaCotizacion;
            movimiento.Archivos = ctrArchivos.ObtenerLista();
            switch (Convert.ToInt32(this.ddlTipoOperacion.SelectedValue))
            {
                case (int)EnumTGETiposOperaciones.RendicionEfectivoForzadoTesoreria: //rendicion de efectivo a tesoreria
                    TESCajas cajaARendir = this.MisCajas[this.ddlCajero.SelectedIndex];
                    cajaARendir.Usuario.FilialPredeterminada = this.UsuarioActivo.FilialPredeterminada;
                    cajaARendir.UsuarioLogueado.IdUsuarioEvento = cajaARendir.Usuario.IdUsuario;
                    movimiento.Caja = TesoreriasF.CajasObtenerDatosCompletos(cajaARendir);
                    movimiento.IdRefTipoOperacion = movimiento.Caja.IdCaja;
                    movimiento.Descripcion = this.txtDescripcion.Text + (this.txtDescripcion.Text.Trim().Length > 0 ? " - " : string.Empty) + movimiento.Caja.Usuario.ApellidoNombre;
                    decimal saldoCaja = TesoreriasF.CajasObtenerImporteEfectivo(cajaARendir.CajasMonedas.Find(x => x.Moneda.IdMoneda == Convert.ToInt32(this.ddlMoneda.SelectedValue)));
                    if (saldoCaja < movimiento.Importe)
                    {
                        this.txtImporte.Text = saldoCaja.ToString("C2");
                        this.MostrarMensaje(this.ObtenerMensajeSistema("TesoreriaMovimientoValidarImporte"), true);
                        return;
                    }
                    break;
                case (int)EnumTGETiposOperaciones.AsignacionEfectivoCaja:
                case (int)EnumTGETiposOperaciones.RefuerzoEfectivoCaja:
                    TESCajas caja = this.MisCajas[this.ddlCajero.SelectedIndex];
                    caja.Usuario.FilialPredeterminada = this.UsuarioActivo.FilialPredeterminada;
                    caja.UsuarioLogueado.IdUsuarioEvento = caja.Usuario.IdUsuario;
                    movimiento.Caja = TesoreriasF.CajasObtenerDatosCompletos(caja);
                    movimiento.IdRefTipoOperacion = movimiento.Caja.IdCaja;
                    movimiento.Descripcion = this.txtDescripcion.Text + (this.txtDescripcion.Text.Trim().Length > 0 ? " - " : string.Empty) + movimiento.Caja.Usuario.ApellidoNombre;
                    if (tesoreriaMoneda.SaldoActual < movimiento.Importe)
                    {
                        this.txtImporte.Text = tesoreriaMoneda.SaldoActual.ToString("C2");
                        this.MostrarMensaje(this.ObtenerMensajeSistema("TesoreriaMovimientoValidarImporte"), true);
                        return;
                    }
                    break;
                case (int)EnumTGETiposOperaciones.TransferenciaBancosDebito:
                    if (tesoreriaMoneda.SaldoActual < movimiento.Importe)
                    {
                        this.txtImporte.Text = tesoreriaMoneda.SaldoActual.ToString("C2");
                        this.MostrarMensaje(this.ObtenerMensajeSistema("TesoreriaMovimientoValidarImporte"), true);
                        return;
                    }
                    movimiento.Fecha = this.ctrFechaCajaContable.dFechaCajaContable.Value;
                    movimiento.TesoreriaMoneda.Moneda.IdMoneda = Convert.ToInt32(this.ddlMoneda.SelectedValue);
                    movimiento.IdRefTipoOperacion = Convert.ToInt32(this.ddlBancosCuentas.SelectedValue);
                    movimiento.Descripcion = this.txtDescripcion.Text;
                    break;
                case (int)EnumTGETiposOperaciones.ExtraccionEfectivoParaFilial:
                    if (tesoreriaMoneda.SaldoActual < movimiento.Importe)
                    {
                        this.txtImporte.Text = tesoreriaMoneda.SaldoActual.ToString("C2");
                        this.MostrarMensaje(this.ObtenerMensajeSistema("TesoreriaMovimientoValidarImporte"), true);
                        return;
                    }
                    movimiento.Fecha = this.ctrFechaCajaContable.dFechaCajaContable.Value;
                    movimiento.TesoreriaMoneda.Moneda.IdMoneda = Convert.ToInt32(this.ddlMoneda.SelectedValue);
                    movimiento.IdRefTipoOperacion = Convert.ToInt32(this.ddlFiliales.SelectedValue);
                    movimiento.Descripcion = this.txtDescripcion.Text;
                    break;
                default:
                    break;
            }

            tesoreriaMoneda.TesoreriasMovimientos.Add(movimiento);
            this.MiTesoreria.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

            if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaBancosDebito
                || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.ExtraccionEfectivoParaFilial)
            {
                movimiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                if (TesoreriasF.TesoreriaMovimientoAgregarTransferenciaBancos(movimiento))
                    this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(movimiento.CodigoMensaje));
                else
                    this.MostrarMensaje(movimiento.CodigoMensaje, true, movimiento.CodigoMensajeArgs);
            }
            else
            {
                if (TesoreriasF.TesoreriasModificar(this.MiTesoreria))
                    this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiTesoreria.CodigoMensaje));
                else
                    this.MostrarMensaje(this.MiTesoreria.CodigoMensaje, true, this.MiTesoreria.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/TesoreriasMovimientos.aspx"), true);
        }

        private void DeshabilitarControles()
        {
            this.txtNumeroCaja.Enabled = false;
            //this.txtSaldoActual.Enabled = false;
            this.txtSaldoCaja.Enabled = false;
        }

        private void CargarCombos()
        {
            MisMonedas = TGEGeneralesF.MonedasCotizacionesObtenerCombo();
            this.ddlMoneda.DataSource = MisMonedas; // TGEGeneralesF.MonedasObtenerListaActiva();
            this.ddlMoneda.DataValueField = "IdMoneda";
            this.ddlMoneda.DataTextField = "Descripcion";
            this.ddlMoneda.DataBind();
            if (this.ddlMoneda.Items.Count == 1)
                this.ddlMoneda_OnSelectedIndexChanged(null, EventArgs.Empty);
            else
                AyudaProgramacion.AgregarItemSeleccione(this.ddlMoneda, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            //ES NECESARIO QUE LAS MONEDAS SE CARGUEN ANTES QUE LAS CAJAS, NO MODIFICAR POR FAVOR            
            TESCajas caja = new TESCajas();
            caja.Tesoreria = this.MiTesoreria;
            this.MisCajas = TesoreriasF.CajasObtenerAbiertas(caja);
            this.ddlCajero.DataSource = this.MisCajas;
            this.ddlCajero.DataValueField = "miUsuarioIdUsuario";
            this.ddlCajero.DataTextField = "miUsuarioApellidoNombre";
            this.ddlCajero.DataBind();
            if (this.ddlCajero.Items.Count == 1)
                this.ddlCajero_OnSelectedIndexChanged(null, EventArgs.Empty);
            else
                AyudaProgramacion.AgregarItemSeleccione(this.ddlCajero, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.MisTiposOperaciones = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(EnumTGETiposFuncionalidades.TesoreriasMovimientos);
            this.ddlTipoOperacion.DataSource = this.MisTiposOperaciones;
            this.ddlTipoOperacion.DataValueField = "IdTipoOperacion";
            this.ddlTipoOperacion.DataTextField = "TipoOperacion";
            this.ddlTipoOperacion.DataBind();
            if (this.ddlTipoOperacion.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoOperacion, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTipoOperacion_OnSelectedIndexChanged(null, EventArgs.Empty);
        }

        protected void ddlTipoOperacion_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.ctrFechaCajaContable.Visible = false;
            if (string.IsNullOrEmpty(this.ddlTipoOperacion.SelectedValue))
                return;

            this.txtSaldoActual.Visible = true;
            this.lblSaldoActual.Visible = true;
            this.pnlCajas.Visible = false;
            this.pnlCuentaInternaDestino.Visible = false;
            this.rfvBancos.Enabled = false;
            this.rfvBancosCuentas.Enabled = false;
            this.pnlFilialesDestino.Visible = false;
            this.rfvFiliales.Enabled = false;
            switch (Convert.ToInt32(this.ddlTipoOperacion.SelectedValue))
            {
                case (int)EnumTGETiposOperaciones.RendicionEfectivoForzadoTesoreria:
                case (int)EnumTGETiposOperaciones.AsignacionEfectivoCaja:
                    this.pnlCajas.Visible = true;
                    this.lblSaldoCaja.Visible = true;
                    this.txtSaldoCaja.Visible = true;
                    //this.txtSaldoActual.Visible = false;
                    //this.lblSaldoActual.Visible = false;
                    break;
                case (int)EnumTGETiposOperaciones.ExtraccionEfectivoParaFilial:
                    this.pnlFilialesDestino.Visible = true;
                    this.rfvFiliales.Enabled = true;
                    this.ctrFechaCajaContable.Visible = true;
                    this.ctrFechaCajaContable.IniciarControl(Gestion.Agregar, DateTime.Now, this.MiTesoreria.FechaAbrir);

                    List<TGEFiliales> filiales = TGEGeneralesF.FilialesObenerLista();
                    filiales = AyudaProgramacion.AcomodarIndices<TGEFiliales>(filiales.Where(x => x.IdFilial != this.MiTesoreria.Filial.IdFilial).ToList());
                    this.ddlFiliales.DataSource = filiales;
                    this.ddlFiliales.DataValueField = "IdFilial";
                    this.ddlFiliales.DataTextField = "Filial";
                    this.ddlFiliales.DataBind();
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlFiliales, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                    break;
                case (int)EnumTGETiposOperaciones.RefuerzoEfectivoCaja:
                    this.pnlCajas.Visible = true;
                    break;
                case (int)EnumTGETiposOperaciones.TransferenciaBancosDebito:
                    this.pnlCuentaInternaDestino.Visible = true;
                    this.rfvBancos.Enabled = true;
                    this.rfvBancosCuentas.Enabled = true;
                    this.ctrFechaCajaContable.Visible = true;
                    this.ctrFechaCajaContable.IniciarControl(Gestion.Agregar, DateTime.Now, this.MiTesoreria.FechaAbrir);

                    List<TESBancos> lista = BancosF.BancosObtenerListaFilialFiltro(this.MiTesoreria.Filial);
                    if (lista.Count > 0)
                    {
                        this.ddlBancos.Items.Clear();
                        this.ddlBancos.ClearSelection();
                        this.ddlBancos.SelectedValue = null;
                        this.ddlBancos.SelectedIndex = -1;
                        this.ddlBancos.DataSource = lista;
                        this.ddlBancos.DataValueField = "IdBanco";
                        this.ddlBancos.DataTextField = "Descripcion";
                        this.ddlBancos.DataBind();
                    }
                    if (ddlBancos.Items.Count != 1)
                        AyudaProgramacion.AgregarItemSeleccione(this.ddlBancos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                    this.ddlBancos_OnSelectedIndexChanged(null, EventArgs.Empty);

                    break;
                default:
                    break;
            }
        }

        protected void ddlBancos_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlBancosCuentas.Items.Clear();
            this.ddlBancosCuentas.ClearSelection();
            this.ddlBancosCuentas.SelectedValue = null;
            this.ddlBancosCuentas.SelectedIndex = -1;

            if (!string.IsNullOrEmpty(this.ddlBancos.SelectedValue)
                && !string.IsNullOrEmpty(this.ddlMoneda.SelectedValue))
            {
                TESBancosCuentas bancoCuenta = new TESBancosCuentas();
                bancoCuenta.Banco.IdBanco = Convert.ToInt32(this.ddlBancos.SelectedValue);
                bancoCuenta.Filial.IdFilial = Convert.ToInt32(this.MiTesoreria.Filial.IdFilial);
                bancoCuenta.Estado.IdEstado = (int)Estados.Activo;
                bancoCuenta.Moneda.IdMoneda = Convert.ToInt32(this.ddlMoneda.SelectedValue);
                List<TESBancosCuentas> lista = BancosF.BancosCuentasObtenerListaFiltro(bancoCuenta);

                if (lista.Count > 0)
                {
                    this.ddlBancosCuentas.DataSource = lista;
                    this.ddlBancosCuentas.DataValueField = "IdBancoCuenta";
                    this.ddlBancosCuentas.DataTextField = "DescripcionFilialBancoTipoCuentaNumero";
                    this.ddlBancosCuentas.DataBind();
                }
            }

            if (ddlBancosCuentas.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlBancosCuentas, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void ddlCajero_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            TESCajas cajaTesoreria = new TESCajas();
            cajaTesoreria.Tesoreria = this.MiTesoreria;
            List<TESCajas> cajas = TesoreriasF.CajasObtenerAbiertas(cajaTesoreria);
            var idUsuario = Convert.ToInt32(this.ddlCajero.SelectedValue);
            TESCajas caja = cajas.Find(delegate (TESCajas c) { return c.Usuario.IdUsuario == idUsuario; });
            this.txtNumeroCaja.Text = caja.NumeroCaja.ToString();
            ddlMoneda_OnSelectedIndexChanged(null, EventArgs.Empty);
        }

        protected void ddlMoneda_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlMoneda.SelectedValue))
            {
                List<TESTesoreriasMonedas> monedas = this.MiTesoreria.TesoreriasMonedas;
                var idMoneda = Convert.ToInt32(this.ddlMoneda.SelectedValue);
                TESTesoreriasMonedas moneda = monedas.Find(delegate (TESTesoreriasMonedas m) { return m.Moneda.IdMoneda == idMoneda; });
                //this.txtSaldoActual.Text = moneda.SaldoFinal.ToString();
                SetInitializeCulture(moneda.Moneda.Moneda);
                this.txtSaldoActual.Text = moneda.SaldoActual.ToString("C2");
            }
            else
                this.txtSaldoActual.Text = (0).ToString("C2");


            this.ddlBancos_OnSelectedIndexChanged(null, EventArgs.Empty);
        }

        private void ConsultarMovimiento(TESTesoreriasMovimientos pParametro)
        {
            this.txtDescripcion.Enabled = false;
            this.txtImporte.Enabled = false;
            this.txtNumeroCaja.Enabled = false;
            this.txtSaldoCaja.Enabled = false;
            this.txtSaldoActual.Enabled = false;
            this.ddlBancos.Enabled = false;
            this.ddlCajero.Enabled = false;
            this.ddlTipoOperacion.Enabled = false;
            this.ddlMoneda.Enabled = false;
            this.ddlFiliales.Enabled = false;
            this.ddlBancosCuentas.Enabled = false;

            this.txtDescripcion.Text = pParametro.Descripcion;
            this.txtImporte.Text = pParametro.Importe.ToString();




            ListItem item3 = ddlTipoOperacion.Items.FindByValue(pParametro.TipoOperacion.IdTipoOperacion.ToString());
            if (item3 == null)
            {
                ddlTipoOperacion.Items.Add(new ListItem(pParametro.TipoOperacion.TipoOperacion, pParametro.TipoOperacion.IdTipoOperacion.ToString()));
            }
            this.ddlTipoOperacion.SelectedValue = pParametro.TipoOperacion.IdTipoOperacion.ToString();
            ddlTipoOperacion_OnSelectedIndexChanged(null, EventArgs.Empty);

            ListItem item2 = ddlMoneda.Items.FindByValue(pParametro.TesoreriaMoneda.Moneda.IdMoneda.ToString());
            if (item2 == null)
            {
                ddlMoneda.Items.Add(new ListItem(pParametro.TesoreriaMoneda.Moneda.Moneda, pParametro.TesoreriaMoneda.Moneda.IdMoneda.ToString()));
            }
            this.ddlMoneda.SelectedValue = pParametro.TesoreriaMoneda.Moneda.IdMoneda.ToString();
            this.ddlMoneda_OnSelectedIndexChanged(null, EventArgs.Empty);

            if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaBancosDebito)
            {

                ListItem item7 = ddlBancos.Items.FindByValue(pParametro.IdBanco.ToString());
                if (item7 == null)
                {
                    ddlBancos.Items.Add(new ListItem(pParametro.Banco, pParametro.IdBanco.ToString()));
                }
                this.ddlBancos.SelectedValue = pParametro.IdBanco.ToString();



                ListItem item5 = ddlBancosCuentas.Items.FindByValue(pParametro.IdRefTipoOperacion.ToString());
                if (item5 == null)
                {
                    ddlBancosCuentas.Items.Add(new ListItem(pParametro.RefTipoOperacion, pParametro.IdRefTipoOperacion.ToString()));
                }
                this.ddlBancosCuentas.SelectedValue = pParametro.IdRefTipoOperacion.ToString();






            }
            else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.ExtraccionEfectivoParaFilial)
            {
                ListItem item = ddlFiliales.Items.FindByValue(pParametro.IdRefTipoOperacion.ToString());
                if (item == null)
                {
                    ddlFiliales.Items.Add(new ListItem(pParametro.RefTipoOperacion, pParametro.IdRefTipoOperacion.ToString()));
                }
                this.ddlFiliales.SelectedValue = pParametro.IdRefTipoOperacion.ToString();
            }





            //this.ctrFechaCajaContable.IniciarControl(Gestion.Consultar, this.MiTesoreria.FechaAbrir);
            //movimiento.IdRefTipoOperacion = Convert.ToInt32(this.ddlFiliales.SelectedValue);

        }
    }
}
