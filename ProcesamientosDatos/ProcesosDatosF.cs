using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProcesosDatos.Entidades;
using ProcesosDatos.LogicaNegocio;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Comunes.Entidades;

namespace ProcesosDatos
{
    public class ProcesosDatosF
    {
        
        public static DataTable BackupsObtenerGrilla()
        {
            return new BackupsLN().ObtenerGrilla();
        }

        public static void BackupGenerar(Objeto pObjeto)
        {
            new BackupsLN().Backup(pObjeto);
        }

        /// <summary>
        /// Devuelve un Proceso del Sistema con sus parametros
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static SisProcesos ProcesosObtenerDatosCompletos(SisProcesos pParametro)
        {
            return new ProcesosDatosLN().ObtenerDatosCompletos(pParametro);
        }

        /// <summary>
        /// Obtiene el ultimo periodo procesado por periodo
        /// </summary>
        /// <param name="pParametro">IdSisProceso</param>
        /// <returns></returns>
        public static SisProcesosProcesamiento ProcesosProcesamientoObtenerUltimoPeriodoProcesado(SisProcesosProcesamiento pParametro)
        {
            return new ProcesosDatosLN().ObtenerUltimoPeriodoProcesado(pParametro);
        }

        /// <summary>
        /// Valida si un periodo tiene los cargos generados
        /// </summary>
        /// <param name="pPeriodo"></param>
        /// <returns></returns>
        public static bool ProcesosProcesamientoValidarCargosGenerados(int pPeriodo, Database bd, DbTransaction tran)
        {
            return new ProcesosDatosLN().ValidarCargosGenerados(pPeriodo, bd, tran);
        }

        public static bool ProcesosProcesamientoValidarCargosGenerados(int pPeriodo)
        {
            return new ProcesosDatosLN().ValidarCargosGenerados(pPeriodo);
        }

        public static SisProcesosProcesamiento ProcesosProcesamientoObtenerUltimoPeriodoProcesado(SisProcesosProcesamiento pParametro, Database bd, DbTransaction tran)
        {
            return new ProcesosDatosLN().ObtenerUltimoPeriodoProcesado(pParametro, bd, tran);
        }

        /// <summary>
        /// Devuelve una lista con los proximos Periodos de Cargos a Procesar
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static List<int> ProcesosProcesamientoObtenerProximosPeriodosCargos()
        {
            return new ProcesosDatosLN().ObtenerProximosPeriodosCargos();
        }

   
        /// <summary>
        /// Devuelve una lista con los ultimos Periodos de Cargos Procesados.
        /// Cantidad = Cantidad de Periodos a devolver.
        /// </summary>
        /// <param name="pParametro">Cantidad</param>
        /// <returns></returns>
        public static List<int> ProcesosProcesamientoObtenerUltimosPeriodosCargos(int cantidad)
        {
            return new ProcesosDatosLN().ObtenerUltimosPeriodosCargos(cantidad);
        }

        /// <summary>
        /// Devuelve una lista de Procesos Activos
        /// </summary>
        /// <returns></returns>
        public static List<SisProcesos> ProcesosObtenerLista()
        {
            return new ProcesosDatosLN().ObtenerLista();
        }

        /// <summary>
        /// Devuelve una lista de Procesos Activos
        /// </summary>
        /// <returns></returns>
        public static List<SisProcesos> ProcesosObtenerLista(Perfiles Perfil)
        {
            return new ProcesosDatosLN().ObtenerLista(Perfil);
        }

        /// <summary>
        /// Devuelve una lista de Procesos Activos por Usuario (Perfiles)
        /// </summary>
        /// <returns></returns>
        public static List<SisProcesos> ProcesosObtenerListaFiltro(SisProcesos pParametro)
        {
            return new ProcesosDatosLN().ObtenerListaFiltro(pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Procesos Ejecutados por Proceso
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static List<SisProcesosProcesamiento> ProcesosObtenerLista(SisProcesos pParametro)
        {
            return new ProcesosDatosLN().ObtenerLista(pParametro);
        }
        /// <summary>
        /// Devuelve un DataTable de Procesos Ejecutados por Proceso
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static DataTable ProcesosObtenerListaDT(SisProcesos pParametro)
        {
            return new ProcesosDatosLN().ObtenerListaDT(pParametro);
        }
        /// <summary>
        /// Devuelve un DataTable con los parametros de un Procesos Ejecutados por Proceso (Consulta)
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static DataTable ProcesosObtenerListaParametros(SisProcesosProcesamiento pParametro)
        {
            return new ProcesosDatosLN().ObtenerListaParametros(pParametro);
        }
        /// <summary>
        /// Obtiene los valores para un Parametro
        /// </summary>
        /// <param name="pProceso"></param>
        /// <returns></returns>
        public static DataSet ProcesosObtenerDatosParametro(SisProcesos pProceso)
        {
            return new ProcesosDatosLN().ObtenerDatosParametro(pProceso);
        }

        /// <summary>
        /// Ejecuta un Proceso en la Base de Datos
        /// No maneja Transacciones
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static bool ProcesosEjecutarProcesos(SisProcesosProcesamiento pParametro)
        {
            return new ProcesosDatosLN().EjecutarProcesos(pParametro);
        }

        /// <summary>
        /// Ejecuta un Proceso en la Base de Datos
        /// No maneja Transacciones
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static bool ProcesosEjecutarProcesos(SisProcesosProcesamiento pParametro, ref DataSet resultado)
        {
            return new ProcesosDatosLN().EjecutarProcesos(pParametro, ref resultado);
        }

        /// <summary>
        /// Ejecuta la primera parte del proceso y devuelve todos los detalles para poder ser modificados o consultados por pantalla
        /// </summary>
        public static bool ProcesosEjecutarProcesosObtenerGrilla(SisProcesosProcesamiento pParametro, ref DataSet resultado)
        {
            return new ProcesosDatosLN().EjecutarProcesosObtenerGrilla(pParametro,ref resultado);
        }

        public static SisProcesos ProcesosObtenerDatosCompletosPorProcesamiento(SisProcesosProcesamiento pParametro)
        {
            return new ProcesosDatosLN().ObtenerDatosCompletosPorProcesamiento(pParametro);
        }
    }
}
