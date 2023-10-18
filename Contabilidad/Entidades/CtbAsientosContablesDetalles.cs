
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Contabilidad.Entidades
{
  [Serializable]
	public partial class CtbAsientosContablesDetalles : Objeto
	{
		// Class CtbAsientosContablesDetalles
	#region "Private Members"
	int _idAsientoContableDetalle;
	int _idAsientoContable;
	CtbCuentasContables _cuentaContable;
	decimal? _debe;
	decimal? _haber;
	string _detalle;
	
	#endregion
		
	#region "Constructors"
	public CtbAsientosContablesDetalles()
	{
	}
	#endregion
		
	#region "Public Properties"
	public int IdAsientoContableDetalle
	{
		get{return _idAsientoContableDetalle ;}
		set{_idAsientoContableDetalle = value;}
	}
	public int IdAsientoContable
	{
		get{return _idAsientoContable;}
		set{_idAsientoContable = value;}
	}

    public CtbCuentasContables CuentaContable
	{
        get { return _cuentaContable == null ? (_cuentaContable = new CtbCuentasContables()) : _cuentaContable; }
		set{_cuentaContable = value;}
	}

	public decimal? Debe
	{
		get{return _debe;}
		set{_debe = value;}
	}

	public decimal? Haber
	{
		get{return _haber;}
		set{_haber = value;}
	}

	public string Detalle
	{
		get{return _detalle == null ? string.Empty : _detalle ;}
		set{_detalle = value;}
	}

	#endregion
	}
}
