
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Comunes.Entidades;
using Generales.Entidades;
namespace Bancos.Entidades
{
    [Serializable]
    public partial class TESBancosCuentas : Objeto
    {

        #region "Private Members"
        int _idBancoCuenta;
        string _numeroCuenta;
        string _denominacion;
        string _detalle;
        string _numeroBancoSucursal;
        decimal _importeDescubierto;
        decimal _saldoActual;
        bool _saldoActualModificado;
        decimal _saldoActualOriginal;
        DateTime _fechaAlta;
        DateTime _fechaDesde;
        DateTime _fechaHasta;
        TGEFiliales _filial;
        UsuariosAlta _usuarioAlta;
        TESBancos _banco;
        TESBancosCuentasTipos _bancoCuentaTipo;
        TGEMonedas _moneda;
        List<TESBancosCuentasMovimientos> _bancosCuentasMovimientos;
        List<TESBancosCuentasMovimientos> _bancosCuentasMovimientosPendientes;
        List<TESBancosCuentasMovimientos> _bancosCuentasMovimientosRechazados;
        List<TESBancosCuentasSaldos> _bancosCuentasSaldos;
        List<TGEArchivos> _archivos;
        List<TGEComentarios> _comentarios;
        List<TGECampos> _campos;


        public string Cbu { get; set; }
        public string Grupo { get; set; }
        #endregion

        #region "Constructors"
        public TESBancosCuentas()
        {
        }
        #endregion

        #region "Public Properties"
        [PrimaryKey()]
        public int IdBancoCuenta
        {
            get { return _idBancoCuenta; }
            set { _idBancoCuenta = value; }
        }

        [Auditoria()]
        public string NumeroCuenta
        {
            get { return _numeroCuenta == null ? string.Empty : _numeroCuenta; }
            set { _numeroCuenta = value; }
        }
        [Auditoria()]
        public string Denominacion
        {
            get { return _denominacion == null ? string.Empty : _denominacion; }
            set { _denominacion = value; }
        }

        /// <summary>
        /// Se utiliza para Filtrar en Pantalla
        /// </summary>
        public string Detalle
        {
            get { return _detalle == null ? string.Empty : _detalle; }
            set { _detalle = value; }
        }

        [Auditoria()]
        public string NumeroBancoSucursal
        {
            get { return _numeroBancoSucursal == null ? string.Empty : _numeroBancoSucursal; }
            set { _numeroBancoSucursal = value; }
        }

        public decimal ImporteDescubierto
        {
            get { return _importeDescubierto; }
            set { _importeDescubierto = value; }
        }

        public decimal SaldoActual
        {
            get { return _saldoActual; }
            set
            {
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

        [Auditoria()]
        public TGEFiliales Filial
        {
            get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
            set { _filial = value; }
        }

        public UsuariosAlta UsuarioAlta
        {
            get { return _usuarioAlta == null ? (_usuarioAlta = new UsuariosAlta()) : _usuarioAlta; }
            set { _usuarioAlta = value; }
        }

        [Auditoria()]
        public TESBancos Banco
        {
            get { return _banco == null ? (_banco = new TESBancos()) : _banco; }
            set { _banco = value; }
        }

        [Auditoria()]
        public TESBancosCuentasTipos BancoCuentaTipo
        {
            get { return _bancoCuentaTipo == null ? (_bancoCuentaTipo = new TESBancosCuentasTipos()) : _bancoCuentaTipo; }
            set { _bancoCuentaTipo = value; }
        }
        [Auditoria()]
        public TGEMonedas Moneda
        {
            get { return _moneda == null ? (_moneda = new TGEMonedas()) : _moneda; }
            set { _moneda = value; }
        }

        public List<TESBancosCuentasMovimientos> BancosCuentasMovimientos
        {
            get { return _bancosCuentasMovimientos == null ? (_bancosCuentasMovimientos = new List<TESBancosCuentasMovimientos>()) : _bancosCuentasMovimientos; }
            set { _bancosCuentasMovimientos = value; }
        }

        public List<TESBancosCuentasMovimientos> BancosCuentasMovimientosPendientes
        {
            get { return _bancosCuentasMovimientosPendientes == null ? (_bancosCuentasMovimientosPendientes = new List<TESBancosCuentasMovimientos>()) : _bancosCuentasMovimientosPendientes; }
            set { _bancosCuentasMovimientosPendientes = value; }
        }

        public List<TESBancosCuentasMovimientos> BancosCuentasMovimientosRechazados
        {
            get { return _bancosCuentasMovimientosRechazados == null ? (_bancosCuentasMovimientosRechazados = new List<TESBancosCuentasMovimientos>()) : _bancosCuentasMovimientosRechazados; }
            set { _bancosCuentasMovimientosRechazados = value; }
        }

        public List<TESBancosCuentasSaldos> BancosCuentasSaldos
        {
            get { return _bancosCuentasSaldos == null ? (_bancosCuentasSaldos = new List<TESBancosCuentasSaldos>()) : _bancosCuentasSaldos; }
            set { _bancosCuentasSaldos = value; }
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
        public string DescripcionFilialBancoTipoCuentaNumero
        {
            get
            {
                string txt;
                    switch (this.BancoCuentaTipo.IdBancoCuentaTipo)
                    {
                        case (int)EnumTesBancosCuentasTipos.FondoFijo:
                            txt = string.Concat(this.Filial.Filial, this.BancoCuentaTipo.Descripcion.Length > 0 ? " - " : string.Empty, this.BancoCuentaTipo.Descripcion, this.Denominacion.Length > 0 ? " - " : string.Empty, this.Denominacion);
                            break;
                        default:
                            txt =string.Concat(this.Filial.Filial, this.Banco.Descripcion.Length > 0 ? " - " : string.Empty, Banco.Descripcion, this.BancoCuentaTipo.Descripcion.Length > 0 ? " - " : string.Empty, this.BancoCuentaTipo.Descripcion, this.NumeroCuenta.Length > 0 ? " - " : string.Empty, this.NumeroCuenta, this.Denominacion.Length > 0 ? " - " : string.Empty, this.Denominacion);
                            break;
                    }
                return txt;
            }
            set { }
        }

        public string CuentaDatos
        {
            get { return string.Concat(this.NumeroCuenta, " - ", this.Denominacion, " - ",  this.Moneda.Moneda, " ", this.SaldoActual.ToString("N2")); }
            set { }
        }

        public int? IdCuentaContable { get; set; }
        public string CuentaContable { get; set; }
        #endregion
    }
}
