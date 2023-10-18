using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Expedientes.Entidades;
using Expedientes.LogicaNegocio;
using Comunes.Entidades;

namespace Expedientes
{
    public class ExpedientesF
    {
        public static ExpExpedientes ExpedientesObtenerDatosCompletos(ExpExpedientes pParametro)
        {
            return new ExpExpedientesLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<ExpExpedientes> ExpedientesObtenerListaFiltro(ExpExpedientes pParametro)
        {
            return new ExpExpedientesLN().ObtenerListaFiltro(pParametro);
        }

        public static bool ExpedientesValidarExpedientesPendientes(Usuarios pUsuario)
        {
            return new ExpExpedientesLN().ValidarExpedientesPendientes(pUsuario);
        }

        /// <summary>
        /// Obtiene una lista de Filiales posibles de Derivar
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static List<TGEFiliales> ExpedientesObtenerFilialesDerivar(ExpExpedientes pParametro, TGEFiliales pFilial, TGESectores pSector)
        {
            return new ExpExpedientesLN().ExpedientesObtenerFilialesDerivar(pParametro, pFilial, pSector);
        }

        /// <summary>
        /// Obtiene la lista de Sectores posibles de Derivar
        /// </summary>
        /// <param name="pParametro">Expediente</param>
        /// <param name="pFilial">Filial a Derivar</param>
        /// <returns></returns>
        public static List<TGESectores> ExpedientesObtenerSectoresDerivar(ExpExpedientes pParametro, TGEFiliales pFilial)
        {
            return new ExpExpedientesLN().ExpedientesObtenerSectoresDerivar(pParametro, pFilial);
        }

        public static bool ExpedientesAgregar(ExpExpedientes pParametro, TGESectores pSector)
        {
            return new ExpExpedientesLN().Agregar(pParametro, pSector);
        }

        public static bool ExpedientesModificar(ExpExpedientes pParametro)
        {
            return new ExpExpedientesLN().Modificar(pParametro);
        }

        public static bool ExpedientesAceptarDerivaciones(Objeto pResultado, List<ExpExpedientes> pLista, TGESectores pSector)
        {
            return new ExpExpedientesLN().AceptarDerivaciones(pResultado, pLista, pSector);
        }

        public static bool ExpedientesAceptarDerivacion(ExpExpedientes pParametro, TGESectores pSector)
        {
            return new ExpExpedientesLN().AceptarDerivacion(pParametro, pSector);
        }

        public static bool ExpedientesRechazarDerivacion(ExpExpedientes pParametro, TGESectores pSector)
        {
            return new ExpExpedientesLN().RechazarDerivacion(pParametro, pSector);
        }
    }
}
