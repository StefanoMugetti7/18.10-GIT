using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Haberes.Entidades;
//using EO.Web;
using Haberes.LogicaNegocio;
using Generales.Entidades;
using Reportes.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Net.Mail;
using System.Data;
using Cargos.Entidades;

namespace Haberes
{
    public class HaberesF
    {
        #region "RecibosCom"
        public static DataTable RecibosComSeleccionarGrilla(HabRecibosCom pParametro)
        {
            return new HabRecibosComLN().ObtenerRecibosGrilla(pParametro);
        }

        /// <summary>
        /// Devuelve el Saldo Actual de la Cuenta Corriente del Socio
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static HabRecibosCom HabRecibosComObtenerUltimoRecibo(HabRecibosCom pParametro)
        {
            return new HabRecibosComLN().ObtenerUltimoRecibo(pParametro);
        }

        public static List<HabRecibosCom> HabRecibosComObtenerListaFiltro(HabRecibosCom pParametro)
        {
            return new HabRecibosComLN().ObtenerListaFiltro(pParametro);
        }

        public static bool HabRecibosComAnular(HabRecibosCom pParametro)
        {
            return new HabRecibosComLN().Anular(pParametro);
        }

        public static bool HabRecibosComPagosAgregar(HabRecibosComPagos pParametro, Database bd, DbTransaction tran)
        {
            return new HabRecibosComLN().AgregarPagos(pParametro, bd, tran);
        }

        public static TGEArchivos HabRecibosReporteRecibosCOMIAF(RepReportes pReporte)
        {
            return new HabRecibosComLN().ReporteRecibosCOMIAF(pReporte);
        }
        
        public static bool HabRecibosArmarMail(HabRemesasDetalles pParametro, MailMessage mail)
        {
            return new HabRecibosComLN().ArmarMailReciboHaberes(pParametro, mail);
        }
        #endregion

        #region "ArchivosCabeceras"
        public static List<HabArchivosDetalles> ArchivosDetallesSeleccionar(HabArchivosDetalles pParametro)
        {
            return new HabArchivosCabecerasLN().ObtenerDatosCompletos(pParametro);
        }

        public static bool HaberesImportarPdfIaf(HabArchivosCabeceras pObjteo)
        {
            return new ImportarArchivosLN().ImportarPdfIaf(pObjteo);
        }
        #endregion

        #region "Remesas"
        public static HabRemesas RemesasObtenerDatosCompletos(HabRemesas pParametro)
        {
            return new HabRemesasLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<HabRemesasDetalles> RemesasDetallesObtenerListaFiltro(HabRemesasDetalles pParametro)
        {
            return new HabRemesasLN().ObtenerListaFiltro(pParametro);
        }

        public static List<HabRemesasDetalles> RemesasDetallesObtenerPendienteEnvio(HabRemesasDetalles pParametro)
        {
            return new HabRemesasLN().ObtenerPendienteEnvio(pParametro);
        }

        public static List<HabRemesas> RemesasObtenerLista(HabRemesas pParametro)
        {
            return new HabRemesasLN().ObtenerListaFiltro(pParametro);
        }

        public static bool RemesasProcesarDepostio(HabRemesas pParametro)
        {
            return new HabRemesasLN().Modificar(pParametro);
        }

        public static bool RemesasProcesarCierre(HabRemesas pParametro)
        {
            return new HabRemesasLN().Cierre(pParametro);
        }

        public static bool RemesasDetallesModificar(HabRemesasDetalles pParametro)
        {
            return new HabRemesasLN().Modificar(pParametro);
        }
        #endregion

        #region "Fondo Sumplementario"
        public static bool FondoSuplementarioAgregar(HabFondoSuplementario pParametro)
        {
            return new HabFondoSuplementarioLN().Agregar(pParametro);
        }

        public static bool FondoSuplementarioModificar(HabFondoSuplementario pParametro)
        {
            return new HabFondoSuplementarioLN().Modificar(pParametro);
        }
        public static HabFondoSuplementario FondoSuplementarioObtenerDatosCompletos(HabFondoSuplementario pParametro)
        {
            return new HabFondoSuplementarioLN().ObtenerDatosCompletos(pParametro);
        }
        public static DataTable FondosSuplementariosCalcularImporteJubilacion(HabFondoSuplementario pParametro)
        {
            return new HabFondoSuplementarioLN().CalcularImporteJubilacion(pParametro);
        }

        public static bool FondoSuplementarioValidacionesCalculos(HabFondoSuplementario pParametro)
        {
            return new HabFondoSuplementarioLN().ValidacionesCalculos(pParametro);
        }

        public static DataTable FondoSuplementarioObtenerDatosCompletosDataTable(HabFondoSuplementario pParametro)
        {
            return new HabFondoSuplementarioLN().ObtenerDatosCompletosDataTable(pParametro);
        }

        public static List<HabFondoSuplementario> FondoSuplementarioObtenerDatosListaFiltro(HabFondoSuplementario pParametro)
        {
            return new HabFondoSuplementarioLN().ObtenerListaFiltro(pParametro);
        }
        public static List<CarCuentasCorrientes> HaberesSumplementariosObtenerCargosPendientes(CarCuentasCorrientes pParametro)
        {
            return new HabFondoSuplementarioLN().ObtenerCargosPendientesCobro(pParametro);
        }

        public static bool HaberesSuplemetariosEliminarAporte(HabSueldoHaberes pParametro)
        {
            return new HabFondoSuplementarioLN().EliminarAporte(pParametro);
        }

        public static DataTable FondoSuplementarioObtenerPlantilla(HabFondoSuplementario pParametro)
        {
            return new HabFondoSuplementarioLN().ObtenerPlantilla(pParametro);
        }
        #endregion

        #region "Aportes Rangos"
        public static bool AportesRangosAgregar(HabSueldosAportesRangos pParametro)
        {
            return new HabSueldosAportesRangosLN().Agregar(pParametro);
        }

        public static bool AportesRangosModificar(HabSueldosAportesRangos pParametro)
        {
            return new HabSueldosAportesRangosLN().Modificar(pParametro);
        }
        public static HabSueldosAportesRangos AportesRangosObtenerDatosCompletos(HabSueldosAportesRangos pParametro)
        {
            return new HabSueldosAportesRangosLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<HabSueldosAportesRangos> AportesRangosObtenerDatosListaFiltro(HabSueldosAportesRangos pParametro)
        {
            return new HabSueldosAportesRangosLN().ObtenerListaFiltro(pParametro);
        }

        public static DataTable AportesRangosObtenerDatosDataTable(HabSueldosAportesRangos pParametro)
        {
            return new HabSueldosAportesRangosLN().ObtenerDataTable(pParametro);
        }
        #endregion
    }
}
