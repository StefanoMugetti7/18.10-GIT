using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hoteles.Entidades
{
    [Serializable]
    public class HTLReservasOcupantes : Objeto
    {
        [PrimaryKey]
        public Int64 IdReservaOcupante { get; set; }

        public Int64 IdReserva { get; set; }
        [Auditoria]
        public Int64? IdAfiliado { get; set; }
        [Auditoria]
        public int? IdSexo { get; set; }
        [Auditoria]
        public int? IdTipoDocumento { get; set; }
        [Auditoria]
        public string NumeroDocumento { get; set; }
        [Auditoria]
        public string Apellido { get; set; }
        [Auditoria]
        public string Nombre { get; set; }
        [Auditoria]
        public DateTime? FechaNacimiento { get; set; }
        [Auditoria]
        public Int64? IdReservaDetalle { get; set; }
        [Auditoria]
        public int? EdadFechaSalida { get; set; }
        [Auditoria]
        public Int64? IdHabitacion { get; set; }
        [Auditoria]
        public string DetalleHabitacion { get; set; }

    }
}
