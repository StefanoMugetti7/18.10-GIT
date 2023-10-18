using System;
using Comunes.Entidades;
using Afiliados.Entidades;
using System.Collections.Generic;
namespace Ahorros.Entidades
{
  [Serializable]
	public partial class AhoCotitulares : Objeto
	{

	#region "Private Members"
	int _idCotitular;
	int? _idCuenta;
    int? _idPlazoFijo;
	DateTime _fechaAlta;
	int _idUsuarioAlta;
      AfiAfiliados _afiliado;
	#endregion
		
	#region "Constructors"
	public AhoCotitulares()
	{
	}
	#endregion
		
	#region "Public Properties"
	public int IdCotitular
	{
		get{return _idCotitular ;}
		set{_idCotitular = value;}
	}
	public int? IdCuenta
	{
		get{return _idCuenta;}
		set{_idCuenta = value;}
	}

    public int? IdPlazoFijo
    {
        get { return _idPlazoFijo; }
        set { _idPlazoFijo = value; }
    }

	public DateTime FechaAlta
	{
		get{return _fechaAlta;}
		set{_fechaAlta = value;}
	}

	public int IdUsuarioAlta
	{
		get{return _idUsuarioAlta;}
		set{_idUsuarioAlta = value;}
	}

      public AfiAfiliados Afiliado
	{
        get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; }
		set{_afiliado = value;}
	}
	#endregion
	}
}
