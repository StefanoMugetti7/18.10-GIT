using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Hoteles.Entidades
{
    [Serializable]
    public class HTLReservasDetalles : Objeto
    {
        //Int64? _idProducto;
        //int? _idHabitacion;
        //string _detalle;

        [PrimaryKey]
        public Int64 IdReservaDetalle { get; set; }
        public Int64 IdReserva { get; set; }
        [Auditoria]
        public DateTime? FechaIngreso { get; set; }
        [Auditoria]
        public DateTime? FechaEgreso { get; set; }
        [Auditoria]
        public int? IdTipoProductoHotel { get; set; }
        [Auditoria]
        public Int64? IdProducto { get; set; }
        //public Int64? IdProducto {
        //    get { return _idProducto; }
        //    set { _idProducto = value; }
        //}
        [Auditoria]
        public decimal Cantidad { get; set; }
        [Auditoria]
        public Int64? IdListaPrecioDetalle { get; set; }
        [Auditoria]
        public decimal Precio { get; set; }
        [Auditoria]
        public bool PrecioEditable { get; set; }
        [Auditoria]
        public decimal DescuentoPorcentaje { get; set; }
        [Auditoria]
        public decimal DescuentoImporte { get; set; }
        [Auditoria]
        public Int64? IdHabitacion { get; set; }
        HTLHabitacionesDetalles _habitacionDetalle;
        [Auditoria]
        public HTLHabitacionesDetalles HabitacionDetalle{ get { return _habitacionDetalle==null ? (_habitacionDetalle=new HTLHabitacionesDetalles()):_habitacionDetalle; } set {_habitacionDetalle=value; } }
        [Auditoria]
        public string Detalle { get; set; }
        [Auditoria]
        public decimal SubTotal { get; set; }
        /// <summary>
        /// Se utiliza para la busqueda de Gastos/Habitaciones
        /// </summary>
        public int IdHotel { get; set; }

        /// <summary>
        /// Se utiliza para la busqueda de Gastos/Habitaciones
        /// </summary>
        public int IdAfiliado { get; set; }

        /// <summary>
        /// Se utiliza para la busqueda de Gastos/Habitaciones
        /// </summary>
        public int IdListaPrecio { get; set; }
        [Auditoria]
        public bool Compartida { get; set; }

        public decimal PrecioHabitacionCompartida { get; set; }

        public bool? LateCheckOut { get; set; }

        public int CantidadPersonas { get; set; }

        public int? CantidadPersonasOpciones { get; set; }

        XmlDocumentSerializationWrapper _loteCamposValores;
        public XmlDocument LoteCamposValores
        {
            get { return _loteCamposValores; }
            set { _loteCamposValores = value; }
        }

        List<HTLReservasDetallesDescuentos> _reservasDetallesDescuentos;
        public List<HTLReservasDetallesDescuentos> ReservasDetallesDescuentos
        {
            get { return _reservasDetallesDescuentos == null ? (_reservasDetallesDescuentos = new List<HTLReservasDetallesDescuentos>()) : _reservasDetallesDescuentos; }
            set { _reservasDetallesDescuentos = value; }
        }

        public string DetalleOpciones
        {
            get {
                return string.Concat(this.HabitacionDetalle.IdHabitacionDetalle > 0 ? string.Concat("Compartida: ", HabitacionDetalle.Descripcion) : string.Empty,
                    ((HabitacionDetalle.IdHabitacionDetalle > 0 && CantidadPersonasOpciones.HasValue && CantidadPersonasOpciones.Value > 1 ) || (HabitacionDetalle.IdHabitacionDetalle > 0 && LateCheckOut.HasValue && LateCheckOut.Value)) ? string.Concat("<br>") : string.Empty,
                  CantidadPersonasOpciones.HasValue && CantidadPersonasOpciones.Value > 1 ? string.Concat("Cantidad Personas: ", CantidadPersonasOpciones.Value.ToString()) : string.Empty,
                  (CantidadPersonasOpciones.HasValue && CantidadPersonasOpciones.Value > 1 && LateCheckOut.HasValue && LateCheckOut.Value) ? string.Concat("<br>") : string.Empty,
                  LateCheckOut.HasValue && LateCheckOut.Value ? " Late Checkout: Si" : string.Empty);
                }
            set {; }
        }
    }

    [Serializable]
    public class HTLReservasDetallesDTO
    {
        [PrimaryKey]
        public Int64 IdProducto { get; set; }
        public string Detalle { get; set; }
        public Int64 IdHabitacion { get; set; }
        public decimal Precio { get; set; }

        public Int64 IdListaPrecioDetalle { get; set; }

        public bool PrecioEditable { get; set; }

        public bool Compartida { get; set; }
        public decimal PrecioHabitacionCompartida { get; set; }

        public int CantidadPersonas { get; set; }

    }
}
