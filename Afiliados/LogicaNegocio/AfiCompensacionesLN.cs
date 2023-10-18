using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Afiliados.Entidades;
using Comunes;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Servicio.AccesoDatos;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Auditoria;
using Auditoria.Entidades;

namespace Afiliados.LogicaNegocio
{
    class AfiCompensacionesLN : BaseLN<AfiCompensaciones>
    {
        public override AfiCompensaciones ObtenerDatosCompletos(AfiCompensaciones pParametro)
        {
            AfiCompensaciones compensacion = new AfiCompensaciones();
            compensacion =  BaseDatos.ObtenerBaseDatos().Obtener<AfiCompensaciones>("AfiCompensacionesSeleccionar", pParametro);
            compensacion.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            compensacion.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return compensacion;
        }

        public override List<AfiCompensaciones> ObtenerListaFiltro(AfiCompensaciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiCompensaciones>("AfiCompensacionesObtenerListaFiltro", pParametro);
        }

        public override bool Agregar(AfiCompensaciones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            //if (!this.Validar(pParametro))
            //    return false;

            pParametro.FechaEvento = DateTime.Now;
            pParametro.Estado.IdEstado = (int)Estados.Activo;
            
            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdCompensacion = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "AfiCompensacionesInsertar");
                    if (pParametro.IdCompensacion == 0)
                        resultado = false;

                    //Control Comentarios y Archivos
                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;
                    //Fin control Comentarios y Archivos

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

        public override bool Modificar(AfiCompensaciones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //if (!this.Validar(pParametro))
            //    return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            AfiCompensaciones valorViejo = new AfiCompensaciones();
            valorViejo.IdCompensacion = pParametro.IdCompensacion;
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
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "AfiCompensacionesActualizar"))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        if (!pParametro.Concurrencia)
                            pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = ex.Message;
                    return false;
                }
            }
            return resultado;
        }

        public bool Anular(AfiCompensaciones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Borrado;
            pParametro.Estado.IdEstado = (int)Estados.Baja;
            
            //if (!this.Validar(pParametro))
            //    return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            AfiCompensaciones valorViejo = new AfiCompensaciones();
            valorViejo.IdCompensacion = pParametro.IdCompensacion;
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
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "AfiCompensacionesActualizarEstado"))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        if (!pParametro.Concurrencia)
                            pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = ex.Message;
                    return false;
                }
            }
            return resultado;
        }
    }
}
