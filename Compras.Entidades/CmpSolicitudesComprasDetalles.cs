using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Generales.Entidades;

namespace Compras.Entidades
{
    [Serializable]
    public partial class CmpSolicitudesComprasDetalles : Objeto
    {

        #region "Private Members"
        int _idSolicitudCompraDetalle;
        int _idSolicitudCompra;
        TGEMonedas _moneda;
        int? _idOrdenCompraDetalle;
        CMPProductos _producto;
        int _cantidad;
        string _descripcion;
        decimal _precioUnitario;
        decimal _cotizacionMoneda;
        int? _plazoEntrega;
        int _idUsuarioEvento;
        int _idIVA;
        decimal _alicuotaIVA;
        int? _idCotizacionDetalle;
        #endregion

        #region "Constructors"
        public CmpSolicitudesComprasDetalles()
        {
        }

        #endregion

        #region "Public Properties"
        public int IdSolicitudCompraDetalle
        {
            get { return _idSolicitudCompraDetalle; }
            set { _idSolicitudCompraDetalle = value; }
        }

        public int IdSolicitudCompra
        {
            get { return _idSolicitudCompra; }
            set { _idSolicitudCompra = value; }
        }

        public TGEMonedas Moneda
        {
            get { return _moneda == null ? (_moneda = new TGEMonedas()) : _moneda ; }
            set { _moneda = value; } 
        }

        /*
         public CmpOrdenCompradetalle
         * {
         *      get{return _ordenCompraDetalle == null ? (_ordenCompraDetalle = new CmpOrdenCompradetalle()) : _ordenCompraDetalle;}
         *      set{_ordenCompraDetalle = value;}
         * }         
         */
        public int? IdOrdenCompraDetalle
        {
            get { return _idOrdenCompraDetalle; }
            set { _idOrdenCompraDetalle = value; }
        }

        public CMPProductos Producto
        {
            get { return _producto == null ? (_producto = new CMPProductos()) : _producto; }
            set { _producto = value; }
        }

        public int Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }

        public string Descripcion
            {
            get {return _descripcion;}
            set { _descripcion = value;}
        }

        public decimal PrecioUnitario
        {
            get {return _precioUnitario;}
            set { _precioUnitario = value;}
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

        public int IdUsuarioEvento
        {
            get {return _idUsuarioEvento;}
            set { _idUsuarioEvento = value;}
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

        public int? IdCotizacionDetalle
        {
            get { return _idCotizacionDetalle; }
            set { _idCotizacionDetalle = value; } 
        }

        
        #region Calculos

        public decimal Subtotal
        {
            get { return (_precioUnitario * _cantidad); }
        }

        public decimal ImporteIvaItem
        {
            get { return Subtotal * _alicuotaIVA / 100; }
        }

        public decimal PrecioTotalItem
        {
            get { return Subtotal + ImporteIvaItem; }
        }
        #endregion
        #endregion
    }
}
