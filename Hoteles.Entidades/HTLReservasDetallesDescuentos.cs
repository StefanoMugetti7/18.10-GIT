using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hoteles.Entidades
{
    [Serializable]
    public class HTLReservasDetallesDescuentos : Objeto
    {
        [PrimaryKey]
        public Int64 IdReservaDetalleDescuento { get; set; }
        public Int64 IdReservaDetalle { get; set; }
        public Int64 IdHabitacion { get; set; }

        public int IdDescuento { get; set; }

        public string Detalle { get; set; }

        public decimal DescuentoPorcentaje { get; set; }

        public decimal DescuentoImporte { get; set; }

        public decimal PrecioBase { get; set; }
        public decimal BaseCalculoImporte { get; set; }

        public decimal Cantidad { get; set; }
        public decimal SubTotal { get; set; }

        HTLTiposDescuentos _tipoDescuento;
        public HTLTiposDescuentos TipoDescuento
        {
            get { return _tipoDescuento == null ? (_tipoDescuento = new HTLTiposDescuentos()) : _tipoDescuento; }
            set { _tipoDescuento = value; }
        }
    }
}
