using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compras.Entidades;
using Comunes;
using Servicio.AccesoDatos;
using Auditoria;
using Auditoria.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using System.Data;

namespace Compras.LogicaNegocio
{
    public class CMPStockLN : BaseLN<CMPStock>
    {
        public override bool Agregar(CMPStock pParametro)
        {
            return false;
            //AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            //bool resultado = true;
            //pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            ////if (!this.Validar(pParametro))
            ////    return false;
            //pParametro.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuario;
            //pParametro.FechaAlta = DateTime.Now;

            //DbTransaction tran;
            //DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            //using (DbConnection con = bd.CreateConnection())
            //{
            //    con.Open();
            //    tran = con.BeginTransaction();

            //    try
            //    {

            //        pParametro.IdStock = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CMPStockInsertar");
            //        if (pParametro.IdStock == 0)
            //            resultado = false;

            //        if (resultado)
            //        {
            //            tran.Commit();
            //            pParametro.CodigoMensaje = "ResultadoTransaccion";
            //        }
            //        else
            //        {
            //            tran.Rollback();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        ExceptionHandler.HandleException(ex, "LogicaNegocio");
            //        tran.Rollback();
            //        pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
            //        pParametro.CodigoMensajeArgs.Add(ex.Message);
            //        return false;
            //    }
            //}
            //return resultado;
        }

        public override bool Modificar(CMPStock pParametro)
        {
            return false;
            //AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            //bool resultado = true;
            //pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            ////Obtengo el valor actual del objeto antes de modificarlo
            ////para el Historial de Auditoria
            //CMPStock valorViejo = new CMPStock();
            //valorViejo.IdStock = pParametro.IdStock;
            //valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            //valorViejo = this.ObtenerDatosCompletos(valorViejo);

            ////if (!this.Validar(pParametro))
            ////    return false;

            //DbTransaction tran;
            //DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            //using (DbConnection con = bd.CreateConnection())
            //{
            //    con.Open();
            //    tran = con.BeginTransaction();

            //    try
            //    {
            //        if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CMPStockActualizar"))
            //            resultado = false;

            //        if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
            //            resultado = false;

                    
            //        if (resultado)
            //        {
            //            tran.Commit();
            //            pParametro.CodigoMensaje = "ResultadoTransaccion";
            //        }
            //        else
            //        {
            //            tran.Rollback();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        ExceptionHandler.HandleException(ex, "LogicaNegocio");
            //        tran.Rollback();
            //        pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
            //        pParametro.CodigoMensajeArgs.Add(ex.Message);
            //        return false;
            //    }
            //}
            //return resultado;
        }

        /// <summary>
        /// Agregar o Actualiza un Producto en la Tabla STOCK.
        /// Valida que el Stock no quede en Negativo.
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool AgregarModificar(CMPStock pParametro, Database bd, DbTransaction tran)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            if (pParametro.IdFilial == 0)
            {
                pParametro.CodigoMensaje = "ParametroStockFilialNoDefinido";
                return false;
            }
            if (pParametro.Producto.IdProducto == 0)
            {
                pParametro.CodigoMensaje = "ParametroStockProductoNoDefinido";
                return false;
            }

            CMPStock stockActual = this.ObtenerDatosCompletos(pParametro, bd, tran);
            stockActual.StockActualOriginal = stockActual.StockActual;
            if (stockActual.IdStock == 0)
            {
                if (pParametro.ValidarStock && stockActual.StockActual < 0)
                {
                    pParametro.CodigoMensaje = "StockInsuficiente";
                    pParametro.CodigoMensajeArgs.Add(pParametro.Producto.Descripcion);
                    pParametro.CodigoMensajeArgs.Add(stockActual.StockActualOriginal.ToString());
                    return false;
                }
                pParametro.IdStock = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CMPStockInsertar");
                if (pParametro.IdStock == 0)
                    resultado = false;
            }
            else
            {
                stockActual.StockActual += pParametro.StockActual;
                if (pParametro.ValidarStock && stockActual.StockActual < 0)
                {
                    pParametro.CodigoMensaje = "StockInsuficiente";
                    pParametro.CodigoMensajeArgs.Add(pParametro.Producto.Descripcion);
                    pParametro.CodigoMensajeArgs.Add(stockActual.StockActualOriginal.ToString());
                    return false;
                }

                if (!BaseDatos.ObtenerBaseDatos().Actualizar(stockActual, bd, tran, "CMPStockActualizar"))
                {
                    AyudaProgramacionLN.MapearError(stockActual, pParametro);
                    resultado = false;
                }
            }

            return resultado;
        }
        
        private CMPStock ObtenerDatosCompletos(CMPStock pParametro, Database bd, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CMPStock>("CMPStockSeleccionar",pParametro, bd, tran);
        }

        public CMPStock ObtenerDatosCompletosPorIdProductoIdFilial(CMPStock pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CMPStock>("CmpStockSeleccionarPorProductoFilial", pParametro);
        }

        public DataTable ObtenerGrilla(CMPStock pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CmpStockListarFiltro", pParametro);
        }

        public override List<CMPStock> ObtenerListaFiltro(CMPStock pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CMPStock>("CmpStockListarFiltro",pParametro);
        }

        public override CMPStock ObtenerDatosCompletos(CMPStock pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CMPStock>("CMPStockSeleccionar", pParametro);
        }
    }
}
