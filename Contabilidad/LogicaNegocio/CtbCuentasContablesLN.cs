using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contabilidad.Entidades;
using Comunes;
using Servicio.AccesoDatos;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Auditoria;
using Auditoria.Entidades;
using Generales.FachadaNegocio;
using System.Data;

namespace Contabilidad.LogicaNegocio
{
    class CtbCuentasContablesLN : BaseLN<CtbCuentasContables>
    {
        public override bool Agregar(CtbCuentasContables pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            pParametro.FechaAlta = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "CtbCuentasContablesValidaciones"))
                        resultado = false;

                    if (resultado)
                    {
                        pParametro.IdCuentaContable = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CtbCuentasContablesInsertar");
                        if (pParametro.IdCuentaContable == 0)
                            resultado = false;
                    }

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
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        public override bool Modificar(CtbCuentasContables pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CtbCuentasContables cuentaContableViejo = new CtbCuentasContables();
            cuentaContableViejo.IdCuentaContable = pParametro.IdCuentaContable;
            cuentaContableViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            cuentaContableViejo = this.ObtenerDatosCompletos(cuentaContableViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "CtbCuentasContablesValidaciones"))
                        resultado = false;

                    if (resultado && !this.Modificar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(cuentaContableViejo, Acciones.Update, pParametro, bd, tran))
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
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        public override CtbCuentasContables ObtenerDatosCompletos(CtbCuentasContables pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CtbCuentasContables>("CtbCuentasContablesSeleccionar", pParametro);
        }

        /// <summary>
        /// Devuelve el proximo numero de Imputación contable. Todos los parametros son obligatorios
        /// </summary>
        /// <param name="pParametro">IdCapitulo, IdRubro, IdMoneda, IdDepartamento, IdSubRubro</param>
        /// <returns></returns>
        public CtbCuentasContables ObtenerImputacion(CtbCuentasContables pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CtbCuentasContables>("[CtbCuentasContablesObtenerImputacion]", pParametro);
        }

        public override List<CtbCuentasContables> ObtenerListaFiltro(CtbCuentasContables pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbCuentasContables>("CtbCuentasContablesSeleccionarFiltro", pParametro);
        }

        public List<CtbCuentasContables> ObtenerListaFiltroPopUp(CtbCuentasContables pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbCuentasContables>("CtbCuentasContablesSeleccionarFiltroPopUp", pParametro);
        }

        public List<CtbCuentasContables> ObtenerPorEjercicioNumeroCuenta(CtbAsientosContables pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbCuentasContables>("CtbCuentasContablesSeleccionarPorEjercicioNumeroCuenta", pParametro);
        }
        private bool Modificar(CtbCuentasContables pParametro, Database bd, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CtbCuentasContablesActualizar"))
                return false;

            return true;
        }

        public List<CtbCuentasContables> ObtenerListaRama()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbCuentasContables>("CtbCuentasContablesObtenerListaRama");
        }

        public List<CtbCuentasContables> ObtenerListaRamaPorIdEjercicio(CtbCuentasContables pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbCuentasContables>("CtbCuentasContablesObtenerListaRamaPorIdEjercicioContable", pParametro);
        }

        public CtbCuentasContables ObtenerSeleccionarRama(CtbCuentasContables pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CtbCuentasContables>("CtbCuentasContablesObtenerSeleccionarRama", pParametro);
        }

        /// <summary>
        /// Devuelve las cuentas contables de una RAMA
        /// </summary>
        /// <param name="pParametro">IdCuentaContableRama</param>
        /// <returns></returns>
        public List<CtbCuentasContables> ObtenerPorRama(CtbCuentasContables pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbCuentasContables>("CtbCuentasContablesSeleccionarPorRama", pParametro);
        }

        public DataTable ObenterCuentasContablesImputables(CtbCuentasContables pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CtbCuentasContablesSeleccionarImputablesPlantilla", pParametro);
        }
    }
}
