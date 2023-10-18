
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace Medicina.Entidades
{
  [Serializable]
	public partial class MedPrestadoresDiasHoras : Objeto
	{
		// Class MedPrestadoresDiasHoras
	#region "Private Members"
	int _idPrestadorDiaHora;
	int _idPrestador;
	TGEDias _dia;
	TimeSpan _horaDesde;
	TimeSpan _horaHasta;
	TGEFiliales _filial;
    MedEspecializaciones _especializacion;
    int _tiempo;
    DateTime _fechaInicioVigencia;
	#endregion
		
	#region "Constructors"
	public MedPrestadoresDiasHoras()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdPrestadorDiaHora
	{
		get{return _idPrestadorDiaHora ;}
		set{_idPrestadorDiaHora = value;}
	}
	public int IdPrestador
	{
		get{return _idPrestador;}
		set{_idPrestador = value;}
	}
      [Auditoria()]
	public TGEDias Dia
	{
        get { return _dia == null ? (_dia = new TGEDias()) : _dia; }
		set{_dia = value;}
	}
      [Auditoria()]
	public TimeSpan HoraDesde
	{
		get{return _horaDesde;}
		set{_horaDesde = value;}
	}
      [Auditoria()]
	public TimeSpan HoraHasta
	{
		get{return _horaHasta;}
		set{_horaHasta = value;}
	}
      [Auditoria()]
	public TGEFiliales Filial
	{
        get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
		set{_filial = value;}
	}
      [Auditoria()]
    public MedEspecializaciones Especializacion
    {
        get { return _especializacion == null ? (_especializacion = new MedEspecializaciones()) : _especializacion; }
        set { _especializacion = value; }
    }

      [Auditoria()]
      public int Tiempo
      {
          get { return _tiempo; }
          set { _tiempo = value; }
      }

      [Auditoria()]
      public DateTime FechaInicioVigencia
      {
          get { return _fechaInicioVigencia; }
          set { _fechaInicioVigencia = value; }
      }

	#endregion
	}
}
