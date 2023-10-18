using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hoteles.Entidades
{
    [Serializable]
    public  class HTLDescuentos : Objeto
    {
        [PrimaryKey]
        public int IdDescuento { get; set; }
        [Auditoria]
        public int IdTipoDescuento { get; set; }
        public string Descripcion { get; set; }
        [Auditoria]
        public int IdHotel { get; set; }
        public string Hotel { get; set; }
        [Auditoria]
        public decimal DescuentoPorcentaje { get; set; }
        [Auditoria]
        public decimal DescuentoImporte { get; set; }

        public decimal PrecioBase { get; set; }

        [Auditoria]
        public decimal Frecuencia { get; set; }
        [Auditoria]
        public decimal PeriodoFrecuencia { get; set; }
        public Int64? DescuentoDetalle { get; set; }
        public decimal Cantidad { get; set; }
        //HTLHoteles _hotel;
        //public HTLHoteles Hotel {
        //    get { return _hotel == null ? (_hotel = new HTLHoteles()) : _hotel; }
        //    set { _hotel = value; }
        //}

       

    }



    [Serializable]
    public class HTLDescuentosFiltros : HTLDescuentos
    {
        public Int64 IdReserva { get; set; }
        public int IdAfiliado { get; set; }
        public int IdAfiliadoTipo { get; set; }
        public DateTime FechaIngreso { get; set; }
    }

    [Serializable]
    public class HTLDescuentosDTO
    {
        public int IdDescuento { get; set; }
        [Auditoria]
        public int IdTipoDescuento { get; set; }
        public string Descripcion { get; set; }
        [Auditoria]
        public int IdHotel { get; set; }
        [Auditoria]
        public decimal DescuentoPorcentaje { get; set; }
        [Auditoria]
        public decimal DescuentoImporte { get; set; }

        public decimal PrecioBase { get; set; }

        public decimal Cantidad { get; set; }
    }
}
