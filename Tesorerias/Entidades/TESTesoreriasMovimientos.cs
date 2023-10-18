
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace Tesorerias.Entidades
{
  [Serializable]
	public partial class TESTesoreriasMovimientos : Objeto
	{
	#region "Private Members"
	int _idTesoreriaMovimiento;
	TESTesoreriasMonedas _tesoreriaMoneda;
	TGETiposOperaciones _tipoOperacion;
    int _idRefTipoOperacion;
	DateTime _fecha;
	string _descripcion;
	decimal _importe;
	TGETiposValores _tipoValor;
    TESCajas _caja;
	#endregion
		
	#region "Constructors"
	public TESTesoreriasMovimientos()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdTesoreriaMovimiento
	{
		get{return _idTesoreriaMovimiento ;}
		set{_idTesoreriaMovimiento = value;}
	}
	public TESTesoreriasMonedas TesoreriaMoneda
	{
        get { return _tesoreriaMoneda == null ? (_tesoreriaMoneda = new TESTesoreriasMonedas()) : _tesoreriaMoneda; }
		set{_tesoreriaMoneda = value;}
	}

    public TGETiposOperaciones TipoOperacion
	{
        get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
		set{_tipoOperacion = value;}
	}

    public int IdRefTipoOperacion
    {
        get { return _idRefTipoOperacion; }
        set { _idRefTipoOperacion = value; }
    }

	public DateTime Fecha
	{
		get{return _fecha;}
		set{_fecha = value;}
	}

	public string Descripcion
	{
		get{return _descripcion == null ? string.Empty : _descripcion ;}
		set{_descripcion = value;}
	}

	public decimal Importe
	{
		get{return _importe;}
		set{_importe = value;}
	}

	public TGETiposValores TipoValor
	{
        get { return _tipoValor == null ? (_tipoValor = new TGETiposValores()) : _tipoValor; }
		set{_tipoValor = value;}
	}

    public TESCajas Caja
    {
        get { return _caja == null ? (_caja = new TESCajas()) : _caja; }
        set { _caja = value; }
    }

	#endregion
	}
}
