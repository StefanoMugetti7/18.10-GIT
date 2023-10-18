using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prestamos.Entidades;
using Prestamos.LogicaNegocio;
using Afiliados.Entidades;
using Comunes.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Generales.Entidades;
using Cargos.Entidades;
using Contabilidad.Entidades;
using System.Data;
using System.Net.Mail;

namespace Prestamos
{
    public class PrePrestamosF
    {
        #region Cesionarios
        public static PreCesionarios CesionariosObtenerDatosCompletos(PreCesionarios pParametro)
        {
            return new PreCesionariosLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<PreCesionarios> CesionariosObtenerListaFiltro(PreCesionarios pParametro)
        {
            return new PreCesionariosLN().ObtenerListaFiltro(pParametro);
        }

        public static bool CesionariosAgregar(PreCesionarios pParametro)
        {
            return new PreCesionariosLN().Agregar(pParametro);
        }

        public static bool CesionariosModificar(PreCesionarios pParametro)
        {
            return new PreCesionariosLN().Modificar(pParametro);
        }
        #endregion

        #region Cesiones
        public static PrePrestamosCesiones PrestamosCesionesObtenerDatosCompletos(PrePrestamosCesiones pParametro)
        {
            return new PrePrestamosCesionesLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<PrePrestamosCesiones> PrestamosCesionesObtenerListaFiltro(PrePrestamosCesiones pParametro)
        {
            return new PrePrestamosCesionesLN().ObtenerListaFiltro(pParametro);
        }

        public static bool PrestamosCesionesAgregar(PrePrestamosCesiones pParametro)
        {
            return new PrePrestamosCesionesLN().Agregar(pParametro);
        }

        public static List<PrePrestamosCesionesDetalles> PrestamosCesionesObtenerPrestamosDisponibles(PrePrestamosCesionesDetalles pParametro)
        {
            return new PrePrestamosCesionesLN().ObtenerPrestamosDisponibles(pParametro);
        }

        public static bool PrestamosCesionesModificar(PrePrestamosCesiones pParametro)
        {
            return new PrePrestamosCesionesLN().Modificar(pParametro);
        }

        public static bool PrestamosCesionesAutorizar(PrePrestamosCesiones pParametro)
        {
            return new PrePrestamosCesionesLN().Autorizar(pParametro);
        }

        public static bool PrestamosCesionesCalcularVAN(PrePrestamosCesiones pParametro)
        {
            return new PrePrestamosCesionesLN().CalcularVAN(pParametro);
        }

        #endregion

        #region  Prestamos

        public static DataTable PrestamosObtenerDocumentosAsociados(PrePrestamos pParametro)
        {
            return new PrePrestamosLN().ObtenerDocumentosAsociados(pParametro);
        }

        public static DataTable PrestamosObtenerCobrosPorPrestamo(PrePrestamos pParametro)
        {
            return new PrePrestamosLN().ObtenerCobrosPorPrestamo(pParametro);
        }

        public static DataTable PrestamosObtenerPorAfiliado(AfiAfiliados pParametro)
        {
            return new PrePrestamosLN().ObtenerPorAfiliado(pParametro);
        }

        public static DataTable PrestamosObtenerPorAfiliadoGeneral (PrePrestamos prePrestamos)
        {
            return new PrePrestamosLN().ObtenerPorAfiliadoGeneral(prePrestamos);
        }

        /// <summary>
        /// Devuelve una lista de Prestamos por Afiliado para Cancelar
        /// </summary>
        /// <param name="pAfiliado"></param>
        /// <returns></returns>
        public static List<PrePrestamos> PrestamosObtenerPorAfiliadoCancelacion(PrePrestamos pParametro)
        {
            return new PrePrestamosLN().ObtenerPorAfiliadoCancelacion(pParametro);
        }

        public static List<PrePrestamosCuotas> PrestamosCuotasObtenerCancelacionAnticipada(AfiAfiliados pAfiliado)
        {
            return new PrePrestamosLN().ObtenerCancelacionAnticipada(pAfiliado);
        }

        /// <summary>
        /// Devuelve una lista de Cargos Pendientes de Cobros para incluir en un Prestamo
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static List<CarCuentasCorrientes> PrestamosObtenerCargosPendientes(CarCuentasCorrientes pParametro)
        {
            return new PrePrestamosLN().ObtenerPendientesCobro(pParametro);
        }

        public static PrePrestamos PrestamosObtenerDatosCompletos(PrePrestamos pPrestamo)
        {
            return new PrePrestamosLN().ObtenerDatosCompletos(pPrestamo);
        }

        public static DataTable PrestamosHabilitarControlesCancelaciones(PrePrestamos pPrestamo)
        {
            return new PrePrestamosLN().HabilitarControlesCancelaciones(pPrestamo);
        }

        /// <summary>
        /// Devuelve un Prestamos Datos Completos con el Imorte Cancelacion
        /// </summary>
        /// <param name="pPrestamo"></param>
        /// <returns></returns>
        public static PrePrestamos PrestamosObtenerDatosCancelacion(PrePrestamos pPrestamo)
        {
            return new PrePrestamosLN().ObtenerDatosCancelacion(pPrestamo);
        }

        /// <summary>
        /// Arma la cuponera y gastos de un prestamo en el objeto
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static bool PrestamosAgregarPrevio(PrePrestamos pPrestamo)
        {
            return new PrePrestamosLN().AgregarPrevio(pPrestamo);
        }

        public static List<PrePrestamos> PrestamosObtenerLista(PrePrestamos pPrestamo)
        {
            return new PrePrestamosLN().ObtenerListaFiltro(pPrestamo);
        }

        public static bool PrestamosAgregar(PrePrestamos pPrestamo)
        {
            return new PrePrestamosLN().Agregar(pPrestamo);
        }

        public static bool PrestamosAgregar(PrePrestamos pPrestamo, Database db, DbTransaction tran)
        {
            return new PrePrestamosLN().Agregar(pPrestamo, db, tran);
        }

        public static bool PrestamosModificar(PrePrestamos pPrestamo)
        {
            return new PrePrestamosLN().Modificar(pPrestamo);
        }

        public static bool PrestamosAnularConfirmado(PrePrestamos pParametro)
        {
            return new PrePrestamosLN().AnularConfirmado(pParametro);
        }

        public static bool PrestamosAplicarCheque(PrePrestamos pParametro)
        {
            return new PrePrestamosLN().AplicarCheque(pParametro);
        }

        //ESTE METODO SE USA PARA ANULAR UN PRESTAMO EN ORDEN DE COBRO
        public static bool PrestamosAnular(PrePrestamos pParametro, Database db, DbTransaction tran)
        {
            return new PrePrestamosLN().AnularPrestamo(pParametro,db,tran);
        }
        /// <summary>
        /// Confirma un Prestamos
        /// Metodo para los movimientos de Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static bool PrestamosConfirmar(Objeto pParametro, DateTime pFecha, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            return new PrePrestamosLN().Confirmar(pParametro, pFecha, pValoresImportes, bd, tran);
        }

        /// <summary>
        /// Confirma una Cancelacion de Prestamo
        /// Metodo para los movimientos de Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static bool PrestamosCancelarConfirmar(Objeto pParametro, DateTime pFecha, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            return new PrePrestamosLN().CancelarConfirmar(pParametro, pFecha, pValoresImportes, false, bd, tran);
        }

        /// <summary>
        /// Proceso para Marcar Cuotas como Cobradas en estado Cobrado en Cuenta Corriente
        /// Actualiza la Cuota y el Prestamo en finalizado si estan todas cobradas
        /// </summary>
        /// <param name="pObjeto"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static bool PrestamosCuotasActualizar(Objeto pObjeto, Database bd, DbTransaction tran)
        {
            return new PrePrestamosLN().PrestamosCuotasActualizar(pObjeto, bd, tran);
        }

        /// <summary>
        /// Calcula el ImporteCancelacion de un Prestamo
        /// </summary>
        /// <param name="pParametro"></param>
        public static void PrestamosCalcularImporteCancelar(PrePrestamos pParametro, bool pValidarIncluir)
        {
            new PrePrestamosLN().CalcularImporteCancelar(pParametro, pValidarIncluir);
        }

        /// <summary>
        /// Arma el Asiento de un Prestamo en memoria
        /// </summary>
        /// <param name="asiento"></param>
        /// <param name="pParametro"></param>
        /// <param name="pValoresImportes"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static bool AsientoOtorgamientoArmar(CtbAsientosContables asiento, PrePrestamos pParametro, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            return new InterfazContableLN().AsientoOtorgamientoArmar(asiento, pParametro, pValoresImportes, bd, tran);
        }

        public static bool PrestamosArmarMailLinkFirmarDocumento(PrePrestamos pParametro, MailMessage mail)
        {
            return new PrePrestamosLN().ArmarMailLinkFirmarDocumento(pParametro, mail);
        }

        /*Obtiene los datos precargados. Se usa para los vendedores que solo van a poder ciertos campos.*/
        public static PrePrestamos PrestamosObtenerDatosPreCargados(PrePrestamos pParametro)
        {
            return new PrePrestamosLN().ObtenerDatosPreCargados(pParametro);
        }
        #endregion

        #region Prestamos IPS CAD
        public static DataTable PrePrestamosIpsCadAutorizacionesSeleccionarGrilla(PrePrestamosIpsCadAutorizaciones pParametro)
        {
            return new PrePrestamosIpsCadAutorizacionesLN().ObtenerSeleccionarGrilla(pParametro);
        }

        public static List<PrePrestamosIpsCadAutorizaciones> PrePrestamosIpsCadAutorizacionesObtenerPeriodos()
        {
            return new PrePrestamosIpsCadAutorizacionesLN().ObtenerPeriodos();
        }

        public static PrePrestamosIpsCadAutorizaciones PrePrestamosIpsCadAutorizacionesObtenerDatosCompletos(PrePrestamosIpsCadAutorizaciones pParametro)
        {
            return new PrePrestamosIpsCadAutorizacionesLN().ObtenerDatosCompletos(pParametro);
        }

        public static PrePrestamosIpsPlanes PrePrestamosIpsPlanesObtenerDatosCompletos(PrePrestamosIpsPlanes pParametro)
        {
            return new PrePrestamosIpsPlanesLN().ObtenerDatosCompletos(pParametro);
        }
        #endregion

        #region Prestamos Planes

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static PrePrestamosPlanes PrestamosPlanesObtenerDatosCompletos(PrePrestamosPlanes pParametro)
        {
            return new PrePrestamosPlanesLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<PrePrestamosPlanes> PrestamosPlanesObtenerLista(PrePrestamosPlanes pPrestamosPlanes)
        {
            return new PrePrestamosPlanesLN().ObtenerListaFiltro(pPrestamosPlanes);
        }

        public static DataTable PrestamosBancoSolParametrosObtener(PrePrestamosPlanes pParametro)
        {
            return new PrePrestamosPlanesLN().ObtenerPrestamosBancoSolParametros(pParametro);
        }

        /// <summary>
        /// Devuelve la ultima Tasa para el Plan
        /// </summary>
        /// <param name="pParametro">IdPrestamoPlan</param>
        /// <returns></returns>
        public static PrePrestamosPlanesTasas PrestamosPlanesTasasObtenerTasaActiva(PrePrestamosPlanes pParametro)
        {
            return new PrePrestamosPlanesLN().ObtenerTasaActiva(pParametro);
        }

        public static bool PrestamosPlanesAgregar(PrePrestamosPlanes pParametro)
        {
            return new PrePrestamosPlanesLN().Agregar(pParametro);
        }

        public static bool PrestamosPlanesModificar(PrePrestamosPlanes pParametro)
        {
            return new PrePrestamosPlanesLN().Modificar(pParametro);
        }

        #endregion

        #region Simulaciones

        public static List<PrePrestamos> SimulacionObtenerPorAfiliado(AfiAfiliados pParametro)
        {
            return new PreSimulacionesLN().ObtenerPorAfiliado(pParametro);
        }

        public static PrePrestamos SimulacionObtenerDatosCompletos(PrePrestamos pPrestamo)
        {
            return new PreSimulacionesLN().ObtenerDatosCompletos(pPrestamo);
        }

        public static List<PrePrestamos> SimulacionObtenerLista(PrePrestamos pPrestamo)
        {
            return new PreSimulacionesLN().ObtenerListaFiltro(pPrestamo);
        }

        public static bool SimulacionAgregar(PrePrestamos pPrestamo)
        {
            return new PreSimulacionesLN().Agregar(pPrestamo);
        }
        #endregion

        public static DataTable PrestamosCargarCardsBootStrap(PrePrestamos pParametro)
        {
            return new PrePrestamosLN().ObtenerCardsBootStrap(pParametro);
        }

        #region Prestamos Lotes

        public static PrePrestamosLotes PrestamosLotesObtenerDatosCompletos(PrePrestamosLotes pPrestamo)
        {
            return new PrePrestamosLotesLN().ObtenerDatosCompletos(pPrestamo);
        }

        public static DataTable PrestamosLotesObtenerGrilla(PrePrestamosLotes prePrestamos)
        {
            return new PrePrestamosLotesLN().ObtenerGrilla(prePrestamos);
        }

        public static DataTable PrestamosLotesObtenerPorProveedor(PrePrestamosLotes pParametro)
        {
            return new PrePrestamosLotesLN().ObtenerPorProveedor(pParametro);
        }

        public static DataTable PrestamosLotesObtenerPrestamosDisponiblesPorFiltro(PrePrestamosLotes pParametro)
        {
            return new PrePrestamosLotesLN().ObtenerPorFiltro(pParametro);
        }

        public static bool PrestamosLotesAgregar(PrePrestamosLotes pPrestamo)
        {
            return new PrePrestamosLotesLN().Agregar(pPrestamo);
        }

        public static bool PrestamosLotesModificar(PrePrestamosLotes pPrestamo)
        {
            return new PrePrestamosLotesLN().Modificar(pPrestamo);
        }
        #endregion

        #region Cotizaciones

        public static PrePrestamosCotizacionesUnidades PrestamosCotizacionesUnidadesObtenerDatosCompletos(PrePrestamosCotizacionesUnidades pParametro)
        {
            return new PrePrestamosCotizacionesUnidadesLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<PrePrestamosCotizacionesUnidades> PrestamosCotizacionesUnidadesObtenerListaFiltro(PrePrestamosCotizacionesUnidades pParametro)
        {
            return new PrePrestamosCotizacionesUnidadesLN().ObtenerListaFiltro(pParametro);
        }

        public static bool PrestamosCotizacionesUnidadesAgregar(PrePrestamosCotizacionesUnidades pParametro)
        {
            return new PrePrestamosCotizacionesUnidadesLN().Agregar(pParametro);
        }

        public static DataTable PrestamosCotizacionesObtenerListaGrilla(PrePrestamosCotizacionesUnidades pParametro)
        {
            return new PrePrestamosCotizacionesUnidadesLN().ObtenerListaGrilla(pParametro);
        }

        #endregion

        public static List<int> PrestamosObtenerPeriodoPrimerVencimiento(TGEFormasCobrosAfiliados pParametro)
        {
            return new PrePrestamosLN().ObtenerProximosPeriodosPrestamos(pParametro);
        }



    }
}
