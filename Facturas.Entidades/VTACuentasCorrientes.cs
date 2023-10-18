
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Facturas.Entidades
{
    [Serializable]
    public partial class VTACuentasCorrientes : Objeto
    {
        // Class VTACuentasCorrientes
        #region "Private Members"
        int _idCuentaCorriente;
        int _idAfiliado;
        DateTime _fechaMovimiento;
        TGETiposOperaciones _tipoOperacion;
        int _idRefTipoOperacion;
        string _concepto;
        TGETiposMovimientos _tipoMovimiento;
        decimal _importe;
        decimal _importeCredito;
        decimal _importeDebito;
        decimal _saldoActual;
        int? _idRefCuentaCorriente;
        
        #endregion

        #region "Constructors"
        public VTACuentasCorrientes()
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

        public DateTime FechaMovimiento
        {
            get { return _fechaMovimiento; }
            set { _fechaMovimiento = value; }
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

        public string Concepto
        {
            get { return _concepto == null ? string.Empty : _concepto; }
            set { _concepto = value; }
        }

        public TGETiposMovimientos TipoMovimiento
        {
            get { return _tipoMovimiento == null ? (_tipoMovimiento = new TGETiposMovimientos()) : _tipoMovimiento; }
            set { _tipoMovimiento = value; }
        }

        public decimal Importe
        {
            get { return _importe; }
            set { _importe = value; }
        }

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

        public decimal SaldoActual
        {
            get { return _saldoActual; }
            set { _saldoActual = value; }
        }

        public int? IdRefCuentaCorriente
        {
            get { return _idRefCuentaCorriente; }
            set { _idRefCuentaCorriente = value; }
        }

        #endregion
    }
}
