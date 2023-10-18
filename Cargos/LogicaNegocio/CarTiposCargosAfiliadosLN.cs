using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Servicio.AccesoDatos;
using Comunes;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Cargos.Entidades;
using Comunes.LogicaNegocio;
using Auditoria;
using Auditoria.Entidades;
using ProcesosDatos;
using ProcesosDatos.Entidades;
using ProcesamientosDatos.Entidades;
using System.Data;


namespace Cargos.LogicaNegocio
{
    class CarTiposCargosAfiliadosLN : BaseLN<CarTiposCargosAfiliadosFormasCobros>
    {
        /// <summary>
        /// Devuelve un Cargo Afiliado
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public override CarTiposCargosAfiliadosFormasCobros ObtenerDatosCompletos(CarTiposCargosAfiliadosFormasCobros pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CarTiposCargosAfiliadosFormasCobros>("CarTiposCargosAfiliadosFormasCobrosSeleccionarDescripcion", pParametro);
            pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            pParametro.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            return pParametro;
        }

        /// <summary>
        /// Devuelve un Cargo Afiliado
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public CarTiposCargosAfiliadosFormasCobros ObtenerDatosCompletos(CarTiposCargosAfiliadosFormasCobros pParametro, Database db, DbTransaction tran)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CarTiposCargosAfiliadosFormasCobros>("CarTiposCargosAfiliadosFormasCobrosSeleccionarDescripcion", pParametro, db, tran);
            return pParametro;
        }

        /// <summary>
        /// Devuelve una lista de Cargos por Afiliado
        /// </summary>
        /// <param name="pAfiliado">IdAfiliado, IdEstado</param>
        /// <param name="CarTiposCargosAfiliadosFormasCobros">IdEstado</param>
        /// <returns></returns>
        public List<CarTiposCargosAfiliadosFormasCobros> ObtenerLista(CarTiposCargosAfiliadosFormasCobros pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CarTiposCargosAfiliadosFormasCobros>("[CarTiposCargosAfiliadosFormasCobrosSeleccionarPorAfiliado]", pParametro);
        }

        public List<CarTiposCargosAfiliadosFormasCobros> ObtenerABonificar(CarTiposCargosAfiliadosFormasCobros pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CarTiposCargosAfiliadosFormasCobros>("CarTiposCargosSeleccionarABonificar", pParametro);
        }

        public override List<CarTiposCargosAfiliadosFormasCobros> ObtenerListaFiltro(CarTiposCargosAfiliadosFormasCobros pParametro)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtiene la Fecha de Alta de un Cargo si ya esta generado los cargos para el periodo
        /// </summary>
        /// <param name="pFecha"></param>
        /// <returns></returns>
        private DateTime ObtenerFechaAlta(DateTime pFecha, Database bd, DbTransaction tran)
        {
            int periodo = Convert.ToInt32(string.Concat(pFecha.Year.ToString(), pFecha.Month.ToString().PadLeft(2,'0')));
            if (ProcesosDatosF.ProcesosProcesamientoValidarCargosGenerados(periodo, bd, tran))
            {
                pFecha = pFecha.AddMonths(1);
                pFecha = pFecha.AddDays(-pFecha.Day + 1);
            }
            return pFecha;
        }

        public DataTable ObtenerCuotasPendientes(CarTiposCargosAfiliadosFormasCobros pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("[CarTiposCargosAfiliadosFormasCobrosSeleccionarCuotasPendientes]", pParametro);
        }

        public DataTable ObtenerReservasTurismo(CarTiposCargosAfiliadosFormasCobros pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("[CarTiposCargosAfiliadosFormasCobrosSeleccionarReservasTurismo]", pParametro);
        }

        /// <summary>
        /// Agrega un Cargo Afiliado Forma Cobro en la BD
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public override bool Agregar(CarTiposCargosAfiliadosFormasCobros pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            if (pParametro.IdTipoCargoAfiliadoFormaCobro > 0)
                return true;

            if (!this.Validar(pParametro))
                return false;

            pParametro.NoModificarFechaAlta = true;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = this.Agregar(pParametro, bd, tran);

                    if (resultado && BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "TGEFormasCobrosCodigosEntidadesValidarTopeCargos"))
                    {
                        pParametro.Estado.IdEstado = (int)EstadosCargos.Pendiente;

                        CarTiposCargosAfiliadosFormasCobros cargoAfiliado = new CarTiposCargosAfiliadosFormasCobros();
                        cargoAfiliado.IdTipoCargoAfiliadoFormaCobro = pParametro.IdTipoCargoAfiliadoFormaCobro;
                        cargoAfiliado = this.ObtenerDatosCompletos(cargoAfiliado, bd, tran);
                        cargoAfiliado.Estado = TGEGeneralesF.TGEEstadosObtener(pParametro.Estado);
                        cargoAfiliado.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!this.Modificar(cargoAfiliado, pParametro, bd, tran))
                        {
                            resultado = false;
                            AyudaProgramacionLN.MapearError(cargoAfiliado, pParametro);
                        }
                    }

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        pParametro.IdTipoCargoAfiliadoFormaCobro = 0;
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.IdTipoCargoAfiliadoFormaCobro = 0;
                    pParametro.CodigoMensaje = ex.Message;
                    return false;
                }
            }
            return resultado;
        }

        private bool Validar(CarTiposCargosAfiliadosFormasCobros pParametro)
        {
            int periodo = Convert.ToInt32(string.Concat(pParametro.FechaAlta.Year.ToString(), pParametro.FechaAlta.Month.ToString().PadLeft(2,'0')));
            List<int> lstPeriodos = ProcesosDatosF.ProcesosProcesamientoObtenerProximosPeriodosCargos();
            if (pParametro.TipoCargo.TipoCargoProceso.IdTipoCargoProceso != (int)EnumTiposCargosProcesos.Turismo && !lstPeriodos.Exists(x => x == periodo))
            {
                pParametro.CodigoMensaje = "ValidarFechaAltaPeriodoFacturado";
                SisProcesosProcesamiento procesamiento = new SisProcesosProcesamiento();
                procesamiento.Proceso.IdProceso = (int)EnumSisProcesos.GeneracionCargos;
                procesamiento = ProcesosDatosF.ProcesosProcesamientoObtenerUltimoPeriodoProcesado(procesamiento);
                pParametro.CodigoMensajeArgs.Add(procesamiento.Periodo.ToString());
                return false;
            }

            if (pParametro.ImporteTotal == 0 && 
                !(pParametro.TipoCargo.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.AdministrablePorcentaje
                    || pParametro.TipoCargo.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.Informativo
                    || (pParametro.Porcentaje.HasValue && pParametro.Porcentaje.Value>0)
                    )
                )
            {
                pParametro.CodigoMensaje = "ValidarImporte";
                return false;
            }

            if(pParametro.Porcentaje < 1 && pParametro.TipoCargo.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.AdministrablePorcentaje)
            {
                pParametro.CodigoMensaje = "ValidarPorcentajeMayorCero";
                return false;
            }    
            
            if (pParametro.Porcentaje.HasValue && pParametro.Porcentaje.Value > 100)
            {
                pParametro.CodigoMensaje = "ValidarPorcentaje";
                return false;
            }

            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CarTiposCargosAfiliadosFormasCobrosValidaciones"))
            {
                return false;
            }

            return true;
        }

        public bool Agregar(CarTiposCargosAfiliadosFormasCobros pParametro, Database bd, DbTransaction tran)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CarTiposCargosAfiliadosFormasCobrosValidaciones"))
            {
                return false;
            }

            pParametro.FechaAltaEvento = DateTime.Now;
            if (!pParametro.NoModificarFechaAlta)
                pParametro.FechaAlta = this.ObtenerFechaAlta(DateTime.Now, bd, tran);

            try
            {
                pParametro.IdTipoCargoAfiliadoFormaCobro = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CarTiposCargosAfiliadosFormasCobrosInsertar");
                if (pParametro.IdTipoCargoAfiliadoFormaCobro == 0)
                    resultado = false;

                if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                    resultado = false;

                if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                    resultado = false;

                if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                    resultado = false;

                if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(pParametro, bd, tran, "CarTiposCargosAfiliadosFormasCobrosInsertarPosterior"))
                    resultado = false;

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, "LogicaNegocio");
                pParametro.CodigoMensaje = ex.Message;
                resultado = false;
            }

            return resultado;
        }
        /// <summary>
        /// Actualiza un Cargo Afiliado Forma Cobro en la BD
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public override bool Modificar(CarTiposCargosAfiliadosFormasCobros pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            //if (!this.Validar(pParametro))
            //    return false;
            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CarTiposCargosAfiliadosFormasCobrosValidaciones"))
            {
                return false;
            }

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CarTiposCargosAfiliadosFormasCobros valorViejo = new CarTiposCargosAfiliadosFormasCobros();
            valorViejo.IdTipoCargoAfiliadoFormaCobro = pParametro.IdTipoCargoAfiliadoFormaCobro;
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
                    resultado = this.Modificar(pParametro, valorViejo, bd, tran);

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
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

        public bool Modificar(CarTiposCargosAfiliadosFormasCobros pParametro, CarTiposCargosAfiliadosFormasCobros valorViejo, Database bd, DbTransaction tran)
        {
            bool resultado = true;

            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CarTiposCargosAfiliadosFormasCobrosActualizar"))
                resultado = false;

            if (!TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                resultado = false;

            if (!TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                resultado = false;

            if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                resultado = false;

            if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                resultado = false;

            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(pParametro, bd, tran, "CarTiposCargosAfiliadosFormasCobrosActualizarPosterior"))
                resultado = false;

            return resultado;
        }

    }
}
