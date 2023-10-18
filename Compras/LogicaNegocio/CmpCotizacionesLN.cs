using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using Compras.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Servicio.AccesoDatos;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Auditoria;
using Auditoria.Entidades;
using Proveedores.Entidades;

namespace Compras.LogicaNegocio
{
    class CmpCotizacionesLN : BaseLN<CmpCotizaciones>
    {
        public override bool Agregar(CmpCotizaciones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            // if (pParametro.TipoSolicitudPago.IdTipoSolicitudPago == (int)EnumTiposSolicitudPago.Compras)
            if (!this.Validar(pParametro, new CmpCotizaciones()))
                return false;

            pParametro.FechaEvento = DateTime.Now;
            pParametro.Estado.IdEstado = (int)EstadosSolicitudesCompras.Activo;
            pParametro.FechaAlta = DateTime.Now;
            //PREGUNTAR¬
            pParametro.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuarioEvento;
            //pParametro.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdCotizacion = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CmpCotizacionesInsertar");
                    if (pParametro.IdCotizacion == 0)
                        resultado = false;

                    if (resultado && !this.ActualizarDetalles(pParametro, new CmpCotizaciones(), bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    //if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                    //    resultado = false;



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

        public override bool Modificar(CmpCotizaciones pParametro)
        {

            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            pParametro.FechaEvento = DateTime.Now;
                //AGREGAR EN ENTIDAD & DESCOMENTAR
            pParametro.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;  
            pParametro.Estado.IdEstado = (int)Estados.Activo; 


            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CmpCotizaciones valorViejo = new CmpCotizaciones();
            valorViejo.IdCotizacion = pParametro.IdCotizacion;
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

                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CmpCotizacionesActualizar"))
                        resultado = false;

                    if (resultado && !this.ActualizarDetalles(pParametro, new CmpCotizaciones(), bd, tran))
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
                        if (pParametro.Concurrencia)
                            pParametro.CodigoMensaje = "Concurrencia";
                        else
                            pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";

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

        public bool Anular(CmpCotizaciones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            pParametro.FechaEvento = DateTime.Now;

            // VER en CapSolicitudPago.cs
            pParametro.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;
            pParametro.Estado.IdEstado = (int)EstadosCotizaciones.Baja;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CmpCotizaciones valorViejo = new CmpCotizaciones();
            valorViejo.IdCotizacion = pParametro.IdCotizacion;
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

                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CmpCotizacionesActualizar"))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
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
                        if (pParametro.Concurrencia)
                            pParametro.CodigoMensaje = "Concurrencia";
                        else
                            pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";

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

        public bool Autorizar(CmpCotizaciones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            pParametro.FechaEvento = DateTime.Now;

            // AGREGAR EN BASE Y ENTIDAD 
            pParametro.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;
            pParametro.Estado.IdEstado = (int)EstadosCotizaciones.Autorizado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CmpCotizaciones valorViejo = new CmpCotizaciones();
            valorViejo.IdCotizacion = pParametro.IdCotizacion;
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

                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CmpCotizacionesActualizar"))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
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
                        if (pParametro.Concurrencia)
                            pParametro.CodigoMensaje = "Concurrencia";
                        else
                            pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";

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

        public override CmpCotizaciones ObtenerDatosCompletos(CmpCotizaciones pParametro)
        {
            CmpCotizaciones cotiz = BaseDatos.ObtenerBaseDatos().Obtener<CmpCotizaciones>("CmpCotizacionesSeleccionar", pParametro);
            cotiz.Proveedor = BaseDatos.ObtenerBaseDatos().Obtener<CapProveedores>("CapProveedoresSeleccionarDescripcion",cotiz);
            cotiz.CotizacionesDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<CmpCotizacionesDetalles>("CmpCotizacionesDetallesSeleccionarPorCotizacion", pParametro);
            return cotiz;
        }

        public override List<CmpCotizaciones> ObtenerListaFiltro(CmpCotizaciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CmpCotizaciones>("CmpCotizacionesSeleccionarDescripcionPorFiltro", pParametro);
        
        }

        public List<CmpCotizacionesDetalles> ObtenerListaFiltroPorProducto(CMPProductos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CmpCotizacionesDetalles>("CmpCotiacionesSeleccionarPorIdProducto", pParametro);
        }

        #region "Cotizacion Detalles"

        private bool ActualizarDetalles(CmpCotizaciones pParametro, CmpCotizaciones pValorViejo, Database db, DbTransaction tran)
        {
            foreach (CmpCotizacionesDetalles Detalle in pParametro.CotizacionesDetalles)
            {


                switch (Detalle.EstadoColeccion)
                {
                    #region "Agregado"
                    case EstadoColecciones.Agregado:
                        
                        Detalle.Estado.IdEstado = (int)Estados.Activo;
                        Detalle.IdCotizacion = pParametro.IdCotizacion;
                        Detalle.IdCotizacionDetalle = BaseDatos.ObtenerBaseDatos().Agregar(Detalle, db, tran, "CmpCotizacionesDetallesInsertar");
                        if (Detalle.IdCotizacionDetalle == 0)
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }
                        break;
                    #endregion

                    #region "Modificado"
                    case EstadoColecciones.Modificado:
                        Detalle.UsuarioLogueado = pParametro.UsuarioLogueado;           //CREAR STORE  ¬
                        //Detalle.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(Detalle, db, tran, "CmpCotizacionesDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.CotizacionesDetalles.Find(x => x.IdCotizacionDetalle == Detalle.IdCotizacionDetalle), Acciones.Update, Detalle, db, tran))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }

                        break;
                    #endregion
                }
            }

            return true;
        }

        #endregion

        #region "Validaciones"

        private bool Validar(CmpCotizaciones pParametro, CmpCotizaciones pValorViejo)
        {
            CmpCotizaciones Cotiz = new CmpCotizaciones();

            switch (pParametro.EstadoColeccion)
            {
                case EstadoColecciones.Agregado:
                    if (pParametro.Proveedor.IdProveedor <= 0)
                    {
                        pParametro.CodigoMensaje = "ValidarProveedorSeleccionado";
                        return false;
                    }

                    if (pParametro.CotizacionesDetalles.Count(x => x.EstadoColeccion == EstadoColecciones.Agregado) == 0)
                    {
                        pParametro.CodigoMensaje = "ValidarCotizacionesDetalles";
                        return false;
                    }

                    //if (pParametro.SolicitudCompraDetalles.Exists(x => x.EstadoColeccion == EstadoColecciones.Agregado && x.Cantidad <= 0))
                    //{
                    //    pParametro.CodigoMensaje = "ValidarSolicitudDetallesCantidad";
                    //    return false;
                    //}

                    //if (pParametro.SolicitudCompraDetalles.Exists(x => x.EstadoColeccion == EstadoColecciones.Agregado && x.PrecioUnitario <= 0))
                    //{
                    //    pParametro.CodigoMensaje = "ValidarSolicitudDetallesPrecio";
                    //    return false;
                    //}

                    //if (pParametro.TiposFacturas.IdTipoFactura == (int)EnumTiposFacturas.FacturaB
                    //    || pParametro.TiposFacturas.IdTipoFactura == (int)EnumTiposFacturas.FacturaC)
                    //{
                    //if (pParametro.SolicitudCompraDetalles.Exists(x => x.EstadoColeccion == EstadoColecciones.Agregado && x.AlicuotaIVA == 0))
                    //{
                    //    pParametro.CodigoMensaje = "ValidarSolicitudDetallesIVA";
                    //    return false;
                    //}
                    //}

                    break;
                case EstadoColecciones.AgregadoBorradoMemoria:
                    break;
                case EstadoColecciones.Borrado:
                    break;
                case EstadoColecciones.Modificado:
                    if (pParametro.CotizacionesDetalles.Count(x => x.EstadoColeccion == EstadoColecciones.Modificado) == 0)
                    {
                        pParametro.CodigoMensaje = "ValidarCotizacionesDetalles";
                        return false;
                    }
                    break;
                default:
                    break;
            }

            return true;
        }

        #endregion


    }
}
