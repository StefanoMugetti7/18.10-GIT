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
using System.Xml;

namespace Hoteles.LogicaNegocio
{
    class ReservasLN : BaseLN<HTLReservas>
    {
        public override List<HTLReservas> ObtenerListaFiltro(HTLReservas pReserva)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<HTLReservas>("HTLReservasSeleccionarDescripcionPorFiltro", pReserva);
        }

        public List<T> ObtenerAjaxComboDetalleGastos<T>(HTLReservasDetalles filtro, List<HTLReservasDetalles> gastosSeleccionados) where T : new()
        {
            #region Armar XML
            filtro.LoteCamposValores = new XmlDocument();
            XmlNode nodos = filtro.LoteCamposValores.CreateElement("ReservasDetalles");
            filtro.LoteCamposValores.AppendChild(nodos);

            XmlNode itemNodo;
            XmlAttribute itemAttribute;
            foreach (HTLReservasDetalles item in gastosSeleccionados)
            {
                if (item.IdHabitacion.HasValue)
                {
                    itemNodo = filtro.LoteCamposValores.CreateElement("ReservaDetalle");
                    itemAttribute = filtro.LoteCamposValores.CreateAttribute("IdHabitacion");
                    itemAttribute.Value = item.IdHabitacion.ToString();
                    itemNodo.Attributes.Append(itemAttribute);

                    itemAttribute = filtro.LoteCamposValores.CreateAttribute("FechaIngreso");
                    itemAttribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(item.FechaIngreso.Value);
                    itemNodo.Attributes.Append(itemAttribute);
                    nodos.AppendChild(itemNodo);

                    itemAttribute = filtro.LoteCamposValores.CreateAttribute("FechaEgreso");
                    itemAttribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(item.FechaEgreso.Value);
                    itemNodo.Attributes.Append(itemAttribute);
                    nodos.AppendChild(itemNodo);

                    itemAttribute = filtro.LoteCamposValores.CreateAttribute("Compartida");
                    itemAttribute.Value = item.Compartida.ToString();
                    itemNodo.Attributes.Append(itemAttribute);
                    nodos.AppendChild(itemNodo);

                    itemAttribute = filtro.LoteCamposValores.CreateAttribute("IdHabitacionDetalle");
                    itemAttribute.Value = item.HabitacionDetalle.IdHabitacionDetalle.ToString();
                    itemNodo.Attributes.Append(itemAttribute);
                    nodos.AppendChild(itemNodo);
                }
            }
            #endregion
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<T>("HTLReservasSeleccionarAjaxComboDetalleGastos", filtro);
        }

        public T ObtenerDetalleGastos<T>(HTLReservasDetalles filtro) where T : new()
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<T>("HTLReservasSeleccionarDetalleGastos", filtro);
        }

        public DataTable ObtenerListaGrilla(HTLReservas pReserva)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("HTLReservasSeleccionarDescripcionPorFiltro", pReserva);
        }

        public DataTable ObtenerAgenda(HTLReservas pReserva)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("HTLReservasDetallesAgenda", pReserva);
        }

        public DataTable ObtenerProductosPorTipo(HTLReservasDetalles pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("HTLProductosSeleccionarPorTipo", pParametro);
        }

        public override HTLReservas ObtenerDatosCompletos(HTLReservas pReserva)
        {
            HTLReservas presupuesto = BaseDatos.ObtenerBaseDatos().Obtener<HTLReservas>("HTLReservasSeleccionar", pReserva);
            presupuesto.ReservasDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<HTLReservasDetalles>("HTLReservasDetallesSeleccionarPorIdReserva", pReserva);
            foreach(HTLReservasDetalles item in presupuesto.ReservasDetalles)
                item.ReservasDetallesDescuentos = BaseDatos.ObtenerBaseDatos().ObtenerLista<HTLReservasDetallesDescuentos>("HTLReservasDetallesDescuentosSeleccionarPorIdReservaDetalle", item);
            presupuesto.ReservasOcupantes = BaseDatos.ObtenerBaseDatos().ObtenerLista<HTLReservasOcupantes>("HTLReservasOcupantesSeleccionarPorIdReserva", pReserva);
            presupuesto.Archivos = TGEGeneralesF.ArchivosObtenerLista(presupuesto);
            presupuesto.Comentarios = TGEGeneralesF.ComentariosObtenerLista(presupuesto);
            return presupuesto;
        }

        public override bool Agregar(HTLReservas pParametro)
        {
            if (pParametro.IdReserva > 0)
                return true;

            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            //pParametro.Estado.IdEstado = (int)Estados.Activo;
            pParametro.FechaAlta = DateTime.Now;

            if (!this.Validaciones(pParametro))
                return false;
            //pParametro.ReservasXML.LoadXml(pParametro.Serialize<HTLReservas>());

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "HTLReservasValidaciones");

                    if (resultado && !this.Agregar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.ItemsActualizar(pParametro, new HTLReservas(), bd, tran))
                        resultado = false;

                    if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(pParametro, bd, tran, "HTLReservasDetallesActualizarDatos"))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoHotelAgregar";
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdReserva.ToString());
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
     
        internal bool Agregar(HTLReservas pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdReserva = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "HTLReservasInsertar");
            if (pParametro.IdReserva == 0)
                return false;

            return true;
        }

        public override bool Modificar(HTLReservas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            if (!this.Validaciones(pParametro))
                return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            HTLReservas valorViejo = new HTLReservas();
            valorViejo.IdReserva = pParametro.IdReserva;
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
                    resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "HTLReservasValidaciones");

                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "HTLReservasActualizar"))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.ItemsActualizar(pParametro, valorViejo, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

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

        public bool ModificarEstado(HTLReservas pParametro, Database db, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, db, tran, "HTLReservasActualizarEstado"))
                return false;

            return true;
        }

        private bool Validaciones(HTLReservas pParametro)
        {
            if (pParametro.ReservasDetalles.Count(x => x.Estado.IdEstado == (int)Estados.Activo) == 0)
            {
                pParametro.CodigoMensaje = "ValidarReservasItemsCantidad";
                return false;
            }
            List<HTLReservasDetallesDescuentos> descuentos = new List<HTLReservasDetallesDescuentos>();
            pParametro.ReservasDetalles.ForEach(x => descuentos.AddRange(x.ReservasDetallesDescuentos));
            if (descuentos.Where(x=>x.Estado.IdEstado!=(int)Estados.Baja).GroupBy(x => x.TipoDescuento.IdTipoDescuento).Where(grp=>grp.Count() > 1).ToList().Count > 0)
            {
                pParametro.CodigoMensaje = "ValidarReservasItemsDescuentosDuplicados";
                return false;
            }
            if (pParametro.ReservasDetalles.Exists(x => x.IdTipoProductoHotel > 0 && (!x.IdListaPrecioDetalle.HasValue || x.IdListaPrecioDetalle.Value == 0)))
            {
                //  pp("Debe ingresar un Detalle para cada item que tenga un Tipo de Producto seleccionado", true);
                pParametro.CodigoMensaje = "ValidarReservasItemsProductosSeleccionados";

                return false;
            }
            if (!pParametro.ReservasDetalles.Exists(x => x.IdListaPrecioDetalle > 0))
            {
               // MostrarMensaje("Debe ingresar al menos un item a la Reserva", true);
                pParametro.CodigoMensaje ="ValidarReservasItemReserva";
           
                return false;
            }
            if (pParametro.ReservasDetalles.Exists(x => x.FechaIngreso < pParametro.FechaIngreso.Value.Date || x.FechaIngreso > pParametro.FechaEgreso.Value.Date))
            {
                //Las Fechas Ingresadas En El Detalle Deben Estar Dentro del Rango de Ingreso y Egreso
                pParametro.CodigoMensaje = "ValidarReservasFechasReservas";
                return false;
            }
            if (pParametro.ReservasDetalles.Exists(x => x.FechaEgreso < pParametro.FechaIngreso.Value.Date || x.FechaEgreso > pParametro.FechaEgreso.Value.Date))
            {
                pParametro.CodigoMensaje = "ValidarReservasFechasReservas";
                return false;
            }
            return true;
        }

        #region "Items Reserva"

        public decimal ObtenerPrecio(HTLReservasDetalles pParametro)
        {
            HTLReservasDetalles precio = BaseDatos.ObtenerBaseDatos().Obtener<HTLReservasDetalles>("HTLReservasDetallesSeleccionarPrecio", pParametro);
            return precio.Precio;
        }

        private bool ItemsActualizar(HTLReservas pParametro, HTLReservas pValorViejo, Database bd, DbTransaction tran)
        {
            foreach (HTLReservasDetalles item in pParametro.ReservasDetalles.Where(x=> x.IdTipoProductoHotel.HasValue))
            {
                switch (item.EstadoColeccion)
                {
                    #region Agregado
                    case EstadoColecciones.Agregado:
                        item.UsuarioLogueado = pParametro.UsuarioLogueado;
                        item.IdReserva = pParametro.IdReserva;
                        item.IdReservaDetalle = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "HTLReservasDetallesInsertar");
                        if (item.IdReservaDetalle == 0)
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
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "HTLReservasDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.ReservasDetalles.Find(x => x.IdReservaDetalle == item.IdReservaDetalle), Acciones.Update, item, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        break;
                    #endregion
                    default:
                        break;
                }
                foreach (HTLReservasDetallesDescuentos descuento in item.ReservasDetallesDescuentos.Where(x => x.TipoDescuento.IdTipoDescuento > 0 && x.Cantidad > 0 ))
                {
                    switch (descuento.EstadoColeccion)
                    {
                        #region Agregado
                        case EstadoColecciones.Agregado:
                            descuento.UsuarioLogueado = item.UsuarioLogueado;
                            descuento.IdReservaDetalle = item.IdReservaDetalle;
                            descuento.IdReservaDetalleDescuento = BaseDatos.ObtenerBaseDatos().Agregar(descuento, bd, tran, "HTLReservasDetallesDescuentosInsertar");
                            if (descuento.IdReservaDetalleDescuento == 0)
                            {
                                AyudaProgramacionLN.MapearError(descuento, pParametro);
                                return false;
                            }
                            break;
                        #endregion

                        #region Modificado
                        case EstadoColecciones.Modificado:
                        case EstadoColecciones.Borrado:
                            descuento.UsuarioLogueado = item.UsuarioLogueado;
                            if (!BaseDatos.ObtenerBaseDatos().Actualizar(descuento, bd, tran, "HTLReservasDetallesDescuentosActualizar"))
                            {
                                AyudaProgramacionLN.MapearError(descuento, pParametro);
                                return false;
                            }
                            //if (!AuditoriaF.AuditoriaAgregar(
                            //    pValorViejo.ReservasDetalles.Find(x => x.IdReservaDetalle == item.IdReservaDetalle), Acciones.Update, item, bd, tran))
                            //{
                            //    AyudaProgramacionLN.MapearError(item, pParametro);
                            //    return false;
                            //}
                            break;
                        #endregion
                        default:
                            break;
                    }

                }
            }

            foreach (HTLReservasOcupantes item in pParametro.ReservasOcupantes.Where(x=> !string.IsNullOrEmpty(x.Apellido) || x.EdadFechaSalida.HasValue ))
            {
                switch (item.EstadoColeccion)
                {
                    #region Agregado
                    case EstadoColecciones.Agregado:
                        item.IdReserva = pParametro.IdReserva;
                        item.IdReservaOcupante = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "HTLReservasOcupantesInsertar");
                        if (item.IdReservaOcupante == 0)
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
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "HTLReservasOcupantesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.ReservasOcupantes.Find(x => x.IdReservaOcupante == item.IdReservaOcupante), Acciones.Update, item, bd, tran))
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


        #endregion
    }
}

