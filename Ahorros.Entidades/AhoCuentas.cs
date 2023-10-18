using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Afiliados.Entidades;
using Generales.Entidades;
using Seguridad.Entidades;
using System.Xml;

namespace Ahorros.Entidades
{
  [Serializable]
	public partial class AhoCuentas : Objeto
	{

	#region "Private Members"

	int _idCuenta;
    TGEFiliales _filial;
	AfiAfiliados _afiliado;
	int _numeroCuenta;
	AhoCuentasTipos _cuentaTipo;
	TGEMonedas _moneda;
	string _denominacion;
	decimal _saldoActual;
    bool _saldoActualModificado;
    decimal _saldoActualOriginal;
	DateTime _fechaAlta;
    DateTime _fechaDesde;
    DateTime _fechaHasta;
	UsuariosAlta _usuarioAlta;
	DateTime? _fechaCierre;
	List<AhoCuentasMovimientos> _cuentasMovimientos;
    List<TGEArchivos> _archivos;
    List<TGEComentarios> _comentarios;
    List<AhoCotitulares> _cotitulares;
        List<TGECampos> _campos;
        XmlDocumentSerializationWrapper _loteCamposValores;

        #endregion

        #region "Constructors"
        public AhoCuentas()
	{
	}
	#endregion
		
	#region "Public Properties"

    [PrimaryKey()]
	public int IdCuenta
	{
		get{return _idCuenta ;}
		set{_idCuenta = value;}
	}

    public TGEFiliales Filial
    {
        get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
        set { _filial = value; }
    }

    public DateTime FechaAlta
    {
        get { return _fechaAlta; }
        set { _fechaAlta = value; }
    }

    public DateTime FechaDesde
    {
        get { return _fechaDesde; }
        set { _fechaDesde = value; }
    }

    public DateTime FechaHasta
    {
        get { return _fechaHasta; }
        set { _fechaHasta = value; }
    }

    public UsuariosAlta UsuarioAlta
    {
        get { return _usuarioAlta == null ? (_usuarioAlta = new UsuariosAlta()) : _usuarioAlta; }
        set { _usuarioAlta = value; }
    }

	public AfiAfiliados Afiliado
	{
		get{return _afiliado==null?(_afiliado=new AfiAfiliados()):_afiliado;}
		set{_afiliado = value;}
	}

	public int NumeroCuenta
	{
		get{return _numeroCuenta;}
		set{_numeroCuenta = value;}
	}

	public AhoCuentasTipos CuentaTipo
	{
		get{return _cuentaTipo==null? (_cuentaTipo=new AhoCuentasTipos()):_cuentaTipo;}
		set{_cuentaTipo = value;}
	}

	public TGEMonedas Moneda
	{
		get{return _moneda==null?(_moneda=new TGEMonedas()):_moneda;}
		set{_moneda = value;}
	}

	public string Denominacion
	{
		get{return _denominacion == null ? string.Empty : _denominacion ;}
		set{_denominacion = value;}
	}

	public decimal SaldoActual
	{
		get{return _saldoActual;}
		set{
            if (!this._saldoActualModificado)
            {
                _saldoActual = value;
                _saldoActualOriginal = value;
                _saldoActualModificado = true;
            }
            else
                _saldoActual = value;
        }
	}

    public decimal SaldoActualOriginal
    {
        get { return _saldoActualOriginal; }
        set { }
    }

	public DateTime? FechaCierre
	{
		get{return _fechaCierre;}
		set{_fechaCierre = value;}
	}

	public List<AhoCuentasMovimientos> CuentasMovimientos
	{
		get{return _cuentasMovimientos==null ? (_cuentasMovimientos = new List<AhoCuentasMovimientos>()) : _cuentasMovimientos;}
		set{_cuentasMovimientos = value;}
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

    public List<AhoCotitulares> Cotitulares
    {
        get { return _cotitulares == null ? (_cotitulares = new List<AhoCotitulares>()) : _cotitulares; }
        set { _cotitulares = value; }
    }

    public string CuentaDatos
    {
        get { return string.Concat(this.NumeroCuenta, " - ",  this.Denominacion, " - ", this.CuentaTipo.CuentaTipo, " ", this.Moneda.Moneda, " ", this.SaldoActual.ToString("N2")); }
        set { }
    }

        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }
        }

        public XmlDocument LoteCamposValores
        {
            get { return _loteCamposValores; }// == null ? (_loteCamposValores = new XmlDocumentSerializationWrapper()) : _loteCamposValores; }
            set { _loteCamposValores = value; }
        }

        #endregion
    }
}
