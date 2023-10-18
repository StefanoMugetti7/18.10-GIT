
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using System.Linq;
using Seguridad.Entidades;
namespace Tesorerias.Entidades
{
  [Serializable]
	public partial class TESTesorerias : Objeto
	{
	#region "Private Members"
	int _idTesoreria;
	TGEFiliales _filial;
	DateTime _fechaAbrir;
	UsuariosAbrir _usuarioAbrir;
	DateTime _fechaCerrar;
	UsuariosCerrar _usuarioCerrar;
	List<TESCajas> _cajas;
	List<TESTesoreriasMonedas> _tesoreriasMonedas;
    List<TESTesoreriasMovimientos> _movimientos;
	#endregion
		
	#region "Constructors"
	public TESTesorerias()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdTesoreria
	{
		get{return _idTesoreria ;}
		set{_idTesoreria = value;}
	}
	public TGEFiliales Filial
	{
        get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
		set{_filial = value;}
	}

	public DateTime FechaAbrir
	{
		get{return _fechaAbrir;}
		set{_fechaAbrir = value;}
	}

	public UsuariosAbrir UsuarioAbrir
	{
        get { return _usuarioAbrir == null ? (_usuarioAbrir = new UsuariosAbrir()) : _usuarioAbrir; }
		set{_usuarioAbrir = value;}
	}

	public DateTime FechaCerrar
	{
		get{return _fechaCerrar;}
		set{_fechaCerrar = value;}
	}

	public UsuariosCerrar UsuarioCerrar
	{
        get { return _usuarioCerrar == null ? (_usuarioCerrar = new UsuariosCerrar()) : _usuarioCerrar; }
		set{_usuarioCerrar = value;}
	}

	public List<TESCajas> Cajas
	{
		get{return _cajas==null ? (_cajas = new List<TESCajas>()) : _cajas;}
		set{_cajas = value;}
	}
				
	public List<TESTesoreriasMonedas> TesoreriasMonedas
	{
		get{return _tesoreriasMonedas==null ? (_tesoreriasMonedas = new List<TESTesoreriasMonedas>()) : _tesoreriasMonedas;}
		set{_tesoreriasMonedas = value;}
	}

    public List<TESTesoreriasMovimientos> TesoreriasMovimientos
    {
        get
        {
            if (_movimientos == null)
            {
                _movimientos = new List<TESTesoreriasMovimientos>();
            }
            foreach (var moneda in this.TesoreriasMonedas)
            {
                foreach (var movimiento in moneda.TesoreriasMovimientos)
                {
                    _movimientos.Add(movimiento);
                }
            }
            return _movimientos;
        }
        set { _movimientos = value; }
    }

    public List<TESTesoreriasMovimientos> ObtenerTesoreriasMovimientos()
    {
        List<TESTesoreriasMovimientos> lista = new List<TESTesoreriasMovimientos>();
        foreach (TESTesoreriasMonedas moneda in this.TesoreriasMonedas)
            foreach (TESTesoreriasMovimientos mov in moneda.TesoreriasMovimientos)
                lista.Add(mov);

        return lista.OrderBy(x => x.TesoreriaMoneda.Moneda.Moneda).ThenBy(x => x.Fecha).ToList();
    }
	#endregion
	}
}
