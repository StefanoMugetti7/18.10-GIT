using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cargos.LogicaNegocio;
using Cargos.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Generales.Entidades;
using Comunes.Entidades;
using Afiliados.Entidades;
using Contabilidad.Entidades;
using System.Data;

namespace Cargos
{
    public class CargosF
    {
        #region Cuenta Corriente

        public static CarCuentasCorrientes CuentasCorrientesObtenerDatosCompletos(CarCuentasCorrientes pParametro)
        {
            return new CarCuentasCorrientesLN().ObtenerDatosCompletos(pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Cuenta Corriente por Afiliado
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static List<CarCuentasCorrientes> CuentasCorrientesObtener(CarCuentasCorrientes pParametro)


        {
            return new CarCuentasCorrientesLN().Obtener(pParametro);
        }


   


        /// <summary>
        /// Devuelve una lista de Cuenta Corriente por Afiliado pendiente de Cobro
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static List<CarCuentasCorrientes> CuentasCorrientesObtenerPendientesCobro(CarCuentasCorrientes pParametro)
        {
            return new CarCuentasCorrientesLN().ObtenerPendienteCobros(pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Periodos de la Cuenta Corriente por Afiliado
        /// </summary>
        /// <param name="pParametro">IdAfiliado</param>
        /// <returns></returns>
        public static List<TGEListasValoresDetalles> CuentasCorrientesObtenerPeriodos(CarCuentasCorrientes pParametro)
        {
            return new CarCuentasCorrientesLN().ObtenerPeriodos(pParametro);
        }

        /// <summary>
        /// Devuelve el Saldo Actual de la Cuenta Corriente del Socio
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static decimal CuentasCorrientesObtenerSaldoActual(CarCuentasCorrientes pParametro)
        {
            return new CarCuentasCorrientesLN().ObtenerSaldoActual(pParametro);
        }

        public static bool CuentasCorrientesAgregar(CarCuentasCorrientes pParametro, Database bd, DbTransaction tran)
        {
            return new CarCuentasCorrientesLN().Agregar(pParametro, new List<InterfazValoresImportes>(), bd, tran);
        }

        public static bool CuentasCorrientesAgregar(CarCuentasCorrientes pParametro, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            return new CarCuentasCorrientesLN().Agregar(pParametro, pValoresImportes, bd, tran);
        }

        public static bool CuentasCorrientesActualizar(CarCuentasCorrientes pParametro)
        {
            return new CarCuentasCorrientesLN().Actualizar(pParametro);
        }

        public static bool CuentasCorrientesActualizar(CarCuentasCorrientes pParametro, Database bd, DbTransaction tran)
        {
            return new CarCuentasCorrientesLN().Actualizar(pParametro, bd, tran);
        }

        public static bool CuentasCorrientesActualizarEstado(CarCuentasCorrientes pParametro, Database bd, DbTransaction tran)
        {
            return new CarCuentasCorrientesLN().ActualizarEstado(pParametro, bd, tran);
        }

        public static DataTable CuentasCorrientesObtenerDT(CarCuentasCorrientes pParametro)
        {
            return new CarCuentasCorrientesLN().ObtenerDT(pParametro);
        }

        public static DataTable CuentasCorrientesObtenerDTGrilla(CarCuentasCorrientes pParametro)
        {
            return new CarCuentasCorrientesLN().ObtenerDTGrilla(pParametro);
        }

        public static DataTable CuentasCorrientesObtenerCobrosPorCargo(CarCuentasCorrientes pParametro)
        {
            return new CarCuentasCorrientesLN().ObtenerCobrosPorCargo(pParametro);
        }

        public static DataTable CuentasCorrientesObtenerFacturacionAnticipada(CarCuentasCorrientes pParametro)
        {
            return new CarCuentasCorrientesLN().ObtenerFacturacionAnticipada(pParametro);
        }

        public static DataTable CuentasCorrientesObetenerCodigosConceptosCargos(CarCuentasCorrientes pParametro)
        {
            return new CarCuentasCorrientesLN().ObetenerCodigosConceptosCargos(pParametro);
        }
            public static bool CuentasCorrientesAgregarFacturacionAnticipada(CarCuentasCorrientes pParametro)
        {
            return new CarCuentasCorrientesLN().AgregarFacturacionAnticipada(pParametro);
        }

        public static bool CuentasCorrientesDesimputarCobro(CarCuentasCorrientes pParametro)
        {
            return new CarCuentasCorrientesLN().DesimputarCobro(pParametro);
        }

        public static bool CuentasCorrientesRevertirCobroCargos(CarCuentasCorrientes pParametro)
        {
            return new CarCuentasCorrientesLN().RevertirCobroCargos(pParametro);
        }
        #endregion

        #region Reservas de Turismo
        public static DataTable TiposCargosAfiliadosObtenerReservasTurismo(CarTiposCargosAfiliadosFormasCobros pParametro)
        {
            return new CarTiposCargosAfiliadosLN().ObtenerReservasTurismo(pParametro);
        }
        #endregion

        #region Tipos Cargos


        /// <summary>
        /// Devuelve un Tipo Cargo con todos sus datos completos
        /// </summary>
        /// <param name="pAfiliado"></param>
        /// <returns></returns>
        public static CarTiposCargos TiposCargosObtenerDatosCompletos(CarTiposCargos pParametro)
        {
            return new CarTiposCargosLN().ObtenerDatosCompletos(pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Tipos de Cargos Filtrada
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static List<CarTiposCargos> TiposCargosObtenerListaFiltro(CarTiposCargos pParametro)
        {
            return new CarTiposCargosLN().ObtenerListaFiltro(pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Tipos de Cargos en estado Activo
        /// pendiente de Configurar para el Afiliado
        /// </summary>
        /// <param name="pParametro">IdAfiliado</param>
        /// <returns></returns>
        public static List<CarTiposCargos> TiposCargosObtenerListaActiva(CarTiposCargosAfiliadosFormasCobros pParametro)
        {
            return new CarTiposCargosLN().ObtenerListaActiva(pParametro);
        }

        public static List<CarTiposCargos> TiposCargosObtenerFacturacionAnticipadaCombo(AfiAfiliados pParametro)
        {
            return new CarTiposCargosLN().ObtenerFacturacionAnticipadaCombo(pParametro);
        }

        /// <summary>
        /// Agrega un Tipo Cargo en la BD
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static bool TiposCargosAgregar(CarTiposCargos pParametro)
        {
            return new CarTiposCargosLN().Agregar(pParametro);
        }

        /// <summary>
        /// Actualiza un Tipo Cargo en la BD
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static bool TiposCargosModificar(CarTiposCargos pParametro)
        {
            return new CarTiposCargosLN().Modificar(pParametro);
        }

        /// <summary>
        /// Actualiza un Tipo Cargo en la BD
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static List<AfiCategorias> TiposCargosObtenerCategoriasSinAsociar(CarTiposCargos pParametro)
        {
            return new CarTiposCargosLN().ObtenerListaCategoriasSinAsociar(pParametro);
        }

        #endregion

        #region Tipos Cargos Afiliados
        /// <summary>
        /// Devuelve un Cargo Afiliado
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static CarTiposCargosAfiliadosFormasCobros TiposCargosAfiliadosObtenerDatosCompletos(CarTiposCargosAfiliadosFormasCobros pParametro)
        {
            return new CarTiposCargosAfiliadosLN().ObtenerDatosCompletos(pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Cargos por Afiliado
        /// </summary>
        /// <param name="pAfiliado"></param>
        /// <returns></returns>
        public static List<CarTiposCargosAfiliadosFormasCobros> TiposCargosAfiliadosObtenerLista(CarTiposCargosAfiliadosFormasCobros pParametro)
        {
            return new CarTiposCargosAfiliadosLN().ObtenerLista(pParametro);
        }

        public static List<CarTiposCargosAfiliadosFormasCobros> TiposCargosAfiliadosObtenerABonificar(CarTiposCargosAfiliadosFormasCobros pParametro)
        {
            return new CarTiposCargosAfiliadosLN().ObtenerABonificar(pParametro);
        }

        public static DataTable CarTiposCargosAfiliadosFormasCobrosObtenerCuotasPendientes(CarTiposCargosAfiliadosFormasCobros pParametro)
        {
            return new CarTiposCargosAfiliadosLN().ObtenerCuotasPendientes(pParametro);
        }

        /// <summary>
        /// Agrega un Cargo Afiliado Forma Cobro en la BD
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static bool TiposCargosAfiliadosAgregar(CarTiposCargosAfiliadosFormasCobros pParametro)
        {
            return new CarTiposCargosAfiliadosLN().Agregar(pParametro);
        }

        /// <summary>
        /// Agrega un Cargo Afiliado Forma Cobro en la BD
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static bool TiposCargosAfiliadosAgregar(CarTiposCargosAfiliadosFormasCobros pParametro, Database bd, DbTransaction tran)
        {
            return new CarTiposCargosAfiliadosLN().Agregar(pParametro, bd, tran);
        }

        /// <summary>
        /// Actualiza un Cargo Afiliado Forma Cobro en la BD
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static bool TiposCargosAfiliadosModificar(CarTiposCargosAfiliadosFormasCobros pParametro)
        {
            return new CarTiposCargosAfiliadosLN().Modificar(pParametro);
        }

        #endregion
    }
}
