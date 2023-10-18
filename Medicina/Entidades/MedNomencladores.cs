
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Medicina.Entidades
{
  [Serializable]
	public partial class MedNomencladores : Objeto
	{
		// Class MedNomenclador
	#region "Private Members"
	int _idNomenclador;
	string _codigo;
	string _prestacion;
	int _idTipoPrestacion;

    //List<MedPrestaciones> _medPrestaciones;
	#endregion
		
	#region "Constructors"
	public MedNomencladores()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdNomenclador
	{
		get{return _idNomenclador ;}
		set{_idNomenclador = value;}
	}
	public string Codigo
	{
		get{return _codigo == null ? string.Empty : _codigo ;}
		set{_codigo = value;}
	}

	public string Prestacion
	{
		get{return _prestacion == null ? string.Empty : _prestacion ;}
		set{_prestacion = value;}
	}

	public int IdTipoPrestacion
	{
		get{return _idTipoPrestacion;}
		set{_idTipoPrestacion = value;}
	}

    //public List<MedPrestaciones> medPrestaciones
    //{
    //    get{return _medPrestaciones==null ? (_medPrestaciones = new List<MedPrestaciones>()) : _medPrestaciones;}
    //    set{_medPrestaciones = value;}
    //}				

	#endregion
	}
}
