
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Afiliados.Entidades;
using Generales.Entidades;
namespace Bancos.Entidades
{
  [Serializable]
	public partial class TESCheques : Objeto
	{

	#region "Private Members"
	int _idCheque;
	DateTime _fecha;
	DateTime _fechaDiferido;
	string _concepto;
	string _numeroCheque;
    decimal _importe;
    string _titularCheque;
    string _cUIT;
    TGETiposOperaciones _tipoOperacion;
	TESBancos _banco;
    TGEFiliales _filial;
    AfiAfiliados _afiliado;
	int _idCajaMovimientoValor;
	List<TESChequesMovimientos> _chequesMovimientos;
    DateTime? _fechaDesde;
    DateTime? _fechaHasta;
    DateTime? _fechaDiferidoDesde;
    DateTime? _fechaDiferidoHasta;
    int _idBancoCuenta;
    bool _chequePropio;
    
	#endregion
		
	#region "Constructors"
	public TESCheques()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdCheque
	{
		get{return _idCheque ;}
		set{_idCheque = value;}
	}
      [Auditoria()]
	public DateTime Fecha
	{
		get{return _fecha;}
		set{_fecha = value;}
	}
      [Auditoria()]
	public DateTime FechaDiferido
	{
		get{return _fechaDiferido;}
		set{_fechaDiferido = value;}
	}
      [Auditoria()]
	public string Concepto
	{
		get{return _concepto == null ? string.Empty : _concepto ;}
		set{_concepto = value;}
	}
      [Auditoria()]
	public string NumeroCheque
	{
		get{return _numeroCheque == null ? string.Empty : _numeroCheque ;}
		set{_numeroCheque = value;}
	}
      [Auditoria()]
    public decimal Importe
    {
        get { return _importe; }
        set { _importe = value; }
    }
      [Auditoria()]
    public string TitularCheque
    {
        get { return _titularCheque == null ? string.Empty : _titularCheque; }
        set { _titularCheque = value; }
    }
      [Auditoria()]
    public string CUIT
    {
        get { return _cUIT == null ? string.Empty : _cUIT; }
        set { _cUIT = value; }
    }

    //public int IdOrdenCobroFormaCobro
    //{
    //    get { return _idOrdenCobroFormaCobro; }
    //    set { _idOrdenCobroFormaCobro = value; }
    //}

    public TGETiposOperaciones TipoOperacion
    {
        get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
        set { _tipoOperacion = value; }
    }
      [Auditoria()]
	public TESBancos Banco
	{
        get { return _banco == null ? (_banco = new TESBancos()) : _banco; }
		set{_banco = value;}
	}
      [Auditoria()]
    public TGEFiliales Filial
    {
        get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
        set { _filial = value; }
    }

    public AfiAfiliados Afiliado
    {
        get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; }
        set { _afiliado = value; }
    }

    public int IdCajaMovimientoValor
    {
        get { return _idCajaMovimientoValor; }
        set { _idCajaMovimientoValor = value; }
    }

	public List<TESChequesMovimientos> ChequesMovimientos
	{
		get{return _chequesMovimientos==null ? (_chequesMovimientos = new List<TESChequesMovimientos>()) : _chequesMovimientos;}
		set{_chequesMovimientos = value;}
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

    public DateTime? FechaDiferidoDesde
    {
        get { return _fechaDiferidoDesde; }
        set { _fechaDiferidoDesde = value; }
    }

    public DateTime? FechaDiferidoHasta
    {
        get { return _fechaDiferidoHasta; }
        set { _fechaDiferidoHasta = value; }
    }

    public int IdBancoCuenta
    {
        get { return _idBancoCuenta; }
        set { _idBancoCuenta = value; }
    }

    public bool ChequePropio
    {
        get { return _chequePropio; }
        set { _chequePropio = value; }
    }

	#endregion
	}
}
