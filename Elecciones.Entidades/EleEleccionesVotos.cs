using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elecciones.Entidades
{
    [Serializable]
    public class EleEleccionesVotos : Objeto
    {
        int _idEleccionVoto;
        int _idAfiliado;
        int _idListaEleccion;
        int _idRegion;
        int _idEstado;

        [PrimaryKey]
        public int IdEleccionVoto { get => _idEleccionVoto; set => _idEleccionVoto = value; }
        public int IdAfiliado { get => _idAfiliado; set => _idAfiliado = value; }
        public int IdListaEleccion { get => _idListaEleccion; set => _idListaEleccion = value; }
        public int IdRegion{ get => _idRegion; set => _idRegion = value; }
        public int IdEstado { get => _idEstado; set => _idEstado = value; }
    }
}
