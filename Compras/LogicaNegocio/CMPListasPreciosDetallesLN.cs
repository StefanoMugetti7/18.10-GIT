using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Comunes.Entidades;
using Servicio.AccesoDatos;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Generales.FachadaNegocio;
using Auditoria;
using System.Collections;
using Compras.Entidades;
using Comunes.LogicaNegocio;
using Auditoria.Entidades;

namespace Compras.LogicaNegocio
{
    class CMPListasPreciosDetallesLN : BaseLN<CMPListasPreciosDetalles>
    {
        public override CMPListasPreciosDetalles ObtenerDatosCompletos(CMPListasPreciosDetalles pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CMPListasPreciosDetalles>("CMPListasPreciosDetallesSeleccionar", pParametro);
        }

        public CMPListasPreciosDetalles ObtenerDatosPorProducto(CMPListasPreciosDetalles pParametro)
        {
            List<CMPListasPreciosDetalles> lista = this.ObtenerListaFiltro(pParametro);
            if (lista.Count > 0)
                return lista[0];
            else
                return new CMPListasPreciosDetalles();
        }
        
        public override List<CMPListasPreciosDetalles> ObtenerListaFiltro(CMPListasPreciosDetalles pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CMPListasPreciosDetalles>("CMPListasPreciosDetallesSeleccionarPorFiltro", pParametro);
        }

      
        public override bool Agregar(CMPListasPreciosDetalles pParametro)
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

                    pParametro.IdListaPrecioDetalle = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CMPListasPreciosDetallesInsertar");
                    if (pParametro.IdListaPrecioDetalle == 0)
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
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


                return resultado;
            }

        }

  
        public override bool Modificar(CMPListasPreciosDetalles pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Guardo los valores viejos para la auditoria.

            CMPListasPreciosDetalles valorViejo = new CMPListasPreciosDetalles();
            valorViejo.IdListaPrecioDetalle = pParametro.IdListaPrecioDetalle;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = ObtenerDatosCompletos(valorViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CMPListasPreciosDetallesActualizar"))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        pParametro.ConfirmarAccion = false;
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        if (pParametro.Concurrencia)
                            pParametro.CodigoMensaje = "Concurrencia";
                        else
                            pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";

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
