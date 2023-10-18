
using System;
using System.Collections.Generic;
using Generales.Entidades;
using Comunes.Entidades;
using Afiliados.Entidades;
using Bancos.Entidades;
using System.Xml;
namespace Tesorerias.Entidades
{
  [Serializable]
	public partial class TESCajasMovimientos : Objeto
	{
	#region "Private Members"
	int _idCajaMovimiento;
	TESCajasMonedas _cajaMoneda;
	DateTime _fecha;
	string _descripcion;
	TGETiposOperaciones _tipoOperacion;
	int _idRefTipoOperacion;
	decimal _importe;
    //TGETiposValores _tipoValor;
	//List<TESCheques> _cheques;
    List<TESCajasMovimientosValores> _cajasMovimientosValores;
    AfiAfiliados _afiliado;
    DateTime? _fechaDesde;
    DateTime? _fechaHasta;
    int _idFilial;
        List<TGECampos> _campos;
        List<TESCajasMovimientosConceptosContables> _cajasMovimientosConceptosContables;
    XmlDocumentSerializationWrapper _loteCajasMovimientosValores;
	#endregion
		
	#region "Constructors"
	public TESCajasMovimientos()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdCajaMovimiento
	{
		get{return _idCajaMovimiento ;}
		set{_idCajaMovimiento = value;}
	}

    public decimal MonedaCotizacion { get; set; }

        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }
        }

        public TESCajasMonedas CajaMoneda
	{
        get { return _cajaMoneda == null ? (_cajaMoneda = new TESCajasMonedas()) : _cajaMoneda; }
		set{_cajaMoneda = value;}
	}

	public DateTime Fecha
	{
		get{return _fecha;}
		set{_fecha = value;}
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

    public int IdFilial
    {
        get { return _idFilial; }
        set { _idFilial = value; }
    }

	public string Descripcion
	{
		get{return _descripcion == null ? string.Empty : _descripcion ;}
		set{_descripcion = value;}
	}

	public TGETiposOperaciones TipoOperacion
	{
        get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
		set{_tipoOperacion = value;}
	}

	public int IdRefTipoOperacion
	{
		get{return _idRefTipoOperacion;}
		set{_idRefTipoOperacion = value;}
	}

	public decimal Importe
	{
		get{return _importe;}
		set{_importe = value;}
	}

    /// <summary>
    /// Para mostrar el Importe con formato negativo en grillas
    /// </summary>
    public decimal ImporteMostrar
    {
        get { return this.TipoOperacion.TipoMovimiento.IdTipoMovimiento == (int)EnumTGETiposMovimientos.Credito ? this.Importe : this.Importe * -1; }
        set { }
    }

    //public TGETiposValores TipoValor
    //{
    //    get { return _tipoValor == null ? (_tipoValor = new TGETiposValores()) : _tipoValor; }
    //    set{_tipoValor = value;}
    //}

	
    //public List<TESCheques> Cheques
    //{
    //    get{return _cheques==null ? (_cheques = new List<TESCheques>()) : _cheques;}
    //    set{_cheques = value;}
    //}

    public List<TESCajasMovimientosValores> CajasMovimientosValores
    {
        get { return _cajasMovimientosValores == null ? (_cajasMovimientosValores = new List<TESCajasMovimientosValores>()) : _cajasMovimientosValores; }
        set { _cajasMovimientosValores = value; }
    }

    public AfiAfiliados Afiliado
    {
        get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; }
        set { _afiliado = value; }
    }

    public List<TESCajasMovimientosConceptosContables> CajasMovimientosConceptosContables
    {
        get { return _cajasMovimientosConceptosContables == null ? (_cajasMovimientosConceptosContables = new List<TESCajasMovimientosConceptosContables>()) : _cajasMovimientosConceptosContables; }
        set { _cajasMovimientosConceptosContables = value; }
    }

    public XmlDocument LoteCajasMovimientosValores
    {
        get { return _loteCajasMovimientosValores; }
        set { _loteCajasMovimientosValores = value; }
    }

        public DateTime FechaAbrir { get; set; }
        #endregion
    }
}
