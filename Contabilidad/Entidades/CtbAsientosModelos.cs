
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Contabilidad.Entidades
{
  [Serializable]
	public partial class CtbAsientosModelos : Objeto
	{
		// Class CtbAsientosModelos
	#region "Private Members"
	int _idAsientoModelo;
    CtbTiposAsientos _tipoAsiento;
	string _detalle;
    TGETiposOperaciones _tipoOperacion;
	List<CtbAsientosModelosDetalles> _asientosModelosDetalles;
	#endregion
		
	#region "Constructors"
	public CtbAsientosModelos()
	{
	}
	#endregion
		
	#region "Public Properties"

      [PrimaryKey()]
	public int IdAsientoModelo
	{
		get{return _idAsientoModelo ;}
		set{_idAsientoModelo = value;}
	}

      [Auditoria()]
    public CtbTiposAsientos TipoAsiento
	{
        get { return _tipoAsiento == null ? (_tipoAsiento = new CtbTiposAsientos()) : _tipoAsiento; }
		set{_tipoAsiento = value;}
	}

      [Auditoria()]
	public string Detalle
	{
		get{return _detalle == null ? string.Empty : _detalle ;}
		set{_detalle = value;}
	}

      [Auditoria()]
    public TGETiposOperaciones TipoOperacion 
    {
        get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
        set { _tipoOperacion = value; }
    }
	
	public List<CtbAsientosModelosDetalles> AsientosModelosDetalles
	{
		get{return _asientosModelosDetalles==null ? (_asientosModelosDetalles = new List<CtbAsientosModelosDetalles>()) : _asientosModelosDetalles;}
		set{_asientosModelosDetalles = value;}
	}				

	#endregion
	}
}
