using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesorerias.Entidades;
using Comunes;
using Servicio.AccesoDatos;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Generales.Entidades;
using Ahorros;
using Ahorros.Entidades;
using Generales.FachadaNegocio;
using Prestamos.Entidades;
using Prestamos;
using Comunes.LogicaNegocio;
using Cobros.Entidades;
using Cobros;
using Bancos.Entidades;
using Bancos;
using Cargos;
using Cargos.Entidades;
using System.Reflection;
using CuentasPagar.FachadaNegocio;
using CuentasPagar.Entidades;
using Contabilidad.Entidades;
using System.Data;

namespace Tesorerias.LogicaNegocio
{
    class TESCajasLN : BaseLN<TESCajas>
    {
        /// <summary>
        /// Devuelve los Totales por Moneda y Tipo de Valor para una Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public DataTable CajasMonedasSeleccionarTotalesTipoValorPorCaja(TESCajas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("TESCajasMonedasSeleccionarTotalesTipoValorPorCaja", pParametro);
        }

        /// <summary>
        /// Si es la primera ves genera la apertura de la Caja y
        /// Obtiene los datos de la Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool AbrirObtenerDatosCaja(TESCajas pParametro)
        {
            if (!DateTime.Equals(pParametro.FechaAbrirEvento.Date, DateTime.Now.Date))
            {
                pParametro.CodigoMensaje = "ValidarFecha";
                return false;
            }
            
            TESCajas caja = this.ObtenerCajaAbierta(pParametro);
            if (caja.IdCaja == 0 || (caja.IdCaja > 0 && caja.Estado.IdEstado == (int)EstadosCajas.Cerrada))
            {

                TESTesorerias teso = new TESTesorerias();
                teso.Filial = pParametro.Usuario.FilialPredeterminada;
                teso.FechaAbrir = pParametro.FechaAbrir;
                teso = new TESTesoreriasLN().ObtenerPorFilialFecha(teso);
                
                pParametro.Tesoreria.IdTesoreria = teso.IdTesoreria;
                pParametro.FechaAbrirEvento = DateTime.Now;
                
                pParametro.Estado.IdEstado = (int)EstadosCajas.Abierta;
                
                if (!this.Agregar(pParametro))
                    return false;
            }
            else if (caja.Usuario.SectorPredeterminado.IdSector != pParametro.Usuario.SectorPredeterminado.IdSector)
            {
                pParametro.CodigoMensaje = "CajaValidarAbiertaUsuarioSector";
                return false;
            }
            
            pParametro.IdCaja = caja.IdCaja;
            return true;
        }

        /// <summary>
        /// Valida si la Caja esta abierta para el usuario
        /// </summary>
        /// <param name="pParametros">IdTesoreria</param>
        /// <returns></returns>
        public bool ValidarAbierta(TESCajas pParametros)
        {
            //pParametros.FechaAbrir = DateTime.Now;
            TESCajas caja = this.ObtenerCajaAbierta(pParametros);

            if (caja.IdCaja == 0)
            {
                pParametros.CodigoMensaje = "CajaValidarAbierta";
                return false;
            }
            //else if (caja.IdCaja > 0 && caja.Estado.IdEstado == (int)EstadosCajas.Cerrada)
            //{
            //    pParametros.CodigoMensaje = "CajaValidarCerrada";
            //    return false;
            //}
            //else if (caja.Usuario.SectorPredeterminado.IdSector != pParametros.Usuario.SectorPredeterminado.IdSector)
            //{
            //    pParametros.CodigoMensaje = "CajaValidarAbiertaUsuarioSector";
            //    return false;
            //}
            else
            {
                pParametros.IdCaja = caja.IdCaja;
            }

            return true;
        }

        public bool ValidarCerradaMismaTesoreria(TESCajas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "TesCajasValidarCerradaMismaTesoreria");
        }

        /// <summary>
        /// Valida si existen Cajas Abiertas para una Tesoreria
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool ValidarAbiertas(TESTesorerias pParametro)
        {
            TESCajas cajaFiltro = new TESCajas();
            cajaFiltro.Tesoreria.IdTesoreria = pParametro.IdTesoreria;
            cajaFiltro.Estado.IdEstado = (int)EstadosCajas.Abierta;
            List<TESCajas> cajasAbiertas = this.ObtenerListaFiltro(cajaFiltro);

            if (cajasAbiertas.Count > 0)
            {
                pParametro.CodigoMensaje = "TesoreriaValidarCajasAbiertas";
                return false;
            }
            return true;
        }

        public bool ValidarNumeroCaja(TESCajas pParametro)
        {
            TESCajas filtro = new TESCajas();
            filtro.Usuario.FilialPredeterminada.IdFilial = pParametro.Usuario.FilialPredeterminada.IdFilial;
            //filtro.Tesoreria.Filial.IdFilial = pParametro.Tesoreria.Filial.IdFilial;
            filtro.FechaAbrir = pParametro.FechaAbrir;
            filtro.NumeroCaja = pParametro.NumeroCaja;
            filtro = this.ObtenerCajaAbierta(filtro);
            //VALIDA QUE EL USUARIO NO TENGA CAJAS ABIERTAS PARA LA FECHA
            if (filtro.IdCaja > 0 && filtro.Estado.IdEstado == (int)EstadosCajas.Abierta)
            {
                pParametro.CodigoMensaje = "ValidarNumeroCaja";
                pParametro.CodigoMensajeArgs.Add(pParametro.NumeroCaja.ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// Obtengo la Caja Abierta por Filial, Fecha y Usuario (solo puede haber una abierta)
        /// </summary>
        /// <param name="pParametro">IdFilial, FechaAbrir, [IdUsuarioEvento], [NumeroCaja]</param>
        /// <returns></returns>
        private TESCajas ObtenerCajaAbierta(TESCajas pParametro)
        {
            TESCajas resultado = new TESCajas();
            List<TESCajas> cajas = BaseDatos.ObtenerBaseDatos().ObtenerLista<TESCajas>("[TESCajasSeleccionarPorUsuarioFechaAbrir]", pParametro);
            if (cajas.Count > 0)
                resultado = cajas[cajas.Count - 1];

            return resultado;
        }

        
        /// <summary>
        /// Obtiene los datos completos de la Caja con los tipos de Monedas Asignados y Movimientos Confirmados
        /// </summary>
        /// <param name="pParametro">FechaAbrir, IdFilial, IdUsuarioEvento</param>
        /// <returns></returns>
        public override TESCajas ObtenerDatosCompletos(TESCajas pParametro)
        {
            TESCajas caja = this.ObtenerCajaAbierta(pParametro);
            caja.CajasMonedas = this.ObtenerPorCaja(caja);

            foreach (TESCajasMonedas moneda in caja.CajasMonedas)
            {
                moneda.CajasMovimientos = BaseDatos.ObtenerBaseDatos().ObtenerLista<TESCajasMovimientos>("TESCajasMovimientosSeleccionarPorCajasMoneda", moneda);
            }
            return caja;
        }

        /// <summary>
        /// Obtiene los datos completos de la Caja con los tipos de Monedas Asignados
        /// </summary>
        /// <param name="pParametro">IdCaja</param>
        /// <returns></returns>
        public TESCajas ObtenerDatosMonedas(TESCajas pParametro)
        {
            pParametro.CajasMonedas = this.ObtenerPorCaja(pParametro);
            foreach (TESCajasMonedas moneda in pParametro.CajasMonedas)
            {
                moneda.CajasMovimientos = BaseDatos.ObtenerBaseDatos().ObtenerLista<TESCajasMovimientos>("TESCajasMovimientosSeleccionarPorCajasMoneda", moneda);
            }
            return pParametro;
        }

        /// <summary>
        /// Devuelve una lista de cajas por Tesoreria
        /// </summary>
        /// <param name="pParametro">IdTesoreria, IdEstado</param>
        /// <returns></returns>
        public override List<TESCajas> ObtenerListaFiltro(TESCajas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESCajas>("TESCajasSeleccionarFiltro", pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Cajas Abiertas para una Tesoreria
        /// con la lista de Cajas Monedas
        /// </summary>
        /// <param name="pParametro">IdTesoreria</param>
        /// <returns></returns>
        public List<TESCajas> ObtenerAbiertas(TESCajas pParametro)
        {
            pParametro.Estado.IdEstado = (int)EstadosCajas.Abierta;
            List<TESCajas> cajas = this.ObtenerListaFiltro(pParametro);
            foreach (TESCajas caja in cajas)
                caja.CajasMonedas = this.ObtenerPorCaja(caja);

            return cajas;
        }

        private List<TESCajasMonedas> ObtenerPorCaja(TESCajas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESCajasMonedas>("TESCajasMonedasSeleccionarPorCaja", pParametro);
        }

        public List<TESCajasMonedas> ObtenerPorCaja(TESCajas pParametro, Database bd, DbTransaction tran)
        {
            pParametro.CajasMonedas = BaseDatos.ObtenerBaseDatos().ObtenerLista<TESCajasMonedas>("TESCajasMonedasSeleccionarPorCaja", pParametro, bd, tran);
            foreach (TESCajasMonedas moneda in pParametro.CajasMonedas)
            {
                moneda.CajasMovimientos = BaseDatos.ObtenerBaseDatos().ObtenerLista<TESCajasMovimientos>("TESCajasMovimientosSeleccionarPorCajasMoneda", moneda, bd, tran);
            }
            return pParametro.CajasMonedas;
        }

        /// <summary>
        /// Devuelve una lista de Cajas Abiertas
        /// </summary>
        /// <param name="pParametro">IdTesoreria, IdEstado</param>
        /// <returns></returns>
        public List<TESCajas> ObtenerTodas(TESCajas pParametro)
        {
            pParametro.Estado.IdEstado = (int)EstadosCajas.Todas;
            return this.ObtenerListaFiltro(pParametro);
        }

        /// <summary>
        /// Devuelve los movimientos pendientes de confirmacion para las cajas
        /// </summary>
        /// <param name="pParametro">IdTesoreria</param>
        /// <returns></returns>


        public DataTable ObtenerPendientesConfirmacion(TESTesorerias pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("TESCajasMovimientosAConfirmarPorFilial", pParametro);
        }

        /// <summary>
        /// Devuelve un movimiento pendiente, si se completa el filtro
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public TESCajasMovimientos ObtenerPendienteConfirmacionFiltroMovimiento(TESFiltroMovimientosPendientes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<TESCajasMovimientos>("TESCajasMovimientosAConfirmarPorFilial", pParametro); 
        }

        /// <summary>
        /// Devuevle el objeto de referencia para Confirmar la Operacion en Caja
        /// o Generar el Comprobante de Impresión
        /// </summary>
        /// <param name="pParametro">IdRefTipoOperacion, IdTipoOperacion</param>
        /// <returns></returns>
        public Objeto ObtenerMovimientoPendienteConfirmacion(TESCajasMovimientos pParametro)
        {
            Objeto resultado = new Objeto();
            switch (pParametro.TipoOperacion.IdTipoOperacion)
            {
                #region Modulo de Ahorros
                //case (int)EnumTGETiposOperaciones.DepositoEfectivo:
                //case (int)EnumTGETiposOperaciones.ExtraccionEfectivo:
                case (int)EnumTGETiposOperaciones.AhorroDepositos:
                case (int)EnumTGETiposOperaciones.AhorroExtracciones:
                case (int)EnumTGETiposOperaciones.AhorroExtraccionesExterior:
                case (int)EnumTGETiposOperaciones.AhorroExtraccionHaberes:
                case (int)EnumTGETiposOperaciones.DepositoCobroJudicial:
                case (int)EnumTGETiposOperaciones.ExtraccionCobroJudicial:
                    AhoCuentasMovimientos movimiento = new AhoCuentasMovimientos();
                    movimiento.IdCuentaMovimiento = pParametro.IdRefTipoOperacion;
                    movimiento.UsuarioLogueado = pParametro.UsuarioLogueado;
                    resultado = AhorroF.MovimientosObtenerDatosCompletos(movimiento);
                    break;
                case (int)EnumTGETiposOperaciones.ExtraccionCheque:
                    break;
                case (int)EnumTGETiposOperaciones.AcreditacionCheque:
                    break;

                case (int)EnumTGETiposOperaciones.DepositoPlazoFijo:
                case (int)EnumTGETiposOperaciones.ExtraccionPlazoFijo:
                case (int)EnumTGETiposOperaciones.CancelacionAnticipadaPlazoFijo:
                case (int)EnumTGETiposOperaciones.RenovacionPlazosFijos:
                case (int)EnumTGETiposOperaciones.RenovacionPlazosFijosDepositos:
                case (int)EnumTGETiposOperaciones.RenovacionPlazosFijosExtraccion:
                    AhoPlazosFijos plazoFijo = new AhoPlazosFijos();
                    plazoFijo.IdPlazoFijo = pParametro.IdRefTipoOperacion;
                    plazoFijo.UsuarioLogueado = pParametro.UsuarioLogueado;
                    resultado = AhorroF.PlazosFijosObtenerDatosCompletos(plazoFijo);
                    break;
                #endregion

                #region Modulo Cuentas a Pagar
                case (int)EnumTGETiposOperaciones.OrdenesPagos:
                case (int)EnumTGETiposOperaciones.OrdenesPagosInterno:
                    CapOrdenesPagos ordenPago = new CapOrdenesPagos();
                    ordenPago.IdOrdenPago = pParametro.IdRefTipoOperacion;
                    ordenPago.UsuarioLogueado = pParametro.UsuarioLogueado;
                    resultado = CuentasPagarF.OrdenesPagosObtenerDatosCompletos(ordenPago);
                    break;
                #endregion

                #region Modulo de Cobros
                case (int)EnumTGETiposOperaciones.OrdenesCobrosAfiliados:
                case (int)EnumTGETiposOperaciones.CancelacionAnticipadaCuotaPrestamo:
                case (int)EnumTGETiposOperaciones.OrdenesCobrosVarios:
                case (int)EnumTGETiposOperaciones.OrdenesCobrosFacturasAdelantos:
                    CobOrdenesCobros ordenCobro = new CobOrdenesCobros();
                    ordenCobro.IdOrdenCobro = pParametro.IdRefTipoOperacion;
                    ordenCobro.UsuarioLogueado = pParametro.UsuarioLogueado;
                    resultado = CobrosF.OrdenesCobrosObtenerDatosCompletos(ordenCobro);
                    break;

                case (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas:
                case (int)EnumTGETiposOperaciones.OrdenesCobrosFacturasInternas:
                    CobOrdenesCobros ordenCobroFactura = new CobOrdenesCobros();
                    ordenCobroFactura.IdOrdenCobro = pParametro.IdRefTipoOperacion;
                    ordenCobroFactura.UsuarioLogueado = pParametro.UsuarioLogueado;
                    resultado = CobrosF.OrdenesCobrosObtenerFacturasDatosCompletos(ordenCobroFactura);
                    break;
                #endregion

                #region Modulo de Prestamos
                case (int)EnumTGETiposOperaciones.PrestamosLargoPlazo:
                case (int)EnumTGETiposOperaciones.PrestamosCortoPlazo:
                case (int)EnumTGETiposOperaciones.PrestamosLargoPlazoCancelacion:
                case (int)EnumTGETiposOperaciones.PrestamosCortoPlazoCancelacion:
                case (int)EnumTGETiposOperaciones.PrestamosBancoDelSol:
                case (int)EnumTGETiposOperaciones.PrestamosBancoDelSolCancelacion:
                case (int)EnumTGETiposOperaciones.PrestamosFondosPropios:
                case (int)EnumTGETiposOperaciones.PrestamosFondosPropiosCancelacion:
                case (int)EnumTGETiposOperaciones.CompraDeCheque:
                case (int)EnumTGETiposOperaciones.PrestamosManual:
                    PrePrestamos prestamo = new PrePrestamos();
                    prestamo.IdPrestamo = pParametro.IdRefTipoOperacion;
                    prestamo.UsuarioLogueado = pParametro.UsuarioLogueado;
                    resultado = PrePrestamosF.PrestamosObtenerDatosCompletos(prestamo);
                    break;
                #endregion

                #region Cajas Movimientos
                case (int)EnumTGETiposOperaciones.IngresosCajas:
                case (int)EnumTGETiposOperaciones.EgresosCajas:
                case (int)EnumTGETiposOperaciones.IngresosCajasInternos:
                case (int)EnumTGETiposOperaciones.EgresosCajasInternos:
                    resultado = pParametro;
                    break;
                #endregion
                default:
                    resultado = new TESCajasMovimientosLN().ObtenerMovmientoAConfirmarPorIdRefTipoOperacion(pParametro);
                    break;
            }

            return resultado;
        }

        /// <summary>
        /// Genera la Apertura de la Caja para el día de la fecha
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public override bool Agregar(TESCajas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.FechaAbrirEvento = DateTime.Now; //Reemplazado FechaAbrir por  FechaAbrirEvento
            pParametro.Usuario.IdUsuario = pParametro.UsuarioLogueado.IdUsuarioEvento;
            pParametro.Estado.IdEstado = (int)EstadosCajas.Abierta;

            if (!this.ValidarNumeroCaja(pParametro))
                return false;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    //Se comenta y se deja ingresar el numero de caja por pantalla
                    //pParametro.NumeroCaja = this.ObtenerNumeroCaja(pParametro, bd, tran);

                    pParametro.IdCaja = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "TESCajasInsertar");
                    if (pParametro.IdCaja == 0)
                        resultado = false;

                    if (resultado && !this.AgregarMonedas(pParametro, bd, tran))
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
        /// Genera las altas de Cajas Monedas
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        private bool AgregarMonedas(TESCajas pParametro, Database bd, DbTransaction tran)
        {
            List<TGEMonedas> monedas = TGEGeneralesF.MonedasObtenerListaActiva();
            TESCajasMonedas cajaMoneda;
            foreach (TGEMonedas mon in monedas)
            {
                cajaMoneda = new TESCajasMonedas();
                cajaMoneda.EstadoColeccion = EstadoColecciones.Agregado;
                cajaMoneda.Estado.IdEstado = (int)EstadosCajasMonedas.Activo;
                cajaMoneda.Caja.IdCaja = pParametro.IdCaja;
                cajaMoneda.Moneda = mon;
                cajaMoneda.UsuarioLogueado = pParametro.UsuarioLogueado;
                //SI LA MONEDA FUE LEVANTADA PARA OBTENER SALDO ANTERIOR, MAPEO
                if (pParametro.CajasMonedas.Exists(x => x.Moneda.IdMoneda == cajaMoneda.Moneda.IdMoneda))
                {
                    cajaMoneda.SaldoInicial = pParametro.CajasMonedas.Find(x => x.Moneda.IdMoneda == cajaMoneda.Moneda.IdMoneda).SaldoFinal;
                    //INICIALIZO LAS MONEDAS CON EL SALDO INICIAL IGUAL AL FINAL
                    cajaMoneda.SaldoFinal = cajaMoneda.SaldoInicial;
                    //UNA VEZ CARGADO EL SALDO INICIAL DE LA NUEVA CAJA MONEDA, BORRO LA VIEJA.
                    pParametro.CajasMonedas.RemoveAt(pParametro.CajasMonedas.FindIndex(x => x.Moneda.IdMoneda == cajaMoneda.Moneda.IdMoneda));
                }
                //DESPUES DE ESTO EN EL AGREGAR SE REALIZA 

                cajaMoneda.IdCajaMoneda = BaseDatos.ObtenerBaseDatos().Agregar(cajaMoneda, bd, tran, "TESCajasMonedasInsertar");
                if (cajaMoneda.IdCajaMoneda == 0)
                {
                    AyudaProgramacionLN.MapearError(cajaMoneda, pParametro);
                    return false;
                }
            }



            pParametro.CajasMonedas = this.ObtenerPorCaja(pParametro, bd, tran);
            return true;
        }

        /// <summary>
        /// Genera el Cierre de la Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public override bool Modificar(TESCajas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = this.Modificar(pParametro, bd, tran);
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
        /// Abre la Caja de un usuario nuevamente
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool ModificarReabrirCaja(TESCajas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TesCajasActualizarAbrirCaja");
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
        /// Genera el Cierre de la Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool Modificar(TESCajas pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            pParametro.FechaCerrar = DateTime.Now;
            pParametro.Estado.IdEstado = (int)EstadosCajas.Cerrada;

            //if (pParametro.Usuario.SectorPredeterminado.IdSector != (int)EnumTGESectores.Bancos)
            //{
                pParametro.Tesoreria.UsuarioLogueado.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;
                TESTesorerias teso = new TESTesoreriasLN().ObtenerDatosCompletos(pParametro.Tesoreria, bd, tran);
                
                TESTesoreriasMonedas tesoMonedas;
                TESTesoreriasMovimientos tesoMovimiento;
                TESCajasMovimientos cajaMovimiento;
                TESCajasMovimientosValores cajaMovValor;
                
                    foreach (TESCajasMonedas cajaMoneda in pParametro.CajasMonedas)
                    {
                        cajaMoneda.CajasMovimientos.RemoveAll(x => x.EstadoColeccion == EstadoColecciones.Agregado);
                        //Actualizao Caja Moneda
                        //cajaMoneda.SaldoFinal = 0; // cajaMoneda.SaldoInicial + cajaMoneda.Ingreso - cajaMoneda.Egreso;
                        cajaMoneda.EstadoColeccion = EstadoColecciones.Modificado;
                        cajaMoneda.UsuarioLogueado = pParametro.UsuarioLogueado;

                        #region Movimiento Tesoreria / Caja Efectivo

                        if (pParametro.TraspasarFondos)
                        {
                            //Genero el Movimiento de Tesoreria EFECTIVO
                            tesoMovimiento = new TESTesoreriasMovimientos();
                            tesoMovimiento.Importe = this.ObtenerImporteEfectivo(cajaMoneda, bd, tran);
                            if (tesoMovimiento.Importe > 0)
                            {
                                tesoMovimiento.TesoreriaMoneda.Moneda.IdMoneda = cajaMoneda.Moneda.IdMoneda;
                                tesoMovimiento.Estado.IdEstado = (int)EstadosTesoreriasMovimientos.Activo;
                                tesoMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
                                tesoMovimiento.Fecha = pParametro.FechaAbrir;
                                tesoMovimiento.Importe = this.ObtenerImporteEfectivo(cajaMoneda, bd, tran);
                                tesoMovimiento.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.DevolucionEfectivoATesoreria;
                                tesoMovimiento.IdRefTipoOperacion = pParametro.IdCaja;
                                tesoMovimiento.TipoOperacion.TipoMovimiento.IdTipoMovimiento = (int)EnumTGETiposMovimientos.Credito;
                                tesoMovimiento.TipoValor.IdTipoValor = (int)EnumTiposValores.Efectivo;
                                tesoMovimiento.UsuarioLogueado = pParametro.UsuarioLogueado;
                                tesoMovimiento.Descripcion = pParametro.Usuario.ApellidoNombre;
                                tesoMonedas = teso.TesoreriasMonedas.Find(x => x.Moneda.IdMoneda == cajaMoneda.Moneda.IdMoneda && x.TipoValor.IdTipoValor == tesoMovimiento.TipoValor.IdTipoValor);
                                tesoMonedas.TesoreriasMovimientos.Add(tesoMovimiento);

                                //Genero el Movimiento de Salida de Efectivo de Caja
                                cajaMovimiento = new TESCajasMovimientos();
                                //cajaMovimiento.CajaMoneda.Moneda = tesoreriaMoneda.Moneda;
                                cajaMovimiento.Importe = tesoMovimiento.Importe;
                                cajaMovimiento.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.TraspasoEfectivoCajaATesoreria, bd, tran);
                                cajaMovimiento.Descripcion = cajaMovimiento.TipoOperacion.TipoOperacion;
                                cajaMovimiento.IdRefTipoOperacion = tesoMovimiento.IdTesoreriaMovimiento;
                                //cajaMovimiento.TipoValor = movimiento.TipoValor;
                                cajaMovimiento.Fecha = pParametro.FechaAbrir;
                                cajaMovimiento.Estado.IdEstado = (int)EstadosCajasMovimientos.Activo;
                                cajaMovimiento.UsuarioLogueado = pParametro.UsuarioLogueado;
                                cajaMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
                                cajaMoneda.CajasMovimientos.Add(cajaMovimiento);

                                cajaMovValor = new TESCajasMovimientosValores();
                                cajaMovValor.Importe = cajaMovimiento.Importe;
                                cajaMovValor.EstadoColeccion = EstadoColecciones.Agregado;
                                cajaMovValor.TipoValor.IdTipoValor = (int)EnumTiposValores.Efectivo;
                                cajaMovimiento.CajasMovimientosValores.Add(cajaMovValor);
                            }

                            //Genero el Movimiento de Tesoreria CHEQUES
                            List<TESCheques> lista = BaseDatos.ObtenerBaseDatos().ObtenerLista<TESCheques>("TESChequesSeleccionarPorCaja", pParametro, bd, tran);
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
                                tesoMonedas = teso.TesoreriasMonedas.Find(x => x.Moneda.IdMoneda == cajaMoneda.Moneda.IdMoneda && x.TipoValor.IdTipoValor == tesoMovimiento.TipoValor.IdTipoValor);
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
                        }
                        #endregion
                    }

                teso.UsuarioLogueado = pParametro.UsuarioLogueado;
                    //Devuelve los Saldos a la Tesoreria
                    if (resultado && !new TESTesoreriasLN().ActualizarMonedasMovimientos(teso, bd, tran))
                        resultado = false;
            //}

            if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TESCajasActualizar"))
                resultado = false;

            //Actualizao los saldos finales de Cajas Monedas
            if (resultado && !this.ActualizarMonedasMovimientos(pParametro, bd, tran))
                resultado = false;

            ////Actualizao los Cheques y los paso a Tesoreria
            //if (resultado
            //    && pParametro.Usuario.SectorPredeterminado.IdSector != (int)EnumTGESectores.Bancos
            //    && !this.ActualizarChequesCierreCaja(pParametro, bd, tran))
            //    resultado = false;

            return resultado;
        }

        /// <summary>
        /// Confirmar un movimiento de Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool ConfirmarMovimiento(TESCajas pParametro, TESCajasMovimientos pCajaMovimiento, Objeto pRefTipoOperacion)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            AyudaProgramacionLN.LimpiarMensajesError(pCajaMovimiento);
            AyudaProgramacionLN.LimpiarMensajesError(pRefTipoOperacion);

            bool resultado = true;            
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            pCajaMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
            pCajaMovimiento.Estado.IdEstado = (int)EstadosCajasMovimientos.Activo;
            pCajaMovimiento.Fecha = DateTime.Now;

            pRefTipoOperacion.EstadoColeccion = EstadoColecciones.Modificado;
            pParametro.CajasMonedas.Find(x => x.Moneda.IdMoneda == pCajaMovimiento.CajaMoneda.Moneda.IdMoneda).CajasMovimientos.RemoveAll(x => x.EstadoColeccion == EstadoColecciones.Agregado);

            pParametro.CajasMonedas.Find(x => x.Moneda.IdMoneda == pCajaMovimiento.CajaMoneda.Moneda.IdMoneda).CajasMovimientos.Add(pCajaMovimiento);

            #region "Renovacion de Plazo Fijo"
            AhoPlazosFijos plazoFijoAnterior;
            switch (pCajaMovimiento.TipoOperacion.IdTipoOperacion)
            {
                case (int)EnumTGETiposOperaciones.RenovacionPlazosFijos:
                    //Genero el Movimiento del Plazo Fijo Pagado
                    TESCajasMovimientos movimiento = new TESCajasMovimientos();
                    plazoFijoAnterior = new AhoPlazosFijos();
                    plazoFijoAnterior.IdPlazoFijo = ((AhoPlazosFijos)pRefTipoOperacion).IdPlazoFijoAnterior;
                    plazoFijoAnterior = AhorroF.PlazosFijosObtenerDatosCompletos(plazoFijoAnterior);

                    movimiento.UsuarioLogueado = pRefTipoOperacion.UsuarioLogueado;
                    movimiento.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.ExtraccionPlazoFijo;
                    movimiento.Fecha = DateTime.Now;
                    movimiento.IdRefTipoOperacion = plazoFijoAnterior.IdPlazoFijo;
                    movimiento.Importe = plazoFijoAnterior.ImporteTotal;
                    movimiento.Estado.IdEstado = (int)EstadosCajasMovimientos.Activo;
                    movimiento.EstadoColeccion = EstadoColecciones.Agregado;
                    movimiento.Afiliado.IdAfiliado = plazoFijoAnterior.IdAfiliado;
                    movimiento.CajaMoneda.Moneda.IdMoneda = plazoFijoAnterior.Moneda.IdMoneda;
                    pParametro.CajasMonedas.Find(x => x.Moneda.IdMoneda == pCajaMovimiento.CajaMoneda.Moneda.IdMoneda).CajasMovimientos.Add(movimiento);

                    TESCajasMovimientosValores valor = new TESCajasMovimientosValores();
                    valor.EstadoColeccion = EstadoColecciones.Agregado;
                    valor.TipoValor = plazoFijoAnterior.TipoValor;
                    valor.Importe = plazoFijoAnterior.ImporteTotal;
                    valor.Estado.IdEstado = (int)Estados.Activo;
                    valor.UsuarioLogueado = movimiento.UsuarioLogueado;
                    movimiento.CajasMovimientosValores.Add(valor);
                    valor.IndiceColeccion = movimiento.CajasMovimientosValores.IndexOf(valor);
                    break;
            }
            #endregion

            #region Ingreso de Cheques propios y terceros en CAJA - NO ACTUALIZA MOV Cheques y Cuenta de Bancos
            if (pCajaMovimiento.TipoOperacion.TipoMovimiento.IdTipoMovimiento == (int)EnumTGETiposMovimientos.Debito)
            {
                TESTesorerias teso = new TESTesorerias();
                TESTesoreriasMonedas tesoMonedas;
                TESTesoreriasMovimientos tesoMovimiento;
                List<TESCajasMovimientosValores> chequesVarios;
                if (pCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque || x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero))
                {
                    pParametro.Tesoreria.UsuarioLogueado.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;
                    teso = new TESTesoreriasLN().ObtenerDatosCompletos(pParametro.Tesoreria);
                }
                
                if (pCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque))// || x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero))
                {
                    chequesVarios = pCajaMovimiento.CajasMovimientosValores.Where(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque).ToList();

                    //INGRESO EN CAJA
                    TESCajasMovimientos movCheques = new TESCajasMovimientos();
                    movCheques.EstadoColeccion = EstadoColecciones.Agregado;
                    movCheques.Estado.IdEstado = (int)EstadosCajasMovimientos.Activo;
                    movCheques.Fecha = pParametro.FechaAbrir;
                    movCheques.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.TraspasoChequesACajas; // = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.TraspasoChequesACajas);
                    movCheques.IdRefTipoOperacion = pCajaMovimiento.IdCajaMovimiento; //tesoMovimiento.IdTesoreriaMovimiento;
                    movCheques.CajasMovimientosValores.AddRange(AyudaProgramacionLN.Clone<List<TESCajasMovimientosValores>>(chequesVarios));
                    movCheques.Importe = movCheques.CajasMovimientosValores.Sum(x => x.Importe);
                    pParametro.CajasMonedas.Find(x => x.Moneda.IdMoneda == pCajaMovimiento.CajaMoneda.Moneda.IdMoneda).CajasMovimientos.Add(movCheques);                    
                }
                if (pCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero))// || x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero))
                {
                    chequesVarios = pCajaMovimiento.CajasMovimientosValores.Where(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero).ToList();

                    //SALIDA DE TESORERIA - SOLO CHEQUES TERCEROS
                    tesoMovimiento = new TESTesoreriasMovimientos();
                    tesoMovimiento.TesoreriaMoneda.Moneda.IdMoneda = pCajaMovimiento.CajaMoneda.Moneda.IdMoneda;
                    tesoMovimiento.Estado.IdEstado = (int)EstadosTesoreriasMovimientos.Activo;
                    tesoMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
                    tesoMovimiento.Fecha = pParametro.FechaAbrir;
                    tesoMovimiento.Importe = chequesVarios.Sum(x => x.Importe);
                    tesoMovimiento.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.RetiroCheques;
                    tesoMovimiento.TipoOperacion.TipoMovimiento.IdTipoMovimiento = (int)EnumTGETiposMovimientos.Debito;
                    tesoMovimiento.TipoValor.IdTipoValor = (int)EnumTiposValores.ChequeTercero;
                    tesoMovimiento.UsuarioLogueado = pParametro.UsuarioLogueado;
                    tesoMonedas = teso.TesoreriasMonedas.Find(x => x.Moneda.IdMoneda == pCajaMovimiento.CajaMoneda.Moneda.IdMoneda && x.TipoValor.IdTipoValor == tesoMovimiento.TipoValor.IdTipoValor);
                    tesoMonedas.TesoreriasMovimientos.Add(tesoMovimiento);
                    //INGRESO EN CAJA
                    TESCajasMovimientos movCheques = new TESCajasMovimientos();
                    movCheques.EstadoColeccion = EstadoColecciones.Agregado;
                    movCheques.Estado.IdEstado = (int)EstadosCajasMovimientos.Activo;
                    movCheques.Fecha = pParametro.FechaAbrir;
                    movCheques.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.TraspasoChequesACajas; // = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.TraspasoChequesACajas);
                    movCheques.IdRefTipoOperacion = tesoMovimiento.IdTesoreriaMovimiento;
                    movCheques.CajasMovimientosValores.AddRange(AyudaProgramacionLN.Clone<List<TESCajasMovimientosValores>>(chequesVarios));
                    movCheques.Importe = movCheques.CajasMovimientosValores.Sum(x => x.Importe);
                    pParametro.CajasMonedas.Find(x => x.Moneda.IdMoneda == pCajaMovimiento.CajaMoneda.Moneda.IdMoneda).CajasMovimientos.Add(movCheques);
                }
            }
            #endregion

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                if (!this.ValidarCajaMovimiento(pParametro, pCajaMovimiento, pRefTipoOperacion, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(pCajaMovimiento, pParametro);
                    tran.Rollback();
                    return false;
                }

                //Guardo Cajas Monedas y Cajas Movimientos
                if (resultado && !this.ActualizarMonedasMovimientos(pParametro, bd, tran))
                {
                    resultado = false;
                    tran.Rollback();
                    return false;
                }

                //Guardo los datos de Caja
                if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TESCajasActualizar"))
                {
                    resultado = false;
                    tran.Rollback();
                    return false;
                }

                try
                {
                    switch (pCajaMovimiento.TipoOperacion.IdTipoOperacion)
                    {
                        #region Modulo de Ahorros
                        //case (int)EnumTGETiposOperaciones.DepositoEfectivo:
                        //case (int)EnumTGETiposOperaciones.ExtraccionEfectivo:
                        case (int)EnumTGETiposOperaciones.AhorroDepositos:
                        case (int)EnumTGETiposOperaciones.AhorroExtracciones:
                        case (int)EnumTGETiposOperaciones.AhorroExtraccionesExterior:
                        case (int)EnumTGETiposOperaciones.AhorroExtraccionHaberes:
                        case (int)EnumTGETiposOperaciones.DepositoCobroJudicial:
                        case (int)EnumTGETiposOperaciones.ExtraccionCobroJudicial:
                            if (resultado && !AhorroF.MovimientosConfirmar(pRefTipoOperacion, pParametro.FechaAbrir, this.ObtenerValoresImporte(pCajaMovimiento), bd, tran))
                                resultado=false;
                            break;
                        case (int)EnumTGETiposOperaciones.ExtraccionCheque:
                            break;
                        case (int)EnumTGETiposOperaciones.AcreditacionCheque:
                            break;
                        case (int)EnumTGETiposOperaciones.DepositoPlazoFijo:
                        case (int)EnumTGETiposOperaciones.ExtraccionPlazoFijo:
                            if (resultado && !AhorroF.PlazosFijosConfirmar(pRefTipoOperacion, pParametro.FechaAbrir, this.ObtenerValoresImporte(pCajaMovimiento), bd, tran))
                                resultado = false;
                            break;
                        case (int)EnumTGETiposOperaciones.RenovacionPlazosFijos:
                            if (resultado && !AhorroF.PlazosFijosConfirmarRenovar(pRefTipoOperacion, pParametro.FechaAbrir, this.ObtenerValoresImporte(pCajaMovimiento), bd, tran))
                                resultado = false;
                            break;
                        case (int)EnumTGETiposOperaciones.CancelacionAnticipadaPlazoFijo:
                            if (resultado && !AhorroF.PlazosFijosConfirmarCancelar(pRefTipoOperacion, pParametro.FechaAbrir, this.ObtenerValoresImporte(pCajaMovimiento), bd, tran))
                                resultado = false;
                            break;
                        #endregion

                        #region Modulo Cuentas a Pagar
                        case (int)EnumTGETiposOperaciones.OrdenesPagos:
                        case (int)EnumTGETiposOperaciones.OrdenesPagosInterno:
                            if (resultado && !CuentasPagarF.OrdenesPagosConfirmarMovimiento(pRefTipoOperacion, pParametro.FechaAbrir, bd, tran))
                                resultado = false;
                            break;
                        #endregion

                        #region Modulo Cuenta Corriente
                        case (int)EnumTGETiposOperaciones.CuentaCorrienteExtraccion:
                        case (int)EnumTGETiposOperaciones.PagoHaberes:
                            if (resultado && !CargosF.CuentasCorrientesAgregar((CarCuentasCorrientes)pRefTipoOperacion, bd, tran))
                                resultado = false;
                            break;
                        #endregion

                        #region Modulo Ordenes de Cobro
                        case (int)EnumTGETiposOperaciones.OrdenesCobrosAfiliados:
                        case (int)EnumTGETiposOperaciones.CancelacionAnticipadaCuotaPrestamo:
                        case (int)EnumTGETiposOperaciones.OrdenesCobrosVarios:
                        case (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas:
                        case (int)EnumTGETiposOperaciones.OrdenesCobrosFacturasInternas:
                        case (int)EnumTGETiposOperaciones.OrdenesCobrosFacturasAdelantos:
                            if (resultado && !CobrosF.OrdenesCobrosConfirmar((CobOrdenesCobros)pRefTipoOperacion, pParametro.FechaAbrir, this.ObtenerValoresImporte(pCajaMovimiento), bd, tran))
                                resultado = false;
                            break;
                        #endregion

                        #region Modulo de Prestamos
                        case (int)EnumTGETiposOperaciones.PrestamosLargoPlazo:
                        case (int)EnumTGETiposOperaciones.PrestamosCortoPlazo:
                        case (int)EnumTGETiposOperaciones.PrestamosBancoDelSol:
                        case (int)EnumTGETiposOperaciones.PrestamosFondosPropios:
                        case (int)EnumTGETiposOperaciones.CompraDeCheque:
                        case (int)EnumTGETiposOperaciones.PrestamosManual:
                            if (resultado && !PrePrestamosF.PrestamosConfirmar(pRefTipoOperacion, pParametro.FechaAbrir, this.ObtenerValoresImporte(pCajaMovimiento), bd, tran))
                                resultado = false;
                            break;
                        case (int)EnumTGETiposOperaciones.PrestamosLargoPlazoCancelacion:
                        case (int)EnumTGETiposOperaciones.PrestamosCortoPlazoCancelacion:
                        case (int)EnumTGETiposOperaciones.PrestamosBancoDelSolCancelacion:
                        case (int)EnumTGETiposOperaciones.PrestamosFondosPropiosCancelacion:
                            if (!PrePrestamosF.PrestamosCancelarConfirmar(pRefTipoOperacion, pParametro.FechaAbrir, this.ObtenerValoresImporte(pCajaMovimiento), bd, tran))
                                resultado = false;
                            break;

                        #endregion

                        default:
                            break;
                    }
                    // Si da error al guardar el Objeto referencia Mapeo el Error
                    if (!resultado)
                        AyudaProgramacionLN.MapearError(pRefTipoOperacion, pParametro);

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

        /// <summary>
        /// Confirmar un movimiento de Caja MANUAL
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool ConfirmarMovimiento(TESCajas pParametro, TESCajasMovimientos pCajaMovimiento)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            AyudaProgramacionLN.LimpiarMensajesError(pCajaMovimiento);

            if (pCajaMovimiento.Importe == 0)
            {
                pParametro.CodigoMensaje = "ValidarImporteMayorCero";
                return false;
            }

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            pCajaMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
            pCajaMovimiento.Estado.IdEstado = (int)EstadosCajasMovimientos.Activo;
            //pCajaMovimiento.Fecha = pParametro.FechaAbrir;
            
            pParametro.CajasMonedas.Find(x => x.Moneda.IdMoneda == pCajaMovimiento.CajaMoneda.Moneda.IdMoneda).CajasMovimientos.RemoveAll(x => x.EstadoColeccion == EstadoColecciones.Agregado);
            pParametro.CajasMonedas.Find(x => x.Moneda.IdMoneda == pCajaMovimiento.CajaMoneda.Moneda.IdMoneda).CajasMovimientos.Add(pCajaMovimiento);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (!this.ValidarCajaMovimiento(pParametro, pCajaMovimiento, new Objeto(), bd, tran))
                    {
                        AyudaProgramacionLN.MapearError(pCajaMovimiento, pParametro);
                        tran.Rollback();
                        return false;
                    }

                    //Guardo Cajas Monedas y Cajas Movimientos
                    if (resultado && !this.ActualizarMonedasMovimientos(pParametro, bd, tran))
                        resultado = false;

                    foreach (TESCajasMovimientosConceptosContables item in pCajaMovimiento.CajasMovimientosConceptosContables)
                    {
                        if (item.EstadoColeccion == EstadoColecciones.Agregado)
                        {
                            item.UsuarioLogueado = pCajaMovimiento.UsuarioLogueado;
                            item.IdCajaMovimiento = pCajaMovimiento.IdCajaMovimiento;
                            item.IdCajaMovimientoConceptoContable = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "TESCajasMovimientosConceptosContablesInsertar");
                            if (item.IdCajaMovimientoConceptoContable == 0)
                            {
                                AyudaProgramacionLN.MapearError(item, pParametro);
                                resultado = false;
                                break;
                            }
                        }
                    }

                    //Guardo los datos de Caja
                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TESCajasActualizar"))
                        resultado = false;

                    if (resultado && !new InterfazContableLN().IngresosEgresosPorCajas(pParametro, pCajaMovimiento, bd, tran))
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
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        /// <summary>
        /// Genera el Alta de una Orden de Cobro desde la Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="pOrdenCobro"></param>
        /// <returns></returns>
        //public bool ConfirmarOrdenCobro(TESCajas pParametro, CobOrdenesCobros pOrdenCobro)
        //{
        //    AyudaProgramacionLN.LimpiarMensajesError(pParametro);
        //    AyudaProgramacionLN.LimpiarMensajesError(pOrdenCobro);

        //    bool resultado = true;
        //    pParametro.EstadoColeccion = EstadoColecciones.Agregado;

        //    DbTransaction tran;
        //    DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

        //    using (DbConnection con = bd.CreateConnection())
        //    {
        //        con.Open();
        //        tran = con.BeginTransaction();

        //        try
        //        {
        //            if (resultado && !CobrosF.OrdenesCobrosAgregar(pOrdenCobro, bd, tran))
        //            {
        //                AyudaProgramacionLN.MapearError(pOrdenCobro, pParametro);
        //                resultado = false;
        //            }

        //            TESCajasMonedas cajaMoneda = pParametro.CajasMonedas.Find(x => x.Moneda.IdMoneda == pOrdenCobro.Moneda.IdMoneda);
        //            TESCajasMovimientos cajaMovimiento;
        //            if (cajaMoneda.CajasMovimientos.Exists(x => x.EstadoColeccion == EstadoColecciones.Agregado && x.IdCajaMovimiento == 0))
        //            {
        //                cajaMovimiento = cajaMoneda.CajasMovimientos.Find(x => x.EstadoColeccion == EstadoColecciones.Agregado && x.IdCajaMovimiento == 0);
        //                cajaMoneda.CajasMovimientos.Remove(cajaMovimiento);
        //            }
                    
        //            cajaMovimiento = new TESCajasMovimientos();
        //            cajaMovimiento.EstadoColeccion = EstadoColecciones.Agregado;
        //            cajaMovimiento.Afiliado = pOrdenCobro.Afiliado;
        //            cajaMovimiento.Importe = pOrdenCobro.ImporteTotal;
        //            cajaMovimiento.IdRefTipoOperacion = pOrdenCobro.IdOrdenCobro;
        //            cajaMovimiento.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.OrdenesCobros;
        //            cajaMovimiento.UsuarioLogueado = pOrdenCobro.UsuarioLogueado;
        //            cajaMoneda.CajasMovimientos.Add(cajaMovimiento);

        //            TESCajasMovimientosValores cajaMovValor;
        //            //TESCheques cheque;
        //            foreach (CobOrdenesCobrosFormasCobros formaCobro in pOrdenCobro.OrdenesCobrosFormasCobros)
        //            {
        //                cajaMovValor = new TESCajasMovimientosValores();
        //                cajaMovValor.Estado.IdEstado = (int)Estados.Activo;
        //                cajaMovValor.EstadoColeccion = EstadoColecciones.Agregado;
        //                cajaMovValor.TipoValor = formaCobro.TipoValor;
        //                cajaMovValor.Importe = formaCobro.Importe;
        //                cajaMovValor.UsuarioLogueado = pOrdenCobro.UsuarioLogueado;
        //                cajaMovValor.Cheques = formaCobro.Cheques;
        //                cajaMovimiento.CajasMovimientosValores.Add(cajaMovValor);
        //            }

        //            if (!this.ActualizarMonedasMovimientos(pParametro, bd, tran))
        //                resultado = false;

        //            if (resultado)
        //            {
        //                tran.Commit();
        //                pParametro.CodigoMensaje = "ResultadoTransaccion";
        //            }
        //            else
        //            {
        //                tran.Rollback();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ExceptionHandler.HandleException(ex, "LogicaNegocio");
        //            tran.Rollback();
        //            pParametro.CodigoMensaje = ex.Message;
        //            return false;
        //        }
        //    }
        //    return resultado;
        //}

        private int ObtenerNumeroCaja(TESCajas pParametro, Database bd, DbTransaction tran)
        {
            TESCajas caja = BaseDatos.ObtenerBaseDatos().Obtener<TESCajas>("[TESCajasSeleccionarUltimoNumeroCaja]", pParametro, bd, tran);
            caja.NumeroCaja++;
            return caja.NumeroCaja;
        }

        /// <summary>
        /// Metodo para generar las Altas de Movimientos de Cajas
        /// Altas de Movimientos Valores
        /// Actualiza los saldos por moneda
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool ActualizarMonedasMovimientos(TESCajas pParametro, Database bd, DbTransaction tran)
        {
            foreach (TESCajasMonedas cajaMoneda in pParametro.CajasMonedas)
            {
                foreach (TESCajasMovimientos movimiento in cajaMoneda.CajasMovimientos)
                {
                    switch (movimiento.EstadoColeccion)
                    {
                        case EstadoColecciones.Agregado:
                            movimiento.CajaMoneda.IdCajaMoneda = cajaMoneda.IdCajaMoneda;
                            //movimiento.Fecha = pParametro.FechaAbrir;// DateTime.Now;
                            movimiento.UsuarioLogueado = pParametro.UsuarioLogueado;
                            movimiento.IdCajaMovimiento = BaseDatos.ObtenerBaseDatos().Agregar(movimiento, bd, tran, "TESCajasMovimientosInsertar");
                            if (movimiento.IdCajaMovimiento == 0)
                            {
                                AyudaProgramacionLN.MapearError(movimiento, pParametro);
                                return false;
                            }

                            if (!this.ActualizarMovimientosValores(movimiento, bd, tran))
                            {
                                AyudaProgramacionLN.MapearError(movimiento, pParametro);
                                return false;
                            }
                            cajaMoneda.EstadoColeccion = EstadoColecciones.Modificado;
                            break;
                        default:
                            break;
                    }
                }
                //Actualizo las Cajas Monedas
                cajaMoneda.Ingreso = cajaMoneda.CajasMovimientos.Where(x => x.TipoOperacion.TipoMovimiento.IdTipoMovimiento == (int)EnumTGETiposMovimientos.Credito).Sum(x => x.Importe);
                cajaMoneda.Egreso = cajaMoneda.CajasMovimientos.Where(x => x.TipoOperacion.TipoMovimiento.IdTipoMovimiento == (int)EnumTGETiposMovimientos.Debito).Sum(x => x.Importe);
                cajaMoneda.SaldoFinal = cajaMoneda.SaldoInicial + cajaMoneda.Ingreso - cajaMoneda.Egreso;
                if (cajaMoneda.EstadoColeccion == EstadoColecciones.Modificado)
                {
                    cajaMoneda.Caja.IdCaja = pParametro.IdCaja;
                    cajaMoneda.UsuarioLogueado = pParametro.UsuarioLogueado;
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(cajaMoneda, bd, tran, "TESCajasMonedasActualizar"))
                    {
                        AyudaProgramacionLN.MapearError(cajaMoneda, pParametro);
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Metodo para generar las Altas de Movimientos de Cajas Valores
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool ActualizarMovimientosValores(TESCajasMovimientos pParametro, Database bd, DbTransaction tran)
        {
            foreach (TESCajasMovimientosValores movValor in pParametro.CajasMovimientosValores)
            {
                switch (movValor.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        movValor.IdCajaMovimiento = pParametro.IdCajaMovimiento;
                        movValor.UsuarioLogueado = pParametro.UsuarioLogueado;
                        movValor.IdCajaMovimientoValor = BaseDatos.ObtenerBaseDatos().Agregar(movValor, bd, tran, "TESCajasMovimientosValoresInsertar");
                        if (movValor.IdCajaMovimientoValor == 0)
                        {
                            AyudaProgramacionLN.MapearError(movValor, pParametro);
                            return false;
                        }

                        //Si Tipo Operacion Traspaso Cheques a Caja No Actualizo Cheques ni Cuenta de Bancos
                        if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TraspasoChequesACajas)
                            break;

                        if (!this.ActualizarCheques(movValor, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(movValor, pParametro);
                            return false;
                        }
                        if (!this.ActualizarBancosCuentas(movValor, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(movValor, pParametro);
                            return false;
                        }
                        if(!this.ActualizarTarjetasTransacciones(movValor,bd,tran))
                        {
                            AyudaProgramacionLN.MapearError(movValor, pParametro);
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        
        private bool ActualizarTarjetasTransacciones(TESCajasMovimientosValores movValor, Database bd, DbTransaction tran)
        {
            //Guardo el detalle de la tarjeta ingresada
            foreach (TESTarjetasTransacciones tarjeta in movValor.TarjetasTransacciones)
            {
                tarjeta.IdCajaMovimientoValor = movValor.IdCajaMovimientoValor;
                tarjeta.UsuarioLogueado = movValor.UsuarioLogueado;
                if(!BancosF.TarjetasTransaccionesAgregar(tarjeta , bd, tran))
                {
                    AyudaProgramacionLN.MapearError(tarjeta, movValor);
                    return false;
                }
               
            }
            return true;
        }

        /// <summary>
        /// Metodo para generar las Altas de Movimientos de Cajas Valores
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool ActualizarCheques(TESCajasMovimientosValores pParametro, Database bd, DbTransaction tran)
        {
            TESBancosCuentasMovimientos movBco;
            TESChequesMovimientos movimiento;
            //Guardeo el detalle de los Cheques Ingresados
            foreach (TESCheques cheque in pParametro.Cheques)
            {
                //cheque.IdOrdenCobroFormaCobro = detalle.IdOrdenCobroFormaCobro;
                cheque.IdCajaMovimientoValor = pParametro.IdCajaMovimientoValor;
                //cheque.Estado.IdEstado = (int)EstadosCheques.IngresoCheque;
                cheque.UsuarioLogueado = pParametro.UsuarioLogueado;
                switch (cheque.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        if (!BancosF.ChequesAgregar(cheque, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(cheque, pParametro);
                            return false;
                        }
                        movimiento = new TESChequesMovimientos();
                        movimiento.EstadoColeccion = EstadoColecciones.Agregado;
                        movimiento.Fecha = DateTime.Now;
                        movimiento.Estado = cheque.Estado;
                        movimiento.Cheque = cheque;
                        movimiento.TipoOperacion = cheque.TipoOperacion;
                        //movimiento.IdRefChequeMovimiento = cheque.IdRefTipoOperacion;
                        movimiento.IdRefChequeMovimiento = pParametro.IdCajaMovimientoValor;
                        if (!BancosF.ChequesAgregarMovimiento(movimiento, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(movimiento, pParametro);
                            return false;
                        }
                        if (cheque.ChequePropio)
                        {
                            movBco = new TESBancosCuentasMovimientos();
                            movBco.EstadoColeccion = EstadoColecciones.Agregado;
                            movBco.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.PendienteConciliacion;
                            movBco.BancoCuenta.IdBancoCuenta = cheque.IdBancoCuenta;
                            movBco.TipoOperacion = cheque.TipoOperacion;
                            movBco.IdCajaMovimientoValor = pParametro.IdCajaMovimientoValor;
                            movBco.Importe = cheque.Importe;
                            movBco.FechaAlta = DateTime.Now;
                            movBco.FechaMovimiento = DateTime.Now;
                            movBco.IdRefTipoOperacion = cheque.IdCheque;
                            movBco.UsuarioAlta.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuario;
                            movBco.UsuarioLogueado = pParametro.UsuarioLogueado;

                            if (!BancosF.BancosCuentasMovimientosAgregar(movBco, bd, tran))
                            {
                                AyudaProgramacionLN.MapearError(movBco, pParametro);
                                return false;
                            }
                        }
                        break;
                    case EstadoColecciones.Modificado:
                        if (!cheque.ChequePropio)
                        {
                            cheque.IdCajaMovimientoValorEntregado = pParametro.IdCajaMovimientoValor;
                            if (!BancosF.ChequesModificar(cheque, bd, tran))
                            {
                                AyudaProgramacionLN.MapearError(cheque, pParametro);
                                return false;
                            }
                            movimiento = new TESChequesMovimientos();
                            movimiento.EstadoColeccion = EstadoColecciones.Agregado;
                            movimiento.Fecha = DateTime.Now;
                            movimiento.Estado = cheque.Estado;
                            movimiento.Cheque = cheque;
                            movimiento.TipoOperacion = cheque.TipoOperacion;
                            movimiento.IdRefChequeMovimiento = cheque.IdRefTipoOperacion;
                            movimiento.IdRefChequeMovimiento = pParametro.IdCajaMovimientoValor;
                            if (!BancosF.ChequesAgregarMovimiento(movimiento, bd, tran))
                            {
                                AyudaProgramacionLN.MapearError(movimiento, pParametro);
                                return false;
                            }
                        }
                        break;
                    default:
                        break;
                }

            }
            return true;
        }

        /// <summary>
        /// Metodo para generar las Altas de Movimientos de Bancos de Cajas Valores
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool ActualizarBancosCuentas(TESCajasMovimientosValores pParametro, Database bd, DbTransaction tran)
        {
            //Guardeo el detalle de las transferencias
            foreach (TESBancosCuentasMovimientos movimiento in pParametro.BancosCuentasMovimientos)
            {
                //cheque.IdOrdenCobroFormaCobro = detalle.IdOrdenCobroFormaCobro;
                movimiento.IdCajaMovimientoValor = pParametro.IdCajaMovimientoValor;
                //cheque.Estado.IdEstado = (int)EstadosCheques.IngresoCheque;
                movimiento.UsuarioLogueado = pParametro.UsuarioLogueado;
                if (!BancosF.BancosCuentasMovimientosAgregar(movimiento, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(movimiento, pParametro);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// No se usa mas....
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        [Obsolete("No se usa mas el metodo")]
        private bool ActualizarChequesCierreCaja(TESCajas pParametro, Database bd, DbTransaction tran)
        {
            List<TESCheques> lista = BaseDatos.ObtenerBaseDatos().ObtenerLista<TESCheques>("TESChequesSeleccionarPorCaja", pParametro, bd, tran);
            TESChequesMovimientos chequeMovimiento;
            foreach (TESCheques cheque in lista)
            {
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
                chequeMovimiento.Fecha = DateTime.Now;
                chequeMovimiento.Filial.IdFilial = cheque.Filial.IdFilial;
                chequeMovimiento.FilialDestino.IdFilial = cheque.Filial.IdFilial;
                chequeMovimiento.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.RecibirChequesCajas;
                chequeMovimiento.Estado.IdEstado = (int)Estados.Activo;
                chequeMovimiento.UsuarioLogueado = cheque.UsuarioLogueado;
                if (!BancosF.ChequesAgregarMovimiento(chequeMovimiento, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(chequeMovimiento, pParametro);
                    return false;
                }
            }

            return true;
        }

        private bool ValidarCajaMovimiento(TESCajas pParametro, TESCajasMovimientos pCajaMovimiento, Objeto pRefTipoOperacion, Database bd, DbTransaction tran)
        {
            bool resultado = true;

            TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ValidarSaldoCajas);
            bool validarSaldoCaja = valor.ParametroValor == string.Empty ? true : Convert.ToBoolean(valor.ParametroValor);

            if (validarSaldoCaja 
                && pCajaMovimiento.TipoOperacion.TipoMovimiento.IdTipoMovimiento == (int)EnumTGETiposMovimientos.Debito
                && pCajaMovimiento.CajaMoneda.Moneda.IdMoneda==(int)EnumTGEMonedas.PesosArgentinos
                && pCajaMovimiento.CajasMovimientosValores.Exists(x=>x.TipoValor.IdTipoValor==(int)EnumTiposValores.Efectivo)
                )
            {
                TESCajasMonedas cajaMoneda = pParametro.CajasMonedas.Find(x => x.Moneda.IdMoneda == pCajaMovimiento.CajaMoneda.Moneda.IdMoneda);
                //Control de Efectivo
                decimal importeMovimientoEfectivo = pCajaMovimiento.CajasMovimientosValores.Where(x=>x.TipoValor.IdTipoValor==(int)EnumTiposValores.Efectivo).Sum(x=>x.Importe);
                decimal importeCajaEfectivo = this.ObtenerImporteEfectivo(cajaMoneda, bd, tran);
                if (importeCajaEfectivo < importeMovimientoEfectivo )
                {
                    pCajaMovimiento.CodigoMensaje = "ValidarMovimientosSaldoCaja";
                    pCajaMovimiento.CodigoMensajeArgs.Add(importeCajaEfectivo.ToString("C2"));
                    pCajaMovimiento.CodigoMensajeArgs.Add(importeMovimientoEfectivo.ToString("C2"));
                    resultado = false;
                }
            }

            //Valido la suma de los Valores con el Importe del Comprobante
            if (pCajaMovimiento.Importe != pCajaMovimiento.CajasMovimientosValores.Sum(x => x.Importe))
            {
                pCajaMovimiento.CodigoMensaje = "ValidarMovimientosValoresImporte";
                resultado = false;
            }

            PropertyInfo prop = pRefTipoOperacion.GetType().GetProperty("TipoValor");
            if (prop != null)
            {
                TGETiposValores tipoValor = (TGETiposValores)prop.GetValue(pRefTipoOperacion, null);
                if (tipoValor.IdTipoValor == (int)EnumTiposValores.Cheque)
                {
                    if (pCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor != (int)EnumTiposValores.Cheque))
                    {
                        pCajaMovimiento.CodigoMensaje = "ValidarMovimientoTipoValor";
                        resultado = false;
                    }
                }
            }

            //Validaciones de Despositos de Cheques Individuales
            prop = pRefTipoOperacion.GetType().GetProperty("TipoOperacion");
            if (prop != null)
            {
                TGETiposOperaciones op = (TGETiposOperaciones)prop.GetValue(pRefTipoOperacion, null);
                if (op.IdTipoOperacion == (int)EnumTGETiposOperaciones.AhorroDepositos)
                {
                    if (pCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero))
                    {
                        if (pCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero).Cheques.Count > 1)
                        {
                            pParametro.CodigoMensaje = "ValidarCantidadValoresChequesTerceros";
                            return false;
                        }
                    }
                }
            }

            //Validaciones de Cheques Fecha Diferido
            if (pCajaMovimiento.CajasMovimientosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque))
            {
                TESCajasMovimientosValores movValor = pCajaMovimiento.CajasMovimientosValores.Find(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque);
                if (movValor.Cheques.Exists(x => !x.FechaDiferido.HasValue))
                {
                    pCajaMovimiento.CodigoMensaje = "ValidarFechaChequeDiferido";
                    pCajaMovimiento.CodigoMensajeArgs.Add(movValor.Cheques.First(x => !x.FechaDiferido.HasValue).NumeroCheque);
                    resultado = false;
                }
                if (movValor.Cheques.Exists(x => x.NumeroCheque == string.Empty))
                {
                    pCajaMovimiento.CodigoMensaje = "ValidarNumeroCheque";
                    pCajaMovimiento.CodigoMensajeArgs.Add(movValor.Cheques.First(x => x.NumeroCheque == string.Empty).NumeroCheque);
                    resultado = false;
                }
            }

            return resultado;
        }

        /// <summary>
        /// Devuelve el Importe de Efectivo de la Caja Moneda
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        private decimal ObtenerImporteEfectivo(TESCajasMonedas pParametro, Database bd, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<TESCajasMonedas>("TESCajasMonedasSeleccionarTipoValor", pParametro, bd, tran).SaldoFinal;
        }

        /// <summary>
        /// Devuelve el Importe de Efectivo de la Caja Moneda
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public decimal ObtenerImporteEfectivo(TESCajasMonedas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<TESCajasMonedas>("TESCajasMonedasSeleccionarTipoValor", pParametro).SaldoFinal;
        }

        private List<InterfazValoresImportes> ObtenerValoresImporte(TESCajasMovimientos pMovimiento)
        {
            List<InterfazValoresImportes> resultado = new List<InterfazValoresImportes>();
            InterfazValoresImportes item;
            foreach (TESCajasMovimientosValores mov in pMovimiento.CajasMovimientosValores)
            {
                switch (mov.TipoValor.IdTipoValor)
                {
                    case (int)EnumTiposValores.Efectivo:
                        item = new InterfazValoresImportes();
                        item.IdTipoValor = mov.TipoValor.IdTipoValor;
                        item.Importe = mov.Importe;
                        resultado.Add(item);
                        break;
                    case (int)EnumTiposValores.Cheque:
                        foreach (TESCheques ch in mov.Cheques)
                        {
                            item = new InterfazValoresImportes();
                            item.IdTipoValor = mov.TipoValor.IdTipoValor;
                            item.Importe = ch.Importe;
                            item.IdBancoCuenta = ch.IdBancoCuenta;
                            resultado.Add(item);
                        }
                        break;
                    case (int)EnumTiposValores.Transferencia:
                        foreach (TESBancosCuentasMovimientos bcoMov in mov.BancosCuentasMovimientos)
                        {
                            item = new InterfazValoresImportes();
                            item.IdTipoValor = mov.TipoValor.IdTipoValor;
                            item.Importe = bcoMov.Importe;
                            item.IdBancoCuenta = bcoMov.BancoCuenta.IdBancoCuenta;
                            resultado.Add(item);
                        }
                        break;
                    case (int)EnumTiposValores.ChequeTercero:
                        foreach (TESCheques ch in mov.Cheques)
                        {
                            item = new InterfazValoresImportes();
                            item.IdTipoValor = mov.TipoValor.IdTipoValor;
                            item.Importe = ch.Importe;
                            //item.IdBancoCuenta = ch.IdBancoCuenta;
                            resultado.Add(item);
                        }
                        break;
                    case (int)EnumTiposValores.TarjetaCredito:
                        foreach (TESTarjetasTransacciones tarjeta in mov.TarjetasTransacciones)
                        {
                            item = new InterfazValoresImportes();
                            item.IdTipoValor = mov.TipoValor.IdTipoValor;
                            item.Importe = tarjeta.Importe;
                            resultado.Add(item);
                        }

                        break;
                    default:
                        break;
                }
            }
            var agrupado = resultado.GroupBy(x => new { x.IdTipoValor, x.IdBancoCuenta })
                .Select(y => new InterfazValoresImportes() { IdTipoValor = y.Key.IdTipoValor, IdBancoCuenta = y.Key.IdBancoCuenta, Importe = y.Sum(z => z.Importe) });

            return agrupado.ToList();
        }

        public int ObtenerNumeroCaja(TESCajas pParametro)
        {
            TESCajas caja = BaseDatos.ObtenerBaseDatos().Obtener<TESCajas>("[TESCajasSeleccionarUltimoNumeroCaja]", pParametro);
            caja.NumeroCaja++;
                        
            return caja.NumeroCaja;
        }

        //private bool AsignacionEfectivoACajaAutomatico(TESTesorerias pParametro, Database bd, DbTransaction tran)
        //{
        //    return new TESTesoreriasLN().ActualizarMonedasMovimientos(pParametro, bd, tran);
        //}

        public List<TESCajasMonedas> ObtenerSaldoAnteriorPorUsuario(TESCajas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESCajasMonedas>("TESCajasMonedasObtenerSaldoAnteriorPorIdUsuario", pParametro);
        }

        
        public bool TraspasoEfectivoActualizarCajas(TESCajas cajaOrigen, TESCajas cajaDestino)
        {
            AyudaProgramacionLN.LimpiarMensajesError(cajaOrigen);

            bool resultado = true;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (resultado && !this.ActualizarMonedasMovimientos(cajaOrigen, bd, tran))
                    {
                        return false;
                    }
                    if (resultado && !this.ActualizarMonedasMovimientos(cajaDestino, bd, tran))
                    {
                        return false;
                    }
                    if (resultado)
                    {
                        tran.Commit();
                        cajaOrigen.CodigoMensaje = "ResultadoTransaccion";
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
                    cajaOrigen.CodigoMensaje = ex.Message;
                    return false;
                }
            }
            return resultado;
        }

        //public TESCajasMovimientos ObtenerMovimientoPendienteConfirmacionTipoOpFiltro(TESCajasMovimientos pParametro)
        //{
        //    return BaseDatos.ObtenerBaseDatos().Obtener<TESCajasMovimientos>("", pParametro);
        //}
    }
}
