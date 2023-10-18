
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
using Bancos.Entidades;
namespace CuentasPagar.Entidades
{
    [Serializable]
    public partial class CapOrdenesPagosValores : Objeto
    {
        // Class CapOrdenesPagoValores
        #region "Private Members"
        int _idOrdenPagoValor;
        int _idOrdenPago;
        TGETiposValores _tipoValor;
        decimal _importe;
        TESBancosCuentas _bancoCuenta;
        string _numeroCheque;
        DateTime? _fecha;
        DateTime? _fechaDiferido;
        bool _chequePropio;
        TESCheques _cheque;
        #endregion

        #region "Constructors"
        public CapOrdenesPagosValores()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdOrdenPagoValor
        {
            get { return _idOrdenPagoValor; }
            set { _idOrdenPagoValor = value; }
        }
        public int IdOrdenPago
        {
            get { return _idOrdenPago; }
            set { _idOrdenPago = value; }
        }

        public TGETiposValores TipoValor
        {
            get { return _tipoValor == null ? (_tipoValor = new TGETiposValores()) : _tipoValor; }
            set { _tipoValor = value; }
        }

        public decimal Importe
        {
            get { return _importe; }
            set { _importe = value; }
        }

        public TESBancosCuentas BancoCuenta
        {
            get { return _bancoCuenta == null ? (_bancoCuenta = new TESBancosCuentas()) : _bancoCuenta; }
            set { _bancoCuenta = value; }
        }

        public string NumeroCheque
        {
            get { return _numeroCheque == null ? string.Empty : _numeroCheque; }
            set { _numeroCheque = value; }
        }

        public DateTime? Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }

        public DateTime? FechaDiferido
        {
            get { return _fechaDiferido; }
            set { _fechaDiferido = value; }
        }

        public TESCheques Cheque
        {
            get { return _cheque == null ? (_cheque = new TESCheques()) : _cheque; }
            set { _cheque = value; }
        }

        public bool ChequePropio
        {
            get { return _chequePropio; }
            set { _chequePropio = value; }
        }

        #endregion
    }
}
