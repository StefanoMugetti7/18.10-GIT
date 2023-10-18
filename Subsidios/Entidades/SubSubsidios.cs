
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace Subsidios.Entidades
{
  [Serializable]
	public partial class SubSubsidios : Objeto
	{

	#region "Private Members"
	int _idSubsidio;
	string _descripcion;
	int _mesesCarencia;
    int _mesesValidezEvento;
    SubSubsidiosTipos _subsidioTipo;
	int _frecuenciaAnual;
	int _cantidadMaxima;
    List<SubEscalas> _escalas;
    List<TGECampos> _campos;
	#endregion

		
	#region "Constructors"
	public SubSubsidios()
	{
	}
	#endregion
		
	#region "Public Properties"

      [PrimaryKey()]
	public int IdSubsidio
	{
		get{return _idSubsidio ;}
		set{_idSubsidio = value;}
	}

      [Auditoria()]
	public string Descripcion
	{
		get{return _descripcion == null ? string.Empty : _descripcion ;}
		set{_descripcion = value;}
	}

      [Auditoria()]
	public int MesesCarencia
	{
		get{return _mesesCarencia;}
		set{_mesesCarencia = value;}
	}

      [Auditoria()]
      public int MesesValidezEvento
      {
          get { return _mesesValidezEvento; }
          set { _mesesValidezEvento = value; }
      }

      [Auditoria()]
	public SubSubsidiosTipos SubsidioTipo
	{
        get { return _subsidioTipo == null ? (_subsidioTipo = new SubSubsidiosTipos()) : _subsidioTipo; }
		set{_subsidioTipo = value;}
	}

      [Auditoria()]
	public int FrecuenciaAnual
	{
		get{return _frecuenciaAnual;}
		set{_frecuenciaAnual = value;}
	}

      [Auditoria()]
	public int CantidadMaxima
	{
		get{return _cantidadMaxima;}
		set{_cantidadMaxima = value;}
	}

      public List<SubEscalas> Escalas
      {
          get { return _escalas == null ? (_escalas = new List<SubEscalas>()) : _escalas; }
          set { _escalas = value; }
      }

      public List<TGECampos> Campos
      {
          get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
          set { _campos = value; }
      }
	#endregion
	}
}
