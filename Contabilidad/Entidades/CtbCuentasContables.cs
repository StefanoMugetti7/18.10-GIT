
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Contabilidad.Entidades
{
  [Serializable]
	public partial class CtbCuentasContables : Objeto
	{

	#region "Private Members"
	int _idCuentaContable;
	CtbCapitulos _capitulo;
	CtbRubros _rubro;
	CtbMonedas _moneda;
	CtbDepartamentos _departamento;
	CtbSubRubros _subRubro;
	int _imputacion;
	string _descripcion;
    string _numeroCuenta;
	DateTime _fechaAlta;
	int _idUsuarioAlta;

	#endregion
		
	#region "Constructors"
	public CtbCuentasContables()
	{
	}
	#endregion
		
	#region "Public Properties"

      [PrimaryKey()]
	public int IdCuentaContable
	{
		get{return _idCuentaContable ;}
		set{_idCuentaContable = value;}
	}
      [Auditoria()]
      public CtbCapitulos Capitulo
	{
        get { return _capitulo == null ? (_capitulo = new CtbCapitulos()) : _capitulo; }
		set{_capitulo = value;}
	}
      [Auditoria()]
      public CtbRubros Rubro
	{
        get { return _rubro == null ? (_rubro = new CtbRubros()) : _rubro; }
		set{_rubro = value;}
	}
      [Auditoria()]
	public CtbMonedas Moneda
	{
        get { return _moneda == null ? (_moneda = new CtbMonedas()) : _moneda; }
		set{_moneda = value;}
	}
      [Auditoria()]
	public CtbDepartamentos Departamento
	{
        get { return _departamento == null ? (_departamento = new CtbDepartamentos()) : _departamento; }
		set{_departamento = value;}
	}
      [Auditoria()]
	public CtbSubRubros SubRubro
	{
        get { return _subRubro == null ? (_subRubro = new CtbSubRubros()) : _subRubro; }
		set{_subRubro = value;}
	}
      [Auditoria()]
	public int Imputacion
	{
		get{return _imputacion;}
		set{_imputacion = value;}
	}

      [Auditoria()]
	public string Descripcion
	{
		get{return _descripcion == null ? string.Empty : _descripcion ;}
		set{_descripcion = value;}
	}

    public string NumeroCuenta
    {
        get { return _numeroCuenta == null ? string.Empty : _numeroCuenta; }
        set { _numeroCuenta = value; }
    }

	public DateTime FechaAlta
	{
		get{return _fechaAlta;}
		set{_fechaAlta = value;}
	}

	public int IdUsuarioAlta
	{
		get{return _idUsuarioAlta;}
		set{_idUsuarioAlta = value;}
	}

	#endregion
	}
}
