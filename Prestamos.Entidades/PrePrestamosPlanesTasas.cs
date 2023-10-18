
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Prestamos.Entidades
{
  [Serializable]
	public partial class PrePrestamosPlanesTasas : Objeto
	{
		
	#region "Private Members"
	int _idPrestamoPlanTasa;
	int _idPrestamoPlan;
	decimal _tasa;
	DateTime _fechaInicioVigencia;
    int _cantidadCuotas;
    int _cantidadCuotasHasta;
    decimal _tasaGastoAdministrativo;
    decimal _tasaSellado;
    decimal _tasaSeguro;
    decimal _tasaEfectivaAnual;
    decimal _tasaEfectivaMensual;
    decimal? _importeDesde;
    decimal? _importeHasta;
	#endregion
		
	#region "Constructors"
	public PrePrestamosPlanesTasas()
	{
	}
	#endregion
		
	#region "Public Properties"
      
      [PrimaryKey()]
	public int IdPrestamoPlanTasa
	{
		get{return _idPrestamoPlanTasa ;}
		set{_idPrestamoPlanTasa = value;}
	}
	public int IdPrestamoPlan
	{
        get { return _idPrestamoPlan; }
		set{_idPrestamoPlan = value;}
	}

	public decimal Tasa
	{
		get{return _tasa;}
		set{_tasa = value;}
	}

    public int CantidadCuotas
    {
        get { return _cantidadCuotas; }
        set { _cantidadCuotas = value; }
    }

    public int CantidadCuotasHasta
    {
        get { return _cantidadCuotasHasta; }
        set { _cantidadCuotasHasta = value; }
    }

	public DateTime FechaInicioVigencia
	{
		get{return _fechaInicioVigencia;}
		set{_fechaInicioVigencia = value;}
	}

    public decimal TasaGastoAdministrativo
    {
        get { return _tasaGastoAdministrativo; }
        set { _tasaGastoAdministrativo = value; }
    }

    public decimal TasaSeguro
    {
        get { return _tasaSeguro; }
        set { _tasaSeguro = value; }
    }

    public decimal TasaSellado
    {
        get { return _tasaSellado; }
        set { _tasaSellado = value; }
    }

    public decimal TasaEfectivaAnual
    {
        get { return _tasaEfectivaAnual; }
        set { _tasaEfectivaAnual = value; }
    }

    public decimal TasaEfectivaMensual
    {
        get { return _tasaEfectivaMensual; }
        set { _tasaEfectivaMensual = value; }
    }

    public decimal? ImporteDesde
    {
        get { return _importeDesde; }
        set { _importeDesde = value; }
    }

    public decimal? ImporteHasta
    {
        get { return _importeHasta; }
        set { _importeHasta = value; }
    }
	#endregion
	}
}
