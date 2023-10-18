using Auditoria;
using Auditoria.Entidades;
using Compras;
using Compras.Entidades;
using Compras.LogicaNegocio;
using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Producciones.Entidades;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Producciones.LogicaNegocio
{
    public class ProduccionesLN : BaseLN<PrdProducciones>
    {
        public override List<PrdProducciones> ObtenerListaFiltro(PrdProducciones pProduccion)
        {
            throw new NotImplementedException();
        }

        public DataTable ObtenerListaGrilla(PrdProducciones pProduccion)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("PrdProduccionesSeleccionarDescripcionPorFiltro", pProduccion);
        }

        public DataTable ObtenerTotalesProductoPorProduccion(PrdProducciones pProduccion)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("PrdProduccionesSeleccionarTotalesProductosPorProduccion", pProduccion);
        }

        public override PrdProducciones ObtenerDatosCompletos(PrdProducciones pProduccion)
        {
            pProduccion = BaseDatos.ObtenerBaseDatos().Obtener<PrdProducciones>("PrdProduccionesSeleccionar", pProduccion);
            pProduccion.ProduccionesDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<PrdProduccionesDetalles>("PrdProduccionesDetallesSeleccionarPorIdProduccion", pProduccion);
            //produccion.ProduccionesCentrosCostosProrrateo = BaseDatos.ObtenerBaseDatos().ObtenerLista<PrdProduccionesOcupantes>("PrdProduccionesOcupantesSeleccionarPorIdProduccion", pProduccion);
            return pProduccion;
        }

        public override bool Agregar(PrdProducciones pParametro)
        {
            if (pParametro.IdProduccion > 0)
                return true;
            
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.Estado.IdEstado = (int)Estados.Activo;
            pParametro.FechaAlta = DateTime.Now;
            pParametro.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuario;

            if (!this.Validar(pParametro, new PrdProducciones()))
                return false;

            //pParametro.ReservasXML.LoadXml(pParametro.Serialize<PrdProducciones>());

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

                    if (resultado && !this.ItemsActualizar(pParametro, new PrdProducciones(), bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    if(pParametro.Estado.IdEstado==(int)EstadosProducciones.Finalizado)
                        if(resultado && !this.ActualizarStockProducido(pParametro, bd, tran))
                            resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoProduccionAgregar";
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdProduccion.ToString());
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

        internal bool Agregar(PrdProducciones pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdProduccion = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "PrdProduccionesInsertar");
            if (pParametro.IdProduccion == 0)
                return false;

            return true;
        }

        private bool Validar(PrdProducciones pParametro, PrdProducciones pValorViejo)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "PrdProduccionesValidaciones");
        }

        public override bool Modificar(PrdProducciones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            if (!this.Validar(pParametro, new PrdProducciones()))
                return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            PrdProducciones valorViejo = new PrdProducciones();
            valorViejo.IdProduccion = pParametro.IdProduccion;
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
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "PrdProduccionesActualizar");

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

                    if (pParametro.Estado.IdEstado == (int)EstadosProducciones.Finalizado)
                        if (resultado && !this.ActualizarStockProducido(pParametro, bd, tran))
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

        public bool ModificarEstado(PrdProducciones pParametro, Database db, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, db, tran, "PrdProduccionesActualizarEstado"))
                return false;

            return true;
        }

        private bool ActualizarStockProducido(PrdProducciones pParametro, Database bd, DbTransaction tran)
        {
            if (pParametro.Producto.IdProducto > 0 && pParametro.CantidadProducida > 0)
            {
                CMPStock stock = new CMPStock();
                stock.ValidarStock = true;
                stock.IdFilial = pParametro.Filial.IdFilial;
                stock.Producto = pParametro.Producto;
                stock.StockActual = pParametro.CantidadProducida;
                if (!ComprasF.StockAgregarModificar(stock, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(stock, pParametro);
                    return false;
                }
            }
            return true;
        }

        #region "Items Reserva"

        private bool ItemsActualizar(PrdProducciones pParametro, PrdProducciones pValorViejo, Database bd, DbTransaction tran)
        {
            CMPStock stock;
            CMPStockLN stockLN = new CMPStockLN();
            foreach (PrdProduccionesDetalles item in pParametro.ProduccionesDetalles.Where(x => x.Producto.IdProducto > 0))
            {
                switch (item.EstadoColeccion)
                {
                    #region Agregado
                    case EstadoColecciones.Agregado:
                        item.UsuarioLogueado = pParametro.UsuarioLogueado;
                        item.UsuarioAlta.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuario;
                        item.IdProduccion = pParametro.IdProduccion;
                        item.IdProduccionDetalle = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "PrdProduccionesDetallesInsertar");
                        if (item.IdProduccionDetalle == 0)
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        #region Stock
                        //Actualizo tambien el Stock de productos
                        stock = new CMPStock();
                        stock.ValidarStock = true;
                        stock.IdFilial = pParametro.Filial.IdFilial;
                        stock.Producto = item.Producto;
                        stock.StockActual = -item.Cantidad * item.Sentido;
                        if (!stockLN.AgregarModificar(stock, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(stock, pParametro);
                            return false;
                        }
                        #endregion
                        break;
                    #endregion

                    #region Modificado
                    case EstadoColecciones.Modificado:
                        item.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "PrdProduccionesDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.ProduccionesDetalles.Find(x => x.IdProduccionDetalle == item.IdProduccionDetalle), Acciones.Update, item, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }

                        #region Stock
                        //Actualizo tambien el Stock de productos
                        stock = new CMPStock();
                        stock.ValidarStock = true;
                        stock.IdFilial = pParametro.Filial.IdFilial;
                        stock.Producto = item.Producto;
                        stock.StockActual = -item.Cantidad * item.Sentido;
                        if (!stockLN.AgregarModificar(stock, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(stock, pParametro);
                            return false;
                        }
                        #endregion
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