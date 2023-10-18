using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elecciones.Entidades
{
    [Serializable]
    public class EleListasEleccionesPostulantes : Objeto
    {
        int _idListaEleccionPostulante;
        int  _idListaEleccion;
        int _idAfiliado;
        string _afiliado;
        int _idPuesto;

        [PrimaryKey]
        public int IdListaEleccionPostulante { get => _idListaEleccionPostulante; set => _idListaEleccionPostulante = value; }
        public int IdAfiliado { get => _idAfiliado; set => _idAfiliado = value; }
        public string Afiliado { get => _afiliado; set => _afiliado = value; }
        public int IdPuesto { get => _idPuesto; set => _idPuesto = value; }
        public string Puesto { get; set; }
        public int IdListaEleccion { get => _idListaEleccion; set => _idListaEleccion = value; }
    }
}
