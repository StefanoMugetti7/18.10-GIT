
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Contabilidad.Entidades
{
  [Serializable]
	public partial class CtbAsientosModelosDetalles : Objeto
	{
		// Class CtbAsientosModelosDetalles
	#region "Private Members"
	int _idAsientoModeloDetalle;
	int _idAsientoModelo;
	CtbCuentasContables _cuentaContable;
    CtbTiposImputaciones _tipoImputacion;
    CtbAsientosModelosDetallesCodigos _codigoAsientoModeloDetalleCodigo;
	#endregion
		
	#region "Constructors"
	public CtbAsientosModelosDetalles()
	{
	}
	#endregion
		
	#region "Public Properties"

      [PrimaryKey()]
	public int IdAsientoModeloDetalle
	{
		get{return _idAsientoModeloDetalle ;}
		set{_idAsientoModeloDetalle = value;}
	}
	public int IdAsientoModelo
	{
		get{return _idAsientoModelo;}
		set{_idAsientoModelo = value;}
	}

      [Auditoria()]
    public CtbCuentasContables CuentaContable
	{
        get { return _cuentaContable == null ? (_cuentaContable = new CtbCuentasContables()) : _cuentaContable; }
		set{_cuentaContable = value;}
	}

      [Auditoria()]
    public CtbTiposImputaciones TipoImputacion
	{
        get { return _tipoImputacion == null ? (_tipoImputacion = new CtbTiposImputaciones()) : _tipoImputacion; }
		set{_tipoImputacion = value;}
	}
      [Auditoria()]
      public CtbAsientosModelosDetallesCodigos AsientoModeloDetalleCodigo
      {
          get { return _codigoAsientoModeloDetalleCodigo == null ? (_codigoAsientoModeloDetalleCodigo = new CtbAsientosModelosDetallesCodigos()) : _codigoAsientoModeloDetalleCodigo; }
          set { _codigoAsientoModeloDetalleCodigo = value; }
      }


	#endregion
	}
}
