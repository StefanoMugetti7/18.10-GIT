
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace Tesorerias.Entidades
{
  [Serializable]
	public partial class TESTesoreriasMonedas : Objeto
	{
	#region "Private Members"
	int _idTesoreriaMoneda;
	TESTesorerias _tesoreria;
	TGEMonedas _moneda;
	decimal _saldoInicial;
	decimal _ingreso;
	decimal _egreso;
	decimal _saldoFinal;
	
	List<TESTesoreriasMovimientos> _tesoreriasMovimientos;
	#endregion
		
	#region "Constructors"
	public TESTesoreriasMonedas()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdTesoreriaMoneda
	{
		get{return _idTesoreriaMoneda ;}
		set{_idTesoreriaMoneda = value;}
	}
     
	public TESTesorerias Tesoreria
	{
        get { return _tesoreria == null ? (_tesoreria = new TESTesorerias()) : _tesoreria; }
		set{_tesoreria = value;}
	}

	public TGEMonedas Moneda
	{
        get { return _moneda == null ? (_moneda = new TGEMonedas()) : _moneda; }
		set{_moneda = value;}
	}

	public decimal SaldoInicial
	{
		get{return _saldoInicial;}
		set{_saldoInicial = value;}
	}

	public decimal Ingreso
	{
		get{return _ingreso;}
		set{_ingreso = value;}
	}

	public decimal Egreso
	{
		get{return _egreso;}
		set{_egreso = value;}
	}

	public decimal SaldoFinal
	{
		get{return _saldoFinal;}
		set{_saldoFinal = value;}
	}

    public decimal SaldoActual
    {
        get { return this.SaldoInicial + this.Ingreso - this.Egreso; }
        set { }
    }

	public List<TESTesoreriasMovimientos> TesoreriasMovimientos
	{
		get{return _tesoreriasMovimientos==null ? (_tesoreriasMovimientos = new List<TESTesoreriasMovimientos>()) : _tesoreriasMovimientos;}
		set{_tesoreriasMovimientos = value;}
	}

    public string miMonedaDescripcion
    {
        get { return Moneda.Descripcion; }
    }

	#endregion
	}
}
