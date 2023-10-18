
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
using Bancos.Entidades;
namespace CuentasPagar.Entidades
{
  [Serializable]
	public partial class CapOrdenesPagos : Objeto
	{
		// Class CapOrdenesPagos
	#region "Private Members"
	int _idOrdenPago;
	string _observacion;
    TGEEntidades _entidad;
	int _idRefTipoOperacion;
	TGETiposValores _tipoValor;
	TGEFiliales _filial;
	DateTime _fechaAlta;
	int _idUsuarioAlta;
	DateTime? _fechaAutorizacion;
	int? _idUsuarioAutorizacion;
	TGEFilialesPagos _filialPago;
	DateTime? _fechaPago;
	int? _idUsuarioPago;
    decimal _importeSubTotal;
    decimal _importeRetenciones;
      decimal _importeTotal;
      string _beneficiario;
      DateTime? _fechaDesde;
      DateTime? _fechaHasta;
      TGETiposOperaciones _tipoOperacion;
      DateTime? _fechaConfirmacion;
      int _idUsuarioConfirmacion;
    List<CapSolicitudPago> _solicitudesPagos;
    List<TGEArchivos> _archivos;
    List<TGEComentarios> _comentarios;
    TESBancosCuentas _bancoCuenta;
    string _numeroCheque;
    //List<CapOrdenesPagosSolicitudesPagos> _OrdenesPagosSolicitudesPagos;
    List<CapOrdenesPagosValores> _ordenesPagosValores;
	#endregion
		
	#region "Constructors"
	public CapOrdenesPagos()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdOrdenPago
	{
		get{return _idOrdenPago ;}
		set{_idOrdenPago = value;}
	}
      [Auditoria()]
	public string Observacion
	{
		get{return _observacion == null ? string.Empty : _observacion ;}
		set{_observacion = value;}
	}

    public TGEEntidades Entidad
	{
        get { return _entidad == null ? (_entidad = new TGEEntidades()) : _entidad; }
		set{_entidad = value;}
	}

	public int IdRefTipoOperacion
	{
		get{return _idRefTipoOperacion;}
		set{_idRefTipoOperacion = value;}
	}
      [Auditoria()]
	public TGETiposValores TipoValor
	{
        get { return _tipoValor == null ? (_tipoValor = new TGETiposValores()) : _tipoValor; }
		set{_tipoValor = value;}
	}

	public TGEFiliales Filial
	{
        get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
		set{_filial = value;}
	}

	public DateTime FechaAlta
	{
		get{return _fechaAlta;}
		set{_fechaAlta = value;}
	}

	public int IdUsuarioAlta
	{
		get{return _idUsuarioAlta;}
		set{_idUsuarioAlta = value;}
	}
      [Auditoria()]
	public DateTime? FechaAutorizacion
	{
		get{return _fechaAutorizacion;}
		set{_fechaAutorizacion = value;}
	}
      [Auditoria()]
	public int? IdUsuarioAutorizacion
	{
		get{return _idUsuarioAutorizacion;}
		set{_idUsuarioAutorizacion = value;}
	}

      [Auditoria()]
	public TGEFilialesPagos FilialPago
	{
        get { return _filialPago == null ? (_filialPago = new TGEFilialesPagos()) : _filialPago; }
		set{_filialPago = value;}
	}

	public DateTime? FechaPago
	{
		get{return _fechaPago;}
		set{_fechaPago = value;}
	}

	public int? IdUsuarioPago
	{
		get{return _idUsuarioPago;}
		set{_idUsuarioPago = value;}
	}

    public decimal ImporteSubTotal
    {
        get { return _importeSubTotal; }
        set { _importeSubTotal = value; }
    }

    public decimal ImporteRetenciones
    {
        get { return _importeRetenciones; }
        set { _importeRetenciones = value; }
    }

	public decimal ImporteTotal
	{
		get{return _importeTotal;}
		set{_importeTotal = value;}
	}

    public string Beneficiario
    {
        get { return _beneficiario == null ? string.Empty : _beneficiario; }
        set { _beneficiario = value; }
    }

    public int IdUsuarioConfirmacion
    {
        get { return _idUsuarioConfirmacion; }
        set { _idUsuarioConfirmacion = value; }
    }

    public DateTime? FechaConfirmacion
    {
        get { return _fechaConfirmacion; }
        set { _fechaConfirmacion = value; }
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

    public TGETiposOperaciones TipoOperacion
    {
        get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
        set { _tipoOperacion = value; }
    }

    public List<CapSolicitudPago> SolicitudesPagos
    {
        get { return _solicitudesPagos == null ? (_solicitudesPagos = new List<CapSolicitudPago>()) : _solicitudesPagos; }
        set { _solicitudesPagos = value; }
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

    //public List<CapOrdenesPagosSolicitudesPagos> capOrdenesPagosSolicitudesPagos
    //{
    //    get{return _OrdenesPagosSolicitudesPagos==null ? (_OrdenesPagosSolicitudesPagos = new List<CapOrdenesPagosSolicitudesPagos>()) : _OrdenesPagosSolicitudesPagos;}
    //    set{_OrdenesPagosSolicitudesPagos = value;}
    //}				

      [Auditoria()]
    public TESBancosCuentas BancoCuenta
    {
        get { return _bancoCuenta == null ? (_bancoCuenta = new TESBancosCuentas()) : _bancoCuenta; }
        set { _bancoCuenta = value; }
    }

      [Auditoria()]
    public string NumeroCheque
    {
        get { return _numeroCheque == null ? string.Empty : _numeroCheque; }
        set { _numeroCheque = value; }
    }

      public List<CapOrdenesPagosValores> OrdenesPagosValore
      {
          get { return _ordenesPagosValores == null ? (_ordenesPagosValores = new List<CapOrdenesPagosValores>()) : _ordenesPagosValores; }
          set { _ordenesPagosValores = value; }
      }		
	#endregion
	}
}
