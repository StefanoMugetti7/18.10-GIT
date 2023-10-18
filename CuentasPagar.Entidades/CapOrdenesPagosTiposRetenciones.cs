
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace CuentasPagar.Entidades
{
  [Serializable]
	public partial class CapOrdenesPagosTiposRetenciones : Objeto
	{
		// Class CapOrdenesPagosTiposRetenciones
	#region "Private Members"
	int _idOrdenPagoTipoRetencion;
	int _idOrdenPago;
	TGETiposRetenciones _tipoRetencion;
	string _numeroCertificado;
	string _concepto;
	decimal _importeTotalRetencion;
    List<CapOrdenesPagosTiposRetencionesDetalles> _ordenesPagosTiposRetencionesDetalle;
	#endregion
		
	#region "Constructors"
	public CapOrdenesPagosTiposRetenciones()
	{
	}

    public CapOrdenesPagosTiposRetenciones(int pIdTipoRetencion, decimal pImporteTotal, string pNumeroCertificado, string pConcepto)
    {
        this.TipoRetencion.IdTipoRetencion = pIdTipoRetencion;
        this.ImporteTotalRetencion = pIdTipoRetencion;
        this.NumeroCertificado = pNumeroCertificado;
        this.Concepto = pConcepto;
    }
	#endregion
		
	#region "Public Properties"

      [PrimaryKey]
	public int IdOrdenPagoTipoRetencion
	{
		get{return _idOrdenPagoTipoRetencion ;}
		set{_idOrdenPagoTipoRetencion = value;}
	}
	public int IdOrdenPago
	{
		get{return _idOrdenPago;}
		set{_idOrdenPago = value;}
	}

    public TGETiposRetenciones TipoRetencion
	{
        get { return _tipoRetencion == null ? (_tipoRetencion = new TGETiposRetenciones()) : _tipoRetencion; }
		set{_tipoRetencion = value;}
	}

	public string NumeroCertificado
	{
		get{return _numeroCertificado == null ? string.Empty : _numeroCertificado ;}
		set{_numeroCertificado = value;}
	}

	public string Concepto
	{
		get{return _concepto == null ? string.Empty : _concepto ;}
		set{_concepto = value;}
	}

	public decimal ImporteTotalRetencion
	{
		get{return _importeTotalRetencion;}
		set{_importeTotalRetencion = value;}
	}

    public List<CapOrdenesPagosTiposRetencionesDetalles> OrdenesPagosTiposRetencionesDetalle
    {
        get { return _ordenesPagosTiposRetencionesDetalle == null ? (_ordenesPagosTiposRetencionesDetalle = new List<CapOrdenesPagosTiposRetencionesDetalles>()) : _ordenesPagosTiposRetencionesDetalle; }
        set { _ordenesPagosTiposRetencionesDetalle = value; }
    }

	#endregion
	}
}
