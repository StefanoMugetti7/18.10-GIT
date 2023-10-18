
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Afiliados.Entidades;
using Generales.Entidades;
namespace Medicina.Entidades
{
  [Serializable]
	public partial class MedPrestaciones : Objeto
	{
		// Class MedPrestaciones
	#region "Private Members"
	int _idPrestacion;
	DateTime _fecha;
    DateTime? _fechaDesde;
    DateTime? _fechaHasta;
	MedNomencladores _nomenclador;
	AfiAfiliados _afiliado;
	string _observaciones;
	MedTurnos _turno;
	MedPrestadores _prestador;
     MedEspecializaciones _especializacion;
	int? _idOrdenCobro;
	DateTime _fechaAlta;
	int _idUsuarioAlta;
    TGEObrasSociales _obraSocial;
    List<TGEArchivos> _archivos;
    List<TGEComentarios> _comentarios;
    List<TGECampos> _campos;
	#endregion
		
	#region "Constructors"
	public MedPrestaciones()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdPrestacion
	{
		get{return _idPrestacion ;}
		set{_idPrestacion = value;}
	}
	public DateTime Fecha
	{
		get{return _fecha;}
		set{_fecha = value;}
	}

	public MedNomencladores Nomenclador
	{
        get { return _nomenclador == null ? (_nomenclador = new MedNomencladores()) : _nomenclador; }
		set{_nomenclador = value;}
	}

	public AfiAfiliados Afiliado
	{
        get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; }
		set{_afiliado = value;}
	}

	public string Observaciones
	{
		get{return _observaciones == null ? string.Empty : _observaciones ;}
		set{_observaciones = value;}
	}

	public MedTurnos Turno
	{
        get { return _turno == null ? (_turno = new MedTurnos()) : _turno; }
		set{_turno = value;}
	}

	public MedPrestadores Prestador
	{
        get { return _prestador == null ? (_prestador = new MedPrestadores()) : _prestador; }
		set{_prestador = value;}
	}

    public MedEspecializaciones Especializacion
    {
        get { return _especializacion == null ? (_especializacion = new MedEspecializaciones()) : _especializacion; }
        set { _especializacion = value; }
    }

    public TGEObrasSociales ObraSocial
    {
        get { return _obraSocial == null ? (_obraSocial = new TGEObrasSociales()) : _obraSocial; }
        set { _obraSocial = value; }
    }

	public int? IdOrdenCobro
	{
		get{return _idOrdenCobro;}
		set{_idOrdenCobro = value;}
	}

	public DateTime FechaAlta
	{
		get{return _fechaAlta;}
		set{_fechaAlta = value;}
	}

    public DateTime? FechaDesde
    {
        get { return _fechaDesde; }
        set { _fechaDesde = value; }
    }

    public DateTime? FechaHasta
    {
        get { return _fechaHasta; }
        set { _fechaHasta = value; }
    }

	public int IdUsuarioAlta
	{
		get{return _idUsuarioAlta;}
		set{_idUsuarioAlta = value;}
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

    public List<TGECampos> Campos
    {
        get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
        set { _campos = value; }
    }

	#endregion
	}
}
