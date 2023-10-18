using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elecciones.Entidades
{
    [Serializable]
    public class EleListasEleccionesAvales : Objeto
    {
        int _idListaEleccionAval;
        int _idAfiliado;
        int _idListaEleccion;
        string _afiliado;

        [PrimaryKey]
        public int IdListaEleccionAval { get => _idListaEleccionAval; set => _idListaEleccionAval = value; }
        public int IdAfiliado { get => _idAfiliado; set => _idAfiliado = value; }
        public string Afiliado { get => _afiliado; set => _afiliado = value; }
        public int IdListaEleccion { get => _idListaEleccion; set => _idListaEleccion = value; }
    }
}
