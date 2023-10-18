using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generales.Entidades;

namespace Contabilidad.Entidades
{
    public class InterfazValoresImportes
    {
        int _idTipoValor;
        decimal _importe;
        int _idBancoCuenta;
        int _idCheque;
        int _periodo;
        string _detalle;
        CtbCuentasContables _cuentaContable;

        public int IdTipoValor
        {
            get { return _idTipoValor; }
            set { _idTipoValor = value; }
        }

        public decimal Importe
        {
            get { return _importe; }
            set { _importe=value;}
        }

        public int IdBancoCuenta
        {
            get { return _idBancoCuenta; }
            set { _idBancoCuenta=value;}
        }

        public int IdCheque
        {
            get { return _idCheque; }
            set { _idCheque = value; }
        }

        public int Periodo
        {
            get { return _periodo; }
            set { _periodo = value; }
        }

        public string Detalle
        {
            get { return _detalle == null ? string.Empty : _detalle; }
            set { _detalle = value; }
        }

        public CtbCuentasContables CuentaContable
        {
            get { return _cuentaContable == null ? (_cuentaContable = new CtbCuentasContables()) : _cuentaContable; }
            set { _cuentaContable = value; }
        }
    }
}
