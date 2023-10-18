using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesorerias.Entidades;
using Tesorerias;
using Comunes.Entidades;
using System.Reflection;
using Generales.Entidades;
using CuentasPagar.Entidades;
using Bancos.Entidades;

namespace ProcesoAutomaticoTesorerias
{
    class Program
    {
        static void Main(string[] args)
        {
            bool resultado = true;
            int idUsuario = 120;
            int idFilial = 1;

            TESTesorerias tesoreria = new TESTesorerias();
            tesoreria.UsuarioLogueado.IdUsuario = idUsuario; // = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            tesoreria.UsuarioLogueado.IdUsuarioEvento = idUsuario;
            tesoreria.FechaAbrir = DateTime.Now;
            tesoreria.Filial.IdFilial = idFilial; //tesoreria.Filial.IdFilial = UsuarioActivo.FilialPredeterminada.IdFilial;

            //Valida que no haya Tesorerias Abiertas anteriores al día de la Fecha
            if (!TesoreriasF.TesoreriasValidarAbiertaFechaAnterior(tesoreria))
            {
                tesoreria = TesoreriasF.TesoreriasObtenerAbierta(tesoreria);
                //Cierro la Tesoreria Anterior
                resultado = TesoreriasF.TesoreriasCierreAutomatico(tesoreria);
            }

            //Genera la apertura automatica de la Tesoreria
            if (resultado && TesoreriasF.TesoreriaAbrirObtenerDatos(tesoreria))
            {
                tesoreria = TesoreriasF.TesoreriasObtenerDatosCompletos(tesoreria);
            }

            TESCajas MiCaja = new TESCajas(); //Abrir una Caja por Filial
            MiCaja.UsuarioLogueado = tesoreria.UsuarioLogueado; // AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            MiCaja.Usuario.IdUsuario = idUsuario; // this.UsuarioActivo;
            MiCaja.Usuario.IdUsuarioEvento = idUsuario;
            MiCaja.Tesoreria = tesoreria;
            MiCaja.FechaAbrir = DateTime.Now;
            MiCaja.Usuario.FilialPredeterminada.IdFilial = idFilial;// this.UsuarioActivo.FilialPredeterminada;

            //Valida que tenga la Caja Abierta
            if (TesoreriasF.CajasValidarAbierta(MiCaja))
            {
                MiCaja = TesoreriasF.CajasObtenerDatosCompletos(MiCaja);
            }
            else
            {
                //Abro la Caja
                MiCaja.NumeroCaja = 1;
                if (TesoreriasF.CajasAbrirObtenerDatos(MiCaja))
                {
                    MiCaja = TesoreriasF.CajasObtenerDatosCompletos(MiCaja);
                }
                else
                {
                    //NO SE PUDO ABRIR LA CAJA SALIR!!! ERRROR!
                }
            }

            List<TESCajasMovimientos> MisCajasMovimientosPendientes = TesoreriasF.CajasObtenerMovimientosPendientes(tesoreria);
            TESCajasMovimientos MiCajaMovimientoPendiente;
            List<TESCajasMovimientosValores> CajasMovimientosValores;

            foreach (TESCajasMovimientos movimiento in MisCajasMovimientosPendientes)
            {
                Objeto MiRefTipoOperacion = TesoreriasF.CajasObtenerMovimientoPendienteConfirmacion(movimiento);

                MiCajaMovimientoPendiente = new TESCajasMovimientos();

                Type t2 = MiRefTipoOperacion.GetType();
                PropertyInfo prop = t2.GetProperty("TipoOperacion");
                TGETiposOperaciones operacion = (TGETiposOperaciones)prop.GetValue(MiRefTipoOperacion, null);

                TESCajasMovimientosValores valor;
                TESCheques cheque;
                switch (operacion.IdTipoOperacion)
                {
                    #region Ordenes de Pago
                    case (int)EnumTGETiposOperaciones.OrdenesPagos:
                        CapOrdenesPagos op = (CapOrdenesPagos)MiRefTipoOperacion;
                        CajasMovimientosValores = new List<TESCajasMovimientosValores>();

                        foreach (CapOrdenesPagosValores item in op.OrdenesPagosValore)
                        {
                            //EFECTIVO
                            if (item.TipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo)
                            {
                                if (MiCajaMovimientoPendiente.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo))
                                    MiCajaMovimientoPendiente.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo).Importe = item.Importe;
                                else
                                {
                                    valor = new TESCajasMovimientosValores();
                                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                                    valor.TipoValor = item.TipoValor;
                                    valor.Importe = item.Importe;
                                    MiCajaMovimientoPendiente.CajasMovimientosValores.Add(valor);
                                    valor.IndiceColeccion = MiCajaMovimientoPendiente.CajasMovimientosValores.IndexOf(valor);
                                }
                            }//CHEQUE
                            else if (item.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque)
                            {
                                if (!MiCajaMovimientoPendiente.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque))
                                {
                                    valor = new TESCajasMovimientosValores();
                                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                                    valor.TipoValor = item.TipoValor;
                                    MiCajaMovimientoPendiente.CajasMovimientosValores.Add(valor);
                                    valor.IndiceColeccion = MiCajaMovimientoPendiente.CajasMovimientosValores.IndexOf(valor);
                                }

                                valor = MiCajaMovimientoPendiente.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque);
                                cheque = new TESCheques();
                                cheque.EstadoColeccion = EstadoColecciones.Agregado;
                                cheque.Estado.IdEstado = (int)EstadosCheques.Entregado;
                                cheque.Filial.IdFilial = idFilial;// UsuarioActivo.FilialPredeterminada.IdFilial;
                                cheque.Fecha = item.Fecha.Value;
                                cheque.FechaDiferido = item.FechaDiferido;
                                cheque.NumeroCheque = item.NumeroCheque;
                                cheque.Banco.IdBanco = item.BancoCuenta.Banco.IdBanco;
                                cheque.Banco.Descripcion = item.BancoCuenta.Banco.Descripcion;
                                cheque.ChequePropio = item.ChequePropio;
                                cheque.Importe = item.Importe;
                                cheque.IdBancoCuenta = item.BancoCuenta.IdBancoCuenta;
                                cheque.TipoOperacion = op.TipoOperacion;
                                //Para buscar el cheque despues
                                item.HashTransaccion = item.GetHashCode();
                                cheque.HashTransaccion = item.HashTransaccion;
                                valor.Cheques.Add(cheque);
                                cheque.IndiceColeccion = valor.Cheques.IndexOf(cheque);
                                valor.Importe = valor.Cheques.Sum(x => x.Importe);
                            }
                            else if (item.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
                            {
                                if (!MiCajaMovimientoPendiente.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero))
                                {
                                    valor = new TESCajasMovimientosValores();
                                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                                    valor.TipoValor = item.TipoValor;
                                    MiCajaMovimientoPendiente.CajasMovimientosValores.Add(valor);
                                    valor.IndiceColeccion = MiCajaMovimientoPendiente.CajasMovimientosValores.IndexOf(valor);
                                }
                                valor = MiCajaMovimientoPendiente.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero);
                                item.Cheque.EstadoColeccion = EstadoColecciones.Modificado;
                                item.Cheque.Estado.IdEstado = (int)EstadosCheques.Entregado;
                                valor.Cheques.Add(item.Cheque);
                                item.Cheque.IndiceColeccion = valor.Cheques.IndexOf(item.Cheque);
                                valor.Importe = valor.Cheques.Sum(x => x.Importe);
                            }
                            else if (item.TipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia)
                            {
                                if (!MiCajaMovimientoPendiente.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia))
                                {
                                    valor = new TESCajasMovimientosValores();
                                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                                    valor.TipoValor = item.TipoValor;
                                    MiCajaMovimientoPendiente.CajasMovimientosValores.Add(valor);
                                    valor.IndiceColeccion = MiCajaMovimientoPendiente.CajasMovimientosValores.IndexOf(valor);
                                }

                                valor = MiCajaMovimientoPendiente.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia);

                                TESBancosCuentasMovimientos bancoMovimiento = new TESBancosCuentasMovimientos();
                                bancoMovimiento.BancoCuenta.IdBancoCuenta = item.BancoCuenta.IdBancoCuenta;
                                bancoMovimiento.Importe = item.Importe;
                                bancoMovimiento.TipoOperacion = op.TipoOperacion;
                                bancoMovimiento.FechaAlta = DateTime.Now;
                                bancoMovimiento.FechaMovimiento = DateTime.Now;
                                bancoMovimiento.FechaConfirmacionBanco = DateTime.Now;
                                bancoMovimiento.FechaConciliacion = DateTime.Now;
                                bancoMovimiento.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.PendienteConfirmacion;
                                bancoMovimiento.EstadoColeccion = EstadoColecciones.Agregado;

                                valor.BancosCuentasMovimientos.Add(bancoMovimiento);
                                valor.Importe = valor.BancosCuentasMovimientos.Sum(x => x.Importe);
                            }
                        }

                        break;
                    #endregion

                    default:
                        break;
                }

                MiCaja.UsuarioLogueado = tesoreria.UsuarioLogueado; //AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                MiCajaMovimientoPendiente.UsuarioLogueado = MiCaja.UsuarioLogueado;
                MiRefTipoOperacion.UsuarioLogueado = MiCaja.UsuarioLogueado;

                //SOLO PESOS ARGENTINOS
                MiCajaMovimientoPendiente.CajaMoneda.IdCajaMoneda = (int)EnumTGEMonedas.PesosArgentinos;

                if (!TesoreriasF.CajasConfirmarMovimiento(MiCaja, MiCajaMovimientoPendiente, MiRefTipoOperacion))
                {
                    //this.MostrarMensaje(this.MiCaja.CodigoMensaje, true, this.MiCaja.CodigoMensajeArgs);
                    //NO SE PUDO CONFIRMAR EL MOVIMIENTO ERROR!!!
                }
            }
        }
    }
}
