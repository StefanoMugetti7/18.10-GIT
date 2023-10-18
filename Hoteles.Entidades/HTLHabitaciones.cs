using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hoteles.Entidades
{
    [Serializable]
    public class HTLHabitaciones : Objeto
    {
        [PrimaryKey]
        public Int64 IdHabitacion { get; set; }
        public int IdHotel { get; set; }
        public Int64? IdProducto { get; set; }
        [Auditoria]
        public string NumeroHabitacion { get; set; }
        [Auditoria]
        public string NombreHabitacion { get; set; }
        [Auditoria]
        public string Piso { get; set; }

        public int IdUsuarioAlta { get; set; }

        public DateTime FechaAlta { get; set; }

        public int Cantidad { get; set; }

        List<TGECampos> _campos;
        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }
        }

        List<HTLHabitacionesDetalles> _habitacionesDetalles;
        public List<HTLHabitacionesDetalles> HabitacionesDetalles
        {
            get { return _habitacionesDetalles == null ? (_habitacionesDetalles = new List<HTLHabitacionesDetalles>()) : _habitacionesDetalles; }
            set { _habitacionesDetalles = value; }
        }

    }
}
