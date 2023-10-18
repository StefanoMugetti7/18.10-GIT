using Cargos.Entidades;
using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Turismo.Entidades;

namespace Turismo.LogicaNegocio
{
    public class PaquetesLN : BaseLN<TurPaquetes>
    {
        public override bool Agregar(TurPaquetes pParametro)
        {

            if (pParametro.IdPaquete > 0)
                return true;

            bool resultado = true;
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);


            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {

                    if (!this.Validaciones(pParametro))
                        return false;

                    if (resultado && !this.Agregar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    //if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                    //    resultado = false;
                    if (resultado)
                    {
                        foreach (TurPaquetesDetalles item in pParametro.Detalles)
                        {
                            item.IdPaquete = pParametro.IdPaquete;
                            item.Estado.IdEstado = (int)Estados.Activo;
                            item.UsuarioLogueado = pParametro.UsuarioLogueado;
                            item.IdPaqueteDetalle = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "TurPaquetesDetallesInsertar");
                            if (item.IdPaqueteDetalle == 0)
                            {
                                resultado = false;
                                break;
                            }
                            else
                            {
                                item.Campos.ForEach(x => x.CampoValor.EstadoColeccion = EstadoColecciones.Agregado);
                                if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(item, bd, tran))
                                {
                                    resultado = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdPaquete.ToString());
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

        internal bool Agregar(TurPaquetes pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdPaquete = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "TurPaquetesInsertar");
            if (pParametro.IdPaquete == 0)
                return false;

            return true;
        }

        private bool Validaciones(TurPaquetes pParametro)
        {
            return true;
        }
        private bool Existe(TurPaquetesDetalles pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "TurPaquetesDetallesValidarExistente");
        }

        public override bool Modificar(TurPaquetes pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            TurPaquetes valorViejo = new TurPaquetes();
            valorViejo.IdPaquete = pParametro.IdPaquete;
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
                    resultado = Validaciones(pParametro);

                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TurPaquetesActualizar"))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    if (pParametro.Detalles.Count == 0 && valorViejo.Detalles.Count > 0)
                    {
                        if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TurPaquetesLiberarDetalles"))
                            resultado = false;
                    }
                    else
                    {
                        foreach (TurPaquetesDetalles item in pParametro.Detalles)
                        {
                            item.IdPaquete = pParametro.IdPaquete;
                            item.UsuarioLogueado = pParametro.UsuarioLogueado;
                            if (!Existe(item))
                            {
                                item.IdPaqueteDetalle = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "TurPaquetesDetallesInsertar");
                                if (item.IdPaqueteDetalle == 0)
                                {
                                    resultado = false;
                                    break;
                                }
                                else
                                {
                                    item.Campos.ForEach(x => x.CampoValor.EstadoColeccion = EstadoColecciones.Agregado);
                                }
                            }
                            else
                            {
                                if (item.EstadoColeccion == EstadoColecciones.Borrado)
                                {
                                    item.Estado.IdEstado = (int)Estados.Baja;
                                }
                                if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "TurPaquetesDetallesActualizar"))
                                {
                                    resultado = false;
                                    break;
                                }
                                else
                                {
                                    foreach (TGECampos item2 in item.Campos)
                                    {
                                        TGECamposValores aux = new TGECamposValores();
                                        item.Filtro = item.IdPaqueteDetalle.ToString() + "," + item2.IdCampo.ToString();
                                        aux = BaseDatos.ObtenerBaseDatos().Obtener<TGECamposValores>("TurPaquetesDetallesObtenerValoresDetalles", item);
                                        item2.CampoValor.IdCampoValor = aux.IdCampoValor;
                                    }
                                }
                            }
                            if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(item, bd, tran))
                            {
                                resultado = false;
                                break;
                            }
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

        public override TurPaquetes ObtenerDatosCompletos(TurPaquetes pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<TurPaquetes>("TurPaquetesSeleccionar", pParametro);
            pParametro.Detalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<TurPaquetesDetalles>("TurPaquetesDetallesSeleccionar", pParametro);
            foreach (TurPaquetesDetalles item in pParametro.Detalles)
            {
                item.IdPaquete = pParametro.IdPaquete;
                item.Campos = TGEGeneralesF.CamposObtenerListaFiltro(item, item.Producto, item.Producto.IdProducto);
                foreach (TGECampos item2 in item.Campos)
                {
                    item.Filtro = item.IdPaqueteDetalle.ToString() + "," + item2.IdCampo.ToString();
                    item2.CampoValor = BaseDatos.ObtenerBaseDatos().Obtener<TGECamposValores>("TurPaquetesDetallesObtenerValoresDetalles", item);
                }
            }
            return pParametro;
        }
        public override List<TurPaquetes> ObtenerListaFiltro(TurPaquetes pParametro)
        {
            throw new NotImplementedException();
        }
        public List<CarTiposCargos> ObtenerTiposCargos(TurPaquetes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CarTiposCargos>("TurPaquetesObtenerCargos", pParametro);
        }
        public DataSet ObtenerReservasDetallesDesdeXML(TGECampos pCampo)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerDataSet("TurPaquetesDetalleObtenerDesdeXML", pCampo);
        }
        public DataTable ObtenerListaGrilla(TurPaquetes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("[TurPaquetesSeleccionarDescripcionPorFiltro]", pParametro);
        }

        public List<TurPaquetesDetalles> ObtenerPaquetes(TurPaquetes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TurPaquetesDetalles>("TurPaquetesObtenerDetalles", pParametro);
        }

        public DataSet ObtenerReservasDetallesDesdeXMLATurismo(TGECampos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerDataSet("TurPaquetesDetalleObtenerDesdeXMLATurismo", pParametro);
        }
    }
}
