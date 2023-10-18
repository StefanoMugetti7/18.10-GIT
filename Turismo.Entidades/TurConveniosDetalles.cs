using Comunes.Entidades;
using System;

namespace Turismo.Entidades
{
    [Serializable]
    public class TurConveniosDetalles : Objeto
    {
        int _idConvenioDetalle;
        int _idTipoHabitacion;
        int _cantidad;

        [PrimaryKey]
        public int IdConvenioDetalle { get => _idConvenioDetalle; set => _idConvenioDetalle = value; }
        public int IdTipoHabitacion { get => _idTipoHabitacion; set => _idTipoHabitacion = value; }
        public string TipoHabitacion { get; set; }
        public int Cantidad { get => _cantidad; set => _cantidad = value; }
    }
}
