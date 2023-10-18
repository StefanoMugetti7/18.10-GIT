
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace CuentasPagar.Entidades
{
  [Serializable]
	public partial class CapOrdenesPagosTiposRetencionesDetalles : Objeto
	{
		// Class CapOrdenesPagosTiposRetencionesDetalles
	#region "Private Members"
	int _idOrdenPagoTipoRetencionDetalle;
	CapOrdenesPagosTiposRetenciones _ordenPagoTipoRetencion;
	int? _idRefImpuestoRetencion;
	decimal _importeMinimoImponible;
	decimal _importeMaximoImponible;
	decimal _importeFijo;
	decimal _alicuota;
	decimal _sobreExcedente;
	decimal _importeRetencion;
    decimal _retencionesAnterioresRealizadas;
    string _concepto;
	#endregion
		
	#region "Constructors"
	public CapOrdenesPagosTiposRetencionesDetalles()
	{
	}
	#endregion
		
	#region "Public Properties"

      [PrimaryKey]
	public int IdOrdenPagoTipoRetencionDetalle
	{
		get{return _idOrdenPagoTipoRetencionDetalle ;}
		set{_idOrdenPagoTipoRetencionDetalle = value;}
	}
	public CapOrdenesPagosTiposRetenciones OrdenPagoTipoRetencion
	{
        get { return _ordenPagoTipoRetencion == null ? (_ordenPagoTipoRetencion = new CapOrdenesPagosTiposRetenciones()) : _ordenPagoTipoRetencion; }
		set{_ordenPagoTipoRetencion = value;}
	}

	public int? IdRefImpuestoRetencion
	{
		get{return _idRefImpuestoRetencion;}
		set{_idRefImpuestoRetencion = value;}
	}

	public decimal ImporteMinimoImponible
	{
		get{return _importeMinimoImponible;}
		set{_importeMinimoImponible = value;}
	}

	public decimal ImporteMaximoImponible
	{
		get{return _importeMaximoImponible;}
		set{_importeMaximoImponible = value;}
	}

	public decimal ImporteFijo
	{
		get{return _importeFijo;}
		set{_importeFijo = value;}
	}

	public decimal Alicuota
	{
		get{return _alicuota;}
		set{_alicuota = value;}
	}

	public decimal SobreExcedente
	{
		get{return _sobreExcedente;}
		set{_sobreExcedente = value;}
	}

	public decimal ImporteRetencion
	{
		get{return _importeRetencion;}
		set{_importeRetencion = value;}
	}

    public decimal RetencionesAnterioresRealizadas
    {
        get { return _retencionesAnterioresRealizadas; }
        set { _retencionesAnterioresRealizadas = value; }
    }

    public string Concepto
    {
        get { return _concepto == null ? string.Empty : _concepto; }
        set { _concepto = value; }
    }

        public decimal ImporteTotal { get; set; }
        #endregion
    }
}
