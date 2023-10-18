using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Compras.Entidades;
using Servicio.AccesoDatos;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Auditoria;
using Auditoria.Entidades;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
//using Proveedores.Entidades;

namespace Compras.LogicaNegocio
{
    class CmpSolicitudesComprasLN : BaseLN<CmpSolicitudesCompras>
    {
        public override CmpSolicitudesCompras ObtenerDatosCompletos(CmpSolicitudesCompras pParametro)
        {
            CmpSolicitudesCompras solCompra = BaseDatos.ObtenerBaseDatos().Obtener<CmpSolicitudesCompras>("CmpSolicitudesComprasSeleccionar",pParametro);
            //solCompra.Proveedor = BaseDatos.ObtenerBaseDatos().Obtener<CapProveedores>("CapProveedoresSeleccionarDescripcion",solCompra);
            solCompra.SolicitudCompraDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<CmpSolicitudesComprasDetalles>("CmpSolicitudesComprasDetallesSeleccionarPorSolicitudCompra", pParametro);
            return solCompra;
        }

        public override List<CmpSolicitudesCompras> ObtenerListaFiltro(CmpSolicitudesCompras pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CmpSolicitudesCompras>("CmpSolicitudesComprasSeleccionarDescripcionPorFiltro",pParametro);
        }

        public override bool Agregar(CmpSolicitudesCompras pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

           // if (pParametro.TipoSolicitudPago.IdTipoSolicitudPago == (int)EnumTiposSolicitudPago.Compras)
            if (!this.Validar(pParametro, new CmpSolicitudesCompras()))
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
                    pParametro.IdSolicitudCompra = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CmpSolicitudesComprasInsertar");
                    if (pParametro.IdSolicitudCompra == 0)
                        resultado = false;

                    if (resultado && !this.ActualizarDetalles(pParametro, new CmpSolicitudesCompras(), bd, tran))
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

        public override bool Modificar(CmpSolicitudesCompras pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            pParametro.FechaAutorizacion = DateTime.Now;
            pParametro.FechaEvento = DateTime.Now;
            pParametro.IdUsuarioAutorizacion = pParametro.UsuarioLogueado.IdUsuarioEvento;
            pParametro.Estado.IdEstado = (int)Estados.Activo; // VER ESTADOS DE SOLIC DE COMPRAS


            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CmpSolicitudesCompras valorViejo = new CmpSolicitudesCompras();
            valorViejo.IdSolicitudCompra = pParametro.IdSolicitudCompra;
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

                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CmpSolicitudesComprasActualizar"))
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

        public bool Cotizar(CmpSolicitudesCompras pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            
            pParametro.FechaEvento = DateTime.Now;

            pParametro.Estado.IdEstado = (int)EstadosSolicitudesCompras.Cotizado;

            if (!this.Validar(pParametro, new CmpSolicitudesCompras()))
                return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CmpSolicitudesCompras valorViejo = new CmpSolicitudesCompras();
            valorViejo.IdSolicitudCompra = pParametro.IdSolicitudCompra;
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

                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CmpSolicitudesComprasActualizar"))
                        resultado = false;

                    if (resultado && !this.ActualizarDetalles(pParametro, new CmpSolicitudesCompras(), bd, tran))
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

        public bool Anular(CmpSolicitudesCompras pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            pParametro.FechaBaja = DateTime.Now;

            // VER en CapSolicitudPago.cs
            pParametro.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;
            pParametro.IdUsuarioBaja = pParametro.UsuarioLogueado.IdUsuarioEvento;
            pParametro.Estado.IdEstado = (int)EstadosSolicitudesCompras.Baja;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CmpSolicitudesCompras valorViejo = new CmpSolicitudesCompras();
            valorViejo.IdSolicitudCompra = pParametro.IdSolicitudCompra;
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

                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CmpSolicitudesComprasActualizar"))
                        resultado = false;

                    if (resultado && !this.ActualizarDetalles(pParametro, new CmpSolicitudesCompras(), bd, tran))
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

        public bool Autorizar(CmpSolicitudesCompras pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            pParametro.FechaAutorizacion = DateTime.Now;

            // VER en CapSolicitudPago.cs
            pParametro.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;
            pParametro.IdUsuarioAutorizacion = pParametro.UsuarioLogueado.IdUsuarioEvento;
            pParametro.Estado.IdEstado = (int)EstadosSolicitudesCompras.Autorizado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CmpSolicitudesCompras valorViejo = new CmpSolicitudesCompras();
            valorViejo.IdSolicitudCompra = pParametro.IdSolicitudCompra;
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

                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CmpSolicitudesComprasActualizar"))
                        resultado = false;

                    if (resultado && !this.ActualizarDetalles(pParametro, new CmpSolicitudesCompras(), bd, tran))
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

        public bool ModificarEstado(CmpSolicitudesCompras pParametro, Database db, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, db, tran, "CmpSolicitudesComprasActualizarEstado"))
                return false;

            return true;
        }

        #region "Validaciones"

        private bool Validar(CmpSolicitudesCompras pParametro, CmpSolicitudesCompras pValorViejo)
        {
            CmpSolicitudesCompras SolicitudCompra = new CmpSolicitudesCompras();

            switch (pParametro.EstadoColeccion)
            {
                case EstadoColecciones.Agregado:
                    //if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CapSolicitudPagoValidarNumeroFactura"))
                    //{
                    //    pParametro.CodigoMensaje = "ValidarNumeroFactura";
                    //    return false;
                    //}

                    if (pParametro.SolicitudCompraDetalles.Count(x => x.EstadoColeccion == EstadoColecciones.Agregado) == 0)
                    {
                        pParametro.CodigoMensaje = "ValidarSolicitudDetalles";
                        return false;
                    }

                    if (pParametro.SolicitudCompraDetalles.Exists(x => x.EstadoColeccion == EstadoColecciones.Agregado && x.Cantidad <= 0))
                    {
                        pParametro.CodigoMensaje = "ValidarSolicitudDetallesCantidad";
                        return false;
                    }

                    if (pParametro.SolicitudCompraDetalles.Exists(x => x.EstadoColeccion == EstadoColecciones.Agregado && x.PrecioUnitario <= 0))
                    {
                        pParametro.CodigoMensaje = "ValidarSolicitudDetallesPrecio";
                        return false;
                    }
                    if (pParametro.SolicitudCompraDetalles.Exists(x => x.EstadoColeccion == EstadoColecciones.Agregado && x.AlicuotaIVA <= 0))
                    {
                        pParametro.CodigoMensaje = "ValidarSolicitudDetallesIVA";
                        return false;
                    }
                    
                    
                    break;
                case EstadoColecciones.AgregadoBorradoMemoria:
                    break;
                case EstadoColecciones.Borrado:
                    break;
                case EstadoColecciones.Modificado:
                   foreach(CmpSolicitudesComprasDetalles Detalle in pParametro.SolicitudCompraDetalles)
                    
                    if ( Detalle.IdCotizacionDetalle == null)
                    {
                        pParametro.CodigoMensaje = "ValidarCotizacionDetalle";
                        return false;
                    }
                        break;
                default:
                    break;
            }

            return true;
        }

        #endregion

        #region "Solicitud Detalles"

        private bool ActualizarDetalles(CmpSolicitudesCompras pParametro, CmpSolicitudesCompras pValorViejo, Database db, DbTransaction tran)
        {
            foreach (CmpSolicitudesComprasDetalles Detalle in pParametro.SolicitudCompraDetalles)
            {
                

                switch (Detalle.EstadoColeccion)
                {
                    #region "Agregado"
                    case EstadoColecciones.Agregado:
                        Detalle.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;
                        Detalle.Estado.IdEstado = (int)EstadosSolicitudesCompras.Activo;
                        Detalle.IdSolicitudCompra = pParametro.IdSolicitudCompra;
                        Detalle.IdSolicitudCompraDetalle = BaseDatos.ObtenerBaseDatos().Agregar(Detalle, db, tran, "CmpSolicitudesComprasDetallesInsertar");
                        if (Detalle.IdSolicitudCompraDetalle == 0)
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }
                        break;
                    #endregion

                    #region "Cotizado"
                    case EstadoColecciones.Modificado:
                        Detalle.UsuarioLogueado = pParametro.UsuarioLogueado;           
                        Detalle.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;
                        Detalle.Estado.IdEstado = (int)EstadosSolicitudesCompras.Cotizado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(Detalle, db, tran, "CmpSolicitudesComprasDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.SolicitudCompraDetalles.Find(x => x.IdSolicitudCompraDetalle == Detalle.IdSolicitudCompraDetalle), Acciones.Update, Detalle, db, tran))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }

                        break;
                    #endregion

                    #region "Anulado"
                    case EstadoColecciones.Borrado:
                        Detalle.UsuarioLogueado = pParametro.UsuarioLogueado;
                        Detalle.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;
                        Detalle.Estado.IdEstado = (int)EstadosSolicitudesCompras.Baja;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(Detalle, db, tran, "CmpSolicitudesComprasDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.SolicitudCompraDetalles.Find(x => x.IdSolicitudCompraDetalle == Detalle.IdSolicitudCompraDetalle), Acciones.Update, Detalle, db, tran))
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
    
    
    
    }
}
