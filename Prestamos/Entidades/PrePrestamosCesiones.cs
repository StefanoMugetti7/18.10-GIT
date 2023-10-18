
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace Prestamos.Entidades
{
  [Serializable]
	public partial class PrePrestamosCesiones : Objeto
	{
		// Class PrePrestamosCesiones
	#region "Private Members"
	int _idPrestamoCesion;
	PreCesionarios _cesionario;
	string _descripcion;
	decimal _tasa;
	decimal _vAN;
	int _cantidad;
	decimal _totalAmortizacion;
	decimal _totalInteres;
	DateTime _fechaAlta;
	int _idUsuarioAlta;
    List<PrePrestamosCesionesDetalles> _prestamosCesionesDetalles;
    List<TGEArchivos> _archivos;
    List<TGEComentarios> _comentarios;
	#endregion
		
	#region "Constructors"
	public PrePrestamosCesiones()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdPrestamoCesion
	{
		get{return _idPrestamoCesion ;}
		set{_idPrestamoCesion = value;}
	}
    public PreCesionarios Cesionario
	{
        get { return _cesionario == null ? (_cesionario = new PreCesionarios()) : _cesionario; }
		set{_cesionario = value;}
	}

	public string Descripcion
	{
		get{return _descripcion == null ? string.Empty : _descripcion ;}
		set{_descripcion = value;}
	}

	public decimal Tasa
	{
		get{return _tasa;}
		set{_tasa = value;}
	}

	public decimal VAN
	{
		get{return _vAN;}
		set{_vAN = value;}
	}

	public int Cantidad
	{
		get{return _cantidad;}
		set{_cantidad = value;}
	}

	public decimal TotalAmortizacion
	{
		get{return _totalAmortizacion;}
		set{_totalAmortizacion = value;}
	}

	public decimal TotalInteres
	{
		get{return _totalInteres;}
		set{_totalInteres = value;}
	}

	public DateTime FechaAlta
	{
		get{return _fechaAlta;}
		set{_fechaAlta = value;}
	}

	public int IdUsuarioAlta
	{
		get{return _idUsuarioAlta;}
		set{_idUsuarioAlta = value;}
	}

    public List<PrePrestamosCesionesDetalles> PrestamosCesionesDetalles
    {
        get { return _prestamosCesionesDetalles == null ? (_prestamosCesionesDetalles = new List<PrePrestamosCesionesDetalles>()) : _prestamosCesionesDetalles; }
        set { _prestamosCesionesDetalles = value; }
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
