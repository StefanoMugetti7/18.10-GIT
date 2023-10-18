
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Contabilidad.Entidades
{
  [Serializable]
	public partial class CtbMonedas : Objeto
	{

	#region "Private Members"
	int _idMoneda;
	string _moneda;
	string _codigoMoneda;
	DateTime _idUsuarioAlta;
	DateTime _fechaAlta;

	#endregion
		
	#region "Constructors"
	public CtbMonedas()
	{
	}
	#endregion
		
	#region "Public Properties"

      [PrimaryKey()]
	public int IdMoneda
	{
		get{return _idMoneda ;}
		set{_idMoneda = value;}
	}

      [Auditoria()]
	public string Moneda
	{
		get{return _moneda == null ? string.Empty : _moneda ;}
		set{_moneda = value;}
	}

      [Auditoria()]
	public string CodigoMoneda
	{
		get{return _codigoMoneda == null ? string.Empty : _codigoMoneda ;}
		set{_codigoMoneda = value;}
	}

	public DateTime IdUsuarioAlta
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
