
using System;
using System.Collections.Generic;
using Afiliados.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
namespace Facturas.Entidades
{
  [Serializable]
	public partial class VTAPresupuestos : Objeto
	{
		// Class VTAPresupuestos
	#region "Private Members"
	int _idPresupuesto;
	int? _idAfiliado;
    TGECondicionesFiscales _condicionFiscal;
    string _razonSocial;
    string _contacto;
    string _telefono;
    string _correoElectronico;
	string _descripcion;
	DateTime _fechaAlta;
    DateTime? _fechaEntrega;
    string _garantia;
    string _formaPago;
    string _plazoEntrega;
	int? _idFilial;
        TGEMonedas _moneda;
        decimal _monedaCotizacion;
        decimal _importeSinIVA;
    decimal _ivaTotal;
    decimal _importeTotal;
	byte[] _presupuestoPDF;
	bool? _firmaDigital;
	DateTime _fechaFinVigencia;
    List<VTAPresupuestosDetalles> _presupuestosDetalles;
    string _appPath;
    string _dirPath;
    DateTime? _fechaDesde;
    DateTime? _fechaHasta;
   AfiAfiliados _afiliado;
        #endregion

        #region "Constructors"
        public VTAPresupuestos()
	{
	}
        #endregion

        #region "Public Properties"
        [PrimaryKey()]
        public int IdPresupuesto
	{
		get{return _idPresupuesto ;}
		set{_idPresupuesto = value;}
	}
        //public int? IdAfiliado
        //{
        //    get { return _idAfiliado; }
        //    set { _idAfiliado = value; }
        //}
 public TGECondicionesFiscales CondicionFiscal
    {
        get { return _condicionFiscal == null ? (_condicionFiscal = new TGECondicionesFiscales()) : _condicionFiscal; }
        set { _condicionFiscal = value; }
    }

        public AfiAfiliados Afiliado
        {
            get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; }
            set { _afiliado = value; }
        }       

    public string RazonSocial
    {
        get { return _razonSocial == null ? string.Empty : _razonSocial; }
        set { _razonSocial = value; }
    }

    public string Contacto
    {
        get { return _contacto == null ? string.Empty : _contacto; }
        set { _contacto = value; }
    }

    public string Telefono
    {
        get { return _telefono == null ? string.Empty : _telefono; }
        set { _telefono = value; }
    }

    public string CorreoElectronico
    {
        get { return _correoElectronico == null ? string.Empty : _correoElectronico; }
        set { _correoElectronico = value; }
    }

	public string Descripcion
	{
		get{return _descripcion == null ? string.Empty : _descripcion ;}
		set{_descripcion = value;}
	}

	public DateTime FechaAlta
	{
		get{return _fechaAlta;}
		set{_fechaAlta = value;}
	}

    public DateTime? FechaEntrega
    {
        get { return _fechaEntrega; }
        set { _fechaEntrega = value; }
    }

    public string PlazoEntrega
    {
        get { return _plazoEntrega == null ? string.Empty : _plazoEntrega; }
        set { _plazoEntrega = value; }
    }
    public string Garantia
    {
        get { return _garantia == null ? string.Empty : _garantia; }
        set { _garantia = value; }
    }
    public string FormaPago
    {
        get { return _formaPago == null ? string.Empty : _formaPago; }
        set { _formaPago = value; }
    }

	public int? IdFilial
	{
		get{return _idFilial;}
		set{_idFilial = value;}
	}

    public decimal ImporteSinIVA
    {
        get { return _importeSinIVA; }
        set { _importeSinIVA = value; }
    }

    public decimal IvaTotal
    {
        get { return _ivaTotal; }
        set { _ivaTotal = value; }
    }

    public decimal ImporteTotal
    {
        get { return _importeTotal; }
        set { _importeTotal = value; }
    }
        public TGEMonedas Moneda
        {
            get { return _moneda == null ? (_moneda = new TGEMonedas()) : _moneda; }
            set { _moneda = value; }
        }

        public decimal MonedaCotizacion
        {
            get { return _monedaCotizacion; }
            set { _monedaCotizacion = value; }
        }
        public byte[] PresupuestoPDF
	{
		get{return _presupuestoPDF;}
		set{_presupuestoPDF = value;}
	}

	public bool? FirmaDigital
	{
		get{return _firmaDigital;}
		set{_firmaDigital = value;}
	}

	public DateTime FechaFinVigencia
	{
		get{return _fechaFinVigencia;}
		set{_fechaFinVigencia = value;}
	}

    public List<VTAPresupuestosDetalles> PresupuestosDetalles
    {
        get { return _presupuestosDetalles == null ? (_presupuestosDetalles = new List<VTAPresupuestosDetalles>()) : _presupuestosDetalles; }
        set { _presupuestosDetalles = value; }
    }

    public string AppPath
    {
        get { return _appPath == null ? string.Empty : _appPath; }
        set { _appPath = value; }
    }

    public string DirPath
    {
        get { return _dirPath == null ? string.Empty : _dirPath; }
        set { _dirPath = value; }
    }

    public DateTime? FechaDesde
    {
        get { return _fechaDesde; }
        set { _fechaDesde = value; }
    }

    public DateTime? FechaHasta
    {
        get { return _fechaHasta; }
        set { _fechaHasta = value; }
    }

	#endregion
	}
}
