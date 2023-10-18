using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
using Contabilidad.Entidades;
using System.Xml;

namespace Proveedores.Entidades
{
  [Serializable]
    public class CapProveedores : Objeto
	{

	#region "Private Members"
	int? _idProveedor;
	string _razonSocial;
	string _cUIT;
    DateTime _cUITVto;
	string _beneficiarioDelCheque;
	string _paginaWeb;
    string _cBU8Digitos;
    string _cBU14Digitos;
    string _detalle;
    TGECondicionesFiscales _condicionFiscal;
    List<CapProveedoresDomicilios> _proveedoresDomicilios;
    List<CapProveedoresTelefonos> _telefonos;
    List<TGEArchivos> _archivos;
    List<TGEComentarios> _comentarios;
    List<TGECampos> _campos;
    DateTime? _fechaDesde;
    DateTime? _fechaHasta;
    decimal _saldoActual;
    bool _tieneSaldo;
    CtbCuentasContables _cuentaContable;
    XmlDocumentSerializationWrapper _loteCamposValores;
    //List<CapProveedoresRetenciones> _proveedoresRetenciones;
    //List<CapTipoDeActividad> _tiposActividades;

	#endregion
		
	#region "Constructors"
	public CapProveedores()
	{
	}
	#endregion
		
	#region "Public Properties"

      [PrimaryKey()]
	public int? IdProveedor
	{
		get{return _idProveedor;}
		set{_idProveedor = value;}
	}

    [Auditoria()]
	public string RazonSocial
	{
		get{return _razonSocial == null ? string.Empty : _razonSocial;}
		set{_razonSocial = value;}
	}

    [Auditoria()]
    public string Detalle
    {
        get { return _detalle == null ? string.Empty : _detalle; }
        set { _detalle = value; }
    }

    [Auditoria()]
	public string CUIT
	{
		get{return _cUIT == null ? string.Empty : _cUIT;}
		set{_cUIT = value;}
	}


    //public string CUITFormato
    //{
    //    get { return _cUIT == null ? string.Empty : (_cUIT = string.Concat(_cUIT.Substring(0,2),"-",_cUIT.Substring(2,8), "-", _cUIT.Substring(10,1)));}
    //}

    public Int64 CUITNumero
    {
        get
        {
            string nroCUIT;
            if (_cUIT == null)
                return 0;
            else
            {
                nroCUIT = _cUIT.Replace("-", "");
                return Convert.ToInt64(nroCUIT);
            }
        }
    }


    public DateTime CUITVto
    {
        get { return _cUITVto; }
        set { _cUITVto = value; }
    }

	public string BeneficiarioDelCheque
	{
		get{return _beneficiarioDelCheque == null ? string.Empty : _beneficiarioDelCheque;}
		set{_beneficiarioDelCheque = value;}
	}

	public string PaginaWeb
	{
		get{return _paginaWeb == null ? string.Empty : _paginaWeb;}
		set{_paginaWeb = value;}
	}

    public string CBU8Digitos
    {
        get { return _cBU8Digitos == null ? string.Empty : _cBU8Digitos; }
        set { _cBU8Digitos = value; }
    }

    public string CBU14Digitos
    {
        get { return _cBU14Digitos == null ? string.Empty : _cBU14Digitos; }
        set { _cBU14Digitos = value; }
    }

    //public int IdEstados
    //{
    //    get { return _idEstados; }
    //    set { _idEstados = value; }
    //}

    public TGECondicionesFiscales CondicionFiscal
    {
        get { return _condicionFiscal == null ? (_condicionFiscal = new TGECondicionesFiscales()) : _condicionFiscal; }
        set { _condicionFiscal = value; }
    }

    public List<CapProveedoresDomicilios> ProveedoresDomicilios
    {
        get { return _proveedoresDomicilios == null ? (_proveedoresDomicilios = new List<CapProveedoresDomicilios>()) : _proveedoresDomicilios; }
        set { _proveedoresDomicilios = value; }
    }
    //public List<CapProveedoresRetenciones> ProveedoresRetenciones
    //{
    //    get { return _proveedoresRetenciones == null ? (_proveedoresRetenciones = new List<CapProveedoresRetenciones>()) : _proveedoresRetenciones; }
    //    set { _proveedoresRetenciones = value; }
    //}

    //public List<CapTipoDeActividad> TiposActividades
    //{
    //    get { return _tiposActividades == null ? (_tiposActividades = new List<CapTipoDeActividad>()) : _tiposActividades; }
    //    set { _tiposActividades = value; }
    //}

    public List<CapProveedoresTelefonos> Telefonos
    {
        get { return _telefonos == null ? (_telefonos = new List<CapProveedoresTelefonos>()) : _telefonos; }
        set { _telefonos = value; }
    }

    public List<TGEArchivos> Archivos
    {
        get { return _archivos == null ? (_archivos = new List<TGEArchivos>()) : _archivos; }
        set { _archivos = value; }
    }

    public List<TGEComentarios> Comentarios
    {
        get { return _comentarios == null ? (_comentarios = new List<TGEComentarios>()) : _comentarios; }
        set { _comentarios = value; }
    }

    public List<TGECampos> Campos
    {
        get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
        set { _campos = value; }
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

    public decimal SaldoActual
    {
        get { return _saldoActual; }
        set { _saldoActual = value; }
    }

    public bool TieneSaldo
    {
        get { return _tieneSaldo; }
        set { _tieneSaldo = value; }
    }

      [Auditoria()]
    public CtbCuentasContables CuentaContable
    {
        get { return _cuentaContable == null ? (_cuentaContable = new CtbCuentasContables()) : _cuentaContable; }
        set { _cuentaContable = value; }
    }

      public XmlDocument LoteCamposValores
      {
          get { return _loteCamposValores;}// == null ? (_loteCamposValores = new XmlDocumentSerializationWrapper()) : _loteCamposValores; }
          set { _loteCamposValores = value; }
      }

    public string CBU { get; set; }
    public string DatosCBU { get; set; }
        #endregion
    }

    [Serializable]
    public partial class CapProveedoresDTO
    {
        [PrimaryKey]
        public int IdProveedor { get; set; }

        public Int64 NumeroDocumento { get; set; }

        public string TipoDocumentoDescripcion { get; set; }

        public int IdCondicionFiscal { get; set; }

        public string CondicionFiscalDescripcion { get; set; }

        public string RazonSocial { get; set; }

        public string EstadoDescripcion { get; set; }

        public string Detalle { get; set; }

        public string CodigoProveedor { get; set; }

        public string CUIT { get; set; }
    }
}
