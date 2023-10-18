
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace Prestamos.Entidades
{
  [Serializable]
	public partial class PrePrestamosPlanes : Objeto
	{

	#region "Private Members"
	int _idPrestamoPlan;
	string _descripcion;
	DateTime _fechaAlta;
    PrePrestamosPlanesTasas _prestamoPlanTasa;
    TGETiposOperaciones _tipoOperacion;
    List<PrePrestamosPlanesTasas> _prestamosPlanesTasas;
    List<TGEArchivos> _archivos;
    List<TGEComentarios> _comentarios;
    TGEFormasCobros _formaCobro;
    List<TGEFormasCobros> _formasCobros;
    DateTime _fechaInicioVigencia;
    DateTime _fechaFinVigencia;
    

	#endregion
		
	#region "Constructors"
	public PrePrestamosPlanes()
	{
	}
	#endregion
		
	#region "Public Properties"

      [PrimaryKey()]
	public int IdPrestamoPlan
	{
		get{return _idPrestamoPlan ;}
		set{_idPrestamoPlan = value;}
	}
	public string Descripcion
	{
		get{return _descripcion == null ? string.Empty : _descripcion ;}
		set{_descripcion = value;}
	}

	public DateTime FechaAlta
	{
		get{return _fechaAlta;}
		set{_fechaAlta = value;}
	}

    public PrePrestamosPlanesTasas PrestamoPlanTasa
    {
        get { return _prestamoPlanTasa == null ? (_prestamoPlanTasa = new PrePrestamosPlanesTasas()) : _prestamoPlanTasa; }
        set { _prestamoPlanTasa = value; }
    }

    public TGETiposOperaciones TipoOperacion
    {
        get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
        set { _tipoOperacion = value; }
    }

    public List<PrePrestamosPlanesTasas> PrestamosPlanesTasas
    {
        get { return _prestamosPlanesTasas == null ? (_prestamosPlanesTasas = new List<PrePrestamosPlanesTasas>()) : _prestamosPlanesTasas; }
        set { _prestamosPlanesTasas = value; }
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

    public TGEFormasCobros FormaCobro
    {
        get { return _formaCobro == null ? (_formaCobro = new TGEFormasCobros()) : _formaCobro; }
        set { _formaCobro = value; }
    }

    public List<TGEFormasCobros> FormasCobros
    {
        get { return _formasCobros == null ? (_formasCobros = new List<TGEFormasCobros>()) : _formasCobros; }
        set { _formasCobros = value; }
    }

    public DateTime FechaInicioVigencia
    {
        get { return _fechaInicioVigencia; }
        set { _fechaInicioVigencia = value; }
    }

    public DateTime FechaFinVigencia
    {
        get { return _fechaFinVigencia; }
        set { _fechaFinVigencia = value; }
    }

	#endregion
	}
}
