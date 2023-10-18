
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace Cargos.Entidades
{
    [Serializable]
    public partial class CarCuentasCorrientes : Objeto
    {
        #region "Private Members"
        int _idCuentaCorriente;
        int _idAfiliado;
        int _periodo;
        DateTime _fechaMovimiento;
        int _idRefTipoOperacion;
        string _concepto;
        //TGETiposMovimientos _tipoMovimiento;
        decimal _importe;
        decimal _importeCobrado;
        decimal _importeEnviar;
        decimal _importeCredito;
        decimal _importeDebito;
        decimal _saldoActual;
        int _idRefCuentaCorriente;
        int? _idReferenciaRegistro;
        bool _incluir;
        string _tipoCargoConcepto;
        TGETiposOperaciones _tipoOperacion;
        CarTiposCargos _tipoCargo;
        
        TGETiposValores _tipoValor;
        
        TGEFormasCobros _formaCobro;

        string _motivoRechazo;
        #endregion

        #region "Constructors"
        public CarCuentasCorrientes()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdCuentaCorriente
        {
            get { return _idCuentaCorriente; }
            set { _idCuentaCorriente = value; }
        }

        public int IdAfiliado
        {
            get { return _idAfiliado; }
            set { _idAfiliado = value; }
        }

        public int Periodo
        {
            get { return _periodo; }
            set { _periodo = value; }
        }

        public DateTime FechaMovimiento
        {
            get { return _fechaMovimiento; }
            set { _fechaMovimiento = value; }
        }

        public int IdRefTipoOperacion
        {
            get { return _idRefTipoOperacion; }
            set { _idRefTipoOperacion = value; }
        }

        public string Concepto
        {
            get { return _concepto == null ? string.Empty : _concepto; }
            set { _concepto = value; }
        }

        //public TGETiposMovimientos TipoMovimiento
        //{
        //    get { return _tipoMovimiento == null ? (_tipoMovimiento = new TGETiposMovimientos()) : _tipoMovimiento; }
        //    set { _tipoMovimiento = value; }
        //}

        public decimal ImporteCredito
        {
            get { return _importeCredito; }
            set { _importeCredito = value; }
        }

        public decimal ImporteDebito
        {
            get { return _importeDebito; }
            set { _importeDebito = value; }
        }

        public decimal Importe
        {
            get { return _importe; }
            set { _importe = value; }
        }

        public decimal ImporteCobrado
        {
            get { return _importeCobrado; }
            set { _importeCobrado = value; }
        }

        public decimal ImporteEnviar
        {
            get { return _importeEnviar; }
            set { _importeEnviar = value; }
        }

        public decimal SaldoActual
        {
            get { return _saldoActual; }
            set { _saldoActual = value; }
        }
        [Auditoria()]
        public int IdRefCuentaCorriente
        {
            get { return _idRefCuentaCorriente; }
            set { _idRefCuentaCorriente = value; }
        }

        public int? IdReferenciaRegistro
        {
            get { return _idReferenciaRegistro; }
            set { _idReferenciaRegistro = value; }
        }

        public TGETiposValores TipoValor
        {
            get { return _tipoValor == null ? (_tipoValor = new TGETiposValores()) : _tipoValor; }
            set { _tipoValor = value; }
        }

        public bool Incluir
        {
            get { return _incluir; }
            set { _incluir = value; }
        }
        [Auditoria()]
        public TGEFormasCobros FormaCobro
        {
            get { return _formaCobro == null ? (_formaCobro = new TGEFormasCobros()) : _formaCobro; }
            set { _formaCobro = value; }
        }

        public TGETiposOperaciones TipoOperacion
        {
            get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
            set { _tipoOperacion = value; }
        }

        public CarTiposCargos TipoCargo
        {
            get { return _tipoCargo == null ? (_tipoCargo = new CarTiposCargos()) : _tipoCargo; }
            set { _tipoCargo = value; }
        }

        public string TipoCargoConcepto
        {
            get { return _tipoCargoConcepto == null ? string.Empty : _tipoCargoConcepto; }
            set { _tipoCargoConcepto = value; }
        }

        public string MotivoRechazo
        {
            get { return _motivoRechazo == null ? string.Empty : _motivoRechazo; }
            set { _motivoRechazo = value; }
        }

        #endregion
    }
}
