using Comunes.Entidades;
using System;

namespace Elecciones.Entidades
{
    [Serializable]
    public class EleListasEleccionesApoderados : Objeto
    {
        int _idListaEleccionApoderado;
        int _idListaEleccion;
        string _afiliado;
        int _idAfiliado;
        [PrimaryKey]
        public int IdListaEleccionApoderado { get => _idListaEleccionApoderado; set => _idListaEleccionApoderado = value; }
        public int IdAfiliado { get => _idAfiliado; set => _idAfiliado = value; }
        public string Afiliado { get => _afiliado; set => _afiliado = value; }
        public int IdListaEleccion { get => _idListaEleccion; set => _idListaEleccion = value; }
    }
}
    