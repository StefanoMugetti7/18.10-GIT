
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Prestamos.Entidades
{
  [Serializable]
	public partial class PrePrestamosCuotas : Objeto
	{

	#region "Private Members"
	int _idPrestamoCuota;
    int _idSimulacionCuota;
	int _idPrestamo;
    int _idSimulacion;
	int _cuotaNumero; 
	DateTime _cuotaFechaVencimiento;
	decimal _importeCuota;
	decimal _importeInteres;
	decimal _importeAmortizacion;
	decimal _importeSaldo;
    int _idFilialCobro;
    DateTime _fechaCobro;
	#endregion
		
	#region "Constructors"
	public PrePrestamosCuotas()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdPrestamoCuota
	{
		get{return _idPrestamoCuota ;}
		set{_idPrestamoCuota = value;}
	}

      public int IdSimulacionCuota
      {
          get { return _idSimulacionCuota; }
          set { _idSimulacionCuota = value; }
      }

	public int IdPrestamo
	{
		get{return _idPrestamo;}
		set{_idPrestamo = value;}
	}

    public int IdSimulacion
    {
        get { return _idSimulacion; }
        set { _idSimulacion = value; }
    }

	public int CuotaNumero
	{
		get{return _cuotaNumero;}
		set{_cuotaNumero = value;}
	}

	public DateTime CuotaFechaVencimiento
	{
		get{return _cuotaFechaVencimiento;}
		set{_cuotaFechaVencimiento = value;}
	}

	public decimal ImporteCuota
	{
		get{return _importeCuota;}
		set{_importeCuota = value;}
	}

	public decimal ImporteInteres
	{
		get{return _importeInteres;}
		set{_importeInteres = value;}
	}

	public decimal ImporteAmortizacion
	{
		get{return _importeAmortizacion;}
		set{_importeAmortizacion = value;}
	}

	public decimal ImporteSaldo
	{
		get{return _importeSaldo;}
		set{_importeSaldo = value;}
	}

    public int IdFilialCobro
    {
        get { return _idFilialCobro; }
        set { _idFilialCobro = value; }
    }

    public DateTime FechaCobro
    {
        get { return _fechaCobro; }
        set { _fechaCobro = value; }
    }
      
	#endregion
	}
}
