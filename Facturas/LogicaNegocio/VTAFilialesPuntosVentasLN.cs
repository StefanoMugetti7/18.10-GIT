using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facturas.Entidades;
using Comunes;
using Servicio.AccesoDatos;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Comunes.Entidades;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Comunes.LogicaNegocio;
using System.Data;
using Auditoria;
using Auditoria.Entidades;

namespace Facturas.LogicaNegocio
{
    public class VTAFilialesPuntosVentasLN : BaseLN<VTAFilialesPuntosVentas>
    {
        public override bool Agregar(VTAFilialesPuntosVentas pParametro)
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
                    if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "VTAFilialesPuntosVentasValidaciones"))
                        resultado = false;

                    if (resultado)
                    {
                        pParametro.IdFilialPuntoVenta = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "VTAFilialesPuntosVentasInsertar");
                        if (pParametro.IdFilialPuntoVenta == 0)
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

        public bool Borrar(VTAFilialesPuntosVentas pParametro)
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
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "VTAFilialesPuntosVentasBorrar");

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

        public override VTAFilialesPuntosVentas ObtenerDatosCompletos(VTAFilialesPuntosVentas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<VTAFilialesPuntosVentas>("VTAFilialesPuntosVentasSeleccionar", pParametro);
        }

        public override List<VTAFilialesPuntosVentas> ObtenerListaFiltro(VTAFilialesPuntosVentas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<VTAFilialesPuntosVentas>("VTAFilialesPuntosVentasSeleccionarFiltro", pParametro);
        }

        /// <summary>
        /// Devuelve los distintos puntos de ventas (tipos de emision) por filial
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public List<VTATiposPuntosVentas> ObtenerPorFilial(VTAFilialesPuntosVentas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<VTATiposPuntosVentas>("VTAFilialesPuntosVentasSeleccionarPorFilial", pParametro);
        }

        public DataTable ObtenerListaGrilla(VTAFilialesPuntosVentas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("[VTAFilialesPuntosVentasSeleccionarGrilla]", pParametro);
        }

        public override bool Modificar(VTAFilialesPuntosVentas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;

            VTAFilialesPuntosVentas valorViejo = new VTAFilialesPuntosVentas();
            valorViejo.IdFilialPuntoVenta = pParametro.IdFilialPuntoVenta;
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
                    if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "VTAFilialesPuntosVentasValidaciones"))
                        resultado = false;

                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "VTAFilialesPuntosVentasActualizar"))
                        resultado = false;

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
    }
}
