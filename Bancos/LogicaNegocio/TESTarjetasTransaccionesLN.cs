using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Bancos.Entidades;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Servicio.AccesoDatos;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Bancos.LogicaNegocio
{
    class TESTarjetasTransaccionesLN : BaseLN<TESTarjetasTransacciones>
    {
        public override bool Agregar(TESTarjetasTransacciones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            //if (!this.Validar(pParametro))
            //    return false;

            pParametro.FechaEvento = DateTime.Now;
            pParametro.Estado.IdEstado = (int)Estados.Activo;
            pParametro.Fecha = DateTime.Now;
            //pParametro.FechaAlta = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {

                    pParametro.IdTarjetaTransaccion = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "TESTarjetasTransaccionesInsertar");
                    if (pParametro.IdTarjetaTransaccion == 0)
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

        public override bool Modificar(TESTarjetasTransacciones pParametro)
        {
            throw new NotImplementedException();
        }

        public override TESTarjetasTransacciones ObtenerDatosCompletos(TESTarjetasTransacciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<TESTarjetasTransacciones>("TESTarjetasTransaccionesSeleccionar", pParametro);
        }

        public override List<TESTarjetasTransacciones> ObtenerListaFiltro(TESTarjetasTransacciones pParametro)
        {
            throw new NotImplementedException();
        }

        public bool Agregar(TESTarjetasTransacciones pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdTarjetaTransaccion = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "TESTarjetasTransaccionesInsertar");
            if (pParametro.IdTarjetaTransaccion == 0)
                return false;

            return true;
        }

        public bool ModificarEstado(TESTarjetasTransacciones pParametro, Database db, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, db, tran, "TESTarjetasTransaccionesActualizarEstado"))
                return false;

            return true;
        }
    }
}
