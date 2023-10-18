
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace Tesorerias.Entidades
{
  [Serializable]
	public partial class TESCajasMonedas : Objeto
	{
	#region "Private Members"
	int _idCajaMoneda;
	TESCajas _caja;
	TGEMonedas _moneda;
	decimal _saldoInicial;
	decimal _ingreso;
	decimal _egreso;
	decimal _saldoFinal;
	List<TESCajasMovimientos> _cajasMovimientos;
	#endregion
		
	#region "Constructors"
	public TESCajasMonedas()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdCajaMoneda
	{
		get{return _idCajaMoneda ;}
		set{_idCajaMoneda = value;}
	}

	public TESCajas Caja
	{
        get { return _caja == null ? (_caja = new TESCajas()) : _caja; }
		set{_caja = value;}
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

	public List<TESCajasMovimientos> CajasMovimientos
	{
		get{return _cajasMovimientos==null ? (_cajasMovimientos = new List<TESCajasMovimientos>()) : _cajasMovimientos;}
		set{_cajasMovimientos = value;}
	}				

	#endregion
	}
}
