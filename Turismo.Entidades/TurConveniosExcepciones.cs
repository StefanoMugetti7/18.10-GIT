using Comunes.Entidades;
using System;

namespace Turismo.Entidades
{
    [Serializable]
    public class TurConveniosExcepciones : Objeto
    {
        int _idConvenioExcepcion;
        DateTime _fechaExcepcion;
        int _cantidadPlazasExcepcion;
        [PrimaryKey]
        public int IdConvenioExcepcion { get => _idConvenioExcepcion; set => _idConvenioExcepcion = value; }
        public DateTime FechaExcepcion { get => _fechaExcepcion; set => _fechaExcepcion = value; }
        public int CantidadPlazasExcepcion { get => _cantidadPlazasExcepcion; set => _cantidadPlazasExcepcion = value; }
    }
}
