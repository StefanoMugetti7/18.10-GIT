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
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hoteles.LogicaNegocio
{
    class DescuentosLN : BaseLN<HTLDescuentos>
    {
        public override bool Agregar(HTLDescuentos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.Estado.IdEstado = (int)Estados.Activo;
            //pParametro.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuarioEvento;
            //pParametro.FechaAlta = DateTime.Now;

            if (!this.Validar(pParametro, new HTLDescuentos()))
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

        internal bool Agregar(HTLDescuentos pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdDescuento = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "HTLDescuentosInsertar");
            if (pParametro.IdDescuento == 0)
                return false;

            return true;
        }
        private bool Validar(HTLDescuentos pParametro, HTLDescuentos pValorViejo)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "HTLDescuentosValidaciones");
        }
        
        public override bool Modificar(HTLDescuentos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            if (!this.Validar(pParametro, new HTLDescuentos()))
                return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            HTLDescuentos valorViejo = new HTLDescuentos();
            valorViejo.IdDescuento = pParametro.IdDescuento;
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
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "HTLDescuentosActualizar");

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
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

        public override HTLDescuentos ObtenerDatosCompletos(HTLDescuentos pParametro)
        {
            HTLDescuentos descuento = BaseDatos.ObtenerBaseDatos().Obtener<HTLDescuentos>("HTLDescuentosSeleccionarABM", pParametro);
            
            //descuento.DescuentosDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<HTLDescuentosDetalles>("HTLDescuentosSeleccionar", pParametro);
            return descuento;
        }

        public override List<HTLDescuentos> ObtenerListaFiltro(HTLDescuentos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<HTLDescuentos>("HTLDescuentosSeleccionarDescripcionPorFiltroABM", pParametro);
        }

        public List<HTLDescuentos> ObtenerPorReserva(HTLDescuentosFiltros pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<HTLDescuentos>("HTLDescuentosSeleccionar", pParametro); 
        }


    }
}
