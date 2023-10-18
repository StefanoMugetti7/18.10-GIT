
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Compras.Entidades;
using Subsidios.Entidades;
namespace CuentasPagar.Entidades
{
  [Serializable]
	public partial class CapSolicitudPagoDetalles : Objeto
	{

	#region "Private Members"
	int _idSolicitudPagoDetalle;
	int _idSolicitudPago;
	string _descripcion;
	int _cantidad;
	decimal _precioUnitarioSinIva;
    decimal _descuentoImporte;
    decimal _alicuotaIVA;
    CMPProductos _producto;
    CMPItemRemitos _itemRemito;
    SubSubsidios _subsidios;
    bool _incluirEnSP;
    TGEFiliales _filial;
	#endregion
		
	#region "Constructors"
	public CapSolicitudPagoDetalles()
	{
	}
	#endregion
		
	#region "Public Properties"

      [PrimaryKey()]
	public int IdSolicitudPagoDetalle
	{
		get{return _idSolicitudPagoDetalle ;}
		set{_idSolicitudPagoDetalle = value;}
	}
	public int IdSolicitudPago
	{
		get{return _idSolicitudPago;}
		set{_idSolicitudPago = value;}
	}

	public string Descripcion
	{
		get{return _descripcion == null ? string.Empty : _descripcion ;}
		set{_descripcion = value;}
	}

	public int Cantidad
	{
		get{return _cantidad;}
		set{_cantidad = value;}
	}

	public decimal PrecioUnitarioSinIva
	{
		get{return _precioUnitarioSinIva;}
		set{_precioUnitarioSinIva = value;}
	}

    public decimal DescuentoImporte
    {
        get { return _descuentoImporte == null ? 0 : _descuentoImporte; }
        set { _descuentoImporte = value; }
    }


	public decimal AlicuotaIVA
	{
		get{return _alicuotaIVA;}
		set{_alicuotaIVA = value;}
	}

  
      public CMPProductos Producto
    {
        get { return _producto == null ? (_producto = new CMPProductos()) : _producto; }
        set { _producto = value; }
    }

    public CMPItemRemitos ItemRemito
    {
        get { return _itemRemito == null ? (_itemRemito = new CMPItemRemitos()) : _itemRemito; }
        set { _itemRemito = value; }
    }

    public SubSubsidios Subsidio
    {
        get { return _subsidios == null ? (_subsidios = new SubSubsidios()) : _subsidios; }
        set { _subsidios = value; }

    }
    public bool IncluirEnSP
    {
        get { return _incluirEnSP; }
        set { _incluirEnSP = value; }
    }

    public decimal ImporteIva
    {
        get { return _precioUnitarioSinIva * _alicuotaIVA / 100; }
    }

    public decimal ImporteIvaTotal
    {
        get { return ImporteIva * _cantidad; }
    }


    public Decimal PrecioUnitario
    {
        get { return _precioUnitarioSinIva + ImporteIva; }
    }

    public decimal Subtotal
    {
        get { return (_precioUnitarioSinIva * _cantidad) - DescuentoImporte; }
    }


    public decimal PrecioTotalItem
    {
        get { return _cantidad * PrecioUnitario - DescuentoImporte; }
    }

    //public decimal ObtenerPrecioSinIVA(decimal precio)
    //{
    //    return Math.Round( precio / (this.Producto.IVA.Alicuota / 100 + 1), 2);
    //}

    //public decimal ObtenerIVA(decimal precio)
    //{
    //    return Math.Round(precio - precio / (this.Producto.IVA.Alicuota / 100 + 1), 2);
    //}

      [Auditoria]
    public TGEFiliales Filial
    {
        get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
        set { _filial = value; }
    }

	#endregion
	}
}
