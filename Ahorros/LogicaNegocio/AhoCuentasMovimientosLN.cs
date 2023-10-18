using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ahorros.Entidades;
using Comunes;
using Servicio.AccesoDatos;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Generales.Entidades;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Comunes.LogicaNegocio;
using Generales.FachadaNegocio;
using Contabilidad.Entidades;
using System.Data;

namespace Ahorros.LogicaNegocio
{
    class AhoCuentasMovimientosLN : BaseLN<AhoCuentasMovimientos>
    {
        AhoCuentasMovimientos _movimientoComisiones;
        AhoCuentasMovimientos _movimientoReferencia;
        public AhoCuentasMovimientosLN()
        {
            this._movimientoComisiones = new AhoCuentasMovimientos();
            this._movimientoReferencia = new AhoCuentasMovimientos();
        }

        public override AhoCuentasMovimientos ObtenerDatosCompletos(AhoCuentasMovimientos pParametro)
        {
            AhoCuentasMovimientos movimiento = BaseDatos.ObtenerBaseDatos().Obtener<AhoCuentasMovimientos>("[AhoCuentasMovimientosSeleccionar]", pParametro);
            movimiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            movimiento.Cuenta = new AhoCuentasLN().ObtenerDatosCompletos(movimiento.Cuenta);
            return movimiento;
        }
        private byte[] ObtenerSelloTiempo(AhoCuentasMovimientos pParametro, Database db, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerSelloTiempo("AhoCuentasMovimientosSeleccionar", pParametro, db, tran);
        }
        public AhoCuentasMovimientos ObtenerDatosCompletos(AhoCuentasMovimientos pParametro, Database bd, DbTransaction tran)
        {
            AhoCuentasMovimientos movimiento = BaseDatos.ObtenerBaseDatos().Obtener<AhoCuentasMovimientos>("[AhoCuentasMovimientosSeleccionar]", pParametro, bd, tran); 
            movimiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            movimiento.Cuenta = new AhoCuentasLN().ObtenerDatosCompletos(movimiento.Cuenta, bd, tran);
            return movimiento;
        }

        public AhoCuentasMovimientos ObtenerDatosCompletosReferencia(AhoCuentasMovimientos pParametro, Database bd, DbTransaction tran)
        {
            AhoCuentasMovimientos movimiento = BaseDatos.ObtenerBaseDatos().Obtener<AhoCuentasMovimientos>("[AhoCuentasMovimientosSeleccionarPorReferencia]", pParametro, bd, tran);
            movimiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            movimiento.Cuenta = new AhoCuentasLN().ObtenerDatosCompletos(movimiento.Cuenta, bd, tran);
            return movimiento;
        }

        public override List<AhoCuentasMovimientos> ObtenerListaFiltro(AhoCuentasMovimientos pParametro)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Devuelve los movimientos de una cuenta de ahorro
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public List<AhoCuentasMovimientos> ObtenerListaPorCuenta(AhoCuentas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AhoCuentasMovimientos>("AhoCuentasMovimientosSeleccionarPorCuenta", pParametro);
        }    
        public DataTable ObtenerListaPorCuentaDT(AhoCuentas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("AhoCuentasMovimientosSeleccionarPorCuenta", pParametro);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool Agregar(AhoCuentasMovimientos pParametro, TGETiposFuncionalidades pTipoFuncionalidad)
        {
            if (pParametro.IdCuentaMovimiento > 0)
                return true;

            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            bool actualizaCuenta = false;

            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            if (!this.Validar(pParametro))
                return false;

            //pParametro.FechaMovimiento = DateTime.Now;
            pParametro.FechaAlta = DateTime.Now;

            //Correccion de Saldo Actual para movimientos pendientes
            pParametro.SaldoActual = this.ObtenerSaldoActual(pParametro);

            if (pTipoFuncionalidad.IdTipoFuncionalidad == (int)EnumTGETiposFuncionalidades.AhorroDepositosEspeciales
                || pTipoFuncionalidad.IdTipoFuncionalidad == (int)EnumTGETiposFuncionalidades.AhorroExtraccionesEspeciales
                )
            {
                actualizaCuenta = true;
                //pParametro.FechaConfirmacion = DateTime.Now;
                pParametro.FechaConfirmacion = pParametro.FechaMovimiento;
                pParametro.Estado.IdEstado = (int)EstadosAhorrosCuentasMovimientos.Confirmado;
                pParametro.UsuarioConfirmacion.IdUsuarioConfirmacion = pParametro.UsuarioAlta.IdUsuarioAlta;
                // Actualiza el saldo de la cuenta en el Objeto
                this.ActulizarSaldo(pParametro);
            }
                        
            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = this.Agregar(pParametro, bd, tran);

                    if (actualizaCuenta && resultado && !new AhoCuentasLN().Modificar(pParametro.Cuenta, bd, tran))
                    {
                        resultado = false;
                        AyudaProgramacionLN.MapearError(pParametro.Cuenta, pParametro);
                    }

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    pParametro.SelloTiempo = this.ObtenerSelloTiempo(pParametro, bd, tran);

                    if (resultado && (pTipoFuncionalidad.IdTipoFuncionalidad == (int)EnumTGETiposFuncionalidades.AhorroDepositosEspeciales
                || pTipoFuncionalidad.IdTipoFuncionalidad == (int)EnumTGETiposFuncionalidades.AhorroExtraccionesEspeciales
                )) {
                        resultado = new InterfazContableLN().AsientoExtraccionesDepositosEspeciales(pParametro, bd, tran);
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
                    pParametro.CodigoMensaje = ex.Message;
                    return false;
                }
                finally
                {
                    if (!resultado)
                        pParametro.IdCuentaMovimiento = 0;
                }
            }
            return resultado;
        }

        public bool Agregar(AhoCuentasMovimientos pParametro, Database bd, DbTransaction tran)
        {
                // Actualizo el saldo de la cuenta y el saldo del movimiento
                //this.ActulizarSaldo(pParametro);
                // Guardo el movimiento
                pParametro.IdCuentaMovimiento = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "AhoCuentasMovimientosInsertar");
                if (pParametro.IdCuentaMovimiento == 0)
                    return false;

                // Guardo el saldo de la cuenta                    
                //if (! new AhoCuentasLN().Modificar(pParametro.Cuenta))
                //    resultado = false;   
                return true;
        }

        public bool Anular(AhoCuentasMovimientos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            TGEEstados estado = new TGEEstados();
            estado.IdEstado = (int)EstadosAhorrosCuentasMovimientos.Baja;
            pParametro.Estado = TGEGeneralesF.TGEEstadosObtener(estado);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "AhoCuentasMovimientosActualizar");
                    
                    //Control Comentarios y Archivos
                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;
                    //Fin control Comentarios y Archivos

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
                    pParametro.CodigoMensaje = ex.Message;
                    return false;
                }
            }
            return resultado;
        }

        public override bool Modificar(AhoCuentasMovimientos pParametro)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Confirma un movimiento de Ahorro
        /// Metodo para los movimientos de Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool ConfirmarMovimiento(Objeto pParametro, DateTime pFecha, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            bool resultado = true;

            AhoCuentasMovimientos movimiento = (AhoCuentasMovimientos)pParametro;

            //Guardo el movimiento de Ahorro
            if (movimiento.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
            {
                movimiento.Estado.IdEstado = (int)EstadosAhorrosCuentasMovimientos.PendienteAcreditacionBancos;

                if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(movimiento, bd, tran, "AhoCuentasMovimientosActualizar"))
                    resultado = false;

            }
            else
            {
                movimiento.Estado.IdEstado = (int)EstadosAhorrosCuentasMovimientos.Confirmado;
                movimiento.FechaConfirmacion = pFecha;
                movimiento.UsuarioConfirmacion.IdUsuarioConfirmacion = movimiento.UsuarioLogueado.IdUsuario;

                if (resultado && !this.ActualizarConfirmarMovimiento(movimiento, bd, tran))
                    resultado = false;
            }

            if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.AhorroDepositos)
            {
                if (resultado && !new InterfazContableLN().AsientoDeposito(movimiento, pFecha, pValoresImportes, this._movimientoComisiones, bd, tran))
                    resultado = false;
            }
            else if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.AhorroExtracciones)
            {
                if (resultado && !new InterfazContableLN().AsientoExtraccion(movimiento, pValoresImportes, this._movimientoComisiones, bd, tran))
                    resultado = false;
            }

            return resultado;
        }

        /// <summary>
        /// Confirma o Rechaza un movimiento en Cheque de Ahorro
        /// </summary>
        /// <param name="cuentaMovimiento"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool ConfirmarMovimientoPorCheque(AhoCuentasMovimientos pParametro, DateTime pFecha, InterfazValoresImportes pInterfazValorImporte, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            //int estado = cuentaMovimiento.Estado.IdEstado;
            //int usuarioLogueado = cuentaMovimiento.UsuarioLogueado.IdUsuario;
            AhoCuentasMovimientos cuentaMovimiento = this.ObtenerDatosCompletos(pParametro, bd, tran);
            cuentaMovimiento.UsuarioLogueado= pParametro.UsuarioLogueado;

            if (pParametro.Estado.IdEstado == (int)EstadosAhorrosCuentasMovimientos.Confirmado)
            {
                cuentaMovimiento.Estado.IdEstado = (int)EstadosAhorrosCuentasMovimientos.Confirmado;
                cuentaMovimiento.FechaConfirmacion = pFecha;
                cuentaMovimiento.UsuarioConfirmacion.IdUsuarioConfirmacion = cuentaMovimiento.UsuarioLogueado.IdUsuario;
                
                if (resultado && !this.ActualizarConfirmarMovimiento(cuentaMovimiento, bd, tran))
                {
                    resultado = false;
                    AyudaProgramacionLN.MapearError(cuentaMovimiento, pParametro);
                }

                if(resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(cuentaMovimiento, bd, tran, "AhoCuentasMovimientosInsertarComisiones"))
                {
                    resultado = false;
                    AyudaProgramacionLN.MapearError(cuentaMovimiento, pParametro);
                }

                if (resultado && !new InterfazContableLN().AsientoAcreditarCheque(cuentaMovimiento, pInterfazValorImporte, this._movimientoComisiones, bd, tran))
                {
                    resultado = false;
                    AyudaProgramacionLN.MapearError(cuentaMovimiento, pParametro);
                }
            }
            else
            {
                cuentaMovimiento.Estado.IdEstado = (int)EstadosAhorrosCuentasMovimientos.Rechazado;
                cuentaMovimiento.FechaConfirmacion = pFecha;
                cuentaMovimiento.UsuarioConfirmacion.IdUsuarioConfirmacion = pParametro.UsuarioLogueado.IdUsuario;

                if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(cuentaMovimiento, bd, tran, "AhoCuentasMovimientosActualizar"))
                {
                    resultado = false;
                    AyudaProgramacionLN.MapearError(cuentaMovimiento, pParametro);
                }

                if (resultado && !new InterfazContableLN().AsientoRechazarCheque(cuentaMovimiento, pInterfazValorImporte, bd, tran))
                {
                    resultado = false;
                    AyudaProgramacionLN.MapearError(cuentaMovimiento, pParametro);
                }
            }
            
            return resultado;
        }

        private bool Validar(AhoCuentasMovimientos pParametro)
        {

            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "AhoCuentasMovimientosValidaciones"))
                return false;

            if (pParametro.TipoOperacion.TipoMovimiento.IdTipoMovimiento == (int)EnumTGETiposMovimientos.Debito)
            {
                if (pParametro.Importe > pParametro.Cuenta.SaldoActual)
                {
                    pParametro.CodigoMensaje = "AhoImporteMayorSaldoActual";
                    pParametro.CodigoMensajeArgs.Add(pParametro.Cuenta.SaldoActual.ToString("C2"));
                    return false;
                }
            }
            if (pParametro.FechaMovimiento.Date > DateTime.Now.Date)
            {
                pParametro.CodigoMensaje = "ValidarFechaMovimiento";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Actualiza el saldo de la cuenta y el saldo del movimiento
        /// </summary>
        /// <param name="pParametro">Movimiento</param>
        private void ActulizarSaldo(AhoCuentasMovimientos pParametro)
        {
            pParametro.Cuenta.SaldoActualOriginal = pParametro.Cuenta.SaldoActual;
            if (pParametro.TipoOperacion.TipoMovimiento.IdTipoMovimiento == (int)EnumTGETiposMovimientos.Debito)
            {
                pParametro.Cuenta.SaldoActual = pParametro.Cuenta.SaldoActualOriginal - pParametro.Importe;
            }
            else
            {
                pParametro.Cuenta.SaldoActual = pParametro.Cuenta.SaldoActualOriginal + pParametro.Importe;
            }
            pParametro.SaldoActual = pParametro.Cuenta.SaldoActual;
        }

        /// <summary>
        /// Calcula el Saldo Actual de la Cuenta
        /// </summary>
        /// <param name="pParametro">Movimiento</param>
        private decimal ObtenerSaldoActual(AhoCuentasMovimientos pParametro)
        {
            decimal saldoActual;
            pParametro.Cuenta.SaldoActualOriginal = pParametro.Cuenta.SaldoActual;
            if (pParametro.TipoOperacion.TipoMovimiento.IdTipoMovimiento == (int)EnumTGETiposMovimientos.Debito)
            {
                saldoActual = pParametro.Cuenta.SaldoActualOriginal - pParametro.Importe;
            }
            else
            {
                saldoActual = pParametro.Cuenta.SaldoActualOriginal + pParametro.Importe;
            }
            return saldoActual;
        }

        public override bool Agregar(AhoCuentasMovimientos pParametro)
        {
            throw new NotImplementedException();
        }

        //private bool AgregarComisiones(AhoCuentasMovimientos pParametro, Database bd, DbTransaction tran)
        //{
        //    //Arreglo hasta que se implemente el sistema en todas las Filiales
        //    //Solicitado por Alejandra mail 27/05/2015
        //    if (! (pParametro.IdFilial==1
        //        || pParametro.IdFilial==2
        //        || pParametro.IdFilial==3
        //        || pParametro.IdFilial==4
        //        || pParametro.IdFilial==5
        //        || pParametro.IdFilial == 7 //Cordoba
        //        || pParametro.IdFilial==13))
        //    {
        //        return true;
        //    }

        //    if (pParametro.IdFilial != pParametro.Cuenta.Filial.IdFilial)
        //    {
        //        TGEParametrosValores paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ComisionesExtraccionDepositoMontoMinimo);
        //        if (pParametro.Importe <= Convert.ToDecimal(paramValor.ParametroValor))
        //            return true;

        //        paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ComisionesExtraccionDepositoOtraSucursal);
        //        _movimientoComisiones = new AhoCuentasMovimientos();
        //        _movimientoComisiones.Cuenta = pParametro.Cuenta;
        //        _movimientoComisiones.FechaMovimiento = DateTime.Now;
        //        _movimientoComisiones.FechaValor = DateTime.Now;
        //        _movimientoComisiones.FechaConfirmacion = DateTime.Now;
        //        _movimientoComisiones.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.ComisionesExtraccionDepositoOtraSucursal;
        //        _movimientoComisiones.TipoOperacion.TipoMovimiento.IdTipoMovimiento = (int)EnumTGETiposMovimientos.Debito;
        //        _movimientoComisiones.Estado.IdEstado = (int)EstadosAhorrosCuentasMovimientos.Confirmado;
        //        _movimientoComisiones.Filial.IdFilial = pParametro.Filial.IdFilial;
        //        _movimientoComisiones.Importe = Math.Round(pParametro.Importe * Convert.ToDecimal(paramValor.ParametroValor) / 100, 2);
        //        _movimientoComisiones.TipoValor.IdTipoValor = (int)EnumTiposValores.Efectivo;
        //        _movimientoComisiones.UsuarioAlta.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuarioEvento;
        //        _movimientoComisiones.UsuarioConfirmacion.IdUsuarioConfirmacion = pParametro.UsuarioLogueado.IdUsuarioEvento;
        //        _movimientoComisiones.UsuarioLogueado = pParametro.UsuarioLogueado;
        //        _movimientoComisiones.IdRefTipoOperacion = pParametro.IdCuentaMovimiento;

        //        //Valido el Saldo de la Cuenta queda Mayor a 0
        //        if (!this.Validar(_movimientoComisiones))
        //        {
        //            AyudaProgramacionLN.MapearError(_movimientoComisiones, pParametro);
        //            pParametro.CodigoMensaje = "AhoImporteComisionMayorSaldoActual";
        //            return false;
        //        }

        //        this.ActulizarSaldo(_movimientoComisiones);

        //        if (!this.Agregar(_movimientoComisiones, bd, tran))
        //        {
        //            AyudaProgramacionLN.MapearError(_movimientoComisiones, pParametro);
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        private bool ActualizarConfirmarMovimiento(AhoCuentasMovimientos movimiento, Database bd, DbTransaction tran)
        {
            bool resultado = true;

            //Valido el Saldo de la Cuenta queda Mayor a 0
            if (!this.Validar(movimiento))
                return false;

            // Actualiza el saldo de la cuenta en el Objeto
            this.ActulizarSaldo(movimiento);

            if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(movimiento, bd, tran, "AhoCuentasMovimientosActualizar"))
                resultado = false;

            //if (resultado && !this.AgregarComisiones(movimiento, bd, tran))
            //    resultado = false;

            if (resultado && !new AhoCuentasLN().Modificar(movimiento.Cuenta, bd, tran))
            {
                resultado = false;
                AyudaProgramacionLN.MapearError(movimiento.Cuenta, movimiento);
            }

            return resultado;
        }
    }
}
