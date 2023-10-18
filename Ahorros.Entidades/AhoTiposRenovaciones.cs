
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Ahorros.Entidades
{
  [Serializable]
	public partial class AhoTiposRenovaciones : Objeto
	{
	#region "Private Members"
	int _idTipoRenovacion;
	string _tipoRenovacion;
	#endregion
		
	#region "Constructors"
	public AhoTiposRenovaciones()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdTipoRenovacion
	{
		get{return _idTipoRenovacion ;}
		set{_idTipoRenovacion = value;}
	}
      [Auditoria()]
	public string TipoRenovacion
	{
		get{return _tipoRenovacion == null ? string.Empty : _tipoRenovacion ;}
		set{_tipoRenovacion = value;}
	}		

	#endregion
	}
}
