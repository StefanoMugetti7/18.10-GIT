using Comunes;
using Comunes.LogicaNegocio;
using Generales.FachadaNegocio;
using Hoteles.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hoteles.LogicaNegocio
{
    class HotelesLN : BaseLN<HTLHoteles>
    {
        public override List<HTLHoteles> ObtenerListaFiltro(HTLHoteles pParametro)
        {
            throw new NotImplementedException();
        }

        public List<HTLHoteles> ObtenerListaActiva(HTLHoteles pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<HTLHoteles>("HTLHotelesSeleccionarListaActiva", pParametro);
        }

        public DataTable ObtenerListaGrilla(HTLHoteles pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("[HTLHotelesSeleccionarDescripcionPorFiltro]", pParametro);
        }

        public override HTLHoteles ObtenerDatosCompletos(HTLHoteles pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<HTLHoteles>("HTLHotelSeleccionar", pParametro);
        }

        public override bool Agregar(HTLHoteles pParametro)
        {
            if (pParametro.IdHotel > 0)
                return true;

            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            if (!this.Validaciones(pParametro))
                return false;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = Validaciones(pParametro);

                    if (resultado && !this.Agregar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoHotelAgregar";
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdHotel.ToString());
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

        internal bool Agregar(HTLHoteles pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdHotel = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "HTLHotelesInsertar");
            if (pParametro.IdHotel == 0)
                return false;

            return true;
        }

        public override bool Modificar(HTLHoteles pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;

            if (!this.Validaciones(pParametro))
                return false;

            HTLHoteles valorViejo = new HTLHoteles();
            valorViejo.IdHotel = pParametro.IdHotel;
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
                    resultado = Validaciones(pParametro);

                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "HTLHotelesActualizar"))
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

        private bool Validaciones(HTLHoteles pParametro)
        {
            return true;
        }
    }
}
