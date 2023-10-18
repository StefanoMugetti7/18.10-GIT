
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Seguridad.Entidades;
using Generales.Entidades;
using Contabilidad.Entidades;
namespace Cargos.Entidades
{
    [Serializable]
    public partial class CarTiposCargos : Objeto
    {
        #region "Private Members"
        int _idTipoCargo;
        int _codigoCargo;
        string _tipoCargo;
        decimal _importe;
        int _prioridad;
        bool _permiteCuotas;
        int _cantidadMaximaCuotas;
        bool _aplicaSiTieneParticipantes;
        bool _aplicaPorCantidadParticipantes;
        bool _cargoIrregular;
        int _sumarizaConIdTipoCargo;
        string _sumarizaConTipoCargo;
        DateTime _fechaAlta;
        CarTiposCargosProcesos _tipoCargoProceso;
        UsuariosAlta _usuarioAlta;
        TGETiposOperaciones _tipoOperacion;
        CtbCuentasContables _cuentaContable;
        List<TGEFormasCobros> _formasCobros;
        List<TGEArchivos> _archivos;
        List<TGEComentarios> _comentarios;
        List<CarTiposCargosCategorias> _tiposCargosCategorias;
        #endregion

        #region "Constructors"
        public CarTiposCargos()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdTipoCargo
        {
            get { return _idTipoCargo; }
            set { _idTipoCargo = value; }
        }
        [Auditoria]
        public int CodigoCargo
        {
            get { return _codigoCargo; }
            set { _codigoCargo = value; }
        }
        [Auditoria]
        public string TipoCargo
        {
            get { return _tipoCargo == null ? string.Empty : _tipoCargo; }
            set { _tipoCargo = value; }
        }
        [Auditoria]
        public decimal Importe
        {
            get { return _importe; }
            set { _importe = value; }
        }
        [Auditoria]
        public int Prioridad
        {
            get { return _prioridad; }
            set { _prioridad = value; }
        }

        public int SumarizaConIdTipoCargo
        {
            get { return _sumarizaConIdTipoCargo; }
            set { _sumarizaConIdTipoCargo = value; }
        }

        public string SumarizaConTipoCargo
        {
            get { return _sumarizaConTipoCargo == null ? string.Empty : _sumarizaConTipoCargo; }
            set { _sumarizaConTipoCargo = value; }
        }

        [Auditoria]
        public bool PermiteCuotas
        {
            get { return _permiteCuotas; }
            set { _permiteCuotas = value; }
        }

        [Auditoria]
        public bool AplicaSiTieneParticipantes
        {
            get { return _aplicaSiTieneParticipantes; }
            set { _aplicaSiTieneParticipantes = value; }
        }

        [Auditoria]
        public bool AplicaPorCantidadParticipantes
        {
            get { return _aplicaPorCantidadParticipantes; }
            set { _aplicaPorCantidadParticipantes = value; }
        }

        [Auditoria]
        public bool CargoIrregular
        {
            get { return _cargoIrregular; }
            set { _cargoIrregular = value; }
        }

        public string PermiteCuotasTexto
        {
            get { return this.ObtenerValorBoolean(this._permiteCuotas); }
            set { }
        }

        public DateTime FechaAlta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }

        [Auditoria]
        public int CantidadMaximaCuotas
        {
            get { return _cantidadMaximaCuotas; }
            set { _cantidadMaximaCuotas = value; }
        }

        public UsuariosAlta UsuarioAlta
        {
            get { return _usuarioAlta == null ? (_usuarioAlta = new UsuariosAlta()) : _usuarioAlta; }
            set { _usuarioAlta = value; }
        }

        [Auditoria]
        public CarTiposCargosProcesos TipoCargoProceso
        {
            get { return _tipoCargoProceso == null ? (_tipoCargoProceso = new CarTiposCargosProcesos()) : _tipoCargoProceso; }
            set { _tipoCargoProceso = value; }
        }

        [Auditoria()]
        public TGETiposOperaciones TipoOperacion
        {
            get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
            set { _tipoOperacion = value; }
        }

        [Auditoria()]
        public CtbCuentasContables CuentaContable
        {
            get { return _cuentaContable == null ? (_cuentaContable = new CtbCuentasContables()) : _cuentaContable; }
            set { _cuentaContable = value; }
        }

        public List<TGEFormasCobros> FormasCobros
        {
            get { return _formasCobros == null ? (_formasCobros = new List<TGEFormasCobros>()) : _formasCobros; }
            set { _formasCobros = value; }
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

        public List<CarTiposCargosCategorias> TiposCargosCategorias
        {
            get { return _tiposCargosCategorias == null ? (_tiposCargosCategorias = new List<CarTiposCargosCategorias>()) : _tiposCargosCategorias; }
            set { _tiposCargosCategorias = value; }
        }

        #endregion
    }
}
