
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace Afiliados.Entidades
{
  [Serializable]
	public partial class AfiCategorias : Objeto
	{
	#region "Private Members"
	int _idCategoria;
	string _categoria;
    decimal _importeCuota;
    decimal _importeGasto;
    List<TGEArchivos> _archivos;
    List<TGEComentarios> _comentarios;
	#endregion
		
	#region "Constructors"
	public AfiCategorias()
	{
	}
	#endregion
		
	#region "Public Properties"

    [PrimaryKey()]
	public int IdCategoria
	{
		get{return _idCategoria ;}
		set{_idCategoria = value;}
	}
	public string Categoria
	{
		get{return _categoria == null ? string.Empty : _categoria ;}
		set{_categoria = value;}
	}

    public decimal ImporteCuota
    {
        get { return _importeCuota; }
        set { _importeCuota = value; }
    }

    public decimal ImporteGasto
    {
        get { return _importeGasto; }
        set { _importeGasto = value; }
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

	#endregion
	}
}
