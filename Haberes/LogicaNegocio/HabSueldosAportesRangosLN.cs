using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Haberes.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haberes.LogicaNegocio
{
    public class HabSueldosAportesRangosLN : BaseLN<HabSueldosAportesRangos>
    {
        public override bool Agregar(HabSueldosAportesRangos pParametro)
        {
            bool resultado = true;

            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;


            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdSueldoAporteRango = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "HabSueldosAportesRangosInsertar");
                    if (pParametro.IdSueldoAporteRango == 0)
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        tran.Rollback();
                        pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
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

        public override bool Modificar(HabSueldosAportesRangos pParametro)
        {
            int resultado = 0;

            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().EjecutarSP(pParametro, "HabSueldosAportesRangosModificar");

                    if (resultado == 1)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        tran.Rollback();
                        pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
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

            if (resultado == 1)
                return true;
            else
                return false;
        }

        public override HabSueldosAportesRangos ObtenerDatosCompletos(HabSueldosAportesRangos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<HabSueldosAportesRangos>("HabSueldosAportesRangosSeleccionar", pParametro);
        }

        public override List<HabSueldosAportesRangos> ObtenerListaFiltro(HabSueldosAportesRangos pParametro)
        {
            throw new NotImplementedException();
        }

        public DataTable ObtenerDataTable(HabSueldosAportesRangos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("HabSueldosAportesRangosSeleccionarFiltro", pParametro);
        }
    }
}
