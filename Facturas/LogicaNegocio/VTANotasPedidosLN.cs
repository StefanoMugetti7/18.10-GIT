using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Servicio.AccesoDatos;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Generales.FachadaNegocio;
using Auditoria;
using Comunes;
using Comunes.Entidades;
using Compras.LogicaNegocio;
using Facturas.Entidades;
using Comunes.LogicaNegocio;
using Auditoria.Entidades;
using System.IO;
using Generales.Entidades;
using System.Data;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using Reportes.FachadaNegocio;

namespace Facturas.LogicaNegocio
{
    class VTANotasPedidosLN : BaseLN<VTANotasPedidos>
    {
        public override List<VTANotasPedidos> ObtenerListaFiltro(VTANotasPedidos pNotaPedido)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<VTANotasPedidos>("VTANotasPedidosSeleccionarDescripcionPorFiltro", pNotaPedido);
        }

        public override VTANotasPedidos ObtenerDatosCompletos(VTANotasPedidos pNotaPedido)
        {
            VTANotasPedidos notaPedido = BaseDatos.ObtenerBaseDatos().Obtener<VTANotasPedidos>("VTANotasPedidosSeleccionar", pNotaPedido);
            notaPedido.NotasPedidosDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<VTANotasPedidosDetalles>("VTANotasPedidosDetallesSeleccionarPorIdNotaPedido", pNotaPedido);
            return notaPedido;
        }
        public VTANotasPedidos ObtenerArchivo(VTANotasPedidos pNotaPedido)
        {
            this.GenerarActualizarPDF(pNotaPedido);
            return pNotaPedido;

        }
        private bool GenerarActualizarPDF(VTANotasPedidos pParametro)
        {
            #region Generao y Guardo el Comprobante
            bool resultadoPDF = true;
            pParametro.NotaPedidoPDF = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.VTANotasPedidos, "VTANotasPedidos", pParametro, pParametro.UsuarioLogueado);
            return resultadoPDF;
            #endregion
     
        }
        public override bool Agregar(VTANotasPedidos pParametro)
        {
            FacturaElectronica.FacturaElectronica feLN = new FacturaElectronica.FacturaElectronica();
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            //bool resultadoFE = true;
            //bool resultadoPDF = true;

            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.FechaAlta = DateTime.Now;

            if (!this.Validar(pParametro, new VTANotasPedidos()))
                return false;

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

                    if (resultado && !this.ItemsActualizar(pParametro, new VTANotasPedidos(), bd, tran))
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


        internal bool Agregar(VTANotasPedidos pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdNotaPedido = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "VTANotasPedidosInsertar");
            if (pParametro.IdNotaPedido == 0)
                return false;

            return true;
        }

        private bool Validar(VTANotasPedidos pParametro, VTANotasPedidos pValorViejo)
        {
            VTANotasPedidos presupuesto = new VTANotasPedidos();

            switch (pParametro.EstadoColeccion)
            {
                case EstadoColecciones.SinCambio:
                    break;
                case EstadoColecciones.Agregado:
                    if (pParametro.NotasPedidosDetalles.Count == 0)
                    {
                        pParametro.CodigoMensaje = "ValidarItemsNotaPedido";
                        return false;
                    }
                    if (pParametro.NotasPedidosDetalles.Exists(x=>x.Cantidad<=0 || x.Cantidad==null))
                    {
                        pParametro.CodigoMensaje = "ValidarItemsNotaPedidoCantidad";
                        return false;
                    }
                    //if (pParametro.NotasPedidosDetalles.Exists(x => x.PrecioUnitarioSinIva <= 0 || x.PrecioUnitarioSinIva==null))
                    //{
                    //    pParametro.CodigoMensaje = "ValidarItemsNotaPedidoPrecio";
                    //    return false;
                    //}
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

        public override bool Modificar(VTANotasPedidos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            if (!this.Validar(pParametro, new VTANotasPedidos()))
                return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            VTANotasPedidos valorViejo = new VTANotasPedidos();
            valorViejo.IdNotaPedido = pParametro.IdNotaPedido;
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
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "VTANotasPedidosActualizar");

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.ItemsActualizar(pParametro, valorViejo, bd, tran))
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

        public bool ModificarEstado(VTANotasPedidos pParametro, Database db, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, db, tran, "VTANotasPedidosActualizarEstado"))
                return false;

            return true;
        }

        #region "Items Nota Pedido"

        private bool ItemsActualizar(VTANotasPedidos pParametro, VTANotasPedidos pValorViejo, Database bd, DbTransaction tran)
        {
            foreach (VTANotasPedidosDetalles item in pParametro.NotasPedidosDetalles)
            {
                switch (item.EstadoColeccion)
                {
                    #region Agregado
                    case EstadoColecciones.Agregado:
                        item.IdNotaPedido = pParametro.IdNotaPedido;
                        //if (item.Descripcion.Length > 0)
                        //    item.Descripcion = string.Concat(item.DescripcionProducto, " - ", item.Descripcion);
                        //else
                        //    item.Descripcion = item.DescripcionProducto;
                        
                        item.IdNotaPedidoDetalle = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "VTANotasPedidosDetallesInsertar");
                        if (item.IdNotaPedidoDetalle == 0)
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        break;
                    #endregion
                    case EstadoColecciones.Borrado:
                        break;
                    #region Modificado
                    case EstadoColecciones.Modificado:
                        item.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "VTANotasPedidosDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.NotasPedidosDetalles.Find(x => x.IdNotaPedidoDetalle== item.IdNotaPedidoDetalle), Acciones.Update, item, bd, tran))
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

        public DataTable ObtenerGrilla(VTANotasPedidos pNotasPedidos)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("VTANotasPedidosSeleccionarDescripcionPorFiltro", pNotasPedidos);
        }

        public List<VTANotasPedidosDetalles> ObtenerListaFiltroPopUp(VTANotasPedidos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<VTANotasPedidosDetalles>("VTANotasPedidosPopUpSeleccionarDescripcionPorFiltro", pParametro);
        }

        public bool Anular(VTANotasPedidos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            VTANotasPedidos valorViejo = new VTANotasPedidos();
            valorViejo.IdNotaPedido = pParametro.IdNotaPedido;
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
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "VTANotasPedidosActualizarEstado");

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    //if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    //if (resultado && !this.ItemsActualizar(pParametro, valorViejo, bd, tran))
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


        #endregion
    }
}

