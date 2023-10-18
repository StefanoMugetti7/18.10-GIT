using Compras.Entidades;
using Compras.LogicaNegocio;
using Comunes.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Compras
{
    public class ComprasF
    {

        #region Solicitudes Compras

        public static bool ComprasAgregar(CmpSolicitudesCompras pParametro)
        {
            return new CmpSolicitudesComprasLN().Agregar(pParametro);
        }

        public static bool ComprasModificar(CmpSolicitudesCompras pParametro)
        {
            return new CmpSolicitudesComprasLN().Modificar(pParametro);
        }

        public static bool ComprasCotizar(CmpSolicitudesCompras pParametro)
        {
            return new CmpSolicitudesComprasLN().Cotizar(pParametro);
        }

        public static CmpSolicitudesCompras ComprasObtenerDatosCompletos(CmpSolicitudesCompras pParametro)
        {
            return new CmpSolicitudesComprasLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<CmpSolicitudesCompras> ComprasObtenerListaFiltro(CmpSolicitudesCompras pParametro)
        {
            return new CmpSolicitudesComprasLN().ObtenerListaFiltro(pParametro);
        }

        public static bool ComprasAnular(CmpSolicitudesCompras pParametro)
        {
            return new CmpSolicitudesComprasLN().Anular(pParametro);
        }


        public static bool ComprasAutorizar(CmpSolicitudesCompras pParametro)
        {
            return new CmpSolicitudesComprasLN().Autorizar(pParametro);
        }

        #endregion

        #region Familias

        public static CMPFamilias FamiliasObtenerDatosCompletos(CMPFamilias pParametro)
        {
            return new CMPFamiliasLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<CMPFamilias> FamiliasObtenerListaFiltro(CMPFamilias pParametro)
        {
            return new CMPFamiliasLN().ObtenerListaFiltro(pParametro);
        }

        public static bool FamiliasAgregar(CMPFamilias pParametro)
        {
            return new CMPFamiliasLN().Agregar(pParametro);
        }

        public static bool FamiliasModificar(CMPFamilias pParametro)
        {
            return new CMPFamiliasLN().Modificar(pParametro);
        }

        #endregion

        #region Productos     

        public static CMPProductos ProductosObtenerPorIdProducto(CMPProductos pParametro)
        {
            return new CMPProductosLN().ObtenerPorIdProducto(pParametro);
        }

        public static CMPProductos ProductosObtenerPorCodigo(CMPProductos pParametro)
        {
            return new CMPProductosLN().ObtenerPorCodigo(pParametro);
        }

        public static List<CMPProductos> ProductosObtenerListaFiltro(CMPProductos pProducto)
        {
            return new CMPProductosLN().ObtenerListaFiltro(pProducto);
        }
        public static DataTable ProductosObtenerListaCompradores(CMPProductos pProducto)
        {
            return new CMPProductosLN().ObtenerListaCompradores(pProducto);
        }

        public static DataTable ProductosObtenerListaFiltroDT(CMPProductos pProducto)
        {
            return new CMPProductosLN().ObtenerListaFiltroDT(pProducto);
        }

        public static DataTable ProductosObtenerGrilla(CMPProductos pProducto)
        {
            return new CMPProductosLN().ObtenerGrilla(pProducto);
        }

        public static DataTable ObtenerProductosServiciosTurismo(CMPProductos pProducto)
        {
            return new CMPProductosLN().ObtenerProductosServiciosTurismo(pProducto);
        }

        public static bool ProductosAgregar(CMPProductos pParametro)
        {
            return new CMPProductosLN().Agregar(pParametro);
        }

        public static bool ProductosModificar(CMPProductos pParametro)
        {
            return new CMPProductosLN().Modificar(pParametro);
        }

        #endregion

        #region Cotizaciones
        public static bool CotizacionesAgregar(CmpCotizaciones pCotizacion)
        {
            return new CmpCotizacionesLN().Agregar(pCotizacion);
        }

        public static bool CotizacionesAnular(CmpCotizaciones pParametro)
        {
            return new CmpCotizacionesLN().Anular(pParametro);
        }

        public static bool CotizacionesAutorizar(CmpCotizaciones pParametro)
        {
            return new CmpCotizacionesLN().Autorizar(pParametro);
        }

        public static bool CotizacionesModificar(CmpCotizaciones pParametro)
        {
            return new CmpCotizacionesLN().Modificar(pParametro);
        }

        public static CmpCotizaciones CotizacionesObtenerDatosCompletos(CmpCotizaciones pCotizacion)
        {
            return new CmpCotizacionesLN().ObtenerDatosCompletos(pCotizacion);
        }

        public static List<CmpCotizaciones> CotizacionesObtenerListaFiltro(CmpCotizaciones pCotizacion)
        {
            return new CmpCotizacionesLN().ObtenerListaFiltro(pCotizacion);
        }

        public static List<CmpCotizacionesDetalles> CotizacionesObtenerListaFiltroPorProducto(CMPProductos pParametro)
        {
            return new CmpCotizacionesLN().ObtenerListaFiltroPorProducto(pParametro);
        }
        #endregion

        #region Ordenes de Compras

        public static bool OrdenCompraAgregar(CmpOrdenesCompras pOrden)
        {
            return new CmpOrdenesComprasLN().Agregar(pOrden);
        }

        public static bool OrdenCompraModificar(CmpOrdenesCompras pOrden)
        {
            return new CmpOrdenesComprasLN().Modificar(pOrden);
        }

        public static bool OrdenCompraModificarEstado(CmpOrdenesCompras pOrden, Database db, DbTransaction tran)
        {
            return new CmpOrdenesComprasLN().ModificarEstado(pOrden, db, tran);
        }

        public static bool OrdenCompraAnular(CmpOrdenesCompras pOrden)
        {
            return new CmpOrdenesComprasLN().Anular(pOrden);
        }

        public static bool OrdenCompraAutorizar(CmpOrdenesCompras pOrden)
        {
            return new CmpOrdenesComprasLN().Autorizar(pOrden);
        }

        public static CmpOrdenesCompras OrdenCompraObtenerDatosCompletos(CmpOrdenesCompras pOrden)
        {
            return new CmpOrdenesComprasLN().ObtenerDatosCompletos(pOrden);
        }

        public static List<CmpOrdenesCompras> OrdenCompraObtenerLista(CmpOrdenesCompras pOrden)
        {
            return new CmpOrdenesComprasLN().ObtenerListaFiltro(pOrden);
        }

        public static List<CmpOrdenesCompras> OrdenCompraObtenerListaPopUp(CmpOrdenesCompras pOrden)
        {
            return new CmpOrdenesComprasLN().ObtenerListaFiltroPopUp(pOrden);
        }
        public static DataTable OrdenCompraDetalleObtenerListaPopUp(CmpOrdenesCompras pOrden)
        {
            return new CmpOrdenesComprasLN().ObtenerListaDetalleFiltroPopUp(pOrden);
        }
        public static List<CmpOrdenesCompras> OrdenCompraObtenerTercerosAutorizadas(CmpOrdenesCompras pOrden)
        {
            return new CmpOrdenesComprasLN().ObtenerTercerosAutorizadas(pOrden);
        }

        public static List<CmpOrdenesCompras> OrdenCompraObtenerTercerosPendientes(CmpOrdenesCompras pOrden)
        {
            return new CmpOrdenesComprasLN().ObtenerTercerosPendientes(pOrden);
        }

        public static List<CmpSolicitudesComprasDetalles> OrdenCompraObtenerListaSCDPorProveedor(CmpOrdenesCompras pOrden)
        {
            return new CmpOrdenesComprasLN().ObtenerSCDPorProveedor(pOrden);
        }

        public static List<CmpOrdenesComprasDetalles> OrdenCompraDetalleObtenerPendientesSolicitudPago(CmpOrdenesCompras pParametro)
        {
            return new CmpOrdenesComprasLN().ObtenerPendientesSolicitudPago(pParametro);
        }

        public static bool OrdenCompraDetalleActualizarDetalle(CmpOrdenesComprasDetalles pParametro, CmpOrdenesComprasDetalles pValorViejo, Database db, DbTransaction tran)
        {
            return new CmpOrdenesComprasLN().ActualizarDetalle(pParametro, pValorViejo, db, tran);
        }

        public static bool OrdenesComprasConfirmarLista(List<CmpOrdenesCompras> pParametro, Objeto pObjeto)
        {
            return new CmpOrdenesComprasLN().ConfirmarLista(pParametro, pObjeto);
        }

        public static List<CmpOrdenesComprasDetalles> OrdenCompraObtenerDetalles(CmpOrdenesCompras pParametro)
        {
            return new CmpOrdenesComprasLN().ObtenerDetallesPorOrdenCompra(pParametro);
        }
        //public static List<CmpOrdenesComprasDetalles> OrdenCompraObtenerListaOCDPorProveedor(CmpOrdenesCompras pOrden)
        //{
        //    return new CmpOrdenesComprasLN().ObtenerOCDPorProveedor(pOrden);
        //}

        #endregion

        #region Informes de Recepciones

        public static CmpInformesRecepciones InformesRecepcionesObtenerDatosCompletos(CmpInformesRecepciones pParametro)
        {
            return new CmpInformesRecepcionesLN().ObtenerDatosCompletos(pParametro);
        }

        public static bool InformesRecepcionesAgregar(CmpInformesRecepciones pParametro)
        {
            return new CmpInformesRecepcionesLN().Agregar(pParametro);
        }

        public static List<CmpInformesRecepciones> InformesRecepcionesObtenerLista(CmpInformesRecepciones pParametro)
        {
            return new CmpInformesRecepcionesLN().ObtenerListaFiltro(pParametro);
        }

        public static DataTable InformesRecepcionesObtenerListaGrilla(CmpInformesRecepciones pParametro)
        {
            return new CmpInformesRecepcionesLN().ObtenerListaGrilla(pParametro);
        }

        public static bool InformesRecepcionesAnular(CmpInformesRecepciones pParametro)
        {
            return new CmpInformesRecepcionesLN().Anular(pParametro);
        }

        public static List<CmpInformesRecepcionesDetalles> InformesRecepcionesObtenerDetallesPendientesFiltroPorProveedor(CmpInformesRecepciones pParametro)
        {
            return new CmpInformesRecepcionesLN().ObtenerDetallesPendientesFiltroPorProveedor(pParametro);
        }

        public static DataTable InformesRecepcionesObtenerDetallesPendientesRecibirFiltroPorProveedor(CmpInformesRecepciones pParametro)
        {
            return new CmpInformesRecepcionesLN().ObtenerDetallesPendientesRecibirFiltroPorProveedor(pParametro);
        }

        public static DataTable InformesRecepcionesObtenerAcopiosPendientesRecibirFiltroPorProveedor(CmpInformesRecepciones pParametro)
        {
            return new CmpInformesRecepcionesLN().ObtenerAcopiosPendientesRecibirFiltroPorProveedor(pParametro);
        }
        #endregion

        #region Stock
        //public static bool StockAgregar(CMPStock pParametro)
        //{
        //    return new CMPStockLN().Agregar(pParametro);
        //}

        /// <summary>
        /// Agregar o Actualiza un Producto en la Tabla STOCK
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static bool StockAgregarModificar(CMPStock pParametro, Database bd, DbTransaction tran)
        {
            return new CMPStockLN().AgregarModificar(pParametro, bd, tran);
        }

        /// <summary>
        /// Obtiene los datos de un Producto por IdProducto y IdFilial
        /// </summary>
        /// <param name="pParametro">IdProducto, IdFilial</param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        //public static CMPStock StockObtenerDatosCompletos(CMPStock pParametro, Database bd, DbTransaction tran)
        //{
        //    return new CMPStockLN().ObtenerDatosCompletos(pParametro, bd, tran);
        //}

        //public static CMPStock StockObtenerDatosCompletosPorIdProducto(CMPStock pParametro)
        //{
        //    return new CMPStockLN().ObtenerDatosCompletosPorIdProducto(pParametro);
        //}

        public static bool StockMovimientosAgregar(CmpStockMovimientos pParametro)
        {
            return new CmpStockMovimientosLN().Agregar(pParametro);
        }

        public static CmpStockMovimientos StockMovimientosObtenerDatosCompletos(CmpStockMovimientos pParametro)
        {
            return new CmpStockMovimientosLN().ObtenerDatosCompletos(pParametro);
        }

        public static bool StockMovimientosConfirmar(CmpStockMovimientos pParametro)
        {
            return new CmpStockMovimientosLN().Confirmar(pParametro);
        }

        public static bool StockMovimientosAnular(CmpStockMovimientos pParametro)
        {
            return new CmpStockMovimientosLN().Anular(pParametro);
        }

        public static List<CmpStockMovimientos> StockMovimientosListaFiltro(CmpStockMovimientos pParametro)
        {
            return new CmpStockMovimientosLN().ObtenerListaFiltro(pParametro);
        }

        public static CMPStock StockObtenerPorProductoFilial(CMPStock pParametro)
        {
            return new CMPStockLN().ObtenerDatosCompletosPorIdProductoIdFilial(pParametro);
        }

        public static CMPStock StockObtenerDatosCompletos(CMPStock pParametro)
        {
            return new CMPStockLN().ObtenerDatosCompletos(pParametro);
        }

        public static DataTable StockObtenerListaFiltro(CMPStock pParametro)
        {
            return new CMPStockLN().ObtenerGrilla(pParametro);
        }

        #endregion

        #region Listas Precios
        public static CMPListasPrecios ListasPreciosObtenerDatosCompletos(CMPListasPrecios pParametro)
        {
            return new CMPListasPreciosLN().ObtenerDatosCompletos(pParametro);
        }

        public static CMPListasPrecios ListasPreciosObtenerDatosCompletosPopUp(CMPListasPrecios pParametro)
        {
            return new CMPListasPreciosLN().ObtenerDatosCompletosPopUp(pParametro);
        }

        public static bool ListasPreciosModificar(CMPListasPrecios pParametro)
        {
            return new CMPListasPreciosLN().Modificar(pParametro);
        }

        public static bool ListasPreciosAgregar(CMPListasPrecios pParametro)
        {
            return new CMPListasPreciosLN().Agregar(pParametro);
        }

        public static bool ListasPreciosAnular(CMPListasPrecios pParametro)
        {
            return new CMPListasPreciosLN().Anular(pParametro);
        }

        public static DataTable ListasPreciosObtenerPlantilla(CMPListasPrecios pParametro)
        {
            return new CMPListasPreciosLN().ObtenerPlantilla(pParametro);
        }

        public static CMPListasPreciosDetalles ListasPreciosDetallesObtenerDatosPorProducto(CMPListasPreciosDetalles pParametro)
        {
            return new CMPListasPreciosDetallesLN().ObtenerDatosPorProducto(pParametro);
        }

        public static List<CMPListasPreciosDetalles> ListasPreciosDetallesObtenerListaFiltro(CMPListasPreciosDetalles pParametro)
        {
            return new CMPListasPreciosDetallesLN().ObtenerListaFiltro(pParametro);
        }

        public static List<CMPListasPrecios> ListasPreciosObtenerListaFiltro(CMPListasPrecios pParametro)
        {
            return new CMPListasPreciosLN().ObtenerListaFiltro(pParametro);
        }

        public static bool ListasPreciosImportarFamiliasProductosValidaciones(CMPListasPrecios pParametro)
        {
            return new CMPListasPreciosLN().ImportarFamiliasProductosValidaciones(pParametro);
        }
        public static DataTable ListasPreciosImportarFamiliasProductos(CMPListasPrecios pParametro)
        {
            return new CMPListasPreciosLN().ImportarFamiliasProductos(pParametro);
        }
        public static DataTable ListasPreciosDetallesSeleccionar(CMPListasPrecios pParametro)
        {
            return new CMPListasPreciosLN().ListasPreciosDetallesSeleccionar(pParametro);
        }
        public static DataTable CMPListasPreciosDetallesSeleccionarBuscarProducto(CMPListasPreciosDetalles pParametro)
        {
            return new CMPListasPreciosLN().CMPListasPreciosDetallesSeleccionarBuscarProducto(pParametro);
        }
        public static List<CMPListasPrecios> ObtenerListasPrecios(CMPListasPrecios pParametro)
        {
            return new CMPListasPreciosLN().CMPListasPreciosObtener(pParametro);
        }
        #endregion
        #region importar archivos
        public static DataTable cmpStockMovimientosObenterPlantilla(CmpStockMovimientos pParametro)
        {
            return new CmpStockMovimientosLN().ObtenerStockMovimientosPlantilla(pParametro);
        }

        public static DataTable cmpStockMovimientosActualizarStockAlImportarExcel(DataTable dt)
        {
            return new CmpStockMovimientosLN().ActualizarStockAlImportarExcel(dt);
        }


        #endregion
    }
}
