using Bancos.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compras.Entidades
{
    [Serializable]
    public partial class CapOrdenesComprasValores : Objeto
    {
        // Class CapOrdenesPagoValores
        #region "Private Members"
        int _idOrdenCompraValor;
        int _idOrdenCompra;
        TGETiposValores _tipoValor;
        decimal _importe;
        decimal _importeCuota;
        TESBancosCuentas _bancoCuenta;
        string _numeroCheque;
        int _cantidadCuotas;
        DateTime? _fecha;
        DateTime? _fechaDiferido;
        bool _chequePropio;
        TESCheques _cheque;
        TGEListasValoresSistemasDetalles _listaValorSistemaDetalle;
        #endregion

        #region "Constructors"
        public CapOrdenesComprasValores()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdOrdenCompraValor
        {
            get { return _idOrdenCompraValor; }
            set { _idOrdenCompraValor = value; }
        }
        public int IdOrdenCompra
        {
            get { return _idOrdenCompra; }
            set { _idOrdenCompra = value; }
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

        [Auditoria()]
        public string NumeroCheque
        {
            get { return _numeroCheque == null ? string.Empty : _numeroCheque; }
            set { _numeroCheque = value; }
        }
        [Auditoria()]
        public int CantidadCuotas
        {
            get { return _cantidadCuotas; }
            set { _cantidadCuotas = value; }
        }
        public decimal ImporteCuota
        {
            get { return _importeCuota; }
            set { _importeCuota = value; }
        }


        public DateTime? Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }

        [Auditoria()]
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

        public TGEListasValoresSistemasDetalles ListaValorSistemaDetalle
        {
            get { return _listaValorSistemaDetalle == null ? (_listaValorSistemaDetalle = new TGEListasValoresSistemasDetalles()) : _listaValorSistemaDetalle; }
            set { _listaValorSistemaDetalle = value; }
        }

        public string CBU { get; set; }
        public string DatosCBU { get; set; }

        #endregion
    }
}
