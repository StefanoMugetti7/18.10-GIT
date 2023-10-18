using Auditoria;
using Auditoria.Entidades;
using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using LavaYa.Entidades;
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

namespace LavaYa.LogicaNegocio
{
    public class PuntosVentasLN : BaseLN<LavPuntosVentas>
    {
  
        public override bool Agregar(LavPuntosVentas pParametro)
        {
            
            if (pParametro.IdPuntoVenta > 0)
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

                    if (resultado && !this.ActualizarPuntosVentasDetalle(pParametro, new LavPuntosVentas(), bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdPuntoVenta.ToString());
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

        public override bool Modificar(LavPuntosVentas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;

            if (!this.Validaciones(pParametro))
                return false;

            LavPuntosVentas valorViejo = new LavPuntosVentas();
            valorViejo.IdPuntoVenta = pParametro.IdPuntoVenta;
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

                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "LavPuntosVentasActualizar"))
                        resultado = false;

                    if (resultado && !this.ActualizarPuntosVentasDetalle(pParametro, new LavPuntosVentas(), bd, tran))
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
        private bool ActualizarPuntosVentasDetalle(LavPuntosVentas pParametro, LavPuntosVentas pValorViejo, Database db, DbTransaction tran)
        {
            foreach (LavPuntosVentasDetalle item in pParametro.PuntosVentasDetalles)
            {
                item.UsuarioLogueado = pParametro.UsuarioLogueado;
                switch (item.EstadoColeccion)
                {
                    #region "Agregado"
                    case EstadoColecciones.Agregado:
                        item.Estado.IdEstado = (int)Estados.Activo;
                        item.IdPuntoVenta = pParametro.IdPuntoVenta;
                        item.IdPuntoVentaDetalle = BaseDatos.ObtenerBaseDatos().Agregar(item, db, tran, "LavPuntosVentasDetalleInsertar");
                        if (item.IdPuntoVentaDetalle == 0)
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        break;
                    #endregion

                    #region "Modificado"
                    case EstadoColecciones.Modificado:
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, db, tran, "LavPuntosVentasDetalleActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.PuntosVentasDetalles.Find(x => x.IdPuntoVentaDetalle == item.IdPuntoVentaDetalle), Acciones.Update, item, db, tran))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }

                        break;
                        #endregion
                }
            }
            return true;
        }

        public override LavPuntosVentas ObtenerDatosCompletos(LavPuntosVentas pParametro)
        {
            var puntoVenta = BaseDatos.ObtenerBaseDatos().Obtener<LavPuntosVentas>("LavPuntosVentasSeleccionar", pParametro);
            puntoVenta.PuntosVentasDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<LavPuntosVentasDetalle>("LavPuntosVentasDetalleSeleccionar", pParametro);
            return puntoVenta;
        }

        public override List<LavPuntosVentas> ObtenerListaFiltro(LavPuntosVentas pParametro)
        {
            throw new NotImplementedException();
        }
        internal bool Agregar(LavPuntosVentas pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdPuntoVenta = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "LavPuntosVentasInsertar");
            if (pParametro.IdPuntoVenta == 0)
                return false;

            return true;
        }
        private bool Validaciones(LavPuntosVentas pParametro)
        {
            return true;
        }
        public DataTable ObtenerListaGrilla(LavPuntosVentas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("[LavPuntosVentasSeleccionarDescripcionPorFiltro]", pParametro);
        }
    }
}
