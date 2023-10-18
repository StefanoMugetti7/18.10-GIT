
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Afiliados.Entidades
{
  [Serializable]
	public partial class AfiAlertasTipos : Objeto
	{
	
	#region "Private Members"
	int _idAlertaTipo;
	string _alertaTipo;

	#endregion
		
	#region "Constructors"
	public AfiAlertasTipos()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdAlertaTipo
	{
		get{return _idAlertaTipo ;}
		set{_idAlertaTipo = value;}
	}
      [Auditoria()]
	public string AlertaTipo
	{
		get{return _alertaTipo == null ? string.Empty : _alertaTipo ;}
		set{_alertaTipo = value;}
	}

	
	#endregion
	}
}
