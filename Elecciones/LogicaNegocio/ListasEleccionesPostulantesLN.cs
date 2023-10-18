using Comunes;
using Elecciones.Entidades;
using Servicio.AccesoDatos;
using System.Collections.Generic;
using System.Data;

namespace Elecciones.LogicaNegocio
{
    public class ListasEleccionesPostulantesLN :BaseLN<EleListasEleccionesPostulantes>
    {
        public override bool Agregar(EleListasEleccionesPostulantes pParametro)
        {
            throw new System.NotImplementedException();
        }

        public override bool Modificar(EleListasEleccionesPostulantes pParametro)
        {
            throw new System.NotImplementedException();
        }

        public EleListasEleccionesPostulantes ObtenerDatosPostulante(EleListasEleccionesPostulantes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<EleListasEleccionesPostulantes>("ELEEleccionesObtenerDatosPostulante", pParametro);
        }

        public DataTable ObtenerPuestos(EleListasElecciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("ELEListasEleccionesPostulantesObtenerPuestos", pParametro);
        }

        public override EleListasEleccionesPostulantes ObtenerDatosCompletos(EleListasEleccionesPostulantes pParametro)
        {
            throw new System.NotImplementedException();
        }

        public override List<EleListasEleccionesPostulantes> ObtenerListaFiltro(EleListasEleccionesPostulantes pParametro)
        {
            throw new System.NotImplementedException();
        }
    }
}