using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CuentasPagar.Entidades;
using CuentasPagar.LogicaNegocio;
using Comunes.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Contabilidad.Entidades;
using System.Data;
using Compras.Entidades;
using Servicio.AccesoDatos;
using Proveedores.Entidades;
using ProcesosDatos.Entidades;

namespace CuentasPagar.FachadaNegocio
{
    public class CuentasPagarF
    {
        #region "Solicitudes de Pago"

        public static DataTable SolicitudPagoObtenerGrilla(CapSolicitudPago pSolicitud)
        {
            return new CapSolicitudPagoLN().ObtenerGrilla(pSolicitud);
        }

        public static List<CapSolicitudPago> SolicitudPagoObtenerListaFiltro(CapSolicitudPago pSolicitud)
        {
            return new CapSolicitudPagoLN().ObtenerListaFiltro(pSolicitud);
        }

        /// <summary>
        /// Devuelve las SP pendiente de Pago
        /// Entidad: Tabla de referencia
        /// IdRefEntidad: Id de la Tabla Referencia
        /// </summary>
        /// <param name="pSolicitud">IdEntidad, IdRefEntidad</param>
        /// <returns></returns>
        public static List<CapSolicitudPago> SolicitudPagoObtenerPendientePago(CapOrdenesPagos pParametro)
        {
            return new CapSolicitudPagoLN().ObtenerPendientePago(pParametro);
        }

        /// <summary>
        /// Devuelve las SP Terceros pendiente de Cobro
        /// </summary>
        /// <param name="pParametro">IdEntidad, IdRefEntidad</param>
        /// <returns></returns>
        public static List<CapSolicitudPago> SolicitudPagoObtenerTercerosPendienteCobro(CapSolicitudPago pParametro)
        {
            return new CapSolicitudPagoLN().ObtenerTercerosPendienteCobro(pParametro);
        }

        public static CapSolicitudPago SolicitudPagoObtenerDatosCompletos(CapSolicitudPago pParametro)
        {
            return new CapSolicitudPagoLN().ObtenerDatosCompletos(pParametro);
        }

        public static bool SolicitudPagoAgregar(CapSolicitudPago pParametro, CmpInformesRecepciones pRemito)
        {
            return new CapSolicitudPagoLN().Agregar(pParametro, pRemito);
        }

        public static bool SolicitudPagoModificar(CapSolicitudPago pParametro)
        {
            return new CapSolicitudPagoLN().Modificar(pParametro);
        }

        public static bool SolicitudPagoAnular(CapSolicitudPago pParametro)
        {
            return new CapSolicitudPagoLN().Anular(pParametro);
        }

        public static bool SolicitudPagoAutorizar(CapSolicitudPago pParametro)
        {
            return new CapSolicitudPagoLN().Autorizar(pParametro);
        }

        public static bool SolicitudPagoAgregarRemesa(Objeto pObjeto, int pIdRemesa, int pPeriodo, decimal pImporte, Database db, DbTransaction tran)
        {
            return new CapSolicitudPagoLN().AgregarSPRemesa(pObjeto, pIdRemesa, pPeriodo, pImporte, db, tran);
        }

        public static DataTable SolicitudPagoCargaMasivaSeleccionarTabla(CapSolicitudPago pParametro)
        {
            return new CapSolicitudPagoLN().CargaMasivaSeleccionarTabla(pParametro);
        }

        public static bool SolicitudPagoCargaMasivaAgregar(DataTable e, CapSolicitudPago pParametro)
        {
            return new CapSolicitudPagoLN().AgregarCargaMasiva(e, pParametro);
        }

        public static List<CapSolicitudPago> SolicitudPagosObtenerComboAsociados(CapSolicitudPago pParametro)
        {
            return new CapSolicitudPagoLN().ObtenerComboAsociados(pParametro);
        }

        #region Subsidios
        public static bool SolicitudPagoSubsidioObtenerImporteLiquidacion(CapSolicitudPago pParametro)
        {
            return new CapSolicitudPagoLN().ObtenerImporteLiquidacion(pParametro);
        }

        #endregion
        #endregion

        #region "Ordenes de Pago"
        public static CapOrdenesPagos OrdenesPagosObtenerDatosCompletos(CapOrdenesPagos pParametro)
        {
            return new CapOrdenesPagosLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<CapOrdenesPagos> OrdenesPagosObtenerListaFiltro(CapOrdenesPagos pParametro)
        {
            return new CapOrdenesPagosLN().ObtenerListaFiltro(pParametro);
        }

        public static DataTable OrdenesPagosObtenerListaFiltroGrilla(CapOrdenesPagos pParametro)
        {
            return new CapOrdenesPagosLN().ObtenerListaFiltroGrilla(pParametro);
        }

        public static DataTable OrdenesPagosObtenerListaFiltroGrillaTurismo(CapOrdenesPagos pParametro)
        {
            return new CapOrdenesPagosLN().ObtenerListaFiltroGrillaTurismo(pParametro);
        }
        public static DataTable OrdenesPagosObtenerListaFiltroPagoTurismo(CapOrdenesPagos pParametro)
        {
            return new CapOrdenesPagosLN().ObtenerListaFiltroPagoTurismo(pParametro);
        }

        public static bool OrdenesPagosAgregar(CapOrdenesPagos pParametro)
        {
            return new CapOrdenesPagosLN().Agregar(pParametro);
        }

        public static bool OrdenesPagosAutorizar(CapOrdenesPagos pParametro)
        {
            return new CapOrdenesPagosLN().Autorizar(pParametro);
        }

        public static bool OrdenesPagosAnular(CapOrdenesPagos pParametro)
        {
            return new CapOrdenesPagosLN().Anular(pParametro);
        }

        public static bool OrdenesPagosAnularPagada(CapOrdenesPagos pParametro)
        {
            return new CapOrdenesPagosLN().AnularPagada(pParametro);
        }

        public static bool OrdenesPagosModificar(CapOrdenesPagos pParametro)
        {
            return new CapOrdenesPagosLN().Modificar(pParametro);
        }

        /// <summary>
        /// Confirma un movimiento de Pagos
        /// Metodo para los movimientos de Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static bool OrdenesPagosConfirmarMovimiento(Objeto pParametro, DateTime pFecha, Database bd, DbTransaction tran)
        {
            return new CapOrdenesPagosLN().ConfirmarMovimiento(pParametro, pFecha, bd, tran);
        }

        public static CapOrdenesPagosValores OrdenesPagosObtenerCbu(CapOrdenesPagos pParametro)
        {
            return new CapOrdenesPagosLN().ObtenerCbu(pParametro);
        }
        public static CapOrdenesPagosValores OrdenesPagosObtenerDatosCbu(CapOrdenesPagosValores pParametro)
        {
            return new CapOrdenesPagosLN().ObtenerDatosCbu(pParametro);
        }
        #endregion

        #region Retenciones

        public static List<CapOrdenesPagosTiposRetenciones> OrdenesPagosRetencionesObtenerCalculosRetenciones(CapOrdenesPagos pParametro)
        {
            return new CapOrdenesPagosTiposRetencionesLN().ObtenerCalculosRetenciones(pParametro);
        }

        #endregion

        #region "Solicitud de Pago Detalle"

        public static CapSolicitudPagoDetalles SolicitudPagoDetallesObtenerDatosCompletos(CapSolicitudPagoDetalles pParametro)
        {
            return new CapSolicitudPagoDetalleLN().ObtenerDatosCompletos(pParametro);
        }

        public static DataTable SolicitudPagoDetalleObtenerItemsBienesUsoGrilla(CapSolicitudPagoDeatallesSolicitudPago pParametro)
        {
            return new CapSolicitudPagoDetalleLN().ObtenerItemsBienesUsoGrilla(pParametro);
        }

        #endregion

        #region "Solicitudes Pagos Anticipos"
        public static bool SolicitudPagoAnticipoAgregar(CapSolicitudPago pParametro)
        {
            return new CapSolicitudPagoLN().AgregarAnticipo(pParametro);
        }

        public static CapSolicitudPago SolicitudPagoAnticipoObtenerDatosCompletos(CapSolicitudPago pParametro)
        {
            return new CapSolicitudPagoLN().ObtenerDatosCompletosAnticipos(pParametro);
        }

        public static List<CapSolicitudPago> SolicitudPagoAnticipoObtenerPendientesPorProveedor(CapOrdenesPagos pParametro)
        {
            return new CapSolicitudPagoLN().ObtenerAnticiposPendientesPorProveedor(pParametro);
        }

        #endregion

        #region COmprobantes Internos
        public static List<CapSolicitudPago> ComprobantesInternosSeleccionarPorFechaDesde(CtbPeriodosIvas pParametro)
        {
            return new CapSolicitudPagoLN().ComprobantesInternosSeleccionarPorFechaDesde(pParametro);
        }

        public static bool ComprobantesInternosModificar(List<CapSolicitudPago> pParametro, Objeto objeto)
        {
            return new CapSolicitudPagoLN().ModificarComprobantesInternos(pParametro, objeto);
        }
        #endregion

        #region Turismo
        public static DataTable SolicitudPagoObtenerAnticiposReservasTurismoPendientes(CapProveedores pParametro)
        {
            return new CapSolicitudPagoLN().ObtenerAnticiposReservasTurismoPendientes(pParametro);
        }
        public static bool SolicitudPagoAgregarAnticiposTurismo(Objeto pResultado, DataTable Datos)
        {
            return new CapSolicitudPagoLN().AgregarAnticiposTurismo(pResultado, Datos);
        }

        public static DataTable SolicitudPagoCargaMasivaImportarArchivo(SisProcesosProcesamiento pParametro)
        {
            return new CapOrdenesPagosLN().ImportarArchivo(pParametro);
        }

        public static DataTable SolicitudPagoObtenerGrillaAnticipoReservas(CapSolicitudPago pParametro)
        {
            return new CapSolicitudPagoLN().ObtenerGrillaAnticipoReservas(pParametro);
        }

        public static bool SolicitudPagoReimputarAnticipo(CapSolicitudPago pParametro)
        {
            return new CapSolicitudPagoLN().ReimputarAnticipo(pParametro);
        }
        #endregion
    }
}
