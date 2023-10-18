using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tesorerias.Entidades;
using System.Collections.Generic;
using Tesorerias;
using Comunes.Entidades;
using Generales.Entidades;
using System.Reflection;
using Generales.FachadaNegocio;
using CuentasPagar.Entidades;
using Bancos.Entidades;
using Cobros.Entidades;
using Reportes.FachadaNegocio;

namespace IU.Modulos.Tesoreria
{
    public partial class CajasMovimientosConfirmar : PaginaCajas
    {
        protected Objeto MiRefTipoOperacion
        {
            get
            {
                if (Session[this.MiSessionPagina + "PaginaCajasMiRefTipoOperacion"] != null)
                    return (Objeto)Session[this.MiSessionPagina + "PaginaCajasMiRefTipoOperacion"];
                else
                {
                    return (Objeto)(Session[this.MiSessionPagina + "PaginaCajasMiRefTipoOperacion"] = new Objeto());
                }
            }
            set { Session[this.MiSessionPagina + "PaginaCajasMiRefTipoOperacion"] = value; }
        }

        protected override void PageLoadEventCajas(object sender, EventArgs e)
        {
            base.PageLoadEventCajas(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
               

                //if (!MisParametrosUrl.Contains("IdRefTipoOperacion"))
                //{
                //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosAutomaticos.aspx"), true);
                //}

                //int IdRefTipoOperacion = Convert.ToInt32(MisParametrosUrl["IdRefTipoOperacion"]);
                //int TipoOperacionIdTipoOperacion = Convert.ToInt32(MisParametrosUrl["TipoOperacionIdTipoOperacion"]);
                //MiCajaMovimientoPendiente.IdRefTipoOperacion = IdRefTipoOperacion;
                //MiCajaMovimientoPendiente.TipoOperacion.IdTipoOperacion = TipoOperacionIdTipoOperacion;
                if (this.MiCajaMovimientoPendiente.IdRefTipoOperacion == 0)
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosAutomaticos.aspx"), true);

                this.MiRefTipoOperacion = TesoreriasF.CajasObtenerMovimientoPendienteConfirmacion(this.MiCajaMovimientoPendiente);

                Type t2 = this.MiRefTipoOperacion.GetType();
                PropertyInfo prop = t2.GetProperty("TipoOperacion");
                TGETiposOperaciones operacion = (TGETiposOperaciones)prop.GetValue(this.MiRefTipoOperacion, null);
                    
                SetInitializeCulture(MiCajaMovimientoPendiente.CajaMoneda.Moneda.Moneda);

                if (!Enumerable.SequenceEqual(this.MiCajaMovimientoPendiente.SelloTiempo, this.MiRefTipoOperacion.SelloTiempo))
                {
                    this.btnAceptar.Visible = false;
                    this.MostrarMensaje("Concurrencia", true);
                }

                string funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarAccion"));
                this.btnAceptar.Attributes.Add("OnClick", funcion);
                
                this.txtAfiliado.Text = this.MiCajaMovimientoPendiente.Afiliado.ApellidoNombre;
                this.txtImporte.Text = this.MiCajaMovimientoPendiente.Importe.ToString("C2");
                this.txtNumeroDocumento.Text = this.MiCajaMovimientoPendiente.Afiliado.NumeroDocumento.ToString();
                this.txtNumeroReferencia.Text = this.MiCajaMovimientoPendiente.IdRefTipoOperacion.ToString();
                this.txtTipoOperacion.Text = this.MiCajaMovimientoPendiente.TipoOperacion.TipoOperacion;
                //this.txtEfectivo.Text = string.Empty;
                //this.txtVuelto.Text = string.Empty;

                

                bool habilitarValores = true;
                TESCajasMovimientosValores valor;
                TESCheques cheque;
                switch (operacion.IdTipoOperacion)
                {
                    case (int)EnumTGETiposOperaciones.OrdenesPagos:
                    case (int)EnumTGETiposOperaciones.OrdenesPagosInterno:
                        habilitarValores = false;
                        CapOrdenesPagos op = (CapOrdenesPagos)this.MiRefTipoOperacion;                        
                        this.MiCajaMovimientoPendiente.CajasMovimientosValores = new List<TESCajasMovimientosValores>();

                        foreach (CapOrdenesPagosValores item in op.OrdenesPagosValore)
                        {
                            //EFECTIVO
                            if (item.TipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo)
                            {
                                if (this.MiCajaMovimientoPendiente.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo))
                                    this.MiCajaMovimientoPendiente.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo).Importe = item.Importe;
                                else
                                {
                                    valor = new TESCajasMovimientosValores();
                                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                                    valor.TipoValor = item.TipoValor;
                                    valor.Importe = item.Importe;
                                    this.MiCajaMovimientoPendiente.CajasMovimientosValores.Add(valor);
                                    valor.IndiceColeccion = this.MiCajaMovimientoPendiente.CajasMovimientosValores.IndexOf(valor);
                                }
                            }//CHEQUE
                            else if (item.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque)
                            {
                                if (!this.MiCajaMovimientoPendiente.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque))
                                {
                                    valor = new TESCajasMovimientosValores();
                                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                                    valor.TipoValor = item.TipoValor; 
                                    this.MiCajaMovimientoPendiente.CajasMovimientosValores.Add(valor);
                                    valor.IndiceColeccion = this.MiCajaMovimientoPendiente.CajasMovimientosValores.IndexOf(valor);
                                }

                                valor = this.MiCajaMovimientoPendiente.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque);
                                cheque = new TESCheques();
                                cheque.EstadoColeccion = EstadoColecciones.Agregado;
                                cheque.Estado.IdEstado = (int)EstadosCheques.Entregado;
                                cheque.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                                cheque.Fecha = item.Fecha.Value;
                                cheque.FechaDiferido = item.FechaDiferido;
                                cheque.NumeroCheque = item.NumeroCheque;
                                cheque.Banco.IdBanco = item.BancoCuenta.Banco.IdBanco;
                                cheque.Banco.Descripcion = item.BancoCuenta.Banco.Descripcion;
                                cheque.ChequePropio = item.ChequePropio;
                                //cheque.TitularCheque = this.txtTitular.Text;
                                //cheque.CUIT = this.txtCUIT.Text;
                                cheque.Importe = item.Importe;
                                cheque.IdBancoCuenta = item.BancoCuenta.IdBancoCuenta;
                                cheque.TipoOperacion = op.TipoOperacion;
                                //Para buscar el cheque despues
                                cheque.HashTransaccion = item.IdOrdenPagoValor;
                                //item.HashTransaccion = item.GetHashCode();
                                //cheque.HashTransaccion = item.HashTransaccion;
                                valor.Cheques.Add(cheque);
                                cheque.IndiceColeccion = valor.Cheques.IndexOf(cheque);
                                valor.Importe = valor.Cheques.Sum(x => x.Importe);
                            }
                            else if (item.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
                            {
                                if (!this.MiCajaMovimientoPendiente.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero))
                                {
                                    valor = new TESCajasMovimientosValores();
                                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                                    valor.TipoValor = item.TipoValor;
                                    this.MiCajaMovimientoPendiente.CajasMovimientosValores.Add(valor);
                                    valor.IndiceColeccion = this.MiCajaMovimientoPendiente.CajasMovimientosValores.IndexOf(valor);
                                }
                                valor = this.MiCajaMovimientoPendiente.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero);
                                item.Cheque.EstadoColeccion = EstadoColecciones.Modificado;
                                item.Cheque.Estado.IdEstado = (int)EstadosCheques.Entregado;
                                item.Cheque.TipoOperacion = operacion;
                                valor.Cheques.Add(item.Cheque);
                                item.Cheque.IndiceColeccion = valor.Cheques.IndexOf(item.Cheque);
                                valor.Importe = valor.Cheques.Sum(x => x.Importe);
                            }
                            else if (item.TipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia)
                            {
                                if (!this.MiCajaMovimientoPendiente.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia))
                                {
                                    valor = new TESCajasMovimientosValores();
                                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                                    valor.TipoValor = item.TipoValor;
                                    this.MiCajaMovimientoPendiente.CajasMovimientosValores.Add(valor);
                                    valor.IndiceColeccion = this.MiCajaMovimientoPendiente.CajasMovimientosValores.IndexOf(valor);
                                }

                                valor = this.MiCajaMovimientoPendiente.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia);
                                TESBancosCuentasMovimientos bancoMovimiento = new TESBancosCuentasMovimientos();
                                bancoMovimiento.BancoCuenta = item.BancoCuenta;
                                bancoMovimiento.BancoCuenta.IdBancoCuenta = item.BancoCuenta.IdBancoCuenta;
                                bancoMovimiento.Importe = item.Importe;
                                bancoMovimiento.TipoOperacion = op.TipoOperacion;
                                bancoMovimiento.FechaAlta = DateTime.Now;
                                bancoMovimiento.FechaMovimiento = DateTime.Now;
                                bancoMovimiento.FechaConfirmacionBanco = item.Fecha.Value;
                                bancoMovimiento.FechaConciliacion = DateTime.Now;
                                bancoMovimiento.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.Confirmado;
                                bancoMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
                                
                                valor.BancosCuentasMovimientos.Add(bancoMovimiento);
                                valor.Importe = valor.BancosCuentasMovimientos.Sum(x => x.Importe);
                            }
                            else if (item.TipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito)
                            {
                                if (!this.MiCajaMovimientoPendiente.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito))
                                {
                                    valor = new TESCajasMovimientosValores();
                                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                                    valor.TipoValor = item.TipoValor;
                                    this.MiCajaMovimientoPendiente.CajasMovimientosValores.Add(valor);
                                    valor.IndiceColeccion = this.MiCajaMovimientoPendiente.CajasMovimientosValores.IndexOf(valor);
                                }

                                valor = this.MiCajaMovimientoPendiente.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito);

                                TESBancosCuentasMovimientos bancoMovimiento = new TESBancosCuentasMovimientos();
                                bancoMovimiento.BancoCuenta = item.BancoCuenta;
                                bancoMovimiento.BancoCuenta.IdBancoCuenta = item.BancoCuenta.IdBancoCuenta;
                                bancoMovimiento.Importe = item.Importe;
                                bancoMovimiento.TipoOperacion = op.TipoOperacion;
                                bancoMovimiento.FechaAlta = DateTime.Now;
                                bancoMovimiento.FechaMovimiento = DateTime.Now;
                                bancoMovimiento.FechaConfirmacionBanco = item.Fecha.Value;
                                bancoMovimiento.FechaConciliacion = DateTime.Now;
                                bancoMovimiento.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.Confirmado;
                                bancoMovimiento.EstadoColeccion = EstadoColecciones.Agregado;

                                valor.BancosCuentasMovimientos.Add(bancoMovimiento);
                                valor.Importe = valor.BancosCuentasMovimientos.Sum(x => x.Importe);
                            }
                            else if (item.TipoValor.IdTipoValor == (int)EnumTiposValores.AfipTiposPercepciones)
                            {
                                if (this.MiCajaMovimientoPendiente.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.AfipTiposPercepciones))
                                    this.MiCajaMovimientoPendiente.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.AfipTiposPercepciones).Importe += item.Importe;
                                else
                                {
                                    valor = new TESCajasMovimientosValores();
                                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                                    valor.TipoValor = item.TipoValor;
                                    valor.Importe = item.Importe;
                                    this.MiCajaMovimientoPendiente.CajasMovimientosValores.Add(valor);
                                    valor.IndiceColeccion = this.MiCajaMovimientoPendiente.CajasMovimientosValores.IndexOf(valor);
                                }
                            }
                            else if (item.TipoValor.IdTipoValor == (int)EnumTiposValores.AfipTiposRetenciones)
                            {
                                if (this.MiCajaMovimientoPendiente.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.AfipTiposRetenciones))
                                    this.MiCajaMovimientoPendiente.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.AfipTiposRetenciones).Importe += item.Importe;
                                else
                                {
                                    valor = new TESCajasMovimientosValores();
                                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                                    valor.TipoValor = item.TipoValor;
                                    valor.Importe = item.Importe;
                                    this.MiCajaMovimientoPendiente.CajasMovimientosValores.Add(valor);
                                    valor.IndiceColeccion = this.MiCajaMovimientoPendiente.CajasMovimientosValores.IndexOf(valor);
                                }
                            }
                            //else if (item.TipoValor.IdTipoValor == (int)EnumTiposValores.CajaAhorro)
                            //{
                            //    if (!this.MiCajaMovimientoPendiente.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.CajaAhorro))
                            //    {
                            //        valor = new TESCajasMovimientosValores();
                            //        valor.EstadoColeccion = EstadoColecciones.Agregado;
                            //        valor.TipoValor = item.TipoValor;
                            //        this.MiCajaMovimientoPendiente.CajasMovimientosValores.Add(valor);
                            //        valor.IndiceColeccion = this.MiCajaMovimientoPendiente.CajasMovimientosValores.IndexOf(valor);
                            //    }

                            //    valor = this.MiCajaMovimientoPendiente.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.CajaAhorro);
                            //    valor.Importe = item.Importe;
                            //}
                        }
                        break;
                    case (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas:
                    case (int)EnumTGETiposOperaciones.OrdenesCobrosFacturasInternas:
                    case (int)EnumTGETiposOperaciones.OrdenesCobrosAfiliados:
                    case (int)EnumTGETiposOperaciones.CancelacionAnticipadaCuotaPrestamo:
                        habilitarValores = false;
                        CobOrdenesCobros oc = (CobOrdenesCobros)this.MiRefTipoOperacion;
                        if (oc.OrdenesCobrosValores.Count == 0)
                            habilitarValores = true;

                        this.MiCajaMovimientoPendiente.CajasMovimientosValores = new List<TESCajasMovimientosValores>();

                        foreach (CobOrdenesCobrosValores item in oc.OrdenesCobrosValores)
                        {
                            //EFECTIVO
                            if (item.TipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo)
                            {
                                if (this.MiCajaMovimientoPendiente.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo))
                                    this.MiCajaMovimientoPendiente.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo).Importe = item.Importe;
                                else
                                {
                                    valor = new TESCajasMovimientosValores();
                                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                                    valor.TipoValor = item.TipoValor;
                                    valor.Importe = item.Importe;
                                    this.MiCajaMovimientoPendiente.CajasMovimientosValores.Add(valor);
                                    valor.IndiceColeccion = this.MiCajaMovimientoPendiente.CajasMovimientosValores.IndexOf(valor);
                                }
                            }//CHEQUE
                            else if (item.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
                            {
                                if (!this.MiCajaMovimientoPendiente.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero))
                                {
                                    valor = new TESCajasMovimientosValores();
                                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                                    valor.TipoValor = item.TipoValor;
                                    this.MiCajaMovimientoPendiente.CajasMovimientosValores.Add(valor);
                                    valor.IndiceColeccion = this.MiCajaMovimientoPendiente.CajasMovimientosValores.IndexOf(valor);
                                }
                                valor = this.MiCajaMovimientoPendiente.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero);

                                item.Cheque = new TESCheques();
                                item.Cheque.EstadoColeccion = EstadoColecciones.Agregado;
                                item.Cheque.Estado.IdEstado = (int)EstadosCheques.EnCaja;
                                item.Cheque.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                                item.Cheque.Fecha = item.Fecha.Value;
                                item.Cheque.FechaDiferido = item.FechaDiferido.Value;
                                //cheque.Concepto = item.conce
                                item.Cheque.NumeroCheque = item.NumeroCheque;
                                item.Cheque.Banco.IdBanco = item.BancoCuenta.Banco.IdBanco;
                                item.Cheque.Banco.Descripcion = item.BancoCuenta.Banco.Descripcion;
                                item.Cheque.TitularCheque = item.Titular;
                                item.Cheque.CUIT = string.Empty;
                                item.Cheque.Importe = item.Importe;
                                //cheque.IdBancoCuenta = this.ddlBancosCuentasCheques.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlBancosCuentasCheques.SelectedValue);
                                item.Cheque.ChequePropio = false;
                                item.Cheque.TipoOperacion = operacion;
                                valor.Cheques.Add(item.Cheque);
                                item.Cheque.IndiceColeccion = valor.Cheques.IndexOf(item.Cheque);

                                valor.Importe = valor.Cheques.Sum(x => x.Importe);
                                
                                //item.Cheque.EstadoColeccion = EstadoColecciones.Modificado;
                                //item.Cheque.Estado.IdEstado = (int)EstadosCheques.Entregado;
                                //item.Cheque.TipoOperacion = operacion;
                                //valor.Cheques.Add(item.Cheque);
                                //item.Cheque.IndiceColeccion = valor.Cheques.IndexOf(item.Cheque);
                                //valor.Importe = valor.Cheques.Sum(x => x.Importe);
                            }
                            else if (item.TipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia)
                            {
                                if (!this.MiCajaMovimientoPendiente.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia))
                                {
                                    valor = new TESCajasMovimientosValores();
                                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                                    valor.TipoValor = item.TipoValor;
                                    this.MiCajaMovimientoPendiente.CajasMovimientosValores.Add(valor);
                                    valor.IndiceColeccion = this.MiCajaMovimientoPendiente.CajasMovimientosValores.IndexOf(valor);
                                }

                                valor = this.MiCajaMovimientoPendiente.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia);

                                TESBancosCuentasMovimientos bancoMovimiento = new TESBancosCuentasMovimientos();
                                bancoMovimiento.BancoCuenta.IdBancoCuenta = item.BancoCuenta.IdBancoCuenta;
                                bancoMovimiento.Importe = item.Importe;
                                bancoMovimiento.TipoOperacion = oc.TipoOperacion;
                                bancoMovimiento.FechaAlta = DateTime.Now;
                                bancoMovimiento.FechaMovimiento = item.Fecha.Value;
                                bancoMovimiento.FechaConfirmacionBanco = item.Fecha.Value;
                                bancoMovimiento.FechaConciliacion = DateTime.Now;
                                bancoMovimiento.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.Confirmado;
                                bancoMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
                                bancoMovimiento.BancoCuenta = item.BancoCuenta;
                                valor.BancosCuentasMovimientos.Add(bancoMovimiento);
                                valor.Importe = valor.BancosCuentasMovimientos.Sum(x => x.Importe);
                            }
                            else if (item.TipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito)
                            {
                                if (!this.MiCajaMovimientoPendiente.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito))
                                {
                                    valor = new TESCajasMovimientosValores();
                                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                                    valor.TipoValor = item.TipoValor;
                                    this.MiCajaMovimientoPendiente.CajasMovimientosValores.Add(valor);
                                    valor.IndiceColeccion = this.MiCajaMovimientoPendiente.CajasMovimientosValores.IndexOf(valor);
                                }

                                valor = this.MiCajaMovimientoPendiente.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito);

                                TESTarjetasTransacciones tarjeta = new TESTarjetasTransacciones();
                                tarjeta.EstadoColeccion = EstadoColecciones.Agregado;
                                tarjeta.Estado.IdEstado = (int)Estados.Activo;
                                tarjeta.Importe = item.Importe;
                                tarjeta.Fecha = DateTime.Now;
                                tarjeta.FechaTransaccion = item.Fecha.HasValue ? item.Fecha.Value : DateTime.Now;
                                tarjeta.IdTarjetaCredito = item.IdTarjetaCredito;
                                tarjeta.Titular = item.Titular;
                                tarjeta.NumeroTarjetaCredito = item.NumeroTarjetaCredito;
                                tarjeta.VencimientoAnio = item.VencimientoAnio;
                                tarjeta.VencimientoMes = item.VencimientoMes;
                                tarjeta.NumeroTransaccionPosnet = item.NumeroTransaccionPosnet;
                                tarjeta.CantidadCuotas = item.CantidadCuotas;
                                //tarjeta.Observaciones = item.des
                                tarjeta.NumeroLote = item.NumeroLote;
                                //tarjeta.TarjetaDescripcion = this.ddlTarjetas.SelectedItem.Text;
                                valor.TarjetasTransacciones.Add(tarjeta);
                                tarjeta.IndiceColeccion = valor.TarjetasTransacciones.IndexOf(tarjeta);
                                valor.Importe = valor.TarjetasTransacciones.Sum(x => x.Importe);
                            }
                            else if (item.TipoValor.IdTipoValor == (int)EnumTiposValores.CajaAhorro)
                            {
                                if (!this.MiCajaMovimientoPendiente.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.CajaAhorro))
                                {
                                    valor = new TESCajasMovimientosValores();
                                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                                    valor.TipoValor = item.TipoValor;
                                    this.MiCajaMovimientoPendiente.CajasMovimientosValores.Add(valor);
                                    valor.IndiceColeccion = this.MiCajaMovimientoPendiente.CajasMovimientosValores.IndexOf(valor);
                                }

                                valor = this.MiCajaMovimientoPendiente.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.CajaAhorro);
                                valor.Importe = item.Importe;
                                valor.IdCuenta =item.IdCuenta.Value;
                                valor.Descripcion = item.Detalle;
                            }
                        }

                        break;
                    default:
                        break;
                }

                if (this.MiRefTipoOperacion is TESCajasMovimientos)
                {
                    habilitarValores = false;
                    this.MiCajaMovimientoPendiente = (TESCajasMovimientos)this.MiRefTipoOperacion;
                }

                this.ctrIngresosValores.Visible = true;
                this.ctrIngresosValores.IniciarControl(this.MiCajaMovimientoPendiente, Gestion.Agregar, operacion, this.UsuarioActivo.SectorPredeterminado, habilitarValores);
                this.txtFechaComprobante.Text = this.MiCajaMovimientoPendiente.Fecha.ToShortDateString();
                this.ctrFechaCajaContable.IniciarControl(Gestion.Agregar, this.MiCajaMovimientoPendiente.Fecha, this.MiCaja.FechaAbrir);
            }
        }

        void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosAutomaticos.aspx"), true);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            Objeto refTipoOp = TesoreriasF.CajasObtenerMovimientoPendienteConfirmacion(this.MiCajaMovimientoPendiente);
            if (!Enumerable.SequenceEqual(refTipoOp.SelloTiempo, this.MiRefTipoOperacion.SelloTiempo))
            {
                //this.btnAceptar.Visible = false;
                this.MostrarMensaje("Concurrencia", true);
                return;
            }
            this.MiRefTipoOperacion = refTipoOp;

            this.MiCaja.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.MiCajaMovimientoPendiente.UsuarioLogueado = this.MiCaja.UsuarioLogueado;
            this.MiRefTipoOperacion.UsuarioLogueado = this.MiCaja.UsuarioLogueado;
            this.MiCajaMovimientoPendiente.CajasMovimientosValores = new List<TESCajasMovimientosValores>(ctrIngresosValores.ObtenerCajaMovimiento().CajasMovimientosValores);

            //ARREGLO FECHAS CHEQUES CAP
            Type t2 = this.MiRefTipoOperacion.GetType();
            PropertyInfo prop = t2.GetProperty("TipoOperacion");
            TGETiposOperaciones operacion = (TGETiposOperaciones)prop.GetValue(this.MiRefTipoOperacion, null);
            if (operacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesPagos
                || operacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesPagosInterno)
            {
                CapOrdenesPagos op = (CapOrdenesPagos)this.MiRefTipoOperacion;
                foreach (CapOrdenesPagosValores item in op.OrdenesPagosValore)
                {
                    if (item.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque)
                    {
                        TESCheques cheque = this.MiCajaMovimientoPendiente.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque).Cheques.Find(y => item.IdOrdenPagoValor == y.HashTransaccion);
                        item.FechaDiferido = cheque.FechaDiferido;
                        item.NumeroCheque = cheque.NumeroCheque;
                        item.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(item, Gestion.Modificar);
                    }
                }
            }
            //FIN ARREGLO FECHAS CAP
            
            TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ConfirmaContabilizaporSP);
            bool bvalor = !string.IsNullOrEmpty(valor.ParametroValor) ? Convert.ToBoolean(valor.ParametroValor) : false;

            TGEParametrosValores deshabilitarImprimir = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.DeshabilitarImprimirComprobanteAutomatico);
            bool bdeshabilitarImprimir = !string.IsNullOrEmpty(deshabilitarImprimir.ParametroValor) ? Convert.ToBoolean(deshabilitarImprimir.ParametroValor) : false;

            bool resultado = true;
            if (!bvalor)
            {
                if (!TesoreriasF.CajasConfirmarMovimiento(this.MiCaja, this.MiCajaMovimientoPendiente, this.MiRefTipoOperacion))
                {
                    this.MostrarMensaje(this.MiCaja.CodigoMensaje, true, this.MiCaja.CodigoMensajeArgs);
                    resultado = false;
                }
            }
            else
            {
                prop = t2.GetProperty("MonedaCotizacion");
                if (prop != null)
                    MiCajaMovimientoPendiente.MonedaCotizacion = Convert.ToDecimal( prop.GetValue(this.MiRefTipoOperacion, null));

                this.MiCajaMovimientoPendiente.Fecha = this.ctrFechaCajaContable.dFechaCajaContable.Value;
                this.MiCajaMovimientoPendiente.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                this.MiCajaMovimientoPendiente.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                this.MiCajaMovimientoPendiente = TesoreriasF.CajasMovimientosObtenerMovimientoValoresXML(this.MiCajaMovimientoPendiente);
                this.MiCajaMovimientoPendiente.SelloTiempo = this.MiRefTipoOperacion.SelloTiempo;
           
                if (!TesoreriasF.CajasConfirmarMovimientoXml(this.MiCajaMovimientoPendiente))
                {
                    this.MostrarMensaje(this.MiCajaMovimientoPendiente.CodigoMensaje, true, this.MiCajaMovimientoPendiente.CodigoMensajeArgs);
                    resultado = false;
                    if (this.MiCajaMovimientoPendiente.dsResultado != null)
                    {
                        this.ctrPopUpGrilla.IniciarControl(this.MiCajaMovimientoPendiente);
                        this.MiCajaMovimientoPendiente.dsResultado = null;
                    }
                }
                if (this.MiRefTipoOperacion is TESCajasMovimientos)
                {
                    ((TESCajasMovimientos)this.MiRefTipoOperacion).IdCajaMovimiento = this.MiCajaMovimientoPendiente.IdCajaMovimiento;
                }
            }
            if (resultado)
            {
                //this.popUpMensajes.MostrarMensaje(this.MiCaja.CodigoMensaje);
                this.btnAceptar.Visible = false;
                ((nmpCajas)this.Master).ActualizarGrilla(this.MiCaja);
                Object comprobante = Enum.Parse(typeof(EnumTGEComprobantes), this.MiRefTipoOperacion.GetType().Name);
                //EnumTGETiposOperaciones tipoOperacion = Enum.ToObject(typeof(EnumTGETiposOperaciones), this.MiCajaMovimientoPendiente.TipoOperacion.IdTipoOperacion);
                if (comprobante != null)
                {
                    this.btnImprimir.Visible = true;
                    if (!bdeshabilitarImprimir)
                        this.btnImprimir_Click(null, null);
                }
                else
                {
                    this.popUpMensajes.MostrarMensaje(this.MiCaja.CodigoMensaje);
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.MiCajaMovimientoPendiente = new TESCajasMovimientos();
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosAutomaticos.aspx"), true);
        }

        protected void btnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ComprobantePlantillaCajas);
            bool bvalor = !string.IsNullOrEmpty(valor.ParametroValor) ? Convert.ToBoolean(valor.ParametroValor) : false;
            int idComprobante;

            TGEPlantillas miPlantilla = TGEGeneralesF.PlantillasObtenerDatosPorTipoOperacion(MiCajaMovimientoPendiente);
            miPlantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(miPlantilla);
            if (this.MiCajaMovimientoPendiente.IdCajaMovimiento > 0 && bvalor)
            {
                idComprobante = this.MiCajaMovimientoPendiente.IdCajaMovimiento;
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.TESCajasMovimientos, "TESCajasMovimientos", this.MiCajaMovimientoPendiente, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this.UpdatePanel1, string.Concat("Comprobante_", idComprobante.ToString().PadLeft(10, '0')), this.UsuarioActivo);
            }
            else if (miPlantilla.HtmlPlantilla.Trim().Length > 0)
            {
                Object comprobante = Enum.Parse(typeof(EnumTGEComprobantes), this.MiCajaMovimientoPendiente.GetType().Name);

                if (comprobante != null)
                {
                    TGEPlantillas plantilla = new TGEPlantillas();
                    plantilla = TGEGeneralesF.PlantillasObtenerDatosPorComprobante((EnumTGEComprobantes)comprobante);

                    Objeto MiTipoOperacion = AyudaProgramacion.ObtenerIdTipoOperacion(MiCajaMovimientoPendiente);


                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF((EnumTGEComprobantes)comprobante, miPlantilla.Codigo, MiTipoOperacion, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.UpdatePanel1, string.Concat("Comprobante_", MiCajaMovimientoPendiente.IdCajaMovimiento.ToString().PadLeft(10, '0')), this.UsuarioActivo);
                }
            }
            else
            {
                Type t2 = this.MiRefTipoOperacion.GetType();
                PropertyInfo prop = t2.GetProperty("TipoOperacion");
                TGETiposOperaciones operacion = (TGETiposOperaciones)prop.GetValue(this.MiRefTipoOperacion, null);
                prop = this.MiRefTipoOperacion.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() == 1).ToList()[0];
                idComprobante = Convert.ToInt32(prop.GetValue(this.MiRefTipoOperacion, null));

                Object comprobante = Enum.Parse(typeof(EnumTGEComprobantes), this.MiRefTipoOperacion.GetType().Name);

                if (comprobante != null)
                {
                    TGEPlantillas plantilla = new TGEPlantillas();
                    plantilla = TGEGeneralesF.PlantillasObtenerDatosPorComprobante((EnumTGEComprobantes)comprobante);

                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF((EnumTGEComprobantes)comprobante, plantilla.Codigo, this.MiRefTipoOperacion, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.UpdatePanel1, string.Concat("Comprobante_", idComprobante.ToString().PadLeft(10, '0')), this.UsuarioActivo);

                    //if (plantilla.HtmlPlantilla.Trim().Length > 0)
                    //{
                    //    TGEComprobantes coprobantePlantilla = TGEGeneralesF.ComprobantesObtenerDatosCompletos(EnumTGEComprobantes.PrePrestamos);
                    //    DataSet ds = ExportPDF.ObtenerDatosReporteComprobante(this.MiRefTipoOperacion, coprobantePlantilla);
                    //    ExportPDF.ConvertirHtmlEnPdf(this.UpdatePanel1, plantilla, ds, this.UsuarioActivo);
                    //}
                    //else
                    //{
                    //    if (operacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas
                    //&& ((CobOrdenesCobros)this.MiRefTipoOperacion).Prestamo.IdPrestamo != 0)
                    //    {
                    //        CobOrdenesCobros ordenCobro = (CobOrdenesCobros)this.MiRefTipoOperacion;
                    //        #region COMPROBANTE ORDEN/PRESTAMO
                    //        List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
                    //        TGEArchivos archivo = new TGEArchivos();
                    //        TGEArchivos archivoPre = new TGEArchivos();
                    //        //CobOrdenesCobros ordenCobroPdf = FacturasF.FacturasObtenerArchivo(factura);
                    //        byte[] pdfOrden;
                    //        byte[] pdfPrestamo;

                    //        TGEComprobantes comprobanteCobro = TGEGeneralesF.ComprobantesObtenerDatosCompletos(EnumTGEComprobantes.CobOrdenesCobros);
                    //        string archivoReporteLeer = string.Concat(this.ObtenerAppPath(), comprobanteCobro.NombreRPT);

                    //        RepReportes pReporte = new RepReportes();
                    //        pReporte.StoredProcedure = comprobanteCobro.NombreSP;
                    //        RepParametros param = new RepParametros();
                    //        param.NombreParametro = "IdOrdenCobro";
                    //        param.Parametro = "IdOrdenCobro";
                    //        param.ValorParametro = ordenCobro.IdOrdenCobro.ToString();
                    //        param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
                    //        pReporte.Parametros.Add(param);
                    //        DataSet dataSet = ReportesF.ReportesObtenerDatos(pReporte);

                    //        CrystalReportSource CryReportSource = new CrystalReportSource();
                    //        CryReportSource.CacheDuration = 1;
                    //        CryReportSource.Report.FileName = Server.MapPath(archivoReporteLeer);
                    //        CryReportSource.ReportDocument.SetDataSource(dataSet);
                    //        Stream reciboCom = CryReportSource.ReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
                    //        CryReportSource.ReportDocument.Close();
                    //        CryReportSource.Dispose();
                    //        pdfOrden = AyudaProgramacionLN.StreamToByteArray(reciboCom);
                    //        //ordenCobroPdf.IdOrdenCobro = ordenCobro.IdOrdenCobro;
                    //        archivo.Archivo = pdfOrden;
                    //        listaArchivos.Add(archivo);

                    //        //// AHORA CARGO EL PRESTAMO
                    //        TGEComprobantes comprobantePrestamo = TGEGeneralesF.ComprobantesObtenerDatosCompletos(EnumTGEComprobantes.PrePrestamos);
                    //        string archivoReporteLeer2 = string.Concat(this.ObtenerAppPath(), comprobantePrestamo.NombreRPT);

                    //        RepReportes pReportePres = new RepReportes();
                    //        pReportePres.StoredProcedure = comprobantePrestamo.NombreSP;
                    //        RepParametros paramPre = new RepParametros();
                    //        paramPre.NombreParametro = "IdPrestamo";
                    //        paramPre.Parametro = "IdPrestamo";
                    //        paramPre.ValorParametro = ordenCobro.Prestamo.IdPrestamo.ToString();
                    //        paramPre.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
                    //        pReportePres.Parametros.Add(paramPre);
                    //        DataSet dataSetPre = ReportesF.ReportesObtenerDatos(pReportePres);

                    //        CryReportSource = new CrystalReportSource();
                    //        CryReportSource.CacheDuration = 1;
                    //        CryReportSource.Report.FileName = Server.MapPath(archivoReporteLeer2);
                    //        CryReportSource.ReportDocument.SetDataSource(dataSetPre);
                    //        Stream reciboPre = CryReportSource.ReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
                    //        CryReportSource.ReportDocument.Close();
                    //        CryReportSource.Dispose();
                    //        pdfPrestamo = AyudaProgramacionLN.StreamToByteArray(reciboPre);
                    //        //ordenCobroPdf.IdOrdenCobro = ordenCobro.IdOrdenCobro;
                    //        archivoPre.Archivo = pdfPrestamo;
                    //        listaArchivos.Add(archivoPre);

                    //        //Levanto la Factura y el Remito
                    //        listaArchivos.AddRange(CobrosF.OrdenesCobrosObtenerArchivos(ordenCobro));

                    //        string nombre = string.Concat(ordenCobro.GetType().Name, "_", ordenCobro.IdOrdenCobro.ToString().PadLeft(10, '0'));
                    //        nombre = string.Format("{0}.pdf", nombre);

                    //        this.CargarReporte(listaArchivos, nombre);
                    //        #endregion
                    //    }
                    //    else
                    //    {
                    //        this.CargarReporte(this.MiRefTipoOperacion, (EnumTGEComprobantes)comprobante, true);
                    //    }
                    //}
                }
            }
        }
    }
}
