
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Contabilidad.Entidades
{
  [Serializable]
	public partial class CtbRubrosSubRubros : Objeto
	{

	#region "Private Members"
	int _idRubroSubRubro;
	int _idRubro;
	int? _idSubRubro;
	#endregion
		
	#region "Constructors"
	public CtbRubrosSubRubros()
	{
	}
	#endregion
		
	#region "Public Properties"

      [PrimaryKey()]
	public int IdRubroSubRubro
	{
		get{return _idRubroSubRubro ;}
		set{_idRubroSubRubro = value;}
	}

      [Auditoria()]
	public int IdRubro
	{
		get{return _idRubro;}
		set{_idRubro = value;}
	}
      [Auditoria()]
	public int? IdSubRubro
	{
		get{return _idSubRubro;}
		set{_idSubRubro = value;}
	}

	
	#endregion
	}
}
