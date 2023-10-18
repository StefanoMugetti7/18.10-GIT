
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Afiliados.Entidades;
using Generales.Entidades;
namespace Medicina.Entidades
{
  [Serializable]
	public partial class MedTurnos : Objeto
	{
		// Class MedTurnos
	#region "Private Members"
	int _idTurno;
    int _idTurnera;
    //MedTurneras _turnera;
    MedPrestadores _prestador;
	DateTime? _fechaHoraDesde;
    DateTime? _fechaHoraHasta;
	MedEspecializaciones _especializacion;
	AfiAfiliados _afiliado;
	DateTime _fechaAlta;
	int _idUsuarioAlta;
    Guid _guidTurnera;
    Guid _guidTurno;
    TGEObrasSociales _obraSocial;
    string _observaciones;
        DateTime? _fechaDesde;
        DateTime? _fechaHasta;
        #endregion

        #region "Constructors"
        public MedTurnos()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdTurno
	{
		get{return _idTurno ;}
		set{_idTurno = value;}
	}
	
    //public MedTurneras Turnera
    //{
    //    get { return _turnera == null ? (_turnera = new MedTurneras()) : _turnera; }
    //    set{_turnera = value;}
    //}

      public int IdTurnera
      {
          get { return _idTurnera; }
          set { _idTurnera = value; }
      }

    public MedPrestadores Prestador
    {
        get { return _prestador == null ? (_prestador = new MedPrestadores()) : _prestador; }
        set { _prestador = value; }
    }

	public DateTime? FechaHoraDesde
	{
		get{return _fechaHoraDesde;}
		set{_fechaHoraDesde = value;}
	}

    [Auditoria()]
    public DateTime? FechaHoraHasta
    {
        get { return _fechaHoraHasta; }
        set { _fechaHoraHasta = value; }
    }

    public MedEspecializaciones Especializacion
	{
        get { return _especializacion == null ? (_especializacion = new MedEspecializaciones()) : _especializacion; }
		set{_especializacion = value;}
	}

	public AfiAfiliados Afiliado
	{
        get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; }
		set{_afiliado = value;}
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

    public string ApellidoNombre
    {
        get { return this.Afiliado.ApellidoNombre; }
        set {  }
    }

    public Guid GuidTurnera
    {
        get { return _guidTurnera; }
        set { _guidTurnera = value; }
    }

    public Guid GuidTurno
    {
        get { return _guidTurno; }
        set { _guidTurno = value; }
    }

    [Auditoria()]
    public TGEObrasSociales ObraSocial
    {
        get { return _obraSocial == null ? (_obraSocial = new TGEObrasSociales()) : _obraSocial; }
        set { _obraSocial = value; }
    }

      [Auditoria()]
    public string Observaciones
    {
        get { return _observaciones == null ? string.Empty : _observaciones; }
        set { _observaciones = value; }
    }
        public string DescripcionCombo { get; set; }

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
        #endregion
    }
}
