using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Contabilidad.Entidades;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Servicio.AccesoDatos;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Auditoria;
using Auditoria.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System.Data;


namespace Contabilidad.LogicaNegocio
{
    class CtbAsientosContablesLN : BaseLN<CtbAsientosContables>
    {

        public override bool Agregar(CtbAsientosContables pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);


            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            pParametro.FechaEvento = DateTime.Now;

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

        public bool Agregar(CtbAsientosContables pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;

            if (!pParametro.IdEjercicioContable.HasValue)
                pParametro.IdEjercicioContable = new CtbEjerciciosContablesLN().ObtenerActivo(pParametro, bd, tran).IdEjercicioContable;

            TGEParametrosValores paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ValidarInterfazContable);
            bool validar = paramValor.ParametroValor.Length == 0 ? false : Convert.ToBoolean(paramValor.ParametroValor);
            if (validar)
                if (!this.Validar(pParametro, bd, tran))
                    return false;

            pParametro.NumeroAsiento = this.ObtenerNumberoAsiento(pParametro, bd, tran).NumeroAsiento;
            pParametro.IdAsientoContable = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CtbAsientosContablesInsertar");
            if (pParametro.IdAsientoContable == 0)
                resultado = false;

            if (resultado && !this.ActualizarAsientosContablesDetalles(pParametro, new CtbAsientosContables(), bd, tran))
                resultado = false;

            return resultado;
        }

        //public bool Agregar(CtbAsientosContables pParametro, Database bd, DbTransaction tran)
        //{
        //    bool resultado = true;

        //    pParametro.NumeroAsiento = this.ObtenerNumberoAsiento(pParametro, bd, tran).NumeroAsiento;
        //    pParametro.IdAsientoContable = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CtbAsientosContablesInsertar");
        //    if (pParametro.IdAsientoContable == 0)
        //        resultado = false;

        //    //Inserta los Asientos Modelos Detalles
        //    //Pasar a Lucio para que Corrija
        //    //Si un item da False y el siguiente da True se pierde el error del medio y puede quedar inconsistente
        //    //la operacion

        //    foreach (CtbAsientosContablesDetalles asientoContableDetalle in pParametro.AsientosContablesDetalles)
        //    {
        //        resultado = GuardarAsientosContablesDetalles(pParametro, resultado, tran, bd, asientoContableDetalle);
        //    }

        //    return resultado;
        //}

        public override bool Modificar(CtbAsientosContables pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            //if (!this.Validar(pParametro))
            //    return false;

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CtbAsientosContables asientoViejo = new CtbAsientosContables();
            asientoViejo.IdAsientoContable = pParametro.IdAsientoContable;
            asientoViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            asientoViejo = this.ObtenerDatosCompletos(asientoViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    
                    if(resultado)
                        resultado = this.Modificar(pParametro, bd, tran);

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

        public override CtbAsientosContables ObtenerDatosCompletos(CtbAsientosContables pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CtbAsientosContables>("CtbAsientosContablesSeleccionar", pParametro);
            pParametro.AsientosContablesDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbAsientosContablesDetalles>("CtbAsientosContablesDetallesSeleccionarPorAsientoContable", pParametro);
            pParametro.TotalDebe = pParametro.AsientosContablesDetalles.Sum(x => x.Debe.HasValue ? x.Debe.Value : 0);
            pParametro.TotalHaber = pParametro.AsientosContablesDetalles.Sum(x => x.Haber.HasValue ? x.Haber.Value : 0);
            return pParametro;
        }

        public CtbAsientosContables ObtenerDatosCompletos(CtbAsientosContables pParametro, Database db, DbTransaction tran)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CtbAsientosContables>("CtbAsientosContablesSeleccionar", pParametro, db, tran);
            pParametro.AsientosContablesDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbAsientosContablesDetalles>("CtbAsientosContablesDetallesSeleccionarPorAsientoContable", pParametro, db, tran);
            return pParametro;
        }

        /// <summary>
        /// Devuleve un Asiento Contable por Tipo de Operacion
        /// </summary>
        /// <param name="pParametro">IdTipoOperacion, IdRefTipoOperacion</param>
        /// <returns></returns>
        public CtbAsientosContables ObtenerDatosCompletosPorTipoOperacion(CtbAsientosContables pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CtbAsientosContables>("CtbAsientosContablesSeleccionarPorTipoOperacion", pParametro);
            pParametro.AsientosContablesDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbAsientosContablesDetalles>("CtbAsientosContablesDetallesSeleccionarPorAsientoContable", pParametro);
            return pParametro;
        }

        /// <summary>
        /// Devuleve un Asiento Contable por Tipo de Operacion
        /// </summary>
        /// <param name="pParametro">IdTipoOperacion, IdRefTipoOperacion</param>
        /// <returns></returns>
        public CtbAsientosContables ObtenerDatosCompletosPorTipoOperacion(CtbAsientosContables pParametro, Database db, DbTransaction tran)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CtbAsientosContables>("CtbAsientosContablesSeleccionarPorTipoOperacion", pParametro, db, tran);
            pParametro.AsientosContablesDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbAsientosContablesDetalles>("CtbAsientosContablesDetallesSeleccionarPorAsientoContable", pParametro, db, tran);
            return pParametro;
        }

        public List<CtbAsientosContables> ObtenerLista(CtbAsientosContables pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbAsientosContables>("CtbAsientosContablesListar", pParametro);
        }

        public override List<CtbAsientosContables> ObtenerListaFiltro(CtbAsientosContables pParametro)
        {
            var asientosContables = BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbAsientosContables>("CtbAsientosContablesListarFiltro", pParametro);
            foreach (var asientoContable in asientosContables)
            {
                asientoContable.AsientosContablesDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbAsientosContablesDetalles>("CtbAsientosContablesDetallesSeleccionarPorAsientoContable", asientoContable);
            }
            return asientosContables;
        }

        public CtbAsientosContables ObtenerNumberoAsiento(CtbAsientosContables pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CtbAsientosContables>("CtbAsientosContablesObtenerNumero", pParametro);
        }

        /// <summary>
        /// Devuelve el proximo numero de asiento
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public CtbAsientosContables ObtenerNumberoAsiento(CtbAsientosContables pParametro, Database bd, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CtbAsientosContables>("CtbAsientosContablesObtenerNumero", pParametro, bd, tran);
        }

        public List<CtbAsientosContablesDetalles> ArmarAsientoCierre(CtbAsientosContables pParametro)
        {
            List<CtbAsientosContablesDetalles> lista = new List<CtbAsientosContablesDetalles>();
            switch (pParametro.AsientoContableTipo.IdAsientoContableTipo)
            {
                case (int)EnumTiposAsientos.Apertura:
                    lista = BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbAsientosContablesDetalles>("CtbAsientosContablesArmarAsientoApertura", pParametro);
                    break;
                case (int)EnumTiposAsientos.Cierre:
                    lista = BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbAsientosContablesDetalles>("CtbAsientosContablesArmarAsientoCierre", pParametro);
                    break;
                case (int)EnumTiposAsientos.CierreActivosYPasivos:
                    lista = BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbAsientosContablesDetalles>("CtbAsientosContablesArmarAsientoCierreActivosPasivos", pParametro);
                    break;
                case (int)EnumTiposAsientos.AjustesPorInflacion:
                case (int)EnumTiposAsientos.AjustesPorInflacionPorSucursal:
                    lista = BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbAsientosContablesDetalles>("CtbAsientosContablesArmarAsientoAjustesPorInflacion", pParametro);
                    break;
                default:
                    break;
            }
            return lista;
        }

        public bool Modificar(CtbAsientosContables pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            bool actualizarCuentas = false;
            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CtbAsientosContables asientoViejo = new CtbAsientosContables();
            asientoViejo.IdAsientoContable = pParametro.IdAsientoContable;
            asientoViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            asientoViejo = this.ObtenerDatosCompletos(asientoViejo,bd, tran);

            if (pParametro.FechaAsiento.Date != asientoViejo.FechaAsiento.Date)
            {
                pParametro.NumeroAsiento = this.ObtenerNumberoAsiento(pParametro, bd, tran).NumeroAsiento;

                CtbEjerciciosContables ejercicio = new CtbEjerciciosContablesLN().ObtenerActivo(pParametro, bd, tran);
                if (ejercicio.IdEjercicioContable != pParametro.IdEjercicioContable)
                {
                    pParametro.IdEjercicioContable = ejercicio.IdEjercicioContable;
                    actualizarCuentas = true;
                }
            }


            TGEParametrosValores paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ValidarInterfazContable);
            bool validar = paramValor.ParametroValor.Length == 0 ? false : Convert.ToBoolean(paramValor.ParametroValor);
            if (validar)
                if (!this.Validar(pParametro, bd, tran))
                    return false;

            //if (!this.Validar(pParametro, bd, tran))
            //{
            //    resultado = false;
            //    return resultado;
            //}

            if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CtbAsientosContablesActualizar"))
                resultado = false;
            
            if (resultado && !this.ActualizarAsientosContablesDetalles(pParametro, asientoViejo, bd, tran))
                resultado = false;

            if (resultado && !AuditoriaF.AuditoriaAgregar(asientoViejo, Acciones.Update, pParametro, bd, tran))
                resultado = false;

            if (resultado && actualizarCuentas && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CtbContabilizacionInterfacesActualizarPorOperaciones"))
                resultado = false;

            return resultado;
        }
        
        private bool ActualizarAsientosContablesDetalles(CtbAsientosContables pParametro, CtbAsientosContables pValorViejo, Database bd, DbTransaction tran)
        {
            foreach (CtbAsientosContablesDetalles asientoContableDetalle in pParametro.AsientosContablesDetalles)
            {
                switch (asientoContableDetalle.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        asientoContableDetalle.IdAsientoContable = pParametro.IdAsientoContable;
                        asientoContableDetalle.Estado.IdEstado = (int)Estados.Activo;
                        asientoContableDetalle.UsuarioLogueado = pParametro.UsuarioLogueado;
                        asientoContableDetalle.IdAsientoContableDetalle = BaseDatos.ObtenerBaseDatos().Agregar(asientoContableDetalle, bd, tran, "CtbAsientosContablesDetallesInsertar");
                        if (asientoContableDetalle.IdAsientoContableDetalle == 0)
                        {
                            AyudaProgramacionLN.MapearError(asientoContableDetalle, pParametro);
                            return false;
                        }
                        break;
                    case EstadoColecciones.Modificado:
                        asientoContableDetalle.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(asientoContableDetalle, bd, tran, "CtbAsientosContablesDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(asientoContableDetalle, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.AsientosContablesDetalles.Find(x => x.IdAsientoContableDetalle == asientoContableDetalle.IdAsientoContableDetalle), Acciones.Update, asientoContableDetalle, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(asientoContableDetalle, pParametro);
                            return false;
                        }
                        break;
                        //switch (asientoContableDetalle.CuentaContable.EstadoColeccion)
                        //{
                        //    case EstadoColecciones.Agregado:
                        //        asientoContableDetalle.IdAsientoContable = pParametro.IdAsientoContable;
                        //        asientoContableDetalle.Estado.IdEstado = (int)Estados.Activo;
                        //        asientoContableDetalle.UsuarioLogueado = pParametro.UsuarioLogueado;
                        //        asientoContableDetalle.IdAsientoContableDetalle = BaseDatos.ObtenerBaseDatos().Agregar(asientoContableDetalle, bd, tran, "CtbAsientosContablesDetallesInsertar");
                        //        if (asientoContableDetalle.IdAsientoContableDetalle == 0)
                        //        {
                        //            AyudaProgramacionLN.MapearError(asientoContableDetalle, pParametro);
                        //            return false;
                        //        }
                        //        break;
                        //    case EstadoColecciones.Modificado:
                        //        if (!BaseDatos.ObtenerBaseDatos().Actualizar(asientoContableDetalle, bd, tran, "CtbAsientosContablesDetallesActualizar"))
                        //        {
                        //            AyudaProgramacionLN.MapearError(asientoContableDetalle, pParametro);
                        //            return false;
                        //        }
                        //        break;
                        //    default:
                        //        break;
                        //}
                        //break;
                    default:
                        break;
                }
            }
            return this.EliminarAsientosContablesDetalles(pValorViejo, pParametro, bd, tran);
        }

        private bool EliminarAsientosContablesDetalles(CtbAsientosContables pAsientoViejo, CtbAsientosContables pParametro, Database bd, DbTransaction tran)
        {
            foreach (CtbAsientosContablesDetalles asientoDetalle in pAsientoViejo.AsientosContablesDetalles)
            {
                CtbAsientosContablesDetalles clone;
                if (!pParametro.AsientosContablesDetalles.Exists(x => x.IdAsientoContableDetalle == asientoDetalle.IdAsientoContableDetalle))
                {
                    clone = AyudaProgramacionLN.Clone<CtbAsientosContablesDetalles>(asientoDetalle);
                    asientoDetalle.UsuarioLogueado = pParametro.UsuarioLogueado;
                    asientoDetalle.Estado.IdEstado = (int)Estados.Baja;
                    asientoDetalle.Estado.Descripcion = Estados.Baja.ToString();
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(asientoDetalle, bd, tran, "CtbAsientosContablesDetallesActualizarEstado"))
                    {
                        AyudaProgramacionLN.MapearError(asientoDetalle, pParametro);
                        return false;
                    }
                    if (!AuditoriaF.AuditoriaAgregar(clone, Acciones.Update, asientoDetalle, bd, tran))
                    {
                        AyudaProgramacionLN.MapearError(asientoDetalle, pParametro);
                        return false;
                    }

                    //if (!BaseDatos.ObtenerBaseDatos().Actualizar(asientoDetalle, bd, tran, "CtbAsientosContablesDetallesEliminar"))
                    //{
                    //    AyudaProgramacionLN.MapearError(asientoDetalle, pParametro);
                    //    return false;
                    //}
                }
            }
            //foreach (CtbAsientosContablesDetalles asientoDetalle in pAsientoViejo.AsientosContablesDetalles)
            //{
            //    if (!pParametro.AsientosContablesDetalles.Exists(x => x.IdAsientoContableDetalle == asientoDetalle.IdAsientoContableDetalle))
            //    {
            //        if (!BaseDatos.ObtenerBaseDatos().Actualizar(asientoDetalle, bd, tran, "CtbAsientosContablesDetallesEliminar"))
            //        {
            //            AyudaProgramacionLN.MapearError(asientoDetalle, pParametro);
            //            return false;
            //        }
            //    }
            //}
            return true;
        }

        private bool Validar(CtbAsientosContables pParametro, Database bd, DbTransaction tran)
        {
            decimal? debe = 0;
            decimal? haber = 0;

            if (!pParametro.IdEjercicioContable.HasValue || pParametro.IdEjercicioContable.Value == 0)
            {
                pParametro.CodigoMensaje = "ValidarEjercicioContable";
                return false;
            }

            //Valido que la fecha del asiento pertenezca al ejercicio seleccionado
            if (!this.ValidarEjercicioConFecha(pParametro, bd, tran))
                return false;

            //Valido que el Periodo Contable no este cerrado
            CtbPeriodosContables periodoContable = new CtbPeriodosContables();
            periodoContable.Periodo = AyudaProgramacionLN.ObtenerPeriodo(pParametro.FechaAsiento);
            if (new CtbPeriodosContablesLN().ValidarCierre(periodoContable, bd, tran))
            {
                pParametro.CodigoMensaje = "ValidarFechaContablePeriodoContableCerrado";
                pParametro.CodigoMensajeArgs.Add(periodoContable.Periodo.ToString());
                return false;
            }

            List<CtbAsientosContablesDetalles> asientos = pParametro.AsientosContablesDetalles.FindAll(x=>x.CuentaContable.IdCuentaContable != 0);
            if (asientos.Count < 2)
            {
                pParametro.CodigoMensaje = "AsientoModeloDosCuentasContables";
                return false;
            }

            if(pParametro.AsientosContablesDetalles.Exists(x=>x.CuentaContable.CentroCostoObligatorio && !x.CentroCostoProrrateo.IdCentroCostoProrrateo.HasValue))
            {
                CtbAsientosContablesDetalles detalle = pParametro.AsientosContablesDetalles.Find(x => x.CuentaContable.CentroCostoObligatorio && !x.CentroCostoProrrateo.IdCentroCostoProrrateo.HasValue);
                pParametro.CodigoMensaje = "CentroCostoObligatorio";
                pParametro.CodigoMensajeArgs.Add(detalle.CuentaContable.NumeroCuenta);
                pParametro.CodigoMensajeArgs.Add(detalle.CuentaContable.Descripcion);
                return false;
            }

            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CtbAsientosContablesValidaciones"))
            {
                return false;
            }

            foreach (var asientoContableDetalle in pParametro.AsientosContablesDetalles)
            {
                //if (asientoContableDetalle.CuentaContable.IdCuentaContable == 0)
                //{
                //    pParametro.CodigoMensaje = "SeleccioneCuentaContable";
                //    pParametro.CodigoMensajeArgs.Add(pParametro.TipoOperacion);
                //    return false;
                //}
                if ((asientoContableDetalle.Debe == null || asientoContableDetalle.Debe == 0) && (asientoContableDetalle.Haber == null || asientoContableDetalle.Haber == 0))
                {
                    pParametro.CodigoMensaje = "CompletarDebeHaber";
                    return false;
                }
                if ((asientoContableDetalle.Debe.HasValue || asientoContableDetalle.Haber.HasValue) && asientoContableDetalle.CuentaContable.IdCuentaContable == 0)
                {
                    pParametro.CodigoMensaje = "CompletarCuentaContable";
                    pParametro.CodigoMensajeArgs.Add( (asientoContableDetalle.IndiceColeccion + 1).ToString());
                    return false;
                }

                if (asientoContableDetalle.Debe != null)
                    debe += asientoContableDetalle.Debe;

                if (asientoContableDetalle.Haber != null)
                    haber += asientoContableDetalle.Haber;
                
            }
            if (debe != haber)
            {
                this.AgregarLog(pParametro);
                pParametro.CodigoMensaje = "DebeHaberIguales";
                pParametro.CodigoMensajeArgs.Add(pParametro.IdAsientoContableLog.ToString());
                return false;
            }
 
            return true;
        }

        private bool ValidarEjercicioConFecha(CtbAsientosContables pParametro, Database bd , DbTransaction tran)
        {
            CtbEjerciciosContables EjercicioPorFecha = (new CtbEjerciciosContablesLN().ObtenerActivo(pParametro, bd, tran));
            if (pParametro.IdEjercicioContable != EjercicioPorFecha.IdEjercicioContable)
            {
                CtbEjerciciosContables ejercicioSeleccionado = new CtbEjerciciosContables();
                ejercicioSeleccionado.IdEjercicioContable = (int)pParametro.IdEjercicioContable;
                ejercicioSeleccionado = new CtbEjerciciosContablesLN().ObtenerDatosCompletos(ejercicioSeleccionado, bd, tran);
                pParametro.CodigoMensaje = "FechaAsientoNoValidaArg";
                pParametro.CodigoMensajeArgs.Add(ejercicioSeleccionado.FechaInicio.ToShortDateString());
                pParametro.CodigoMensajeArgs.Add(ejercicioSeleccionado.FechaFin.ToShortDateString());
                return false;
            }
            

            return true;
        }

        public bool RevertirAsientoContable(CtbAsientosContables anular, TGETiposOperaciones pTipoOperacionAnular, Database bd, DbTransaction tran)
        {
            //Busco el Asiento de la Solicitud de Compra para Anular
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pTipoOperacionAnular.IdTipoOperacion;
            asiento.IdRefTipoOperacion = anular.IdRefTipoOperacion;
            asiento = ContabilidadF.AsientosContablesObtenerDatosCompletosPorTipoOperacion(asiento, bd, tran);

            if (asiento.IdAsientoContable==0)
            {
                anular.CodigoMensaje = "AsientoContableNoExiste";
                return false;
            }

            anular.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            anular.Estado.IdEstado = (int)EstadosContabilidad.Activo;

            //anular.IdRefTipoOperacion = asiento.IdRefTipoOperacion;
            anular.FechaAsiento = DateTime.Now;
            //anular.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(bd, tran).IdEjercicioContable;

            CtbAsientosContablesDetalles detalleAnular;
            foreach (CtbAsientosContablesDetalles detalle in asiento.AsientosContablesDetalles)
            {
                detalleAnular = new CtbAsientosContablesDetalles();
                detalleAnular.Estado.IdEstado = (int)EstadosContabilidad.Activo;
                detalleAnular.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
                detalleAnular.CuentaContable = detalle.CuentaContable;
                detalleAnular.Debe = detalle.Haber;
                detalleAnular.Haber = detalle.Debe;
                anular.AsientosContablesDetalles.Add(detalleAnular);
            }
            
            return true;
        }

        public DataTable ObtenerListaFiltroGrilla(CtbAsientosContables pCuentaContable)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CtbAsientosContablesListarFiltro", pCuentaContable);
        }

        public DataTable ObtenerLibroMayorGrilla(CtbCuentasContables pCuentaContable)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("ReportesCtbAsientosContablesLibroMayor", pCuentaContable);
        }

        public bool AgregarLog(CtbAsientosContables pParametro)
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();
            DbCommand cmd = bd.GetStoredProcCommand("CtbAsientosContablesLogInsertar");
            bd.DiscoverParameters(cmd);
            Mapeador.MapearEntidadParametros(pParametro, cmd);
            Object resultado = bd.ExecuteScalar(cmd);
            if (DBNull.Value == resultado)
                return false;

            pParametro.IdAsientoContableLog = Convert.ToInt32(resultado);
            cmd = bd.GetStoredProcCommand("CtbAsientosContablesDetallesLogInsertar");
            bd.DiscoverParameters(cmd);
            foreach (CtbAsientosContablesDetalles asientoContableDetalle in pParametro.AsientosContablesDetalles)
            {
                asientoContableDetalle.IdAsientoContableLog = pParametro.IdAsientoContableLog;
                asientoContableDetalle.Estado.IdEstado = (int)Estados.Activo;
                asientoContableDetalle.UsuarioLogueado = pParametro.UsuarioLogueado;
                Mapeador.MapearEntidadParametros(asientoContableDetalle, cmd);
                resultado = bd.ExecuteScalar(cmd);
                if (DBNull.Value == resultado)
                return false;
                asientoContableDetalle.IdAsientoContableDetalleLog = Convert.ToInt32(resultado);
            }
            return true;
        }
    }
}
