
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace ProcesosDatos.Entidades
{
  [Serializable]
	public partial class SisTiposParametros : Objeto
	{

	#region "Private Members"
	int _idTipoParametro;
	string _descripcion;

	#endregion
		
	#region "Constructors"
	public SisTiposParametros()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdTipoParametro
	{
		get{return _idTipoParametro ;}
		set{_idTipoParametro = value;}
	}
	public string Descripcion
	{
		get{return _descripcion == null ? string.Empty : _descripcion ;}
		set{_descripcion = value;}
	}

	#endregion
	}
}
