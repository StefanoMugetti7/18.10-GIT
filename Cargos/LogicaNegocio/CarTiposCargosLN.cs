using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Servicio.AccesoDatos;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Cargos.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Auditoria.Entidades;
using System.Collections;
using Auditoria;
using Comunes.LogicaNegocio;
using Afiliados.Entidades;

namespace Cargos.LogicaNegocio
{
    class CarTiposCargosLN : BaseLN<CarTiposCargos>
    {

        public override CarTiposCargos ObtenerDatosCompletos(CarTiposCargos pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CarTiposCargos>("CarTiposCargoSeleccionarDescripcion", pParametro);
            pParametro.TiposCargosCategorias = BaseDatos.ObtenerBaseDatos().ObtenerLista<CarTiposCargosCategorias>("CarTiposCargosCategoriasSeleccionarPorCargos", pParametro);
            pParametro.TiposCargosRangos = BaseDatos.ObtenerBaseDatos().ObtenerLista<CarTiposCargosRangos>("CarTiposCargosRangosSeleccionarPorCargos", pParametro);
            pParametro.FormasCobros = BaseDatos.ObtenerBaseDatos().ObtenerLista<TGEFormasCobros>("CarTiposCargosFormasCobrosSeleccionarPorCargos", pParametro);
            pParametro.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return pParametro;
        }

        /// <summary>
        /// Devuelve una lista de Tipos de Cargos Filtrada
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public override List<CarTiposCargos> ObtenerListaFiltro(CarTiposCargos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CarTiposCargos>("CarTiposCargosSeleccionarFiltro", pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Tipos de Cargos en estado Activo
        /// pendiente de Configurar para el Afiliado
        /// </summary>
        /// <param name="pParametro">IdAfiliado</param>
        /// <returns></returns>
        public List<CarTiposCargos> ObtenerListaActiva(CarTiposCargosAfiliadosFormasCobros pParametro)
        {
            pParametro.Estado.IdEstado = (int)Estados.Activo;
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CarTiposCargos>("CarTiposCargosSeleccionarFiltro", pParametro);
        }

        public List<CarTiposCargos> ObtenerFacturacionAnticipadaCombo(AfiAfiliados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CarTiposCargos>("CarTiposCargosFacturacionAnticipadaCombo", pParametro);
        }

        public List<AfiCategorias> ObtenerListaCategoriasSinAsociar(CarTiposCargos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiCategorias>("AfiCategoriasFaltantesPorTiposCargos", pParametro);
        }

        public override bool Agregar(CarTiposCargos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            if (!this.Validar(pParametro))
                return false;

            pParametro.FechaAlta = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdTipoCargo = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CarTiposCargosInsertar");
                    if (pParametro.IdTipoCargo == 0)
                        resultado = false;

                    if (resultado && !this.GuardarCargosFormasCobros(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.ActualizarCategorias(pParametro, new CarTiposCargos(), bd, tran))
                        resultado = false;

                    if (resultado && !this.ActualizarRangos(pParametro, new CarTiposCargos(), bd, tran))
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
                    pParametro.CodigoMensaje = ex.Message;
                    return false;
                }
            }
            return resultado;
        }

        private bool Validar(CarTiposCargos pParametro)
        {
            //if (pParametro.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.Bonificacion)
            //{
            //    if (pParametro.PermiteCuotas == true)
            //    {
            //        pParametro.CodigoMensaje = "ValidarProcesoBonificacionPermiteCuotas";
            //        return false;
            //    }
            //}
            return true;
        }

        public override bool Modificar(CarTiposCargos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            if (!this.Validar(pParametro))
                return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CarTiposCargos valorViejo = new CarTiposCargos();
            valorViejo.IdTipoCargo = pParametro.IdTipoCargo;
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
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CarTiposCargosActualizar"))
                        resultado = false;

                    if (resultado && resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.GuardarCargosFormasCobros(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.ActualizarCategorias(pParametro, valorViejo, bd, tran))
                        resultado = false;

                    if (resultado && !this.ActualizarRangos(pParametro, new CarTiposCargos(), bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    if (pParametro.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.Administrable)
                    {
                        //Actualizo los Importes en CarTiposCargosAfiliadosFormasCobros cuando se modifica el Importe Categoria
                        CarTiposCargosCategorias datos;
                        if (resultado)
                        {
                            foreach (CarTiposCargosCategorias cargoCate in pParametro.TiposCargosCategorias.Where(x => x.ImporteModificado).ToList())
                            {
                                datos = new CarTiposCargosCategorias();
                                datos.IdTipoCargo = pParametro.IdTipoCargo;
                                datos.Categoria.IdCategoria = cargoCate.Categoria.IdCategoria;
                                cargoCate.ImporteOriginal = cargoCate.Importe;
                                datos.Importe = cargoCate.ImporteOriginal;
                                if (!BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(datos, bd, tran, "CarTiposCargosAfiliadosFormasCobrosActualizarProceso"))
                                {
                                    resultado = false;
                                    AyudaProgramacionLN.MapearError(datos, pParametro);
                                    break;
                                }
                            }
                        }
                        //Actualizo los Importes en CarTiposCargosAfiliadosFormasCobros cuando se da de baja una categoria
                        if (resultado)
                        {
                            foreach (CarTiposCargosCategorias cargoCate in pParametro.TiposCargosCategorias.Where(x => x.EstadoColeccion == EstadoColecciones.Modificado && x.Estado.IdEstado == (int)Estados.Baja).ToList())
                            {
                                datos = new CarTiposCargosCategorias();
                                datos.IdTipoCargo = pParametro.IdTipoCargo;
                                datos.Categoria.IdCategoria = cargoCate.Categoria.IdCategoria;
                                cargoCate.ImporteOriginal = cargoCate.Importe;
                                datos.Importe = cargoCate.ImporteOriginal;
                                if (!BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(datos, bd, tran, "CarTiposCargosAfiliadosFormasCobrosActualizarProceso"))
                                {
                                    resultado = false;
                                    AyudaProgramacionLN.MapearError(datos, pParametro);
                                    break;
                                }
                            }
                        }
                        //Actualizo los Importes en CarTiposCargosAfiliadosFormasCobros cuando se da de Alta una categoria que estaba en Baja
                        if (resultado)
                        {
                            foreach (CarTiposCargosCategorias cargoCate in pParametro.TiposCargosCategorias.Where(x => x.EstadoColeccion == EstadoColecciones.Modificado && x.Estado.IdEstado == (int)Estados.Activo
                                && (valorViejo.TiposCargosCategorias.Exists(y => y.IdTipoCargoCategoria == x.IdTipoCargoCategoria && y.Estado.IdEstado == (int)Estados.Baja))).ToList())
                            {
                                datos = new CarTiposCargosCategorias();
                                datos.IdTipoCargo = pParametro.IdTipoCargo;
                                datos.Categoria.IdCategoria = cargoCate.Categoria.IdCategoria;
                                cargoCate.ImporteOriginal = cargoCate.Importe;
                                datos.Importe = pParametro.ImporteOriginal;
                                if (!BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(datos, bd, tran, "CarTiposCargosAfiliadosFormasCobrosActualizarProceso"))
                                {
                                    resultado = false;
                                    AyudaProgramacionLN.MapearError(datos, pParametro);
                                    break;
                                }
                            }
                        }
                        //Actualizo los Importes en CarTiposCargosAfiliadosFormasCobros cuando no existen Importes por Categorias
                        if (resultado && pParametro.ImporteModificado)
                        {
                            datos = new CarTiposCargosCategorias();
                            datos.IdTipoCargo = pParametro.IdTipoCargo;
                            datos.Categoria.IdCategoria = 0;
                            pParametro.ImporteOriginal = pParametro.Importe;
                            datos.Importe = pParametro.ImporteOriginal;
                            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(pParametro, bd, tran, "CarTiposCargosAfiliadosFormasCobrosActualizarProceso"))
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
                    pParametro.CodigoMensaje = ex.Message;
                    return false;
                }
            }
            return resultado;
        }

        private bool GuardarCargosFormasCobros(CarTiposCargos pTipoCargo, Database pBd, DbTransaction pTran)
        {
            //bool resultado = true;
            string sp = string.Empty;
            Hashtable param = new Hashtable();
            HistorialCambios cambio = new HistorialCambios();
            cambio.CampoCambiado = "TipoCargo -> FormaCobro";
            foreach (TGEFormasCobros fp in pTipoCargo.FormasCobros)
            {
                param = new Hashtable();
                param.Add("IdTipoCargo", pTipoCargo.IdTipoCargo);
                param.Add("IdFormaCobro", fp.IdFormaCobro);
                cambio.ValorViejo = string.Empty;
                cambio.ValorNuevo = string.Empty;

                switch (fp.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        sp = "CarTiposCargosFormasCobrosInsertar";
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(param, pBd, pTran, sp))
                            return false;
                        cambio.ValorNuevo = fp.FormaCobro;
                        if (!AuditoriaF.AuditoriaAgregar(pTipoCargo, cambio, Acciones.Insert, pBd, pTran))
                            return false;
                        break;
                    case EstadoColecciones.Borrado:
                        sp = "CarTiposCargosFormasCobrosBorrar";
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(param, pBd, pTran, sp))
                            return false;
                        cambio.ValorViejo = fp.FormaCobro;
                        if (!AuditoriaF.AuditoriaAgregar(pTipoCargo, cambio, Acciones.Delete, pBd, pTran))
                            return false;
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        private bool ActualizarCategorias(CarTiposCargos pTipoCargo, CarTiposCargos pValorViejo, Database pBd, DbTransaction pTran)
        {
            string sp = string.Empty;
            foreach (CarTiposCargosCategorias categoria in pTipoCargo.TiposCargosCategorias)
            {
                switch (categoria.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        categoria.IdTipoCargo = pTipoCargo.IdTipoCargo;
                        categoria.Estado.IdEstado = (int)Estados.Activo;
                        categoria.FechaAlta = DateTime.Now;
                        categoria.UsuarioAlta.IdUsuarioAlta = pTipoCargo.UsuarioLogueado.IdUsuarioEvento;
                        categoria.UsuarioLogueado = pTipoCargo.UsuarioLogueado;
                        categoria.FechaVigenciaDesde = DateTime.Now;

                        sp = "CarTiposCargosCategoriasInsertar";
                        categoria.IdTipoCargoCategoria = BaseDatos.ObtenerBaseDatos().Agregar(categoria, pBd, pTran, sp);
                        if (categoria.IdTipoCargoCategoria == 0)
                        {
                            AyudaProgramacionLN.MapearError(categoria, pTipoCargo);
                            return false;
                        }
                        //if (!AuditoriaF.AuditoriaAgregar(categoria, cambio, Acciones.Insert, pBd, pTran))
                        //{
                        //    AyudaProgramacionLN.MapearError(categoria, pTipoCargo);
                        //    return false;
                        //}
                        break;
                    case EstadoColecciones.Modificado:
                        //categoria.Estado.IdEstado = (int)Estados.Baja;
                        categoria.FechaEvento = DateTime.Now;
                        categoria.UsuarioLogueado = pTipoCargo.UsuarioLogueado;

                        sp = "CarTiposCargosCategoriasActualizar";
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(categoria, pBd, pTran, sp))
                        {
                            AyudaProgramacionLN.MapearError(categoria, pTipoCargo);
                            return false;
                        }

                        if (!AuditoriaF.AuditoriaAgregar(pValorViejo.TiposCargosCategorias.Find(X => X.IdTipoCargoCategoria == categoria.IdTipoCargoCategoria), Acciones.Update, categoria, pBd, pTran))
                        {
                            AyudaProgramacionLN.MapearError(categoria, pTipoCargo);
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        private bool ActualizarRangos(CarTiposCargos pTipoCargo, CarTiposCargos pValorViejo, Database pBd, DbTransaction pTran)
        {
            string sp = string.Empty;
            foreach (CarTiposCargosRangos rango in pTipoCargo.TiposCargosRangos)
            {
                switch (rango.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        rango.IdTipoCargo = pTipoCargo.IdTipoCargo;
                        rango.Estado.IdEstado = (int)Estados.Activo;
                        rango.FechaAlta = DateTime.Now;
                        rango.IdUsuarioAlta = pTipoCargo.UsuarioLogueado.IdUsuarioEvento;
                        rango.UsuarioLogueado = pTipoCargo.UsuarioLogueado;
                        //categoria.FechaVigenciaDesde = DateTime.Now;

                        sp = "CarTiposCargosRangosInsertar";
                        rango.IdTipoCargoRango = BaseDatos.ObtenerBaseDatos().Agregar(rango, pBd, pTran, sp);
                        if (rango.IdTipoCargoRango == 0)
                        {
                            AyudaProgramacionLN.MapearError(rango, pTipoCargo);
                            return false;
                        }
                        //if (!AuditoriaF.AuditoriaAgregar(categoria, cambio, Acciones.Insert, pBd, pTran))
                        //{
                        //    AyudaProgramacionLN.MapearError(categoria, pTipoCargo);
                        //    return false;
                        //}
                        break;
                    case EstadoColecciones.Modificado:
                        //categoria.Estado.IdEstado = (int)Estados.Baja;
                        rango.FechaEvento = DateTime.Now;
                        rango.UsuarioLogueado = pTipoCargo.UsuarioLogueado;

                        sp = "CarTiposCargosRangosActualizar";
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(rango, pBd, pTran, sp))
                        {
                            AyudaProgramacionLN.MapearError(rango, pTipoCargo);
                            return false;
                        }

                        if (!AuditoriaF.AuditoriaAgregar(pValorViejo.TiposCargosRangos.Find(X => X.IdTipoCargoRango == rango.IdTipoCargoRango), Acciones.Update, rango, pBd, pTran))
                        {
                            AyudaProgramacionLN.MapearError(rango, pTipoCargo);
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

    }
}
