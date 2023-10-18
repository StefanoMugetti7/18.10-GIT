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

namespace Contabilidad.LogicaNegocio
{
    class CtbRubrosLN : BaseLN<CtbRubros>
    {
        public override CtbRubros ObtenerDatosCompletos(CtbRubros pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CtbRubros>("CtbRubrosSeleccionar", pParametro);
            List<CtbRubrosSubRubros> subRubros = BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbRubrosSubRubros>("CtbRubrosSubRubrosSeleccionarPorCtbRubros", pParametro);
            foreach (CtbRubrosSubRubros rubroSubRubro in subRubros)
            {
                CtbSubRubros subRubro = BaseDatos.ObtenerBaseDatos().Obtener<CtbSubRubros>("CtbSubRubrosSeleccionar", rubroSubRubro);
                pParametro.SubRubros.Add(subRubro);
            }
            return pParametro;
        }

        public List<CtbRubros> ObtenerLista(CtbRubros pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbRubros>("CtbRubrosListar", pParametro);
        }

        public override List<CtbRubros> ObtenerListaFiltro(CtbRubros pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbRubros>("CtbRubrosSeleccionarFiltro", pParametro);
        }

        public override bool Agregar(CtbRubros pParametro)
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
                    pParametro.IdRubro = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CtbRubrosInsertar");
                    if (pParametro.IdRubro == 0)
                        resultado = false;

                    //Inserta los subRubros
                    foreach (CtbSubRubros subRubro in pParametro.SubRubros)
                    {
                        resultado = GuardarSubRubro(pParametro, resultado, tran, bd, subRubro);
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

        private static bool GuardarSubRubro(CtbRubros pParametro, bool resultado, DbTransaction tran, Database bd, CtbSubRubros subRubro)
        {
            CtbRubrosSubRubros rubroSubRubros = new CtbRubrosSubRubros();
            rubroSubRubros.IdRubro = pParametro.IdRubro;
            rubroSubRubros.IdSubRubro = subRubro.IdSubRubro;
            rubroSubRubros.Estado.IdEstado = (int)Estados.Activo;
            rubroSubRubros.UsuarioLogueado = pParametro.UsuarioLogueado;
            rubroSubRubros.IdRubroSubRubro = BaseDatos.ObtenerBaseDatos().Agregar(rubroSubRubros, bd, tran, "CtbRubrosSubRubrosInsertar");
            if (rubroSubRubros.IdRubroSubRubro == 0)
                resultado = false;
            return resultado;
        }

        public override bool Modificar(CtbRubros pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CtbRubros rubroViejo = new CtbRubros();
            rubroViejo.IdRubro = pParametro.IdRubro;
            rubroViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            rubroViejo = this.ObtenerDatosCompletos(rubroViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = this.Modificar(pParametro, bd, tran);
                    //Inserta los subRubros
                    foreach (CtbSubRubros subRubro in pParametro.SubRubros)
                    {
                        if (!rubroViejo.SubRubros.Exists(x => x.IdSubRubro == subRubro.IdSubRubro))
                        {
                            resultado = GuardarSubRubro(pParametro, resultado, tran, bd, subRubro);
                        }
                    }
                    //Elimina los subRubros
                    foreach (CtbSubRubros subRubro in rubroViejo.SubRubros)
                    {
                        if (!pParametro.SubRubros.Exists(x => x.IdSubRubro == subRubro.IdSubRubro))
                        {
                            CtbRubrosSubRubros rubroSubRubros = new CtbRubrosSubRubros();
                            rubroSubRubros.IdRubro = pParametro.IdRubro;
                            rubroSubRubros.IdSubRubro = subRubro.IdSubRubro;
                            if (!BaseDatos.ObtenerBaseDatos().Actualizar(rubroSubRubros, bd, tran, "CtbRubrosSubRubrosEliminar"))
                                resultado = false;
                        }
                    }

                    if (resultado && !AuditoriaF.AuditoriaAgregar(rubroViejo, Acciones.Update, pParametro, bd, tran))
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

        public bool Modificar(CtbRubros pParametro, Database bd, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CtbRubrosActualizar"))
                return false;

            return true;
        }
    }
}
