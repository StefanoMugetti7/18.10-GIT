
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
using System.Xml;
namespace Prestamos.Entidades
{
  [Serializable]
	public partial class PrePrestamosCesiones : Objeto
	{
		// Class PrePrestamosCesiones
	#region "Private Members"
	int _idPrestamoCesion;
	PreCesionarios _cesionario;
	string _descripcion;
	decimal _tasa;
	decimal _vAN;
	int _cantidad;
	decimal _totalAmortizacion;
	decimal _totalInteres;
	DateTime _fechaAlta;
	int _idUsuarioAlta;
    int? _idUsuarioAutorizar;
    DateTime? _fechaAutorizar;
    DateTime? _fechaBaja;
    List<PrePrestamosCesionesDetalles> _prestamosCesionesDetalles;
    List<TGEArchivos> _archivos;
    List<TGEComentarios> _comentarios;
    TGETiposOperaciones _tipoOperacione;
    decimal _importeCuotasFacturadas;
    decimal _importeCuotasFacturadasPendientes;
    decimal _importeAmortizacionContable;
    decimal _importeInteresContable;
    decimal _importeAmortizacionContableNoCorriente;
    decimal _importeInteresContableNoCorriente;
    XmlDocumentSerializationWrapper _lotePrestamos;
	#endregion
		
	#region "Constructors"
	public PrePrestamosCesiones()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdPrestamoCesion
	{
		get{return _idPrestamoCesion ;}
		set{_idPrestamoCesion = value;}
	}
    public PreCesionarios Cesionario
	{
        get { return _cesionario == null ? (_cesionario = new PreCesionarios()) : _cesionario; }
		set{_cesionario = value;}
	}

	public string Descripcion
	{
		get{return _descripcion == null ? string.Empty : _descripcion ;}
		set{_descripcion = value;}
	}

	public decimal Tasa
	{
		get{return _tasa;}
		set{_tasa = value;}
	}

	public decimal VAN
	{
		get{return _vAN;}
		set{_vAN = value;}
	}

	public int Cantidad
	{
		get{return _cantidad;}
		set{_cantidad = value;}
	}

	public decimal TotalAmortizacion
	{
		get{return _totalAmortizacion;}
		set{_totalAmortizacion = value;}
	}

	public decimal TotalInteres
	{
		get{return _totalInteres;}
		set{_totalInteres = value;}
	}

	public DateTime FechaAlta
	{
		get{return _fechaAlta;}
		set{_fechaAlta = value;}
	}

    public DateTime? FechaAutorizar
    {
        get { return _fechaAutorizar; }
        set { _fechaAutorizar = value; }
    }

    public DateTime? FechaBaja
    {
        get { return _fechaBaja; }
        set { _fechaBaja = value; }
    }

	public int IdUsuarioAlta
	{
		get{return _idUsuarioAlta;}
		set{_idUsuarioAlta = value;}
	}

    public int? IdUsuarioAutorizar
    {
        get { return _idUsuarioAutorizar; }
        set { _idUsuarioAutorizar = value; }
    }

    public List<PrePrestamosCesionesDetalles> PrestamosCesionesDetalles
    {
        get { return _prestamosCesionesDetalles == null ? (_prestamosCesionesDetalles = new List<PrePrestamosCesionesDetalles>()) : _prestamosCesionesDetalles; }
        set { _prestamosCesionesDetalles = value; }
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

    [Auditoria()]
    public TGETiposOperaciones TipoOperacion
    {
        get { return _tipoOperacione == null ? (_tipoOperacione = new TGETiposOperaciones()) : _tipoOperacione; }
        set { _tipoOperacione = value; }
    }

    public decimal ImporteCuotasFacturadas
    {
        get { return _importeCuotasFacturadas; }
        set { _importeCuotasFacturadas = value; }
    }

    public decimal ImporteCuotasFacturadasPendientes
    {
        get { return _importeCuotasFacturadasPendientes; }
        set { _importeCuotasFacturadasPendientes = value; }
    }

    public decimal ImporteAmortizacionContable
    {
        get { return _importeAmortizacionContable; }
        set { _importeAmortizacionContable = value; }
    }

    public decimal ImporteInteresContable
    {
        get { return _importeInteresContable; }
        set { _importeInteresContable = value; }
    }

    public decimal ImporteAmortizacionContableNoCorriente
    {
        get { return _importeAmortizacionContableNoCorriente; }
        set { _importeAmortizacionContableNoCorriente = value; }
    }

    public decimal ImporteInteresContableNoCorriente
    {
        get { return _importeInteresContableNoCorriente; }
        set { _importeInteresContableNoCorriente = value; }
    }

    public XmlDocument LotePrestamos
    {
        get { return _lotePrestamos;}//==null ? (_lotePrestamos=new XmlDocument()) : _lotePrestamos; }
        set { _lotePrestamos = value; }
    }

	#endregion
	}
}
