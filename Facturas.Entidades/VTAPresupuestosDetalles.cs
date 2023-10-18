
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Compras.Entidades;
using Generales.Entidades;
namespace Facturas.Entidades
{
  [Serializable]
	public partial class VTAPresupuestosDetalles : Objeto
	{
		// Class VTAPresupuestosDetalles
	#region "Private Members"
	int _idPresupuestoDetalle;
	int _idPresupuesto;
    CMPListasPreciosDetalles _listaPrecioDetalle;
    decimal? _cantidad;
    decimal? _precioUnitarioSinIva;
    //decimal? _aliCuotaIVA;
    decimal? _descuentoPorcentual;
    decimal? _descuentoImporte;
    decimal? _subTotal;

    decimal? _margenImporte;
    decimal? _subTotalConIva;
    decimal? _importeIVA;
    TGEIVA _IVA;
    string _descripcion;
    string _descripcionProducto;
	#endregion
		
	#region "Constructors"
	public VTAPresupuestosDetalles()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdPresupuestoDetalle
	{
		get{return _idPresupuestoDetalle ;}
		set{_idPresupuestoDetalle = value;}
	}
	public int IdPresupuesto
	{
		get{return _idPresupuesto;}
		set{_idPresupuesto = value;}
	}

    public decimal? Cantidad
    {
        get { return _cantidad; }
        set { _cantidad = value; }
    }

        public decimal? MargenImporte
        {
            get { return _margenImporte; }
            set { _margenImporte = value; }
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

        public decimal Costo { get; set; }
        public decimal? MargenPorcentaje { get; set; }

        #endregion
    }
}
