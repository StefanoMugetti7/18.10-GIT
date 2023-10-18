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
using Auditoria.Entidades;
using Auditoria;

namespace Contabilidad.LogicaNegocio
{
    class CtbSubRubrosLN : BaseLN<CtbSubRubros>
    {
        public override CtbSubRubros ObtenerDatosCompletos(CtbSubRubros pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CtbSubRubros>("CtbSubRubrosSeleccionar", pParametro);
            return pParametro;
        }

        public List<CtbSubRubros> ObtenerLista(CtbSubRubros pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbSubRubros>("CtbSubRubrosListar", pParametro);
        }

        public override List<CtbSubRubros> ObtenerListaFiltro(CtbSubRubros pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbSubRubros>("CtbSubRubrosSeleccionarFiltro", pParametro);
        }

        public override bool Agregar(CtbSubRubros pParametro)
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
                    pParametro.IdSubRubro = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CtbSubRubrosInsertar");
                    if (pParametro.IdSubRubro == 0)
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

        public override bool Modificar(CtbSubRubros pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CtbSubRubros subRubroViejo = new CtbSubRubros();
            subRubroViejo.IdSubRubro = pParametro.IdSubRubro;
            subRubroViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            subRubroViejo = this.ObtenerDatosCompletos(subRubroViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = this.Modificar(pParametro, bd, tran);

                    if (resultado && !AuditoriaF.AuditoriaAgregar(subRubroViejo, Acciones.Update, pParametro, bd, tran))
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

        public bool Modificar(CtbSubRubros pParametro, Database bd, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CtbSubRubrosActualizar"))
                return false;

            return true;
        }
    }
}
