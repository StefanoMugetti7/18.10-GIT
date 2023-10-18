
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Compras.Entidades;
using Generales.Entidades;
using Contabilidad.Entidades;

namespace Facturas.Entidades
{
  [Serializable]
	public partial class VTAFacturacionesHabitualesDetalles : Objeto
	{
		// Class VTAFacturacionesHabitualesDetalles
	#region "Private Members"
	int _idFacturacionHabitualDetalle;
	int _idFacturacionHabitual;
	CMPProductos _producto;
    decimal _cantidad;
	decimal _importe;
	TGEIVA _iVA;
	CtbCentrosCostosProrrateos _centroCostoProrrateo;

		#endregion

		#region "Constructors"
		public VTAFacturacionesHabitualesDetalles()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey]
	public int IdFacturacionHabitualDetalle
	{
		get{return _idFacturacionHabitualDetalle ;}
		set{_idFacturacionHabitualDetalle = value;}
	}
	public int IdFacturacionHabitual
	{
		get{return _idFacturacionHabitual;}
		set{_idFacturacionHabitual = value;}
	}
	public CtbCentrosCostosProrrateos CentroCostoProrrateo
	{
		get { return _centroCostoProrrateo == null ? (_centroCostoProrrateo = new CtbCentrosCostosProrrateos()) : _centroCostoProrrateo; }
		set { _centroCostoProrrateo = value; }
	}

	[Auditoria]
    public CMPProductos Producto
	{
		get{return _producto==null? (_producto=new CMPProductos()) : _producto;}
		set{_producto = value;}
	}
      [Auditoria]
      public decimal Cantidad
      {
          get { return _cantidad; }
          set { _cantidad = value; }
      }

      [Auditoria]
	public decimal Importe
	{
		get{return _importe;}
		set{_importe = value;}
	}
      [Auditoria]
      public TGEIVA IVA
	{
        get { return _iVA == null ? (_iVA = new TGEIVA()) : _iVA; }
		set{_iVA = value;}
	}
	#endregion
	}
}
