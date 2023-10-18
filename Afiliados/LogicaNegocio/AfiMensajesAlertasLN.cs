using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Afiliados.Entidades;
using Comunes;
using Servicio.AccesoDatos;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Auditoria;
using Auditoria.Entidades;

namespace Afiliados.LogicaNegocio
{
    class AfiMensajesAlertasLN : BaseLN<AfiMensajesAlertas>
    {
        public override AfiMensajesAlertas ObtenerDatosCompletos(AfiMensajesAlertas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<AfiMensajesAlertas>("AfiMensajesAlertasSeleccionar", pParametro);
        }

        public override List<AfiMensajesAlertas> ObtenerListaFiltro(AfiMensajesAlertas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiMensajesAlertas>("AfiMensajesAlertasSeleccionarFiltro", pParametro);
        }

        public override bool Agregar(AfiMensajesAlertas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            //if (!this.Validar(pParametro, new CapSolicitudPago()))
            //    return false;

            //pParametro.FechaEvento = DateTime.Now;
            pParametro.Estado.IdEstado = (int)Estados.Activo;
            pParametro.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuario;
            pParametro.FechaAlta = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {

                    pParametro.IdMensajeAlerta = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "AfiMensajesAlertasInsertar");
                    if (pParametro.IdMensajeAlerta == 0)
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
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        public override bool Modificar(AfiMensajesAlertas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //GUARDO VALOR ANTERIOR PARA LA AUDITORIA
            AfiMensajesAlertas pValorViejo = new AfiMensajesAlertas();
            pValorViejo.IdMensajeAlerta = pParametro.IdMensajeAlerta;
            pValorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            pValorViejo = ObtenerDatosCompletos(pValorViejo);

            pParametro.FechaEvento = DateTime.Now;
            pParametro.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuario;
            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "AfiMensajesAlertasActualizar"))
                        resultado = false;

                    if (resultado && resultado && !AuditoriaF.AuditoriaAgregar(pValorViejo, Acciones.Update, pParametro, bd, tran))
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
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }
    }
}
