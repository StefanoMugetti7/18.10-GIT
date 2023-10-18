
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Contabilidad.Entidades;
using Cargos.Entidades;
using Facturas.Entidades;
using Bancos.Entidades;
namespace Cobros.Entidades
{
    [Serializable]
    public partial class CobOrdenesCobrosDetalles : Objeto
    {

        #region "Private Members"
        int _idOrdenCobroDetalle;
        int _idOrdenCobro;
        string _detalle;
        decimal _importe;
        CtbConceptosContables _conceptoContable;
        CarCuentasCorrientes _cuentaCorriente;
        VTAFacturas _factura;
        bool _incluirEnOP;
        bool _esAnticipo;
        int? _idPrestamoCuota;
        List<TESTarjetasTransacciones> _tarjetasTransacciones;
        int _idRefEstado;
        //decimal _importeParcialCobrado;
        //decimal _importeParcial;
        //string _numeroFactura;
        //DateTime? _fechaFactura;
        //int _idFactura;
        //decimal _importeTotal;

        #endregion

        #region "Constructors"
        public CobOrdenesCobrosDetalles()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdOrdenCobroDetalle
        {
            get { return _idOrdenCobroDetalle; }
            set { _idOrdenCobroDetalle = value; }
        }
        public int IdOrdenCobro
        {
            get { return _idOrdenCobro; }
            set { _idOrdenCobro = value; }
        }

        public string Detalle
        {
            get { return _detalle == null ? string.Empty : _detalle; }
            set { _detalle = value; }
        }

        public decimal Importe
        {
            get { return _importe; }
            set { _importe = value; }
        }

        public CtbConceptosContables ConceptoContable
        {
            get { return _conceptoContable == null ? (_conceptoContable = new CtbConceptosContables()) : _conceptoContable; }
            set { _conceptoContable = value; }
        }

        public CarCuentasCorrientes CuentaCorriente
        {
            get { return _cuentaCorriente == null ? (_cuentaCorriente = new CarCuentasCorrientes()) : _cuentaCorriente; }
            set { _cuentaCorriente = value; }
        }

        public VTAFacturas Factura
        {
            get { return _factura == null ? (_factura = new VTAFacturas()) : _factura; }
            set { _factura = value; }
        }

        public bool IncluirEnOP
        {
            get { return _incluirEnOP; }
            set { _incluirEnOP = value; }
        }

        public bool EsAnticipo
        {
            get { return _esAnticipo; }
            set { _esAnticipo = value; }
        }

        public List<TESTarjetasTransacciones> TarjetasTransacciones
        {
            get { return _tarjetasTransacciones == null ? (_tarjetasTransacciones = new List<TESTarjetasTransacciones>()) : _tarjetasTransacciones; }
            set { _tarjetasTransacciones = value; }
        }

        public int? IdPrestamoCuota
        {
            get { return _idPrestamoCuota; }
            set { _idPrestamoCuota = value; }
        }

        public int IdRefEstado
        {
            get { return _idRefEstado; }
            set { _idRefEstado = value; }
        }

        #endregion
    }
}
