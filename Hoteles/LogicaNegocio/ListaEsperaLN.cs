using Comunes;
using Comunes.Entidades;
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
using System.Threading.Tasks;

namespace Hoteles.LogicaNegocio
{
    class ListaEsperaLN : BaseLN<HTLListaEspera>
    {
        public override HTLListaEspera ObtenerDatosCompletos(HTLListaEspera pReserva)
        {
            HTLListaEspera presupuesto = BaseDatos.ObtenerBaseDatos().Obtener<HTLListaEspera>("HTLListaEsperaSeleccionar", pReserva);
            return presupuesto;
        }

        public override List<HTLListaEspera> ObtenerListaFiltro(HTLListaEspera pReserva)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<HTLListaEspera>("HTLListaEsperaSeleccionarDescripcionPorFiltro", pReserva);
        }

        public DataTable ObtenerListaGrilla(HTLListaEspera pReserva)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("HTLListaEsperaSeleccionarDescripcionPorFiltro", pReserva);
        }

        public override bool Agregar(HTLListaEspera pParametro)
        {
            if (pParametro.IdListaEspera > 0)
                return true;

            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            pParametro.Fecha = DateTime.Now;

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
                    //resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "HTLReservasValidaciones");

                    if (resultado && !this.Agregar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoHotelAgregar";
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdListaEspera.ToString());
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
        internal bool Agregar(HTLListaEspera pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdListaEspera = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "HTLListaEsperaInsertar");
            if (pParametro.IdListaEspera == 0)
                return false;

            return true;
        }
        public override bool Modificar(HTLListaEspera pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            if (!this.Validaciones(pParametro))
                return false;

            HTLListaEspera valorViejo = new HTLListaEspera();
            valorViejo.IdListaEspera = pParametro.IdListaEspera;
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
                    //resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "HTLReservasValidaciones");

                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "HTLListaEsperaActualizar"))
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
        private bool Validaciones(HTLListaEspera pParametro)
        {
            if(pParametro.Cantidad < 1)
            {
                pParametro.CodigoMensaje = "ValidarReservasItemsDescuentosDuplicados";
                return false;
            }
            return true;
        }
    }
}
