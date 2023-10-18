
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
using Haberes.Entidades;
using System.Xml;
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
        decimal _importeCobradoOriginal;
        bool _importeCobradoModificado;
        decimal _importeEnviar;
        decimal _importeCredito;
        decimal _importeDebito;
        decimal _saldoActual;
        decimal _importeContabilizar;
        int _idRefCuentaCorriente;
        int? _idReferenciaRegistro;
        bool _incluir;
        TGEMonedas _moneda;
        string _tipoCargoConcepto;
        TGETiposOperaciones _tipoOperacion;
        CarTiposCargos _tipoCargo;
        
        TGETiposValores _tipoValor;
        
        TGEFormasCobros _formaCobro;

        string _motivoRechazo;
        TGEFiliales _filial;
        bool _cargosACobrarTerceros;
        HabRecibosComPagos _reciboComPago;
        XmlDocumentSerializationWrapper _loteCuentasCorrientes;
        int _idCuenta;
        private List<TGECampos> _campos;
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

        public TGEMonedas Moneda
        {
            get { return _moneda == null ? (_moneda = new TGEMonedas()) : _moneda; }
            set { _moneda = value; }
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
            set {
                if (!this._importeCobradoModificado)
                {
                    this.ImporteCobradoOriginal = value;
                    this._importeCobradoModificado = true;
                }
                _importeCobrado = value; 
            }
        }

        public decimal ImporteCobradoOriginal
        {
            get { return _importeCobradoOriginal; }
            set {
                if (!this._importeCobradoModificado)
                    _importeCobradoOriginal = value; 
            }
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

        /// <summary>
        /// Se utiliza en la cancelacion de prestamos para guardar el importe a contabilizar
        /// </summary>
        public decimal ImporteContabilizar
        {
            get { return _importeContabilizar; }
            set { _importeContabilizar = value; }
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


        public TGEFiliales Filial
        {
            get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
            set { _filial = value; }
        }

        public bool CargosACobrarTerceros
        {
            get { return _cargosACobrarTerceros; }
            set { _cargosACobrarTerceros = value; }
        }

        public HabRecibosComPagos ReciboComPago
        {
            get { return _reciboComPago == null ? (_reciboComPago = new HabRecibosComPagos()) : _reciboComPago; }
            set { _reciboComPago = value; }
        }

        public XmlDocument LoteCuentasCorrientes
        {
            get { return _loteCuentasCorrientes; }
            set { _loteCuentasCorrientes = value; }
        }

        public int IdCuenta
        {
            get { return _idCuenta; }
            set { _idCuenta = value; }
        }

        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }
        }

        public int? IdRefAfiliadoFormaCobro { get; set; }
        #endregion
    }
}
