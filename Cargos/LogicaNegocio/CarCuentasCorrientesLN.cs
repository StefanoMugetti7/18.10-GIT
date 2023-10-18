using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cargos.Entidades;
using Servicio.AccesoDatos;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Comunes.LogicaNegocio;
using Generales.Entidades;
using Comunes.Entidades;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Auditoria;
using Auditoria.Entidades;
using Haberes.Entidades;
using Haberes;
using Contabilidad.Entidades;
using System.Data;
using Generales.FachadaNegocio;

namespace Cargos.LogicaNegocio
{
    class CarCuentasCorrientesLN
    {
        public CarCuentasCorrientes ObtenerDatosCompletos(CarCuentasCorrientes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CarCuentasCorrientes>("[CarCuentasCorrientesSeleccionar]", pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Cuenta Corriente por Afiliado
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public List<CarCuentasCorrientes> Obtener(CarCuentasCorrientes pParametro)


        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CarCuentasCorrientes>("CarCuentasCorrientesSeleccionarPorAfiliado", pParametro);
        }


    
        /// <summary>
        /// Devuelve una lista de Cuenta Corriente por Afiliado pendiente de Cobro
        /// </summary>
        /// <param name="pParametro">IdAfiliado</param>
        /// <returns></returns>
        public List<CarCuentasCorrientes> ObtenerPendienteCobros(CarCuentasCorrientes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CarCuentasCorrientes>("CarCuentasCorrientesSeleccionarPorAfiliadoPendienteCobro", pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Periodos de la Cuenta Corriente por Afiliado
        /// </summary>
        /// <param name="pParametro">IdAfiliado</param>
        /// <returns></returns>
        public List<TGEListasValoresDetalles> ObtenerPeriodos(CarCuentasCorrientes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TGEListasValoresDetalles>("CarCuentasCorrientesSeleccionarPeriodosPorAfiliado", pParametro);
        }

        /// <summary>
        /// Devuelve el Saldo Actual de la Cuenta Corriente del Socio
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public decimal ObtenerSaldoActual(CarCuentasCorrientes pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CarCuentasCorrientes>("CarCuentasCorrientesSeleccionarPorAfiliadoSaldoActual", pParametro);
            return pParametro.SaldoActual;
        }

        public decimal ObtenerSaldoPorOperacion(CarCuentasCorrientes pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CarCuentasCorrientes>("CarCuentasCorrientesObtenerSaldoPorOperacion", pParametro);
            return pParametro.SaldoActual;
        }

        /// <summary>
        /// Devuelve el Saldo Actual de la Cuenta Corriente del Socio
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public decimal ObtenerSaldoActual(CarCuentasCorrientes pParametro, Database bd, DbTransaction tran)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CarCuentasCorrientes>("CarCuentasCorrientesSeleccionarPorAfiliadoSaldoActual", pParametro, bd, tran);
            return pParametro.SaldoActual;
        }

        public DataTable ObetenerCodigosConceptosCargos(CarCuentasCorrientes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CarCuentasCorrientesSeleccionarCodigosConceptosCargos", pParametro);
        }

        public bool Agregar(CarCuentasCorrientes pParametro, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            if (!this.Validar(pParametro, bd, tran))
                return false;

            pParametro.IdCuentaCorriente = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CarCuentasCorrientesInsertar");
            if (pParametro.IdCuentaCorriente == 0)
            {
                resultado = false;
            }

            if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PagoHaberes)
            {
                pParametro.ReciboComPago.IdCuentaCorriente = pParametro.IdCuentaCorriente;
                pParametro.ReciboComPago.Estado.IdEstado = (int)Estados.Activo;
                pParametro.ReciboComPago.IdReciboCOM = pParametro.IdRefTipoOperacion;
                pParametro.ReciboComPago.UsuarioLogueado = pParametro.UsuarioLogueado;

                if (resultado && !HaberesF.HabRecibosComPagosAgregar(pParametro.ReciboComPago, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(pParametro.ReciboComPago, pParametro);
                    resultado = false;
            }

                if (resultado && !new InterfazContableLN().PagoHaberes(pParametro, pValoresImportes, bd, tran))
                    resultado = false;

                if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                    resultado = false;
            }

            return resultado;
        }

        public bool Actualizar(CarCuentasCorrientes pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CarCuentasCorrientes valorViejo = new CarCuentasCorrientes();
            valorViejo.IdCuentaCorriente = pParametro.IdCuentaCorriente;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = this.Actualizar(pParametro, bd, tran);

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

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
                    pParametro.CodigoMensaje = ex.Message;
                    return false;
                }
            }
            return resultado;
        }

        public bool Actualizar(CarCuentasCorrientes pParametro, Database bd, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CarCuentasCorrientesActualizar"))
                return false;

            return true;
        }

        public bool ActualizarEstado(CarCuentasCorrientes pParametro, Database bd, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CarCuentasCorrientesActualizarEstado"))
                return false;

            return true;
        }

        private bool Validar(CarCuentasCorrientes pParametro, Database bd, DbTransaction tran)
        {
            decimal saldoActual = 0;
            CarCuentasCorrientes filtro = new CarCuentasCorrientes();
            filtro.IdAfiliado = pParametro.IdAfiliado;

            //Valido de Saldo para Pago de Haberes
            if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PagoHaberes)
            {
                //filtro.TipoOperacion.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
                //filtro.IdRefTipoOperacion = pParametro.IdRefTipoOperacion;
                //saldoActual = this.ObtenerSaldoPorOperacion(filtro);
                HabRecibosCom recibo = new HabRecibosCom();
                recibo.IdReciboCom = pParametro.IdRefTipoOperacion;
                recibo.IdAfiliado = pParametro.IdAfiliado;
                recibo.Periodo = pParametro.Periodo;
                recibo = HaberesF.HabRecibosComObtenerUltimoRecibo(recibo);

                if ((recibo.NetoPagar - pParametro.Importe) < 0)
                {
                    pParametro.CodigoMensaje = "CuentaCorrienteValidarSaldoActual";
                    pParametro.CodigoMensajeArgs.Add(recibo.NetoPagar.ToString("C2"));
                    return false;
                }
            }
            // Cuando es una Extracción Valido el Saldo Actual de la Cuenta Corriente
            else if (pParametro.TipoOperacion.TipoMovimiento.IdTipoMovimiento == (int)EnumTGETiposMovimientos.Debito)
            {
                saldoActual = this.ObtenerSaldoActual(filtro);

                if ((saldoActual - pParametro.Importe) < 0)
                {
                    pParametro.CodigoMensaje = "CuentaCorrienteValidarSaldoActual";
                    pParametro.CodigoMensajeArgs.Add(saldoActual.ToString("C2"));
                    return false;
                }
                
            }
            return true;
        }

        public DataTable ObtenerDT(CarCuentasCorrientes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CarCuentasCorrientesSeleccionarPorAfiliadoDataTable", pParametro);
        }

        public DataTable ObtenerDTGrilla(CarCuentasCorrientes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CarCuentasCorrientesSeleccionarPorAfiliado", pParametro);
        }

        public DataTable ObtenerCobrosPorCargo(CarCuentasCorrientes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CarCuentasCorrientesSeleccionarCobrosPorCargoDataTable", pParametro);
        }

        public DataTable ObtenerFacturacionAnticipada(CarCuentasCorrientes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CarProcesoLevantamientoCargosFacturacionAnticipada", pParametro);
        }
        public bool AgregarFacturacionAnticipada(CarCuentasCorrientes pParametro)
        {
            bool resultado = true;
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(pParametro, bd, tran, "CarProcesoLevantamientoCargosFacturacionAnticipadaInsertar");

                    if (resultado)
                    {
                        tran.Commit();
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

        public bool DesimputarCobro(CarCuentasCorrientes pParametro)
        {
            bool resultado = true;
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CarCuentasCorrientesFacturasValidaciones"))
            {
                return resultado = false;
            }

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(pParametro, bd, tran, "CarCuentasCorrientesDesimputarCobroV2");
                    
                    if (resultado)
                    {
                        tran.Commit();
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

        public bool RevertirCobroCargos(CarCuentasCorrientes pParametro)
        {
            bool resultado = true;
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CarCuentasCorrientesFacturasValidaciones"))
            {
                return resultado = false;
            }

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(pParametro, bd, tran, "CarCuentasCorrientesRevertirCobroCargos");

                    if (resultado)
                    {
                        tran.Commit();
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

    }
}
