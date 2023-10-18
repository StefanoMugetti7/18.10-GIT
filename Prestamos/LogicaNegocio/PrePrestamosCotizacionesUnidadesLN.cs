using Comunes;
using Comunes.LogicaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prestamos.Entidades;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prestamos.LogicaNegocio
{
    class PrePrestamosCotizacionesUnidadesLN : BaseLN<PrePrestamosCotizacionesUnidades>
    {
        public override bool Modificar(PrePrestamosCotizacionesUnidades pParametro)
        {
            throw new NotImplementedException();
        }

        public override PrePrestamosCotizacionesUnidades ObtenerDatosCompletos(PrePrestamosCotizacionesUnidades pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<PrePrestamosCotizacionesUnidades>("PrePrestamosCotizacionesUnidadesListar", pParametro);
        }

        public override List<PrePrestamosCotizacionesUnidades> ObtenerListaFiltro(PrePrestamosCotizacionesUnidades pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<PrePrestamosCotizacionesUnidades>("PrePrestamosCotizacionesUnidadesSeleccionarPorFiltro", pParametro);
        }

        public DataTable ObtenerListaGrilla(PrePrestamosCotizacionesUnidades pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("PrePrestamosCotizacionesUnidadesSeleccionarPorFiltro", pParametro);
        }


        public override bool Agregar(PrePrestamosCotizacionesUnidades pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {

                    if (resultado && !this.Agregar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdCotizacionUnidad.ToString());
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

        internal bool Agregar(PrePrestamosCotizacionesUnidades pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdCotizacionUnidad = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "PrePrestamosCotizacionesUnidadesInsertar");
            if (pParametro.IdCotizacionUnidad == 0)
                return false;

            return true;
        }
    }
}
