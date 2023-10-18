using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Contabilidad.Entidades;
using Servicio.AccesoDatos;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Auditoria;
using Auditoria.Entidades;
using System.Data;

namespace Contabilidad.LogicaNegocio
{
    class CtbAsientosContablesCuentasContablesParametrosLN : BaseLN<CtbAsientosContablesCuentasContablesParametros>
    {
        public override CtbAsientosContablesCuentasContablesParametros ObtenerDatosCompletos(CtbAsientosContablesCuentasContablesParametros pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CtbAsientosContablesCuentasContablesParametros>("CtbAsientosContablesCuentasContablesParametrosSeleccionar", pParametro);
            return pParametro;
        }

        /// <summary>
        /// Devuelve el primer registro que coincide con los filtros aplicados
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public CtbAsientosContablesCuentasContablesParametros ObtenerCuentaContable(CtbAsientosContablesCuentasContablesParametros pParametro, Database bd, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CtbAsientosContablesCuentasContablesParametros>("CtbAsientosContablesCuentasContablesParametrosFiltro", pParametro, bd, tran);
        }

        public List<CtbAsientosContablesCuentasContablesParametros> ObtenerLista(CtbAsientosContablesCuentasContablesParametros pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbAsientosContablesCuentasContablesParametros>("CtbAsientosContablesCuentasContablesParametrosListar", pParametro);
        }

        public override List<CtbAsientosContablesCuentasContablesParametros> ObtenerListaFiltro(CtbAsientosContablesCuentasContablesParametros pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbAsientosContablesCuentasContablesParametros>("CtbAsientosContablesCuentasContablesParametrosFiltro", pParametro);
        }

        public DataTable ObtenerListaFiltroDT(CtbAsientosContablesCuentasContablesParametros pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CtbAsientosContablesCuentasContablesParametrosFiltro", pParametro);
        }

        public override bool Agregar(CtbAsientosContablesCuentasContablesParametros pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            //pParametro.FechaAlta = DateTime.Now;
            if (!this.ValidarCuentaContableParametro(pParametro))
                return false;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdAsientoContableCuentaContableParametro = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CtbAsientosContablesCuentasContablesParametrosInsertar");
                    if (pParametro.IdAsientoContableCuentaContableParametro == 0)
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

        public override bool Modificar(CtbAsientosContablesCuentasContablesParametros pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            if (!this.ValidarCuentaContableParametro(pParametro))
                return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CtbAsientosContablesCuentasContablesParametros capituloViejo = new CtbAsientosContablesCuentasContablesParametros();
            capituloViejo.IdAsientoContableCuentaContableParametro = pParametro.IdAsientoContableCuentaContableParametro;
            capituloViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            capituloViejo = this.ObtenerDatosCompletos(capituloViejo);  

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = this.Modificar(pParametro, bd, tran);

                    if (resultado && !AuditoriaF.AuditoriaAgregar(capituloViejo, Acciones.Update, pParametro, bd, tran))
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

        public bool Modificar(CtbAsientosContablesCuentasContablesParametros pParametro, Database bd, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CtbAsientosContablesCuentasContablesParametrosActualizar"))
                return false;

            return true;
        }

        private bool ValidarCuentaContableParametro(CtbAsientosContablesCuentasContablesParametros pParametro)
        {
            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CtbAsientosContablesCuentasContablesParametrosValidarCuentaContable"))
            {
                pParametro.CodigoMensaje = "ValidarCuentaContableParametroExiste";
                return false;
            }
            return true;
        }
    }
}