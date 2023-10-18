using Elecciones.Entidades;
using Elecciones.LogicaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elecciones
{
    public class EleccionesF
    {
        #region POSTULANTES

        public static EleListasEleccionesPostulantes EleccionesObtenerDatosPostulante(EleListasEleccionesPostulantes pParametro)
        {
            return new ListasEleccionesPostulantesLN().ObtenerDatosPostulante(pParametro);
        }
        public static DataTable ListasEleccionesPostulantesObtenerPuestos(EleListasElecciones pParametro)
        {
            return new ListasEleccionesPostulantesLN().ObtenerPuestos(pParametro);
        }


        #endregion
        #region ELECCION
        public static DataTable EleccionesObtenerEtapas(EleElecciones pParametro)
        {
            return new EleccionesLN().ObtenerEtapas(pParametro);
        }
        public static EleElecciones EleccionesObtenerDatosCompletos(EleElecciones pParametro)
        {
            return new EleccionesLN().ObtenerDatosCompletos(pParametro);
        }
        public static bool EleccionesAgregar(EleElecciones pParametro)
        {
            return new EleccionesLN().Agregar(pParametro);
        }

        public static bool EleccionesModificar(EleElecciones pParametro)
        {
            return new EleccionesLN().Modificar(pParametro);
        }

        public static DataTable EleccionesObtenerListaGrilla(EleElecciones pParametro)
        {
            return new EleccionesLN().ObtenerListaGrilla(pParametro);
        }

        public static EleElecciones EleccionesObtenerEleccionVigente(EleElecciones pParametro)
        {
            return new EleccionesLN().ObtenerEleccionVigente(pParametro);
        }
        #endregion
        #region LISTAS ELECCIONES

        public static EleListasElecciones ListasEleccionesObtenerDatosCompletos(EleListasElecciones pParametro)
        {
            return new ListasEleccionesLN().ObtenerDatosCompletos(pParametro);
        }
        public static bool ListasEleccionesAgregar(EleListasElecciones pParametro)
        {
            return new ListasEleccionesLN().Agregar(pParametro);
        }

        public static bool ListasEleccionesModificar(EleListasElecciones pParametro)
        {
            return new ListasEleccionesLN().Modificar(pParametro);
        }

        public static bool ListasEleccionesAutorizar(EleListasElecciones pParametro)
        {
            return new ListasEleccionesLN().Autorizar(pParametro);
        }
        public static List<EleElecciones> ListasEleccionesObtenerElecciones()
        {
            return new ListasEleccionesLN().ObtenerElecciones();
        }
        public static List<EleListasElecciones> ListasEleccionesObtenerListasEleccionesRegionalesSinRepresentantes(EleListasElecciones pParametro)
        {
            return new ListasEleccionesLN().ObtenerListasEleccionesRegionalesSinRepresentantes(pParametro);
        }
        public static DataTable ListasEleccionesObtenerListaGrilla(EleListasElecciones pParametro)
        {
            return new ListasEleccionesLN().ObtenerListaGrilla(pParametro);
        }

        public static DataTable ListasEleccionesObtenerRegiones(EleListasElecciones pParametro)
        {
            return new ListasEleccionesLN().ObtenerRegiones(pParametro);
        }

        public static DataTable EleccionesObtenerResultadosVotacion(EleListasElecciones pParametro)
        {
            return new EleccionesLN().ObtenerResultadosVotacion(pParametro);
        }

        public static DataTable ListasEleccionesObtenerListasNacionalesDT(EleListasElecciones pParametro)
        {
            return new ListasEleccionesLN().ObtenerListaGrillaNacionalesDT(pParametro);
        }

        public static DataTable ListasEleccionesObtenerListasRegionalesDT(EleListasElecciones pParametro)
        {
            return new ListasEleccionesLN().ObtenerListaGrillaRegionalesDT(pParametro);
        }
        public static DataTable ListasEleccionesObtenerListasRepresentantesDT(EleListasElecciones pParametro)
        {
            return new ListasEleccionesLN().ObtenerListaGrillaRepresentantesDT(pParametro);
        }
        public static List<EleListasElecciones> ListasEleccionesObtenerListasNacionales(EleListasElecciones pParametro)
        {
            return new ListasEleccionesLN().ObtenerListaGrillaNacionales(pParametro);
        }

        public static List<EleListasElecciones> ListasEleccionesObtenerListasRegionales(EleListasElecciones pParametro)
        {
            return new ListasEleccionesLN().ObtenerListaGrillaRegionales(pParametro);
        }
        public static List<EleListasElecciones> ListasEleccionesObtenerListasRepresentantes(EleListasElecciones pParametro)
        {
            return new ListasEleccionesLN().ObtenerListaGrillaRepresentantes(pParametro);
        }
        public static DataTable ListasEleccionesObtenerRepresentantesPopUp(EleListasElecciones pParametro)
        {
            return new ListasEleccionesLN().ObtenerRepresentantesPopUp(pParametro);
        }

        #endregion
        #region VOTOS
        public static bool EleccionesValidarVotacion(EleElecciones pParametro)
        {
            return new EleccionesVotosLN().ValidarVotacion(pParametro);
        } 
        public static bool EleccionesValidarVotacionConfirmar(EleElecciones pParametro)
        {
            return new EleccionesVotosLN().ValidarVotacionConfirmar(pParametro);
        }

        public static bool EleccionesVotosAgregar(EleElecciones pParametro)
        {
            return new EleccionesVotosLN().AgregarVotos(pParametro);
        }

  

        #endregion
    }
}
