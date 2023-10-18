
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Bancos.Entidades;
using Generales.Entidades;
namespace Cobros.Entidades
{
  [Serializable]
	public partial class CobOrdenesCobrosFormasCobros : Objeto
	{
	
	#region "Private Members"
	int _idOrdenCobroFormaCobro;
	int _idOrdenCobro;
	TGETiposValores _tipoValor;
	decimal _importe;
	List<TESCheques> _cheques;
	#endregion
		
	#region "Constructors"
	public CobOrdenesCobrosFormasCobros()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdOrdenCobroFormaCobro
	{
		get{return _idOrdenCobroFormaCobro ;}
		set{_idOrdenCobroFormaCobro = value;}
	}
	public int IdOrdenCobro
	{
		get{return _idOrdenCobro;}
		set{_idOrdenCobro = value;}
	}

    public TGETiposValores TipoValor
	{
        get { return _tipoValor == null ? (_tipoValor = new TGETiposValores()) : _tipoValor; }
		set{_tipoValor = value;}
	}

	public decimal Importe
	{
		get{return _importe;}
		set{_importe = value;}
	}

	public List<TESCheques> Cheques
	{
		get{return _cheques==null ? (_cheques = new List<TESCheques>()) : _cheques;}
		set{_cheques = value;}
	}				

	#endregion
	}
}
