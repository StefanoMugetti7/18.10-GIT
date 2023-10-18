
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Haberes.Entidades
{
  [Serializable]
	public partial class HabRemesas : Objeto
	{

	#region "Private Members"
	int _idRemesa;
	int _periodo;
	int _cantidadRegistros;
	decimal _importeTotal;
    int _cantidadDepositar;
    decimal _importeDepositar;
    int _cantidadDepositada;
    decimal _importeDepositado;
	UsuariosAlta _usuarioAlta;
    List<HabRemesasDetalles> _remesasDetalles;
	
	#endregion
		
	#region "Constructors"
	public HabRemesas()
	{
	}
	#endregion
		
	#region "Public Properties"

      [PrimaryKey()]
	public int IdRemesa
	{
		get{return _idRemesa ;}
		set{_idRemesa = value;}
	}
	public int Periodo
	{
		get{return _periodo;}
		set{_periodo = value;}
	}

	public int CantidadRegistros
	{
		get{return _cantidadRegistros;}
		set{_cantidadRegistros = value;}
	}

	public decimal ImporteTotal
	{
		get{return _importeTotal;}
		set{_importeTotal = value;}
	}

    public int CantidadDepositar
    {
        get { return _cantidadDepositar; }
        set { _cantidadDepositar = value; }
    }

    public decimal ImporteDepositar
    {
        get { return _importeDepositar; }
        set { _importeDepositar = value; }
    }

    public int CantidadDepositada
    {
        get { return _cantidadDepositada; }
        set { _cantidadDepositada = value; }
    }

    public decimal ImporteDepositado
    {
        get { return _importeDepositado; }
        set { _importeDepositado = value; }
    }

	public UsuariosAlta UsuarioAlta
	{
        get { return _usuarioAlta == null ? (_usuarioAlta = new UsuariosAlta()) : _usuarioAlta; }
		set{_usuarioAlta = value;}
	}

    public List<HabRemesasDetalles> RemesasDetalles
    {
        get { return _remesasDetalles == null ? (_remesasDetalles = new List<HabRemesasDetalles>()) : _remesasDetalles; }
        set { _remesasDetalles = value; }
    }

	#endregion
	}
}
