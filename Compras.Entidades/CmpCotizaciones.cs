using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Proveedores.Entidades;

namespace Compras.Entidades
{
    [Serializable]
    public partial class CmpCotizaciones : Objeto
    {

        #region "Private Members" 
        
        int _idCotizacion;
        CapProveedores _proveedor;
        List<CmpCotizacionesDetalles> _cotizacionesDetalles;
        CmpCondicionesPagos _condicionPago;
        DateTime? _fechaRecibido;
        decimal? _descuentoTotal;
        string _descuentoDetalle;
        decimal? _costoFlete;
        string _observaciones;
        string _mail;
        DateTime _fechaAlta;
        int _idUsuarioAlta;
        int _idUsuarioEvento;
        DateTime _fechaEvento;
        DateTime? _fechaDesde;
        DateTime? _fechaHasta;
    
        #endregion

        #region "Constructors"
        public CmpCotizaciones()
        { 
        }
        #endregion

        #region "Public Properties" 
        [PrimaryKey()]
        public int IdCotizacion
        {
            get { return _idCotizacion; }
            set { _idCotizacion = value; }
        }

        public CapProveedores Proveedor
        {
            get {return _proveedor == null ? (_proveedor = new CapProveedores()) : _proveedor; }
            set { _proveedor = value; } 
        }

        public List<CmpCotizacionesDetalles> CotizacionesDetalles
        {
            get { return _cotizacionesDetalles == null ? (_cotizacionesDetalles = new List<CmpCotizacionesDetalles>()) : _cotizacionesDetalles; }
            set { _cotizacionesDetalles = value; }
        }

        public CmpCondicionesPagos CondicionPago
        {
            get { return _condicionPago == null ? (_condicionPago = new CmpCondicionesPagos()) : _condicionPago; }
            set { _condicionPago = value; }        
        }

        public DateTime? FechaRecibido
        {
            get { return _fechaRecibido; }
            set { _fechaRecibido = value; }
        }

        public decimal? DescuentoTotal
        {
            get { return _descuentoTotal; }
            set { _descuentoTotal = value; }
        }

        public string DescuentoDetalle
        {
            get { return _descuentoDetalle == null ? string.Empty : _descuentoDetalle; }
            set { _descuentoDetalle = value; }
        }

        public decimal? CostoFlete
        {
            get { return _costoFlete == null ? 0 : _costoFlete; }
            set { _costoFlete = value; }
        }

        public string Observaciones
        {
            get { return _observaciones == null ? string.Empty : _observaciones; }
            set { _observaciones = value; }
        }

        public string Mail
        {
            get { return _mail == null ? string.Empty : _mail; }
            set { _mail = value; }
        }

        public DateTime FechaAlta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }

        public int IdUsuarioAlta
        {
            get { return _idUsuarioAlta; }
            set { _idUsuarioAlta = value; }
        }

        public DateTime FechaEvento
        {
            get { return _fechaEvento; }
            set { _fechaEvento = value; }
        }

        public int IdUsuarioEvento
        {
            get { return _idUsuarioEvento; }
            set { _idUsuarioEvento = value; }
        }

        public DateTime? FechaDesde
        {
            get { return _fechaDesde; }
            set { _fechaDesde = value; }
        }

        public DateTime? FechaHasta
        {
            get { return _fechaHasta; }
            set { _fechaHasta = value; }
        }
        

        #endregion
    }
}
