
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Contabilidad.Entidades
{
  [Serializable]
	public partial class CtbRubros : Objeto
	{

	#region "Private Members"
	int _idRubro;
	string _rubro;
	string _codigoRubro;
	int _idEstado;
	int _idUsuarioAlta;
	DateTime _fechaAlta;
	List<CtbSubRubros> _subRubros;
	#endregion
		
	#region "Constructors"
	public CtbRubros()
	{
	}
	#endregion
		
	#region "Public Properties"
      
      [PrimaryKey()]
	public int IdRubro
	{
		get{return _idRubro ;}
		set{_idRubro = value;}
	}

      [Auditoria()]
	public string Rubro
	{
		get{return _rubro == null ? string.Empty : _rubro ;}
		set{_rubro = value;}
	}

      [Auditoria()]
	public string CodigoRubro
	{
		get{return _codigoRubro == null ? string.Empty : _codigoRubro ;}
		set{_codigoRubro = value;}
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

	public List<CtbSubRubros> SubRubros
	{
		get{return _subRubros==null ? (_subRubros = new List<CtbSubRubros>()) : _subRubros;}
		set{_subRubros = value;}
	}

	#endregion
	}
}
