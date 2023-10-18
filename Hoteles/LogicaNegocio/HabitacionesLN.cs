using Auditoria;
using Auditoria.Entidades;
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

namespace Hoteles.LogicaNegocio
{
    class HabitacionesLN : BaseLN<HTLHabitaciones>
    {
        public override List<HTLHabitaciones> ObtenerListaFiltro(HTLHabitaciones pReserva)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<HTLHabitaciones>("HTLHabitacionesSeleccionarDescripcionPorFiltro", pReserva);
        }

        public List<HTLHabitaciones> ObtenerAgenda(HTLHabitaciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<HTLHabitaciones>("HTLHabitacionesSeleccionarAgenda", pParametro);
        }

        public DataTable ObtenerListaGrilla(HTLHabitaciones pReserva)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("HTLHabitacionesSeleccionarDescripcionPorFiltro", pReserva);
        }

        public override HTLHabitaciones ObtenerDatosCompletos(HTLHabitaciones pReserva)
        {
            HTLHabitaciones presupuesto = BaseDatos.ObtenerBaseDatos().Obtener<HTLHabitaciones>("HTLHabitacionesSeleccionar", pReserva);
            presupuesto.HabitacionesDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<HTLHabitacionesDetalles>("HTLHabitacionesDetalleSeleccionar", pReserva);
            return presupuesto;
        }

        //public HTLReservasDetalles ObtenerHabitacionDetalle(HTLReservasDetalles pParamtro)
        //{
        //    return BaseDatos.ObtenerBaseDatos().Obtener<HTLReservasDetalles>("HTLHabitacionesDetalleSeleccionar", pParamtro);
        //}

        /*HTLReservas - Dashboard*/
        public HTLReservasDetalles SeleccionarHabitacionesDetallesAgenda(HTLReservasDetalles pParamtro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<HTLReservasDetalles>("HTLReservasSeleccionarHabitacionesAgenda", pParamtro);
        }

        public override bool Agregar(HTLHabitaciones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.Estado.IdEstado = (int)Estados.Activo;
            pParametro.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuarioEvento;
            pParametro.FechaAlta = DateTime.Now;

            if (!this.Validar(pParametro, new HTLHabitaciones()))
                return false;

            //pParametro.ReservasXML.LoadXml(pParametro.Serialize<HTLHabitaciones>());

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

                    if (resultado && !this.ItemsActualizar(pParametro, new HTLHabitaciones(), bd, tran))
                        resultado = false;

                    //if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    //if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
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
     
        internal bool Agregar(HTLHabitaciones pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdHabitacion = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "HTLHabitacionesInsertar");
            if (pParametro.IdHabitacion == 0)
                return false;

            return true;
        }

        private bool Validar(HTLHabitaciones pParametro, HTLHabitaciones pValorViejo)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "HTLHabitacionesValidaciones");
        }
       
        public override bool Modificar(HTLHabitaciones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            if (!this.Validar(pParametro, new HTLHabitaciones()))
                return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            HTLHabitaciones valorViejo = new HTLHabitaciones();
            valorViejo.IdHabitacion = pParametro.IdHabitacion;
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
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "HTLHabitacionesActualizar");

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.ItemsActualizar(pParametro, valorViejo, bd, tran))
                        resultado = false;

                    //if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    //if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
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

        public bool ModificarEstado(HTLHabitaciones pParametro, Database db, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, db, tran, "HTLHabitacionesActualizarEstado"))
                return false;

            return true;
        }

        private bool ItemsActualizar(HTLHabitaciones pParametro, HTLHabitaciones pValorViejo, Database bd, DbTransaction tran)
        {

            foreach (HTLHabitacionesDetalles item in pParametro.HabitacionesDetalles.Where(x=>x.Moviliario.IdMoviliario > 0))
            {
                switch (item.EstadoColeccion)
                {
                    #region Agregado
                    case EstadoColecciones.Agregado:
                        item.UsuarioLogueado = pParametro.UsuarioLogueado;
                        item.IdHabitacion = pParametro.IdHabitacion;
                        item.IdHabitacionDetalle = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "HTLHabitacionesDetallesInsertar");
                        if (item.IdHabitacionDetalle == 0)
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        break;
                    #endregion
                    
                    #region Modificado
                    case EstadoColecciones.Modificado:
                    case EstadoColecciones.Borrado:
                        item.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "HTLHabitacionesDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.HabitacionesDetalles.Find(x => x.IdHabitacionDetalle == item.IdHabitacionDetalle), Acciones.Update, item, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            return true;
        }

        public DataTable ObtenerTiposProductosHoteles(Objeto pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CmpProductosSeleccionarTiposProductosHoteles", pParametro);
        }
    }
}

