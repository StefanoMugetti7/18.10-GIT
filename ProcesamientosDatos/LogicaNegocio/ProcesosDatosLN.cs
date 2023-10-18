using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Servicio.AccesoDatos;
using Generales.Entidades;
using Generales.FachadaNegocio;
using ProcesosDatos.Entidades;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Comunes.LogicaNegocio;
using System.Collections;
using ProcesamientosDatos.Entidades;
using Comunes.Entidades;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
//using EO.Web;

namespace ProcesosDatos.LogicaNegocio
{
    public class ProcesosDatosLN
    {
        private class msCallback
        {
            public int Progress { get; set; }
            public string Msg { get; set; }
        }

        public delegate void ProcesosDatosEjecutarSPMensajes(List<string> e);
        public event ProcesosDatosEjecutarSPMensajes ProcesoDatosEjecutarSPMensajesCallback;

        public List<string> mensajesBasedatos;

        /// <summary>
        /// Devuelve un Proceso del Sistema con sus parametros
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public SisProcesos ObtenerDatosCompletos(SisProcesos pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<SisProcesos>("SisProcesosSeleccionarDetalle",pParametro);
            pParametro.Parametros = BaseDatos.ObtenerBaseDatos().ObtenerLista<SisParametros>("SisParametrosSeleccionarDescripcionesPorSisProcesos", pParametro);
            //pParametro.ProcesoArchivo = BaseDatos.ObtenerBaseDatos().Obtener<SisProcesosArchivos>("", pParametro);
            pParametro.ProcesoArchivo.ProcesosArchivosCampos = BaseDatos.ObtenerBaseDatos().ObtenerLista<SisProcesosArchivosCampos>("SisProcesosArchivosCamposSeleccionarDetalle", pParametro.ProcesoArchivo);
            return pParametro;
        }
         
        /// <summary>
        /// Devuelve una lista de Procesos Activos
        /// </summary>
        /// <returns></returns>
        public List<SisProcesos> ObtenerLista()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<SisProcesos>("SisProcesosListar");
        }

        public List<SisProcesos> ObtenerLista(Perfiles Perfil)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<SisProcesos>("SisProcesosListarPerfil", Perfil);
        }

        public List<SisProcesos> ObtenerListaFiltro(SisProcesos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<SisProcesos>("SisProcesosSeleccionarFiltros", pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Procesos Ejecutados por Proceso
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public List<SisProcesosProcesamiento> ObtenerLista(SisProcesos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<SisProcesosProcesamiento>("SisProcesosProcesamientoPorProceso", pParametro);
        }
        /// <summary>
        /// Devuelve un datatable de Procesos Ejecutados por Proceso
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public DataTable ObtenerListaDT(SisProcesos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("SisProcesosProcesamientoPorProcesoDT", pParametro);
        }
        /// <summary>
        /// Devuelve un datatable con los parametros de Procesos Ejecutados por Proceso (Consultar)
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public DataTable ObtenerListaParametros(SisProcesosProcesamiento pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("[SisProcesosProcesamientoObtenerValoresParametros]", pParametro);
        }
        public SisProcesos ObtenerDatosCompletosPorProcesamiento(SisProcesosProcesamiento pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<SisProcesos>("SisProcesosSeleccionarPorIdProcesamiento", pParametro);
        }
        /// <summary>
        /// Obtiene el ultimo periodo procesado por periodo
        /// </summary>
        /// <param name="pParametro">IdSisProceso</param>
        /// <returns></returns>
        public SisProcesosProcesamiento ObtenerUltimoPeriodoProcesado(SisProcesosProcesamiento pParametro, Database bd, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<SisProcesosProcesamiento>("SisProcesosProcesamientoSeleccionarUltimo", pParametro, bd, tran);
        }

        /// <summary>
        /// Obtiene el ultimo periodo procesado por periodo
        /// </summary>
        /// <param name="pParametro">IdSisProceso</param>
        /// <returns></returns>
        public SisProcesosProcesamiento ObtenerUltimoPeriodoProcesado(SisProcesosProcesamiento pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<SisProcesosProcesamiento>("SisProcesosProcesamientoSeleccionarUltimo", pParametro);
        }

        /// <summary>
        /// Valida si un periodo tiene los cargos generados
        /// </summary>
        /// <param name="pPeriodo"></param>
        /// <returns></returns>
        public bool ValidarCargosGenerados(int pPeriodo,Database bd, DbTransaction tran)
        {
            SisProcesosProcesamiento procesamiento = new SisProcesosProcesamiento();
            procesamiento.Proceso.IdProceso = (int)EnumSisProcesos.GeneracionCargos;
            procesamiento = this.ObtenerUltimoPeriodoProcesado(procesamiento, bd, tran);

            if (pPeriodo >= procesamiento.Periodo)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Valida si un periodo tiene los cargos generados
        /// </summary>
        /// <param name="pPeriodo"></param>
        /// <returns></returns>
        public bool ValidarCargosGenerados(int pPeriodo)
        {
            SisProcesosProcesamiento procesamiento = new SisProcesosProcesamiento();
            procesamiento.Proceso.IdProceso = (int)EnumSisProcesos.GeneracionCargos;
            procesamiento = this.ObtenerUltimoPeriodoProcesado(procesamiento);

            if (pPeriodo >= procesamiento.Periodo)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Devuelve una lista con los proximos Periodos de Cargos a Procesar
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public List<int> ObtenerProximosPeriodosCargos()
        {
            //SisProcesosProcesamiento procesamiento = new SisProcesosProcesamiento();
            //procesamiento.Proceso.IdProceso = (int)EnumSisProcesos.GeneracionCargos;
            //procesamiento = this.ObtenerUltimoPeriodoProcesado(procesamiento);
            //int periodo = 0;
            //if (procesamiento.IdProcesoProcesamiento > 0)
            //{
            //    periodo = procesamiento.Periodo;
            //}
            //else
            //    periodo = AyudaProgramacionLN.ObtenerPeriodo(DateTime.Now);

            //DateTime fecha = new DateTime(Convert.ToInt32(periodo.ToString().Substring(0, 4)), Convert.ToInt32(periodo.ToString().Substring(4, 2)), 1);
            //List<int> resultado = new List<int>();
            //for(int i=0;i<20;i++)
            //{
            //    resultado.Add(AyudaProgramacionLN.ObtenerPeriodo(fecha));
            //    fecha = fecha.AddMonths(1);
            //}

            DataTable dt = BaseDatos.ObtenerBaseDatos().ObtenerLista("SisProcesosProcesamientoSeleccionarPeriodosCargos", new Objeto());
            List<int> resultado = new List<int>();
            if(dt.Rows.Count > 0)
                resultado.AddRange(dt.AsEnumerable().Select(x=>x.Field<int>("Periodo")));
            return resultado;
        }



        /// <summary>
        /// Devuelve una lista con los ultimos Periodos de Cargos Procesados.
        /// Cantidad = Cantidad de Periodos a devolver.
        /// </summary>
        /// <param name="pParametro">Cantidad</param>
        /// <returns></returns>
        public List<int> ObtenerUltimosPeriodosCargos(int cantidad)
        {
            SisProcesosProcesamiento procesamiento = new SisProcesosProcesamiento();
            procesamiento.Proceso.IdProceso = (int)EnumSisProcesos.GeneracionCargos;
            procesamiento = this.ObtenerUltimoPeriodoProcesado(procesamiento);
            int periodo = 0;
            if (procesamiento.IdProcesoProcesamiento > 0)
            {
                periodo = procesamiento.Periodo;
            }
            else
                periodo = AyudaProgramacionLN.ObtenerPeriodo(DateTime.Now);


            DateTime fecha = new DateTime(Convert.ToInt32(periodo.ToString().Substring(0, 4)), Convert.ToInt32(periodo.ToString().Substring(4, 2)), 1);
            //fecha = fecha.AddMonths(cantidad * -1);
            List<int> resultado = new List<int>();
            for (int i = 0; i < cantidad; i++)
            {
                resultado.Add(AyudaProgramacionLN.ObtenerPeriodo(fecha));
                fecha = fecha.AddMonths(-1);
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene los valores para un Parametro
        /// </summary>
        /// <param name="pProceso"></param>
        /// <returns></returns>
        public DataSet ObtenerDatosParametro(SisProcesos pProceso)
        {
            DataSet dataSet = new DataSet();
            try
            {
                int tiempoEspera = Convert.ToInt32(TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.DBTiempoEjecucionProcesosDatos).ParametroValor);

                DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database db = factory.CreateDefault();
                DbCommand dbCommand = db.GetStoredProcCommand(pProceso.ProcesoArchivo.StoredProcedure);
                if (tiempoEspera > dbCommand.CommandTimeout)
                    dbCommand.CommandTimeout = tiempoEspera;
                db.DiscoverParameters(dbCommand);
                foreach (SisParametros parametro in pProceso.Parametros)
                {
                    if (!dbCommand.Parameters.Contains(string.Concat("@", parametro.Parametro)))
                        continue;

                    switch (parametro.TipoParametro.IdTipoParametro)
                    {
                        case (int)EnumSisTipoParametros.Int:
                        case (int)EnumSisTipoParametros.DropDownListSPAutoComplete:
                            dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(int));
                            break;
                        case (int)EnumSisTipoParametros.CheckBoxList:
                        case (int)EnumSisTipoParametros.GridViewCheckFecha:
                        case (int)EnumSisTipoParametros.GridViewCheckDinamico:
                        case (int)EnumSisTipoParametros.GridViewCheckImporte:
                            dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : parametro.ValorParametro;
                            break;
                        case (int)EnumSisTipoParametros.DateTime:
                            dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime));
                            break;
                        case (int)EnumSisTipoParametros.TextBox:
                            dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : parametro.ValorParametro;
                            break;
                        case (int)EnumSisTipoParametros.IntNumericInput:
                            dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(int));
                            break;
                        case (int)EnumSisTipoParametros.DateTimeRange:
                            dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime));
                            break;
                        case (int)EnumSisTipoParametros.YearMonthCombo:
                            dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime));
                            break;
                        default:
                            break;
                    }
                }
                dataSet = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                pProceso.ErrorAccesoDatos = true;
                pProceso.CodigoMensaje = "RepAccesoDatos";
                pProceso.ErrorException = ex.Message;
                throw ex;
            }
            return dataSet;
        }

        /// <summary>
        /// Ejecuta un Proceso en la Base de Datos
        /// No maneja Transacciones
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool EjecutarProcesos(SisProcesosProcesamiento pParametro)
        {
            bool resultado = true;
            try
            {
                AyudaProgramacionLN.LimpiarMensajesError(pParametro);
                Hashtable listaParametros = new Hashtable();
                string sp = string.Empty;

                //TGEParametros param = new TGEParametros();
                //param.IdParametro = (int)EnumTGEParametros.DBTiempoEjecucionProcesosDatos;
                int tiempoEspera = Convert.ToInt32(TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.DBTiempoEjecucionProcesosDatos).ParametroValor);

                listaParametros.Add("IdProceso", pParametro.Proceso.IdProceso);
                listaParametros.Add("NombreArchivo", pParametro.Proceso.ProcesoArchivo.NombreArchivo);
                listaParametros.Add("Path", pParametro.Proceso.ProcesoArchivo.Path);
                listaParametros.Add("IdUsuarioEvento", pParametro.UsuarioLogueado.IdUsuarioEvento.ToString());

                foreach (SisParametros parametro in pParametro.Proceso.Parametros)
                {
                    //listaParametros.Add(sisParam.Parametro, sisParam.ValorParametro);
                    switch (parametro.TipoParametro.IdTipoParametro)
                    {
                        case (int)EnumSisTipoParametros.Int:
                        case (int)EnumSisTipoParametros.DropDownListSPAutoComplete:
                            listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(int)));
                            //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(int));
                            break;
                        case (int)EnumSisTipoParametros.CheckBoxList:
                        case (int)EnumSisTipoParametros.GridViewCheckFecha:
                        case (int)EnumSisTipoParametros.GridViewCheckDinamico:
                        case (int)EnumSisTipoParametros.GridViewCheckImporte:
                            listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : parametro.ValorParametro);
                            //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : parametro.ValorParametro;
                            break;
                        case (int)EnumSisTipoParametros.DateTime:
                            //listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : (object)(Convert.ToDateTime(parametro.ValorParametro).ToString("s")));
                            listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime)));
                            //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime));
                            break;
                        case (int)EnumSisTipoParametros.TextBox:
                            listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : parametro.ValorParametro);
                            //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : parametro.ValorParametro;
                            break;
                        case (int)EnumSisTipoParametros.IntNumericInput:
                            listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(Int64)));
                            //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(int));
                            break;
                        case (int)EnumSisTipoParametros.DateTimeRange:
                            listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime)));
                            //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime));
                            break;
                        case (int)EnumSisTipoParametros.YearMonthCombo:
                            listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime)));
                            //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime));
                            break;
                        default:
                            break;
                    }
                }

                sp = pParametro.Proceso.ProcesoArchivo.StoredProcedure;
                BaseDatos accesoDatos = BaseDatos.ObtenerBaseDatos();
                mensajesBasedatos = new List<string>();
                Objeto obj = new Objeto();
                if (!accesoDatos.EjecutarSP(obj, "SisProcesosEliminarTablasTemporales", BaseDatos.conexionPredeterminada))
                {
                    resultado = false;
                    AyudaProgramacionLN.MapearError(obj, pParametro);
                }

                if (resultado)
                {
                    accesoDatos.BaseDatosEjecutarSPMensajesCallback += new BaseDatos.BaseDatosEjecutarSPMensajes(bd_BaseDatosEjecutarSPMensajesCallback);
                    pParametro.IdProcesoProcesamiento = accesoDatos.EjecutarSPMensajes(pParametro, listaParametros, sp, tiempoEspera);

                    if (pParametro.IdProcesoProcesamiento == 0)
                    {
                        resultado = false;
                    }
                }

                if (resultado)
                {
                    bool resultadoLog = true;
                    DbTransaction tran;
                    DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();
                    using (DbConnection con = bd.CreateConnection())
                    {
                        con.Open();
                        tran = con.BeginTransaction();
                        try
                        {
                            foreach (SisParametros parametro in pParametro.Proceso.Parametros)
                            {
                                parametro.IdProcesoProcesamiento = pParametro.IdProcesoProcesamiento;
                                parametro.IdProcesoProcesamientoParametroValor = BaseDatos.ObtenerBaseDatos().Agregar(parametro, bd, tran, "SisProcesosProcesamientoParametrosValoresINSERT");
                            }

                            if (resultadoLog)
                            {
                                tran.Commit();
                                pParametro.CodigoMensaje = "ResultadoTransaccion";
                            }
                            else
                            {
                                tran.Rollback();
                                resultadoLog = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionHandler.HandleException(ex, "LogicaNegocio");
                            tran.Rollback();
                            string msgError = string.Empty;
                            if (ex is SqlException)
                            {
                                SqlErrorCollection erroCol = ((SqlException)ex).Errors;
                                if (erroCol.Count > 0)
                                    msgError = erroCol[0].Message;
                            }
                            pParametro.ErrorAccesoDatos = true;
                            pParametro.ErrorException = ex.Message;
                            pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                            pParametro.CodigoMensajeArgs.Add(msgError == string.Empty ? ex.Message : msgError);
                            resultadoLog = false;
                        }
                    }
                }
            }
            //catch (SqlException sqlex)
            //{
            //    string s = sqlex.Message;
            //}
            catch (Exception ex)
            {
                pParametro.CodigoMensaje = ex.Message;
            }
            finally {
            }
            return resultado;
        }

        public void bd_BaseDatosEjecutarSPMensajesCallback(string e)
        {
            this.mensajesBasedatos.Add(e);
            if (this.ProcesoDatosEjecutarSPMensajesCallback != null)
                this.ProcesoDatosEjecutarSPMensajesCallback(this.mensajesBasedatos);
        }

        /// <summary>
        /// Ejecuta un Proceso en la Base de Datos
        /// No maneja Transacciones
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool EjecutarProcesos(SisProcesosProcesamiento pParametro, ref DataSet dataSet)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            Hashtable listaParametros = new Hashtable();
            string sp = string.Empty;
            //DataSet dataSet = new DataSet();
            int tiempoEspera = Convert.ToInt32(TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.DBTiempoEjecucionProcesosDatos).ParametroValor);

            listaParametros.Add("IdProceso", pParametro.Proceso.IdProceso);
            listaParametros.Add("NombreArchivo", pParametro.Proceso.ProcesoArchivo.NombreArchivo);
            listaParametros.Add("Path", pParametro.Proceso.ProcesoArchivo.Path);
            listaParametros.Add("IdUsuarioEvento", pParametro.UsuarioLogueado.IdUsuarioEvento.ToString());

            foreach (SisParametros parametro in pParametro.Proceso.Parametros)
            {
                //listaParametros.Add(sisParam.Parametro, sisParam.ValorParametro);
                switch (parametro.TipoParametro.IdTipoParametro)
                {
                    case (int)EnumSisTipoParametros.Int:
                    case (int)EnumSisTipoParametros.DropDownListSPAutoComplete:
                        listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(int)));
                        //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(int));
                        break;
                    case (int)EnumSisTipoParametros.CheckBoxList:
                    case (int)EnumSisTipoParametros.GridViewCheckFecha:
                    case (int)EnumSisTipoParametros.GridViewCheckDinamico:
                    case (int)EnumSisTipoParametros.GridViewCheckImporte:
                        listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : parametro.ValorParametro);
                        //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : parametro.ValorParametro;
                        break;
                    case (int)EnumSisTipoParametros.DateTime:
                        //listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : (object)(Convert.ToDateTime(parametro.ValorParametro).ToString("s")));
                        listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime)));
                        //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime));
                        break;
                    case (int)EnumSisTipoParametros.TextBox:
                        listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : parametro.ValorParametro);
                        //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : parametro.ValorParametro;
                        break;
                    case (int)EnumSisTipoParametros.IntNumericInput:
                        listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(Int64)));
                        //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(int));
                        break;
                    case (int)EnumSisTipoParametros.DateTimeRange:
                        listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime)));
                        //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime));
                        break;
                    case (int)EnumSisTipoParametros.YearMonthCombo:
                        listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime)));
                        //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime));
                        break;
                    default:
                        break;
                }
            }

            try
            {
                DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database db = factory.CreateDefault();
                DbCommand dbCommand = db.GetStoredProcCommand(pParametro.Proceso.ProcesoArchivo.StoredProcedure);
                db.DiscoverParameters(dbCommand);
                Servicio.AccesoDatos.Mapeador.MapearEntidadParametros(listaParametros, dbCommand);

                if (tiempoEspera > dbCommand.CommandTimeout)
                    dbCommand.CommandTimeout = tiempoEspera;

                if (dbCommand.Parameters.Contains("@IdUsuarioEvento"))
                {
                    if (pParametro.GetType().GetProperty("UsuarioLogueado") != null)
                        dbCommand.Parameters["@IdusuarioEvento"].Value = pParametro.GetType().GetProperty("UsuarioLogueado").GetValue(pParametro, null).GetType().GetProperty("IdUsuarioEvento").GetValue(pParametro.GetType().GetProperty("UsuarioLogueado").GetValue(pParametro, null), null);
                }
                dataSet = db.ExecuteDataSet(dbCommand);

                pParametro.ErrorAccesoDatos = false;
                pParametro.CodigoMensaje = "ResultadoTransaccion";
                return true;
            }
            catch (Exception ex)
            {
                string msgError = string.Empty;
                if (ex is SqlException)
                {
                    SqlErrorCollection erroCol = ((SqlException)ex).Errors;
                    if (erroCol.Count > 0)
                        msgError = erroCol[0].Message;
                }
                pParametro.ErrorAccesoDatos = true;
                pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                pParametro.ErrorException = ex.Message;
                pParametro.CodigoMensajeArgs.Add(msgError == string.Empty ? ex.Message : msgError);
                return false;
            }
        }

        public bool EjecutarProcesosObtenerGrilla(SisProcesosProcesamiento pParametro,ref DataSet dataSet)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            Hashtable listaParametros = new Hashtable();
            string sp = string.Empty;
            //DataSet dataSet = new DataSet();
            int tiempoEspera = Convert.ToInt32(TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.DBTiempoEjecucionProcesosDatos).ParametroValor);

            listaParametros.Add("IdProceso", pParametro.Proceso.IdProceso);
            listaParametros.Add("NombreArchivo", pParametro.Proceso.ProcesoArchivo.NombreArchivo);
            listaParametros.Add("Path", pParametro.Proceso.ProcesoArchivo.Path);
            listaParametros.Add("IdUsuarioEvento", pParametro.UsuarioLogueado.IdUsuarioEvento.ToString());

            foreach (SisParametros parametro in pParametro.Proceso.Parametros)
            {
                //listaParametros.Add(sisParam.Parametro, sisParam.ValorParametro);
                switch (parametro.TipoParametro.IdTipoParametro)
                {
                    case (int)EnumSisTipoParametros.Int:
                    case (int)EnumSisTipoParametros.DropDownListSPAutoComplete:
                        listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(int)));
                        //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(int));
                        break;
                    case (int)EnumSisTipoParametros.CheckBoxList:
                    case (int)EnumSisTipoParametros.GridViewCheckFecha:
                    case (int)EnumSisTipoParametros.GridViewCheckDinamico:
                    case (int)EnumSisTipoParametros.GridViewCheckImporte:
                        listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : parametro.ValorParametro);
                        //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : parametro.ValorParametro;
                        break;
                    case (int)EnumSisTipoParametros.DateTime:
                        //listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : (object)(Convert.ToDateTime(parametro.ValorParametro).ToString("s")));
                        listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime)));
                        //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime));
                        break;
                    case (int)EnumSisTipoParametros.TextBox:
                        listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : parametro.ValorParametro);
                        //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : parametro.ValorParametro;
                        break;
                    case (int)EnumSisTipoParametros.IntNumericInput:
                        listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(Int64)));
                        //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(int));
                        break;
                    case (int)EnumSisTipoParametros.DateTimeRange:
                        listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime)));
                        //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime));
                        break;
                    case (int)EnumSisTipoParametros.YearMonthCombo:
                        listaParametros.Add(parametro.Parametro, parametro.ValorParametro.ToString() == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime)));
                        //dbCommand.Parameters[string.Concat("@", parametro.Parametro)].Value = parametro.ValorParametro == string.Empty ? DBNull.Value : Convert.ChangeType(parametro.ValorParametro, typeof(DateTime));
                        break;
                    default:
                        break;
                }
            }

            try
            {
                DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database db = factory.CreateDefault();
                DbCommand dbCommand = db.GetStoredProcCommand(pParametro.Proceso.ProcesoArchivo.StoredProcedure);
                db.DiscoverParameters(dbCommand);
                Servicio.AccesoDatos.Mapeador.MapearEntidadParametros(listaParametros, dbCommand);

                if (tiempoEspera > dbCommand.CommandTimeout)
                    dbCommand.CommandTimeout = tiempoEspera;

                if (dbCommand.Parameters.Contains("@IdUsuarioEvento"))
                {
                    if (pParametro.GetType().GetProperty("UsuarioLogueado") != null)
                        dbCommand.Parameters["@IdusuarioEvento"].Value = pParametro.GetType().GetProperty("UsuarioLogueado").GetValue(pParametro, null).GetType().GetProperty("IdUsuarioEvento").GetValue(pParametro.GetType().GetProperty("UsuarioLogueado").GetValue(pParametro, null), null);
                }
                dataSet = db.ExecuteDataSet(dbCommand);

                pParametro.ErrorAccesoDatos = false;
                pParametro.CodigoMensaje = "ResultadoTransaccion";
                return true;
            }
            catch (Exception ex)
            {
                string msgError = string.Empty;
                if (ex is SqlException)
                {
                    SqlErrorCollection erroCol = ((SqlException)ex).Errors;
                    if (erroCol.Count > 0)
                        msgError = erroCol[0].Message;
                }
                pParametro.ErrorAccesoDatos = true;
                pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                pParametro.ErrorException = ex.Message;
                pParametro.CodigoMensajeArgs.Add(msgError == string.Empty ? ex.Message : msgError);
                return false;
            }
        }

  
    }

}
