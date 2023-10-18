
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Proveedores.Entidades;
namespace Compras.Entidades
{
  [Serializable]
	public partial class CMPRemitos : Objeto
	{
	#region "Private Members"
	int _idRemitos;
	DateTime _fechaRemito;
	string _numeroRemitoPrefijo;
    private CapProveedores _proveedor;
	string _numeroRemitoSuFijo;
    int _idAgenciaEmision;
	#endregion
		
	#region "Constructors"
	public CMPRemitos()
	{
	}
	#endregion
		
	#region "Public Properties"
	public int IdRemitos
	{
		get{return _idRemitos ;}
		set{_idRemitos = value;}
	}

    public CapProveedores Proveedor
    {
        get { return _proveedor == null ? (_proveedor = new CapProveedores()) : _proveedor; }
        set { _proveedor = value; }
    }

	public DateTime FechaRemito
	{
		get{return _fechaRemito;}
		set{_fechaRemito = value;}
	}

	public string NumeroRemitoPrefijo
	{
		get{return _numeroRemitoPrefijo == null ? string.Empty : _numeroRemitoPrefijo ;}
		set{_numeroRemitoPrefijo = value;}
	}

	public string NumeroRemitoSuFijo
	{
		get{return _numeroRemitoSuFijo == null ? string.Empty : _numeroRemitoSuFijo ;}
		set{_numeroRemitoSuFijo = value;}
	}

    public int IdAenciaEmision
    {
        get { return _idAgenciaEmision; }
        set { _idAgenciaEmision = value; }
    }

    public string NumeroRemitoDescripcion
    {
        get { return string.Concat(NumeroRemitoPrefijo, "-", NumeroRemitoSuFijo); }
    }

	#endregion
	}
}
