
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace Cobros.Entidades
{
  [Serializable]
	public partial class CobOrdenesCobrosTiposRetenciones : Objeto
	{
		// Class CapOrdenesCobrosTiposRetenciones
	#region "Private Members"
	int _idOrdenCobroTipoRetencion;
	int _idOrdenCobro;
	TGETiposRetenciones _tipoRetencion;
	string _numeroCertificado;
	string _concepto;
	decimal _importeTotalRetencion;
    int? _idRefImpuestoRetencion;
    //List<CobOrdenesCobrosTiposRetencionesDetalles> _ordenesCobrosTiposRetencionesDetalle;
	#endregion
		
	#region "Constructors"
	public CobOrdenesCobrosTiposRetenciones()
	{
	}

    public CobOrdenesCobrosTiposRetenciones(int pIdTipoRetencion, decimal pImporteTotal, string pNumeroCertificado, string pConcepto)
    {
        this.TipoRetencion.IdTipoRetencion = pIdTipoRetencion;
        this.ImporteTotalRetencion = pIdTipoRetencion;
        this.NumeroCertificado = pNumeroCertificado;
        this.Concepto = pConcepto;
    }
	#endregion
		
	#region "Public Properties"

      [PrimaryKey]
	public int IdOrdenCobroTipoRetencion
	{
		get{return _idOrdenCobroTipoRetencion ;}
		set{_idOrdenCobroTipoRetencion = value;}
	}
	public int IdOrdenCobro
	{
		get{return _idOrdenCobro;}
		set{_idOrdenCobro = value;}
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

    public int? IdRefImpuestoRetencion
    {
        get { return _idRefImpuestoRetencion; }
        set { _idRefImpuestoRetencion = value; }
    }

    //public List<CobOrdenesCobrosTiposRetencionesDetalles> OrdenesCobrosTiposRetencionesDetalle
    //{
    //    get { return _ordenesCobrosTiposRetencionesDetalle == null ? (_ordenesCobrosTiposRetencionesDetalle = new List<CobOrdenesCobrosTiposRetencionesDetalles>()) : _ordenesCobrosTiposRetencionesDetalle; }
    //    set { _ordenesCobrosTiposRetencionesDetalle = value; }
    //}

	#endregion
	}
}
