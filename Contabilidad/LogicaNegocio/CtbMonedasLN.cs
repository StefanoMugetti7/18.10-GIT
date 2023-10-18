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
    class CtbMonedasLN : BaseLN<CtbMonedas>
    {
        public override CtbMonedas ObtenerDatosCompletos(CtbMonedas pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CtbMonedas>("CtbMonedasSeleccionar", pParametro);
            return pParametro;
        }

        public List<CtbMonedas> ObtenerLista(CtbMonedas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbMonedas>("CtbMonedasListar", pParametro);
        }

        public override List<CtbMonedas> ObtenerListaFiltro(CtbMonedas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbMonedas>("CtbMonedasSeleccionarFiltro", pParametro);
        }

        public override bool Agregar(CtbMonedas pParametro)
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
                    pParametro.IdMoneda = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CtbMonedasInsertar");
                    if (pParametro.IdMoneda == 0)
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

        public override bool Modificar(CtbMonedas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CtbMonedas monedaVieja = new CtbMonedas();
            monedaVieja.IdMoneda = pParametro.IdMoneda;
            monedaVieja.UsuarioLogueado = pParametro.UsuarioLogueado;
            monedaVieja = this.ObtenerDatosCompletos(monedaVieja);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = this.Modificar(pParametro, bd, tran);

                    if (resultado && !AuditoriaF.AuditoriaAgregar(monedaVieja, Acciones.Update, pParametro, bd, tran))
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

        public bool Modificar(CtbMonedas pParametro, Database bd, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CtbMonedasActualizar"))
                return false;

            return true;
        }
    }
}
