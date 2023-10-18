
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Seguridad.Entidades;
using Generales.Entidades;
using System.Xml;

namespace Ahorros.Entidades
{
  [Serializable]
	public partial class AhoPlazosFijos : Objeto
	{
	#region "Private Members"
	int _idPlazoFijo;
    int _idPlazoFijoAnterior;
	int _idAfiliado;
    int _idFilial;
    TGETiposOperaciones _tipoOperacion;
	AhoCuentas _cuenta;
	decimal _importeCapital;
	decimal _tasaInteres;
    decimal _importeInteres;
	int _plazoDias;
	DateTime _fechaAlta;
	UsuariosAlta _usuarioAlta;
    UsuariosConfirmacion _usuarioConfirmacion;
	bool _tasaEspecial;
        bool _RegistrarCajaAhorros;
        DateTime _fechaConfirmacion;
	AhoTiposRenovaciones _tipoRenovacion;
	DateTime _fechaPago;
	DateTime _fechaCancelacion;
    DateTime _fechaVencimiento;
    DateTime _fechaInicioVigencia;
	UsuariosCancelacion _usuarioCancelacion;
    List<TGEArchivos> _archivos;
    List<TGEComentarios> _comentarios;
    TGETiposValores _tipoValor;
    TGEMonedas _moneda;
    AhoPlazos _plazo;
    List<AhoCotitulares> _cotitulares;
    bool _confirmarRenovar;
    TGETiposValoresPagos _tipoValorPago;
      TGEFilialesPagos _filialPago;
        List<TGECampos> _campos;
        XmlDocumentSerializationWrapper _loteCamposValores;
        #endregion

        #region "Constructors"
        public AhoPlazosFijos()
	{
	}
	#endregion
		
	#region "Public Properties"

      [PrimaryKey()]
	public int IdPlazoFijo
	{
		get{return _idPlazoFijo ;}
		set{_idPlazoFijo = value;}
	}

      [Auditoria()]
      public int IdPlazoFijoAnterior
      {
          get { return _idPlazoFijoAnterior; }
          set { _idPlazoFijoAnterior = value; }
      }

	public int IdAfiliado
	{
		get{return _idAfiliado;}
		set{_idAfiliado = value;}
	}

    public int IdFilial
    {
        get { return _idFilial; }
        set { _idFilial = value; }
    }

	public decimal ImporteCapital
	{
		get{return _importeCapital;}
		set{_importeCapital = value;}
	}
        [Auditoria()]
        public decimal TasaInteres
	{
		get{return _tasaInteres;}
		set{_tasaInteres = value;}
	}
        [Auditoria()]
        public int PlazoDias
	{
		get{return _plazoDias;}
		set{_plazoDias = value;}
	}

	public DateTime FechaAlta
	{
		get{return _fechaAlta;}
		set{_fechaAlta = value;}
	}

	public UsuariosAlta UsuarioAlta
	{
        get { return _usuarioAlta == null ? (_usuarioAlta = new UsuariosAlta()) : _usuarioAlta; }
		set{_usuarioAlta = value;}
	}

    public UsuariosConfirmacion UsuarioConfirmacion
    {
        get { return _usuarioConfirmacion == null ? (_usuarioConfirmacion = new UsuariosConfirmacion()) : _usuarioConfirmacion; }
        set { _usuarioConfirmacion = value; }
    }

	public bool TasaEspecial
	{
		get{return _tasaEspecial;}
		set{_tasaEspecial = value;}
	}

    public bool RegistrarCajaAhorros
    {
        get { return _RegistrarCajaAhorros; }
        set { _RegistrarCajaAhorros = value; }
    }

    public TGETiposOperaciones TipoOperacion
    {
        get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
        set { _tipoOperacion = value; }
    }
    [Auditoria()]
	public AhoTiposRenovaciones TipoRenovacion
	{
        get { return _tipoRenovacion == null ? (_tipoRenovacion = new AhoTiposRenovaciones()) : _tipoRenovacion; }
		set{_tipoRenovacion = value;}
	}

	public DateTime FechaPago
	{
		get{return _fechaPago;}
		set{_fechaPago = value;}
	}

	public DateTime FechaCancelacion
	{
		get{return _fechaCancelacion;}
		set{_fechaCancelacion = value;}
	}

	public UsuariosCancelacion UsuarioCancelacion
	{
        get { return _usuarioCancelacion == null ? (_usuarioCancelacion = new UsuariosCancelacion()) : _usuarioCancelacion; }
		set{_usuarioCancelacion = value;}
	}

    public decimal ImporteInteres
    {
        get { return _importeInteres; }
        set { _importeInteres = value; }
    }

    public decimal ImporteTotal
    {
        get { return this.ImporteCapital + this.ImporteInteres; }
        set { }
    }

    public TGETiposValores TipoValor
    {
        get { return _tipoValor == null ? (_tipoValor = new TGETiposValores()) : _tipoValor; }
        set { _tipoValor = value; }
    }

    public TGEMonedas Moneda
    {
        get { return _moneda == null ? (_moneda = new TGEMonedas()) : _moneda; }
        set { _moneda = value; }
    }

    public AhoCuentas Cuenta
    {
        get { return _cuenta == null ? (_cuenta = new AhoCuentas()) : _cuenta; }
        set { _cuenta = value; }
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

    public DateTime FechaConfirmacion
    {
        get { return _fechaConfirmacion; }
        set { _fechaConfirmacion = value; }
    }

    public DateTime FechaVencimiento
    {
        get { return _fechaVencimiento; }
        set { _fechaVencimiento = value; }
    }

    public DateTime FechaInicioVigencia
    {
        get { return _fechaInicioVigencia; }
        set { _fechaInicioVigencia = value; }
    }

    public AhoPlazos Plazo
    {
        get { return _plazo == null ? (_plazo = new AhoPlazos()) : _plazo; }
        set { _plazo = value; }
    }

    public List<AhoCotitulares> Cotitulares
    {
        get { return _cotitulares == null ? (_cotitulares = new List<AhoCotitulares>()) : _cotitulares; }
        set { _cotitulares = value; }
    }

    public bool ConfirmarRenovar
    {
        get { return _confirmarRenovar; }
        set { _confirmarRenovar = value; }
    }

    public TGETiposValoresPagos TipoValorPago
    {
        get { return _tipoValorPago == null ? (_tipoValorPago = new TGETiposValoresPagos()) : _tipoValorPago; }
        set { _tipoValorPago = value; }
    }

    public TGEFilialesPagos FilialPago
    {
        get { return _filialPago == null ? (_filialPago = new TGEFilialesPagos()) : _filialPago; }
        set { _filialPago = value; }
    }

        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }

        }

        public XmlDocument LoteCamposValores
        {
            get { return _loteCamposValores; }//  ? (_loteCamposValores = new XmlDocumentSerializationWrapper()) : _loteCamposValores; }
            set { _loteCamposValores = value; }
        }
        [Auditoria()]
        public decimal MonedaCotizacion { get; set; }

        public bool CancelaCajaAhorros { get; set; }
        #endregion
    }
}
