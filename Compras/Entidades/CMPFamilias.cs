
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Contabilidad.Entidades;

namespace Compras.Entidades
{
  [Serializable]
	public partial class CMPFamilias : Objeto
	{

	#region "Private Members"
	int _idFamilia;
    bool _stockeable;
	string _descripcion;
    int _idUsuarioAlta;
    DateTime _fechaAlta;
    
   CtbCuentasContables _cuentacontable;
	#endregion
		
	#region "Constructors"
	public CMPFamilias()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdFamilia
	{
		get{return _idFamilia ;}
		set{_idFamilia = value;}
	}
      [Auditoria()]
    public bool Stockeable
	{
		get{return _stockeable  ;}
        set { _stockeable = value; }
	}
      [Auditoria()]
	public string Descripcion
	{
		get{return _descripcion == null ? string.Empty : _descripcion ;}
		set{_descripcion = value;}
	}

      [Auditoria()]
    public CtbCuentasContables CuentaContable
    {
        get { return _cuentacontable == null ? (_cuentacontable = new CtbCuentasContables()) : _cuentacontable; }
        set { _cuentacontable = value; }
    }

    public int IdUsuarioAlta
    {
        get { return _idUsuarioAlta; }
        set { _idUsuarioAlta = value; }
    }

    public DateTime FechaAlta
    {
        get { return _fechaAlta; }
        set { _fechaAlta = value; }
    }

	#endregion
	}
}
