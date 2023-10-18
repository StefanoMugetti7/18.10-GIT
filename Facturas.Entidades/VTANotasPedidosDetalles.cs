
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Compras.Entidades;
using Generales.Entidades;
namespace Facturas.Entidades
{
  [Serializable]
	public partial class VTANotasPedidosDetalles : Objeto
	{
		// Class VTANotasPedidosDetalles
	#region "Private Members"
	    int _idNotaPedidoDetalle;
	    int _idNotaPedido;
        CMPListasPreciosDetalles _listaPrecioDetalle;
        decimal? _cantidad;
        decimal? _precioUnitarioSinIva;
        //decimal? _aliCuotaIVA;
        decimal? _descuentoPorcentual;
        decimal? _descuentoImporte;
        decimal? _subTotal;
        decimal? _subTotalConIva;
        decimal? _importeIVA;
        TGEIVA _IVA;
        string _descripcion;
        string _descripcionProducto;
        CMPProductos _producto;
        bool _incluir;
        TGEMonedas _moneda;
        bool _listaPrecio;
        int _idListaPrecioDetalle;
        int _idFacturaDetalle;
        int _idRemitoDetalle;
        int? _cantidadRemitada;
        #endregion

        #region "Constructors"
        public VTANotasPedidosDetalles()
	{
	}
	#endregion
		
	#region "Public Properties"
        [PrimaryKey()]
	    public int IdNotaPedidoDetalle
	    {
		    get{return _idNotaPedidoDetalle ;}
		    set{_idNotaPedidoDetalle = value;}
	    }
	    public int IdNotaPedido
	    {
		    get{return _idNotaPedido;}
		    set{_idNotaPedido = value;}
	    }
        public decimal? Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }
        public decimal? PrecioUnitarioSinIva
        {
            get { return _precioUnitarioSinIva; }
            set { _precioUnitarioSinIva = value; }
        }
        public decimal? DescuentoPorcentual
        {
            get { return _descuentoPorcentual; }
            set { _descuentoPorcentual = value; }
        }
        public decimal? DescuentoImporte
        {
            get { return _descuentoImporte; }
            set { _descuentoImporte = value; }
        }
        public decimal? SubTotal
        {
            get { return _subTotal; }
            set { _subTotal = value; }
        }
        public decimal? SubTotalConIva
        {
            get { return _subTotalConIva; }
            set { _subTotalConIva = value; }
        }
        public decimal? ImporteIVA
        {
            get { return _importeIVA; }
            set { _importeIVA = value; }
        }
        public TGEIVA IVA
        {
            get { return _IVA == null ? (_IVA = new TGEIVA()) : _IVA; }
            set { _IVA = value; }
        }
        public CMPListasPreciosDetalles ListaPrecioDetalle
        {
            get { return _listaPrecioDetalle == null ? (_listaPrecioDetalle = new CMPListasPreciosDetalles()) : _listaPrecioDetalle; }
            set { _listaPrecioDetalle = value; }
        }
        public string DescripcionProducto
        {
            get { return _descripcionProducto; }
            set { _descripcionProducto = value; }
        }
        public string Descripcion
        {
            get { return _descripcion == null ? string.Empty : _descripcion; }
            set { _descripcion = value; }
        }
        public CMPProductos Producto
        {
            get { return _producto == null ? (_producto = new CMPProductos()) : _producto; }
            set { _producto = value; }
        }

        public bool Incluir
        {
            get { return _incluir; }
            set { _incluir = value; }
        }

        public TGEMonedas Moneda
        {
            get { return _moneda == null ? (_moneda = new TGEMonedas()) : _moneda; }
            set { _moneda = value; }
        }

        public int IdListaPrecioDetalle
        {
            get { return _idListaPrecioDetalle; }
            set { _idListaPrecioDetalle = value; }
        }

        public bool ListaPrecio
        {
            get { return _listaPrecio; }
            set { _listaPrecio = value; }
        }

        public int IdFacturaDetalle
        {
            get { return _idFacturaDetalle; }
            set { _idFacturaDetalle = value; }
        }

        public int IdRemitoDetalle
        {
            get { return _idRemitoDetalle; }
            set { _idRemitoDetalle = value; }
        }

        public int? CantidadRemitada
        {
            get { return _cantidadRemitada; }
            set { _cantidadRemitada = value; }
        }
        #endregion
    }
}
