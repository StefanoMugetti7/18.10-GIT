using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Generales.Entidades;
using Proveedores.Entidades;

namespace Compras.Entidades
{
    [Serializable]
    public partial class CmpCotizacionesDetalles : Objeto
    {
        #region "Private Members"

        int _idCotizacionDetalle;
        int _idCotizacion;
        TGEMonedas _moneda;
        CMPProductos _producto;
        decimal _cotizacionMoneda;
        int? _plazoEntrega;
        decimal _precioUnitario;
        decimal _precioCantidad;
        int _cantidad;
        decimal _alicuotaIVA;
        int _idIVA;
        decimal _descuento;
        string _razonSocial; //para ver nombre del proveedor en el PopUp

        #endregion

        #region "Constructors" 

        public CmpCotizacionesDetalles()
        { }

        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdCotizacionDetalle
        {
            get { return _idCotizacionDetalle; }
            set { _idCotizacionDetalle = value; }
        }

        public int IdCotizacion
        {
            get { return _idCotizacion; }
            set { _idCotizacion = value; }
        }


        public TGEMonedas Moneda
        {
            get { return _moneda == null ? (_moneda = new TGEMonedas()) : _moneda; }
            set { _moneda = value; }
        }

        public CMPProductos Producto
        {
            get { return _producto == null ? (_producto = new CMPProductos()) : _producto; }
            set { _producto = value; }
        }

        
        public decimal CotizacionMoneda
        {
            get {return _cotizacionMoneda;}
            set { _cotizacionMoneda = value;}
        }

        public int? PlazoEntrega
        {
            get {return _plazoEntrega;}
            set { _plazoEntrega = value;}
        }

        public decimal PrecioUnitario
        {
            get { return _precioUnitario; }
            set { _precioUnitario = value; }
        }

        public decimal PrecioCantidad
        {
            get { return _precioCantidad; }
            set { _precioCantidad = value; }
        }

        public int IdIVA
        {
            get { return _idIVA; }
            set { _idIVA = value; }
        }

        public decimal AlicuotaIVA
        {
            get { return _alicuotaIVA; }
            set { _alicuotaIVA = value; }
        }

        public int Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }

        public decimal Descuento
        {
            get { return _descuento; }
            set { _descuento = value; }
        }

        public string RazonSocial
        {
            get { return _razonSocial; }
            set { _razonSocial = value; }
        }
        #endregion



    }
}
