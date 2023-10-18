using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Generales.Entidades;
using Afiliados.Entidades;

namespace Facturas.Entidades
{
    public class VTAFacturas : Objeto
    {
    #region "Private Members"
    
        int _idFactura;
        AfiAfiliados _afiliado;   
        TGETiposFacturas _tipoFactura;
        VTAConceptosComprobantes _conceptoComprobante;
        TGEMonedas _moneda;
        string _prefijoNumeroFactura;
        string _numeroFactura;
        DateTime _fechaFactura;
        DateTime _fechaContable;
        string _observacion;
        decimal _importeSinIVA;
        decimal _ivaTotal;
        //decimal _costoFlete;
        decimal _descuentoPorcentual;
        decimal _descuentoImporte;
        decimal _importeTotal;
        decimal _monedaCotizacion;
        string _CAE;
        string _CAEFechaVencimiento;
        DateTime _fechaVencimiento;
        TGEFiliales _agenciaAlta;
        DateTime _fechaAlta;
        UsuariosAlta _usuarioAlta;
        DateTime _fechaAnulacion;
        TGECondicionesFiscales _condicionFiscal;

        List<VTAFacturasDetalles> _facturasDetalles;
        string _appPath;
    #endregion

    #region "Constructors"
	public VTAFacturas()
	{
        this._monedaCotizacion = 1;
	}
	#endregion

    #region "Public Properties"

    [PrimaryKey()]
    public int IdFactura
    {
        get { return _idFactura; }
        set { _idFactura = value; }
    }

    public AfiAfiliados Afiliado
    {
        get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; }
        set { _afiliado = value; }
    }

    public TGETiposFacturas TipoFactura
    {
        get { return _tipoFactura == null ? (_tipoFactura = new TGETiposFacturas()) : _tipoFactura; }
        set { _tipoFactura = value; }
    }

    public VTAConceptosComprobantes ConceptoComprobante
    {
        get { return _conceptoComprobante == null ? (_conceptoComprobante = new VTAConceptosComprobantes()) : _conceptoComprobante; }
        set { _conceptoComprobante = value; }
    }

    public TGEMonedas Moneda
    {
        get { return _moneda == null ? (_moneda = new TGEMonedas()) : _moneda; }
        set { _moneda = value; }
    }

    public string PrefijoNumeroFactura
    {
        get { return _prefijoNumeroFactura==null ? string.Empty : _prefijoNumeroFactura; }
        set { _prefijoNumeroFactura = value; }
    }

    public string NumeroFactura 
    {
        get {return _numeroFactura==null? string.Empty : _numeroFactura ;}
        set {_numeroFactura = value;}
    }

    public DateTime FechaFactura
    {
        get {return _fechaAlta;}
        set {_fechaFactura = value;}
    }

    public DateTime FechaContable
    {
        get {return _fechaContable;}
        set {_fechaContable = value;}
    }

    public string Observacion
    {
        get {return _observacion;}
        set {_observacion = value;}
    }

    public decimal ImporteSinIVA
    {
        get {return _importeSinIVA;}
        set {_importeSinIVA = value;}
    }

    public decimal IvaTotal
    {
        get {return _ivaTotal;}
        set {_ivaTotal = value;}
    }

    //public decimal CostoFlete
    //{
    //    get {return _costoFlete;}
    //    set {_costoFlete = value;}
    //}

    public decimal MonedaCotizacion
    {
        get { return _monedaCotizacion; }
        set { _monedaCotizacion = value; }
    }

    public decimal DescuentoPorcentual
    {
        get {return _descuentoPorcentual;}
        set {_descuentoPorcentual = value;}
    }

    public decimal DescuentoImporte
    {
        get { return _descuentoImporte; }
        set { _descuentoImporte = value; }
    }

    public decimal ImporteTotal
    {
        get {return _importeTotal;}
        set {_importeTotal = value;}
    }

    public string CAE
    {
        get { return _CAE == null ? string.Empty : _CAE; }
        set { _CAE = value; }
    }

    public string CAEFechaVencimiento
    {
        get { return _CAEFechaVencimiento == null ? string.Empty : _CAEFechaVencimiento; }
        set { _CAEFechaVencimiento = value; }
    }

    public DateTime FechaVencimiento
    {
        get {return _fechaVencimiento;}
        set {_fechaVencimiento = value;}
    }

    public TGEFiliales AgenciaAlta
    {
        get {return _agenciaAlta == null? (_agenciaAlta = new TGEFiliales()) : _agenciaAlta;}
        set {_agenciaAlta = value;}
    }

    public DateTime FechaEmision
    {
        get {return _fechaAlta;}
        set {_fechaAlta = value;}
    }

    public UsuariosAlta UsuarioAlta
    {
        get { return _usuarioAlta == null ? (_usuarioAlta = new UsuariosAlta()) : _usuarioAlta; }
        set { _usuarioAlta = value; }
    }

    public DateTime FechaAnulacion
    {
        get {return _fechaAnulacion;}
        set {_fechaAnulacion = value;}
    }

    public List<VTAFacturasDetalles> FacturasDetalles
    {
        get { return _facturasDetalles == null ? (_facturasDetalles= new List<VTAFacturasDetalles>()) : _facturasDetalles; }
        set { _facturasDetalles = value; }
    }

    public TGECondicionesFiscales CondicionFiscal
    {
        get { return _condicionFiscal == null ? (_condicionFiscal = new TGECondicionesFiscales()) : _condicionFiscal; }
        set { _condicionFiscal = value; }
    }

    public string NumeroFacturaCompleto
    {
        get { return this.PrefijoNumeroFactura.Length>0 && this.NumeroFactura.Length > 0? string.Concat(this.PrefijoNumeroFactura, "-", this.NumeroFactura) : string.Empty; }
        set { }
    }

    public string AppPath
    {
        get { return _appPath == null ? string.Empty : _appPath; }
        set { _appPath = value; }
    }

    #endregion

    }
}
