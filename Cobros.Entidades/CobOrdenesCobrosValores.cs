
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
using Bancos.Entidades;
using System.Xml;

namespace Cobros.Entidades
{
    [Serializable]
    public partial class CobOrdenesCobrosValores : Objeto
    {
        // Class CapOrdenesPagoValores
        #region "Private Members"
        int _idOrdenCobroValor;
        int _idOrdenCobro;
        TGETiposValores _tipoValor;
        decimal _importe;
        TESBancosCuentas _bancoCuenta;
        string _numeroCheque;
        DateTime? _fecha;
        DateTime? _fechaDiferido;
        TESCheques _cheque;
        int _idTarjetaCredito;
        string _numeroTarjetaCredito;
        string _titular;
        int _vencimientoMes;
        int _vencimientoAnio;
        string _numeroTransaccionPosnet;
        string _numeroLote;
        int _cantidadCuotas;
        string _detalle;
        TGEListasValoresSistemasDetalles _listaValorSistemaDetalle;
        List<TGECampos> _campos;
        XmlDocumentSerializationWrapper _loteCamposValores;
        #endregion

        #region "Constructors"
        public CobOrdenesCobrosValores()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdOrdenCobroValor
        {
            get { return _idOrdenCobroValor; }
            set { _idOrdenCobroValor = value; }
        }
        public int IdOrdenCobro
        {
            get { return _idOrdenCobro; }
            set { _idOrdenCobro = value; }
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

        public int IdTarjetaCredito
        {
            get { return _idTarjetaCredito; }
            set { _idTarjetaCredito = value; }
        }

        public string NumeroTarjetaCredito
        {
            get { return _numeroTarjetaCredito == null ? string.Empty : _numeroTarjetaCredito; }
            set { _numeroTarjetaCredito = value; }
        }

        public string Titular
        {
            get { return _titular == null ? string.Empty : _titular; }
            set { _titular = value; }
        }

        public int VencimientoMes
        {
            get { return _vencimientoMes; }
            set { _vencimientoMes = value; }
        }

        public int VencimientoAnio
        {
            get { return _vencimientoAnio; }
            set { _vencimientoAnio = value; }
        }

        public string NumeroTransaccionPosnet
        {
            get { return _numeroTransaccionPosnet == null ? string.Empty : _numeroTransaccionPosnet; }
            set { _numeroTransaccionPosnet = value; }
        }

        public string NumeroLote
        {
            get { return _numeroLote == null ? string.Empty :_numeroLote; }
            set { _numeroLote = value; }
        }

        public int CantidadCuotas
        {
            get { return _cantidadCuotas; }
            set { _cantidadCuotas = value; }
        }

        public string Detalle
        {
            get { return _detalle == null ? string.Empty : _detalle; }
            set { _detalle = value; }
        }

        public TGEListasValoresSistemasDetalles ListaValorSistemaDetalle
        {
            get { return _listaValorSistemaDetalle == null ? (_listaValorSistemaDetalle = new TGEListasValoresSistemasDetalles()) : _listaValorSistemaDetalle; }
            set { _listaValorSistemaDetalle = value; }
        }

        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }
        }

        public XmlDocument LoteCamposValores
        {
            get { return _loteCamposValores; }// == null ? (_loteCamposValores = new XmlDocumentSerializationWrapper()) : _loteCamposValores; }
            set { _loteCamposValores = value; }
        }

        public int? IdCuenta { get; set; }

        #endregion
    }
}