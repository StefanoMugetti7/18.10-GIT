
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Afiliados.Entidades
{
  [Serializable]
	public partial class AfiGrados : Objeto
	{
	#region "Private Members"
	int _idGrado;
	string _grado;
	#endregion
		
	#region "Constructors"
	public AfiGrados()
	{
	}
	#endregion
		
	#region "Public Properties"
    [PrimaryKey()]
	public int IdGrado
	{
		get{return _idGrado ;}
		set{_idGrado = value;}
	}
	public string Grado
	{
		get{return _grado == null ? string.Empty : _grado ;}
		set{_grado = value;}
	}

	#endregion
	}
}
