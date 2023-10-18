
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace Ahorros.Entidades
{
  [Serializable]
	public partial class AhoTiposOperaciones : Objeto
	{

	#region "Private Members"
	int _idTipoOperacion;
	string _tipoOperacion;
	TGETiposMovimientos _tipoMovimiento;
	DateTime _fechaAlta;
	UsuariosAlta _usuarioAlta;
	#endregion
		
	#region "Constructors"
	public AhoTiposOperaciones()
	{
	}
	#endregion
		
	#region "Public Properties"

    [PrimaryKey()]
	public int IdTipoOperacion
	{
		get{return _idTipoOperacion ;}
		set{_idTipoOperacion = value;}
	}
	public string TipoOperacion
	{
		get{return _tipoOperacion == null ? string.Empty : _tipoOperacion ;}
		set{_tipoOperacion = value;}
	}

	public TGETiposMovimientos TipoMovimiento
	{
		get{return _tipoMovimiento==null? (_tipoMovimiento=new TGETiposMovimientos()):_tipoMovimiento;}
		set{_tipoMovimiento = value;}
	}

	public DateTime FechaAlta
	{
		get{return _fechaAlta;}
		set{_fechaAlta = value;}
	}

	public UsuariosAlta UsuarioAlta
	{
		get{return _usuarioAlta==null ? (_usuarioAlta=new UsuariosAlta()) : _usuarioAlta;}
		set{_usuarioAlta = value;}
	}
		

	#endregion
	}
}
