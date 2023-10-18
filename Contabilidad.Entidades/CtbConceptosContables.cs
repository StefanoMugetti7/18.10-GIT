using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Contabilidad.Entidades
{
    [Serializable]
    public class CtbConceptosContables : Objeto
    {
        int? _idConceptoContable;
        string _conceptoContable;
        //int _idCuentaContable;
        CtbCuentasContables _cuentaContable;
        List<TGETiposOperaciones> _tiposOperaciones;

        [PrimaryKey()]
        public int? IdConceptoContable
        {
            get { return _idConceptoContable; }
            set { _idConceptoContable = value; }
        }

        [Auditoria()]
        public string ConceptoContable
        {
            get { return _conceptoContable == null ? string.Empty : _conceptoContable; }
            set { _conceptoContable = value; }
        }

        //public int IdCuentaContable
        //{
        //    get { return _idCuentaContable; }
        //    set { _idCuentaContable = value; }
        //}

        public CtbCuentasContables CuentaContable
        {
            get { return _cuentaContable == null ? (_cuentaContable = new CtbCuentasContables()) : _cuentaContable; }
            set { _cuentaContable = value; }
        }

        public List<TGETiposOperaciones> TiposOperaciones
        {
            get { return _tiposOperaciones == null ? (_tiposOperaciones = new List<TGETiposOperaciones>()) : _tiposOperaciones; }
            set { _tiposOperaciones = value; }
        }
    }
}
