using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ahorros.Entidades;
using Ahorros.LogicaNegocio;
using Afiliados.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Comunes.Entidades;
using Contabilidad.Entidades;
using System.Data;

namespace Ahorros
{
    public class AhorroF
    {
        #region Cuentas de Ahorro

        public static AhoCuentas CuentasObtenerDatosCompletos(AhoCuentas pParametro)
        {
            return new AhoCuentasLN().ObtenerDatosCompletos(pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Cuentas de Ahorro del Afiliado
        /// </summary>
        /// <param name="pParametro">IdAfiliado, [IdCuentaTipo]</param>
        /// <returns></returns>
        public static List<AhoCuentas> CuentasObtenerListaFiltro(AhoCuentas pParametro)
        {
            return new AhoCuentasLN().ObtenerListaFiltro(pParametro);
        }

        public static bool CuentasAgregar(AhoCuentas pParametro)
        {
            return new AhoCuentasLN().Agregar(pParametro);
        }

        public static bool CuentasModificar(AhoCuentas pParametro)
        {
            return new AhoCuentasLN().Modificar(pParametro);
        }

        #endregion

        #region Cuentas de Ahorro Movimientos

        /// <summary>
        /// Devuleve un movimiento de Ahorro
        /// </summary>
        /// <param name="pParametro">IdCuentaMovimiento</param>
        /// <returns></returns>
        public static AhoCuentasMovimientos MovimientosObtenerDatosCompletos(AhoCuentasMovimientos pParametro)
        {
            return new AhoCuentasMovimientosLN().ObtenerDatosCompletos(pParametro);
        }

        /// <summary>
        /// Devuelve los movimientos de una cuenta de ahorro
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static List<AhoCuentasMovimientos> MovimientosObtenerListaPorCuenta(AhoCuentas pParametro)
        {
            return new AhoCuentasMovimientosLN().ObtenerListaPorCuenta(pParametro);
        }  
        public static DataTable MovimientosObtenerListaPorCuentaDT(AhoCuentas pParametro)
        {
            return new AhoCuentasMovimientosLN().ObtenerListaPorCuentaDT(pParametro);
        }

        /// <summary>
        /// Agrega un movimiento de ahorro en la BD
        /// Deposito o Extracción
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static bool MovimientosAgregar(AhoCuentasMovimientos pParametro, TGETiposFuncionalidades tipoFuncionalidad)
        {
            return new AhoCuentasMovimientosLN().Agregar(pParametro, tipoFuncionalidad);
        }

        /// <summary>
        /// Anular un movimiento de ahorro en la BD
        /// Deposito o Extracción
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static bool MovimientosAnular(AhoCuentasMovimientos pParametro)
        {
            return new AhoCuentasMovimientosLN().Anular(pParametro);
        }

        /// <summary>
        /// Confirma un movimiento de Ahorro
        /// Metodo para los movimientos de Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static bool MovimientosConfirmar(Objeto pParametro, DateTime pFecha, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            return new AhoCuentasMovimientosLN().ConfirmarMovimiento(pParametro, pFecha, pValoresImportes, bd, tran);
        }

        /// <summary>
        /// Confirma o Rechaza un movimiento en Cheque de Ahorro
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static bool MovimientosConfirmarPorCheque(AhoCuentasMovimientos pParametro, DateTime pFecha, InterfazValoresImportes pInterfazValorImporte, Database bd, DbTransaction tran)
        {
            return new AhoCuentasMovimientosLN().ConfirmarMovimientoPorCheque(pParametro, pFecha, pInterfazValorImporte, bd, tran);
        }

        #endregion

        #region Cuentas de Ahorro Tipos

        /// <summary>
        /// Devuelve una lista de Tipos de Cuentas de Ahorros
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static List<AhoCuentasTipos> CuentasTiposObtenerListaFiltro(AhoCuentasTipos pParametro)
        {
            return new AhoCuentasTiposLN().ObtenerListaFiltro(pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Tipos de Cuentas de Ahorros en estado Activo
        /// </summary>
        /// <returns></returns>
        public static List<AhoCuentasTipos> CuentasTiposObtenerListaActiva()
        {
            return new AhoCuentasTiposLN().ObtenerListaActiva();
        }


        public static AhoCuentasTipos CuentasTiposObtenerDatosCompletos(AhoCuentasTipos pParametro)
        {
            return new AhoCuentasTiposLN().ObtenerDatosCompletos(pParametro);
        }

        public static bool CuentasTiposAgregar(AhoCuentasTipos pParametro)
        {
            return new AhoCuentasTiposLN().Agregar(pParametro);
        }

        public static bool CuentasTiposModificar(AhoCuentasTipos pParametro)
        {
            return new AhoCuentasTiposLN().Modificar(pParametro);
        }
        #endregion

        #region Plazos

        public static AhoPlazos PlazosObtenerDatosCompletos(AhoPlazos pParametro)
        {
            return new AhoPlazosLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<AhoPlazos> PlazosObtenerListaFiltro(AhoPlazos pParametro)
        {
            return new AhoPlazosLN().ObtenerListaFiltro(pParametro);
        }

        public static bool PlazosAgregar(AhoPlazos pParametro)
        {
            return new AhoPlazosLN().Agregar(pParametro);
        }

        public static bool PlazosModificar(AhoPlazos pParametro)
        {
            return new AhoPlazosLN().Modificar(pParametro);
        }

        public static List<AhoPlazos> PlazosObtenerAnterior (AhoPlazos pParametro)
        {
            return new AhoPlazosLN().ObtenerAnterior(pParametro);
        }
        #endregion

        #region Plazos Fijos

        public static AhoPlazosFijos PlazosFijosObtenerDatosCompletos(AhoPlazosFijos pParametro)
        {
            return new AhoPlazosFijosLN().ObtenerDatosCompletos(pParametro);
        }
        public static AhoPlazosFijos PlazosFijosRecalcularRenovacionAnticipada(AhoPlazosFijos pParametro)
        {
            return new AhoPlazosFijosLN().RecalcularRenovacionAnticipada(pParametro);
        }
        public static List<AhoPlazosFijos> PlazosFijosObtenerListaFiltro(AhoPlazosFijos pParametro)
        {
            return new AhoPlazosFijosLN().ObtenerListaFiltro(pParametro);
        }
        public static DataTable PlazosFijosObtenerListaFiltroDT(AhoPlazosFijos pParametro)
        {
            return new AhoPlazosFijosLN().ObtenerListaFiltroDT(pParametro);
        }
        public static bool PlazosFijosAgregar(AhoPlazosFijos pParametro)
        {
            return new AhoPlazosFijosLN().Agregar(pParametro);
        }

        /// <summary>
        /// Anula un Plazo Fijo que no fue Confirmado en Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static bool PlazosFijosBorrar(AhoPlazosFijos pParametro)
        {
            return new AhoPlazosFijosLN().Borrar(pParametro);
        }
        public static bool PlazosFijosAnularCancelacion(AhoPlazosFijos pParametro)
        {
            return new AhoPlazosFijosLN().AnularCancelacion(pParametro);
        }


        public static bool PlazosFijosRenovacionAnticipada(AhoPlazosFijos pParametro)
        {
            return new AhoPlazosFijosLN().RenovacionAnticipada(pParametro);
        }

        /// <summary>
        /// Cancela un Plazo Fijo que fue confirmado en Caja.
        /// El plazo fijo queda pendiente de cancelacion por caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static bool PlazosFijosCancelar(AhoPlazosFijos pParametro)
        {
            return new AhoPlazosFijosLN().Cancelar(pParametro);
        }

        /// <summary>
        /// Confirma un Plazo Fijo de Ahorro
        /// Metodo para los movimientos de Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static bool PlazosFijosConfirmar(Objeto pParametro, DateTime pFecha, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            return new AhoPlazosFijosLN().Confirmar(pParametro, pFecha, pValoresImportes, bd, tran);
        }

        /// <summary>
        /// Confirma la Cancelación de un Plazo Fijo de Ahorro
        /// Metodo para los movimientos de Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static bool PlazosFijosConfirmarCancelar(Objeto pParametro, DateTime pFecha, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            return new AhoPlazosFijosLN().ConfirmarCancelar(pParametro, pFecha, pValoresImportes, bd, tran);
        }

        /// <summary>
        /// Renueva un Plazo Fijo de Forma Manual
        /// </summary>
        /// <param name="pRenovarPlazoFijo">Plazo Fijo Nuevo</param>
        /// <param name="pPlazoFijoAnterior">Plazo Fijo que se renueva</param>
        /// <returns></returns>
        public static bool PlazosFijosRenovar(AhoPlazosFijos pRenovarPlazoFijo)
        {
            return new AhoPlazosFijosLN().Renovar(pRenovarPlazoFijo);
        }

        /// <summary>
        /// Pasa un Plazo Fijo a la Caja para su Pago
        /// </summary>
        /// <param name="pRenovarPlazoFijo">Plazo Fijo</param>
        /// <returns></returns>
        public static bool PlazosFijosPagar(AhoPlazosFijos pRenovarPlazoFijo)
        {
            return new AhoPlazosFijosLN().Pagar(pRenovarPlazoFijo);
        }

        /// <summary>
        /// Confirma la Renovación de un Plazo Fijo de Ahorro
        /// Metodo para los movimientos de Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static bool PlazosFijosConfirmarRenovar(Objeto pParametro, DateTime pFecha, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            return new AhoPlazosFijosLN().ConfirmarRenovar(pParametro, pFecha, pValoresImportes, bd, tran);
        }

        /// <summary>
        /// Modificar un Plazo Fijo
        /// </summary>
        /// <param name="pRenovarPlazoFijo">Plazo Fijo</param>
        /// <returns></returns>
        public static bool PlazosFijosModificar(AhoPlazosFijos pParametro)
        {
            return new AhoPlazosFijosLN().Modificar(pParametro);
        }

        #endregion

        #region Tipos Operaciones

        //public static List<AhoTiposOperaciones> TiposOperacionesObtenerListaFiltro(AhoTiposOperaciones pParametro)
        //{
        //    return new AhoTiposOperacionesLN().ObtenerListaFiltro(pParametro);
        //}

        #endregion

        #region Tipos Renovaciones

        public static List<AhoTiposRenovaciones> TiposRenovacionesObtenerLista(AhoTiposRenovaciones pParametro)
        {
            return new AhoTiposRenovacionesLN().ObtenerLista(pParametro);
        }

        #endregion
    }
}
