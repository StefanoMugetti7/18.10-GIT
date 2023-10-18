
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Contabilidad.Entidades
{
  [Serializable]
	public partial class CtbDepartamentos : Objeto
	{

	#region "Private Members"
	int _idDepartamento;
	string _departamento;
	string _codigoDepartamento;
	int _idUsuarioAlta;
	DateTime _fechaAlta;
	
	#endregion
		
	#region "Constructors"
	public CtbDepartamentos()
	{
	}
	#endregion
		
	#region "Public Properties"

      [PrimaryKey()]
	public int IdDepartamento
	{
		get{return _idDepartamento ;}
		set{_idDepartamento = value;}
	}

      [Auditoria()]
	public string Departamento
	{
		get{return _departamento == null ? string.Empty : _departamento ;}
		set{_departamento = value;}
	}

      [Auditoria()]
	public string CodigoDepartamento
	{
		get{return _codigoDepartamento == null ? string.Empty : _codigoDepartamento ;}
		set{_codigoDepartamento = value;}
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
