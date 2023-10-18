
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
    int _idAsientoContableDetalleLog;
	int _idAsientoContable;
    int _idAsientoContableLog;
	decimal? _debe;
	decimal? _haber;
	string _detalle;
    CtbCentrosCostosProrrateos _centroCostoProrrateo;
    CtbCuentasContables _cuentaContable;
	#endregion
		
	#region "Constructors"
	public CtbAsientosContablesDetalles()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey]
	public int IdAsientoContableDetalle
	{
		get{return _idAsientoContableDetalle ;}
		set{_idAsientoContableDetalle = value;}
	}
      public int IdAsientoContableDetalleLog
      {
          get { return _idAsientoContableDetalleLog; }
          set { _idAsientoContableDetalleLog = value; }
      }
	public int IdAsientoContable
	{
		get{return _idAsientoContable;}
		set{_idAsientoContable = value;}
	}

    public int IdAsientoContableLog
    {
        get { return _idAsientoContableLog; }
        set { _idAsientoContableLog = value; }
    }

      [Auditoria]
	public decimal? Debe
	{
		get{return _debe;}
		set{_debe = value;}
	}
      [Auditoria]
	public decimal? Haber
	{
		get{return _haber;}
		set{_haber = value;}
	}
      [Auditoria]
	public string Detalle
	{
		get{return _detalle == null ? string.Empty : _detalle ;}
		set{_detalle = value;}
	}

      public CtbCentrosCostosProrrateos CentroCostoProrrateo
      {
          get { return _centroCostoProrrateo == null ? (_centroCostoProrrateo = new CtbCentrosCostosProrrateos()) : _centroCostoProrrateo; }
          set { _centroCostoProrrateo = value; }
      }

      [Auditoria]
      public CtbCuentasContables CuentaContable
      {
          get { return _cuentaContable == null ? (_cuentaContable = new CtbCuentasContables()) : _cuentaContable; }
          set { _cuentaContable = value; }
      }

	#endregion
	}
}
