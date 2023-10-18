
using System;
using System.Collections.Generic;
using System.Linq;
using Afiliados.Entidades;
using Generales.Entidades;
using Comunes.Entidades;
using Seguridad.Entidades;
using Cargos.Entidades;
namespace Prestamos.Entidades
{
  [Serializable]
	public partial class PrePrestamos : Objeto
	{

	#region "Private Members"
	int _idPrestamo;
    int _idSimulacion;
	DateTime _fechaPrestamo;
    DateTime _fechaPreAutorizado;
    DateTime _fechaAutorizado;
    DateTime _fechaValidezAutorizado;
    DateTime _fechaConfirmacion;
    DateTime _fechaCancelacion;
    DateTime _fechaConfirmacionCancelacion;
    int _nroDeIdentificacion;
    int _cantidadCuotas;
    decimal _importeSolicitado;
    decimal _importeAutorizado;
    decimal _subPeriodico;
    decimal _saldoCapital;
    decimal _saldoDeuda;
    decimal _importeCuota;
    decimal _importeCancelacion;
    decimal _importeGastos;
    decimal _importeInteres;
    decimal _porcentajeGastos;
    decimal _bonificacion;
    decimal _importePrestamo;
    int _idRefPrestamoCancelacion;
    decimal _amortizacionCuotasNoCobradas;
    decimal _interesesNoDevengados;
    decimal _comisionCancelacion;
    //int _idFilialCancelacion;
    decimal _importeExcedido;
    TGEFiliales _filial;
    TGEFilialesPagos _filialPago;
    UsuariosPreAutorizar _usuarioPreAutorizar;
    UsuariosAutorizar _usuarioAutorizar;
    UsuariosCancelacion _usuarioCancelacion;
	AfiAfiliados _afiliado;
	TGETiposOperaciones _tipoOperacione;
    PrePrestamosPlanes _prestamoPlan;
	TGEMonedas _moneda;
	TGETiposValores  _tipoValor;
    UsuariosConfirmacion _usuarioConfirmacion;
    CarTiposCargos _tipoCargo;
    TGEFormasCobrosAfiliados _formaCobroAfiliado;
    TGEFilialesCancelacion _filialCancelacion;
    TGETiposValoresCancelacion _tipoValorCancelacion;
	List<PrePrestamosCuotas> _prestamosCuotas;
    List<TGEArchivos> _archivos;
    List<TGEComentarios> _comentarios;
    List<TGECampos> _campos;
    bool _confirmarExcedido;
    List<PrePrestamos> _cancelaciones;
    List<CarCuentasCorrientes> _cargosExcedidos;
    List<CarCuentasCorrientes> _cuotasPendientesCuentaCorriente;
    //List<PrePrestamos> _anticipos;
	#endregion
		
	#region "Constructors"
	public PrePrestamos()
	{
	}
	#endregion
		
	#region "Public Properties"
    [PrimaryKey()]
	public int IdPrestamo
	{
		get{return _idPrestamo ;}
		set{_idPrestamo = value;}
	}

    public int IdSimulacion
    {
        get { return _idSimulacion; }
        set { _idSimulacion = value; }
    }

    public DateTime FechaPreAutorizado
    {
        get { return _fechaPreAutorizado; }
        set { _fechaPreAutorizado = value; }
    }

    public DateTime FechaAutorizado
    {
        get { return _fechaAutorizado; }
        set { _fechaAutorizado = value; }
    }

    public DateTime FechaValidezAutorizado
    {
        get { return _fechaValidezAutorizado; }
        set { _fechaValidezAutorizado = value; }
    }

    public DateTime FechaConfirmacion
    {
        get { return _fechaConfirmacion; }
        set { _fechaConfirmacion = value; }
    }

    public DateTime FechaCancelacion
    {
        get { return _fechaCancelacion; }
        set { _fechaCancelacion = value; }
    }

    public DateTime FechaConfirmacionCancelacion
    {
        get { return _fechaConfirmacionCancelacion; }
        set { _fechaConfirmacionCancelacion = value; }
    }
      
    public int NroDeIdentificacion
    {
        get { return _nroDeIdentificacion == 0 ? this.IdPrestamo : _nroDeIdentificacion; }
        set { _nroDeIdentificacion = value; }
    }
    [Auditoria()]
    public int CantidadCuotas
    {
        get { return _cantidadCuotas; }
        set { _cantidadCuotas = value; }
    }
    [Auditoria()]
    public decimal ImporteSolicitado
    {
        get { return _importeSolicitado; }
        set { _importeSolicitado = value; }
    }

      /// <summary>
      /// Utilizado como importe Neto a Pagar en Caja
      /// </summary>
    [Auditoria()]
    public decimal ImporteAutorizado
    {
        get { return _importeAutorizado; }
        set { _importeAutorizado = value; }
    }

    [Auditoria()]
    public decimal Bonificacion
    {
        get { return _bonificacion; }
        set { _bonificacion = value; }
    }

    [Auditoria()]
    public decimal ImportePrestamo
    {
        get { return _importePrestamo; }
        set { _importePrestamo = value; }
    }

      /// <summary>
      /// Importe utilizado para los calculos de gastos y cuponera:
      /// ImporteAutorizado + ImporteCancelaciones + ImporteExcedido
      /// </summary>
    public decimal ObtenerImportePrestamo()
    {
        return (this.ImporteAutorizado > 0 ? this.ImporteAutorizado : this.ImporteSolicitado) + this.ImporteCancelaciones + this.ImporteExcedido + this.ImporteGastos;
    }

    public decimal ImporteInteres
    {
        get { return _importeInteres; }
        set { _importeInteres = value; }
    }

    public decimal SubPeriodico
    {
        get { return _subPeriodico; }
        set { _subPeriodico = value; }
    }

    public decimal SaldoCapital
    {
        get { return _saldoCapital; }
        set { _saldoCapital = value; }
    }

    public decimal ImporteCuota
    {
        get { return _importeCuota; }
        set { _importeCuota = value; }
    }

    public decimal SaldoDeuda
    {
        get { return _saldoDeuda; }
        set { _saldoDeuda = value; }
    }

    public decimal ImporteCancelacion
    {
        get { return _importeCancelacion; }
        set { _importeCancelacion = value; }
    }

    public decimal ImporteGastos
    {
        get { return _importeGastos; }
        set { _importeGastos = value; }
    }

    public decimal PorcentajeGastos
    {
        get { return _porcentajeGastos; }
        set { _porcentajeGastos = value; }
    }

    public int IdRefPrestamoCancelacion
    {
        get { return _idRefPrestamoCancelacion; }
        set { _idRefPrestamoCancelacion = value; }
    }

    //public int IdFilialCancelacion
    //{
    //    get { return _idFilialCancelacion; }
    //    set { _idFilialCancelacion = value; }
    //}

    public decimal ImporteExcedido
    {
        get { return _importeExcedido; }
        set { _importeExcedido = value; }
    }

    public TGEFiliales Filial
    {
        get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
        set { _filial = value; }
    }

    [Auditoria()]
    public TGEFilialesPagos FilialPago
    {
        get { return _filialPago == null ? (_filialPago = new TGEFilialesPagos()) : _filialPago; }
        set { _filialPago = value; }
    }

    public UsuariosPreAutorizar UsuarioPreAutorizar
    {
        get { return _usuarioPreAutorizar == null ? (_usuarioPreAutorizar = new UsuariosPreAutorizar()) : _usuarioPreAutorizar; }
        set { _usuarioPreAutorizar = value; }
    }

    public UsuariosAutorizar UsuarioAutorizar
    {
        get { return _usuarioAutorizar == null ? (_usuarioAutorizar = new UsuariosAutorizar()) : _usuarioAutorizar; }
        set { _usuarioAutorizar = value; }
    }

	public DateTime FechaPrestamo
	{
		get{return _fechaPrestamo;}
		set{_fechaPrestamo = value;}
	}

	public AfiAfiliados Afiliado
	{
        get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; }
		set{_afiliado = value;}
	}

    [Auditoria()]
    public TGETiposOperaciones TipoOperacion
	{
        get { return _tipoOperacione == null ? (_tipoOperacione = new TGETiposOperaciones()) : _tipoOperacione; }
		set{_tipoOperacione = value;}
	}

    public TGEMonedas Moneda
    {
        get { return _moneda == null ? (_moneda = new TGEMonedas()) : _moneda; }
        set { _moneda = value; }
    }

    [Auditoria()]
    public CarTiposCargos TipoCargo
    {
        get { return _tipoCargo == null ? (_tipoCargo = new CarTiposCargos()) : _tipoCargo; }
        set { _tipoCargo = value; }
    }

    [Auditoria()]
    public PrePrestamosPlanes PrestamoPlan
    {
        get { return _prestamoPlan == null ? (_prestamoPlan = new PrePrestamosPlanes()) : _prestamoPlan; }
        set { _prestamoPlan = value; }
    }


	public TGETiposValores TipoValor
	{
        get { return _tipoValor == null ? (_tipoValor = new TGETiposValores()) : _tipoValor; }
		set{_tipoValor = value;}
	}

    public UsuariosConfirmacion UsuarioConfirmacion
    {
        get { return _usuarioConfirmacion == null ? (_usuarioConfirmacion = new UsuariosConfirmacion()) : _usuarioConfirmacion; }
        set { _usuarioConfirmacion = value; }
    }

    public UsuariosCancelacion UsuarioCancelacion
    {
        get { return _usuarioCancelacion == null ? (_usuarioCancelacion = new UsuariosCancelacion()) : _usuarioCancelacion; }
        set { _usuarioCancelacion = value; }
    }

    [Auditoria()]
    public TGEFormasCobrosAfiliados FormaCobroAfiliado
    {
        get { return _formaCobroAfiliado == null ? (_formaCobroAfiliado = new TGEFormasCobrosAfiliados()) : _formaCobroAfiliado; }
        set { _formaCobroAfiliado = value; }
    }

    [Auditoria()]
    public TGEFilialesCancelacion FilialCancelacion
    {
        get { return _filialCancelacion == null ? (_filialCancelacion = new TGEFilialesCancelacion()) : _filialCancelacion; }
        set { _filialCancelacion = value; }
    }

    [Auditoria()]
    public TGETiposValoresCancelacion TipoValorCancelacion
    {
        get { return _tipoValorCancelacion == null ? (_tipoValorCancelacion = new TGETiposValoresCancelacion()) : _tipoValorCancelacion; }
        set { _tipoValorCancelacion = value; }
    }

	public List<PrePrestamosCuotas> PrestamosCuotas
	{
		get{return _prestamosCuotas==null ? (_prestamosCuotas = new List<PrePrestamosCuotas>()) : _prestamosCuotas;}
		set{_prestamosCuotas = value;}
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

    public bool ConfirmarExcedido
    {
        get { return _confirmarExcedido; }
        set { _confirmarExcedido = value; }
    }
      
    public List<PrePrestamos> Cancelaciones
    {
        get { return _cancelaciones == null ? (_cancelaciones = new List<PrePrestamos>()) : _cancelaciones; }
        set { _cancelaciones = value; }
    }

    public List<CarCuentasCorrientes> CargosExcedidos
    {
        get { return _cargosExcedidos == null ? (_cargosExcedidos = new List<CarCuentasCorrientes>()) : _cargosExcedidos; }
        set { _cargosExcedidos = value; }
    }

    public List<CarCuentasCorrientes> CuotasPendientesCuentaCorriente
    {
        get { return _cuotasPendientesCuentaCorriente == null ? (_cuotasPendientesCuentaCorriente = new List<CarCuentasCorrientes>()) : _cuotasPendientesCuentaCorriente; }
        set { _cuotasPendientesCuentaCorriente = value; }
    }

    public List<TGECampos> Campos
    {
        get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
        set { _campos = value; }
    }

      /// <summary>
      /// Devuelve la Sumatorias de los importes a Cancelar en las Cancelaciones incluidas
      /// </summary>
      public decimal ImporteCancelaciones
      {
          get { return this.Cancelaciones.Count > 0 ? this.Cancelaciones.Sum(x => x.ImporteCancelacion) : 0; }
          set { }
      }

      /// <summary>
      /// Devuelve la Sumatorias de los importes de los cargos incluidos
      /// </summary>
      public decimal ImporteCargosPendientes
      {
          get { return this.CargosExcedidos.Count > 0 ? this.CargosExcedidos.Sum(x => x.Importe) : 0; }
          set { }
      }

      [Auditoria]
      public decimal AmortizacionCuotasNoCobradas
      {
          get { return _amortizacionCuotasNoCobradas; }
          set { _amortizacionCuotasNoCobradas = value; }
      }

      [Auditoria]
      public decimal InteresesNoDevengados
      {
          get { return _interesesNoDevengados; }
          set { _interesesNoDevengados = value; }
      }

      [Auditoria]
      public decimal ComisionCancelacion
      {
          get { return _comisionCancelacion; }
          set { _comisionCancelacion = value; }
      }

	#endregion
	}
}
