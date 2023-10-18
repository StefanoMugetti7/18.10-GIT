using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesorerias.Entidades;
using Tesorerias.LogicaNegocio;
using Comunes.Entidades;
using Cobros.Entidades;
using Bancos.Entidades;
using Generales.Entidades;
using Contabilidad.Entidades;
using System.Data;

namespace Tesorerias
{
    public class TesoreriasF
    {
        #region Cajas

        /// <summary>
        /// Devuelve los Totales por Moneda y Tipo de Valor para una Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static DataTable CajasMonedasSeleccionarTotalesTipoValorPorCaja(TESCajas pParametro)
        {
            return new TESCajasLN().CajasMonedasSeleccionarTotalesTipoValorPorCaja(pParametro);
        }

        /// <summary>
        /// Valida si la Caja esta abierta para el usuario
        /// </summary>
        /// <param name="pParametros">IdTesoreria</param>
        /// <returns></returns>
        public static bool CajasValidarAbierta(TESCajas pParametro)
        {
            return new TESCajasLN().ValidarAbierta(pParametro);
        }

        /// <summary>
        /// Si es la primera ves genera la apertura de la Caja y
        /// Obtiene los datos de la Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static bool CajasAbrirObtenerDatos(TESCajas pParametro)
        {
            return new TESCajasLN().AbrirObtenerDatosCaja(pParametro);
        }

        /// <summary>
        /// Obtiene los datos completos de la Caja con los tipos de Monedas Asignados y Movimientos Confirmados
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static TESCajas CajasObtenerDatosCompletos(TESCajas pParametro)
        {
            return new TESCajasLN().ObtenerDatosCompletos(pParametro);
        }

        /// <summary>
        /// Devuelve el Importe de Efectivo de la Caja Moneda
        /// </summary>
        /// <param name="pParametro">Caja Moneda</param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static decimal CajasObtenerImporteEfectivo(TESCajasMonedas pParametro)
        {
            return new TESCajasLN().ObtenerImporteEfectivo(pParametro);
        }

        /// <summary>
        /// Devuevle el objeto de referencia para Confirmar la Operacion en Caja
        /// </summary>
        /// <param name="pParametro">IdRefTipoOperacion, IdTipoOperacion</param>
        /// <returns></returns>
        public static Objeto CajasObtenerMovimientoPendienteConfirmacion(TESCajasMovimientos pParametro)
        {
            return new TESCajasLN().ObtenerMovimientoPendienteConfirmacion(pParametro);
        }

        /// <summary>
        /// Confirma un movimiento de Caja
        /// </summary>
        /// <param name="pParametro">Datos de la Caja</param>
        /// <param name="pCajaMovimiento">Movimiento a confirmar</param>
        /// <param name="pRefTipoOperacion">Referencia con el objecto a Confirmar por la caja</param>
        /// <returns></returns>
        public static bool CajasConfirmarMovimiento(TESCajas pParametro, TESCajasMovimientos pCajaMovimiento, Objeto pRefTipoOperacion)
        {
            return new TESCajasLN().ConfirmarMovimiento(pParametro, pCajaMovimiento, pRefTipoOperacion);
        }

        /// <summary>
        /// Anula un movimiento de Ingesos / Egresos de Caja
        /// </summary>
        /// <param name="pCajaMovimiento">Movimiento a ANULAR</param>
        /// <returns></returns>
        public static bool CajasAnularMovimientoIngresosEgresos(TESCajasMovimientos pParametro)
        {
            return new TESCajasMovimientosLN().AnularMovimientoIngresosEgreso(pParametro);
        }

        public static bool CajasMovimientosCircuitoDiarioCajasAutomatico(TESCajasMovimientos pParametro)
        {
            return new TESCajasMovimientosLN().CircuitoDiarioCajasAutomatico(pParametro);
        }


        /// <summary>
        /// Confirma un movimiento manual de Caja 
        /// </summary>
        /// <param name="pParametro">Datos de la Caja</param>
        /// <param name="pCajaMovimiento">Movimiento manual a confirmar</param>
        /// <returns></returns>
        public static bool CajasConfirmarMovimiento(TESCajas pParametro, TESCajasMovimientos pCajaMovimiento)
        {
            return new TESCajasLN().ConfirmarMovimiento(pParametro, pCajaMovimiento);
        }

        /// <summary>
        /// Confirma una Orden de Cobro
        /// </summary>
        /// <param name="pParametro">Datos de la Caja</param>
        /// <param name="pCajaMovimiento">Orden de Cobro</param>
        /// <returns></returns>
        public static bool CajasConfirmarOrdenesCobros(TESCajas pParametro, CobOrdenesCobros pOrdenCobro)
        {
            throw new NotImplementedException();
            //return new TESCajasLN().ConfirmarOrdenCobro(pParametro,  pOrdenCobro);
        }
        
        /// <summary>
        /// Genera el Cierre de la Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static bool CajasCerrar(TESCajas pParametro)
        {
            return new TESCajasLN().Modificar(pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Cajas Abiertas para una Tesoreria
        /// con la lista de Cajas Monedas
        /// </summary>
        /// <param name="pParametro">IdTesoreria</param>
        /// <returns></returns>
        public static List<TESCajas> CajasObtenerAbiertas(TESCajas pParametro)
        {
            return new TESCajasLN().ObtenerAbiertas(pParametro);
        }

        /// <summary>
        /// Devuelve los movimientos pendientes de confirmacion para las cajas
        /// </summary>
        /// <param name="pParametro">IdTesoreria</param>


        public static DataTable CajasObtenerMovimientosPendientes(TESTesorerias pParametro)
        {
            return new TESCajasLN().ObtenerPendientesConfirmacion(pParametro);
        }

        public static int CajasObtenerNumeroCaja(TESCajas pParametro)
        {
            return new TESCajasLN().ObtenerNumeroCaja(pParametro);
        }

        public static List<TESCajasMonedas> CajasObtenerSaldoAnteriorPorUsuario(TESCajas pParametro)
        {
            return new TESCajasLN().ObtenerSaldoAnteriorPorUsuario(pParametro);
        }

        public static bool CajasTraspasoEfectivoActualizarCajas(TESCajas cajaOrigen, TESCajas cajaDestino)
        {
            return new TESCajasLN().TraspasoEfectivoActualizarCajas(cajaOrigen, cajaDestino);
        }

        public static TESCajasMovimientos CajasObtenerMovimientosPendientesFiltro(TESFiltroMovimientosPendientes pParametro)
        {
            return new TESCajasLN().ObtenerPendienteConfirmacionFiltroMovimiento(pParametro);
        }

        #endregion

        #region Reabrir Cajas

        public static bool CajasValidarCerradaMismaTesoreria(TESCajas pParametro)
        {
            return new TESCajasLN().ValidarCerradaMismaTesoreria(pParametro);
        }

        public static bool CajasModificarReabrirCaja(TESCajas pParametro)
        {
            return new TESCajasLN().ModificarReabrirCaja(pParametro);
        }

        #endregion

        #region Cajas Movimientos

        public static DataTable CajasMovimientosObtenerGrilla(TESCajasMovimientos pParametro)
        {
            return new TESCajasMovimientosLN().ObtenerGrilla(pParametro);
        }

        public static DataTable CajasMovimientosObtenerGrillaFlujoEfectivo(TESCajasMovimientos pParametro)
        {
            return new TESCajasMovimientosLN().ObtenerGrillaFlujoEfectivo(pParametro);
        }

        public static TESCajasMovimientos CajasMovimientosObtenerDatosCompletos(TESCajasMovimientos pParametro)
        {
            return new TESCajasMovimientosLN().ObtenerDatosCompletos(pParametro);
        }

        public static TESCajasMovimientos CajasMovimientosObtenerMovimientoValoresXML(TESCajasMovimientos pParametro)
        {
            return new TESCajasMovimientosLN().ObtenerMovimientoValoresXML(pParametro);
        }

        /// <summary>
        /// Confirma un movimiento de Caja Metodo Base de Datos (nuevo)
        /// </summary>
        /// <param name="pCajaMovimiento">Movimiento a confirmar</param>
        /// <returns></returns>
        public static bool CajasConfirmarMovimientoXml(TESCajasMovimientos pCajaMovimiento)
        {
            return new TESCajasMovimientosLN().ConfirmarMovimientoXml(pCajaMovimiento);
        }

        #endregion

        public static bool TesoreriaMovimientoAgregarTransferenciaBancos(TESTesoreriasMovimientos pParametro)
        {
            return new TESTesoreriasLN().AgregarMovimientoTransferenciaBancos(pParametro);
        }

        public static bool TesoreriaMovimientoAnularTraspasoChequess(TESTesoreriasMovimientos pParametro)
        {
            return new TESTesoreriasLN().AnularTraspaso(pParametro);
        }

        public static TESTesoreriasMovimientos TesoreriaMovimientosObtenerDatosCompletos(TESTesoreriasMovimientos pParametro)
        {
            return new TESTesoreriasLN().MovimientosObtenerDatosCompletos(pParametro);
        }

        #region Tesorerias

        /// <summary>
        /// Valida si la Tesoreria esta abierta para el día de la fecha
        /// </summary>
        /// <param name="pParametros">IdFilial</param>
        /// <returns></returns>
        public static bool TesoreriasValidarAbierta(TESTesorerias pParametro)
        {
            return new TESTesoreriasLN().ValidarAbierta(pParametro);
        }

        /// <summary>
        /// Valida si la Tesoreria esta cerrada para el día de la fecha
        /// </summary>
        /// <param name="pParametros">IdFilial</param>
        /// <returns></returns>
        public static bool TesoreriasValidarCerrada(TESTesorerias pParametro)
        {
            return new TESTesoreriasLN().ValidarCerrada(pParametro);
        }

        /// <summary>
        /// Valida si la Tesoreria esta abierta para una fecha anterior
        /// </summary>
        /// <param name="pParametros">IdFilial</param>
        /// <returns></returns>
        public static bool TesoreriasValidarAbiertaFechaAnterior(TESTesorerias pParametro)
        {
            return new TESTesoreriasLN().ValidarAbiertaFechaAnterior(pParametro);
        }

        /// <summary>
        /// Si es la primera ves genera la apertura de la Tesoreria y
        /// Obtiene los datos completos de la Tesoreria
        /// En caso de error devuelve FALSE y el mensaje
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static bool TesoreriaAbrirObtenerDatos(TESTesorerias pParametro)
        {
            return new TESTesoreriasLN().AbrirObtenerDatosTesoreria(pParametro);
        }

        /// <summary>
        /// Obtiene los datos completos de la Tesoreria con los tipos de monedas y valores
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static TESTesorerias TesoreriasObtenerDatosCompletos(TESTesorerias pParametro)
        {
            return new TESTesoreriasLN().ObtenerDatosCompletos(pParametro);
        }
   
        public static DataTable TesoreriasObtenerDatosCompletosSaldosCajas(TESTesorerias pParametro)
        {
            return new TESTesoreriasLN().ObtenerDatosCompletosSaldosCajas(pParametro);
        }
        /// <summary>
        /// Devuelve la última Tesoreria Abierta
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static TESTesorerias TesoreriasObtenerAbierta(TESTesorerias pParametro)
        {
            return new TESTesoreriasLN().ObtenerPorAbierta(pParametro);
        }

        /// <summary>
        /// Guarda los movimientos de Tesoreria
        /// Actualiza los saldos de las monedas de la tesoreria
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static bool TesoreriasModificar(TESTesorerias pParametro)
        {
            return new TESTesoreriasLN().Modificar(pParametro);
        }

        /// <summary>
        /// Reabrir Tesoreria
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static bool TesoreriasModificarReabrir(TESTesorerias pParametro)
        {
            return new TESTesoreriasLN().ModificarReabrirTesoreria(pParametro);
        }

        /// <summary>
        /// Confirma un movimiento pendiente
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        //public static bool TesoreriasConfirmarTesoreriaMovimiento(TESTesorerias pTesoreria, TESTesoreriasMovimientos pParametro)
        //{
        //    return new TESTesoreriasLN().ConfirmarTesoreriaMovimiento(pTesoreria, pParametro);
        //}

        /// <summary>
        /// Genera el cierre de la Tesoreria
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static bool TesoreriasCerrar(TESTesorerias pParametro)
        {
            return new TESTesoreriasLN().Cerrar(pParametro);
        }

        /// <summary>
        /// Genera el cierre de la Tesoreria para un día Anterior
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static bool TesoreriasCierreAutomatico(TESTesorerias pParametro)
        {
            return new TESTesoreriasLN().CierreAutomatico(pParametro);
        }

        /// <summary>
        /// Transfiere los Cheques de la Caja a Tesoreria y Tesoreria al sector Bancos
        /// </summary>
        /// <param name="pLista"></param>
        /// <param name="pResultado"></param>
        /// <param name="pTipoOperacion"></param>
        /// <param name="pBancoCuentaMovimiento"></param>
        /// <returns></returns>
        public static bool TesoreriasTransferirCheques(List<TESCheques> pLista, TESTesorerias pTesoreria, EnumTGETiposOperaciones pTipoOperacion)
        {
            return new TESTesoreriasLN().TransferirCheques(pLista, pTesoreria, pTipoOperacion);
        }

        /// <summary>
        /// Devuelve los movimientos Pendientes de Confirmación (aceptacion) para una Tesoreria
        /// </summary>
        /// <param name="pParametro">IdFilial</param>
        /// <returns></returns>
        public static List<TESTesoreriasMovimientos> TesoreriasMovimientosObtenerPendientesTransferencia(TESTesorerias pParametro)
        {
            return new TESTesoreriasLN().ObtenerPendientesTransferencia(pParametro);
        }

        #region Remplazo FechaAbrir por FechaAbrirEvento
        public static TESTesorerias TesoreriaObtenerPorFilialFechaEvento(TESTesorerias pParametro)
        {
            return new TESTesoreriasLN().ObtenerPorFilialFechaEvento(pParametro);
        }

        public static bool TesoreriaValidarAbiertaFechaEventoAnterior(TESTesorerias pParametro)
        {
            return new TESTesoreriasLN().ValidarAbiertaFechaEventoAnterior(pParametro);
        }

        public static bool TesoreriaValidarCerradaEvento(TESTesorerias pParametro)
        {
            return new TESTesoreriasLN().ValidarCerradaEvento(pParametro);
        }

        public static TESTesorerias TesoreriasObtenerUltimoCierreFilialUsuarioEvento(TESTesorerias pParametro)
        {
            return new TESTesoreriasLN().ObtenerUltimoCierreFilialUsuarioEvento(pParametro);
        }

        

        #endregion
        #endregion
    }
}
