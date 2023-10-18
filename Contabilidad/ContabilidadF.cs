using Comunes.Entidades;
using Contabilidad.Entidades;
using Contabilidad.LogicaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Contabilidad
{
    public class ContabilidadF
    {
        #region Asientos Contables Cuentas Contables Parametros

        public static CtbAsientosContablesCuentasContablesParametros AsientosContablesCuentasContablesParametrosObtenerDatosCompletos(CtbAsientosContablesCuentasContablesParametros pParametro)
        {
            return new CtbAsientosContablesCuentasContablesParametrosLN().ObtenerDatosCompletos(pParametro);
        }

        /// <summary>
        /// Devuelve el parametro contable
        /// </summary>
        /// <param name="pParametro">IdFilial, IdEntidadContable, IdTipoValor, IdMoneda, [BancoCuentaIdBancoCuenta], IdEstado</param>
        /// <returns></returns>
        public static CtbAsientosContablesCuentasContablesParametros AsientosContablesCuentasContablesParametrosObtenerCuentaContable(CtbAsientosContablesCuentasContablesParametros pParametro, Database bd, DbTransaction tran)
        {
            return new CtbAsientosContablesCuentasContablesParametrosLN().ObtenerCuentaContable(pParametro, bd, tran);
        }

        public static List<CtbAsientosContablesCuentasContablesParametros> AsientosContablesCuentasContablesParametrosObtenerListar(CtbAsientosContablesCuentasContablesParametros pParametro)
        {
            return new CtbAsientosContablesCuentasContablesParametrosLN().ObtenerListaFiltro(pParametro);
        }

        public static List<CtbAsientosContablesCuentasContablesParametros> AsientosContablesCuentasContablesParametrosObtenerListaFiltro(CtbAsientosContablesCuentasContablesParametros pParametro)
        {
            return new CtbAsientosContablesCuentasContablesParametrosLN().ObtenerListaFiltro(pParametro);
        }

        public static DataTable AsientosContablesCuentasContablesParametrosObtenerListaFiltroDT(CtbAsientosContablesCuentasContablesParametros pParametro)
        {
            return new CtbAsientosContablesCuentasContablesParametrosLN().ObtenerListaFiltroDT(pParametro);
        }

        public static bool AsientosContablesCuentasContablesParametrosAgregar(CtbAsientosContablesCuentasContablesParametros pParametro)
        {
            return new CtbAsientosContablesCuentasContablesParametrosLN().Agregar(pParametro);
        }

        public static bool AsientosContablesCuentasContablesParametrosModificar(CtbAsientosContablesCuentasContablesParametros pParametro)
        {
            return new CtbAsientosContablesCuentasContablesParametrosLN().Modificar(pParametro);
        }

        #endregion

        #region Centros de Costos
        public static List<CtbCentrosCostosProrrateos> CentrosCostosProrrateosObtenerCombo(CtbCentrosCostosProrrateos pParametro)
        {
            return new CtbCentrosCostosProrrateosLN().ObtenerCombo(pParametro);
        }

        public static CtbCentrosCostosProrrateos CentrosCostosProrrateosObtenerDatosCompletos(CtbCentrosCostosProrrateos pParametro)
        {
            return new CtbCentrosCostosProrrateosLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<CtbCentrosCostosProrrateos> CentrosCostosProrrateosObtenerListarFiltro(CtbCentrosCostosProrrateos pParametro)
        {
            return new CtbCentrosCostosProrrateosLN().ObtenerListaFiltro(pParametro);
        }

        public static bool CentrosCostosProrrateosAgregar(CtbCentrosCostosProrrateos pParametro)
        {
            return new CtbCentrosCostosProrrateosLN().Agregar(pParametro);
        }

        public static bool CentrosCostosProrrateosModificar(CtbCentrosCostosProrrateos pParametro)
        {
            return new CtbCentrosCostosProrrateosLN().Modificar(pParametro);
        }

        #endregion

        #region Conceptos Contables

        public static CtbConceptosContables ConceptosContablesObtenerDatosCompletos(CtbConceptosContables pParametro)
        {
            return new CtbConceptosContablesLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<CtbConceptosContables> ConceptosContablesObtenerListar(CtbConceptosContables pParametro)
        {
            return new CtbConceptosContablesLN().ObtenerListaFiltro(pParametro);
        }

        public static List<CtbConceptosContables> ConceptosContablesObtenerListaFiltroCompleta(CtbConceptosContables pParametro)
        {
            return new CtbConceptosContablesLN().ObtenerListaFiltroCompleta(pParametro);
        }

        public static bool ConceptosContablesAgregar(CtbConceptosContables pParametro)
        {
            return new CtbConceptosContablesLN().Agregar(pParametro);
        }

        public static bool ConceptosContablesModificar(CtbConceptosContables pParametro)
        {
            return new CtbConceptosContablesLN().Modificar(pParametro);
        }

        public static List<CtbConceptosContables> ConceptosContablesObtenerListaFiltro(CtbConceptosContables pParametro)
        {
            return new CtbConceptosContablesLN().ObtenerListaFiltro(pParametro);
        }

        public static List<CtbConceptosContables> ConceptosContablesObtenerListaFiltro(TGETiposOperaciones pParametro)
        {
            return new CtbConceptosContablesLN().ObtenerListaFiltro(pParametro);
        }
        #endregion

        #region Capitulos

        public static CtbCapitulos CapitulosObtenerDatosCompletos(CtbCapitulos pParametro)
        {
            return new CtbCapitulosLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<CtbCapitulos> CapitulosObtenerListar(CtbCapitulos pParametro)
        {
            return new CtbCapitulosLN().ObtenerListaFiltro(pParametro);
        }

        public static List<CtbCapitulos> CapitulosObtenerListaFiltro(CtbCapitulos pParametro)
        {
            return new CtbCapitulosLN().ObtenerListaFiltro(pParametro);
        }

        public static bool CapitulosAgregar(CtbCapitulos pParametro)
        {
            return new CtbCapitulosLN().Agregar(pParametro);
        }

        public static bool CapitulosModificar(CtbCapitulos pParametro)
        {
            return new CtbCapitulosLN().Modificar(pParametro);
        }

        #endregion

        #region Departamentos

        public static CtbDepartamentos DepartamentosObtenerDatosCompletos(CtbDepartamentos pParametro)
        {
            return new CtbDepartamentosLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<CtbDepartamentos> DepartamentosObtenerListar(CtbDepartamentos pParametro)
        {
            return new CtbDepartamentosLN().ObtenerListaFiltro(pParametro);
        }

        public static List<CtbDepartamentos> DepartamentosObtenerListaFiltro(CtbDepartamentos pParametro)
        {
            return new CtbDepartamentosLN().ObtenerListaFiltro(pParametro);
        }

        public static bool DepartamentosAgregar(CtbDepartamentos pParametro)
        {
            return new CtbDepartamentosLN().Agregar(pParametro);
        }

        public static bool DepartamentosModificar(CtbDepartamentos pParametro)
        {
            return new CtbDepartamentosLN().Modificar(pParametro);
        }

        #endregion

        #region Ejercicios Contables

        public static CtbEjerciciosContables EjerciciosContablesObtenerActivo()
        {
            return new CtbEjerciciosContablesLN().ObtenerActivo();
        }

        public static CtbEjerciciosContables EjerciciosContablesObtenerActivo(Database bd, DbTransaction tran)
        {
            return new CtbEjerciciosContablesLN().ObtenerActivo(bd, tran);
        }

        /// <summary>
        /// Devuleve el Ejercicio Contable Activo segun la Fecha del Asiento
        /// </summary>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <param name="pAsiento"></param>
        /// <returns></returns>
        public static CtbEjerciciosContables EjerciciosContablesObtenerActivo(CtbAsientosContables pAsiento, Database bd, DbTransaction tran)
        {
            return new CtbEjerciciosContablesLN().ObtenerActivo(pAsiento, bd, tran);
        }

        public static CtbEjerciciosContables EjerciciosContablesObtenerUltimoActivo()
        {
            return new CtbEjerciciosContablesLN().ObtenerUltimoActivo();
        }

        public static CtbEjerciciosContables EjerciciosContablesObtenerDatosCompletos(CtbEjerciciosContables pParametro)
        {
            return new CtbEjerciciosContablesLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<CtbEjerciciosContables> EjerciciosContablesObtenerListar(CtbEjerciciosContables pParametro)
        {
            return new CtbEjerciciosContablesLN().ObtenerListaFiltro(pParametro);
        }

        public static List<CtbEjerciciosContables> EjerciciosContablesObtenerListaFiltro(CtbEjerciciosContables pParametro)
        {
            return new CtbEjerciciosContablesLN().ObtenerListaFiltro(pParametro);
        }

        public static bool EjerciciosContablesAgregar(CtbEjerciciosContables pParametro)
        {
            return new CtbEjerciciosContablesLN().Agregar(pParametro);
        }
        public static bool EjerciciosContablesActualizarPlanCuentas(CtbEjerciciosContables pParametro)
        {
            return new CtbEjerciciosContablesLN().ActualizarPlanCuentas(pParametro);
        }

        public static bool EjerciciosContablesModificar(CtbEjerciciosContables pParametro)
        {
            return new CtbEjerciciosContablesLN().Modificar(pParametro);
        }

        #endregion

        #region Filiales Cuentas Contables

        public static CtbFilialesCuentasContables FilialesCuentasContablesObtenerDatosCompletos(CtbFilialesCuentasContables pParametro, Database bd, DbTransaction tran)
        {
            return new CtbFilialesCuentasContablesLN().ObtenerDatosCompletos(pParametro, bd, tran);
        }

        #endregion

        #region Monedas

        public static CtbMonedas MonedasObtenerDatosCompletos(CtbMonedas pParametro)
        {
            return new CtbMonedasLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<CtbMonedas> MonedasObtenerListar(CtbMonedas pParametro)
        {
            return new CtbMonedasLN().ObtenerListaFiltro(pParametro);
        }

        public static List<CtbMonedas> MonedasObtenerListaFiltro(CtbMonedas pParametro)
        {
            return new CtbMonedasLN().ObtenerListaFiltro(pParametro);
        }

        public static bool MonedasAgregar(CtbMonedas pParametro)
        {
            return new CtbMonedasLN().Agregar(pParametro);
        }

        public static bool MonedasModificar(CtbMonedas pParametro)
        {
            return new CtbMonedasLN().Modificar(pParametro);
        }

        #endregion

        #region Rubros

        public static CtbRubros RubrosObtenerDatosCompletos(CtbRubros pParametro)
        {
            return new CtbRubrosLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<CtbRubros> RubrosObtenerListar(CtbRubros pParametro)
        {
            return new CtbRubrosLN().ObtenerListaFiltro(pParametro);
        }

        public static List<CtbRubros> RubrosObtenerListaFiltro(CtbRubros pParametro)
        {
            return new CtbRubrosLN().ObtenerListaFiltro(pParametro);
        }

        public static bool RubrosAgregar(CtbRubros pParametro)
        {
            return new CtbRubrosLN().Agregar(pParametro);
        }

        public static bool RubrosModificar(CtbRubros pParametro)
        {
            return new CtbRubrosLN().Modificar(pParametro);
        }

        #endregion

        #region SubRubros

        public static CtbSubRubros SubRubrosObtenerDatosCompletos(CtbSubRubros pParametro)
        {
            return new CtbSubRubrosLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<CtbSubRubros> SubRubrosObtenerListar(CtbSubRubros pParametro)
        {
            return new CtbSubRubrosLN().ObtenerListaFiltro(pParametro);
        }

        public static List<CtbSubRubros> SubRubrosObtenerListaFiltro(CtbSubRubros pParametro)
        {
            return new CtbSubRubrosLN().ObtenerListaFiltro(pParametro);
        }

        public static bool SubRubrosAgregar(CtbSubRubros pParametro)
        {
            return new CtbSubRubrosLN().Agregar(pParametro);
        }

        public static bool SubRubrosModificar(CtbSubRubros pParametro)
        {
            return new CtbSubRubrosLN().Modificar(pParametro);
        }

        #endregion

        #region CuentasContables

        public static CtbCuentasContables CuentasContablesObtenerDatosCompletos(CtbCuentasContables pParametro)
        {
            return new CtbCuentasContablesLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<CtbCuentasContables> CuentasContablesObtenerListar(CtbCuentasContables pParametro)
        {
            return new CtbCuentasContablesLN().ObtenerListaFiltro(pParametro);
        }

        public static List<CtbCuentasContables> CuentasContablesObtenerListaFiltro(CtbCuentasContables pParametro)
        {
            return new CtbCuentasContablesLN().ObtenerListaFiltro(pParametro);
        }

        public static List<CtbCuentasContables> CuentasContablesObtenerListaFiltroPopUp(CtbCuentasContables pParametro)
        {
            return new CtbCuentasContablesLN().ObtenerListaFiltroPopUp(pParametro);
        }

        public static List<CtbCuentasContables> CuentasContablesObtenerPorEjercicioNumeroCuenta(CtbAsientosContables pParametro)
        {
            return new CtbCuentasContablesLN().ObtenerPorEjercicioNumeroCuenta(pParametro);
        }
        public static bool CuentasContablesAgregar(CtbCuentasContables pParametro)
        {
            return new CtbCuentasContablesLN().Agregar(pParametro);
        }

        public static bool CuentasContablesModificar(CtbCuentasContables pParametro)
        {
            return new CtbCuentasContablesLN().Modificar(pParametro);
        }

        public static CtbCuentasContables CuentasContablesObtenerImputacion(CtbCuentasContables pParametro)
        {
            return new CtbCuentasContablesLN().ObtenerImputacion(pParametro);
        }

        public static List<CtbCuentasContables> CuentasContablesObtenerListaRama()
        {
            return new CtbCuentasContablesLN().ObtenerListaRama();
        }

        public static CtbCuentasContables CuentasContablesObtenerSeleccionarRama(CtbCuentasContables pParametro)
        {
            return new CtbCuentasContablesLN().ObtenerSeleccionarRama(pParametro);
        }

        /// <summary>
        /// Devuelve las cuentas contables de una Rama
        /// </summary>
        /// <param name="pParametro">IdCuentaContableRama</param>
        /// <returns></returns>
        public static List<CtbCuentasContables> CuentasContablesObtenerPorRama(CtbCuentasContables pParametro)
        {
            return new CtbCuentasContablesLN().ObtenerPorRama(pParametro);
        }

        public static List<CtbCuentasContables> CuentasContablesObtenerListaRamaPorIdEjercicio(CtbCuentasContables pParametro)
        {
            return new CtbCuentasContablesLN().ObtenerListaRamaPorIdEjercicio(pParametro);
        }

        public static DataTable CtbCuentasContablesObenterImputablesPlantilla(CtbCuentasContables pParametro)
        {
            return new CtbCuentasContablesLN().ObenterCuentasContablesImputables(pParametro);
        }

        #endregion

        #region AsientosModelos

        public static CtbAsientosModelos AsientosModelosObtenerDatosCompletos(CtbAsientosModelos pParametro)
        {
            return new CtbAsientosModelosLN().ObtenerDatosCompletos(pParametro);
        }

        /// <summary>
        /// Devuelve un Asiento Modelo por Tipo de Operacion
        /// </summary>
        /// <param name="pParametro">IdTipoOperacion</param>
        /// <param name="db"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static CtbAsientosModelos AsientosModelosObtenerDatosCompletos(CtbAsientosContables pParametro, Database db, DbTransaction tran)
        {
            return new CtbAsientosModelosLN().ObtenerDatosCompletos(pParametro, db, tran);
        }

        public static List<CtbAsientosModelos> AsientosModelosObtenerListar(CtbAsientosModelos pParametro)
        {
            return new CtbAsientosModelosLN().ObtenerLista(pParametro);
        }

        public static List<CtbAsientosModelos> AsientosModelosObtenerListaFiltro(CtbAsientosModelos pParametro)
        {
            return new CtbAsientosModelosLN().ObtenerListaFiltro(pParametro);
        }

        public static bool AsientosModelosAgregar(CtbAsientosModelos pParametro)
        {
            return new CtbAsientosModelosLN().Agregar(pParametro);
        }

        public static bool AsientosModelosModificar(CtbAsientosModelos pParametro)
        {
            return new CtbAsientosModelosLN().Modificar(pParametro);
        }

        /// <summary>
        /// Agrega un Asiento Detalle al Asiento Contable
        /// </summary>
        /// <param name="pAsiento"></param>
        /// <param name="pImporte"></param>
        /// <param name="pModelo"></param>
        /// <param name="pCodigoAsientoModelo"></param>
        /// <returns></returns>
        public static bool AsientosModelosObtenerAsientoDetalle(CtbAsientosContables pAsiento, decimal pImporte, CtbAsientosModelos pModelo, EnumCodigosAsientosModelos pCodigoAsientoModelo)
        {
            return new CtbAsientosModelosLN().ObtenerAsientoDetalle(pAsiento, pImporte, pModelo, pCodigoAsientoModelo);
        }

        /// <summary>
        /// Agrega un Asiento Detalle al Asiento Contable
        /// </summary>
        /// <param name="pAsiento"></param>
        /// <param name="pImporte"></param>
        /// <param name="pModelo"></param>
        /// <param name="pCodigoAsientoModelo"></param>
        /// <param name="pFilial"></param>
        /// <param name="pTipoValor"></param>
        /// <param name="pIdBancoCuenta"></param>
        /// <returns></returns>
        public static bool AsientosModelosObtenerAsientoDetalle(CtbAsientosContables pAsiento, decimal pImporte, CtbAsientosModelos pModelo, EnumCodigosAsientosModelos pCodigoAsientoModelo, TGEFiliales pFilial, int pIdTipoValor, int pIdBancoCuenta, Database bd, DbTransaction tran)
        {
            return new CtbAsientosModelosLN().ObtenerAsientoDetalle(pAsiento, pImporte, pModelo, pCodigoAsientoModelo, pFilial, pIdTipoValor, pIdBancoCuenta, bd, tran);
        }

        public static bool AsientosModelosObtenerAsientoDetalle(CtbAsientosContables pAsiento, decimal pImporte, CtbAsientosModelos pModelo, EnumCodigosAsientosModelos pCodigoAsientoModelo, CtbCuentasContables pCuentaContable)
        {
            return new CtbAsientosModelosLN().ObtenerAsientoDetalle(pAsiento, pImporte, pModelo, pCodigoAsientoModelo, pCuentaContable, pCuentaContable.CentroCostoProrrateo);
        }

        public static bool AsientosModelosObtenerAsientoDetalle(CtbAsientosContables pAsiento, decimal pImporte, CtbAsientosModelos pModelo, EnumCodigosAsientosModelos pCodigoAsientoModelo, CtbCuentasContables pCuentaContable, CtbCentrosCostosProrrateos pCentroCosto)
        {
            return new CtbAsientosModelosLN().ObtenerAsientoDetalle(pAsiento, pImporte, pModelo, pCodigoAsientoModelo, pCuentaContable, pCentroCosto);
        }

        public static bool AsientosModelosObtenerAsientoDetalle(CtbAsientosContables pAsiento, decimal pImporte, CtbAsientosModelos pModelo, EnumCodigosAsientosModelos pCodigoAsientoModelo, TGEListasValoresSistemasDetalles pListaValorSistemaDetalle, Database bd, DbTransaction tran)
        {
            return new CtbAsientosModelosLN().ObtenerAsientoDetalle(pAsiento, pImporte, pModelo, pCodigoAsientoModelo, pListaValorSistemaDetalle, bd, tran);
        }
        #endregion

        #region AsientosContables

        public static CtbAsientosContables AsientosContablesObtenerDatosCompletos(CtbAsientosContables pParametro)
        {
            return new CtbAsientosContablesLN().ObtenerDatosCompletos(pParametro);
        }

        /// <summary>
        /// Devuleve un Asiento Contable por Tipo de Operacion
        /// </summary>
        /// <param name="pParametro">IdTipoOperacion, IdRefTipoOperacion</param>
        /// <returns></returns>
        public static CtbAsientosContables AsientosContablesObtenerDatosCompletosPorTipoOperacion(CtbAsientosContables pParametro)
        {
            return new CtbAsientosContablesLN().ObtenerDatosCompletosPorTipoOperacion(pParametro);
        }

        /// <summary>
        /// Devuleve un Asiento Contable por Tipo de Operacion
        /// </summary>
        /// <param name="pParametro">IdTipoOperacion, IdRefTipoOperacion</param>
        /// <returns></returns>
        public static CtbAsientosContables AsientosContablesObtenerDatosCompletosPorTipoOperacion(CtbAsientosContables pParametro, Database db, DbTransaction tran)
        {
            return new CtbAsientosContablesLN().ObtenerDatosCompletosPorTipoOperacion(pParametro, db, tran);
        }

        public static List<CtbAsientosContables> AsientosContablesObtenerListar(CtbAsientosContables pParametro)
        {
            return new CtbAsientosContablesLN().ObtenerLista(pParametro);
        }

        public static List<CtbAsientosContables> AsientosContablesObtenerListaFiltro(CtbAsientosContables pParametro)
        {
            return new CtbAsientosContablesLN().ObtenerListaFiltro(pParametro);
        }

        public static List<CtbAsientosContablesDetalles> AsientosContablesDetallesArmarAsientoCierre(CtbAsientosContables pParametro)
        {
            return new CtbAsientosContablesLN().ArmarAsientoCierre(pParametro);
        }

        public static bool AsientosContablesAgregar(CtbAsientosContables pParametro)
        {
            return new CtbAsientosContablesLN().Agregar(pParametro);
        }

        public static bool AsientosContablesAgregar(CtbAsientosContables pParametro, Database bd, DbTransaction tran)
        {
            return new CtbAsientosContablesLN().Agregar(pParametro, bd, tran);
        }

        public static bool AsientosContablesModificar(CtbAsientosContables pParametro)
        {
            return new CtbAsientosContablesLN().Modificar(pParametro);
        }

        public static bool AsientosContablesModificar(CtbAsientosContables pParametro, Database bd, DbTransaction tran)
        {
            return new CtbAsientosContablesLN().Modificar(pParametro, bd, tran);
        }

        public static CtbAsientosContables AsientosContablesObtenerNumeroAsiento(CtbAsientosContables pParametro)
        {
            return new CtbAsientosContablesLN().ObtenerNumberoAsiento(pParametro);
        }

        public static bool AsientosContablesRevertirAsiento(CtbAsientosContables anular, TGETiposOperaciones pTipoOperacionAnular, Database bd, DbTransaction tran)
        {
            return new CtbAsientosContablesLN().RevertirAsientoContable(anular, pTipoOperacionAnular, bd, tran);
        }

        public static DataTable AsientosContablesObtenerListaFiltroGrilla(CtbAsientosContables pParametro)
        {
            return new CtbAsientosContablesLN().ObtenerListaFiltroGrilla(pParametro);
        }

        public static DataTable AsientosContablesObtenerLibroMayorGrilla(CtbCuentasContables pParametro)
        {
            return new CtbAsientosContablesLN().ObtenerLibroMayorGrilla(pParametro);
        }

        #endregion

        #region BienesUsos

        public static CtbBienesUsos BienesUsosObtenerDatosCompletos(CtbBienesUsos pParametro)
        {
            return new CtbBienesUsosLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<CtbBienesUsos> BienesUsosObtenerListar(CtbBienesUsos pParametro)
        {
            return new CtbBienesUsosLN().ObtenerLista(pParametro);
        }

        public static DataTable BienesUsosObtenerListaGrilla(CtbBienesUsos pParametro)
        {
            return new CtbBienesUsosLN().ObtenerListaGrilla(pParametro);
        }

        public static List<CtbBienesUsos> BienesUsosObtenerListaFiltro(CtbBienesUsos pParametro)
        {
            return new CtbBienesUsosLN().ObtenerListaFiltro(pParametro);
        }

        public static bool BienesUsosAgregar(CtbBienesUsos pParametro)
        {
            return new CtbBienesUsosLN().Agregar(pParametro);
        }

        public static bool BienesUsosModificar(CtbBienesUsos pParametro)
        {
            return new CtbBienesUsosLN().Modificar(pParametro);
        }

        #endregion

        #region PeriodosContables

        public static CtbPeriodosContables PeriodosContablesObtenerDatosCompletos(CtbPeriodosContables pParametro)
        {
            return new CtbPeriodosContablesLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<CtbPeriodosContables> PeriodosContablesObtenerListar(CtbPeriodosContables pParametro)
        {
            return new CtbPeriodosContablesLN().ObtenerListaFiltro(pParametro);
        }

        public static List<CtbPeriodosContables> PeriodosContablesObtenerListaFiltro(CtbPeriodosContables pParametro)
        {
            return new CtbPeriodosContablesLN().ObtenerListaFiltro(pParametro);
        }

        public static bool PeriodosContablesAgregar(CtbPeriodosContables pParametro)
        {
            return new CtbPeriodosContablesLN().Agregar(pParametro);
        }

        public static bool PeriodosContablesModificar(CtbPeriodosContables pParametro)
        {
            return new CtbPeriodosContablesLN().Modificar(pParametro);
        }

        public static CtbPeriodosContables PeriodosContablesObtenerUltimoCerrado(CtbPeriodosContables pParametro)
        {
            return new CtbPeriodosContablesLN().ObtenerUltimoCerrado(pParametro);
        }

        /// <summary>
        /// Valida si un Periodo esta Cerrado
        /// Devuelve True si esta Cerrado
        /// </summary>
        /// <param name="pParametro">Periodo</param>
        /// <returns></returns>
        //public static bool PeriodosContablesValidarCierre(CtbPeriodosContables pParametro)
        //{
        //    return new CtbPeriodosContablesLN().ValidarCierre(pParametro);
        //}

        public static CtbPeriodosContables PeriodosContablesObtenerUltimoCerrado()
        {
            return new CtbPeriodosContablesLN().ObtenerUltimoCerrado();
        }

        public static bool PeriodosContablesValidarCierre(CtbPeriodosContables pParametro)
        {
            return new CtbPeriodosContablesLN().ValidarCierre(pParametro);
        }
        #endregion

        #region PeriodosIvas

        public static CtbPeriodosIvas PeriodosIvasObtenerDatosCompletos(CtbPeriodosIvas pParametro)
        {
            return new CtbPeriodosIvasLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<CtbPeriodosIvas> PeriodosIvasObtenerListar(CtbPeriodosIvas pParametro)
        {
            return new CtbPeriodosIvasLN().ObtenerListaFiltro(pParametro);
        }

        public static List<CtbPeriodosIvas> PeriodosIvasObtenerListaFiltro(CtbPeriodosIvas pParametro)
        {
            return new CtbPeriodosIvasLN().ObtenerListaFiltro(pParametro);
        }

        public static bool PeriodosIvasAgregar(CtbPeriodosIvas pParametro)
        {
            return new CtbPeriodosIvasLN().Agregar(pParametro);
        }

        public static bool PeriodosIvasModificar(CtbPeriodosIvas pParametro)
        {
            return new CtbPeriodosIvasLN().Modificar(pParametro);
        }

        public static CtbPeriodosIvas PeriodosIvasObtenerUltimoCerrado(CtbPeriodosIvas pParametro)
        {
            return new CtbPeriodosIvasLN().ObtenerUltimoCerrado(pParametro);
        }
        public static CtbPeriodosIvas PeriodosIvasObtenerArmarLiquidacionIVA(CtbPeriodosIvas pParametro)
        {
            return new CtbPeriodosIvasLN().ObtenerArmarLiquidacionIVA(pParametro);
        }


        /// <summary>
        /// Valida si un Periodo esta Cerrado
        /// Devuelve True si esta Cerrado
        /// </summary>
        /// <param name="pParametro">Periodo</param>
        /// <returns></returns>
        public static bool PeriodosIvasValidarCierre(CtbPeriodosIvas pParametro)
        {
            return new CtbPeriodosIvasLN().ValidarCierre(pParametro);
        }

        public static CtbPeriodosIvas PeriodosIvasObtenerUltimoCerrado()
        {
            return new CtbPeriodosIvasLN().ObtenerUltimoCerrado();
        }
        #endregion

        #region ValoresSistemasCuentasContables

        public static bool ValoresSistemasCuentasContablesAgregar(TGEListasValoresSistemasDetallesCuentasContables pParametro)
        {
            return new TGEListasValoresSistemasDetallesCuentasContablesLN().Agregar(pParametro);
        }

        public static bool ValoresSistemasCuentasContablesModificar(TGEListasValoresSistemasDetallesCuentasContables pParametro)
        {
            return new TGEListasValoresSistemasDetallesCuentasContablesLN().Modificar(pParametro);
        }

        public static List<TGEListasValoresSistemasDetallesCuentasContables> ValoresSistemasCuentasContablesObtenerPorIdListaValor(TGEListasValoresSistemas pParametro)
        {
            return new TGEListasValoresSistemasDetallesCuentasContablesLN().ObtenerListaPorIdListaValor(pParametro);
        }

        public static List<TGEListasValoresSistemasDetallesCuentasContables> ValoresSistemasCuentasContablesObtenerListaFiltro(TGEListasValoresSistemasDetallesCuentasContables pParametro)
        {
            return new TGEListasValoresSistemasDetallesCuentasContablesLN().ObtenerListaFiltro(pParametro);
        }

        public static TGEListasValoresSistemasDetallesCuentasContables ValoresSistemasCuentasContablesObtenerDatosCompletos(TGEListasValoresSistemasDetallesCuentasContables pParametro)
        {
            return new TGEListasValoresSistemasDetallesCuentasContablesLN().ObtenerDatosCompletos(pParametro);
        }

        public static TGEListasValoresSistemasDetallesCuentasContables ValoresSistemasCuentasContablesObtenerDatosCompletos(TGEListasValoresSistemasDetalles pParametro, Database bd, DbTransaction tran)
        {
            return new TGEListasValoresSistemasDetallesCuentasContablesLN().ObtenerDatosCompletos(pParametro, bd, tran);
        }

        #endregion

    }
}
