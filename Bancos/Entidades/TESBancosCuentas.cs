
using System;
using System.Collections.Generic;
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
        string _numeroBancoSucursal;
        decimal _importeDescubierto;
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

        public string DescripcionFilialBancoTipoCuentaNumero
        {
            get { return string.Concat(this.Filial.Filial, this.Banco.Descripcion.Length>0? " - " : string.Empty, Banco.Descripcion, this.BancoCuentaTipo.Descripcion.Length>0?  " - " : string.Empty, this.BancoCuentaTipo.Descripcion, this.NumeroCuenta.Length>0?  " - " : string.Empty, this.NumeroCuenta); }
            set { }
        }

        #endregion
    }
}
