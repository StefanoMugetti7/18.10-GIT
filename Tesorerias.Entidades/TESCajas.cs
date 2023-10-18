
using System;
using System.Collections.Generic;
using System.Linq;
using Comunes.Entidades;
using Generales.Entidades;
namespace Tesorerias.Entidades
{
  [Serializable]
	public partial class TESCajas : Objeto
	{

	#region "Private Members"
	int _idCaja;
    int _numeroCaja;
    DateTime _fechaAbrirEvento;
    DateTime _fechaAbrir;
    DateTime _fechaCerrar;
    Usuarios _usuario;
	TESTesorerias _tesoreria;
	List<TESCajasMonedas> _cajasMonedas;
    bool _traspasarFondos;
	#endregion
		
	#region "Constructors"
	public TESCajas()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
    public int IdCaja
    {
        get{return _idCaja ;}
        set{_idCaja = value;}
    }

      public int NumeroCaja
      {
          get { return _numeroCaja; }
          set { _numeroCaja = value; }
      }

      public DateTime FechaAbrirEvento
      {
          get { return _fechaAbrirEvento; }
          set { _fechaAbrirEvento = value; }
      }

      public DateTime FechaAbrir
      {
          get { return _fechaAbrir; }
          set { _fechaAbrir = value; }
      }

      public DateTime FechaCerrar
      {
          get { return _fechaCerrar; }
          set { _fechaCerrar = value; }
      }

      public bool TraspasarFondos
      {
          get { return _traspasarFondos; }
          set { _traspasarFondos = value; }
      }

    public Usuarios Usuario
      {
          get { return _usuario == null ? (_usuario = new Usuarios()) : _usuario; }
          set { _usuario = value; }
      }

	public TESTesorerias Tesoreria
	{
        get { return _tesoreria == null ? (_tesoreria = new TESTesorerias()) : _tesoreria;}
		set{_tesoreria = value;}
	}

	public List<TESCajasMonedas> CajasMonedas
	{
		get{return _cajasMonedas==null ? (_cajasMonedas = new List<TESCajasMonedas>()) : _cajasMonedas;}
		set{_cajasMonedas = value;}
	}

    public List<TESCajasMovimientos> ObtenerCajasMovimientos()
    {
        List<TESCajasMovimientos> lista = new List<TESCajasMovimientos>();
        foreach (TESCajasMonedas moneda in this.CajasMonedas)
            foreach (TESCajasMovimientos mov in moneda.CajasMovimientos)
                lista.Add(mov);

        return lista.OrderBy(x => x.CajaMoneda.Moneda.Moneda).ThenBy(x => x.Fecha).ToList();
    }

    public int miUsuarioIdUsuario
    {
        get { return Usuario.IdUsuario; }
    }

    public string miUsuarioApellidoNombre
    {
        get { return Usuario.ApellidoNombre; }
    }
	#endregion
	}
}
