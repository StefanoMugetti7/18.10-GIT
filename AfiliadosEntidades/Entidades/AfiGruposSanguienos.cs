
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Afiliados.Entidades
{
  [Serializable]
	public partial class AfiGruposSanguienos : Objeto
	{
	#region "Private Members"
	int _idGrupoSanguienio;
	string _grupoSanguineo;

	#endregion
		
	#region "Constructors"
	public AfiGruposSanguienos()
	{
	}
	#endregion
		
	#region "Public Properties"
	public int IdGrupoSanguienio
	{
		get{return _idGrupoSanguienio ;}
		set{_idGrupoSanguienio = value;}
	}
	public string GrupoSanguineo
	{
		get{return _grupoSanguineo == null ? string.Empty : _grupoSanguineo ;}
		set{_grupoSanguineo = value;}
	}
			

	#endregion
	}
}
