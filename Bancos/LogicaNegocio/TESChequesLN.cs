using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bancos.Entidades;
using Comunes;
using Servicio.AccesoDatos;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Comunes.Entidades;
using Generales.Entidades;
using Comunes.LogicaNegocio;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Auditoria;
using Auditoria.Entidades;
using Ahorros.Entidades;
using Ahorros;
using Contabilidad.Entidades;
using Tesorerias.Entidades;
using Tesorerias.LogicaNegocio;

namespace Bancos.LogicaNegocio
{
    class TESChequesLN : BaseLN<TESCheques>
    {
        public override TESCheques ObtenerDatosCompletos(TESCheques pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<TESCheques>("TESChequesSeleccionar", pParametro);
            pParametro.ChequesMovimientos = BaseDatos.ObtenerBaseDatos().ObtenerLista<TESChequesMovimientos>("TESChequesMovimientosSeleccionarPorTESCheques", pParametro);
            return pParametro;
        }

        public override List<TESCheques> ObtenerListaFiltro(TESCheques pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESCheques>("TESChequesSeleccionarFiltro", pParametro);
        }

        public List<TESCheques> ObtenerDisponibles(TESCheques pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESCheques>("TESChequesSeleccionarDisponibles", pParametro);
        }

        public List<TESCheques> ObtenerListaFiltroPropios(TESCheques pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESCheques>("TESChequesSeleccionarFiltroPropios", pParametro);
        }

        public override bool Agregar(TESCheques pParametro)
        {
            throw new NotImplementedException();
        }

        public override bool Modificar(TESCheques pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //if (!this.Validar(pParametro))
            //    return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            TESCheques valorViejo = new TESCheques();
            valorViejo.IdCheque = pParametro.IdCheque;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {

                    resultado = this.Modificar(pParametro, bd, tran);

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (pParametro.Estado.IdEstado == (int)Estados.Baja)
                    {
                        // Valida si el Cheque viene de Ahorros Movimientos Cuentas
                        if (BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "TESChequesValidarAhoCuentasMovimientos"))
                        {
                            AhoCuentasMovimientos movimiento = BaseDatos.ObtenerBaseDatos().Obtener<AhoCuentasMovimientos>("TESChequesAhoCuentasMovimientosSeleccionar", pParametro, bd, tran);
                            movimiento.Estado.IdEstado = pParametro.Estado.IdEstado;
                            InterfazValoresImportes valorImporte = new InterfazValoresImportes();
                            valorImporte.Importe = pParametro.Importe;
                            valorImporte.IdTipoValor = (int)EnumTiposValores.ChequeTercero;
                            valorImporte.IdCheque = pParametro.IdCheque;
                            if (!AhorroF.MovimientosConfirmarPorCheque(movimiento, DateTime.Now, null, bd, tran))
                            {
                                resultado = false;
                                AyudaProgramacionLN.MapearError(movimiento, pParametro);
                            }
                        }
                    }
         
                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        public bool Transferir(List<TESCheques> pLista, Objeto pResultado, EnumTGETiposOperaciones pTipoOperacion, 
            TESBancosCuentasMovimientos pBancoCuentaMovimiento, TESChequesMovimientos pChequeCuentaMovimiento)
        {
            bool resultado = true;
            bool actualizarTesoreria = false;
            TESChequesMovimientos chequeMovimiento;
            TESChequesLN chequesLN;
            TESBancosCuentasMovimientos cuentaMovimiento;
            TESBancosCuentasLN bcoCtasLN;
            TESTesorerias teso = new TESTesorerias();
            TESTesoreriasMonedas tesoMonedas;
            TESTesoreriasMovimientos tesoMovimiento;
            TESTesoreriasLN tesoLN  = new TESTesoreriasLN();
            AyudaProgramacionLN.LimpiarMensajesError(pResultado);

            if (pLista.Count(x => x.EstadoColeccion == EstadoColecciones.Modificado) == 0)
            {
                pResultado.CodigoMensaje = "ValidarItemsTransferencia";
                return false;
            }
            //if (pTipoOperacion == EnumTGETiposOperaciones.DepositosCuentasBancarias
            //    && pLista.Exists(x =>x.EstadoColeccion == EstadoColecciones.Modificado && x.FechaDiferido.Value.Date > DateTime.Now.Date))
            //{
            //    pResultado.CodigoMensaje = "ValidarFechaDiferidoDeposito";
            //    return false;
            //}

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    foreach (TESCheques cheque in pLista)
                    {
                        if (cheque.EstadoColeccion == EstadoColecciones.Modificado)
                        {
                            switch (pTipoOperacion)
                            {
                                case EnumTGETiposOperaciones.RecibirChequesCajas:
                                    cheque.Estado.IdEstado = (int)EstadosCheques.EnTesoreria;
                                    break;
                                case EnumTGETiposOperaciones.TransferirChequesBancos:
                                    cheque.Estado.IdEstado = (int)EstadosCheques.EnviadoSectorBancos;
                                    break;
                                case EnumTGETiposOperaciones.RecibirChequesTesoreria:
                                    cheque.Estado.IdEstado = (int)EstadosCheques.EnSectorBancos;
                                    break;
                                case EnumTGETiposOperaciones.DepositosCuentasBancarias:
                                    cheque.Estado.IdEstado = (int)EstadosCheques.EnviadoDepositar;
                                    break;
                                case EnumTGETiposOperaciones.TraspasoChequesFilial:
                                    cheque.Estado.IdEstado = (int)EstadosCheques.Traspaso;
                                    break;
                                default:
                                    break;
                            }
                            if (!this.Modificar(cheque, bd, tran))
                            {
                                resultado = false;
                                AyudaProgramacionLN.MapearError(cheque, pResultado);
                                break;
                            }

                            if (pTipoOperacion == EnumTGETiposOperaciones.DepositosCuentasBancarias)
                            {
                                bcoCtasLN = new TESBancosCuentasLN();
                                cuentaMovimiento = new TESBancosCuentasMovimientos();
                                cuentaMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
                                cuentaMovimiento.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.PendienteConciliacion;
                                cuentaMovimiento.BancoCuenta = pBancoCuentaMovimiento.BancoCuenta;
                                cuentaMovimiento.Detalle = pBancoCuentaMovimiento.Detalle;
                                cuentaMovimiento.NumeroTipoOperacion = pBancoCuentaMovimiento.NumeroTipoOperacion;
                                cuentaMovimiento.FechaAlta = DateTime.Now;
                                cuentaMovimiento.FechaMovimiento = DateTime.Now;
                                cuentaMovimiento.Importe = cheque.Importe;
                                cuentaMovimiento.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.DepositosCuentasBancarias;

                                cuentaMovimiento.UsuarioAlta.IdUsuarioAlta = cheque.UsuarioLogueado.IdUsuarioEvento;
                                cuentaMovimiento.UsuarioLogueado = cheque.UsuarioLogueado;

                                chequeMovimiento = new TESChequesMovimientos();
                                chequeMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
                                chequeMovimiento.Cheque.IdCheque = cheque.IdCheque;
                                chequeMovimiento.Fecha = DateTime.Now;
                                chequeMovimiento.Filial.IdFilial = cheque.Filial.IdFilial;
                                chequeMovimiento.FilialDestino.IdFilial = cheque.Filial.IdFilial;
                                chequeMovimiento.TipoOperacion.IdTipoOperacion = (int)pTipoOperacion;
                                chequeMovimiento.Estado.IdEstado = (int)Estados.Activo;
                                chequeMovimiento.UsuarioLogueado = cheque.UsuarioLogueado;
                                if (!bcoCtasLN.AgregarMovimiento(cuentaMovimiento, bd, tran))
                                {
                                    resultado = false;
                                    AyudaProgramacionLN.MapearError(cuentaMovimiento, pResultado);
                                    break;
                                }
                                chequeMovimiento.IdRefBancoCuentaMovimiento = cuentaMovimiento.IdBancoCuentaMovimiento;

                                //Saco el Cheque de la Tesoreria si es un Deposito de Ch Terceros
                                if (!cheque.ChequePropio)
                                {
                                    if (teso.IdTesoreria==0)
                                    {
                                        teso.Filial.IdFilial = cheque.Filial.IdFilial;
                                        teso.UsuarioLogueado = cheque.UsuarioLogueado;
                                        teso = tesoLN.ObtenerPorAbierta(teso, bd, tran);
                                        if (teso.IdTesoreria == 0)
                                        {
                                            resultado = false;
                                            pResultado.CodigoMensaje = "TesoreriaValidarAbierta";
                                            break;
                                        }
                                        else
                                        {
                                            teso.UsuarioLogueado = cheque.UsuarioLogueado;
                                            teso = tesoLN.ObtenerDatosCompletos(teso, bd, tran);
                                        }

                                    }
                                    //SALIDA DE TESORERIA - SOLO CHEQUES TERCEROS
                                    tesoMovimiento = new TESTesoreriasMovimientos();
                                    tesoMovimiento.TesoreriaMoneda.Moneda.IdMoneda = (int)EnumTGEMonedas.PesosArgentinos;
                                    tesoMovimiento.Estado.IdEstado = (int)EstadosTesoreriasMovimientos.Activo;
                                    tesoMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
                                    tesoMovimiento.Fecha = teso.FechaAbrir;
                                    tesoMovimiento.Importe = cheque.Importe;
                                    tesoMovimiento.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.TransferirChequesBancos;
                                    tesoMovimiento.TipoOperacion.TipoMovimiento.IdTipoMovimiento = (int)EnumTGETiposMovimientos.Debito;
                                    tesoMovimiento.TipoValor.IdTipoValor = (int)EnumTiposValores.ChequeTercero;
                                    tesoMovimiento.IdRefTipoOperacion = cuentaMovimiento.IdBancoCuentaMovimiento;
                                    tesoMovimiento.UsuarioLogueado = cheque.UsuarioLogueado;
                                    tesoMonedas = teso.TesoreriasMonedas.Find(x => x.Moneda.IdMoneda == tesoMovimiento.TesoreriaMoneda.Moneda.IdMoneda && x.TipoValor.IdTipoValor == tesoMovimiento.TipoValor.IdTipoValor);
                                    tesoMonedas.TesoreriasMovimientos.Add(tesoMovimiento);
                                    actualizarTesoreria = true; 
                                }
                                if (!this.AgregarMovimiento(chequeMovimiento, bd, tran))
                                {
                                    resultado = false;
                                    AyudaProgramacionLN.MapearError(chequeMovimiento, pResultado);
                                    break;
                                }
                            }
                            else if (pTipoOperacion == EnumTGETiposOperaciones.TraspasoChequesFilial)
                            {
                                chequesLN = new TESChequesLN();
                                chequeMovimiento = new TESChequesMovimientos();
                                chequeMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
                                chequeMovimiento.Cheque.IdCheque = cheque.IdCheque;
                                chequeMovimiento.Fecha = DateTime.Now;
                                chequeMovimiento.Filial.IdFilial = pChequeCuentaMovimiento.Filial.IdFilial;
                                chequeMovimiento.FilialDestino.IdFilialDestino = pChequeCuentaMovimiento.FilialDestino.IdFilialDestino;
                                chequeMovimiento.FilialDestino.Descripcion = pChequeCuentaMovimiento.FilialDestino.Descripcion;
                                chequeMovimiento.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.TraspasoChequesFilial;
                                chequeMovimiento.Descripcion = pChequeCuentaMovimiento.Descripcion;
                                chequeMovimiento.Estado.IdEstado = (int)EstadosCheques.Traspaso;
                                chequeMovimiento.UsuarioLogueado = cheque.UsuarioLogueado;

                                if (!chequesLN.AgregarMovimiento(chequeMovimiento, bd, tran))
                                {
                                    resultado = false;
                                    AyudaProgramacionLN.MapearError(chequeMovimiento, pResultado);
                                    break;
                                }
                                //Saco el Cheque de la Tesoreria si es un Deposito de Ch Terceros
                                if (!cheque.ChequePropio)
                                {
                                    if (teso.IdTesoreria == 0)
                                    {
                                        teso.Filial.IdFilial = cheque.Filial.IdFilial;
                                        teso.UsuarioLogueado = cheque.UsuarioLogueado;
                                        teso = tesoLN.ObtenerPorAbierta(teso, bd, tran);
                                        if (teso.IdTesoreria == 0)
                                        {
                                            resultado = false;
                                            pResultado.CodigoMensaje = "TesoreriaValidarAbierta";
                                            break;
                                        }
                                        else
                                        {
                                            teso.UsuarioLogueado = cheque.UsuarioLogueado;
                                            teso = tesoLN.ObtenerDatosCompletos(teso, bd, tran);
                                        }

                                    }
                                    //SALIDA DE TESORERIA - SOLO CHEQUES TERCEROS
                                    tesoMovimiento = new TESTesoreriasMovimientos();
                                    tesoMovimiento.TesoreriaMoneda.Moneda.IdMoneda = (int)EnumTGEMonedas.PesosArgentinos;
                                    tesoMovimiento.Estado.IdEstado = (int)EstadosTesoreriasMovimientos.Activo;
                                    tesoMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
                                    tesoMovimiento.Fecha = teso.FechaAbrir;
                                    tesoMovimiento.Importe = cheque.Importe;
                                    tesoMovimiento.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.TraspasoChequesFilial;
                                    tesoMovimiento.TipoOperacion.TipoMovimiento.IdTipoMovimiento = (int)EnumTGETiposMovimientos.Debito;
                                    tesoMovimiento.TipoValor.IdTipoValor = (int)EnumTiposValores.ChequeTercero;
                                    tesoMovimiento.IdRefTipoOperacion = chequeMovimiento.IdChequeMovimiento;
                                    tesoMovimiento.Descripcion = chequeMovimiento.FilialDestino.Descripcion;
                                    tesoMovimiento.UsuarioLogueado = cheque.UsuarioLogueado;
                                    tesoMonedas = teso.TesoreriasMonedas.Find(x => x.Moneda.IdMoneda == tesoMovimiento.TesoreriaMoneda.Moneda.IdMoneda && x.TipoValor.IdTipoValor == tesoMovimiento.TipoValor.IdTipoValor);
                                    tesoMonedas.TesoreriasMovimientos.Add(tesoMovimiento);
                                    actualizarTesoreria = true;
                                }
                            }
                        }
                    }

                    if (resultado && actualizarTesoreria)
                    {
                        if (!tesoLN.ActualizarMonedasMovimientos(teso, bd, tran))
                        {
                            resultado = false;
                            AyudaProgramacionLN.MapearError(teso, pResultado);
                        }
                    }
                    
                    if (resultado)
                    {
                        tran.Commit();
                        pResultado.CodigoMensaje = "ResultadoTransaccion";
                    }

                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pResultado.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pResultado.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }

            return resultado;
        }

        public bool Agregar(TESCheques pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdCheque = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "TESChequesInsertar");
            if (pParametro.IdCheque == 0)
                return false;

            return true;
        }

        /// <summary>
        /// Modifica un Cheque
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool Modificar(TESCheques pParametro, Database bd, DbTransaction tran)
        {
            if(! BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TESChequesActualizar"))
                return false;

            return true;
        }

        /// <summary>
        /// Acredita o Rechaza un Cheque
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool AcreditarCheque(TESBancosCuentasMovimientos pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;

            TESCheques cheque = BaseDatos.ObtenerBaseDatos().Obtener<TESCheques>("[TESChequesSeleccionarPorBancoCuentaMovimiento]", pParametro, bd, tran);
            if (cheque.IdCheque > 0)
            {
                cheque.Estado.IdEstado = pParametro.Estado.IdEstado == (int)EstadosBancosCuentasMovimientos.Confirmado ? (int)EstadosCheques.Depositado : (int)EstadosCheques.Rechazado;
                cheque.UsuarioLogueado = pParametro.UsuarioLogueado;

                TESChequesMovimientos chequeMovimiento = new TESChequesMovimientos();
                chequeMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
                chequeMovimiento.Cheque.IdCheque = cheque.IdCheque;
                chequeMovimiento.Fecha = DateTime.Now;
                chequeMovimiento.Filial.IdFilial = cheque.Filial.IdFilial;
                chequeMovimiento.FilialDestino.IdFilial = cheque.Filial.IdFilial;
                chequeMovimiento.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.AcreditacionCheque;
                chequeMovimiento.Estado.IdEstado = cheque.Estado.IdEstado;
                chequeMovimiento.UsuarioLogueado = cheque.UsuarioLogueado;

                if (!this.Modificar(cheque, bd, tran))
                {
                    resultado = false;
                    AyudaProgramacionLN.MapearError(cheque, pParametro);
                }

                if (resultado && !this.AgregarMovimiento(chequeMovimiento, bd, tran))
                {
                    resultado = false;
                    AyudaProgramacionLN.MapearError(chequeMovimiento, pParametro);
                }

                // Valida si el Cheque viene de Ahorros Movimientos Cuentas
                if (resultado && BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(cheque, bd, tran, "TESChequesValidarAhoCuentasMovimientos"))
                {
                    AhoCuentasMovimientos movimiento = BaseDatos.ObtenerBaseDatos().Obtener<AhoCuentasMovimientos>("TESChequesAhoCuentasMovimientosSeleccionar", cheque, bd, tran);
                    movimiento.Estado.IdEstado = pParametro.Estado.IdEstado;
                    movimiento.UsuarioLogueado = pParametro.UsuarioLogueado;
                    InterfazValoresImportes pValorImporte = new InterfazValoresImportes();
                    pValorImporte.IdBancoCuenta = pParametro.BancoCuenta.IdBancoCuenta;
                    pValorImporte.IdCheque = cheque.IdCheque;
                    pValorImporte.IdTipoValor = (int)EnumTiposValores.ChequeTercero;
                    pValorImporte.Importe = cheque.Importe;

                    if (!AhorroF.MovimientosConfirmarPorCheque(movimiento, pParametro.FechaConfirmacionBanco, pValorImporte, bd, tran))
                    {
                        resultado = false;
                        AyudaProgramacionLN.MapearError(movimiento, pParametro);
                    }
                }
                // Valida si el Cheque viene de Prestamos con Cheques
                else if (resultado &&
                    BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(cheque, bd, tran, "TESChequesValidarPrePrestamosCheques"))
                {
                    if (pParametro.Estado.IdEstado == (int)EstadosBancosCuentasMovimientos.Confirmado)
                    {
                        if (!BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(cheque, bd, tran, "TesChequesActualizarPrestamosCheques"))
                        {
                            resultado = false;
                            AyudaProgramacionLN.MapearError(cheque, pParametro);
                        }
                    }
                    else if (pParametro.Estado.IdEstado == (int)EstadosBancosCuentasMovimientos.Rechazado)
                    {
                        if (!BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(cheque, bd, tran, "TesChequesActualizarPrestamosChequesRechazo"))
                        {
                            resultado = false;
                            AyudaProgramacionLN.MapearError(cheque, pParametro);
                        }
                    }
                }
            }
            return resultado;
        }

        /// <summary>
        /// Anula el Deposito de un Cheque
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool AnularDepositoCheque(TESBancosCuentasMovimientos pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;

            TESCheques cheque = BaseDatos.ObtenerBaseDatos().Obtener<TESCheques>("[TESChequesSeleccionarPorBancoCuentaMovimiento]", pParametro, bd, tran);
            cheque.Estado.IdEstado = (int)EstadosCheques.EnTesoreria;  //(int)EstadosCheques.EnSectorBancos;
            cheque.UsuarioLogueado = pParametro.UsuarioLogueado;

            TESChequesMovimientos chequeMovimiento = new TESChequesMovimientos();
            chequeMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
            chequeMovimiento.Cheque.IdCheque = cheque.IdCheque;
            chequeMovimiento.Fecha = DateTime.Now;
            chequeMovimiento.Filial.IdFilial = cheque.Filial.IdFilial;
            chequeMovimiento.FilialDestino.IdFilial = cheque.Filial.IdFilial;
            chequeMovimiento.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.AnularDepositosCuentasBancarias;
            chequeMovimiento.Estado.IdEstado = cheque.Estado.IdEstado;
            chequeMovimiento.UsuarioLogueado = cheque.UsuarioLogueado;

            if (!this.Modificar(cheque, bd, tran))
            {
                resultado = false;
                AyudaProgramacionLN.MapearError(cheque, pParametro);
            }

            if (!this.AgregarMovimiento(chequeMovimiento, bd, tran))
            {
                resultado = false;
                AyudaProgramacionLN.MapearError(chequeMovimiento, pParametro);
            }

            if (!cheque.ChequePropio)
            {
                TESTesorerias teso = new TESTesorerias();
                TESTesoreriasMonedas tesoMonedas;
                TESTesoreriasMovimientos tesoMovimiento;
                TESTesoreriasLN tesoLN = new TESTesoreriasLN();

                teso.Filial.IdFilial = cheque.Filial.IdFilial;
                teso.UsuarioLogueado = cheque.UsuarioLogueado;
                teso = tesoLN.ObtenerPorAbierta(teso, bd, tran);
                if (teso.IdTesoreria == 0)
                {
                    pParametro.CodigoMensaje = "TesoreriaValidarAbierta";
                    return false;
                }
                else
                {
                    teso.UsuarioLogueado = cheque.UsuarioLogueado;
                    teso = tesoLN.ObtenerDatosCompletos(teso, bd, tran);
                }

                //SALIDA DE TESORERIA - SOLO CHEQUES TERCEROS
                tesoMovimiento = new TESTesoreriasMovimientos();
                tesoMovimiento.TesoreriaMoneda.Moneda.IdMoneda = (int)EnumTGEMonedas.PesosArgentinos;
                tesoMovimiento.Estado.IdEstado = (int)EstadosTesoreriasMovimientos.Activo;
                tesoMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
                tesoMovimiento.Fecha = teso.FechaAbrir;
                tesoMovimiento.Importe = cheque.Importe;
                tesoMovimiento.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.AnularDepositosCuentasBancarias;
                tesoMovimiento.TipoOperacion.TipoMovimiento.IdTipoMovimiento = (int)EnumTGETiposMovimientos.Credito;
                tesoMovimiento.TipoValor.IdTipoValor = (int)EnumTiposValores.ChequeTercero;
                tesoMovimiento.IdRefTipoOperacion = pParametro.IdBancoCuentaMovimiento;
                tesoMovimiento.UsuarioLogueado = cheque.UsuarioLogueado;
                tesoMonedas = teso.TesoreriasMonedas.Find(x => x.Moneda.IdMoneda == tesoMovimiento.TesoreriaMoneda.Moneda.IdMoneda && x.TipoValor.IdTipoValor == tesoMovimiento.TipoValor.IdTipoValor);
                tesoMonedas.TesoreriasMovimientos.Add(tesoMovimiento);

            }

            return resultado;
        }

        /// <summary>
        /// Agrega un Movimiento de Cheque
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool AgregarMovimiento(TESChequesMovimientos pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdChequeMovimiento = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "TESChequesMovimientosInsertar");
            if (pParametro.IdChequeMovimiento == 0)
                return false;

            return true;
        }

        public List<TESCheques> ObtenerChequesTerceros(TESCheques pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESCheques>("TESChequesSeleccionarCambiarValores", pParametro);
        }

        public List<TESCheques> ObtenerChequesTercerosFiltro(TESCheques pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESCheques>("TESChequesSeleccionarCambiarValoresPorFiltro", pParametro);
        }
        public TESChequesMovimientos ObtenerDatosCompletosChequesMovimientos(TESChequesMovimientos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<TESChequesMovimientos>("TESChequesMovimientosSeleccionar", pParametro);
        }
    }
}
