
using System;
using System.Collections.Generic;
using Comunes.Entidades;

namespace Compras.Entidades
{
  [Serializable]
	public partial class CMPItemRemitos : Objeto
	{


	#region "Private Members"
	int? _idItemRemitos;
	int _idRemitos;
	int _cantidad;
    decimal _precioUnitario;
    decimal _alicuotaIVA;
    string _numeroRemitoDescripcion;
    DateTime _fechaRemito;
    CMPProductos _producto;
    bool _incluirEnSP;
    int? _idItemSolicitudPago;
	#endregion
		
	#region "Constructors"
	public CMPItemRemitos()
	{
	}
	#endregion
		
	#region "Public Properties"
	public int? IdItemRemitos
	{
		get{return _idItemRemitos ;}
		set{_idItemRemitos = value;}
	}
	public int IdRemitos
	{
		get{return _idRemitos;}
		set{_idRemitos = value;}
	}

	public int Cantidad
	{
		get{return _cantidad;}
		set{_cantidad = value;}
	}

    public decimal PrecioUnitario
    {
        get { return _precioUnitario; }
        set { _precioUnitario = value; }
    }

    public decimal AlicuotaIVA
    {
        get { return _alicuotaIVA; }
        set { _alicuotaIVA = value; }
    }

    public string NumeroRemitoDescripcion
    {
        get { return _numeroRemitoDescripcion; }
        set { _numeroRemitoDescripcion = value; }
    }

    public DateTime FechaRemito
    {
        get { return _fechaRemito <= DateTime.MinValue  ? (_fechaRemito = DateTime.Now) : _fechaRemito; }
        set { _fechaRemito = value; }
    }

    public CMPProductos Producto
    {
        get { return _producto == null ? (_producto = new CMPProductos()) : _producto; }
        set { _producto = value; }
    }

    public bool IncluirEnSP
    {
        get { return _incluirEnSP; }
        set { _incluirEnSP = value; }
    }

    public int? IdItemSolicitudPago
    {
        get { return _idItemSolicitudPago; }
        set { _idItemSolicitudPago = value; }
    }
	#endregion
	}
}
