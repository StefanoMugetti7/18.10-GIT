using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hoteles.Entidades
{
    [Serializable]
    public class HTLHabitacionesDetalles : Objeto
    {
        [PrimaryKey]
        public Int64? IdHabitacionDetalle { get; set; }

        public Int64 IdHabitacion { get; set; }
        [Auditoria]
        public string Descripcion { get; set; }
        HTLMoviliarios _moviliario;
        [Auditoria]
        public HTLMoviliarios Moviliario { get { return _moviliario == null ? (_moviliario = new HTLMoviliarios()) : _moviliario; } set { _moviliario = value; } }
    }
}
