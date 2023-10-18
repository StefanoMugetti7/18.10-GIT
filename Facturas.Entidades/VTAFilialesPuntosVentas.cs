
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace Facturas.Entidades
{
  [Serializable]
	public partial class VTAFilialesPuntosVentas : Objeto
	{
		// Class VTAFilialesPuntosVentas
	#region "Private Members"
	int _idFilialPuntoVenta;
	int _idFilial;
	int _afipPuntoVenta;
    int _idTipoFactura;
	//int? _idTipoPuntoVenta;
    string _descripcion;
    VTATiposPuntosVentas _tipoPuntoVenta;
    int _ultimoNumeroFacturaAnterior;
	#endregion
		
	#region "Constructors"
	public VTAFilialesPuntosVentas()
	{
	}
	#endregion
		
	#region "Public Properties"

      [PrimaryKey]
	public int IdFilialPuntoVenta
	{
		get{return _idFilialPuntoVenta ;}
		set{_idFilialPuntoVenta = value;}
	}
      [Auditoria]
	public int IdFilial
	{
		get{return _idFilial;}
		set{_idFilial = value;}
	}
      [Auditoria]
	public int AfipPuntoVenta
	{
		get{return _afipPuntoVenta;}
		set{_afipPuntoVenta = value;}
	}

    public string AfipPuntoVentaNumero
    {
        set { }
        get { return this.AfipPuntoVenta.ToString().PadLeft(4, '0'); }
    }
      [Auditoria]
    public int IdTipoFactura
    {
        get { return _idTipoFactura; }
        set { _idTipoFactura = value; }
    }

      [Auditoria]
    public string Descripcion
    {
        get { return _descripcion == null ? string.Empty : _descripcion; }
        set { _descripcion = value; }
    }
      [Auditoria]
    public VTATiposPuntosVentas TipoPuntoVenta
    {
        get { return _tipoPuntoVenta == null ? (_tipoPuntoVenta = new VTATiposPuntosVentas()) : _tipoPuntoVenta; }
        set { _tipoPuntoVenta = value; }
    }

    public string TipoPuntoVentaDescripcion
    {
        get { return this.TipoPuntoVenta.Descripcion; }
        set { }
    }

    public int UltimoNumeroFacturaAnterior
    {
        get { return _ultimoNumeroFacturaAnterior; }
        set { _ultimoNumeroFacturaAnterior = value; }
    }
	#endregion
	}
}
