
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Contabilidad.Entidades
{
  [Serializable]
	public partial class CtbCapitulos : Objeto
	{

	#region "Private Members"
	int _idCapitulo;
	string _capitulo;
	string _codigoCapitulo;
	int _idUsuarioAlta;
	DateTime _fechaAlta;

	#endregion
		
	#region "Constructors"
	public CtbCapitulos()
	{
	}
	#endregion
		
	#region "Public Properties"

      [PrimaryKey()]
	public int IdCapitulo
	{
		get{return _idCapitulo ;}
		set{_idCapitulo = value;}
	}

      [Auditoria()]
	public string Capitulo
	{
		get{return _capitulo == null ? string.Empty : _capitulo ;}
		set{_capitulo = value;}
	}

      [Auditoria()]
	public string CodigoCapitulo
	{
		get{return _codigoCapitulo == null ? string.Empty : _codigoCapitulo ;}
		set{_codigoCapitulo = value;}
	}

	public int IdUsuarioAlta
	{
		get{return _idUsuarioAlta;}
		set{_idUsuarioAlta = value;}
	}

	public DateTime FechaAlta
	{
		get{return _fechaAlta;}
		set{_fechaAlta = value;}
	}

	#endregion
	}
}
