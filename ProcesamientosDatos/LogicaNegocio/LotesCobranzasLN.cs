using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Generales.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ProcesosDatos.Entidades;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace ProcesosDatos
{
    public class LotesCobranzasLN : BaseLN<CarTiposCargosLotesEnviados>
    {
        public override CarTiposCargosLotesEnviados ObtenerDatosCompletos(CarTiposCargosLotesEnviados pParametro)
        {
            throw new System.NotImplementedException();
        }

        public override List<CarTiposCargosLotesEnviados> ObtenerListaFiltro(CarTiposCargosLotesEnviados pParametro)
        {
            throw new System.NotImplementedException();
        }

        public override bool Agregar(CarTiposCargosLotesEnviados pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            if (!this.Validar(pParametro))
                return false;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                   if(!BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(pParametro, bd, tran, "CarProcesoLevantamientoCargosAplicacionCobranzaV2"))
                    {
                        resultado = false;
                    }

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

        private bool Validar(CarTiposCargosLotesEnviados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CarProcesoLevantamientoCargosAplicacionCobranzaValidaciones");
        }
        public override bool Modificar(CarTiposCargosLotesEnviados pParametro)
        {
            throw new System.NotImplementedException();
        }
        public List<TGEFormasCobros> ObtenerFormasCobros()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TGEFormasCobros>("CarTiposCargosLotesEnviadosObtenerFormasCobros");
        }
        public List<CarTiposCargosLotesEnviados> ObtenerPeriodos()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CarTiposCargosLotesEnviados>("CarTiposCargosLotesEnviadosObtenerPeriodos");
        }
        public DataTable ObtenerListaDetalles(CarTiposCargosLotesEnviados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("[CarTiposCargosLotesEnviadosSeleccionarFiltro]", pParametro);
        }
        public List<CarTiposCargosLotesEnviadosDetalles> ObtenerDetalles(CarTiposCargosLotesEnviadosDetalles pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CarTiposCargosLotesEnviadosDetalles>("CarTiposCargosLotesEnviadosDetallesSeleccionar", pParametro);
        }
        public List<CarTiposCargosLotesEnviadosDetalles> ObtenerDetallesPorAfiliado(CarTiposCargosLotesEnviadosDetalles pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CarTiposCargosLotesEnviadosDetalles>("CarTiposCargosLotesEnviadosDetallesSeleccionarPorAfiliado", pParametro);
        }
        public DataTable ObtenerListaDetallesPaginado(CarTiposCargosLotesEnviadosDetalles pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("[CarTiposCargosLotesEnviadosDetallesSeleccionarGrilla]", pParametro);
        }

        public List<CarTiposCargosLotesEnviadosDetalles> ObtenerDetallesPorId(CarTiposCargosLotesEnviados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CarTiposCargosLotesEnviadosDetalles>("CarTiposCargosLotesEnviadosDetallesSeleccionarGrillaPorId", pParametro);
        }
    }
}