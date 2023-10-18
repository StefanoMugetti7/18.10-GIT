
using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
namespace Tesorerias.Entidades
{
    [Serializable]
    public partial class TESTesoreriasMovimientos : Objeto
    {
        #region "Private Members"
        int _idTesoreriaMovimiento;
        TESTesoreriasMonedas _tesoreriaMoneda;
        TGETiposOperaciones _tipoOperacion;
        int _idRefTipoOperacion;
        string _refTipoOperacion;
        int? _idBanco;
        string _banco;
        DateTime _fecha;
        string _descripcion;
        decimal _importe;
        TGETiposValores _tipoValor;
        TESCajas _caja;
        string _transaccion;
        List<TGEArchivos> _archivos;
        #endregion

        #region "Constructors"
        public TESTesoreriasMovimientos()
        {
        }
        #endregion

        #region "Public Properties"
        [PrimaryKey()]
        public int IdTesoreriaMovimiento
        {
            get { return _idTesoreriaMovimiento; }
            set { _idTesoreriaMovimiento = value; }
        }

        public TGETiposValores TipoValor
        {
            get { return _tipoValor == null ? (_tipoValor = new TGETiposValores()) : _tipoValor; }
            set { _tipoValor = value; }
        }

        public TESTesoreriasMonedas TesoreriaMoneda
        {
            get { return _tesoreriaMoneda == null ? (_tesoreriaMoneda = new TESTesoreriasMonedas()) : _tesoreriaMoneda; }
            set { _tesoreriaMoneda = value; }
        }

        public TGETiposOperaciones TipoOperacion
        {
            get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
            set { _tipoOperacion = value; }
        }

        public int IdRefTipoOperacion
        {
            get { return _idRefTipoOperacion; }
            set { _idRefTipoOperacion = value; }
        }

        public DateTime Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }

        public string Descripcion
        {
            get { return _descripcion == null ? string.Empty : _descripcion; }
            set { _descripcion = value; }
        }
        public string Transaccion
        {
            get { return _transaccion == null ? string.Empty : _transaccion; }
            set { _transaccion = value; }
        }

        public decimal Importe
        {
            get { return _importe; }
            set { _importe = value; }
        }

        public decimal ImporteSigno { get; set; }

        [Auditoria()]
        public decimal MonedaCotizacion { get; set; }

        public TESCajas Caja
        {
            get { return _caja == null ? (_caja = new TESCajas()) : _caja; }
            set { _caja = value; }
        }
        public List<TGEArchivos> Archivos
        {
            get { return _archivos == null ? (_archivos = new List<TGEArchivos>()) : _archivos; }
            set { _archivos = value; }
        }
        /// <summary>
        /// SE GUARDARA LA INFORMACION DE LA REFERENCIA DEL IDREFTIPOOPERACION (ESTO ES PARA CONSULTAR MOVIMIENTOS)
        /// </summary>
        public string RefTipoOperacion { get => _refTipoOperacion; set => _refTipoOperacion = value; }
        /// <summary>
        /// PROPIEDAD AUXILIAR, PARA MAPEO EN CONSULTA DE Modulos/Tesoreria/TesoreriasMovimientos
        /// </summary>
        public int? IdBanco { get => _idBanco; set => _idBanco = value; }
        /// <summary>
        /// PROPIEDAD AUXILIAR, PARA MAPEO EN CONSULTA DE Modulos/Tesoreria/TesoreriasMovimientos
        /// </summary>
        public string Banco { get => _banco; set => _banco = value; }

        #endregion
    }
}
