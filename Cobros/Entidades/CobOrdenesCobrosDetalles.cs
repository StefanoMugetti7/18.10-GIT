
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Contabilidad.Entidades;
using Cargos.Entidades;
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
        //CtbCuentasContables _cuentasContables;
        CarCuentasCorrientes _cuentaCorriente;

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

        //public CtbCuentasContables CuentaContable
        //{
        //    get { return _cuentasContables == null ? (_cuentasContables = new CtbCuentasContables()) : _cuentasContables; }
        //    set { _cuentasContables = value; }
        //}

        public CarCuentasCorrientes CuentaCorriente
        {
            get { return _cuentaCorriente == null ? (_cuentaCorriente = new CarCuentasCorrientes()) : _cuentaCorriente; }
            set { _cuentaCorriente = value; }
        }

        #endregion
    }
}
