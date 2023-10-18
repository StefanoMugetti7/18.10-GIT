using Afiliados.Entidades;
using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afiliados.LogicaNegocio
{
    class AfiGruposSanguineosLN : BaseLN<AfiGruposSanguineos>
    {
    
        public override AfiGruposSanguineos ObtenerDatosCompletos(AfiGruposSanguineos pParametro)
        {
            throw new NotImplementedException();
        }

        public override List<AfiGruposSanguineos> ObtenerListaFiltro(AfiGruposSanguineos pParametro)
        {
            throw new NotImplementedException();
        }

        public List<AfiGruposSanguineos> ObtenerListaActiva()
        {
            List<AfiGruposSanguineos> lista = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiGruposSanguineos>("AfiGruposSanguineosListar");
            return AyudaProgramacionLN.ReacomodarIndicesColecion<AfiGruposSanguineos>(lista);
        }


        public override bool Agregar(AfiGruposSanguineos pParametro)
        {
            throw new NotImplementedException();
        }

        public override bool Modificar(AfiGruposSanguineos pParametro)
        {
            throw new NotImplementedException();
        }
    }
}
