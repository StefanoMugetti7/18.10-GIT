using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Comunes.Entidades;
using Servicio.AccesoDatos;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Generales.FachadaNegocio;
using Auditoria;
using System.Collections;
using Compras.Entidades;
using Comunes.LogicaNegocio;
using Auditoria.Entidades;
using Afiliados.Entidades;
using System.Data;
using Generales.Entidades;

namespace Compras.LogicaNegocio
{
    class CMPListasPreciosLN : BaseLN<CMPListasPrecios>
    {

        /// <summary>
        /// Obtiene los datos completos por ID
        /// </summary>
        /// <param name="pParametro">IdListaPrecio</param>
        /// <returns>la lista completa, con el Id pasado por parametro</returns>
        public override CMPListasPrecios ObtenerDatosCompletos(CMPListasPrecios pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CMPListasPrecios>("CMPListasPreciosSeleccionar", pParametro);
            pParametro.DataTableListasPreciosDetalle = ListasPreciosDetallesSeleccionar(pParametro);
            //pParametro.ListaPrecioDetalle = BaseDatos.ObtenerBaseDatos().ObtenerLista<CMPListasPreciosDetalles>("CMPListasPreciosDetallesSeleccionarPorIdListaPrecio", pParametro);
            pParametro.Afiliados = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiAfiliados>("CMPListasPreciosAfiliadosSeleccionarPorIdListaPrecio", pParametro);
            pParametro.Filiales = BaseDatos.ObtenerBaseDatos().ObtenerLista<TGEFiliales>("CMPListasPreciosFilialesSeleccionarPorIdListaPrecio", pParametro);
            return pParametro;
        }

        public DataTable ObtenerPlantilla(CMPListasPrecios pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CMPListasPreciosDetallesSeleccionarPlantilla", pParametro);
        }

        public CMPListasPrecios ObtenerDatosCompletosPopUp(CMPListasPrecios pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CMPListasPrecios>("CMPListasPreciosSeleccionar", pParametro);
            //pParametro.ListaPrecioDetalle = BaseDatos.ObtenerBaseDatos().ObtenerLista<CMPListasPreciosDetalles>("CMPListasPrecioDetallesSeleccionarPorIdListaPrecioPopUp", pParametro);
            pParametro.DataTableListasPreciosDetalle = ListasPreciosDetallesSeleccionar(pParametro);
            return pParametro;
        }

        public override List<CMPListasPrecios> ObtenerListaFiltro(CMPListasPrecios pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CMPListasPrecios>("CMPListasPreciosSeleccionarPorFiltro", pParametro);
        }

        public override bool Agregar(CMPListasPrecios pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            if(!this.Validar(pParametro, new CMPListasPrecios()))
                return false;

            pParametro.DataTableListasPreciosDetalle.TableName = "ListasPreciosDetalles";
            pParametro.LoteListasPreciosDetalles = pParametro.DataTableListasPreciosDetalle.ToXmlDocument();
            
            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            pParametro.FechaAlta = DateTime.Now;

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {

                    pParametro.IdListaPrecio = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CMPListasPreciosInsertar");
                    if (pParametro.IdListaPrecio == 0)
                        resultado = false;

                    //if (resultado && !this.ActualizarDetalles(pParametro, new CMPListasPrecios(), bd, tran))
                    //    resultado = false;

                    if (resultado && !this.ActualizarAfiliados(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.ActualizarFiliales(pParametro, bd, tran))
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


                return resultado;
            }
        }

        public bool Anular(CMPListasPrecios pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            CMPListasPrecios valorViejo = new CMPListasPrecios();
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo.IdListaPrecio = pParametro.IdListaPrecio;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            pParametro.EstadoColeccion = EstadoColecciones.Borrado;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();
            
            pParametro.FechaEvento = DateTime.Now;
            pParametro.Estado.IdEstado = (int)EstadosListasPrecios.Baja;
            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {

                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CMPListasPreciosActualizar"))
                        resultado = false;

                    if (resultado && !this.ActualizarDetalles(pParametro, new CMPListasPrecios(), bd, tran))
                        resultado = false;

                    if (resultado && !this.ActualizarAfiliados(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.ActualizarFiliales(pParametro, bd, tran))
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


                return resultado;
            }
        }

        private bool ActualizarDetalles(CMPListasPrecios pParametro, CMPListasPrecios pValorViejo, Database bd, DbTransaction tran)
        {

            foreach (CMPListasPreciosDetalles Detalle in pParametro.ListaPrecioDetalle.Where(x => x.Producto.IdProducto > 0))
            {
                switch (Detalle.EstadoColeccion)
                {
                    #region "Agregado"
                    case EstadoColecciones.Agregado:
                        
                        //Detalle.Estado.IdEstado = (int)EstadosListasPrecios.Activo;
                        Detalle.ListaPrecio.IdListaPrecio = pParametro.IdListaPrecio;
                        Detalle.IdListaPrecioDetalle = BaseDatos.ObtenerBaseDatos().Agregar(Detalle, bd, tran, "CMPListasPreciosDetallesInsertar");
                        if (Detalle.IdListaPrecioDetalle == 0)
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }
                        break;
                    #endregion

                    #region "Modificado"
                    case EstadoColecciones.Modificado:
                        Detalle.UsuarioLogueado = pParametro.UsuarioLogueado;
                        
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(Detalle, bd, tran, "CMPListasPreciosDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.ListaPrecioDetalle.Find(x => x.IdListaPrecioDetalle == Detalle.IdListaPrecioDetalle), Acciones.Update, Detalle, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }

                        break;
                    #endregion

                    #region "Anulado"
                    case EstadoColecciones.Borrado:
                        Detalle.UsuarioLogueado = pParametro.UsuarioLogueado;
                        Detalle.Estado.IdEstado = (int)EstadosListasPrecios.Baja;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(Detalle, bd, tran, "CMPListasPreciosDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.ListaPrecioDetalle.Find(x => x.IdListaPrecioDetalle == Detalle.IdListaPrecioDetalle), Acciones.Update, Detalle, bd, tran))
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

        private bool ActualizarAfiliados(CMPListasPrecios pParametro, Database pBd, DbTransaction pTran)
        {
            //bool resultado = true;
            string sp = string.Empty;
            Hashtable param = new Hashtable();
            HistorialCambios cambio = new HistorialCambios();
            cambio.CampoCambiado = "Lista de Precio -> Afiliado";
            foreach (AfiAfiliados to in pParametro.Afiliados)
            {
                param = new Hashtable();
                param.Add("IdListaPrecio", pParametro.IdListaPrecio);
                param.Add("IdAfiliado", to.IdAfiliado);
                cambio.ValorViejo = string.Empty;
                cambio.ValorNuevo = string.Empty;

                switch (to.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        sp = "CMPListasPreciosAfiliadosInsertar";
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(param, pBd, pTran, sp))
                            return false;
                        cambio.ValorNuevo = to.RazonSocial;
                        if (!AuditoriaF.AuditoriaAgregar(pParametro, cambio, Acciones.Insert, pBd, pTran))
                            return false;
                        break;
                    case EstadoColecciones.Borrado:
                        sp = "CMPListasPreciosAfiliadosBorrar";
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(param, pBd, pTran, sp))
                            return false;
                        cambio.ValorViejo = to.RazonSocial;
                        if (!AuditoriaF.AuditoriaAgregar(pParametro, cambio, Acciones.Delete, pBd, pTran))
                            return false;
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        private bool ActualizarFiliales(CMPListasPrecios pParametro, Database pBd, DbTransaction pTran)
        {
            //bool resultado = true;
            string sp = string.Empty;
            Hashtable param = new Hashtable();
            HistorialCambios cambio = new HistorialCambios();
            cambio.CampoCambiado = "Lista de Precio -> Filiales";
            foreach (TGEFiliales to in pParametro.Filiales)
            {
                param = new Hashtable();
                param.Add("IdListaPrecio", pParametro.IdListaPrecio);
                param.Add("IdFilial", to.IdFilial);
                cambio.ValorViejo = string.Empty;
                cambio.ValorNuevo = string.Empty;

                switch (to.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        sp = "CMPListasPreciosFilialesInsertar";
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(param, pBd, pTran, sp))
                            return false;
                        cambio.ValorNuevo = to.Filial;
                        if (!AuditoriaF.AuditoriaAgregar(pParametro, cambio, Acciones.Insert, pBd, pTran))
                            return false;
                        break;
                    case EstadoColecciones.Borrado:
                        sp = "CMPListasPreciosFilialesBorrar";
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(param, pBd, pTran, sp))
                            return false;
                        cambio.ValorViejo = to.Filial;
                        if (!AuditoriaF.AuditoriaAgregar(pParametro, cambio, Acciones.Delete, pBd, pTran))
                            return false;
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        public override bool Modificar(CMPListasPrecios pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            CMPListasPrecios valorViejo = new CMPListasPrecios();

            bool resultado = true;

            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            
            valorViejo.IdListaPrecio = pParametro.IdListaPrecio;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            if (!this.Validar(pParametro, valorViejo))
                return false;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            pParametro.DataTableListasPreciosDetalle.TableName = "ListasPreciosDetalles";
            pParametro.LoteListasPreciosDetalles = pParametro.DataTableListasPreciosDetalle.ToXmlDocument();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CMPListasPreciosActualizar"))
                        resultado = false;

                    //if (resultado && !this.ActualizarDetalles(pParametro, new CMPListasPrecios(), bd, tran))
                    //    resultado = false;

                    if (resultado && !this.ActualizarAfiliados(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.ActualizarFiliales(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        pParametro.ConfirmarAccion = false;
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

        public bool Validar(CMPListasPrecios pLista, CMPListasPrecios pValorViejo)
        {
            var duplicateExists = pLista.ListaPrecioDetalle.GroupBy(n => n.Producto.IdProducto).Where(g => g.Count() > 1).Select(x => x.Key);
            if (duplicateExists.ToList().Count > 0)
            {
                pLista.CodigoMensaje = "Los Codigo de Productos {0} se encuentran duplicados en la lista.";
                pLista.CodigoMensajeArgs.Add(string.Join(",", duplicateExists));
                return false;
            }

            switch (pLista.EstadoColeccion)
            {
                case EstadoColecciones.Agregado:
                    //if (pLista.ListaPrecioDetalle.Count(x => x.EstadoColeccion == EstadoColecciones.Agregado && x.Producto.IdProducto > 0 && x.Estado.IdEstado == (int)Estados.Activo) == 0)
                    //{
                    //    pLista.CodigoMensaje = "ValidarListaPrecioDetalle";
                    //    return false;
                    //}
                    foreach (CMPListasPreciosDetalles det in pLista.ListaPrecioDetalle)
                    {
                        if (det.Producto.IdProducto > 0 && det.Precio == 0 && !det.PrecioEditable && det.Estado.IdEstado == (int)Estados.Activo)
                        {
                            pLista.CodigoMensaje = "ValidarPrecioListaPrecio";
                            pLista.CodigoMensajeArgs.Add(det.Producto.Descripcion);
                            return false;
                        }
                    }

                    break;
                case EstadoColecciones.AgregadoBorradoMemoria:
                    break;
                case EstadoColecciones.Borrado:
                    break;
                case EstadoColecciones.Modificado:

                    //if (pLista.FechaInicioVigencia != pValorViejo.FechaInicioVigencia)
                    //{
                    //    if (pLista.FechaInicioVigencia < DateTime.Now || pLista.FechaInicioVigencia <= this.ObtenerMaximaFechaListaActiva().FechaInicioVigencia)
                    //    {
                    //        pLista.CodigoMensaje = "ValidarFechaInicioVigencia";
                    //        return false;
                    //    }

                    //}

                    foreach (CMPListasPreciosDetalles det in pLista.ListaPrecioDetalle)
                    {
                        if (det.Producto.IdProducto > 0 && det.Precio == 0 && !det.PrecioEditable && det.Estado.IdEstado == (int)EstadosListasPrecios.Activo)
                        {
                            pLista.CodigoMensaje = "ValidarPrecioListaPrecio";
                            pLista.CodigoMensajeArgs.Add(det.Producto.Descripcion);
                            return false;
                        }
                    }
                    break;
                default:
                    break;
            }

            return true;
        }

        private CMPListasPrecios ObtenerMaximaFechaListaActiva()
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CMPListasPrecios>("CMPListasPreciosObtenerMaximaFechaInicioVigencia");
        }

        public bool ImportarFamiliasProductosValidaciones(CMPListasPrecios pParametro)
        {
            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CMPListasPreciosImportarFamiliasProductosValidaciones"))
            {
                if (pParametro.CodigoMensaje != "ErrorAccesoDatos")
                {
                    pParametro.CodigoMensaje = pParametro.CodigoMensaje.Replace("|", "<BR/>");
                }
                return false;
            }
            return true;
        }

        public DataTable ImportarFamiliasProductos(CMPListasPrecios pParametro)
        {
            int tiempoEspera = Convert.ToInt32(TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.DBTiempoEjecucionProcesosDatos).ParametroValor);
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CMPListasPreciosImportarFamiliasProductos", pParametro, tiempoEspera);
        }

        public DataTable ListasPreciosDetallesSeleccionar(CMPListasPrecios pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CMPListasPreciosDetallesSeleccionarPorIdListaPrecio", pParametro);
        }
        public DataTable CMPListasPreciosDetallesSeleccionarBuscarProducto(CMPListasPreciosDetalles pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CMPListasPreciosDetallesSeleccionarBuscarProducto", pParametro);
        }

        public List<CMPListasPrecios> CMPListasPreciosObtener(CMPListasPrecios pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CMPListasPrecios>("CMPListasPreciosObtenerLista", pParametro);
        }
    }
}
