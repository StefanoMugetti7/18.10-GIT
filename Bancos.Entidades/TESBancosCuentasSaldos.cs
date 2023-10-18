
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Bancos.Entidades
{
  [Serializable]
	public partial class TESBancosCuentasSaldos : Objeto
	{

	#region "Private Members"
	int _idBancoCuentaSaldo;
	TESBancosCuentas _bancoCuenta;
	int _periodo;
	decimal _saldoInicial;
	decimal _debito;
	decimal _credito;
	decimal _saldoFinal;
	#endregion
		
	#region "Constructors"
	public TESBancosCuentasSaldos()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdBancoCuentaSaldo
	{
		get{return _idBancoCuentaSaldo ;}
		set{_idBancoCuentaSaldo = value;}
	}

	public TESBancosCuentas BancoCuenta
	{
        get { return _bancoCuenta == null ? (_bancoCuenta = new TESBancosCuentas()) : _bancoCuenta; }
		set{_bancoCuenta = value;}
	}

	public int Periodo
	{
		get{return _periodo;}
		set{_periodo = value;}
	}

	public decimal SaldoInicial
	{
		get{return _saldoInicial;}
		set{_saldoInicial = value;}
	}

	public decimal Debito
	{
		get{return _debito;}
		set{_debito = value;}
	}

	public decimal Credito
	{
		get{return _credito;}
		set{_credito = value;}
	}

	public decimal SaldoFinal
	{
		get{return _saldoFinal;}
		set{_saldoFinal = value;}
	}

	#endregion
	}
}
