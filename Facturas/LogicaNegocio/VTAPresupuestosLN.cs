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
    class VTAPresupuestosLN : BaseLN<VTAPresupuestos>
    {
        public DataTable ObtenerListaGrilla(VTAPresupuestos pPresupuesto)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("VTAPresupuestosSeleccionarGrilla", pPresupuesto);
        }

        public override List<VTAPresupuestos> ObtenerListaFiltro(VTAPresupuestos pPresupuesto)
        {
            return new List<VTAPresupuestos>();
        }

        public override VTAPresupuestos ObtenerDatosCompletos(VTAPresupuestos pPresupuesto)
        {
            VTAPresupuestos presupuesto = BaseDatos.ObtenerBaseDatos().Obtener<VTAPresupuestos>("VTAPresupuestosSeleccionar", pPresupuesto);
            presupuesto.PresupuestosDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<VTAPresupuestosDetalles>("VTAPresupuestosDetallesSeleccionarPorIdPresupuesto", pPresupuesto);
            return presupuesto;
        }

        public VTAPresupuestos ObtenerArchivo(VTAPresupuestos pPresupuesto)
        {
            this.GenerarActualizarPDF(pPresupuesto);
            return pPresupuesto;
            //VTAPresupuestos presupuesto = BaseDatos.ObtenerBaseDatos().Obtener<VTAPresupuestos>("VTAPresupuestosSeleccionarArchivo", pPresupuesto);
            //if (presupuesto.PresupuestoPDF == null)
            //{
            //    DbTransaction tran;
            //    DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();
            //    using (DbConnection con = bd.CreateConnection())
            //    {
            //        con.Open();
            //        tran = con.BeginTransaction();
            //        try
            //        {
            //            bool resultado = this.GenerarActualizarPDF(pPresupuesto, bd, tran);
            //            if (resultado)
            //            {
            //                presupuesto.PresupuestoPDF = pPresupuesto.PresupuestoPDF;
            //                tran.Commit();
            //            }
            //            else
            //            {
            //                tran.Rollback();
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            ExceptionHandler.HandleException(ex, "LogicaNegocio");
            //            tran.Rollback();
            //            pPresupuesto.CodigoMensaje = "ResultadoTransaccionIncorrecto";
            //            pPresupuesto.CodigoMensajeArgs.Add(ex.Message);
            //        }
            //    }
            //}
            //return presupuesto;
        }

        public override bool Agregar(VTAPresupuestos pParametro)
        {
            if (pParametro.IdPresupuesto > 0)
                return true;

            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.Estado.IdEstado = (int)EstadosFacturas.Activo;
            pParametro.FechaAlta = DateTime.Now;

            if (!this.Validar(pParametro, new VTAPresupuestos()))
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

                    if (resultado && !this.ItemsActualizar(pParametro, new VTAPresupuestos(), bd, tran))
                        resultado = false;

                    //if (resultado && !this.GenerarActualizarPDF(pParametro, bd, tran))
                    //    resultadoPDF = false;

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
                    resultado = false;
                }
                finally
                {
                    if (!resultado)
                        pParametro.IdPresupuesto = 0;
                }

            }
            return resultado;
        }

        /// <summary>
        /// Genera el PDF y lo Guarda en la BD
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        private bool GenerarActualizarPDF(VTAPresupuestos pParametro)
        {
            #region Generao y Guardo el Comprobante
            bool resultadoPDF = true;
            pParametro.PresupuestoPDF = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.VTAPresupuestos, "VTAPresupuestos", pParametro, pParametro.UsuarioLogueado);
            return resultadoPDF;
            #endregion
            //#region Generao y Guardo el Comprobante
            //bool resultadoPDF = true;

            //TGEComprobantes comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(EnumTGEComprobantes.VTAPresupuestos);
            //string archivoReporteLeer = string.Concat(System.AppDomain.CurrentDomain.BaseDirectory, comprobante.NombreRPT.Replace('/', '\\'));

            ////DataSet ds = ReportesF.ReportesObtenerDatos(pReporte);
            //DbCommand dbCommand = bd.GetStoredProcCommand(comprobante.NombreSP);
            //bd.DiscoverParameters(dbCommand);
            //Mapeador.MapearEntidadParametros(pParametro, dbCommand);
            //DataSet dataSet = bd.ExecuteDataSet(dbCommand, tran);

            //CrystalReportSource CryReportSource = new CrystalReportSource();
            //CryReportSource.CacheDuration = 1;
            //CryReportSource.Report.FileName = archivoReporteLeer;
            //CryReportSource.ReportDocument.SetDataSource(dataSet);
            //Stream presupuesto = CryReportSource.ReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
            //pParametro.PresupuestoPDF = AyudaProgramacionLN.StreamToByteArray(presupuesto);

            //if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "VTAPresupuestosInsertarPDF"))
            //    resultadoPDF = false;

            //return resultadoPDF;
            //#endregion
        }

        internal bool Agregar(VTAPresupuestos pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdPresupuesto = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "VTAPresupuestosInsertar");
            if (pParametro.IdPresupuesto == 0)
                return false;

            return true;
        }

        private bool Validar(VTAPresupuestos pParametro, VTAPresupuestos pValorViejo)
        {
            VTAPresupuestos presupuesto = new VTAPresupuestos();

            switch (pParametro.EstadoColeccion)
            {
                case EstadoColecciones.SinCambio:
                    break;
                case EstadoColecciones.Agregado:
                    if (pParametro.PresupuestosDetalles.Count == 0)
                    {
                        pParametro.CodigoMensaje = "ValidarItemsPresupuesto";
                        return false;
                    }
                    if (pParametro.PresupuestosDetalles.Exists(x=>x.Cantidad<=0 || x.Cantidad==null))
                    {
                        pParametro.CodigoMensaje = "ValidarItemsPresupuestoCantidad";
                        return false;
                    }
                    if (pParametro.PresupuestosDetalles.Exists(x => x.PrecioUnitarioSinIva <= 0 || x.PrecioUnitarioSinIva==null))
                    {
                        pParametro.CodigoMensaje = "ValidarItemsPresupuestoPrecio";
                        return false;
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

        //public bool ActualizarPDF(VTAPresupuestos pParametro)
        //{
        //    bool resultado = true;
        //    DbTransaction tran;
        //    DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

        //    using (DbConnection con = bd.CreateConnection())
        //    {
        //        con.Open();
        //        tran = con.BeginTransaction();

        //        try
        //        {
        //            resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "VTAPresupuestosInsertarPDF");
        //            if (resultado)
        //            {
        //                tran.Commit();
        //                pParametro.CodigoMensaje = "ResultadoTransaccion";
        //            }
        //            else
        //            {
        //                tran.Rollback();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ExceptionHandler.HandleException(ex, "LogicaNegocio");
        //            tran.Rollback();
        //            pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
        //            pParametro.CodigoMensajeArgs.Add(ex.Message);
        //            return false;
        //        }
        //    }
        //    return resultado;
        //}

        public override bool Modificar(VTAPresupuestos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            if (!this.Validar(pParametro, new VTAPresupuestos()))
                return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            VTAPresupuestos valorViejo = new VTAPresupuestos();
            valorViejo.IdPresupuesto = pParametro.IdPresupuesto;
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
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "VTAPresupuestosActualizar");

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

        public bool ModificarEstado(VTAPresupuestos pParametro, Database db, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, db, tran, "VTAPresupuestosActualizarEstado"))
                return false;

            return true;
        }

        #region "Items Presupuestos"

        private bool ItemsActualizar(VTAPresupuestos pParametro, VTAPresupuestos pValorViejo, Database bd, DbTransaction tran)
        {
            foreach (VTAPresupuestosDetalles item in pParametro.PresupuestosDetalles)
            {
                switch (item.EstadoColeccion)
                {
                    #region Agregado
                    case EstadoColecciones.Agregado:
                        item.IdPresupuesto = pParametro.IdPresupuesto;
                        //if (item.Descripcion.Length > 0)
                        //    item.Descripcion = string.Concat(item.DescripcionProducto, " - ", item.Descripcion);
                        //else
                        //    item.Descripcion = item.DescripcionProducto;
                        
                        item.IdPresupuestoDetalle = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "VTAPresupuestosDetallesInsertar");
                        if (item.IdPresupuestoDetalle == 0)
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
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "VTAPresupuestosDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.PresupuestosDetalles.Find(x => x.IdPresupuestoDetalle== item.IdPresupuestoDetalle), Acciones.Update, item, bd, tran))
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

