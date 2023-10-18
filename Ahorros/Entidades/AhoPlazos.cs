
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace Ahorros.Entidades
{
    [Serializable]
	public partial class AhoPlazos : Objeto
	{

    #region "Private Members"

    int _idPlazos;
    int _plazoDias;
    decimal _tasaInteres;
    DateTime _fechaDesde;
    DateTime _fechaAlta;
    UsuariosAlta _UsuarioAlta;
    List<AhoCuentasMovimientos> _cuentasMovimientos;
    List<TGEArchivos> _archivos;
    List<TGEComentarios> _comentarios;

    #endregion
		
	#region "Constructors"
	public AhoPlazos()
	{
	}
	#endregion
		
	#region "Public Properties"

    [PrimaryKey()]
	public int IdPlazos
	{
		get{return _idPlazos ;}
		set{_idPlazos = value;}
	}
	public int PlazoDias
	{
		get{return _plazoDias;}
		set{_plazoDias = value;}
	}

	public decimal TasaInteres
	{
		get{return _tasaInteres;}
		set{_tasaInteres = value;}
	}

    public DateTime FechaDesde
    {
        get { return _fechaDesde; }
        set { _fechaDesde = value; }
    }

	public DateTime FechaAlta
	{
		get{return _fechaAlta;}
		set{_fechaAlta = value;}
	}

    public string PlazoDiasInteres
    {
        get { return string.Concat(this.PlazoDias.ToString(), " - ", this.TasaInteres.ToString()); }
        set { }
    }

	public UsuariosAlta UsuarioAlta
	{
        get { return _UsuarioAlta == null ? (_UsuarioAlta = new UsuariosAlta()) : _UsuarioAlta; }
		set{_UsuarioAlta = value;}
	}

    public List<TGEArchivos> Archivos
    {
        get { return _archivos == null ? (_archivos = new List<TGEArchivos>()) : _archivos; }
        set { _archivos = value; }
    }

    public List<TGEComentarios> Comentarios
    {
        get { return _comentarios == null ? (_comentarios = new List<TGEComentarios>()) : _comentarios; }
        set { _comentarios = value; }
    }

	#endregion
	}
}
