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
    class CtbCapitulosLN : BaseLN<CtbCapitulos>
    {
        public override CtbCapitulos ObtenerDatosCompletos(CtbCapitulos pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CtbCapitulos>("CtbCapitulosSeleccionar", pParametro);
            return pParametro;
        }

        public List<CtbCapitulos> ObtenerLista(CtbCapitulos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbCapitulos>("CtbCapitulosListar", pParametro);
        }

        public override List<CtbCapitulos> ObtenerListaFiltro(CtbCapitulos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbCapitulos>("CtbCapitulosSeleccionarFiltro", pParametro);
        }

        public override bool Agregar(CtbCapitulos pParametro)
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
                    pParametro.IdCapitulo = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CtbCapitulosInsertar");
                    if (pParametro.IdCapitulo == 0)
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

        public override bool Modificar(CtbCapitulos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CtbCapitulos capituloViejo = new CtbCapitulos();
            capituloViejo.IdCapitulo = pParametro.IdCapitulo;
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

        public bool Modificar(CtbCapitulos pParametro, Database bd, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CtbCapitulosActualizar"))
                return false;

            return true;
        }
    }
}
