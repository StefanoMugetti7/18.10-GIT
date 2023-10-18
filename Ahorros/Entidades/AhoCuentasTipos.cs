
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Ahorros.Entidades
{
  [Serializable]
	public partial class AhoCuentasTipos : Objeto
	{
	#region "Private Members"
	int _idCuentaTipo;
	string _codigoBNRA;
	string _cuentaTipo;
	string _descripcion;
	#endregion
		
	#region "Constructors"
	public AhoCuentasTipos()
	{
	}
	#endregion
		
	#region "Public Properties"

    [PrimaryKey()]
	public int IdCuentaTipo
	{
		get{return _idCuentaTipo ;}
		set{_idCuentaTipo = value;}
	}
	public string CodigoBNRA
	{
		get{return _codigoBNRA == null ? string.Empty : _codigoBNRA ;}
		set{_codigoBNRA = value;}
	}

	public string CuentaTipo
	{
		get{return _cuentaTipo == null ? string.Empty : _cuentaTipo ;}
		set{_cuentaTipo = value;}
	}

	public string Descripcion
	{
		get{return _descripcion == null ? string.Empty : _descripcion ;}
		set{_descripcion = value;}
	}		

	#endregion
	}
}
