
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Prestamos.Entidades
{
  [Serializable]
	public partial class PrePrestamosCuotas : Objeto
	{

	#region "Private Members"
	int _idPrestamoCuota;
    int _idSimulacionCuota;
	int _idPrestamo;
    int _idSimulacion;
	decimal _cuotaNumero; 
	DateTime _cuotaFechaVencimiento;
	decimal _importeCuota;
	decimal _importeInteres;
	decimal _importeAmortizacion;
	decimal _importeSaldo;
    int _idFilialCobro;
    DateTime _fechaCobro;
    int? _idCuentaCorriente;
    decimal? _ImporteCobrado;
    int? _idTipoOperacionCobro;
    int? _idRefTipoOperacionCobro;
    bool _incluir;
    string _detalle;
        decimal? _importeInteresNoCobrado;
        decimal? _importeCapitalSocial;
        decimal? _importeNetoAmortizacion;
        bool _incluirOriginal;
        bool _incluirModificado;

        #endregion

        #region "Constructors"
        public PrePrestamosCuotas()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdPrestamoCuota
	{
		get{return _idPrestamoCuota ;}
		set{_idPrestamoCuota = value;}
	}

      public int IdSimulacionCuota
      {
          get { return _idSimulacionCuota; }
          set { _idSimulacionCuota = value; }
      }

	public int IdPrestamo
	{
		get{return _idPrestamo;}
		set{_idPrestamo = value;}
	}

    public int IdSimulacion
    {
        get { return _idSimulacion; }
        set { _idSimulacion = value; }
    }

	public decimal CuotaNumero
	{
		get{return _cuotaNumero;}
		set{_cuotaNumero = value;}
	}

	public DateTime CuotaFechaVencimiento
	{
		get{return _cuotaFechaVencimiento;}
		set{_cuotaFechaVencimiento = value;}
	}

	public decimal ImporteCuota
	{
		get{return _importeCuota;}
		set{_importeCuota = value;}
	}

	public decimal ImporteInteres
	{
		get{return _importeInteres;}
		set{_importeInteres = value;}
	}

	public decimal ImporteAmortizacion
	{
		get{return _importeAmortizacion;}
		set{_importeAmortizacion = value;}
	}

	public decimal ImporteSaldo
	{
		get{return _importeSaldo;}
		set{_importeSaldo = value;}
	}

    public int IdFilialCobro
    {
        get { return _idFilialCobro; }
        set { _idFilialCobro = value; }
    }

    public DateTime FechaCobro
    {
        get { return _fechaCobro; }
        set { _fechaCobro = value; }
    }

    public int? IdCuentaCorriente
    {
        get { return _idCuentaCorriente; }
        set { _idCuentaCorriente = value; }
    }

    public decimal? ImporteCobrado
    {
        get { return _ImporteCobrado; }
        set { _ImporteCobrado = value; }
    }

    public int? IdTipoOperacionCobro
    {
        get { return _idTipoOperacionCobro; }
        set { _idTipoOperacionCobro = value; }
    }

    public int? IdRefTipoOperacionCobro
    {
        get { return _idRefTipoOperacionCobro; }
        set { _idRefTipoOperacionCobro = value; }
    }

    public bool Incluir
    {
            //get { return _incluir; }
            //set
            //{
            //    if (_incluir != _incluirOriginal && !this._incluirModificado && _incluir != value)
            //    {
            //        this._incluirModificado = true;
            //        this._incluirOriginal = _incluir;
            //    }
            //    _incluir = value;
            //}
            get { return _incluir; }
            set { _incluir = value; }
        }

    public bool IncluirOriginal
    {
        get { return _incluirOriginal; }
        set { _incluirOriginal = value; }
    }

        public string Detalle
    {
        get { return _detalle == null ? string.Empty : _detalle; }
        set { _detalle = value; }
    }
        public decimal? ImporteInteresNoCobrado
        {
            get { return _importeInteresNoCobrado; }
            set { _importeInteresNoCobrado = value; }
        }
        public decimal? ImporteCapitalSocial
        {
            get { return _importeCapitalSocial; }
            set { _importeCapitalSocial = value; }
        }
        public decimal? ImporteNetoAmortizacion
        {
            get { return _importeNetoAmortizacion; }
            set { _importeNetoAmortizacion = value; }
        }
        public string Moneda { get; set; }

        public decimal ? ImporteSeguroCuota { get; set; }

        public decimal? UnidadesCuota { get; set; }

        public decimal? UnidadesAmortizacion { get; set; }

        public decimal? UnidadesIntereses { get; set; }

        public decimal ? PorcentajeSeguroCuota { get; set; }

        public decimal ? Coeficiente { get; set; }

        public decimal? UnidadesSaldo { get; set; }

        public decimal? DiferenciaVariacionCoeficiente { get; set; }
        public decimal? ImporteGastoCuota { get; set; }
        public decimal? ImporteIvaCuota { get; set; }


        #endregion
    }
}
