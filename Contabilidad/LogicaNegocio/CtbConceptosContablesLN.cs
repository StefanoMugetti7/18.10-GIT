using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Contabilidad.Entidades;
using Servicio.AccesoDatos;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Auditoria;
using Auditoria.Entidades;

namespace Contabilidad.LogicaNegocio
{
    class CtbConceptosContablesLN : BaseLN<CtbConceptosContables>
    {
        public override CtbConceptosContables ObtenerDatosCompletos(CtbConceptosContables pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CtbConceptosContables>("CtbConceptosContablesSeleccionar", pParametro);
            List<TGETiposOperaciones> tiposOperaciones = BaseDatos.ObtenerBaseDatos().ObtenerLista<TGETiposOperaciones>("CtbConceptosContablesTipoOperacionSeleccionarPorCtbConceptosContables", pParametro);
            foreach (var tipoOperacion in tiposOperaciones)
            {
                TGETiposOperaciones operacion = BaseDatos.ObtenerBaseDatos().Obtener<TGETiposOperaciones>("TGETiposOperacionesSeleccionar", tipoOperacion);
                pParametro.TiposOperaciones.Add(operacion);
            }
            return pParametro;
        }

        public override List<CtbConceptosContables> ObtenerListaFiltro(CtbConceptosContables pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbConceptosContables>("CtbConceptosContablesSeleccionarFiltro", pParametro);
        }

        public List<CtbConceptosContables> ObtenerListaFiltro(TGETiposOperaciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbConceptosContables>("CtbConceptosContablesSeleccionarFiltro", pParametro);
        }

        public List<CtbConceptosContables> ObtenerListaFiltroCompleta(CtbConceptosContables pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbConceptosContables>("CtbConceptosContablesCuentasContablesSeleccionarFiltro", pParametro);
        }

        public override bool Agregar(CtbConceptosContables pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdConceptoContable = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CtbConceptosContablesInsertar");
                    if (pParametro.IdConceptoContable == 0)
                        resultado = false;

                    if (resultado && !this.ActualizarTiposOperacioes(pParametro, bd, tran))
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

        public override bool Modificar(CtbConceptosContables pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CtbConceptosContables conceptoContableViejo = new CtbConceptosContables();
            conceptoContableViejo.IdConceptoContable = pParametro.IdConceptoContable;
            conceptoContableViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            conceptoContableViejo = this.ObtenerDatosCompletos(conceptoContableViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = this.Modificar(pParametro, bd, tran);

                    if (resultado && !this.ActualizarTiposOperacioes(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(conceptoContableViejo, Acciones.Update, pParametro, bd, tran))
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

        public bool Modificar(CtbConceptosContables pParametro, Database bd, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CtbConceptosContablesActualizar"))
                return false;

            return true;
        }

        private bool ActualizarTiposOperacioes(CtbConceptosContables pParametro, Database bd, DbTransaction tran)
        {
            foreach (TGETiposOperaciones tipoOperacion in pParametro.TiposOperaciones)
            {
                CtbConceptosContablesTipoOperacion conceptoContableTipoOperacion = new CtbConceptosContablesTipoOperacion();
                conceptoContableTipoOperacion.IdConceptoContable = pParametro.IdConceptoContable;
                conceptoContableTipoOperacion.IdTipoOperacion = tipoOperacion.IdTipoOperacion;
                switch (tipoOperacion.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:                        
                        BaseDatos.ObtenerBaseDatos().Agregar(conceptoContableTipoOperacion, bd, tran, "CtbConceptosContablesTipoOperacionInsertar");
                        break;
                    case EstadoColecciones.Borrado:
                        BaseDatos.ObtenerBaseDatos().Agregar(conceptoContableTipoOperacion, bd, tran, "CtbConceptosContablesTipoOperacionBorrar");
                        break;
                    default:
                        break;
                }
            }
            return true;
        }
    }
}
