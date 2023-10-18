
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
    TGETiposValores _tipoValor;
	TESTesorerias _tesoreria;
	TGEMonedas _moneda;
	decimal _saldoInicial;
	decimal _ingreso;
	decimal _egreso;
	decimal _saldoFinal;
        decimal _saldoFinalCajas;
        decimal _saldoTesoreriaCajas;
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

      public TGETiposValores TipoValor
      {
          get { return _tipoValor == null ? (_tipoValor = new TGETiposValores()) : _tipoValor; }
          set { _tipoValor = value; }
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
        public decimal SaldoFinalCajas
        {
            get { return _saldoFinalCajas; }
            set { _saldoFinalCajas = value; }
        }        
            public decimal SaldoTesoreriaCajas
        {
            get { return _saldoTesoreriaCajas; }
            set { _saldoTesoreriaCajas = value; }
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
