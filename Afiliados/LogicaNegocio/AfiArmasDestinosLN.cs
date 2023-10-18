using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Afiliados.Entidades;
using Comunes;
using Comunes.Entidades;
using Servicio.AccesoDatos;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Generales.FachadaNegocio;
using Auditoria;
using Auditoria.Entidades;
using System.Collections;
using Comunes.LogicaNegocio;
namespace Afiliados.LogicaNegocio
{
    class AfiArmasDestinosLN : BaseLN<AfiArmasDestinos>
    {


        public override AfiArmasDestinos ObtenerDatosCompletos(AfiArmasDestinos pParametro)
        {
           return BaseDatos.ObtenerBaseDatos().Obtener<AfiArmasDestinos>( "AfiArmasDestinosSeleccionar", pParametro);
        }

        public override List<AfiArmasDestinos> ObtenerListaFiltro(AfiArmasDestinos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiArmasDestinos>("AfiArmasDestinosSeleccionarDescripcionFiltro", pParametro);
        }

        public override bool Agregar(AfiArmasDestinos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            //if (!this.Validar(pParametro, new CapSolicitudPago()))
            //    return false;

            pParametro.FechaEvento = DateTime.Now;
            pParametro.Estado.IdEstado = (int)Estados.Activo;
            pParametro.FechaAlta = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {

                    pParametro.IdArmaDestino = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "AfiArmasDestinosInsertar");
                    if (pParametro.IdArmaDestino == 0)
                        resultado = false;

                    //if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    //if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                    //    resultado = false;

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

        public override bool Modificar(AfiArmasDestinos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            AfiArmasDestinos valorViejo = new AfiArmasDestinos();
            valorViejo.IdArmaDestino = pParametro.IdArmaDestino;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            //if (!this.Validar(pParametro, valorViejo))
            //    return false;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "AfiArmasDestinosActualizar"))
                        resultado = false;

                    //if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    //if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    if (resultado)
                    {
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
