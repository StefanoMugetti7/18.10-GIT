using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Contabilidad.Entidades;
using Servicio.AccesoDatos;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Auditoria.Entidades;
using Auditoria;
using Generales.Entidades;
using Generales.FachadaNegocio;

namespace Contabilidad.LogicaNegocio
{
    internal class CtbEjerciciosContablesLN : BaseLN<CtbEjerciciosContables>
    {

       
        public bool ActualizarPlanCuentas(CtbEjerciciosContables pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;





            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {



                    if (pParametro.IdEjercicioContableOrigen != 0)
                    {
                        if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CtbCuentasContablesCopiarPlan"))
                            resultado = false;

                        //if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CtbAsientosModelosCopiarEjercicioAEjercicio"))
                        //    resultado = false;

                    }
                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ActualizarPlanCuentas";
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
                    pParametro.CodigoMensaje = "ActualizarPlanCuentasIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        public override bool Agregar(CtbEjerciciosContables pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;


            if (!this.Validar(pParametro, new CtbEjerciciosContables()))
                return false;
                        
            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();
            
            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    
                    pParametro.IdEjercicioContable = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CtbEjerciciosContablesInsertar");
                    if (!resultado || pParametro.IdEjercicioContable == 0)
                        resultado = false;

                    if (pParametro.IdEjercicioContableOrigen != 0)
                    {
                        if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CtbCuentasContablesCopiarPlan"))
                            resultado = false;

                        if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CtbAsientosModelosCopiarEjercicioAEjercicio"))
                            resultado = false;

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

        public override bool Modificar(CtbEjerciciosContables pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CtbEjerciciosContables ejercicioContableViejo = new CtbEjerciciosContables();
            ejercicioContableViejo.IdEjercicioContable = pParametro.IdEjercicioContable;
            ejercicioContableViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            ejercicioContableViejo = this.ObtenerDatosCompletos(ejercicioContableViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = this.Modificar(pParametro, bd, tran);

                    if (resultado && !AuditoriaF.AuditoriaAgregar(ejercicioContableViejo, Acciones.Update, pParametro, bd, tran))
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

        public override CtbEjerciciosContables ObtenerDatosCompletos(CtbEjerciciosContables pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CtbEjerciciosContables>("CtbEjerciciosContablesSeleccionar", pParametro);
        }

        public CtbEjerciciosContables ObtenerDatosCompletos(CtbEjerciciosContables pParametro, Database db, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CtbEjerciciosContables>("CtbEjerciciosContablesSeleccionar", pParametro, db, tran);
        }

        /// <summary>
        /// Devuleve el Ejercicio Contable Activo segun la Fecha
        /// </summary>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <param name="pAsiento"></param>
        /// <returns></returns>
        public CtbEjerciciosContables ObtenerActivo()
        {
            CtbAsientosContables filtro = new CtbAsientosContables();
            filtro.FechaAsiento = DateTime.Now;
            //return BaseDatos.ObtenerBaseDatos().Obtener<CtbEjerciciosContables>("CtbEjerciciosContablesSeleccionarActivo", filtro);
            CtbEjerciciosContables resultado = BaseDatos.ObtenerBaseDatos().Obtener<CtbEjerciciosContables>("CtbEjerciciosContablesSeleccionarActivo", filtro);
            if (!resultado.IdEjercicioContable.HasValue)
            {
                TGEParametrosValores paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ValidarInterfazContable);
                bool validar = paramValor.ParametroValor.Length == 0 ? false : Convert.ToBoolean(paramValor.ParametroValor);
                if (validar)
                    throw new Exception("No se encontro un Ejercicio Contable para la Fecha " + DateTime.Now.ToShortDateString());
            }
            return resultado;
        }

        public CtbEjerciciosContables ObtenerActivo(Database bd, DbTransaction tran)
        {
            CtbAsientosContables filtro = new CtbAsientosContables();
            filtro.FechaAsiento = DateTime.Now;
            return this.ObtenerActivo(filtro, bd, tran);
        }

        /// <summary>
        /// Devuleve el Ejercicio Contable Activo segun la Fecha del Asiento
        /// </summary>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <param name="pAsiento"></param>
        /// <returns></returns>
        public CtbEjerciciosContables ObtenerActivo(CtbAsientosContables pAsiento, Database bd, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CtbEjerciciosContables>("CtbEjerciciosContablesSeleccionarActivo", pAsiento, bd, tran);
        }

        public CtbEjerciciosContables ObtenerUltimoActivo()
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CtbEjerciciosContables>("CtbEjerciciosContablesSeleccionarUltimoActivo");
        }

        public override List<CtbEjerciciosContables> ObtenerListaFiltro(CtbEjerciciosContables pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbEjerciciosContables>("CtbEjerciciosContablesListarFiltro", pParametro);
        }

        public List<CtbEjerciciosContables> ObtenerLista(CtbEjerciciosContables pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbEjerciciosContables>("CtbEjerciciosContablesListar", pParametro);
        }

        public bool Modificar(CtbEjerciciosContables pParametro, Database bd, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CtbEjerciciosContablesActualizar"))
                return false;

            return true;
        }

        public CtbEjerciciosContables ObtenerActivo(CtbAsientosContables pAsiento)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CtbEjerciciosContables>("CtbEjerciciosContablesSeleccionarActivo", pAsiento);
        }

        public bool Validar(CtbEjerciciosContables pParametro,CtbEjerciciosContables pValorViejo)
        {
            
            switch (pParametro.EstadoColeccion)
            {
                case EstadoColecciones.Agregado:
                    if (pParametro.FechaInicio >= pParametro.FechaFin)
                    {
                        pParametro.CodigoMensaje = "ValidarFechaInicio";
                        return false;
                    }
                    
                    List<CtbEjerciciosContables> ejercicios;
                    CtbEjerciciosContables filtro = new CtbEjerciciosContables();
                    filtro.Estado.IdEstado = (int)Estados.Activo; 
                    ejercicios = this.ObtenerListaFiltro(filtro); //obtengo todos los ejercicios ACTIVOS
                    if (ejercicios.Exists(x => x.FechaFin >= pParametro.FechaInicio))
                    {
                        pParametro.CodigoMensaje = "FechaFinConFechaInicio";
                        pParametro.CodigoMensajeArgs.Add(ejercicios.Max(x=>x.FechaFin).ToString("ddMMyyyy"));
                        return false;
                    }

                    if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CtbEjerciciosContablesValidaciones"))
                    {
                        return false;
                    }

                    //CtbAsientosContables filtro = new CtbAsientosContables();
                    //filtro.FechaAsiento = DateTime.Now;
                    ////voy a usar pValorViejo para aprovechar la variable vacia
                    //pValorViejo = this.ObtenerActivo(filtro);
                    //pValorViejo = this.ObtenerDatosCompletos(pValorViejo);
                    //if (pValorViejo.FechaCierre.HasValue)
                    //{
                    //    //Si pasa el IF es porque tiene un valor distinto de null, y la copio en variable dateTime para poder usar los metodos de agregado(para no andar casteando)
                    //    DateTime fechaCierre = Convert.ToDateTime(pValorViejo.FechaCierre);
                    //    if (fechaCierre.AddDays(1).ToString("ddMMyyyy") != pParametro.FechaInicio.ToString("ddMMyyyy"))
                    //    {
                    //        pParametro.CodigoMensaje = "FechaCierreConFechaInicio";
                    //        pParametro.CodigoMensajeArgs.Add(fechaCierre.ToString("ddMMyyyy"));
                    //        pParametro.CodigoMensajeArgs.Add(pParametro.FechaInicio.ToString("ddMMyyyy"));
                    //        return false;
                    //    }
                    //}
                    break;
                default:
                    break;

                /*Validar, en el caso de que exista un ejercicio activo, que al dar de alta la Fecha de inicio debe ser igual a la fecha de cierre mas un dia del ejercicio activo. 
                Validar (si no esta hecho) que la fecha de inicio sea menor a la fecha de fin.*/
            }
            return true;
        }
    }
}
