using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Bancos.Entidades;
using Servicio.AccesoDatos;
using Generales.Entidades;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Auditoria.Entidades;
using Auditoria;
using CuentasPagar.Entidades;
using Generales.FachadaNegocio;
using System.Runtime.InteropServices;

namespace Bancos.LogicaNegocio
{
    class TESCobranzasExternasConciliacionesLN : BaseLN<TESCobranzasExternasConciliaciones>
    {
        public override bool Agregar(TESCobranzasExternasConciliaciones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            if (pParametro.IdCobranzaExternaConciliacion > 0)
                return true;

            if (!this.Validar(pParametro, new TESCobranzasExternasConciliaciones()))
                return false;

            pParametro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.ConciliacionCobranzaExterna;
            pParametro.Estado.IdEstado = (int)Estados.Activo;
            pParametro.FechaAlta = DateTime.Now;
            pParametro.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuario;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {

                    pParametro.IdCobranzaExternaConciliacion = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "TESCobranzasExternasConciliacionesInsertar");
                    if (pParametro.IdCobranzaExternaConciliacion == 0)
                        resultado = false;

                    #region Movimiento Cuenta bancaria
                    TESBancosCuentasMovimientos movBco;
                    movBco = new TESBancosCuentasMovimientos();
                    movBco.EstadoColeccion = EstadoColecciones.Agregado;
                    movBco.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.Confirmado; //ver bien que estado poner
                    movBco.BancoCuenta.IdBancoCuenta = pParametro.IdBancoCuenta;
                    movBco.TipoOperacion.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
                    movBco.IdRefTipoOperacion = pParametro.IdCobranzaExternaConciliacion;
                    movBco.IdCajaMovimientoValor = 0;
                    movBco.Importe = pParametro.ImporteNeto;
                    movBco.FechaAlta = DateTime.Now;
                    movBco.FechaMovimiento = DateTime.Now;
                    movBco.FechaConciliacion = DateTime.Now;
                    movBco.IdUsuarioConciliacion = pParametro.UsuarioLogueado.IdUsuario;
                    movBco.FechaConfirmacionBanco = pParametro.FechaConfirmacion;
                    movBco.IdRefTipoOperacion = pParametro.IdCobranzaExternaConciliacion;
                    movBco.UsuarioAlta.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuario;
                    movBco.UsuarioLogueado = pParametro.UsuarioLogueado;

                    if (!BancosF.BancosCuentasMovimientosAgregar(movBco, bd, tran))
                    {
                        AyudaProgramacionLN.MapearError(movBco, pParametro);
                        return false;
                    }

                    #endregion

                    if (resultado && !this.ActualizarDetalles(pParametro, new TESCobranzasExternasConciliaciones(), bd, tran))
                        resultado = false;

                    if (resultado && !new InterfazContableLN().ConciliacionCobranzaExterna(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        pParametro.IdCobranzaExternaConciliacion = 0;
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.IdCobranzaExternaConciliacion = 0;
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        public bool Anular(TESCobranzasExternasConciliaciones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            //pParametro.IdCobranzaExternaConciliacion = 
            pParametro.EstadoColeccion = EstadoColecciones.Borrado;
            pParametro.Estado.IdEstado = (int)Estados.Baja;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();
                try
                {
                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TESCobranzasExternasConciliacionesAnular"))
                    {
                        resultado = false;
                    }
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

        private bool ActualizarDetalles(TESCobranzasExternasConciliaciones pParametro, TESCobranzasExternasConciliaciones pValorViejo, Database bd, DbTransaction tran)
        {
             
            TESTarjetasTransaccionesLN tarjetaLN = new TESTarjetasTransaccionesLN(); //a modificar estado
            foreach (TESCobranzasExternasConciliacionesDetalles Detalle in pParametro.CobranzaExternaConciliacionDetalles.Where(x => x.Checked).ToList())
            {
                Detalle.Estado.IdEstado = (int)Estados.Activo;
                Detalle.IdCobranzaExternaConciliacion = pParametro.IdCobranzaExternaConciliacion;
                Detalle.IdCobranzaExternaConciliacionDetalle = BaseDatos.ObtenerBaseDatos().Agregar(Detalle, bd, tran, "TESCobranzasExternasConciliacionesDetallesInsertar");
                if (Detalle.IdCobranzaExternaConciliacionDetalle == 0)
                {
                    AyudaProgramacionLN.MapearError(Detalle, pParametro);
                    return false;
                }
                //Modifico el estado de la Tarjeta relacionada
                /*
                Detalle.TarjetaTransaccion.UsuarioLogueado = pParametro.UsuarioLogueado;
                Detalle.TarjetaTransaccion.FechaEvento = DateTime.Now;
                Detalle.TarjetaTransaccion.Estado.IdEstado = (int)EstadosCheques.Depositado; //CONSULTAR ESTADO DE LA TRANSACCION
                if (!tarjetaLN.ModificarEstado(Detalle.TarjetaTransaccion, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(Detalle.TarjetaTransaccion, pParametro);
                    return false;
                }
                 * */
                
            }
            //Cargo las deducciones
            foreach (TESCobranzasExternasConciliacionesDeducciones deduccion in pParametro.CobranzaExternaConciliacionDeducciones)
            {
                deduccion.Estado.IdEstado = (int)Estados.Activo;
                deduccion.IdCobranzaExternaConciliacion = pParametro.IdCobranzaExternaConciliacion;
                deduccion.IdCobranzaExternaConciliacionDeduccion = BaseDatos.ObtenerBaseDatos().Agregar(deduccion, bd, tran, "TESCobranzasExternasConciliacionesDeduccionesInsertar");
                if (deduccion.IdCobranzaExternaConciliacionDeduccion == 0)
                {
                    AyudaProgramacionLN.MapearError(deduccion, pParametro);
                    return false;
                } 
            }
            return true;
        }

        private bool Validar(TESCobranzasExternasConciliaciones pParametro, TESCobranzasExternasConciliaciones tESCobranzasExternasConciliaciones)
        {
            return true;
        }

        public override bool Modificar(TESCobranzasExternasConciliaciones pParametro)
        {
            throw new NotImplementedException();
        }

        public override TESCobranzasExternasConciliaciones ObtenerDatosCompletos(TESCobranzasExternasConciliaciones pParametro)
        {
            TESCobranzasExternasConciliaciones cobranza = new TESCobranzasExternasConciliaciones();
            cobranza =  BaseDatos.ObtenerBaseDatos().Obtener<TESCobranzasExternasConciliaciones>("TESCobranzasExternasConciliacionesSeleccionar", pParametro);
            cobranza.CobranzaExternaConciliacionDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<TESCobranzasExternasConciliacionesDetalles>("TESCobranzasExternasConciliacionesDetallesSeleccionarPorIdCEC", pParametro);
            cobranza.CobranzaExternaConciliacionDeducciones = BaseDatos.ObtenerBaseDatos().ObtenerLista<TESCobranzasExternasConciliacionesDeducciones>("TESCobranzasExternasConciliacionesDeduccionesPorIdCEC", pParametro);
            return cobranza;
        }

        public override List<TESCobranzasExternasConciliaciones> ObtenerListaFiltro(TESCobranzasExternasConciliaciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESCobranzasExternasConciliaciones>("TESCobranzasExternasConciliacionesListaFiltro", pParametro);
        }

        public List<TESTarjetasTransacciones> ObtenerActivosEntreFechas(TESCobranzasExternasConciliaciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESTarjetasTransacciones>("TESTarjetasTransaccionesObtenerActivosEntreFechas", pParametro);
        }
    }
}
