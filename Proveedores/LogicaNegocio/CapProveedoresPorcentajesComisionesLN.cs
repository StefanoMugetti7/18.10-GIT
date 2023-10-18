using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proveedores.Entidades;
using Comunes;
using Servicio.AccesoDatos;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Auditoria;
using Auditoria.Entidades;

namespace Proveedores.LogicaNegocio
{
    public class CapProveedoresPorcentajesComisionesLN : BaseLN<CapProveedoresPorcentajesComisiones>
    {
        public override bool Agregar(CapProveedoresPorcentajesComisiones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdProveedorPorcentajeComision = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CapProveedoresPorcentajesComisionesInsertar");
                    if (pParametro.IdProveedorPorcentajeComision == 0)
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

        public override bool Modificar(CapProveedoresPorcentajesComisiones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CapProveedoresPorcentajesComisiones valorViejo = new CapProveedoresPorcentajesComisiones();
            valorViejo.IdProveedorPorcentajeComision = pParametro.IdProveedorPorcentajeComision;
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
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CapProveedoresPorcentajesComisionesActualizar");

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
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

        public override CapProveedoresPorcentajesComisiones ObtenerDatosCompletos(CapProveedoresPorcentajesComisiones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CapProveedoresPorcentajesComisiones>("CapProveedoresPorcentajesComisionesSeleccionar", pParametro);
        }

        public override List<CapProveedoresPorcentajesComisiones> ObtenerListaFiltro(CapProveedoresPorcentajesComisiones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CapProveedoresPorcentajesComisiones>("CapProveedoresPorcentajesComisionesSeleccionarFiltros", pParametro);
        }

        public List<CapProveedores> ObtenerProveedores(CapProveedoresPorcentajesComisiones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CapProveedores>("CapProveedoresPorcentajesComisionesSeleccionarPorTipoOperacion", pParametro);
        }
    }
}
