
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Seguridad.Entidades;
using Generales.Entidades;
using System.Xml;

namespace Cargos.Entidades
{
  [Serializable]
	public partial class CarTiposCargosAfiliadosFormasCobros : Objeto
	{
	#region "Private Members"
	int _idTipoCargoAfiliadoFormaCobro;
    DateTime _fechaAlta;
    DateTime _fechaAltaEvento;
    int _cantidadCuotas;
    int _ultimaCuotaPaga;
    decimal? _importeCargo;
    decimal? _tasaInteres;
    decimal? _importeInteres;
    decimal _importeCuota;
    decimal _importeTotal;
    DateTime _fechaFinVigencia;
    int _idAfiliado;
    int _periodo;
    UsuariosAlta _usuarioAlta;
        TGEMonedas _moneda;
        CarTiposCargos _tipoCargo;
    TGEListasValoresDetalles _listaValorDetalle;
	TGEFormasCobrosAfiliados _formaCobroAfiliado;
    List<TGEArchivos> _archivos;
    List<TGEComentarios> _comentarios;
    List<TGECampos> _campos;
    int _idReferenciaRegistro;
    string _tablaReferenciaRegistro;
    string _detalle;
    bool _cargoFacturado;
    bool _noModificarFechaAlta;
    XmlDocumentSerializationWrapper _loteCamposValores;
        #endregion

        #region "Constructors"
        public CarTiposCargosAfiliadosFormasCobros()
	{
            MonedaCotizacion = 1;
        }
	#endregion
		
	#region "Public Properties"

    [PrimaryKey()]
	public int IdTipoCargoAfiliadoFormaCobro
	{
		get{return _idTipoCargoAfiliadoFormaCobro ;}
		set{_idTipoCargoAfiliadoFormaCobro = value;}
	}
      [Auditoria()]
    public DateTime FechaAlta
    {
        get { return _fechaAlta; }
        set { _fechaAlta = value; }
    }

    public DateTime FechaAltaEvento
    {
        get { return _fechaAltaEvento; }
        set { _fechaAltaEvento = value; }
    }

    [Auditoria()]
    public int CantidadCuotas
    {
        get { return _cantidadCuotas; }
        set { _cantidadCuotas = value; }
    }

    public int UltimaCuotaPaga
    {
        get { return _ultimaCuotaPaga; }
        set { _ultimaCuotaPaga = value; }
    }
      [Auditoria()]
    public decimal? ImporteCargo
    {
        get { return _importeCargo; }
        set { _importeCargo = value; }
    }
      [Auditoria()]
    public decimal? TasaInteres
    {
        get { return _tasaInteres; }
        set { _tasaInteres = value; }
    }
      [Auditoria()]
    public decimal? ImporteInteres
    {
        get { return _importeInteres; }
        set { _importeInteres = value; }
    }

    [Auditoria()]
    public decimal ImporteCuota
    {
        get { return _importeCuota; }
        set { _importeCuota = value; }
    }

    public decimal ImporteTotal
    {
        get { return _importeTotal; }
        set { _importeTotal = value; }
    }

    public int IdAfiliado
    {
        get { return _idAfiliado; }
        set { _idAfiliado = value; }
    }

    public DateTime FechaFinVigencia
    {
        get { return _fechaFinVigencia; }
        set { _fechaFinVigencia = value; }
    }

    public int Periodo
    {
        get { return _periodo; }
        set { _periodo = value; }
    }

        public TGEMonedas Moneda
        {
            get { return _moneda == null ? (_moneda = new TGEMonedas()) : _moneda; }
            set { _moneda = value; }
        }

        public UsuariosAlta UsuarioAlta
    {
        get { return _usuarioAlta == null ? (_usuarioAlta = new UsuariosAlta()) : _usuarioAlta; }
        set { _usuarioAlta = value; }
    }

    public CarTiposCargos TipoCargo
	{
        get { return _tipoCargo == null ? (_tipoCargo = new CarTiposCargos()) : _tipoCargo; }
		set{_tipoCargo = value;}
	}
      [Auditoria()]
    public TGEListasValoresDetalles ListaValorDetalle
    {
        get { return _listaValorDetalle == null ? (_listaValorDetalle = new TGEListasValoresDetalles()) : _listaValorDetalle; }
        set { _listaValorDetalle = value; }
    }

      [Auditoria()]
    public TGEFormasCobrosAfiliados FormaCobroAfiliado
	{
        get { return _formaCobroAfiliado == null ? (_formaCobroAfiliado = new TGEFormasCobrosAfiliados()) : _formaCobroAfiliado; }
		set{_formaCobroAfiliado = value;}
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

    public int IdReferenciaRegistro
    {
        get { return _idReferenciaRegistro; }
        set { _idReferenciaRegistro = value; }
    }

    public string TablaReferenciaRegistro
    {
        get { return _tablaReferenciaRegistro == null ? string.Empty : _tablaReferenciaRegistro; }
        set { _tablaReferenciaRegistro = value; }
    }

    public string Detalle
    {
        get { return _detalle == null ? string.Empty : _detalle; }
        set { _detalle = value; }
    }

    public bool CargoFacturado
    {
        get { return _cargoFacturado; }
        set { _cargoFacturado = value; }
    }

    public bool NoModificarFechaAlta
    {
        get { return _noModificarFechaAlta; }
        set { _noModificarFechaAlta = value; }
    }

        [Auditoria()]
        public decimal? Porcentaje { get; set; }

        public Int64? IdAfiliadoRef { get; set; }

        [Auditoria()]
        public decimal MonedaCotizacion { get; set; }
        public XmlDocument LoteCamposValores
        {
            get { return _loteCamposValores; }// == null ? (_loteCamposValores = new XmlDocumentSerializationWrapper()) : _loteCamposValores; }
            set { _loteCamposValores = value; }
        }

        #endregion
    }
}
