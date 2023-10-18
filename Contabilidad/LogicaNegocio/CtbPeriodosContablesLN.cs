using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using Contabilidad.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Servicio.AccesoDatos;
using Auditoria;
using Auditoria.Entidades;

namespace Contabilidad.LogicaNegocio
{
    class CtbPeriodosContablesLN : BaseLN<CtbPeriodosContables>
    {
        public override bool Agregar(CtbPeriodosContables pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            pParametro.FechaCierre = DateTime.Now;
            pParametro.FechaEvento = DateTime.Now;
            

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (!this.Validar(pParametro, bd, tran))
                        return false;

                    pParametro.IdPeriodoContable = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CtbPeriodosContablesInsertar");
                    if (pParametro.IdPeriodoContable == 0)
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

        private bool Validar(CtbPeriodosContables pParametro, Database bd, DbTransaction tran)
        {
            //string fechaFin = pParametro.EjercicioContable.FechaFin.Year.ToString();
            //fechaFin = string.Concat(fechaFin, pParametro.EjercicioContable.FechaFin.Month.ToString().PadLeft(2, '0'));

            //if (pParametro.Periodo > Convert.ToInt32(fechaFin))
            //{
            //    pParametro.CodigoMensaje = "EjercicioContableSuperado";
            //    return false;
            //}

            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CtbPeriodosContablesValidaciones"))
            {
                return false;
            }

            return true;
            
        }

        /// <summary>
        /// Valida si un Periodo esta Cerrado
        /// Devuelve True si esta Cerrado
        /// </summary>
        /// <param name="pParametro">Periodo</param>
        /// <returns></returns>
        public bool ValidarCierre(CtbPeriodosContables pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CtbPeriodosContablesValidarCerrado");
        }

        /// <summary>
        /// Valida si un Periodo esta Cerrado
        /// Devuelve True si esta Cerrado
        /// </summary>
        /// <param name="pParametro">Periodo</param>
        /// <returns></returns>
        public bool ValidarCierre(CtbPeriodosContables pParametro, Database bd, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "CtbPeriodosContablesValidarCerrado");
        }

        public override bool Modificar(CtbPeriodosContables pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CtbPeriodosContables periodoContableViejo = new CtbPeriodosContables();
            periodoContableViejo.IdPeriodoContable = pParametro.IdPeriodoContable;
            periodoContableViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            periodoContableViejo = this.ObtenerDatosCompletos(periodoContableViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (!this.Validar(pParametro, bd, tran))
                        return false;

                    resultado = this.Modificar(pParametro, bd, tran);

                    if (resultado && !AuditoriaF.AuditoriaAgregar(periodoContableViejo, Acciones.Update, pParametro, bd, tran))
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

        public override CtbPeriodosContables ObtenerDatosCompletos(CtbPeriodosContables pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CtbPeriodosContables>("CtbPeriodosContablesSeleccionar", pParametro);
        }

        public override List<CtbPeriodosContables> ObtenerListaFiltro(CtbPeriodosContables pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbPeriodosContables>("CtbPeriodosContablesListarFiltro", pParametro);
        }

        public List<CtbPeriodosContables> ObtenerLista(CtbPeriodosContables pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbPeriodosContables>("CtbPeriodosContablesListar", pParametro);
        }

        public CtbPeriodosContables ObtenerUltimoCerrado(CtbPeriodosContables pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CtbPeriodosContables>("CtbPeriodosContablesObtenerUltimoCerrado", pParametro);
        }

        public bool Modificar(CtbPeriodosContables pParametro, Database bd, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CtbPeriodosContablesActualizar"))
                return false;

            return true;
        }

        public CtbPeriodosContables ObtenerUltimoCerrado()
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CtbPeriodosContables>("CtbPeriodosContablesObtenerUltimoCerradoSinParam");
        }
    }
}
