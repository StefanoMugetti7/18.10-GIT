
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace Medicina.Entidades
{
  [Serializable]
	public partial class MedHistoriasClinicas : Objeto
	{
		// Class MedHistoriasClinicas
	#region "Private Members"
	int _idHistoriaClinica;
	int _idAfiliado;
	DateTime _fechaAlta;
	int _idUsuarioAlta;
    List<MedHistoriasClinicasEvoluciones> _historiasClinicasEvoluciones;
    List<TGEArchivos> _archivos;
    List<TGEComentarios> _comentarios;
    List<TGECampos> _campos;
	#endregion
		
	#region "Constructors"
	public MedHistoriasClinicas()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdHistoriaClinica
	{
		get{return _idHistoriaClinica ;}
		set{_idHistoriaClinica = value;}
	}
	public int IdAfiliado
	{
		get{return _idAfiliado;}
		set{_idAfiliado = value;}
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

    public List<MedHistoriasClinicasEvoluciones> HistoriasClinicasEvoluciones
    {
        get { return _historiasClinicasEvoluciones == null ? (_historiasClinicasEvoluciones = new List<MedHistoriasClinicasEvoluciones>()) : _historiasClinicasEvoluciones; }
        set { _historiasClinicasEvoluciones = value; }
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
