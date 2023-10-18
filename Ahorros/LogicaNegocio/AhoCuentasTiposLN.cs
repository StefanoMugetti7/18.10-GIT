using Ahorros.Entidades;
using Auditoria;
using Auditoria.Entidades;
using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Ahorros.LogicaNegocio
{
    class AhoCuentasTiposLN : BaseLN<AhoCuentasTipos>
    {
        public override AhoCuentasTipos ObtenerDatosCompletos(AhoCuentasTipos pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<AhoCuentasTipos>("AhoCuentasTiposSeleccionar", pParametro);
            pParametro.CuentasContables = BaseDatos.ObtenerBaseDatos().ObtenerLista<AhoCuentasTiposCuentasContables>("AhoCuentasTiposCuentasContablesSeleccionar", pParametro);
            return pParametro;
        }

        /// <summary>
        /// Devuelve una lista de Tipos de Cuentas de Ahorros
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public override List<AhoCuentasTipos> ObtenerListaFiltro(AhoCuentasTipos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AhoCuentasTipos>("AhoCuentasTiposSeleccionarFiltro", pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Tipos de Cuentas de Ahorros en estado Activo
        /// </summary>
        /// <returns></returns>
        public List<AhoCuentasTipos> ObtenerListaActiva()
        {
            AhoCuentasTipos parametro = new AhoCuentasTipos();
            parametro.Estado.IdEstado = (int)Estados.Activo;
            return this.ObtenerListaFiltro(parametro);
        }

        private bool ActualizarItems(AhoCuentasTipos pParametro, DbTransaction tran, Database bd, AhoCuentasTipos valorViejo)
        {
            foreach (AhoCuentasTiposCuentasContables item in pParametro.CuentasContables)
            {
                item.IdCuentaTipo = pParametro.IdCuentaTipo;
                if (item.EstadoColeccion == EstadoColecciones.Agregado && item.IdCuentaContable > 0)
                {
                    item.IdCuentaTipoCuentaContable = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "AhoCuentasTiposCuentasContablesInsertar");
                    if (item.IdCuentaTipoCuentaContable == 0)
                        return false;
                }
                else if (item.EstadoColeccion == EstadoColecciones.Modificado && item.IdCuentaTipoCuentaContable > 0)
                {
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "AhoCuentasTiposCuentasContablesActualizar"))
                        return false;
                }
            }
            if (valorViejo.IdCuentaTipo > 0)
            {
                List<AhoCuentasTiposCuentasContables> retorno = new List<AhoCuentasTiposCuentasContables>();
                foreach (AhoCuentasTiposCuentasContables item in valorViejo.CuentasContables)
                {
                    AhoCuentasTiposCuentasContables aux = pParametro.CuentasContables.Find(x => x.IdCuentaTipoCuentaContable == item.IdCuentaTipoCuentaContable);
                    if (aux == null)
                    {
                        retorno.Add(item);
                    }
                }
                if(retorno.Count > 0)
                {
                    foreach (AhoCuentasTiposCuentasContables item in retorno)
                    {
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "AhoCuentasTiposActualizarBaja"))
                            return false;
                    }
                }
            }
            return true;
        }
        public override bool Agregar(AhoCuentasTipos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            if (!this.Validar(pParametro))
                return false;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdCuentaTipo = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "AhoCuentasTiposInsertar");
                    if (pParametro.IdCuentaTipo == 0)
                        resultado = false;

                    if (resultado)
                    {
                        if (!this.ActualizarItems(pParametro, tran, bd, new AhoCuentasTipos()))
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

        public override bool Modificar(AhoCuentasTipos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            if (!this.Validar(pParametro))
                return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            AhoCuentasTipos valorViejo = new AhoCuentasTipos();
            valorViejo.IdCuentaTipo = pParametro.IdCuentaTipo;
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
                    resultado = this.Modificar(pParametro, bd, tran);

                    if (resultado)
                    {
                        resultado = this.ActualizarItems(pParametro, tran, bd, valorViejo);
                    }

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
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
        public bool Modificar(AhoCuentasTipos pParametro, Database bd, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "AhoCuentasTiposActualizar"))
                return false;

            return true;
        }
        private bool Validar(AhoCuentasTipos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "AhoCuentasTiposValidaciones");
        }
    }
}
