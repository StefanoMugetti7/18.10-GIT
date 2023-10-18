using Auditoria;
using Auditoria.Entidades;
using Cargos;
using Cargos.Entidades;
using Compras.Entidades;
using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Proveedores.Entidades;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Compras.LogicaNegocio
{
    class CmpOrdenesComprasLN : BaseLN<CmpOrdenesCompras>
    {
        public override bool Modificar(CmpOrdenesCompras pParametro)
        {
            throw new NotImplementedException();
        }
        public override CmpOrdenesCompras ObtenerDatosCompletos(CmpOrdenesCompras pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CmpOrdenesCompras>("CmpOrdenesComprasSeleccionar", pParametro);
            pParametro.Proveedor = BaseDatos.ObtenerBaseDatos().Obtener<CapProveedores>("CapProveedoresSeleccionarDescripcion", pParametro);
            pParametro.OrdenesComprasDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<CmpOrdenesComprasDetalles>("CmpOrdenesComprasDetallesSeleccionarPorOrdenCompra", pParametro);
            pParametro.SolicitudesComprasDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<CmpSolicitudesComprasDetalles>("CmpSolicitudesComprasDetallesSeleccionarPorOrdenCompra", pParametro);
            pParametro.OrdenesComprasValores = BaseDatos.ObtenerBaseDatos().ObtenerLista<CapOrdenesComprasValores>("CapOrdenesComprasValoresSeleccionarPorOrdenCompra", pParametro);
            return pParametro;
        }
        public override List<CmpOrdenesCompras> ObtenerListaFiltro(CmpOrdenesCompras pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CmpOrdenesCompras>("CmpOrdenesComprasSeleccionarDescripcionPorFiltro", pParametro);
        }
        public List<CmpOrdenesCompras> ObtenerListaFiltroPopUp(CmpOrdenesCompras pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CmpOrdenesCompras>("CmpOrdenesComprasSeleccionarDescripcionPorFiltroPopUp", pParametro);
        }
        public DataTable ObtenerListaDetalleFiltroPopUp(CmpOrdenesCompras pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CmpOrdenesComprasDetallesSeleccionarDescripcionPorFiltroPopUp", pParametro);
        }
        /// <summary>
        /// Devuelve las OC de Terceros Autorizadas para Confirmar
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public List<CmpOrdenesCompras> ObtenerTercerosAutorizadas(CmpOrdenesCompras pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CmpOrdenesCompras>("CmpOrdenesComprasSeleccionarAutorizadas", pParametro);
        }
        /// <summary>
        /// Devuelve las Ordenes de Compras de Terceros pendientes de pago a un proveedor
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public List<CmpOrdenesCompras> ObtenerTercerosPendientes(CmpOrdenesCompras pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CmpOrdenesCompras>("CmpOrdenesComprasSeleccionarTercerosPendientes", pParametro);
        }
        /// <summary>
        /// Devuelve las OCD Tipo Terceros Pendientes de Solicitud de Pago
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public List<CmpOrdenesComprasDetalles> ObtenerPendientesSolicitudPago(CmpOrdenesCompras pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CmpOrdenesComprasDetalles>("CmpOrdenesComprasDetallesSeleccionarPendientesSolicitudPago", pParametro);
        }
        public List<CmpSolicitudesComprasDetalles> ObtenerSCDPorProveedor(CmpOrdenesCompras pOrden)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CmpSolicitudesComprasDetalles>("CmpSolicitudesComprasDetallesCotizadasPorProveedor", pOrden);
        }
        public override bool Agregar(CmpOrdenesCompras pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            // if (pParametro.TipoSolicitudPago.IdTipoSolicitudPago == (int)EnumTiposSolicitudPago.Compras)
            if (!this.Validar(pParametro, new CmpOrdenesCompras()))
                return false;

            pParametro.FechaEvento = DateTime.Now;
            pParametro.Estado.IdEstado = (int)EstadosOrdenesCompras.Activo;
            pParametro.FechaAlta = DateTime.Now;
            pParametro.UsuarioAlta.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuarioEvento;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdOrdenCompra = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CmpOrdenesComprasInsertar");
                    if (pParametro.IdOrdenCompra == 0)
                        resultado = false;

                    if (resultado && !this.ActualizarDetalles(pParametro, new CmpOrdenesCompras(), bd, tran))
                        resultado = false;

                    if (resultado && !this.ModificarDetallesSolicitudesCompras(pParametro, new CmpOrdenesCompras(), bd, tran))
                        resultado = false;

                    if (resultado && !this.ActualizarOrdenesComprasValores(pParametro, new CmpOrdenesCompras(), bd, tran))
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
        public bool Autorizar(CmpOrdenesCompras pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            if (!this.Validar(pParametro, new CmpOrdenesCompras()))
                return false;

            pParametro.FechaEvento = DateTime.Now;
            pParametro.Estado.IdEstado = (int)EstadosOrdenesCompras.Autorizado;
            pParametro.UsuarioAutorizar.IdUsuarioAutorizar = pParametro.UsuarioLogueado.IdUsuarioEvento;
            pParametro.FechaAutorizacion = DateTime.Now;
            //pParametro.fecha = DateTime.Now;


            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CmpOrdenesComprasActualizar"))
                        resultado = false;

                    if (resultado && !this.ActualizarDetalles(pParametro, new CmpOrdenesCompras(), bd, tran))
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
        public bool Anular(CmpOrdenesCompras pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Borrado;

            // if (pParametro.TipoSolicitudPago.IdTipoSolicitudPago == (int)EnumTiposSolicitudPago.Compras)
            //if (!this.Validar(pParametro, new CmpOrdenesCompras()))
            //    return false;

            pParametro.FechaEvento = DateTime.Now;
            pParametro.Estado.IdEstado = (int)EstadosOrdenesCompras.Baja;
            pParametro.FechaBaja = DateTime.Now;
            //PREGUNTAR¬
            pParametro.IdUsuarioBaja = pParametro.UsuarioLogueado.IdUsuarioEvento;
            //pParametro.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {

                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CmpOrdenesComprasActualizar"))
                        resultado = false;

                    if (resultado && !this.ActualizarDetalles(pParametro, new CmpOrdenesCompras(), bd, tran))
                        resultado = false;

                    if (resultado && !this.ModificarDetallesSolicitudesCompras(pParametro, new CmpOrdenesCompras(), bd, tran))
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
        public bool ModificarEstado(CmpOrdenesCompras pParametro, Database db, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, db, tran, "CmpOrdenesComprasActualizarEstado"))
            {
                return false;
            }
            //if (!AuditoriaF.AuditoriaAgregar(pValorViejo, Acciones.Update, pParametro, db, tran))
            //{
            //    return false;
            //}
            return true;
        }
        private bool ActualizarOrdenesComprasValores(CmpOrdenesCompras pParametro, CmpOrdenesCompras pValorViejo, Database db, DbTransaction tran)
        {
            foreach (CapOrdenesComprasValores opValor in pParametro.OrdenesComprasValores)
            {
                opValor.UsuarioLogueado = pParametro.UsuarioLogueado;
                switch (opValor.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        opValor.IdOrdenCompra = pParametro.IdOrdenCompra;
                        opValor.Estado.IdEstado = (int)Estados.Activo;
                        opValor.IdOrdenCompraValor = BaseDatos.ObtenerBaseDatos().Agregar(opValor, db, tran, "CapOrdenesComprasValoresInsertar");
                        if (opValor.IdOrdenCompraValor == 0)
                        {
                            AyudaProgramacionLN.MapearError(opValor, pParametro);
                            return false;
                        }
                        break;
                    case EstadoColecciones.Modificado:
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(opValor, db, tran, "CapOrdenesComprasValoresActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(opValor, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.OrdenesComprasValores.Find(x => x.IdOrdenCompraValor == opValor.IdOrdenCompraValor), Acciones.Update, opValor, db, tran))
                        {
                            AyudaProgramacionLN.MapearError(opValor, pParametro);
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }
            return true;
        }
        #region "Validaciones"
        private bool Validar(CmpOrdenesCompras pParametro, CmpOrdenesCompras pValorViejo)
        {

            switch (pParametro.EstadoColeccion)
            {
                case EstadoColecciones.Agregado:
                    if (pParametro.Proveedor.IdProveedor.HasValue == false || pParametro.Proveedor.IdProveedor == 0)
                    {
                        pParametro.CodigoMensaje = "ValidarOrdenCompraProveedor";
                        return false;
                    }
                    if (pParametro.OrdenesComprasDetalles.Count(x => x.EstadoColeccion == EstadoColecciones.Agregado) == 0)
                    {
                        pParametro.CodigoMensaje = "ValidarOrdenCompraDetalles";
                        return false;
                    }
                    if (pParametro.TipoOrdenCompra.IdTipoOrdenCompra == (int)EnumTiposOrdenesCompras.Terceros)
                    {
                        //Si se saca esta validcion se debe revisar el modulo de Solicitud de Pago
                        if (pParametro.OrdenesComprasDetalles.Count(x => x.EstadoColeccion == EstadoColecciones.Agregado) != 1)
                        {
                            pParametro.CodigoMensaje = "ValidarOrdenCompraTerceroDetalles";
                            return false;
                        }
                        if (!pParametro.CuotasDescuentoAfiliado.HasValue || pParametro.CuotasDescuentoAfiliado.Value == 0)
                        {
                            pParametro.CodigoMensaje = "ValidarCuotasDescuentoAfiliado";
                            return false;
                        }
                        if (!pParametro.CuotasPagoProveedor.HasValue || pParametro.CuotasPagoProveedor.Value == 0)
                        {
                            pParametro.CodigoMensaje = "ValidarCuotasPagoProveedor";
                            return false;
                        }
                        if (pParametro.FormaCobroAfiliado.IdFormaCobroAfiliado == 0)
                        {
                            pParametro.CodigoMensaje = "ValidarFormaCobroAfiliado";
                            return false;
                        }
                    }

                    break;
                case EstadoColecciones.AgregadoBorradoMemoria:
                    break;
                case EstadoColecciones.Borrado:
                    break;
                case EstadoColecciones.Modificado:
                    break;
                default:
                    break;
            }

            return true;
        }
        #endregion
        #region "Orden Compras Detalles"
        private bool ActualizarDetalles(CmpOrdenesCompras pParametro, CmpOrdenesCompras pValorViejo, Database db, DbTransaction tran)
        {
            foreach (CmpOrdenesComprasDetalles Detalle in pParametro.OrdenesComprasDetalles)
            {
                switch (Detalle.EstadoColeccion)
                {
                    #region "Agregado"
                    case EstadoColecciones.Agregado:
                        if (Detalle.Producto.IdProducto > 0)
                        {
                            Detalle.UsuarioLogueado = pParametro.UsuarioLogueado;
                            Detalle.Estado.IdEstado = (int)Estados.Activo;
                            //Detalle.IdOrdenCompra = pParametro.IdOrdenCompra;
                            Detalle.OrdenCompra.IdOrdenCompra = pParametro.IdOrdenCompra;
                            Detalle.IdOrdenCompraDetalle = BaseDatos.ObtenerBaseDatos().Agregar(Detalle, db, tran, "CmpOrdenComprasDetallesInsertar");
                            if (Detalle.IdOrdenCompraDetalle == 0)
                            {
                                AyudaProgramacionLN.MapearError(Detalle, pParametro);
                                return false;
                            }
                        }
                        break;
                    #endregion

                    #region "Modificado"
                    case EstadoColecciones.Modificado:
                        Detalle.UsuarioLogueado = pParametro.UsuarioLogueado;           //CREAR STORE  ¬
                        Detalle.OrdenCompra.IdOrdenCompra = pParametro.IdOrdenCompra;

                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(Detalle, db, tran, "CmpOrdenesComprasDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.OrdenesComprasDetalles.Find(x => x.IdOrdenCompraDetalle == Detalle.IdOrdenCompraDetalle), Acciones.Update, Detalle, db, tran))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }

                        break;
                    #endregion

                    #region "Borrado"
                    case EstadoColecciones.Borrado:
                        Detalle.UsuarioLogueado = pParametro.UsuarioLogueado;
                        //Detalle.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;
                        Detalle.Estado.IdEstado = (int)Estados.Baja;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(Detalle, db, tran, "CmpOrdenesComprasDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.OrdenesComprasDetalles.Find(x => x.IdOrdenCompraDetalle == Detalle.IdOrdenCompraDetalle), Acciones.Update, Detalle, db, tran))
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
        public bool ActualizarDetalle(CmpOrdenesComprasDetalles pParametro, CmpOrdenesComprasDetalles pValorViejo, Database db, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, db, tran, "CmpOrdenesComprasDetallesActualizar"))
            {
                return false;
            }
            if (!AuditoriaF.AuditoriaAgregar(pValorViejo, Acciones.Update, pParametro, db, tran))
            {
                return false;
            }
            return true;
        }
        public List<CmpOrdenesComprasDetalles> ObtenerDetallesPorOrdenCompra(CmpOrdenesCompras pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CmpOrdenesComprasDetalles>("CmpOrdenesComprasDetallesSeleccionarPorOrdenCompra", pParametro);
        }
        #endregion
        private bool ModificarDetallesSolicitudesCompras(CmpOrdenesCompras pParametro, CmpOrdenesCompras pValorViejo, Database bd, DbTransaction tran)
        {


            foreach (CmpSolicitudesComprasDetalles Detalle in pParametro.SolicitudesComprasDetalles)
            {

                switch (pParametro.EstadoColeccion)
                {

                    case EstadoColecciones.Agregado:

                        Detalle.UsuarioLogueado = pParametro.UsuarioLogueado;
                        Detalle.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;

                        if (pParametro.OrdenesComprasDetalles.Exists(x => x.Producto.IdProducto == Detalle.Producto.IdProducto && x.IncluirEnOP == true) == true)
                        {
                            Detalle.IdOrdenCompraDetalle = pParametro.OrdenesComprasDetalles.Find(x => x.Producto.IdProducto == Detalle.Producto.IdProducto).IdOrdenCompraDetalle;
                            Detalle.Estado.IdEstado = (int)EstadosSolicitudesCompras.EnOrdenCompra;
                            if (!BaseDatos.ObtenerBaseDatos().Actualizar(Detalle, bd, tran, "CmpSolicitudesComprasDetallesActualizar"))
                            {
                                AyudaProgramacionLN.MapearError(Detalle, pParametro);
                                return false;
                            }

                            //  if (!AuditoriaF.AuditoriaAgregar(
                            //      pValorViejo.SolicitudCompraDetalles.Find(x => x.IdSolicitudCompraDetalle == Detalle.IdSolicitudCompraDetalle), Acciones.Update, Detalle, db, tran))
                            //  {
                            //      AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            //      return false;
                            //  }


                        }
                        break;

                    case EstadoColecciones.Borrado:

                        Detalle.IdOrdenCompraDetalle = null;
                        Detalle.Estado.IdEstado = (int)EstadosSolicitudesCompras.Cotizado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(Detalle, bd, tran, "CmpSolicitudesComprasDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }

                        break;

                }

            }

            return true;
        }
        public bool ConfirmarLista(List<CmpOrdenesCompras> pParametro, Objeto pObjeto)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pObjeto);
            bool resultado = true;

            if (pParametro.Where(x => x.Check).Count() == 0)
            {
                pObjeto.CodigoMensaje = "SeleccioneAlgunaOrden";
                return false;
            }

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {

                    foreach (CmpOrdenesCompras orden in pParametro.Where(x => x.Check))
                    {
                        if (orden.Check)
                        {
                            orden.Estado.IdEstado = (int)EstadosOrdenesCompras.Confirmado;

                            if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(orden, bd, tran, "CmpOrdenesComprasActualizar"))
                            {
                                AyudaProgramacionLN.MapearError(orden, pObjeto);
                                resultado = false;
                                break;
                            }

                            //Genero los Cargos a Descontar al Socio
                            CarTiposCargosAfiliadosFormasCobros cargoAfiliado = new CarTiposCargosAfiliadosFormasCobros();
                            cargoAfiliado.IdAfiliado = orden.Afiliado.IdAfiliado;
                            cargoAfiliado.TipoCargo.IdTipoCargo = (int)EnumTiposCargos.OrdenesComprasTerceros;
                            cargoAfiliado.FormaCobroAfiliado = orden.FormaCobroAfiliado;
                            cargoAfiliado.FechaAlta = new DateTime(Convert.ToInt32(orden.PeriodoPrimerVencimiento.ToString().Substring(0, 4)), Convert.ToInt32(orden.PeriodoPrimerVencimiento.ToString().Substring(4, 2)), 1);
                            cargoAfiliado.NoModificarFechaAlta = true;
                            cargoAfiliado.FechaAltaEvento = DateTime.Now;
                            cargoAfiliado.Estado.IdEstado = (int)EstadosCargos.Activo;
                            cargoAfiliado.UsuarioAlta.IdUsuarioAlta = orden.UsuarioLogueado.IdUsuarioEvento;
                            cargoAfiliado.UsuarioLogueado = orden.UsuarioLogueado;
                            cargoAfiliado.CantidadCuotas = orden.CuotasDescuentoAfiliado.Value;
                            // Calculo del interes directo
                            //TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.TasaDeInteres);
                            //decimal importeInteres = Math.Round(orden.CuotasDescuentoAfiliado.Value * Convert.ToDecimal(valor.ParametroValor) / 100 * orden.ImporteDescontar.Value, 2);
                            //decimal importeNuevo = orden.ImporteDescontar.Value + importeInteres;
                            cargoAfiliado.ImporteCargo = orden.ImporteDescontar.Value;
                            cargoAfiliado.ImporteInteres = 0;// importeInteres;
                            cargoAfiliado.ImporteCuota = Math.Round(cargoAfiliado.ImporteCargo.Value / cargoAfiliado.CantidadCuotas, 2);//Math.Round(importeNuevo / cargoAfiliado.CantidadCuotas, 2);
                            cargoAfiliado.ImporteTotal = cargoAfiliado.ImporteCargo.Value; //importeNuevo;
                            cargoAfiliado.IdReferenciaRegistro = orden.IdOrdenCompra;
                            cargoAfiliado.TablaReferenciaRegistro = orden.GetType().Name;
                            cargoAfiliado.Detalle = string.Concat("Nro: ", orden.IdOrdenCompra.ToString(), " ", orden.Proveedor.RazonSocial);

                            if (!CargosF.TiposCargosAfiliadosAgregar(cargoAfiliado, bd, tran))
                            {
                                AyudaProgramacionLN.MapearError(cargoAfiliado, pObjeto);
                                resultado = false;
                                break;
                            }
                        }
                    }
                    if (resultado)
                    {
                        tran.Commit();
                        pObjeto.CodigoMensaje = "ResultadoTransaccion";
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
                    pObjeto.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pObjeto.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;

        }
    }
}
