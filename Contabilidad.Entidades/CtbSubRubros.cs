
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Contabilidad.Entidades
{
  [Serializable]
	public partial class CtbSubRubros : Objeto
	{

	#region "Private Members"
	int? _idSubRubro;
	string _subRubro;
	string _codigoSubRubro;
	int _idUsuarioAlta;
	DateTime _fechaAlta;
	List<CtbSubRubros> _subRubros;
	#endregion
		
	#region "Constructors"
	public CtbSubRubros()
	{
	}
	#endregion
		
	#region "Public Properties"
	public int? IdSubRubro
	{
		get{return _idSubRubro ;}
		set{_idSubRubro = value;}
	}
	public string SubRubro
	{
		get{return _subRubro == null ? string.Empty : _subRubro ;}
		set{_subRubro = value;}
	}

	public string CodigoSubRubro
	{
		get{return _codigoSubRubro == null ? string.Empty : _codigoSubRubro ;}
		set{_codigoSubRubro = value;}
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
