
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using CuentasPagar.Entidades;
//using CuentasPagar.Entidades;
namespace Contabilidad.Entidades
{
  [Serializable]
	public partial class CtbBienesUsos : Objeto
	{
		// Class CtbBienesUsos
	#region "Private Members"
	int _idBienUso;
	int? _idClasificador;
    CapSolicitudPagoDetalles _solicitudPagoDetalles;
    int? _idSolicitudPagoDetalle;
	DateTime _fechaActivacion;
	string _descripcion;
	int? _cantidad;
	int? _vidaUtil;
	int? _vidaTranscurrida;
	int? _vidaRestante;
	decimal? _importe;
	decimal? _amortAcumulada;
    DateTime? _fechaActivacionDesde;
    DateTime? _fechaActivacionHasta;

    List<CtbBienesUsosDetalles> _bienesUsosDetalles;

	#endregion
		
	#region "Constructors"
	public CtbBienesUsos()
	{
	}
	#endregion
		
	#region "Public Properties"
	public int IdBienUso
	{
		get{return _idBienUso ;}
		set{_idBienUso = value;}
	}
	public int? IdClasificador
	{
		get{return _idClasificador;}
		set{_idClasificador = value;}
	}

    public CapSolicitudPagoDetalles SolicitudPagoDetalles
    {
        get { return _solicitudPagoDetalles == null ? (_solicitudPagoDetalles = new CapSolicitudPagoDetalles()) : _solicitudPagoDetalles; }
        set { _solicitudPagoDetalles = value; }
    }

    public int? IdSolicitudPagoDetalle
    {
        get { return _idSolicitudPagoDetalle; }
        set { _idSolicitudPagoDetalle = value; }
    }

	public DateTime FechaActivacion
	{
		get{return _fechaActivacion;}
		set{_fechaActivacion = value;}
	}

	public string Descripcion
	{
		get{return _descripcion == null ? string.Empty : _descripcion ;}
		set{_descripcion = value;}
	}

	public int? Cantidad
	{
		get{return _cantidad;}
		set{_cantidad = value;}
	}

	public int? VidaUtil
	{
		get{return _vidaUtil;}
		set{_vidaUtil = value;}
	}

	public int? VidaTranscurrida
	{
		get{return _vidaTranscurrida;}
		set{_vidaTranscurrida = value;}
	}

	public int? VidaRestante
	{
		get{return _vidaRestante;}
		set{_vidaRestante = value;}
	}

	public decimal? Importe
	{
		get{return _importe;}
		set{_importe = value;}
	}

	public decimal? AmortAcumulada
	{
		get{return _amortAcumulada;}
		set{_amortAcumulada = value;}
	}

    public DateTime? FechaActivacionDesde
    {
        get { return _fechaActivacionDesde; }
        set { _fechaActivacionDesde = value; }
    }

    public DateTime? FechaActivacionHasta
    {
        get { return _fechaActivacionHasta; }
        set { _fechaActivacionHasta = value; }
    }

    public List<CtbBienesUsosDetalles> BienesUsosDetalles
    {
        get { return _bienesUsosDetalles == null ? (_bienesUsosDetalles = new List<CtbBienesUsosDetalles>()) : _bienesUsosDetalles; }
        set { _bienesUsosDetalles = value; }
    }

	#endregion
	}
}
