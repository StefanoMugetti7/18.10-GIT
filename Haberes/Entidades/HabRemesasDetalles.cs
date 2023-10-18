
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Afiliados.Entidades;
namespace Haberes.Entidades
{
  [Serializable]
	public partial class HabRemesasDetalles : Objeto
	{

	#region "Private Members"
	int _idRemesaDetalle;
	int _idRemesa;
	AfiAfiliados _afiliado;
	decimal _netoIAF;
	int _numeroDocumentoIAF;
	int _idTipoDocumentoIAF;
    string _tipoDocumentoIAF;
	int _idEstadoAfiliado;
    string _estadoAfiliado;
	int _idCategoriaAfiliado;
    string _categoriaAfiliado;
    string _apellidoNombre;
    int _periodo;
    HabRemesasTipos _remesaTipo;
	#endregion
		
	#region "Constructors"
	public HabRemesasDetalles()
	{
	}
	#endregion
		
	#region "Public Properties"

      [PrimaryKey()]
	public int IdRemesaDetalle
	{
		get{return _idRemesaDetalle ;}
		set{_idRemesaDetalle = value;}
	}
	public int IdRemesa
	{
		get{return _idRemesa;}
		set{_idRemesa = value;}
	}

	public AfiAfiliados Afiliado
	{
        get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; }
        set { _afiliado = value; }
	}

	public decimal NetoIAF
	{
		get{return _netoIAF;}
		set{_netoIAF = value;}
	}

      [Auditoria()]
	public int NumeroDocumentoIAF
	{
		get{return _numeroDocumentoIAF;}
		set{_numeroDocumentoIAF = value;}
	}

	public int IdTipoDocumentoIAF
	{
		get{return _idTipoDocumentoIAF;}
		set{_idTipoDocumentoIAF = value;}
	}

    [Auditoria()]
    public string TipoDocumentoIAF
    {
        get { return _tipoDocumentoIAF == null ? string.Empty : _tipoDocumentoIAF; }
        set { _tipoDocumentoIAF = value; }
    }

	public int IdEstadoAfiliado
	{
		get{return _idEstadoAfiliado;}
		set{_idEstadoAfiliado = value;}
	}

    [Auditoria()]
    public string EstadoAfiliado
    {
        get { return _estadoAfiliado == null ? string.Empty : _estadoAfiliado; }
        set { _estadoAfiliado = value; }
    }

	public int IdCategoriaAfiliado
	{
		get{return _idCategoriaAfiliado;}
		set{_idCategoriaAfiliado = value;}
	}

    [Auditoria()]
    public string CategoriaAfiliado
    {
        get { return _categoriaAfiliado == null ? string.Empty : _categoriaAfiliado; }
        set { _categoriaAfiliado = value; }
    }

    [Auditoria()]
    public string ApellidoNombre
    {
        get { return _apellidoNombre == null ? string.Empty : _apellidoNombre; }
        set { _apellidoNombre = value; }
    }

    public int Periodo
    {
        get { return _periodo; }
        set { _periodo = value; }
    }

    public HabRemesasTipos RemesaTipo
    {
        get { return _remesaTipo == null ? (_remesaTipo = new HabRemesasTipos()) : _remesaTipo; }
        set { _remesaTipo = value; }
    }
	
	#endregion
	}
}
