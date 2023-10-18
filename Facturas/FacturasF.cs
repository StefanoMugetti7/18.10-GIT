using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facturas.Entidades;
using Facturas.LogicaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Afiliados.Entidades;
using Generales.Entidades;
using System.Data;
using System.Net.Mail;
using Comunes.Entidades;

namespace Facturas
{
    public class FacturasF
    {

        #region CUENTA CORRIENTE

        public static List<VTACuentasCorrientes> CuentasCorrientesSeleccionarPorCliente(AfiAfiliados pParametro)
        {
            return new VTACuentasCorrientesLN().ObtenerPorCliente(pParametro);
        }

        public static DataTable CuentasCorrientesSeleccionarPorClienteTable(AfiAfiliados pParametro)
        {
            return new VTACuentasCorrientesLN().ObtenerPorClienteTable(pParametro);
        }
        public static DataTable CuentasCorrientesSeleccionarPorClienteIdRefTablaTable(AfiAfiliados pParametro)
        {
            return new VTACuentasCorrientesLN().ObtenerPorClienteIdRefTablaTable(pParametro);
        }
        public static DataTable CuentasCorrientesSeleccionarTiposValoresImportesPorClienteIdRefTablaTable(AfiAfiliados pParametro)
        {
            return new VTACuentasCorrientesLN().ObtenerTiposValoresImportesPorClienteIdRefTablaTable(pParametro);
        }

        #endregion

        #region FACTURAS
        public static DataTable FacturasObtenerGrilla(VTAFacturas pFactura)
        {
            return new VTAFacturasLN().ObtenerGrilla(pFactura);
        }

        public static List<VTAFacturas> FacturasObtenerListaFiltro(VTAFacturas pFactura)
        {
            return new VTAFacturasLN().ObtenerListaFiltro(pFactura);
        }

        public static List<VTAFacturas> FacturasObtenerListaComboAsociadosFiltro(VTAFacturas pFactura)
        {
            return new VTAFacturasLN().ObtenerListaFiltroComboAsociados(pFactura);
        }

        public static VTAFacturas FacturasValidacionesCuentasCorrientesCargosFacturados(VTAFacturas pFactura)
        {
            return new VTAFacturasLN().ValidacionesCuentasCorrientesCargosFacturados(pFactura);
        }

        public static VTAFacturas FacturasObtenerDatosCompletos(VTAFacturas pFactura)
        {
            return new VTAFacturasLN().ObtenerDatosCompletos(pFactura);
        }

        public static VTAFacturas FacturasObtenerDatosPreCargados(VTAFacturas pFactura)
        {
            return new VTAFacturasLN().ObtenerDatosPreCargados(pFactura);
        }
        public static VTAFacturas FacturasSimpleObtenerDatosPreCargados(VTAFacturas pFactura)
        {
            return new VTAFacturasLN().ObtenerDatosPreCargadosSimple(pFactura);
        }
        public static VTAFacturas FacturasObtenerArchivo(VTAFacturas pFactura)
        {
            return new VTAFacturasLN().ObtenerArchivo(pFactura);
        }

        public static bool FacturasAgregar(VTAFacturas pParametro, VTARemitos pRemito)
        {
            return new VTAFacturasLN().Agregar(pParametro, pRemito);
        }

        public static bool FacturasObtenerProximoNumeroComprobanteTmp(VTAFacturas pFactura)
        {
            return new VTAFacturasLN().ObtenerProximoNumeroFacturaTmp(pFactura);
        }

        //public static bool FacturasActualizarPDF(VTAFacturas pParametro)
        //{
        //    return new VTAFacturasLN().ActualizarPDF(pParametro);
        //}

        public static bool FacturasModificar(VTAFacturas pParametro)
        {
            return new VTAFacturasLN().Modificar(pParametro);
        }

        public static bool FacturasAnular(VTAFacturas pParametro)
        {
            return new VTAFacturasLN().Anular(pParametro);
        }

        public static bool FacturasModificarEstado(VTAFacturas pParametro, Database db, DbTransaction tran)
        {
            return new VTAFacturasLN().ModificarEstado(pParametro, db, tran);
        }

        public static bool FacturaElectronicaValidarServicio(VTAFacturas pParametro)
        {
            return new FacturaElectronica.FacturaElectronica().ValidarServicio(pParametro);
        }

        public static bool FacturaElectronicaValidarProximoNumeroComprobante(VTAFacturas pParametro)
        {
            return new FacturaElectronica.FacturaElectronica().ValidarProximoNumeroComprobante(pParametro);
        }

        public static bool FacturaElectronicaValidarComprobanteAfip(Objeto objResulatdo)
        {
            return new VTAFacturasLN().ValidarFE(objResulatdo);
        }

        public static bool FacturaElectronicaConsultarDatosAutorizacion2(VTAFacturas objResulatdo)
        {
            return new FacturaElectronica.FacturaElectronica().ConsultarDatosAutorizacion2(objResulatdo);
        }

        public static DataTable FacturasObtenerDetallesPorIdFactura(VTAFacturas pParametro)
        {
            return new VTAFacturasLN().ObtenerDetallesPorIdFactura(pParametro);
        }

        public static DataTable FacturasObtenerDetallesPendienteEntrega(VTAFacturas pParametro) {
            return new VTAFacturasLN().ObtenerDetallesPendienteEntrega(pParametro);
        }

        public static DataTable FacturasObtenerDetallesPendienteFacturar(VTAFacturas pParametro)
        {
            return new VTAFacturasLN().ObtenerDetallesPendienteFacturar(pParametro);
        }

        public static DataTable FacturasObtenerDetallesAcopiosPendienteEntrega(VTAFacturas pParametro)
        {
            return new VTAFacturasLN().ObtenerDetallesAcopiosPendienteEntrega(pParametro);
        }

        public static bool FacturaArmarMail(VTAFacturas pParametro, MailMessage mail)
        {
            return new VTAFacturasLN().ArmarMailFactura(pParametro, mail);
        }
    

        public static bool FacturaElectronicaConsultarDatos(VTAFacturas pParametro)
        {
            return new FacturaElectronica.FacturaElectronica().ConsultarDatosAutorizacion(pParametro);
        }

        public static List<VTAFacturas> FacturasObtenerComboAsociados(VTAFacturas pFactura)
        {
            return new VTAFacturasLN().ObtenerComboAsociados(pFactura);
        }
        #endregion

        #region FACTURAS LOTES

        public static bool FacturasLotesEnviadosValidaciones(VTAFacturasLotesEnviados pParametro)
        {
            return new VTAFacturasLotesEnviadosLN().Validaciones(pParametro);
        }

        public static List<VTAFacturasLotesEnviados> FacturasLotesEnviadosObtenerCombo()
        {
            return new VTAFacturasLotesEnviadosLN().ObtenerCombo();
        }

        public static DataTable FacturasLotesEnviadosObtenerGrilla(VTAFacturasLotesEnviados pParametro)
        {
            return new VTAFacturasLotesEnviadosLN().ObtenerGrilla(pParametro);
        }

        public static DataTable FacturasLotesEnviadosObtenerFacturasGrilla(VTAFacturasLotesEnviados pParametro)
        {
            return new VTAFacturasLotesEnviadosLN().ObtenerFacturasGrilla(pParametro);
        }

        public static DataSet FacturasLotesEnviadosAgregarDevolverLote(VTAFacturasLotesEnviados pParametro)
        {
            return new VTAFacturasLotesEnviadosLN().AgregarDevolverLote(pParametro);
        }

        public static bool FacturasLotesEnviadosConfirmarLote(VTAFacturasLotesEnviados pParametro)
        {
            return new VTAFacturasLotesEnviadosLN().ConfirmarLote(pParametro);
        }

        public static DataTable FacturasLotesObtenerGrilla(VTAFacturasLotesEnviados pParametro)
        {
            throw new NotImplementedException();
        }

        public static List<TGEListasValoresDetalles> FacturasLotesObtenerCargosPorPeriodo(VTAFacturasLotesEnviados pParametros)
        {
            return new VTAFacturasLotesEnviadosLN().ObtenerCargosPorPeriodo(pParametros);
        }

            #endregion

            #region Facturas Hoteles
            public static List<VTAFacturasDetalles> FacturasDetallesObtenerDetallesPendientesPorRefTabla(VTAFacturas pParametro)
        {
            return new VTAFacturasLN().ObtenerDetallesPendientesPorRefTabla(pParametro);
        }
         
        public static List<VTAFacturasDetalles> FacturasDetallesObtenerDetallesServiciosPendientesPorRefTabla(VTAFacturas pParametro)
        {
            return new VTAFacturasLN().ObtenerDetallesServiciosPendientesPorRefTabla(pParametro);
        }

        /*Para notas de credito modulo hotel*/
        public static List<VTAFacturasDetalles> FacturasDetallesObtenerPorIdFacturaRefTabla (VTAFacturas pParametro)
        {
            return new VTAFacturasLN().ObtenerDetallesPorIdFacturaRefTabla(pParametro);
        }
        /************************************/
        #endregion

        #region Tipos Facturas

        public static List<TGETiposFacturas> TiposFacturasSeleccionarPorCondicionFiscal(VTAFacturas pParametro)
        {
            return new VTAFacturasLN().ObtenerPorCondicionFiscal(pParametro);
        }

        public static List<TGETiposFacturas> TiposFacturasActivosPorIdTipoFactura(TGETiposFacturas pParametro)
        {
            return new VTAFacturasLN().ObtenerActivosPorIdTipoFactura(pParametro);
        }

        public static List<TGETiposFacturas> TiposFacturasSeleccionarPorRemitos(VTARemitos pParametro)
        {
            return new VTARemitosLN().ObtenerPorRemitos(pParametro);
        }

        #endregion

        #region Facturaciones Habituales
        public static List<VTAFacturacionesHabituales> FacturacionesHabitualesObtenerListaFiltro(VTAFacturacionesHabituales pParametro)
        {
            return new VTAFacturacionesHabitualesLN().ObtenerListaFiltro(pParametro);
        }

        public static VTAFacturacionesHabituales FacturacionesHabitualesObtenerDatosCompletos(VTAFacturacionesHabituales pParametro)
        {
            return new VTAFacturacionesHabitualesLN().ObtenerDatosCompletos(pParametro);
        }

        public static bool FacturacionesHabitualesAgregar(VTAFacturacionesHabituales pParametro)
        {
            return new VTAFacturacionesHabitualesLN().Agregar(pParametro);
        }

        public static bool FacturacionesHabitualesModificar(VTAFacturacionesHabituales pParametro)
        {
            return new VTAFacturacionesHabitualesLN().Modificar(pParametro);
        }

        #endregion

        #region Presupuestos

        public static DataTable PresupuestosObtenerListaGrilla(VTAPresupuestos pFactura)
        {
            return new VTAPresupuestosLN().ObtenerListaGrilla(pFactura);
        }

        public static List<VTAPresupuestos> PresupuestosObtenerListaFiltro(VTAPresupuestos pFactura)
        {
            return new VTAPresupuestosLN().ObtenerListaFiltro(pFactura);
        }

        public static VTAPresupuestos PresupuestosObtenerDatosCompletos(VTAPresupuestos pFactura)
        {
            return new VTAPresupuestosLN().ObtenerDatosCompletos(pFactura);
        }

        public static VTAPresupuestos PresupuestosObtenerArchivo(VTAPresupuestos pFactura)
        {
            return new VTAPresupuestosLN().ObtenerArchivo(pFactura);
        }

        public static bool PresupuestosAgregar(VTAPresupuestos pParametro)
        {
            return new VTAPresupuestosLN().Agregar(pParametro);
        }

        //public static bool PresupuestosActualizarPDF(VTAPresupuestos pParametro)
        //{
        //    return new VTAPresupuestosLN().ActualizarPDF(pParametro);
        //}

        public static bool PresupuestosModificar(VTAPresupuestos pParametro)
        {
            return new VTAPresupuestosLN().Modificar(pParametro);
        }

        public static bool PresupuestosModificarEstado(VTAPresupuestos pParametro, Database db, DbTransaction tran)
        {
            return new VTAPresupuestosLN().ModificarEstado(pParametro, db, tran);
        }

        #endregion

        #region "PUNTOS DE VENTAS"

        public static bool VTAFilialesPuntosVentasAgregar(VTAFilialesPuntosVentas pParametro)
        {
            return new VTAFilialesPuntosVentasLN().Agregar(pParametro);
        }

        public static bool VTAFilialesPuntosVentasModificar(VTAFilialesPuntosVentas pParametro)
        {
            return new VTAFilialesPuntosVentasLN().Modificar(pParametro);
        }

        public static VTAFilialesPuntosVentas VTAFilialesPuntosVentasObtenerDatosCompletos(VTAFilialesPuntosVentas pParametro)
        {
            return new VTAFilialesPuntosVentasLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<VTAFilialesPuntosVentas> VTAFilialesPuntosVentasObtenerListaFiltro(VTAFilialesPuntosVentas pParametro)
        {
            return new VTAFilialesPuntosVentasLN().ObtenerListaFiltro(pParametro);
        }

        /// <summary>
        /// Devuelve los distintos puntos de ventas (tipos de emision) por filial
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static List<VTATiposPuntosVentas> VTAFilialesPuntosVentasObtenerPorFilial(VTAFilialesPuntosVentas pParametro)
        {
            return new VTAFilialesPuntosVentasLN().ObtenerPorFilial(pParametro);
        }

        public static DataTable VTAFilialesPuntosVentasObtenerListaGrilla(VTAFilialesPuntosVentas pParametro)
        {
            return new VTAFilialesPuntosVentasLN().ObtenerListaGrilla(pParametro);
        }

        #endregion

        #region REMITOS

        public static VTARemitos RemitosObtenerDatosCompletos(VTARemitos pParametro)
        {
            return new VTARemitosLN().ObtenerDatosCompletos(pParametro);
        }

        public static VTARemitos RemitosObtenerDatosPreCargados(VTARemitos pParametro)
        {
            return new VTARemitosLN().ObtenerDatosPreCargados(pParametro);
        }

        public static VTARemitos RemitosObtenerPorFactura(VTAFacturas pParametro)
        {
            return new VTARemitosLN().ObtenerPorFactura(pParametro);
        }

        public static VTARemitos RemitosObtenerArchivo(VTARemitos pRemito)
        {
            return new VTARemitosLN().ObtenerArchivo(pRemito);
        }

        public static DataTable RemitosObtenerGrilla(VTARemitos pFactura)
        {
            return new VTARemitosLN().ObtenerGrilla(pFactura);
        }
        public static DataTable RemitosObtenerAfiliadoGrilla(VTARemitos pFactura)
        {
            return new VTARemitosLN().ObtenerAfiliadoGrilla(pFactura);
        }
        
        public static List<VTARemitos> RemitosObtenerListaFiltro(VTARemitos pParametro)
        {
            return new VTARemitosLN().ObtenerListaFiltro(pParametro);
        }

        public static List<VTARemitos> RemitosObtenerListaOrdenesCobrosPendientes(VTARemitos pParametro)
        {
            return new VTARemitosLN().ObtenerListaOrdenesCobrosPendientes(pParametro);
        }

        public static List<VTARemitosDetalles> RemitosObtenerListaFiltroPopUp(VTARemitos pParametro)
        {
            return new VTARemitosLN().ObtenerListaFiltroPopUp(pParametro);
        }

        public static bool RemitosObtenerProximoNumeroComprobanteTmp(VTARemitos pParametro)
        {
            return new VTARemitosLN().ObtenerProximoNumeroRemitoTmp(pParametro);
        }

        public static bool RemitosAgregar(VTARemitos pParametro)
        {
            return new VTARemitosLN().Agregar(pParametro);
        }

        public static bool RemitosModificar(VTARemitos pParametro)
        {
            return new VTARemitosLN().Modificar(pParametro);
        }

        public static bool RemitosModificarPDF(VTARemitos pParametro)
        {
            return new VTARemitosLN().ModificarPDF(pParametro);
        }

        public static bool RemitoArmarMail(VTARemitos pParametro, MailMessage mail)
        {
            return new VTARemitosLN().ArmarMailRemito(pParametro, mail);
        }

        public static bool RemitosAnular(VTARemitos pParametro)
        {
            return new VTARemitosLN().Anular(pParametro);
        }

        /// <summary>
        /// Metodo que Genera y Guarda el PDF del Remito
        /// </summary>
        /// <param name="pParametro">IdRemito</param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        //public static bool RemitosGeneroGuardoPDF(VTARemitos pParametro, Database bd, DbTransaction tran)
        //{
        //    return new VTARemitosLN().GeneroGuardoPDF(pParametro, bd, tran);
        //}

        public static List<VTAFacturas> RemitosObtenerFacturasPendientesARemitarPorIdAfiliado(VTAFacturas pParametro)
        {
            return new VTARemitosLN().ObtenerFacturasPendientesARemitarPorIdAfiliado(pParametro);
        }

        public static bool RemitosElectronicaValidarProximoNumeroComprobante(VTARemitos pParametro)
        {
            return true;// new FacturaElectronica.FacturaElectronica().ValidarProximoNumeroComprobante(pParametro);
        }

        public static bool RemitosElectronicaValidarComprobanteAfip(VTARemitos pParametro)
        {
            return true;// new VTAFacturasLN().ValidarFE(pParametro);
        }
        #endregion

        #region NOTAS PEDIDOS

        public static VTANotasPedidos NotasPedidosObtenerDatosCompletos(VTANotasPedidos pParametro)
        {
            return new VTANotasPedidosLN().ObtenerDatosCompletos(pParametro);
        }

        public static DataTable NotasPedidosObtenerGrilla(VTANotasPedidos pNotasPedidos)
        {
            return new VTANotasPedidosLN().ObtenerGrilla(pNotasPedidos);
        }

        public static VTANotasPedidos NotasPedidosObtenerArchivo(VTANotasPedidos pFactura)
        {
            return new VTANotasPedidosLN().ObtenerArchivo(pFactura);
        }

        public static List<VTANotasPedidosDetalles> NotasPedidosObtenerListaFiltroPopUp(VTANotasPedidos pParametro)
        {
            return new VTANotasPedidosLN().ObtenerListaFiltroPopUp(pParametro);
        }
        public static DataTable NotasPedidosObtenerAfiliadoGrilla(VTANotasPedidos pFactura)
        {
            throw new NotImplementedException(); //return new VTANotasPedidosLN().ObtenerAfiliadoGrilla(pFactura);
        }

        public static List<VTANotasPedidos> NotasPedidosObtenerListaFiltro(VTANotasPedidos pParametro)
        {
            return new VTANotasPedidosLN().ObtenerListaFiltro(pParametro);
        }

        public static bool NotasPedidosAgregar(VTANotasPedidos pParametro)
        {
            return new VTANotasPedidosLN().Agregar(pParametro);
        }

        public static bool NotasPedidosModificar(VTANotasPedidos pParametro)
        {
            return new VTANotasPedidosLN().Modificar(pParametro);
        }


        public static bool NotasPedidosAnular(VTANotasPedidos pParametro)
        {
            return new VTANotasPedidosLN().Anular(pParametro);
        }

        #endregion

        #region SECTORES PUNTOS VENTAS

        public static DataTable VTASectoresPuntosVentasObtenerListaGrilla(VTASectoresPuntosVentas pParametro)
        {
            return new VTASectoresPuntosVentasLN().ObtenerListaGrilla(pParametro);
        }

        public static VTASectoresPuntosVentas VTASectoresPuntosVentasObtenerDatosCompletos(VTASectoresPuntosVentas pParametro)
        {
            return new VTASectoresPuntosVentasLN().ObtenerDatosCompletos(pParametro);
        }

        public static bool VTASectoresPuntosVentasAgregar(VTASectoresPuntosVentas pParametro)
        {
            return new VTASectoresPuntosVentasLN().Agregar(pParametro);
        }

        public static bool VTASectoresPuntosVentasModificar(VTASectoresPuntosVentas pParametro)
        {
            return new VTASectoresPuntosVentasLN().Modificar(pParametro);
        }

        public static List<TGESectores> VTASectoresPuntosVentasObtenerSectoresPorFilial(VTASectoresPuntosVentas pParametro,int idFilial)
        {
            return new VTASectoresPuntosVentasLN().ObtenerSectoresPorFilial(pParametro, idFilial);
        }

        #endregion

        #region Percepciones

        public static List<VTAFacturasTiposPercepciones> FacturasPercepcionesObtenerPorIdRefTabla(VTAFacturas pParametro)
        {
            return new VTAFacturasLN().ObtenerPercepcionesPorIdRefTabla(pParametro);
        }

        #endregion
    }
}
