using Auditoria;
using Auditoria.Entidades;
using Compras.Entidades;
using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Contabilidad.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Xml;

namespace Compras.LogicaNegocio
{
    public class CmpStockMovimientosLN : BaseLN<CmpStockMovimientos>
    {
        public override bool Agregar(CmpStockMovimientos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            if (!this.Validar(pParametro))
                return false;
            pParametro.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuario;

            if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.StockTransferencia)
            {
                TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.CircuitoConfirmacionTransferenciaStock);
                bool bvalor = false;
                if (!string.IsNullOrEmpty(valor.ParametroValor) && bool.TryParse(valor.ParametroValor, out bvalor))
                {
                    if (!bvalor)
                    {
                        pParametro.Estado.IdEstado = (int)EstadosStock.Confirmado;
                    }
                }
            }

            TGETiposOperaciones tipoOp = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(pParametro.TipoOperacion);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdStockMovimiento = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CmpStockMovimientosInsertar");
                    if (pParametro.IdStockMovimiento == 0)
                        resultado = false;

                    if (resultado && !ActualizarDetalles(pParametro, new CmpStockMovimientos(), bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (tipoOp.Contabiliza)
                    {
                        CtbAsientosContables asiento = new CtbAsientosContables();
                        asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
                        asiento.IdRefTipoOperacion = pParametro.IdStockMovimiento;
                        asiento.Filial.IdFilial = pParametro.Filial.IdFilial;
                        asiento.FechaAsiento = pParametro.FechaAlta;
                        asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
                        {
                            AyudaProgramacionLN.MapearError(asiento, pParametro);
                            resultado = false;
                        }
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

        private bool Validar(CmpStockMovimientos pParametro)
        {
            if (pParametro.Filial.IdFilial == pParametro.IdFilialDestino)
            {
                pParametro.CodigoMensaje = "StockMovimientosFiliales";
                return false;
            }
            if (pParametro.StockMovimientosDetalles.Exists(x => x.Producto.IdProducto == 0 && x.Producto.EstadoColeccion == EstadoColecciones.Agregado))
            {
                pParametro.CodigoMensaje = "StockMovimientosSeleccioneItem";
                return false;
            }
            if (pParametro.StockMovimientosDetalles.Exists(x => x.Producto.IdProducto != 0 && x.Cantidad == 0))
            {
                pParametro.CodigoMensaje = "ValidarItemsFacturaCantidad";
                return false;
            }
            return true;
        }

        private bool ActualizarDetalles(CmpStockMovimientos pParametro, CmpStockMovimientos pValorViejo, Database bd, DbTransaction tran)
        {
            CMPStock stock;
            CMPStockLN stockLN = new CMPStockLN();
            foreach (CmpStockMovimientosDetalles Detalle in pParametro.StockMovimientosDetalles)
            {
                Detalle.UsuarioLogueado = pParametro.UsuarioLogueado;
                switch (Detalle.EstadoColeccion)
                {
                    #region "Agregado"
                    case EstadoColecciones.Agregado:
                        Detalle.Estado.IdEstado = (int)EstadosStock.Activo;
                        Detalle.IdStockMovimiento = pParametro.IdStockMovimiento;
                        Detalle.IdStockMovimientoDetalle = BaseDatos.ObtenerBaseDatos().Agregar(Detalle, bd, tran, "CmpStockMovimientosDetallesInsertar");
                        if (Detalle.IdStockMovimientoDetalle == 0)
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }
                        #region Stock
                        //Actualizo tambien el Stock de productos
                        //if (Detalle.Producto.Familia.Stockeable)
                        //{
                        //Stock Fililal Origen o Alta y Baja
                        stock = new CMPStock();
                        stock.IdFilial = pParametro.Filial.IdFilial;
                        stock.Producto = Detalle.Producto;
                        if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.StockTransferencia
                            || pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.StockBaja)
                            stock.StockActual = -(int)Detalle.Cantidad;
                        else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.StockAlta)
                            stock.StockActual = (int)Detalle.Cantidad;

                        if (!stockLN.AgregarModificar(stock, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(stock, pParametro);
                            return false;
                        }

                        //Stock Destino Para Circuito Automatico
                        if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.StockTransferencia
                            && pParametro.Estado.IdEstado == (int)EstadosStock.Confirmado)
                        {
                            stock = new CMPStock();
                            stock.IdFilial = pParametro.IdFilialDestino.Value;
                            stock.Producto = Detalle.Producto;
                            stock.StockActual = (int)Detalle.Cantidad;
                            if (!stockLN.AgregarModificar(stock, bd, tran))
                            {
                                AyudaProgramacionLN.MapearError(stock, pParametro);
                                return false;
                            }
                        }
                        //}
                        #endregion
                        break;
                    #endregion

                    #region "Modificado"
                    case EstadoColecciones.Modificado:
                        Detalle.Estado.IdEstado = (int)EstadosStock.Activo;

                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(Detalle, bd, tran, "CmpStockMovimientosDetallesActualizarEstado"))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                           pValorViejo.StockMovimientosDetalles.Find(x => x.IdStockMovimientoDetalle == Detalle.IdStockMovimientoDetalle), Acciones.Update, Detalle, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }
                        #region Stock
                        //Actualizo tambien el Stock de productos
                        //if (Detalle.Producto.Familia.Stockeable)
                        //{
                        stock = new CMPStock();
                        stock.IdFilial = Convert.ToInt32(pParametro.IdFilialDestino);
                        stock.Producto = Detalle.Producto;
                        stock.StockActual = (int)Detalle.Cantidad;
                        if (!stockLN.AgregarModificar(stock, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(stock, pParametro);
                            return false;
                        }
                        //}
                        #endregion
                        break;
                    #endregion

                    #region Baja
                    case EstadoColecciones.Borrado:
                        Detalle.Estado.IdEstado = (int)EstadosStock.Baja;

                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(Detalle, bd, tran, "CmpStockMovimientosDetallesActualizarEstado"))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                           pValorViejo.StockMovimientosDetalles.Find(x => x.IdStockMovimientoDetalle == Detalle.IdStockMovimientoDetalle), Acciones.Update, Detalle, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }
                        #region Stock
                        //Actualizo tambien el Stock de productos
                        //if (Detalle.Producto.Familia.Stockeable)
                        //{
                        stock = new CMPStock();
                        stock.IdFilial = pParametro.Filial.IdFilial;
                        stock.Producto = Detalle.Producto;
                        if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.StockTransferencia
                            || pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.StockBaja)
                            stock.StockActual = (int)Detalle.Cantidad;
                        else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.StockAlta)
                            stock.StockActual = -(int)Detalle.Cantidad;
                        if (!stockLN.AgregarModificar(stock, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(stock, pParametro);
                            return false;
                        }
                        //Stock Destino Para Circuito Automatico
                        if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.StockTransferencia
                            && pParametro.Estado.IdEstado == (int)EstadosStock.Confirmado)
                        {
                            stock = new CMPStock();
                            stock.IdFilial = pParametro.IdFilialDestino.Value;
                            stock.Producto = Detalle.Producto;
                            stock.StockActual = -(int)Detalle.Cantidad;
                            if (!stockLN.AgregarModificar(stock, bd, tran))
                            {
                                AyudaProgramacionLN.MapearError(stock, pParametro);
                                return false;
                            }
                        }
                        //}
                        #endregion
                        break;
                        #endregion
                }
            }

            return true;
        }

        public override bool Modificar(CmpStockMovimientos pParametro)
        {
            return true;
        }

        public override CmpStockMovimientos ObtenerDatosCompletos(CmpStockMovimientos pParametro)
        {
            CmpStockMovimientos stockMov = BaseDatos.ObtenerBaseDatos().Obtener<CmpStockMovimientos>("CmpStockMovimientosSeleccionar", pParametro);
            stockMov.StockMovimientosDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<CmpStockMovimientosDetalles>("CmpStockMovimientosDetallesSeleccionarPorIdStockMovimiento", stockMov);
            stockMov.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            stockMov.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return stockMov;
        }

        public override List<CmpStockMovimientos> ObtenerListaFiltro(CmpStockMovimientos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CmpStockMovimientos>("CmpStockMovimientosObtenerListaFiltro", pParametro);
        }

        public bool Confirmar(CmpStockMovimientos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //if (!this.Validar(pParametro))
            //    return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CmpStockMovimientos valorViejo = new CmpStockMovimientos();
            valorViejo.IdStockMovimiento = pParametro.IdStockMovimiento;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            TGETiposOperaciones tipoOp = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(pParametro.TipoOperacion);

            pParametro.Estado.IdEstado = (int)EstadosStock.Confirmado;
            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CmpStockMovimientosActualizarEstado"))
                        resultado = false;

                    if (resultado && !ActualizarDetalles(pParametro, valorViejo, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (tipoOp.Contabiliza)
                    {
                        CtbAsientosContables asiento = new CtbAsientosContables();
                        asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
                        asiento.IdRefTipoOperacion = pParametro.IdStockMovimiento;
                        asiento.Filial.IdFilial = pParametro.Filial.IdFilial;
                        asiento.FechaAsiento = pParametro.FechaAlta;
                        asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
                        {
                            AyudaProgramacionLN.MapearError(asiento, pParametro);
                            resultado = false;
                        }
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

        public bool Anular(CmpStockMovimientos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Borrado;

            //if (!this.Validar(pParametro))
            //    return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CmpStockMovimientos valorViejo = new CmpStockMovimientos();
            valorViejo.IdStockMovimiento = pParametro.IdStockMovimiento;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            TGETiposOperaciones tipoOp = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(pParametro.TipoOperacion);

            pParametro.Estado.IdEstado = (int)EstadosStock.Baja;
            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CmpStockMovimientosActualizarEstado"))
                        resultado = false;

                    if (resultado && !ActualizarDetalles(pParametro, valorViejo, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (tipoOp.Contabiliza)
                    {
                        CtbAsientosContables asiento = new CtbAsientosContables();
                        asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
                        asiento.IdRefTipoOperacion = pParametro.IdStockMovimiento;
                        asiento.Filial.IdFilial = pParametro.Filial.IdFilial;
                        asiento.FechaAsiento = pParametro.FechaAlta;
                        asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbAsientosContablesAnular"))
                        {
                            AyudaProgramacionLN.MapearError(asiento, pParametro);
                            resultado = false;
                        }
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
        public DataTable ObtenerStockMovimientosPlantilla(CmpStockMovimientos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("cmpStopckMovimientosSeleccionarPlantilla", pParametro);
        }

        public DataTable ActualizarStockAlImportarExcel(DataTable dt)
        {
            DataTable filteredDt = dt.Clone();

            foreach (DataRow row in dt.Rows)
            {
                if (!row.ItemArray.All(field => field is DBNull || string.IsNullOrWhiteSpace(field.ToString())))
                    filteredDt.ImportRow(row);
            }
            Objeto OBJ = new Objeto();
            OBJ.LoteXML = filteredDt.ToXmlDocument();
            DataSet ds = BaseDatos.ObtenerBaseDatos().ObtenerDataSet("cmpActualizarStockAlImportarExcel", OBJ);
            if (ds.Tables.Count > 0 && ds.Tables[0] != null)
                return ds.Tables[0];
            else
                return filteredDt;
        }
    }
}
