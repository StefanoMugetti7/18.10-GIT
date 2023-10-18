
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Prestamos.Entidades
{
  [Serializable]
	public partial class PrePrestamosCesionesDetalles : Objeto
	{
		// Class PrePrestamosCesionesDetalles
	#region "Private Members"
	int _idPrestamoCesionDetalle;
	int _idPrestamoCesion;
	int _idPrestamo;
	decimal _importeAmortizacion;
	decimal _importeInteres;

	int _cantidadCuotas;
    DateTime _fechaConfirmacion;
    DateTime? _fechaDesde;
    DateTime? _fechaHasta;
    bool _incluir;
    int _riesgoCrediticio;
    int _idPrestamoPlan;
	#endregion
		
	#region "Constructors"
	public PrePrestamosCesionesDetalles()
	{
	}
	#endregion
		
	#region "Public Properties"

      [PrimaryKey()]
	public int IdPrestamoCesionDetalle
	{
		get{return _idPrestamoCesionDetalle ;}
		set{_idPrestamoCesionDetalle = value;}
	}
	public int IdPrestamoCesion
	{
		get{return _idPrestamoCesion;}
		set{_idPrestamoCesion = value;}
	}

	public int IdPrestamo
	{
		get{return _idPrestamo;}
		set{_idPrestamo = value;}
	}

	public decimal ImporteAmortizacion
	{
		get{return _importeAmortizacion;}
		set{_importeAmortizacion = value;}
	}

	public decimal ImporteInteres
	{
		get{return _importeInteres;}
		set{_importeInteres = value;}
	}

	public int CantidadCuotas
	{
		get{return _cantidadCuotas;}
		set{_cantidadCuotas = value;}
	}

    public DateTime FechaConfirmacion
    {
        get { return _fechaConfirmacion; }
        set { _fechaConfirmacion = value; }
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

    public bool Incluir
    {
        get { return _incluir; }
        set { _incluir = value; }
    }

    public int RiesgoCrediticio
    {
        get { return _riesgoCrediticio; }
        set { _riesgoCrediticio = value; }
    }

    public int IdPrestamoPlan
    {
        get { return _idPrestamoPlan; }
        set { _idPrestamoPlan = value; }
    }

	#endregion
	}
}
