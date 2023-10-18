
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using CuentasPagar.Entidades;
using Generales.Entidades;

namespace Contabilidad.Entidades
{
  [Serializable]
	public partial class CtbBienesUsos : Objeto
	{
		// Class CtbBienesUsos
	#region "Private Members"
	int _idBienUso;
	//int? _idClasificador;
    //int? _idSolicitudPagoDetalle;
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
    CtbClasificadores _clasificador;
        TGEFiliales _filial;
    CtbCuentasContables _cuentaContable;
    CapSolicitudPagoDetalles _solicitudPagoDetalles;
    List<CtbBienesUsosDetalles> _bienesUsosDetalles;
        List<TGECampos> _campos;
	#endregion
		
	#region "Constructors"
	public CtbBienesUsos()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdBienUso
	{
		get{return _idBienUso ;}
		set{_idBienUso = value;}
	}
    //public int? IdClasificador
    //{
    //    get{return _idClasificador;}
    //    set{_idClasificador = value;}
    //}

    //public int? IdSolicitudPagoDetalle
    //{
    //    get { return _idSolicitudPagoDetalle; }
    //    set { _idSolicitudPagoDetalle = value; }
    //}

      [Auditoria]
	public DateTime FechaActivacion
	{
		get{return _fechaActivacion;}
		set{_fechaActivacion = value;}
	}
      [Auditoria]
	public string Descripcion
	{
		get{return _descripcion == null ? string.Empty : _descripcion ;}
		set{_descripcion = value;}
	}
      [Auditoria]
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

      [Auditoria]
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

        public TGEFiliales Filial
        {
            get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
            set { _filial = value; }
        }

      [Auditoria]
    public CtbClasificadores Clasificador
    {
        get { return _clasificador == null ? (_clasificador = new CtbClasificadores()) : _clasificador; }
        set { _clasificador = value; }
    }

      [Auditoria]
    public CtbCuentasContables CuentaContable
    {
        get { return _cuentaContable == null ? (_cuentaContable = new CtbCuentasContables()) : _cuentaContable; }
        set { _cuentaContable = value; }
    }

      [Auditoria]
        public CapSolicitudPagoDetalles SolicitudPagoDetalles
        {
            get { return _solicitudPagoDetalles == null ? (_solicitudPagoDetalles = new CapSolicitudPagoDetalles()) : _solicitudPagoDetalles; }
            set { _solicitudPagoDetalles = value; }
        }

        public List<CtbBienesUsosDetalles> BienesUsosDetalles
    {
        get { return _bienesUsosDetalles == null ? (_bienesUsosDetalles = new List<CtbBienesUsosDetalles>()) : _bienesUsosDetalles; }
        set { _bienesUsosDetalles = value; }
    }

        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }
        }
        #endregion
    }
}
