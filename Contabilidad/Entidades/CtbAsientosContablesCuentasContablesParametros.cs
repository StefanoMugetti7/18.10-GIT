
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace Contabilidad.Entidades
{
    [Serializable]
    public partial class CtbAsientosContablesCuentasContablesParametros : Objeto
    {
        // Class CtbAsientosContablesCuentasContablesParametros
        #region "Private Members"
        int _idAsientoContableCuentaContableParametro;
        TGEFiliales _filial;
        TGETiposValores _tipoValor;
        //CtbEntidadContable _entidadContable;
        //int? _idRefEntidadContable;
        TGEMonedas _moneda;
        int? _idTipoMovimiento;

        #region "_TESBancosCuentas"
        int? _bancoCuentaIdBancoCuenta;
        string _bancoCuentaNumeroCuenta;
        int? _bancoCuentaBancoIdBanco;
        string _bancoCuentaBancoDescripcion;
        #endregion

        CtbCuentasContables _cuentaContable;
        #endregion

        #region "Constructors"
        public CtbAsientosContablesCuentasContablesParametros()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey]
        public int IdAsientoContableCuentaContableParametro
        {
            get { return _idAsientoContableCuentaContableParametro; }
            set { _idAsientoContableCuentaContableParametro = value; }
        }

        [Auditoria]
        public TGEFiliales Filial
        {
            get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
            set { _filial = value; }
        }

        [Auditoria]
        public TGETiposValores TipoValor
        {
            get { return _tipoValor == null ? (_tipoValor = new TGETiposValores()) : _tipoValor; }
            set { _tipoValor = value; }
        }

        //[Auditoria]
        //public CtbEntidadContable EntidadContable
        //{
        //    get { return _entidadContable == null ? (_entidadContable = new CtbEntidadContable()) : _entidadContable; }
        //    set { _entidadContable = value; }
        //}

        //[Auditoria]
        //public int? IdRefEntidadContable
        //{
        //    get { return _idRefEntidadContable; }
        //    set { _idRefEntidadContable = value; }
        //}

        [Auditoria]
        public TGEMonedas Moneda
        {
            get { return _moneda == null ? (_moneda = new TGEMonedas()) : _moneda; }
            set { _moneda = value; }
        }

        [Auditoria]
        public int? IdTipoMovimiento
        {
            get { return _idTipoMovimiento; }
            set { _idTipoMovimiento = value; }
        }

        [Auditoria]
        public CtbCuentasContables CuentaContable
        {
            get { return _cuentaContable == null ? (_cuentaContable = new CtbCuentasContables()) : _cuentaContable; }
            set { _cuentaContable = value; }
        }

        #region "TESBancosCuentas"
        
        [Auditoria]
        public int? BancoCuentaIdBancoCuenta
        {
            get { return _bancoCuentaIdBancoCuenta; }
            set { _bancoCuentaIdBancoCuenta = value; }
        }

        [Auditoria]
        public int? BancoCuentaBancoIdBanco
        {
            get { return _bancoCuentaBancoIdBanco; }
            set { _bancoCuentaBancoIdBanco = value; }
        }

        [Auditoria]
        public string BancoCuentaNumeroCuenta
        {
            get { return _bancoCuentaNumeroCuenta== null? string.Empty : _bancoCuentaNumeroCuenta; }
            set { _bancoCuentaNumeroCuenta = value; }
        }

        [Auditoria]
        public string BancoCuentaBancoDescripcion
        {
            get { return _bancoCuentaBancoDescripcion == null ? string.Empty : _bancoCuentaBancoDescripcion; }
            set { _bancoCuentaBancoDescripcion = value; }
        }

        #endregion

        #endregion
    }
}
