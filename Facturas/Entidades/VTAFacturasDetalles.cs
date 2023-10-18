using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generales.Entidades;
using Comunes.Entidades;
using Compras.Entidades;

namespace Facturas.Entidades
{
    public class VTAFacturasDetalles : Objeto
    {

        #region "Private Members"
       
        int _idItemFactura;
        int _idFactura;
        //int? _idItemRemito;
        //int? _idProducto;
        //string _descripcion;
        
        CMPProductos _producto;
        int? _cantidad;
        decimal? _precioUnitarioSinIva;
        //decimal? _aliCuotaIVA;
        decimal? _descuentoPorcentual;
        decimal? _descuentoImporte;
        decimal? _subTotal;
        decimal? _subTotalConIva;
        decimal? _importeIVA;
        TGEIVA _IVA;

        #endregion

        #region "Constructors"
	    public VTAFacturasDetalles()
	    {
	    }
	    #endregion
		
	    #region "Public Properties"

        [PrimaryKey()]
        public int IdItemFactura
        {
            get { return _idItemFactura; }
            set {_idItemFactura = value; }
        }

        public int IdFactura
        {
            get { return _idFactura;}
            set { _idFactura = value; }
        }

        //public int? IdItemRemito
        //{
        //    get { return _idItemRemito; }
        //    set { _idItemRemito = value; }
        //}

        //public int? IdProducto
        //{
        //    get { return _idProducto; }
        //    set { _idProducto = value; }
        //}

        //public string Descripcion
        //{
        //    get { return _descripcion; }
        //    set { _descripcion = value; }
        //}

        public int? Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }

        public decimal? PrecioUnitarioSinIva
        {
            get { return _precioUnitarioSinIva; }
            set { _precioUnitarioSinIva = value; }
        }

        //public decimal? AlicuotaIVA
        //{
        //    get { return _aliCuotaIVA; }
        //    set { _aliCuotaIVA = value; }
        //}

        public decimal? DescuentoPorcentual
        {
           // get { return _descuentoPorcentual == null ? 0 : _descuentoPorcentual; }
            get { return _descuentoPorcentual ; }
            set { _descuentoPorcentual = value; }
        }

        public decimal? DescuentoImporte
        {
           // get { return _descuentoImporte == null ? 0 : _descuentoImporte; }
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

        public CMPProductos Producto
        {
            get { return _producto == null ? (_producto = new CMPProductos()) : _producto; }
            set { _producto = value; }
        }

        public string DescripcionProducto
        {
            get { return this.Producto.Descripcion; }
            set { }
        }

        #endregion

    }
}
