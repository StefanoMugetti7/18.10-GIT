
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Seguridad.Entidades;
using Generales.Entidades;
namespace Cargos.Entidades
{
  [Serializable]
	public partial class CarTiposCargosAfiliadosFormasCobros : Objeto
	{
	#region "Private Members"
	int _idTipoCargoAfiliadoFormaCobro;
    DateTime _fechaAlta;
    DateTime _fechaAltaEvento;
    int _cantidadCuotas;
    int _ultimaCuotaPaga;
    decimal _importeCuota;
    decimal _importeTotal;
    DateTime _fechaFinVigencia;
    int _idAfiliado;
    int _periodo;
    UsuariosAlta _usuarioAlta;
	CarTiposCargos _tipoCargo;
	TGEFormasCobrosAfiliados _formaCobroAfiliado;
    List<TGEArchivos> _archivos;
    List<TGEComentarios> _comentarios;
    List<TGECampos> _campos;
	#endregion
		
	#region "Constructors"
	public CarTiposCargosAfiliadosFormasCobros()
	{
	}
	#endregion
		
	#region "Public Properties"

    [PrimaryKey()]
	public int IdTipoCargoAfiliadoFormaCobro
	{
		get{return _idTipoCargoAfiliadoFormaCobro ;}
		set{_idTipoCargoAfiliadoFormaCobro = value;}
	}

    public DateTime FechaAlta
    {
        get { return _fechaAlta; }
        set { _fechaAlta = value; }
    }

    public DateTime FechaAltaEvento
    {
        get { return _fechaAltaEvento; }
        set { _fechaAltaEvento = value; }
    }

    [Auditoria()]
    public int CantidadCuotas
    {
        get { return _cantidadCuotas; }
        set { _cantidadCuotas = value; }
    }

    public int UltimaCuotaPaga
    {
        get { return _ultimaCuotaPaga; }
        set { _ultimaCuotaPaga = value; }
    }

    [Auditoria()]
    public decimal ImporteCuota
    {
        get { return _importeCuota; }
        set { _importeCuota = value; }
    }

    public decimal ImporteTotal
    {
        get { return _importeTotal; }
        set { _importeTotal = value; }
    }

    public int IdAfiliado
    {
        get { return _idAfiliado; }
        set { _idAfiliado = value; }
    }

    public DateTime FechaFinVigencia
    {
        get { return _fechaFinVigencia; }
        set { _fechaFinVigencia = value; }
    }

    public int Periodo
    {
        get { return _periodo; }
        set { _periodo = value; }
    }

    public UsuariosAlta UsuarioAlta
    {
        get { return _usuarioAlta == null ? (_usuarioAlta = new UsuariosAlta()) : _usuarioAlta; }
        set { _usuarioAlta = value; }
    }

    public CarTiposCargos TipoCargo
	{
        get { return _tipoCargo == null ? (_tipoCargo = new CarTiposCargos()) : _tipoCargo; }
		set{_tipoCargo = value;}
	}

      [Auditoria()]
    public TGEFormasCobrosAfiliados FormaCobroAfiliado
	{
        get { return _formaCobroAfiliado == null ? (_formaCobroAfiliado = new TGEFormasCobrosAfiliados()) : _formaCobroAfiliado; }
		set{_formaCobroAfiliado = value;}
	}

    public List<TGEArchivos> Archivos
    {
        get { return _archivos == null ? (_archivos = new List<TGEArchivos>()) : _archivos; }
        set { _archivos = value; }
    }

    public List<TGEComentarios> Comentarios
    {
        get { return _comentarios == null ? (_comentarios = new List<TGEComentarios>()) : _comentarios; }
        set { _comentarios = value; }
    }

    public List<TGECampos> Campos
    {
        get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
        set { _campos = value; }
    }

	#endregion
	}
}
