using Auditoria;
using Auditoria.Entidades;
using Bancos;
using Bancos.Entidades;
using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Tesorerias.Entidades;

namespace Tesorerias.LogicaNegocio
{
    public class TESTesoreriasLN : BaseLN<TESTesorerias>
    {
        /// <summary>
        /// Si es la primera ves genera la apertura de la Tesoreria y
        /// Obtiene los datos completos de la Tesoreria
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool AbrirObtenerDatosTesoreria(TESTesorerias pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            if (pParametro.FechaAbrir > DateTime.Now)
            {
                pParametro.CodigoMensaje = "ValidarFecha";
                return false;
            }
            TESTesorerias teso = this.ObtenerPorFilialFecha(pParametro);
            if (teso.IdTesoreria == 0 && DateTime.Equals(pParametro.FechaAbrirEvento.Date, DateTime.Now.Date))
            {
                if (!this.Agregar(pParametro))
                    return false;
            }
            else
            {
                pParametro.IdTesoreria = teso.IdTesoreria;
            }
            return true;
        }

        /// <summary>
        /// Valida si la Tesoreria esta abierta para el día de la fecha
        /// </summary>
        /// <param name="pParametros">IdFilial</param>
        /// <returns></returns>
        public bool ValidarAbierta(TESTesorerias pParametros)
        {
            //pParametros.FechaAbrir = DateTime.Now;
            TESTesorerias teso = this.ObtenerPorFilialFecha(pParametros);

            if (teso.IdTesoreria == 0)
            {
                pParametros.CodigoMensaje = "TesoreriaValidarAbierta";
                return false;
            }
            pParametros.IdTesoreria = teso.IdTesoreria;
            return true;
        }

        /// <summary>
        /// Valida si la Tesoreria esta cerrada para el día de la fecha
        /// </summary>
        /// <param name="pParametros">IdFilial</param>
        /// <returns></returns>
        public bool ValidarCerrada(TESTesorerias pParametros)
        {
            pParametros.FechaAbrir = DateTime.Now;
            TESTesorerias teso = this.ObtenerPorFilialFecha(pParametros);

            if (teso.Estado.IdEstado == (int)EstadosTesorerias.Cerrada)
            {
                pParametros.CodigoMensaje = "TesoreriaValidarCerrada";
                return false;
            }

            pParametros.IdTesoreria = teso.IdTesoreria;
            return true;
        }

        /// <summary>
        /// Valida si la Tesoreria esta abierta para un día Anterior
        /// </summary>
        /// <param name="pParametros">IdFilial</param>
        /// <returns></returns>
        public bool ValidarAbiertaFechaAnterior(TESTesorerias pParametro)
        {
            TESTesorerias teso = BaseDatos.ObtenerBaseDatos().Obtener<TESTesorerias>("TESTesoreriasSeleccionarAbiertas", pParametro);

            if (teso.IdTesoreria > 0 && teso.FechaAbrirEvento.Date < DateTime.Now.Date)
            {
                pParametro.CodigoMensaje = "ValidarAbiertaFechaAnterior";
                pParametro.CodigoMensajeArgs.Add(teso.FechaAbrirEvento.Date.ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// Devuelve una Tesoreria por Filial y Fecha
        /// </summary>
        /// <param name="pParametro">IdFilial, FechaAbrir</param>
        /// <returns></returns>
        public TESTesorerias ObtenerPorFilialFecha(TESTesorerias pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<TESTesorerias>("TESTesoreriasSeleccionarPorFilialFechaAbrir", pParametro);
        }

        /// <summary>
        /// Devuelve la última Tesoreria Abierta
        /// </summary>
        /// <param name="pParametro">IdFilial, IdUsuarioEvento</param>
        /// <returns></returns>
        public TESTesorerias ObtenerPorAbierta(TESTesorerias pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<TESTesorerias>("TESTesoreriasSeleccionarAbiertas", pParametro);
        }

        /// <summary>
        /// Devuelve la última Tesoreria Abierta
        /// </summary>
        /// <param name="pParametro">IdFilial, IdUsuarioEvento</param>
        /// <returns></returns>
        public TESTesorerias ObtenerPorAbierta(TESTesorerias pParametro, Database bd, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<TESTesorerias>("TESTesoreriasSeleccionarAbiertas", pParametro, bd, tran);
        }

        /// <summary>
        /// Obtiene los datos completos de la Tesoreria con los tipos de monedas y valores
        /// </summary>
        /// <param name="pParametro">IdTesoreria</param>
        /// <returns></returns>
        public override TESTesorerias ObtenerDatosCompletos(TESTesorerias pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<TESTesorerias>("TESTesoreriasSeleccionar", pParametro);
            pParametro.TesoreriasMonedas = BaseDatos.ObtenerBaseDatos().ObtenerLista<TESTesoreriasMonedas>("TESTesoreriasMonedasSeleccionarPorTesoreria", pParametro);
            foreach (TESTesoreriasMonedas monedas in pParametro.TesoreriasMonedas)
            {
                monedas.TesoreriasMovimientos = BaseDatos.ObtenerBaseDatos().ObtenerLista<TESTesoreriasMovimientos>("TESTesoreriasMovimientosSeleccionarPorTesoreriaMoneda", monedas);
            }
            return pParametro;
        }
        public DataTable ObtenerDatosCompletosSaldosCajas(TESTesorerias pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("TESTesoreriasMonedasSeleccionarPorTesoreriaSaldosCajas", pParametro);
        }

        /// <summary>
        /// Obtiene los datos completos de la Tesoreria con los tipos de monedas y valores
        /// </summary>
        /// <param name="pParametro">IdTesoreria</param>
        /// <returns></returns>
        public TESTesorerias ObtenerDatosCompletos(TESTesorerias pParametro, Database bd, DbTransaction tran)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<TESTesorerias>("TESTesoreriasSeleccionar", pParametro, bd, tran);
            pParametro.TesoreriasMonedas = BaseDatos.ObtenerBaseDatos().ObtenerLista<TESTesoreriasMonedas>("TESTesoreriasMonedasSeleccionarPorTesoreria", pParametro, bd, tran);
            foreach (TESTesoreriasMonedas monedas in pParametro.TesoreriasMonedas)
            {
                monedas.TesoreriasMovimientos = BaseDatos.ObtenerBaseDatos().ObtenerLista<TESTesoreriasMovimientos>("TESTesoreriasMovimientosSeleccionarPorTesoreriaMoneda", monedas, bd, tran);
            }
            return pParametro;
        }

        public override List<TESTesorerias> ObtenerListaFiltro(TESTesorerias pParametro)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Devuelve los movimientos Pendientes de Confirmación (aceptacion) para una Tesoreria
        /// </summary>
        /// <param name="pParametro">IdFilial</param>
        /// <returns></returns>, Database bd, DbTransaction tran
        public List<TESTesoreriasMovimientos> ObtenerPendientesTransferencia(TESTesorerias pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESTesoreriasMovimientos>("TESTesoreriasMovimientosSeleccionarTransferenciasPendientes", pParametro);
        }

        /// <summary>
        /// Confirma un movimiento pendiente
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool ConfirmarTesoreriaMovimiento(TESTesorerias pTesoreria, TESTesoreriasMovimientos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pTesoreria);
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            pTesoreria.TesoreriasMonedas.Find(x => x.Moneda.IdMoneda == (int)EnumTGEMonedas.PesosArgentinos).TesoreriasMovimientos.RemoveAll(x => x.EstadoColeccion == EstadoColecciones.Agregado);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            TESBancosCuentasMovimientos movReferencia = new TESBancosCuentasMovimientos();
            movReferencia.IdBancoCuentaMovimiento = pParametro.IdRefTipoOperacion;
            movReferencia = BancosF.BancosCuentasMovimientosObtenerDatosCompletos(movReferencia);
            movReferencia.Estado = TGEGeneralesF.TGEEstadosObtener(new TGEEstados() { IdEstado = (int)EstadosBancosCuentasMovimientos.Confirmado });
            movReferencia.UsuarioLogueado = pParametro.UsuarioLogueado;
            movReferencia.SelloTiempo = pParametro.SelloTiempo;
            movReferencia.EstadoColeccion = EstadoColecciones.Modificado;
            pTesoreria.TesoreriasMonedas.Find(x => x.Moneda.IdMoneda == (int)EnumTGEMonedas.PesosArgentinos && x.TipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo).TesoreriasMovimientos.Add(pParametro);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    //Guardo Tesorerias Monedas yTesorerias Movimientos
                    resultado = this.ActualizarMonedasMovimientos(pTesoreria, bd, tran);

                    if (resultado)
                    {
                        movReferencia.IdRefTipoOperacion = pParametro.IdTesoreriaMovimiento;
                        if (!new InterfazContableLN().TransferenciaDesdeBancos(pTesoreria, pParametro, movReferencia, bd, tran))
                        {
                            resultado = false;
                        }
                        //if (!BancosF.BancosCuentasMovimientoTrasnferenciaTesoreriaActualizar(movReferencia, bd, tran))
                        //{
                        //    AyudaProgramacionLN.MapearError(movReferencia, pTesoreria);
                        //    resultado = false;
                        //}
                    }

                    if (resultado)
                    {
                        tran.Commit();
                        pTesoreria.CodigoMensaje = "ResultadoTransaccion";
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
                    pTesoreria.CodigoMensaje = ex.Message;
                    return false;
                }
            }
            return resultado;
        }

        /// <summary>
        /// Genera la Apertura de la Tesoreria para el día de la fecha
        /// Copia los saldos de la última tesoreria
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public override bool Agregar(TESTesorerias pParametro)
        {
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.FechaAbrirEvento = DateTime.Now;
            pParametro.UsuarioAbrir.IdUsuarioAbrir = pParametro.UsuarioLogueado.IdUsuarioEvento;
            pParametro.Estado.IdEstado = (int)EstadosTesorerias.Abierta;

            if (!this.ValidarAbiertaFechaAnterior(pParametro))
            {
                return false;
            }

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    //OBTENGO LA ULTIMA TESORERIA CERRADA PARA VER QUE LA FECHA ACTUAL NO SEA MENOR A LA TESORERIA ANTERIOR
                    TESTesorerias teso = this.ObtenerUltimoCierreFilialUsuarioEvento(pParametro, bd, tran);
                    if (pParametro.FechaAbrir <= teso.FechaAbrir)
                    {
                        pParametro.CodigoMensaje = "ValidarFechaApertura";
                        resultado = false;
                    }
                    //si pasa la validacion de fecha, continuo...
                    pParametro.IdTesoreria = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "TESTesoreriasInsertar");
                    if (pParametro.IdTesoreria == 0)
                        resultado = false;

                    if (resultado && !this.CopiarDatosUltimaTesoreria(pParametro, bd, tran))
                        resultado = false;

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

        /// <summary>
        /// Actualiza los datos de la tesoreria
        /// Guarda los movimientos de Tesoreria Monedas y Tesorerias Movimientos
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public override bool Modificar(TESTesorerias pParametro)
        {
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    //Guardo Tesorerias Monedas yTesorerias Movimientos
                    resultado = this.ActualizarMonedasMovimientos(pParametro, bd, tran);

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

        public bool ModificarReabrirTesoreria(TESTesorerias pParametro)
        {
            bool resultado = true;

            TESTesorerias tesoClone = AyudaProgramacionLN.Clone<TESTesorerias>(pParametro);

            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            pParametro.Estado.IdEstado = (int)EstadosTesorerias.Abierta;
            pParametro.FechaCerrar = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    //Guardo Tesorerias Monedas yTesorerias Movimientos
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TESTesoreriasActualizar");

                    if (resultado && !AuditoriaF.AuditoriaAgregar(tesoClone, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

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

        /// <summary>
        /// Metodo para generar el cierre de la tesoreria
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool Cerrar(TESTesorerias pParametro)
        {
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            pParametro.FechaCerrar = DateTime.Now;
            pParametro.Estado.IdEstado = (int)EstadosTesorerias.Cerrada;
            pParametro.UsuarioCerrar.IdUsuarioCerrar = pParametro.UsuarioLogueado.IdUsuarioEvento;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    ////VALIDA QUE NO HAYA CAJAS ABIERTAS
                    //if (!new TESCajasLN().ValidarAbiertas(pParametro))
                    //    resultado = false;

                    //CIERRO TODAS LAS CAJAS
                    TESCajasLN cajasLN = new TESCajasLN();
                    foreach (TESCajas caja in pParametro.Cajas)
                    {
                        if (caja.Estado.IdEstado == (int)EstadosCajas.Abierta)
                        {
                            caja.UsuarioLogueado = pParametro.UsuarioLogueado;
                            #region Actualizacion de Cajas

                            //if (!new TESCajasLN().Modificar(caja, pParametro, bd, tran))
                            //{
                            //    //si algo sale mal, mapeo el error de la caja
                            //    pParametro.CodigoMensaje = caja.CodigoMensaje;
                            //    return false;
                            //}

                            caja.EstadoColeccion = EstadoColecciones.Modificado;
                            caja.FechaCerrar = DateTime.Now;
                            caja.Estado.IdEstado = (int)EstadosCajas.Cerrada;

                            if (caja.Usuario.SectorPredeterminado.IdSector != (int)EnumTGESectores.Bancos)
                            {
                                caja.Tesoreria.UsuarioLogueado.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;
                                TESTesoreriasMonedas tesoMonedas;
                                TESTesoreriasMovimientos tesoMovimiento;
                                TESCajasMovimientos cajaMovimiento;
                                TESCajasMovimientosValores cajaMovValor;

                                foreach (TESCajasMonedas cajaMoneda in caja.CajasMonedas)
                                {
                                    cajaMoneda.CajasMovimientos.RemoveAll(x => x.EstadoColeccion == EstadoColecciones.Agregado);
                                    //Actualizao Caja Moneda
                                    //cajaMoneda.SaldoFinal = 0; // cajaMoneda.SaldoInicial + cajaMoneda.Ingreso - cajaMoneda.Egreso;
                                    cajaMoneda.EstadoColeccion = EstadoColecciones.Modificado;
                                    cajaMoneda.UsuarioLogueado = pParametro.UsuarioLogueado;

                                    #region Movimiento Tesoreria / Caja Efectivo

                                    //Genero el Movimiento de Tesoreria CHEQUES
                                    List<TESCheques> lista = BaseDatos.ObtenerBaseDatos().ObtenerLista<TESCheques>("TESChequesSeleccionarPorCaja", caja, bd, tran);
                                    if (lista.Count > 0)
                                    {
                                        tesoMovimiento = new TESTesoreriasMovimientos();
                                        tesoMovimiento.TesoreriaMoneda.Moneda.IdMoneda = cajaMoneda.Moneda.IdMoneda;
                                        tesoMovimiento.Estado.IdEstado = (int)EstadosTesoreriasMovimientos.Activo;
                                        tesoMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
                                        tesoMovimiento.Fecha = pParametro.FechaAbrir;
                                        tesoMovimiento.Importe = lista.Sum(x => x.Importe);
                                        tesoMovimiento.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.RecibirChequesCajas;
                                        tesoMovimiento.TipoOperacion.TipoMovimiento.IdTipoMovimiento = (int)EnumTGETiposMovimientos.Credito;
                                        tesoMovimiento.TipoValor.IdTipoValor = (int)EnumTiposValores.ChequeTercero;
                                        tesoMovimiento.UsuarioLogueado = pParametro.UsuarioLogueado;
                                        tesoMonedas = pParametro.TesoreriasMonedas.Find(x => x.Moneda.IdMoneda == cajaMoneda.Moneda.IdMoneda && x.TipoValor.IdTipoValor == tesoMovimiento.TipoValor.IdTipoValor);
                                        tesoMonedas.TesoreriasMovimientos.Add(tesoMovimiento);

                                        TESChequesMovimientos chequeMovimiento;
                                        foreach (TESCheques cheque in lista)
                                        {
                                            cajaMovimiento = new TESCajasMovimientos();
                                            cajaMovimiento.UsuarioLogueado = pParametro.UsuarioLogueado;
                                            cajaMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
                                            cajaMovimiento.Estado.IdEstado = (int)EstadosCajasMovimientos.Activo;
                                            cajaMovimiento.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.RetiroCheques);
                                            cajaMovimiento.Descripcion = string.Concat("Cheque ", cheque.NumeroCheque);
                                            //cajaMovimiento.IdRefTipoOperacion = cheque.IdCheque;
                                            cajaMovimiento.IdRefTipoOperacion = tesoMovimiento.IdTesoreriaMovimiento;
                                            cajaMovimiento.Importe = cheque.Importe;
                                            cajaMoneda.CajasMovimientos.Add(cajaMovimiento);

                                            cajaMovValor = new TESCajasMovimientosValores();
                                            cajaMovValor.EstadoColeccion = EstadoColecciones.Agregado;
                                            cajaMovValor.TipoValor.IdTipoValor = (int)EnumTiposValores.ChequeTercero;
                                            cajaMovValor.Importe = cajaMovimiento.Importe;
                                            cajaMovValor.Estado.IdEstado = (int)Estados.Activo;
                                            cajaMovValor.UsuarioLogueado = cajaMovimiento.UsuarioLogueado;
                                            cajaMovimiento.CajasMovimientosValores.Add(cajaMovValor);

                                            cheque.EstadoColeccion = EstadoColecciones.Modificado;
                                            cheque.UsuarioLogueado = pParametro.UsuarioLogueado;
                                            cheque.Estado.IdEstado = (int)EstadosCheques.EnTesoreria;
                                            if (!BancosF.ChequesModificar(cheque, bd, tran))
                                            {
                                                AyudaProgramacionLN.MapearError(cheque, pParametro);
                                                return false;
                                            }
                                            chequeMovimiento = new TESChequesMovimientos();
                                            chequeMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
                                            chequeMovimiento.Cheque.IdCheque = cheque.IdCheque;
                                            chequeMovimiento.Fecha = pParametro.FechaAbrir;
                                            chequeMovimiento.Filial.IdFilial = cheque.Filial.IdFilial;
                                            chequeMovimiento.FilialDestino.IdFilial = cheque.Filial.IdFilial;
                                            chequeMovimiento.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.RecibirChequesCajas;
                                            chequeMovimiento.IdRefChequeMovimiento = tesoMovimiento.IdTesoreriaMovimiento;
                                            chequeMovimiento.Estado.IdEstado = (int)Estados.Activo;
                                            chequeMovimiento.UsuarioLogueado = cheque.UsuarioLogueado;
                                            if (!BancosF.ChequesAgregarMovimiento(chequeMovimiento, bd, tran))
                                            {
                                                AyudaProgramacionLN.MapearError(chequeMovimiento, pParametro);
                                                return false;
                                            }
                                        }
                                    }
                                    #endregion
                                }

                            }

                            if (!BaseDatos.ObtenerBaseDatos().Actualizar(caja, bd, tran, "TESCajasActualizar"))
                                resultado = false;

                            //Actualizao los saldos finales de Cajas Monedas
                            if (resultado && !cajasLN.ActualizarMonedasMovimientos(caja, bd, tran))
                                resultado = false;

                            #endregion
                        }
                    }

                    //Devuelve los Saldos a la Tesoreria
                    foreach (TESTesoreriasMonedas tesoreriaMoneda in pParametro.TesoreriasMonedas)
                    {
                        foreach (TESTesoreriasMovimientos movimiento in tesoreriaMoneda.TesoreriasMovimientos)
                        {
                            switch (movimiento.EstadoColeccion)
                            {
                                case EstadoColecciones.Agregado:
                                    movimiento.TesoreriaMoneda.IdTesoreriaMoneda = tesoreriaMoneda.IdTesoreriaMoneda;
                                    movimiento.Fecha = pParametro.FechaAbrir;
                                    movimiento.UsuarioLogueado = pParametro.UsuarioLogueado;
                                    movimiento.Estado.IdEstado = (int)EstadosTesoreriasMovimientos.Activo;
                                    movimiento.IdTesoreriaMovimiento = BaseDatos.ObtenerBaseDatos().Agregar(movimiento, bd, tran, "TESTesoreriasMovimientosInsertar");
                                    if (movimiento.IdTesoreriaMovimiento == 0)
                                        return false;
                                    break;
                                default:
                                    break;
                            }
                        }
                        //Actualizo las Tesorerias Monedas para que tome el saldo FINAL
                        tesoreriaMoneda.Ingreso = tesoreriaMoneda.TesoreriasMovimientos.Where(x => x.TipoOperacion.TipoMovimiento.IdTipoMovimiento == (int)EnumTGETiposMovimientos.Credito).Sum(x => x.Importe);
                        tesoreriaMoneda.Egreso = tesoreriaMoneda.TesoreriasMovimientos.Where(x => x.TipoOperacion.TipoMovimiento.IdTipoMovimiento == (int)EnumTGETiposMovimientos.Debito).Sum(x => x.Importe);
                        tesoreriaMoneda.SaldoFinal = tesoreriaMoneda.SaldoInicial + tesoreriaMoneda.Ingreso - tesoreriaMoneda.Egreso;
                        tesoreriaMoneda.Tesoreria.IdTesoreria = pParametro.IdTesoreria;
                        tesoreriaMoneda.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(tesoreriaMoneda, bd, tran, "TESTesoreriasMonedasActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(tesoreriaMoneda, pParametro);
                            return false;
                        }
                    }
                    //Actualizo la (o cierro) tesoreria  
                    if (resultado && !this.ActualizarTesoreria(pParametro, bd, tran))
                    {
                        pParametro.CodigoMensaje = "TesoreriaCerrar";
                        resultado = false;
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
            }
            return resultado;
        }

        /// <summary>
        /// Genera el cierre de una tesoreria
        /// Si existen cajas abiertas para la Tesoreria anterior a la fecha las cierra.
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool CierreAutomatico(TESTesorerias pParametro)
        {
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            pParametro.FechaCerrar = DateTime.Now;
            pParametro.Estado.IdEstado = (int)EstadosTesorerias.Cerrada;
            pParametro.UsuarioCerrar.IdUsuarioCerrar = pParametro.UsuarioLogueado.IdUsuarioEvento;

            if (pParametro.FechaAbrir.Date == DateTime.Now.Date)
            {
                pParametro.CodigoMensaje = "ValidarCierreAutomaticoFechaDia";
                return false;
            }

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();
                try
                {
                    TESCajasLN cajaLN = new TESCajasLN();
                    TESCajas cajaFiltro = new TESCajas();
                    cajaFiltro.Tesoreria.IdTesoreria = pParametro.IdTesoreria;
                    cajaFiltro.Estado.IdEstado = (int)EstadosCajas.Abierta;
                    List<TESCajas> cajasAbiertas = cajaLN.ObtenerListaFiltro(cajaFiltro);
                    //Validar que no haya Cajas Abiertas para la Tesoreria a cerrar del día anterior a la fecha.
                    if (cajasAbiertas.Count > 0)
                    {
                        TESCajas cajaCerrar;
                        foreach (TESCajas caja in cajasAbiertas)
                        {
                            cajaCerrar = caja;
                            cajaCerrar = cajaLN.ObtenerDatosMonedas(caja);
                            cajaCerrar.Tesoreria = pParametro;
                            cajaCerrar.UsuarioLogueado = pParametro.UsuarioLogueado;
                            if (!cajaLN.Modificar(cajaCerrar, bd, tran))
                            {
                                pParametro.CodigoMensaje = "CajaCerrarDiaAnterior";
                                resultado = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (TESTesoreriasMonedas tesoreriaMoneda in pParametro.TesoreriasMonedas)
                        {
                            //Actualizo las Tesorerias Monedas para que tome el saldo FINAL
                            tesoreriaMoneda.Ingreso = tesoreriaMoneda.TesoreriasMovimientos.Where(x => x.TipoOperacion.TipoMovimiento.IdTipoMovimiento == (int)EnumTGETiposMovimientos.Credito).Sum(x => x.Importe);
                            tesoreriaMoneda.Egreso = tesoreriaMoneda.TesoreriasMovimientos.Where(x => x.TipoOperacion.TipoMovimiento.IdTipoMovimiento == (int)EnumTGETiposMovimientos.Debito).Sum(x => x.Importe);
                            tesoreriaMoneda.SaldoFinal = tesoreriaMoneda.SaldoInicial + tesoreriaMoneda.Ingreso - tesoreriaMoneda.Egreso;
                            tesoreriaMoneda.Tesoreria.IdTesoreria = pParametro.IdTesoreria;
                            tesoreriaMoneda.UsuarioLogueado = pParametro.UsuarioLogueado;
                            if (!BaseDatos.ObtenerBaseDatos().Actualizar(tesoreriaMoneda, bd, tran, "TESTesoreriasMonedasActualizar"))
                            {
                                AyudaProgramacionLN.MapearError(tesoreriaMoneda, pParametro);
                                return false;
                            }
                        }
                    }

                    if (resultado && !this.ActualizarTesoreria(pParametro, bd, tran))
                        resultado = false;

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

        /// <summary>
        /// Actualiza los datos de la Tesoreria
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        private bool ActualizarTesoreria(TESTesorerias pParametro, Database bd, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TESTesoreriasActualizar");
        }

        /// <summary>
        /// Copia los datos de monedas de la tesoreria anterior
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        private bool CopiarDatosUltimaTesoreria(TESTesorerias pParametro, Database bd, DbTransaction tran)
        {
            TESTesorerias tesUltimoCierre = BaseDatos.ObtenerBaseDatos().Obtener<TESTesorerias>("TESTesoreriasSeleccionarUltimoCierre", pParametro, bd, tran);
            if (tesUltimoCierre.IdTesoreria > 0)
            {
                //tesUltimoCierre.UsuarioLogueado = pParametro.UsuarioLogueado;
                pParametro.TesoreriasMonedas = BaseDatos.ObtenerBaseDatos().ObtenerLista<TESTesoreriasMonedas>("TESTesoreriasMonedasSeleccionarPorTesoreriaCopiar", tesUltimoCierre, bd, tran);
                if (this.AgregarMonedas(pParametro, bd, tran))
                    return true;
            }
            else
            {
                //Si nunca se utilizo la Tesoreria copia las monedas de Sede Central
                TESTesorerias tesoFiltro = new TESTesorerias();
                tesoFiltro.Filial.IdFilial = 1;
                tesoFiltro.UsuarioLogueado.IdUsuarioEvento = 1;
                tesUltimoCierre = BaseDatos.ObtenerBaseDatos().Obtener<TESTesorerias>("TESTesoreriasSeleccionarUltimoCierre", tesoFiltro, bd, tran);
                if (tesUltimoCierre.IdTesoreria > 0)
                {
                    pParametro.TesoreriasMonedas = BaseDatos.ObtenerBaseDatos().ObtenerLista<TESTesoreriasMonedas>("TESTesoreriasMonedasSeleccionarPorTesoreriaCopiar", tesUltimoCierre, bd, tran);
                    foreach (TESTesoreriasMonedas moneda in pParametro.TesoreriasMonedas)
                        moneda.SaldoFinal = 0;

                    if (this.AgregarMonedas(pParametro, bd, tran))
                        return true;
                }
            }
            pParametro.CodigoMensaje = "ValidarTesoreriaUltimoCierre";
            return false;
        }

        /// <summary>
        /// Guarda las Monedas para un nuevo dia de Tesoreria
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        private bool AgregarMonedas(TESTesorerias pParametro, Database bd, DbTransaction tran)
        {
            foreach (TESTesoreriasMonedas moneda in pParametro.TesoreriasMonedas)
            {
                moneda.Tesoreria.IdTesoreria = pParametro.IdTesoreria;
                moneda.Ingreso = 0;
                moneda.Egreso = 0;
                moneda.SaldoInicial = moneda.SaldoFinal;
                moneda.SaldoFinal = moneda.SaldoFinal;
                moneda.Estado.IdEstado = (int)EstadosTesoreriasMonedas.Activo;
                moneda.UsuarioLogueado = pParametro.UsuarioLogueado;
                moneda.IdTesoreriaMoneda = BaseDatos.ObtenerBaseDatos().Agregar(moneda, bd, tran, "TESTesoreriasMonedasInsertar");
                if (moneda.IdTesoreriaMoneda == 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Metodo para generar las Altas de Movimientos de Tesorerias
        /// Actualiza los saldos por moneda
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool ActualizarMonedasMovimientos(TESTesorerias pParametro, Database bd, DbTransaction tran)
        {
            foreach (TESTesoreriasMonedas tesoreriaMoneda in pParametro.TesoreriasMonedas)
            {
                foreach (TESTesoreriasMovimientos movimiento in tesoreriaMoneda.TesoreriasMovimientos)
                {
                    switch (movimiento.EstadoColeccion)
                    {
                        case EstadoColecciones.Agregado:
                            movimiento.TesoreriaMoneda.IdTesoreriaMoneda = tesoreriaMoneda.IdTesoreriaMoneda;
                            movimiento.Fecha = pParametro.FechaAbrir;
                            movimiento.UsuarioLogueado = pParametro.UsuarioLogueado;
                            movimiento.Estado.IdEstado = (int)EstadosTesoreriasMovimientos.Activo;
                            //if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaBancosDebito)
                            //{
                            //    movimiento.Estado.IdEstado = (int)EstadosTesoreriasMovimientos.PendienteConfirmacion;
                            //}
                            //else
                            //{
                            //    movimiento.Estado.IdEstado = (int)EstadosTesoreriasMovimientos.Activo;
                            //}
                            movimiento.IdTesoreriaMovimiento = BaseDatos.ObtenerBaseDatos().Agregar(movimiento, bd, tran, "TESTesoreriasMovimientosInsertar");
                            if (movimiento.IdTesoreriaMovimiento == 0)
                                return false;

                            switch (movimiento.TipoOperacion.IdTipoOperacion)
                            {
                                // Genero el movimiento en la CAJA.
                                case (int)EnumTGETiposOperaciones.RendicionEfectivoForzadoTesoreria:
                                case (int)EnumTGETiposOperaciones.AsignacionEfectivoCaja:
                                case (int)EnumTGETiposOperaciones.RefuerzoEfectivoCaja:
                                    TESCajasMovimientos cajaMovimiento = new TESCajasMovimientos();
                                    cajaMovimiento.CajaMoneda.Moneda = tesoreriaMoneda.Moneda;
                                    cajaMovimiento.Importe = movimiento.Importe;
                                    cajaMovimiento.Descripcion = movimiento.TipoOperacion.TipoOperacion;
                                    //SE AGREGO EL IF PARA MOVIMIENTO DE DEBITO O CREDITO
                                    if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.RendicionEfectivoForzadoTesoreria)
                                        cajaMovimiento.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.TraspasoEfectivoCajaATesoreria, bd, tran);
                                    else
                                        cajaMovimiento.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.IngresoEfectivoDesdeTesoreria, bd, tran);

                                    cajaMovimiento.IdRefTipoOperacion = movimiento.IdTesoreriaMovimiento;
                                    //cajaMovimiento.TipoValor = movimiento.TipoValor;
                                    cajaMovimiento.Fecha = DateTime.Now;
                                    cajaMovimiento.Estado.IdEstado = (int)EstadosCajasMovimientos.Activo;
                                    cajaMovimiento.UsuarioLogueado = movimiento.UsuarioLogueado;
                                    cajaMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
                                    TESCajasMovimientosValores cajaMovValor = new TESCajasMovimientosValores();
                                    cajaMovValor.Importe = movimiento.Importe;
                                    cajaMovValor.EstadoColeccion = EstadoColecciones.Agregado;
                                    cajaMovValor.TipoValor.IdTipoValor = (int)EnumTiposValores.Efectivo;
                                    cajaMovimiento.CajasMovimientosValores.Add(cajaMovValor);

                                    TESCajas caja = movimiento.Caja;
                                    caja.CajasMonedas.Find(x => x.Moneda.IdMoneda == tesoreriaMoneda.Moneda.IdMoneda).CajasMovimientos.Add(cajaMovimiento);
                                    if (!new TESCajasLN().ActualizarMonedasMovimientos(caja, bd, tran))
                                    {
                                        AyudaProgramacionLN.MapearError(caja, pParametro);
                                        return false;
                                    }
                                    break;
                                //Transferencias a Bancos
                                case (int)EnumTGETiposOperaciones.TransferenciaBancosDebito:
                                    TESBancosCuentasMovimientos bancoMovimiento = new TESBancosCuentasMovimientos();
                                    bancoMovimiento.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.TransferenciaTesoreriaCredito;
                                    bancoMovimiento.BancoCuenta.IdBancoCuenta = movimiento.IdRefTipoOperacion;
                                    bancoMovimiento.IdRefTipoOperacion = movimiento.IdTesoreriaMovimiento;
                                    bancoMovimiento.FechaAlta = DateTime.Now;
                                    bancoMovimiento.FechaMovimiento = DateTime.Now;
                                    bancoMovimiento.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.PendienteConciliacion;
                                    bancoMovimiento.IdTesoreriaMovimiento = movimiento.IdTesoreriaMovimiento;
                                    bancoMovimiento.Importe = movimiento.Importe;
                                    bancoMovimiento.UsuarioAlta.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuarioEvento;
                                    bancoMovimiento.UsuarioLogueado = pParametro.UsuarioLogueado;
                                    if (!BancosF.BancosCuentasMovimientosAgregar(bancoMovimiento, bd, tran))
                                    {
                                        AyudaProgramacionLN.MapearError(bancoMovimiento, pParametro);
                                        return false;
                                    }
                                    break;
                                default:
                                    break;
                            }

                            tesoreriaMoneda.EstadoColeccion = EstadoColecciones.Modificado;
                            break;
                        default:
                            break;
                    }
                }
                //Actualizo las Tesorerias Monedas
                tesoreriaMoneda.Ingreso = tesoreriaMoneda.TesoreriasMovimientos.Where(x => x.TipoValor.IdTipoValor == tesoreriaMoneda.TipoValor.IdTipoValor && x.TipoOperacion.TipoMovimiento.IdTipoMovimiento == (int)EnumTGETiposMovimientos.Credito).Sum(x => x.Importe);
                tesoreriaMoneda.Egreso = tesoreriaMoneda.TesoreriasMovimientos.Where(x => x.TipoValor.IdTipoValor == tesoreriaMoneda.TipoValor.IdTipoValor && x.TipoOperacion.TipoMovimiento.IdTipoMovimiento == (int)EnumTGETiposMovimientos.Debito).Sum(x => x.Importe);
                tesoreriaMoneda.SaldoFinal = tesoreriaMoneda.SaldoInicial + tesoreriaMoneda.Ingreso - tesoreriaMoneda.Egreso;
                if (tesoreriaMoneda.EstadoColeccion == EstadoColecciones.Modificado)
                {
                    tesoreriaMoneda.Tesoreria.IdTesoreria = pParametro.IdTesoreria;
                    tesoreriaMoneda.UsuarioLogueado = pParametro.UsuarioLogueado;
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(tesoreriaMoneda, bd, tran, "TESTesoreriasMonedasActualizar"))
                    {
                        AyudaProgramacionLN.MapearError(tesoreriaMoneda, pParametro);
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Transfiere los Cheques de la Caja a Tesoreria y Tesoreria al sector Bancos
        /// </summary>
        /// <param name="pLista"></param>
        /// <param name="pResultado"></param>
        /// <param name="pTipoOperacion"></param>
        /// <param name="pBancoCuentaMovimiento"></param>
        /// <returns></returns>
        public bool TransferirCheques(List<TESCheques> pLista, TESTesorerias pTesoreria, EnumTGETiposOperaciones pTipoOperacion)
        {
            bool resultado = true;
            TESChequesMovimientos chequeMovimiento;
            AyudaProgramacionLN.LimpiarMensajesError(pTesoreria);
            pTesoreria.TesoreriasMonedas.Find(x => x.Moneda.IdMoneda == (int)EnumTGEMonedas.PesosArgentinos).TesoreriasMovimientos.RemoveAll(x => x.EstadoColeccion == EstadoColecciones.Agregado);

            List<TESCheques> listaCheques = pLista.Where(x => x.EstadoColeccion == EstadoColecciones.Modificado).ToList();

            if (listaCheques.Count == 0)
            {
                pTesoreria.CodigoMensaje = "ValidarItemsTransferencia";
                return false;
            }

            TGETiposOperaciones tipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(pTipoOperacion);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    TESTesoreriasMovimientos movTeso = new TESTesoreriasMovimientos();
                    movTeso.EstadoColeccion = EstadoColecciones.Agregado;
                    movTeso.Estado.IdEstado = (int)EstadosTesoreriasMovimientos.Activo;
                    movTeso.Fecha = DateTime.Now;
                    movTeso.Importe = listaCheques.Sum(x => x.Importe);
                    movTeso.TipoOperacion = tipoOperacion;
                    movTeso.TipoValor.IdTipoValor = (int)EnumTiposValores.ChequeTercero;
                    movTeso.TesoreriaMoneda.Moneda.IdMoneda = (int)EnumTGEMonedas.PesosArgentinos;
                    movTeso.UsuarioLogueado = pTesoreria.UsuarioLogueado;
                    pTesoreria.TesoreriasMonedas.Find(x => x.Moneda.IdMoneda == movTeso.TesoreriaMoneda.Moneda.IdMoneda && x.TipoValor.IdTipoValor == movTeso.TipoValor.IdTipoValor).TesoreriasMovimientos.Add(movTeso);

                    if (!this.ActualizarMonedasMovimientos(pTesoreria, bd, tran))
                        return false;

                    foreach (TESCheques cheque in listaCheques)
                    {
                        switch (pTipoOperacion)
                        {
                            case EnumTGETiposOperaciones.RecibirChequesCajas:
                                cheque.Estado.IdEstado = (int)EstadosCheques.EnTesoreria;
                                break;
                            case EnumTGETiposOperaciones.TransferirChequesBancos:
                                cheque.Estado.IdEstado = (int)EstadosCheques.EnviadoSectorBancos;
                                break;
                            default:
                                break;
                        }
                        if (!BancosF.ChequesModificar(cheque, bd, tran))
                        {
                            resultado = false;
                            AyudaProgramacionLN.MapearError(cheque, pTesoreria);
                            break;
                        }

                        chequeMovimiento = new TESChequesMovimientos();
                        chequeMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
                        chequeMovimiento.Cheque.IdCheque = cheque.IdCheque;
                        chequeMovimiento.Fecha = DateTime.Now;
                        chequeMovimiento.Filial.IdFilial = cheque.Filial.IdFilial;
                        chequeMovimiento.FilialDestino.IdFilial = cheque.Filial.IdFilial;
                        chequeMovimiento.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.TransferirChequesBancos);
                        chequeMovimiento.IdRefChequeMovimiento = movTeso.IdTesoreriaMovimiento;
                        chequeMovimiento.Estado.IdEstado = (int)Estados.Activo;
                        chequeMovimiento.UsuarioLogueado = cheque.UsuarioLogueado;

                        if (!BancosF.ChequesAgregarMovimiento(chequeMovimiento, bd, tran))
                        {
                            resultado = false;
                            AyudaProgramacionLN.MapearError(chequeMovimiento, pTesoreria);
                            break;
                        }
                    }

                    if (pTipoOperacion == EnumTGETiposOperaciones.RecibirChequesCajas
                        && !this.AgregarMovimientoTransferirCheques(listaCheques, pTesoreria, bd, tran))
                    {
                        resultado = false;
                    }

                    if (resultado)
                    {
                        tran.Commit();
                        pTesoreria.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                        tran.Rollback();

                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pTesoreria.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pTesoreria.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }

            return resultado;
        }

        ///// <summary>
        ///// Transfiere Efectivo de Bancos a Tesoreria y Tesoreria a Bancos
        ///// </summary>
        ///// <param name="pLista"></param>
        ///// <param name="pResultado"></param>
        ///// <param name="pTipoOperacion"></param>
        ///// <param name="pBancoCuentaMovimiento"></param>
        ///// <returns></returns>
        //public bool TransferirEfectivo(TESBancosCuentasMovimientos pParmetro)
        //{
        //    bool resultado = true;
        //    AyudaProgramacionLN.LimpiarMensajesError(pParmetro);

        //    TESTesorerias teso = new TESTesorerias();
        //    teso.Filial.IdFilial = pParmetro.BancoCuentaDestino.Filial.IdFilial;
        //    teso.FechaAbrir = DateTime.Now;
        //    teso.UsuarioLogueado = pParmetro.UsuarioLogueado;

        //    if (!this.ValidarAbierta(teso))
        //    {
        //        AyudaProgramacionLN.MapearError(teso, pParmetro);
        //        return false;
        //    }            
        //    teso = this.ObtenerPorAbierta(teso);

        //    TESTesoreriasMovimientos movTeso;
        //    movTeso.EstadoColeccion = EstadoColecciones.Agregado;
        //    movTeso.Estado.IdEstado = (int)EstadosTesoreriasMovimientos.PendienteConfirmacion;
        //    movTeso.Fecha = DateTime.Now;
        //    movTeso.Importe = pParmetro.Importe;
        //    movTeso.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.TransferenciaEfectivoCredito;
        //    movTeso.TipoValor.IdTipoValor = (int)EnumTiposValores.Efectivo;
        //    movTeso.TesoreriaMoneda.Moneda.IdMoneda = (int)EnumTGEMonedas.PesosArgentinos;
        //    movTeso.UsuarioLogueado = pParmetro.UsuarioLogueado;
        //    teso.TesoreriasMonedas.Find(x => x.Moneda.IdMoneda == movTeso.TesoreriaMoneda.Moneda.IdMoneda).TesoreriasMovimientos.Add(movTeso);

        //    DbTransaction tran;
        //    DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

        //    using (DbConnection con = bd.CreateConnection())
        //    {
        //        con.Open();
        //        tran = con.BeginTransaction();

        //        try
        //        {
        //            if (!this.ActualizarMonedasMovimientos(pTesoreria, bd, tran))
        //                return false;

        //            foreach (TESCheques cheque in listaCheques)
        //            {
        //                switch (pTipoOperacion)
        //                {
        //                    case EnumTGETiposOperaciones.RecibirChequesCajas:
        //                        cheque.Estado.IdEstado = (int)EstadosCheques.EnTesoreria;
        //                        break;
        //                    case EnumTGETiposOperaciones.TransferirChequesBancos:
        //                        cheque.Estado.IdEstado = (int)EstadosCheques.EnviadoSectorBancos;
        //                        break;
        //                    default:
        //                        break;
        //                }
        //                if (!BancosF.ChequesModificar(cheque, bd, tran))
        //                {
        //                    resultado = false;
        //                    AyudaProgramacionLN.MapearError(cheque, pTesoreria);
        //                    break;
        //                }

        //                chequeMovimiento = new TESChequesMovimientos();
        //                chequeMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
        //                chequeMovimiento.Cheque.IdCheque = cheque.IdCheque;
        //                chequeMovimiento.Fecha = DateTime.Now;
        //                chequeMovimiento.Filial.IdFilial = cheque.Filial.IdFilial;
        //                chequeMovimiento.FilialDestino.IdFilial = cheque.Filial.IdFilial;
        //                chequeMovimiento.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.TransferirChequesBancos);
        //                chequeMovimiento.IdRefChequeMovimiento = movTeso.IdTesoreriaMovimiento;
        //                chequeMovimiento.Estado.IdEstado = (int)Estados.Activo;
        //                chequeMovimiento.UsuarioLogueado = cheque.UsuarioLogueado;

        //                if (!BancosF.ChequesAgregarMovimiento(chequeMovimiento, bd, tran))
        //                {
        //                    resultado = false;
        //                    AyudaProgramacionLN.MapearError(chequeMovimiento, pTesoreria);
        //                    break;
        //                }
        //            }

        //            if (pTipoOperacion == EnumTGETiposOperaciones.RecibirChequesCajas
        //                && !this.AgregarMovimientoTransferirCheques(listaCheques, pTesoreria, bd, tran))
        //            {
        //                resultado = false;
        //            }

        //            if (resultado)
        //            {
        //                tran.Commit();
        //                pTesoreria.CodigoMensaje = "ResultadoTransaccion";
        //            }
        //            else
        //                tran.Rollback();

        //        }
        //        catch (Exception ex)
        //        {
        //            ExceptionHandler.HandleException(ex, "LogicaNegocio");
        //            tran.Rollback();
        //            pTesoreria.CodigoMensaje = "ResultadoTransaccionIncorrecto";
        //            pTesoreria.CodigoMensajeArgs.Add(ex.Message);
        //            return false;
        //        }
        //    }

        //    return resultado;
        //}

        /// <summary>
        /// Agrega los movimientos en las cajas cuando se retiran los cheques
        /// </summary>
        /// <param name="pLista"></param>
        /// <param name="pTesoreria"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool AgregarMovimientoTransferirCheques(List<TESCheques> pLista, TESTesorerias pTesoreria, Database bd, DbTransaction tran)
        {
            TESCajasLN cajaLN = new TESCajasLN();
            List<TESCajas> listaCajas = new List<TESCajas>();
            TESCajas caja;
            TESCajasMovimientos cajaMovimiento;
            TESCajasMovimientosValores valor;
            foreach (TESCheques cheque in pLista)
            {
                caja = BaseDatos.ObtenerBaseDatos().Obtener<TESCajas>("TESCajasSeleccionarPorCheque", cheque, bd, tran);
                if (listaCajas.Exists(x => x.IdCaja == caja.IdCaja))
                {
                    caja = listaCajas.Find(x => x.IdCaja == caja.IdCaja);
                }
                else
                {
                    caja.CajasMonedas = cajaLN.ObtenerPorCaja(caja, bd, tran);
                    listaCajas.Add(caja);
                }
                caja.EstadoColeccion = EstadoColecciones.Modificado;

                cajaMovimiento = new TESCajasMovimientos();
                cajaMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
                cajaMovimiento.Estado.IdEstado = (int)EstadosCajasMovimientos.Activo;
                cajaMovimiento.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.RetiroCheques);
                cajaMovimiento.IdRefTipoOperacion = cheque.IdCheque;
                cajaMovimiento.Importe = cheque.Importe;

                valor = new TESCajasMovimientosValores();
                valor.EstadoColeccion = EstadoColecciones.Agregado;
                valor.TipoValor.IdTipoValor = (int)EnumTiposValores.ChequeTercero;
                valor.Importe = cheque.Importe;
                valor.Estado.IdEstado = (int)Estados.Activo;
                valor.UsuarioLogueado = cajaMovimiento.UsuarioLogueado;
                cajaMovimiento.CajasMovimientosValores.Add(valor);

                caja.CajasMonedas.Find(x => x.Moneda.IdMoneda == (int)EnumTGEMonedas.PesosArgentinos).CajasMovimientos.Add(cajaMovimiento);

            }

            foreach (TESCajas item in listaCajas)
            {
                if (item.Estado.IdEstado == (int)EstadosCajas.Cerrada)
                {
                    pTesoreria.CodigoMensaje = "ValidarCajaRetiroCheque";
                    pTesoreria.CodigoMensajeArgs.Add(item.NumeroCaja.ToString());
                    return false;
                }

                if (!cajaLN.ActualizarMonedasMovimientos(item, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(item, pTesoreria);
                    return false;
                }
            }

            return true;
        }

        public bool AgregarMovimientoTransferenciaBancos(TESTesoreriasMovimientos pParametro)
        {
            bool resultado = true;
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            TESBancosCuentasMovimientos movReferencia = new TESBancosCuentasMovimientos();
            if (resultado && pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaBancosCredito)
            {
                movReferencia.IdBancoCuentaMovimiento = pParametro.IdRefTipoOperacion;
                movReferencia = BancosF.BancosCuentasMovimientosObtenerDatosCompletos(movReferencia);
            }

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(pParametro, bd, tran, "TESTesoreriasMovimientosInsertarTransferenciaBancos");

                    //if (resultado && pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaBancosCredito)
                    //{
                    //    if (!BancosF.BancosCuentasMovimientoTrasnferenciaTesoreriaActualizar(movReferencia, bd, tran))
                    //    {
                    //        AyudaProgramacionLN.MapearError(movReferencia, pParametro);
                    //        resultado = false;
                    //    }
                    //}

                    if(pParametro.Archivos.Count > 0)
                    {
                        TESTesoreriasMovimientos aux = new TESTesoreriasMovimientos();
                        aux = BaseDatos.ObtenerBaseDatos().Obtener<TESTesoreriasMovimientos>("TESTesoreriasMovimientosObtenerUltimoId",pParametro,bd,tran);
                        if(aux.IdTesoreriaMovimiento > 0)
                        {
                            pParametro.IdTesoreriaMovimiento = aux.IdTesoreriaMovimiento;
                            if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                                resultado = false;

                            }
                    }
                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                        tran.Rollback();

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

        public bool AnularTraspaso(TESTesoreriasMovimientos pParametro)
        {
            bool resultado = true;
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();
            //idref --> envia el de idchequemov, me falta el de teso.
            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(pParametro, bd, tran, "TESTesoreriasMovimientosAnularTraspasoCheques");
                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                        tran.Rollback();

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

        #region Modificaciones FechaAbrir por FechaAbrirEvento
        /// <summary>
        /// Devuelve una Tesoreria por Filial y Fecha
        /// </summary>
        /// <param name="pParametro">IdFilial, FechaAbrir</param>
        /// <returns></returns>
        public TESTesorerias ObtenerPorFilialFechaEvento(TESTesorerias pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<TESTesorerias>("TESTesoreriasSeleccionarPorFilialFechaAbrirEvento", pParametro);
        }

        /// <summary>
        /// Valida si la Tesoreria esta cerrada para el día de la fecha Evento
        /// </summary>
        /// <param name="pParametros">IdFilial</param>
        /// <returns></returns>
        public bool ValidarCerradaEvento(TESTesorerias pParametros)
        {
            pParametros.FechaAbrirEvento = DateTime.Now;
            TESTesorerias teso = this.ObtenerPorFilialFecha(pParametros);

            if (teso.Estado.IdEstado == (int)EstadosTesorerias.Cerrada)
            {
                pParametros.CodigoMensaje = "TesoreriaValidarCerrada";
                return false;
            }

            pParametros.IdTesoreria = teso.IdTesoreria;
            return true;
        }

        /// <summary>
        /// Valida si la Tesoreria esta abierta para un día Anterior
        /// </summary>
        /// <param name="pParametros">IdFilial</param>
        /// <returns></returns>
        public bool ValidarAbiertaFechaEventoAnterior(TESTesorerias pParametro)
        {
            TESTesorerias teso = BaseDatos.ObtenerBaseDatos().Obtener<TESTesorerias>("TESTesoreriasSeleccionarAbiertas", pParametro);

            if (teso.IdTesoreria > 0 && teso.FechaAbrirEvento.Date < DateTime.Now.Date)
            {
                pParametro.CodigoMensaje = "ValidarAbiertaFechaAnterior";
                pParametro.CodigoMensajeArgs.Add(teso.FechaAbrirEvento.Date.ToString());
                return false;
            }
            return true;
        }

        public TESTesorerias ObtenerUltimoCierreFilialUsuarioEvento(TESTesorerias pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<TESTesorerias>("TESTesoreriasSeleccionarUltimoCierre", pParametro);
        }

        public TESTesorerias ObtenerUltimoCierreFilialUsuarioEvento(TESTesorerias pParametro, Database bd, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<TESTesorerias>("TESTesoreriasSeleccionarUltimoCierre", pParametro, bd, tran);
        }
        #endregion

        public TESTesoreriasMovimientos MovimientosObtenerDatosCompletos(TESTesoreriasMovimientos pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<TESTesoreriasMovimientos>("TESTesoreriasMovimientosSeleccionar", pParametro);
            pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return pParametro;
        }
    }
}
