using Afiliados.Entidades;
using Comunes.Entidades;
using Proveedores.Entidades;
using Seguridad.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Prestamos.Entidades
{
    [Serializable]
    public class PrePrestamosLotes : Objeto
    {
        #region "Private Members"
        CapProveedores _proveedor;
        //List<PrePrestamosLotesDetalles> _prePrestamosLotesDetalles;
        AfiAfiliados _afiliado;
        PrePrestamos _prestamo;
        XmlDocumentSerializationWrapper _lotePrestamosLotes;
        DataTable _detalleSeleccionado;
        #endregion

        #region "Constructors"
        public PrePrestamosLotes() { }
        #endregion

        #region "Public Properties"
        [PrimaryKey()]
        public int IdPrestamoLote { get; set; }

        public string Detalle { get; set; }

        public CapProveedores Proveedor
        {
            get { return _proveedor == null ? (_proveedor = new CapProveedores()) : _proveedor; }
            set { _proveedor = value; }
        }

        public decimal TasaInversor { get; set; }

        public decimal TasaInaes { get; set; }

        public DateTime FechaAlta { get; set; }

        public DateTime? FechaConfirmacion { get; set; }

        public int IdUsuarioAlta { get; set; }

        public int? IdUsuarioConfirmacion { get; set; }

        //public List<PrePrestamosLotesDetalles> PrestamosDetalles
        //{
        //    get { return _prePrestamosLotesDetalles == null ? (_prePrestamosLotesDetalles = new List<PrePrestamosLotesDetalles>()) : _prePrestamosLotesDetalles; }
        //    set { _prePrestamosLotesDetalles = value; }
        //}

        public AfiAfiliados Afiliado
        {
            get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; }
            set { _afiliado = value; }
        }

        public PrePrestamos Prestamo
        {
            get { return _prestamo == null ? (_prestamo = new PrePrestamos()) : _prestamo; }
            set { _prestamo = value; }
        }

        public bool Incluir { get; set; }

        public DateTime? FechaDesde { get; set; }

        public DateTime? FechaHasta { get; set; }

        public decimal? ImporteDesde { get; set; }

        public decimal? ImporteHasta { get; set; }

        public int? CantidadCuotas { get; set; }

        public XmlDocument LotePrestamosLotes
        {
            get { return _lotePrestamosLotes; }
            set { _lotePrestamosLotes = value; }
        }

        public DataTable DetalleSeleccionado
        {
            get { return _detalleSeleccionado == null ? (_detalleSeleccionado = new DataTable()) : _detalleSeleccionado; }
            set { _detalleSeleccionado = value; }
        }

        #endregion
    }
}
