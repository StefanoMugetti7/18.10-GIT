
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Contabilidad.Entidades
{
  [Serializable]
	public partial class CtbBienesUsosDetalles : Objeto
	{
		// Class CtbBienesUsosDetalles
	#region "Private Members"
	int _idBienUsoDetalle;
	int _idBienUso;
	decimal? _importeAmortizado;
	CtbAsientosContables _asientoContable;

	#endregion
		
	#region "Constructors"
	public CtbBienesUsosDetalles()
	{
	}
	#endregion
		
	#region "Public Properties"
	public int IdBienUsoDetalle
	{
		get{return _idBienUsoDetalle ;}
		set{_idBienUsoDetalle = value;}
	}
	public int IdBienUso
	{
		get{return _idBienUso;}
		set{_idBienUso = value;}
	}

	public decimal? ImporteAmortizado
	{
		get{return _importeAmortizado;}
		set{_importeAmortizado = value;}
	}

    public CtbAsientosContables AsientoContable
	{
        get { return _asientoContable == null ? (_asientoContable = new CtbAsientosContables()) : _asientoContable; }
        set { _asientoContable = value; }
	}
	
	#endregion
	}
}
