using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tesorerias.Entidades;
using Tesorerias;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Comunes.Entidades;
using System.Globalization;

namespace IU.Modulos.Tesoreria
{
    public partial class CajasTraspasoEfectivo : PaginaCajas
    {

        private List<TESCajas> MisCajas
        {
            get { return this.PropiedadObtenerValor<List<TESCajas>>("CajasTraspasoEfectivoMisCajas"); }
            set { this.PropiedadGuardarValor("CajasTraspasoEfectivoMisCajas", value); }
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
                //this.DeshabilitarControles();
            }
        }

        private void CargarCombos()
        {
            TESCajas caja = new TESCajas();
            caja.Tesoreria = this.MiCaja.Tesoreria;
            this.MisCajas = TesoreriasF.CajasObtenerAbiertas(caja);
            this.MisCajas.RemoveAt(this.MisCajas.FindIndex(x => x.Usuario.IdUsuario == this.MiCaja.Usuario.IdUsuario));
            AyudaProgramacion.AcomodarIndices<TESCajas>(this.MisCajas);
            this.ddlCajero.DataSource = this.MisCajas;
            this.ddlCajero.DataValueField = "miUsuarioIdUsuario";
            this.ddlCajero.DataTextField = "miUsuarioApellidoNombre";
            this.ddlCajero.DataBind();
            if (this.ddlCajero.Items.Count == 1)
                this.ddlCajero_OnSelectedIndexChanged(null, EventArgs.Empty);
            else
                AyudaProgramacion.AgregarItemSeleccione(this.ddlCajero, this.ObtenerMensajeSistema("SeleccioneOpcion"));


            this.ddlMoneda.DataSource = TGEGeneralesF.MonedasObtenerListaActiva();
            this.ddlMoneda.DataValueField = "IdMoneda";
            this.ddlMoneda.DataTextField = "Descripcion";
            this.ddlMoneda.DataBind();
            if (this.ddlMoneda.Items.Count == 1)
                this.ddlMoneda_OnSelectedIndexChanged(null, EventArgs.Empty);
            else
                AyudaProgramacion.AgregarItemSeleccione(this.ddlMoneda, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            //BLOQUEO EL tEXTBOX DEL SALDO DE CAJA
            this.txtSaldoActual.Enabled = false;
        }

        protected void ddlCajero_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            TESCajas cajaTesoreria = new TESCajas();
            cajaTesoreria.Tesoreria = this.MiCaja.Tesoreria;
            List<TESCajas> cajas = TesoreriasF.CajasObtenerAbiertas(cajaTesoreria);
            var idUsuario = Convert.ToInt32(this.ddlCajero.SelectedValue);
            TESCajas caja = cajas.Find(delegate(TESCajas c) { return c.Usuario.IdUsuario == idUsuario; });
            this.txtNumeroCaja.Text = caja.NumeroCaja.ToString();
        }

        protected void ddlMoneda_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            List<TESCajasMonedas> monedas = this.MiCaja.CajasMonedas;
            var idMoneda = Convert.ToInt32(this.ddlMoneda.SelectedValue);
            TESCajasMonedas moneda = monedas.Find(delegate(TESCajasMonedas m) { return m.Moneda.IdMoneda == idMoneda; });
                SetInitializeCulture(moneda.Moneda.Moneda);
            
            //this.txtSaldoActual.Text = moneda.SaldoFinal.ToString();
            this.txtSaldoActual.Text = TesoreriasF.CajasObtenerImporteEfectivo(moneda).ToString("C2");
        }

        void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosAutomaticos.aspx"), true);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Validate("Aceptar");
            if (!this.IsValid)
                return;

            if (decimal.Parse(this.txtImporte.Text, NumberStyles.Currency) <= 0)
            {
                this.MostrarMensaje(this.ObtenerMensajeSistema("ValidarImporteMayorCero"), true);
                return;
            }

            List<TESCajasMonedas> monedas = this.MiCaja.CajasMonedas;
            var idMoneda = Convert.ToInt32(this.ddlMoneda.SelectedValue);
            TESCajasMonedas moneda = monedas.Find(delegate (TESCajasMonedas m) { return m.Moneda.IdMoneda == idMoneda; });
            decimal saldoFinal = TesoreriasF.CajasObtenerImporteEfectivo(moneda);
            decimal Importe = txtImporte.Decimal;
            if ((saldoFinal - Importe ) < 0)
            {
                this.MostrarMensaje(this.ObtenerMensajeSistema("TesoreriaMovimientoValidarImporte"), true);
                return;
            }
            #region GENERAR MOVIMIENTOS DE CAJA ORIGEN Y DESTINO
            //CREO LOS 2 MOVIMIENTOS
            TESCajasMovimientos movimientoOrigen = new TESCajasMovimientos();
            
            //MOVIMIENTO CAJA ORIGEN
            var cajaMonedaOrigen = this.MiCaja.CajasMonedas.Find(x => x.Moneda.IdMoneda == Convert.ToInt32(this.ddlMoneda.SelectedValue));
            movimientoOrigen.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.TraspasoEfectivoDebito);
            movimientoOrigen.Fecha = DateTime.Now;
            movimientoOrigen.Importe = decimal.Parse(this.txtImporte.Text, NumberStyles.Currency); //Convert.ToDecimal(this.txtImporte.Text);
            movimientoOrigen.Descripcion = this.txtDescripcion.Text;
            movimientoOrigen.Estado.IdEstado = (int)Estados.Activo;
            movimientoOrigen.EstadoColeccion = EstadoColecciones.Agregado;
            //MOVIMIENTO VALOR (ORIGEN)
            TESCajasMovimientosValores movValorO = new TESCajasMovimientosValores();
            movValorO.Estado.IdEstado = (int)EstadosCajasMovimientos.Activo;
            movValorO.EstadoColeccion = EstadoColecciones.Agregado;
            movValorO.Importe = movimientoOrigen.Importe;
            movValorO.TipoValor.IdTipoValor = (int)EnumTiposValores.Efectivo;
            movimientoOrigen.CajasMovimientosValores.Add(movValorO);
            

            //CAJA DESTINO
            TESCajasMovimientos movimientoDestino = new TESCajasMovimientos();
            movimientoDestino.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.TraspasoEfectivoCredito);  
            movimientoDestino.Fecha = DateTime.Now;
            movimientoDestino.Importe = decimal.Parse(this.txtImporte.Text, NumberStyles.Currency);
            movimientoDestino.Descripcion = this.txtDescripcion.Text;
            movimientoDestino.Estado.IdEstado = (int)Estados.Activo;
            movimientoDestino.EstadoColeccion = EstadoColecciones.Agregado;

            TESCajas caja = this.MisCajas[this.ddlCajero.SelectedIndex];
            caja.Usuario.FilialPredeterminada = this.UsuarioActivo.FilialPredeterminada;
            caja.UsuarioLogueado.IdUsuarioEvento = caja.Usuario.IdUsuario;
            caja = TesoreriasF.CajasObtenerDatosCompletos(caja);
            var cajaMonedaDestino = caja.CajasMonedas.Find(x => x.Moneda.IdMoneda == Convert.ToInt32(this.ddlMoneda.SelectedValue));
            //MOVIMIENTO VALOR (DESTINO)
            TESCajasMovimientosValores movValorDest = new TESCajasMovimientosValores();
            movValorDest.Estado.IdEstado = (int)EstadosCajasMovimientos.Activo;
            movValorDest.EstadoColeccion = EstadoColecciones.Agregado;
            movValorDest.Importe = movimientoOrigen.Importe;
            movValorDest.TipoValor.IdTipoValor = (int)EnumTiposValores.Efectivo;
            movimientoDestino.CajasMovimientosValores.Add(movValorDest);

            //referencias
            movimientoOrigen.IdRefTipoOperacion = caja.IdCaja;
            movimientoDestino.IdRefTipoOperacion = this.MiCaja.IdCaja;

                    //if (cajaMonedaOrigen.SaldoFinal < movimientoOrigen.Importe)
                    //{
                    //    this.txtImporte.Text = cajaMonedaOrigen.SaldoFinal.ToString();
                    //    this.MostrarMensaje(this.ObtenerMensajeSistema("TesoreriaMovimientoValidarImporte"), true);
                    //    return;
                    //}

            //caja ORIGEN a actualizar
            cajaMonedaOrigen.CajasMovimientos.Add(movimientoOrigen);
            cajaMonedaOrigen.EstadoColeccion = EstadoColecciones.Modificado;

            cajaMonedaDestino.CajasMovimientos.Add(movimientoDestino);
            cajaMonedaDestino.EstadoColeccion = EstadoColecciones.Modificado;

            //vuelvo a seter los usuarios logueados porque al levantar la caja pisa el IdUsuarioEvento
            caja.UsuarioLogueado.IdUsuarioEvento = caja.Usuario.IdUsuario;
            this.MiCaja.UsuarioLogueado.IdUsuarioEvento = this.MiCaja.Usuario.IdUsuario;
            #endregion

            if (TesoreriasF.CajasTraspasoEfectivoActualizarCajas(this.MiCaja, caja))
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiCaja.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiCaja.CodigoMensaje, true, this.MiCaja.CodigoMensajeArgs);
            }
        }
       
                protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosAutomaticos.aspx"), true);
        }

    }
}