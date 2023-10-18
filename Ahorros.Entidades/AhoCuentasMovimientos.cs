
using System;
using System.Collections.Generic;
using System.Xml;
using Afiliados.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Seguridad.Entidades;
namespace Ahorros.Entidades
{
  [Serializable]
	public partial class AhoCuentasMovimientos : Objeto
	{
	#region "Private Members"
	int _idCuentaMovimiento;
    UsuariosAlta _usuarioAlta;
    UsuariosConfirmacion _usuarioConfirmacion;
    TGEFiliales _filial;
	AhoCuentas _cuenta;
    DateTime _fechaAlta;
	DateTime _fechaMovimiento;
	TGETiposOperaciones _tipoOperacion;
	string _concepto;
	decimal _importe;
    bool _importeModificado;
    decimal _importeOriginal;
	decimal _saldoActual;
    decimal _importeComisionRechazo;
	TGETiposValores _tipoValor;
	int _numeroValor;
	DateTime _fechaValor;
	int _idBanco;
    DateTime _fechaConfirmacion;
    List<TGEArchivos> _archivos;
    List<TGEComentarios> _comentarios;
    int _idRefTipoOperacion;
    List<TGECampos> _campos;
        List<AfiAfiliados> _cotitulares;
        XmlDocumentSerializationWrapper _loteCuentasMovimientosCotitulares;
        //int? _idAfiliadoCotitular;
        //string _afiliadoCotitular;
        #endregion

        #region "Constructors"
        public AhoCuentasMovimientos()
	{
	}
	#endregion
		
	#region "Public Properties"

    [PrimaryKey()]
	public int IdCuentaMovimiento
	{
		get{return _idCuentaMovimiento ;}
		set{_idCuentaMovimiento = value;}
	}

    public int IdFilial
    {
        get { return Filial.IdFilial; }
        set { }
    }

    public UsuariosAlta UsuarioAlta
    {
        get { return _usuarioAlta == null ? (_usuarioAlta = new UsuariosAlta()) : _usuarioAlta; }
        set { _usuarioAlta = value; }
    }

    public UsuariosConfirmacion UsuarioConfirmacion
    {
        get { return _usuarioConfirmacion == null ? (_usuarioConfirmacion = new UsuariosConfirmacion()) : _usuarioConfirmacion; }
        set { _usuarioConfirmacion = value; }
    }

    public decimal SaldoActual
    {
        get { return _saldoActual; }
        set { _saldoActual = value; }
    }

	public AhoCuentas Cuenta
	{
		get{return _cuenta==null? (_cuenta=new AhoCuentas()):_cuenta;}
		set{_cuenta = value;}
	}

	public DateTime FechaMovimiento
	{
		get{return _fechaMovimiento;}
		set{_fechaMovimiento = value;}
	}

    public DateTime FechaAlta
    {
        get { return _fechaAlta; }
        set { _fechaAlta = value; }
    }

    public TGETiposOperaciones TipoOperacion
	{
        get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
		set{_tipoOperacion = value;}
	}

	public string Concepto
	{
		get{return _concepto == null ? string.Empty : _concepto ;}
		set{_concepto = value;}
	}

    //public decimal Importe
    //{
    //    get{return _importe;}
    //    set{_importe = value;}
    //}

	public decimal Importe
	{
		get{return _importe;}
        set
        {
            if (!this._importeModificado)
            {
                _importe = value;
                _importeOriginal = value;
                _importeModificado = true;
            }
            else
                _importe = value;
        }
    }

    public decimal ImporteOriginal
    {
        get { return _importeOriginal; }
        set { }
    }

    public decimal ImporteComisionRechazo
    {
        get { return _importeComisionRechazo; }
        set { _importeComisionRechazo = value; }
	}

      /// <summary>
      /// Para mostrar el Importe con formato negativo en grillas
      /// </summary>
    public decimal ImporteMostrar
    {
        get { return this.TipoOperacion.TipoMovimiento.IdTipoMovimiento == (int)EnumTGETiposMovimientos.Credito ? this.Importe : this.Importe * -1; }
        set { }
    }


    public TGEFiliales Filial
    {
        get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
        set { _filial = value; }
    }

	public TGETiposValores TipoValor
	{
		get{return _tipoValor==null?(_tipoValor=new TGETiposValores()):_tipoValor;}
		set{_tipoValor = value;}
	}

	public int NumeroValor
	{
		get{return _numeroValor;}
		set{_numeroValor = value;}
	}

	public DateTime FechaValor
	{
		get{return _fechaValor;}
		set{_fechaValor = value;}
	}

	public int IdBanco
	{
		get{return _idBanco;}
		set{_idBanco = value;}
	}

    public string ConceptoGrilla
    {
        get {return TipoOperacion.TipoOperacion + " " + TipoValor.TipoValor + " " + NumeroValor.ToString() + " " + FechaValor.ToString() ; }
        set { }
    }

    public DateTime FechaConfirmacion
    {
        get { return _fechaConfirmacion; }
        set { _fechaConfirmacion = value; }
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

    public int IdRefTipoOperacion
    {
        get { return _idRefTipoOperacion; }
        set { _idRefTipoOperacion = value; }
    }

        public XmlDocument LoteCuentasMovimientosCotitulares
        {
            get { return _loteCuentasMovimientosCotitulares; }
            set { _loteCuentasMovimientosCotitulares = value; }
        }

        public int? IdConceptoContable { get; set; }

    public List<TGECampos> Campos
    {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }
    }

        public List<AfiAfiliados> Cotitulares
        {
            get { return _cotitulares == null ? (_cotitulares = new List<AfiAfiliados>()) : _cotitulares; }
            set { _cotitulares = value; }
        }

        [Auditoria()]
        public decimal MonedaCotizacion { get; set; }
        #endregion
    }
}
