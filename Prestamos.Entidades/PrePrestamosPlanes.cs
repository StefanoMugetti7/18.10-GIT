
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
using System.Xml;
namespace Prestamos.Entidades
{
  [Serializable]
	public partial class PrePrestamosPlanes : Objeto
	{

	#region "Private Members"
	int _idPrestamoPlan;
	string _descripcion;
	DateTime _fechaAlta;
    PrePrestamosPlanesTasas _prestamoPlanTasa;
    TGETiposOperaciones _tipoOperacion;
    List<TGETiposOperaciones> _tiposOperaciones;
    List<PrePrestamosPlanesTasas> _prestamosPlanesTasas;
    List<PrePrestamosIpsPlanes> _prestamosIpsPlanes;
    List<TGEArchivos> _archivos;
    List<TGEComentarios> _comentarios;
    TGEFormasCobros _formaCobro;
    List<TGEFormasCobros> _formasCobros;
    DateTime _fechaInicioVigencia;
    DateTime _fechaFinVigencia;
    decimal _porcentajeGasto;
    decimal _importeGasto;
    List<TGECampos> _campos;
    List<PrePrestamosBancoSolParametros> _prestamosBancoSolParametros;
    XmlDocumentSerializationWrapper _loteXML;
    decimal? _porcentajeCapitalSocial;
    TGEMonedas _moneda;
    PreTiposUnidades _tipoUnidad;
    decimal? _porcentajeSeguroCuota;
	#endregion
		
	#region "Constructors"
	public PrePrestamosPlanes()
	{
	}
	#endregion
		
	#region "Public Properties"

      [PrimaryKey()]
	public int IdPrestamoPlan
	{
		get{return _idPrestamoPlan ;}
		set{_idPrestamoPlan = value;}
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

    public PrePrestamosPlanesTasas PrestamoPlanTasa
    {
        get { return _prestamoPlanTasa == null ? (_prestamoPlanTasa = new PrePrestamosPlanesTasas()) : _prestamoPlanTasa; }
        set { _prestamoPlanTasa = value; }
    }

        public PreTiposUnidades TipoUnidad
        {
            get { return _tipoUnidad == null ? (_tipoUnidad = new PreTiposUnidades()) : _tipoUnidad; }
            set { _tipoUnidad = value; }
        }

        public TGETiposOperaciones TipoOperacion
    {
        get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
        set { _tipoOperacion = value; }
    }

    public List<TGETiposOperaciones> TiposOperaciones
    {
        get { return _tiposOperaciones == null ? (_tiposOperaciones = new List<TGETiposOperaciones>()) : _tiposOperaciones; }
        set { _tiposOperaciones = value; }
    }
    public List<PrePrestamosPlanesTasas> PrestamosPlanesTasas
    {
        get { return _prestamosPlanesTasas == null ? (_prestamosPlanesTasas = new List<PrePrestamosPlanesTasas>()) : _prestamosPlanesTasas; }
        set { _prestamosPlanesTasas = value; }
    }

    public List<PrePrestamosIpsPlanes> PrestamosIpsPlanes
    {
        get { return _prestamosIpsPlanes == null ? (_prestamosIpsPlanes = new List<PrePrestamosIpsPlanes>()) : _prestamosIpsPlanes; }
        set { _prestamosIpsPlanes = value; }
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

    public TGEFormasCobros FormaCobro
    {
        get { return _formaCobro == null ? (_formaCobro = new TGEFormasCobros()) : _formaCobro; }
        set { _formaCobro = value; }
    }

    public List<TGEFormasCobros> FormasCobros
    {
        get { return _formasCobros == null ? (_formasCobros = new List<TGEFormasCobros>()) : _formasCobros; }
        set { _formasCobros = value; }
    }

    public DateTime FechaInicioVigencia
    {
        get { return _fechaInicioVigencia; }
        set { _fechaInicioVigencia = value; }
    }

    public DateTime FechaFinVigencia
    {
        get { return _fechaFinVigencia; }
        set { _fechaFinVigencia = value; }
    }

    public decimal PorcentajeGasto
    {
        get { return _porcentajeGasto; }
        set { _porcentajeGasto = value; }
    }

    public decimal ImporteGasto
    {
        get { return _importeGasto; }
        set { _importeGasto = value; }
    }

    public List<TGECampos> Campos
    {
        get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
        set { _campos = value; }
    }

    public List<PrePrestamosBancoSolParametros> PrestamosBancoSolParametros
    {
        get { return _prestamosBancoSolParametros == null ? (_prestamosBancoSolParametros = new List<PrePrestamosBancoSolParametros>()) : _prestamosBancoSolParametros; }
        set { _prestamosBancoSolParametros = value; }
    }

    public XmlDocument LoteXML
    {
        get { return _loteXML; }//==null ? (_lotePrestamos=new XmlDocument()) : _lotePrestamos; }
        set { _loteXML = value; }
    }

        public decimal? PorcentajeCapitalSocial
        {
            get { return _porcentajeCapitalSocial; }
            set { _porcentajeCapitalSocial = value; }
        }

        public decimal? PorcentajeSeguroCuota
        {
            get { return _porcentajeSeguroCuota; }
            set { _porcentajeSeguroCuota = value; }
        }

        public int? IdFilial { get; set; }

        [Auditoria]
        public TGEMonedas Moneda
        {
            get { return _moneda == null ? (_moneda = new TGEMonedas()) : _moneda; }
            set { _moneda = value; }
        }

        public int? IdTipoCargo { get; set; }
        #endregion
    }
}
