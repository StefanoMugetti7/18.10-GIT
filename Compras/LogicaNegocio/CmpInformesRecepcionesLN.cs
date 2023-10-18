using Auditoria;
using Auditoria.Entidades;
using Compras.Entidades;
using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Compras.LogicaNegocio
{
    class CmpInformesRecepcionesLN : BaseLN<CmpInformesRecepciones>
    {

        public override bool Agregar(CmpInformesRecepciones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;


            if (!this.Validar(pParametro, new CmpInformesRecepciones()))
                return false;
            if (pParametro.IdSolicitudPago > 0)
            {
                decimal? total = 0;
                total = pParametro.ImporteSolicitudPago - pParametro.ImportePrevioRecibido;
                if (pParametro.PrecioTotal > total)
                {
                    pParametro.CodigoMensaje = "RemitoValidarTotal";
                    return false;
                }
            }
            pParametro.FechaEvento = DateTime.Now;
            pParametro.Estado.IdEstado = (int)EstadosOrdenesCompras.Activo;
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
                    if (!this.ValidarNumeroRemito(pParametro, new CmpInformesRecepciones(), bd, tran))
                        return false;

                    pParametro.IdInformeRecepcion = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CmpInformesRecepcionesInsertar");
                    if (pParametro.IdInformeRecepcion == 0)
                        resultado = false;

                    if (resultado && !this.ActualizarDetalles(pParametro, new CmpInformesRecepciones(), bd, tran))
                        resultado = false;

                    if (pParametro.OrdenCompra.IdOrdenCompra > 0)
                        if (resultado && !this.ActualizarOCDetalles(pParametro, new CmpInformesRecepciones(), bd, tran))
                            resultado = false;

                    //if (resultado && !this.ModificarDetallesSolicitudesCompras(pParametro, new CmpOrdenesCompras(), bd, tran))
                    //    resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
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

        public override bool Modificar(CmpInformesRecepciones pParametro)
        {
            throw new NotImplementedException();
        }

        public override CmpInformesRecepciones ObtenerDatosCompletos(CmpInformesRecepciones pParametro)
        {
            CmpInformesRecepciones informe = BaseDatos.ObtenerBaseDatos().Obtener<CmpInformesRecepciones>("CmpInformesRecepcionesSeleccionar", pParametro);
            informe.InformesRecepcionesDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<CmpInformesRecepcionesDetalles>("CmpInformesRecepcionesDetallesSeleccionarPorInformeRecepcion", pParametro);
            if (informe.OrdenCompra.IdOrdenCompra > 0)
                informe.OrdenCompra = new CmpOrdenesComprasLN().ObtenerDatosCompletos(informe.OrdenCompra);
            return informe;
        }

        public override List<CmpInformesRecepciones> ObtenerListaFiltro(CmpInformesRecepciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CmpInformesRecepciones>("CmpInformesRecepcionesSeleccionarDescripcionPorFiltro", pParametro);
        }

        public DataTable ObtenerListaGrilla(CmpInformesRecepciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CmpInformesRecepcionesSeleccionarDescripcionPorFiltroDataTable", pParametro);
        }

        public bool Anular(CmpInformesRecepciones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Borrado;


            if (!this.Validar(pParametro, new CmpInformesRecepciones()))
                return false;

            pParametro.FechaEvento = DateTime.Now;
            pParametro.Estado.IdEstado = (int)EstadosOrdenesCompras.Baja;
            pParametro.FechaBaja = DateTime.Now;
            //PREGUNTAR¬
            pParametro.IdUsuarioBaja = pParametro.UsuarioLogueado.IdUsuarioEvento;
            //pParametro.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;
            pParametro.InformesRecepcionesDetalles.ForEach(x => x.EstadoColeccion = EstadoColecciones.Borrado);
            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();
                try
                {
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CmpInformesRecepcionesActualizar"))
                        resultado = false;

                    if (pParametro.OrdenCompra.IdOrdenCompra > 0)
                        if (resultado && !this.ActualizarOCDetalles(pParametro, new CmpInformesRecepciones(), bd, tran))
                            resultado = false;

                    if (resultado && !this.ActualizarDetalles(pParametro, new CmpInformesRecepciones(), bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
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

        #region "Validaciones"

        private bool Validar(CmpInformesRecepciones pParametro, CmpInformesRecepciones pValorViejo)
        {
            switch (pParametro.EstadoColeccion)
            {
                case EstadoColecciones.Agregado:
                    //if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CapSolicitudPagoValidarNumeroFactura"))
                    //{
                    //    pParametro.CodigoMensaje = "ValidarNumeroFactura";
                    //    return false;
                    //}
                    if (pParametro.InformesRecepcionesDetalles.Count() == 0)
                    {
                        pParametro.CodigoMensaje = "ValidarCantidadItems";
                        return false;
                    }

                    foreach (CmpInformesRecepcionesDetalles detalle in pParametro.InformesRecepcionesDetalles)
                    {
                        if (detalle.IdOrdenCompraDetalle > 0 && detalle.CantidadRecibida > detalle.CantidadPedida)
                        {
                            pParametro.CodigoMensaje = "ValidarInformeDetalleCantidadRecibida";
                            return false;
                        }
                    }
                    if (pParametro.InformesRecepcionesDetalles.Exists(x => x.CantidadRecibida == 0))
                    {
                        pParametro.CodigoMensaje = "ValidarInformeDetalleCantidad";
                        return false;
                    }
                    if (pParametro.IdSolicitudPago > 0
                        && pParametro.InformesRecepcionesDetalles.Exists(x => x.PrecioUnitario == 0))
                    {
                        pParametro.CodigoMensaje = "ValidarInformeDetallePrecioUnitario";
                        return false;
                    }

                    //if (pParametro.SolicitudCompraDetalles.Exists(x => x.EstadoColeccion == EstadoColecciones.Agregado && x.Cantidad <= 0))
                    //{
                    //    pParametro.CodigoMensaje = "ValidarSolicitudDetallesCantidad";
                    //    return false;
                    //}
                    break;
                case EstadoColecciones.AgregadoBorradoMemoria:
                    break;
                case EstadoColecciones.Borrado:

                    //if (pParametro.MotivoCancelado == string.Empty)
                    //{
                    //    pParametro.CodigoMensaje = "ValidarMotivoCancelacion";
                    //    return false;
                    //}
                    break;
                case EstadoColecciones.Modificado:
                    break;
                default:
                    break;
            }

            return true;
        }

        private bool ValidarNumeroRemito(CmpInformesRecepciones pParametro, CmpInformesRecepciones pValorViejo, Database db, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, db, tran, "CmpInformesRecepcionesValidarNumeroRemito"))
            {
                pParametro.CodigoMensaje = "ValidarNumeroRemito";
                return false;
            }
            return true;
        }
        #endregion

        #region Informes Recepciones Detalles
        private bool ActualizarDetalles(CmpInformesRecepciones pParametro, CmpInformesRecepciones pValorViejo, Database db, DbTransaction tran)
        {
            CMPStock stock;
            CMPStockLN stockLN = new CMPStockLN();
            foreach (CmpInformesRecepcionesDetalles detalle in pParametro.InformesRecepcionesDetalles)
            {
                switch (detalle.EstadoColeccion)
                {
                    #region "Agregado"
                    case EstadoColecciones.Agregado:
                        detalle.IdInformeRecepcion = pParametro.IdInformeRecepcion;
                        detalle.UsuarioLogueado = pParametro.UsuarioLogueado;
                        detalle.Estado.IdEstado = (int)Estados.Activo;
                        detalle.IdInformeRecepcionDetalle = BaseDatos.ObtenerBaseDatos().Agregar(detalle, db, tran, "CmpInformesRecepcionesDetallesInsertar");
                        if (detalle.IdInformeRecepcionDetalle == 0)
                        {
                            AyudaProgramacionLN.MapearError(detalle, pParametro);
                            return false;
                        }

                        #region Stock
                        //Actualizo tambien el Stock de productos
                        //if (detalle.Producto.Familia.Stockeable)
                        //{
                        stock = new CMPStock
                        {
                            IdFilial = pParametro.Filial.IdFilial,
                            Producto = detalle.Producto,
                            StockActual = detalle.CantidadRecibida
                        };
                        if (!stockLN.AgregarModificar(stock, db, tran))
                        {
                            AyudaProgramacionLN.MapearError(stock, pParametro);
                            return false;
                        }
                        //}
                        #endregion

                        break;
                    #endregion

                    #region "Borrado"
                    case EstadoColecciones.Borrado:
                        detalle.Estado.IdEstado = (int)Estados.Baja;
                        detalle.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(detalle, db, tran, "CmpInformesRecepcionesDetallesActualizarEstado"))
                        {
                            AyudaProgramacionLN.MapearError(detalle, pParametro);
                            return false;
                        }

                        #region Stock
                        //Actualizo tambien el Stock de productos
                        //if (detalle.Producto.Familia.Stockeable)
                        //{
                        stock = new CMPStock();
                        stock.IdFilial = pParametro.Filial.IdFilial;
                        stock.Producto = detalle.Producto;
                        stock.StockActual = -detalle.CantidadRecibida;
                        if (!stockLN.AgregarModificar(stock, db, tran))
                        {
                            AyudaProgramacionLN.MapearError(stock, pParametro);
                            return false;
                        }
                        //}
                        #endregion
                        break;
                        #endregion
                }

            }
            return true;
        }
        #endregion

        #region Ordenes Compras Detalles

        private bool ActualizarOCDetalles(CmpInformesRecepciones pParametro, CmpInformesRecepciones pValorViejo, Database db, DbTransaction tran)
        {
            bool actualizarEstadoOrden = true;

            foreach (CmpOrdenesComprasDetalles Detalle in pParametro.OrdenCompra.OrdenesComprasDetalles)
            {
                switch (Detalle.EstadoColeccion)
                {
                    //al agregar un Informe se debe actualizar la orden de compra
                    #region "Modificado"
                    case EstadoColecciones.Modificado:
                        Detalle.UsuarioLogueado = pParametro.UsuarioLogueado;           //CREAR STORE  ¬
                        //Detalle.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;
                        if (Detalle.CantidadRecibida >= Detalle.Cantidad)
                        {
                            //si se recibe todo lo pedido modifico el estado del item
                            Detalle.Estado.IdEstado = (int)EstadosInformesRecepciones.Recibido;
                        }
                        if (Detalle.Cantidad > Detalle.CantidadRecibida)
                        {
                            Detalle.Estado.IdEstado = (int)EstadosInformesRecepciones.PatcialmenteRecibido;
                            //si no se recibe todo no cambio el estado de la orden de compra
                            actualizarEstadoOrden = false;
                        }
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(Detalle, db, tran, "CmpOrdenesComprasDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }

                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.OrdenCompra.OrdenesComprasDetalles.Find(x => x.IdOrdenCompraDetalle == Detalle.IdOrdenCompraDetalle), Acciones.Update, Detalle, db, tran))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }

                        //si una orden de compra esta completamente recibida modifico el estado

                        break;
                    #endregion

                    #region "Anulado"
                    case EstadoColecciones.Borrado:
                        Detalle.UsuarioLogueado = pParametro.UsuarioLogueado;
                        //Detalle.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;

                        decimal informeCRecibida = pParametro.InformesRecepcionesDetalles.Find(x => x.IdOrdenCompraDetalle == Detalle.IdOrdenCompraDetalle).CantidadRecibida;
                        //int informeCPagada = pParametro.InformesRecepcionesDetalles.Find(x => x.IdOrdenCompraDetalle == Detalle.IdOrdenCompraDetalle).CantidadPagada;
                        //para anular un informe de recepcion tengo que ver que items de la orden de compra fueron modificados y revertir los cambios
                        if (informeCRecibida != 0)
                        {

                            Detalle.CantidadRecibida = Detalle.CantidadRecibida - informeCRecibida;
                            if (Detalle.CantidadRecibida > 0)
                                Detalle.Estado.IdEstado = (int)EstadosInformesRecepciones.PatcialmenteRecibido;
                            else
                                Detalle.Estado.IdEstado = (int)EstadosInformesRecepciones.Activo;
                        }

                        //if (informeCPagada != 0)
                        //{

                        //    Detalle.CantidadPagada = Detalle.CantidadPagada - informeCPagada;
                        //}

                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(Detalle, db, tran, "CmpOrdenesComprasDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }



                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.OrdenCompra.OrdenesComprasDetalles.Find(x => x.IdOrdenCompraDetalle == Detalle.IdOrdenCompraDetalle), Acciones.Update, Detalle, db, tran))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }

                        break;
                        #endregion
                }

            }
            switch (pParametro.EstadoColeccion)
            {
                case EstadoColecciones.Agregado:
                    if (actualizarEstadoOrden)
                    {
                        pParametro.OrdenCompra.Estado.IdEstado = (int)EstadosOrdenesCompras.Recibido;
                        pParametro.OrdenCompra.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;
                        pParametro.OrdenCompra.FechaEvento = DateTime.Now;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro.OrdenCompra, db, tran, "CmpOrdenesComprasActualizarEstado"))
                        {
                            AyudaProgramacionLN.MapearError(pParametro.OrdenCompra, pParametro);
                            return false;
                        }
                    }
                    break;

                case EstadoColecciones.Borrado:
                    //si la orden estaba completamente recibida y algun item se modifico cambio estado a Activo
                    if (pParametro.OrdenCompra.Estado.IdEstado == (int)EstadosOrdenesCompras.Recibido && actualizarEstadoOrden)
                    {
                        pParametro.OrdenCompra.Estado.IdEstado = (int)EstadosOrdenesCompras.Activo;
                        pParametro.OrdenCompra.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;
                        pParametro.OrdenCompra.FechaEvento = DateTime.Now;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro.OrdenCompra, db, tran, "CmpOrdenesComprasActualizarEstado"))
                        {
                            AyudaProgramacionLN.MapearError(pParametro.OrdenCompra, pParametro);
                            return false;
                        }
                    }
                    break;
            }
            return true;
        }
        #endregion

        public List<CmpInformesRecepcionesDetalles> ObtenerDetallesPendientesFiltroPorProveedor(CmpInformesRecepciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CmpInformesRecepcionesDetalles>("CMPInformesRecepcionesListaFiltroPopUp", pParametro);
        }

        public DataTable ObtenerDetallesPendientesRecibirFiltroPorProveedor(CmpInformesRecepciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CMPInformesRecepcionesDetallesPendientesRecibirFiltroPorProveedor", pParametro);
        }

        public DataTable ObtenerAcopiosPendientesRecibirFiltroPorProveedor(CmpInformesRecepciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CMPInformesRecepcionesAcopiosPendientesRecibirFiltroPorProveedor", pParametro);
        }
    }
}
