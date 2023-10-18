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

namespace Contabilidad.LogicaNegocio
{
    class CtbAsientosModelosDetallesLN : BaseLN<CtbAsientosModelosDetalles>
    {
        public override bool Agregar(CtbAsientosModelosDetalles pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            pParametro.FechaEvento = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdAsientoModeloDetalle = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CtbAsientosModelosDetallesInsertar");
                    if (pParametro.IdAsientoModeloDetalle == 0)
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

        public override bool Modificar(CtbAsientosModelosDetalles pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CtbAsientosModelosDetalles asientoModeloViejo = new CtbAsientosModelosDetalles();
            asientoModeloViejo.IdAsientoModeloDetalle = pParametro.IdAsientoModeloDetalle;
            asientoModeloViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            asientoModeloViejo = this.ObtenerDatosCompletos(asientoModeloViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = this.Modificar(pParametro, bd, tran);

                    if (resultado && !AuditoriaF.AuditoriaAgregar(asientoModeloViejo, Acciones.Update, pParametro, bd, tran))
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

        public override CtbAsientosModelosDetalles ObtenerDatosCompletos(CtbAsientosModelosDetalles pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CtbAsientosModelosDetalles>("CtbAsientosModelosDetallesSeleccionar", pParametro);
            return pParametro;
        }

        public override List<CtbAsientosModelosDetalles> ObtenerListaFiltro(CtbAsientosModelosDetalles pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbAsientosModelosDetalles>("CtbAsientosModelosDetallesListarFiltro", pParametro);
        }

        public bool Modificar(CtbAsientosModelosDetalles pParametro, Database bd, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CtbAsientosModelosDetallesActualizar"))
                return false;

            return true;
        }
    }
}
