using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bancos.Entidades;
using Bancos.LogicaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Comunes.Entidades;
using Generales.Entidades;
using System.Data;

namespace Bancos
{
    public class BancosF
    {
        #region Bancos

        public static List<TESBancos> BancosObtenerListaFiltro(TESBancos pParametro)
        {
            return new TESBancosLN().ObtenerListaFiltro(pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Bancos que tienen una Cuenta Banacaria relacionada
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static List<TESBancos> BancosObtenerListaFiltroConCuentas(TESBancos pParametro)
        {
            return new TESBancosLN().ObtenerListaFiltroConCuentas(pParametro);
        }

        /// <summary>
        /// Devuelve una Lista de Bancos que tienen Cuentas Bancarias para una Filial
        /// </summary>
        /// <param name="pParametro">IdFilial, [IdEstado]</param>
        /// <returns></returns>
        public static List<TESBancos> BancosObtenerListaFilialFiltro(TGEFiliales pParametro)
        {
            return new TESBancosLN().ObtenerListaFilialFiltro(pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Filiales Destino para Transferencias Bancarias
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static List<TGEFiliales> BancosFilialesObtenerListaTransferenciaDestino(TGEFiliales pParametro)
        {
            return new TESBancosLN().ObtenerListaTransferenciaDestino(pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Filiales Destino para Tesorerias
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static List<TGEFiliales> BancosFilialesObtenerListaTesoreriasDestino(TGEFiliales pParametro)
        {
            return new TESBancosLN().ObtenerListaTesoreriasDestino(pParametro);
        }

        #endregion

        #region Bancos Cuentas

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static TESBancosCuentas BancosCuentasObtenerDatosCompletos(TESBancosCuentas pParametro)
        {
            return new TESBancosCuentasLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<TESBancosCuentas> CuentasObtenerListaFiltro(TESBancosCuentas pParametro)
        {
            return new TESBancosCuentasLN().CuentasObtenerListaFiltro(pParametro);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static DataTable BancosCuentasObtenerMovimientos(TESBancosCuentas pParametro)
        {
            return new TESBancosCuentasLN().ObtenerMovimientos(pParametro);
        }

        public static DataTable BancosCuentasObtenerMovimientosPendientes(TESBancosCuentas pParametro)
        {
            return new TESBancosCuentasLN().ObtenerMovimientosPendientes(pParametro);
        }

        public static DataTable BancosCuentasObtenerMovimientosRechazados(TESBancosCuentas pParametro)
        {
            return new TESBancosCuentasLN().ObtenerMovimientosRechazados(pParametro);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static List<TESBancosCuentas> BancosCuentasObtenerListaFiltro(TESBancosCuentas pParametro)
        {
            return new TESBancosCuentasLN().ObtenerListaFiltro(pParametro);
        }
        public static List<TESBancosCuentas> BancosCuentasObtenerListaFiltroTransferencia(TESBancosCuentas pParametro)
        {
              return new TESBancosCuentasLN().ObtenerListaFiltroTransferencia(pParametro);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static List<TESBancosCuentas> BancosCuentasObtenerListaDestinoDepositoTesoreria(TESBancosCuentas pParametro)
        {
            return new TESBancosCuentasLN().ObtenerListaDestinoDepositoTesoreria(pParametro);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static List<TESBancosCuentas> BancosCuentasObtenerDepositar(TGEFiliales pParametro)
        {
            return new TESBancosCuentasLN().ObtenerDepositar(pParametro);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static bool BancosCuentasAgregar(TESBancosCuentas pParametro)
        {
            return new TESBancosCuentasLN().Agregar(pParametro);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static bool BancosCuentasModificar(TESBancosCuentas pParametro)
        {
            return new TESBancosCuentasLN().Modificar(pParametro);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tESBancosCuentas"></param>
        /// <returns></returns>
        public static bool BancosCuentasActualizarMovimientosPendientes(TESBancosCuentas pParametro)
        {
            return new TESBancosCuentasLN().ActualizarMovimientosPendientes(pParametro);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static TESBancosCuentasMovimientos BancosCuentasMovimientosObtenerDatosCompletos(TESBancosCuentasMovimientos pParametro)
        {
            return new TESBancosCuentasLN().ObtenerDatosCompletos(pParametro);
        }

        public static bool BancosCuentasMovimientosAgregar(TESBancosCuentasMovimientos pParametro)
        {
            return new TESBancosCuentasLN().AgregarMovimiento(pParametro);
        }
        public static bool BancosCuentasMovimientosMultiplesAgregar(List<TESBancosCuentasMovimientos> pParametro,ref string mensajeRetorno)
        {
            return new TESBancosCuentasLN().AgregarMovimientosMultiples(pParametro, ref mensajeRetorno);
        }
        public static bool BancosCuentasMovimientosAgregar(TESBancosCuentasMovimientos pParametro, Database bd, DbTransaction tran)
        {
            return new TESBancosCuentasLN().AgregarMovimiento(pParametro, bd, tran);
        }

        public static bool BancosCuentasMovimientosAnular(TESBancosCuentasMovimientos pParametro)
        {
            return new TESBancosCuentasLN().AnularMovimiento(pParametro);
        }

        public static bool BancosCuentasMovimientosAnularConfirmado(TESBancosCuentasMovimientos pParametro)
        {
            return new TESBancosCuentasLN().AnularMovimientoConfirmado(pParametro);
        }

        /// <summary>
        /// Actualiza el estado de un movimiento de Bancos
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        //public static bool BancosCuentasMovimientoTrasnferenciaTesoreriaActualizar(TESBancosCuentasMovimientos pParametro, Database bd, DbTransaction tran)
        //{
        //    return new TESBancosCuentasLN().MovimientoTransferenciaTesoreriaActualizar(pParametro, bd, tran);
        //}

        /// <summary>
        /// Devuelve los movimientos pendientes de confirmacion para Bancos
        /// </summary>
        /// <param name="pParametro">IdTesoreria</param>
        /// <returns></returns>
        //public static List<TESBancosCuentasMovimientos> BancosCuentasMovimientosObtenerPendientesConfirmacion(TESBancosCuentas pParametro)
        //{
        //    return new TESBancosCuentasLN().ObtenerPendientesConfirmacion(pParametro);
        //}
        //public static bool BancosCuentasMovimientosConfirmarOperaciones(Objeto pParametro, List<TESBancosCuentasMovimientos> pMovimientos)
        //{
        //    return new TESBancosCuentasLN().ConfirmarOperaciones(pParametro, pMovimientos);
        //}

        ///// <summary>
        ///// Metodo que trae el listado de cuentas al seleccionar la opcion de "Contado" al dar de alta una Factura de venta
        ///// </summary>
        ///// <param name="pParametro">No se utiliza</param>
        ///// <returns></returns>

        public static List<TESBancosCuentas> BancosCuentasObtenerLista(TESBancosCuentas pParametro)
        {
            return new TESBancosCuentasLN().ObtenerLista(pParametro);
        }
        public static List<TESBancosCuentas> BancosCuentasObtenerListaGrupo(TESBancosCuentas pParametro)
        {
            return new TESBancosCuentasLN().ObtenerListaGrupo(pParametro);
        }
        #endregion

        #region Cheques

        public static TESCheques ChequesObtenerDatosCompletos(TESCheques pParametro)
        {
            return new TESChequesLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<TESCheques> ChequesObtenerListaFiltro(TESCheques pParametro)
        {
            return new TESChequesLN().ObtenerListaFiltro(pParametro);
        }

        public static List<TESCheques> ChequesObtenerDisponibles(TESCheques pParametro)
        {
            return new TESChequesLN().ObtenerDisponibles(pParametro);
        }

        public static List<TESCheques> ChequesObtenerListaFiltroPropios(TESCheques pParametro)
        {
            return new TESChequesLN().ObtenerListaFiltroPropios(pParametro);
        }

        public static bool ChequesAgregar(TESCheques pParametro, Database bd, DbTransaction tran)
        {
            return new TESChequesLN().Agregar(pParametro, bd, tran);
        }

        public static bool ChequesTransferir(List<TESCheques> pLista, Objeto pParametro, EnumTGETiposOperaciones pTipoOperacion, 
            TESBancosCuentasMovimientos pBancoCuentaMovimiento, TESChequesMovimientos pChequeMovimiento)
        {
            return new TESChequesLN().Transferir(pLista, pParametro, pTipoOperacion, pBancoCuentaMovimiento, pChequeMovimiento);
        }

        /// <summary>
        /// Modifica un Cheque
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static bool ChequesModificar(TESCheques pParametro, Database bd, DbTransaction tran)
        {
            return new TESChequesLN().Modificar(pParametro, bd, tran);
        }

        /// <summary>
        /// Modifica los datos adicionales de un Cheque
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static bool ChequesModificar(TESCheques pParametro)
        {
            return new TESChequesLN().Modificar(pParametro);
        }

        /// <summary>
        /// Agrega un Movimiento de Cheque
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static bool ChequesAgregarMovimiento(TESChequesMovimientos pParametro, Database bd, DbTransaction tran)
        {
            return new TESChequesLN().AgregarMovimiento(pParametro, bd, tran);
        }

        public static List<TESCheques> ChequesObtenerChequesTerceros(TESCheques pParametro)
        {
            return new TESChequesLN().ObtenerChequesTerceros(pParametro);
        }

        public static List<TESCheques> ChequesObtenerChequesTercerosFiltro(TESCheques pParametro)
        {
            return new TESChequesLN().ObtenerChequesTercerosFiltro(pParametro);
        }
        public static TESChequesMovimientos ChequesObtenerDatosCompletosChequesMovimientos(TESChequesMovimientos pParametro)
        {
            return new TESChequesLN().ObtenerDatosCompletosChequesMovimientos(pParametro);
        }
        #endregion

        #region Cobranzas Externas Conciliaciones

        public static bool CobranzasExternasConciliacionesAgregar(TESCobranzasExternasConciliaciones pParametro)
        {
            return new TESCobranzasExternasConciliacionesLN().Agregar(pParametro);
        }

        public static bool CobranzasExternasConciliacionesAnular(TESCobranzasExternasConciliaciones pParametro)
        {
            return new TESCobranzasExternasConciliacionesLN().Anular(pParametro);
        }

        public static TESCobranzasExternasConciliaciones CobranzasExternasConciliacionesObtenerDatosCompletos(TESCobranzasExternasConciliaciones pParametro)
        {
            return new TESCobranzasExternasConciliacionesLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<TESCobranzasExternasConciliaciones> CobranzasExternasConciliacionesObtenerListaFiltro(TESCobranzasExternasConciliaciones pParametro)
        {
            return new TESCobranzasExternasConciliacionesLN().ObtenerListaFiltro(pParametro);
        }

        public static List<TESTarjetasTransacciones> TarjetasTransaccionesObtenerActivoEntreFechas(TESCobranzasExternasConciliaciones pParametro)
        {
            return new TESCobranzasExternasConciliacionesLN().ObtenerActivosEntreFechas(pParametro);
        }

        #endregion

        #region Tarjetas Transacciones

        public static bool TarjetasTransaccionesAgregar(TESTarjetasTransacciones pParametro)
        {
            return new TESTarjetasTransaccionesLN().Agregar(pParametro);
        }

        public static bool TarjetasTransaccionesAgregar(TESTarjetasTransacciones pParametro, Database bd, DbTransaction tran)
        {
            return new TESTarjetasTransaccionesLN().Agregar(pParametro, bd, tran);
        }

        public static TESTarjetasTransacciones TarjetasTransaccionesObtenerDatosCompletos(TESTarjetasTransacciones pParametro)
        {
            return new TESTarjetasTransaccionesLN().ObtenerDatosCompletos(pParametro);
        }

        #endregion

        #region Tarjetas Transacciones

        //public static bool PlazosFijosAgregar(TESPlazosFijos pParametro)
        //{
        //    //return new AhoPlazosFijosLN().Agregar(pParametro);
        //}

        #endregion

        #region Plazos Fijos
        public static bool PlazosFijosAgregar(TESPlazosFijos pParametro)
        {
            return new TesPlazosFijosLN().Agregar(pParametro);
        }

        public static List<TESPlazosFijos> PlazosFijosObtenerListaFiltro(TESPlazosFijos pParametro)
        {
            return new TesPlazosFijosLN().ObtenerListaFiltro(pParametro);
        }

        public static DataTable PlazosFijosObtenerListaFiltroDT(TESPlazosFijos pParametro)
        {
            return new TesPlazosFijosLN().ObtenerListaFiltroDT(pParametro);
        }

        public static bool PlazosFijosModificar(TESPlazosFijos pParametro)
        {
            return new TesPlazosFijosLN().Modificar(pParametro);
        }

        public static TESPlazosFijos PlazosFijosObtenerDatosCompletos(TESPlazosFijos pParametro)
        {
            return new TesPlazosFijosLN().ObtenerDatosCompletos(pParametro);
        }

        public static bool PlazosFijosBorrar(TESPlazosFijos pParametro)
        {
            return new TesPlazosFijosLN().Borrar(pParametro);
        }

        public static bool PlazosFijosPagar(TESPlazosFijos pParametro)
        {
            return new TesPlazosFijosLN().Pagar(pParametro);
        }
        #endregion

        #region Lotes

        public static bool BancosLotesEnviadosAgregar(TESBancosLotesEnviados pParametro)
        {
            return new TESBancosLotesEnviadosLN().Agregar(pParametro);
        }

        public static bool BancosLotesEnviadosModificar(TESBancosLotesEnviados pParametro)
        {
            return new TESBancosLotesEnviadosLN().Modificar(pParametro);
        }
        public static bool BancosLotesEnviadosAutorizar(TESBancosLotesEnviados pParametro)
        {
            return new TESBancosLotesEnviadosLN().Autorizar(pParametro);
        }  
        public static bool BancosLotesEnviadosAnular(TESBancosLotesEnviados pParametro)
        {
            return new TESBancosLotesEnviadosLN().Anular(pParametro);
        } 
        public static bool BancosLotesEnviadosConfirmar(TESBancosLotesEnviados pParametro)
        {
            return new TESBancosLotesEnviadosLN().Confirmar(pParametro);
        }
        public static bool BancosLotesEnviadosCancelarConfirmado(TESBancosLotesEnviados pParametro)
        {
            return new TESBancosLotesEnviadosLN().CancelarConfirmado(pParametro);
        }
        public static TESBancosLotesEnviados BancosCuentasObtenerDatosCompletos(TESBancosLotesEnviados pParametro)
        {
            return new TESBancosLotesEnviadosLN().ObtenerDatosCompletos(pParametro);
        }

        public static TESBancosLotesEnviados BancosLotesEnviadosObtenerDatosCompletos(TESBancosLotesEnviados pParametro)
        {
            return new TESBancosLotesEnviadosLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<TESBancosLotesEnviados> BancosLotesEnviadosObtenerCuentas()
        {
            return new TESBancosLotesEnviadosLN().ObtenerCuentas();
        }   
        public static List<TESBancosLotesEnviados> BancosLotesEnviadosObtenerTiposArchivos()
        {
            return new TESBancosLotesEnviadosLN().ObtenerTiposArchivos();
        } 
        public static List<TESBancosLotesEnviadosDetalle> BancosLotesEnviadosObtenerTiposOperaciones()
        {
            return new TESBancosLotesEnviadosLN().ObtenerTiposOperaciones();
        }

        public static List<TESBancosLotesEnviados> BancosLotesEnviadosObtenerListaFiltro(TESBancosLotesEnviados pParametro)
        {
            return new TESBancosLotesEnviadosLN().ObtenerListaFiltro(pParametro);
        }
        public static TESBancosLotesEnviados BancosObtenerDetalles(TESBancosLotesEnviados pParametro)
        {
            pParametro.Detalles = new TESBancosLotesEnviadosLN().ObtenerDetalles(pParametro);

            return pParametro;
        }

        public static DataTable BancosLotesEnviadosObtenerDatosTxt(TESBancosLotesEnviados pParametro)
        {
            return new TESBancosLotesEnviadosLN().ObtenerDatosTxt(pParametro);
        }



        #endregion
    }
}
