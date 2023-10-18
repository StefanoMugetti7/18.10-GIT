using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Ahorros.Entidades
{
    [Serializable]
    public partial class AhoCuentasTipos : Objeto
    {
        #region "Private Members"
        int _idCuentaTipo;
        string _codigoBNRA;
        string _cuentaTipo;
        string _descripcion;
        int? _idCuentaContable;
        XmlDocumentSerializationWrapper _loteDetalles;
        string _cuentaContable;
        List<AhoCuentasTiposCuentasContables> _cuentasContables;
        #endregion

        #region "Constructors"
        public AhoCuentasTipos()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdCuentaTipo
        {
            get { return _idCuentaTipo; }
            set { _idCuentaTipo = value; }
        }
        [Auditoria]
        public string CodigoBNRA
        {
            get { return _codigoBNRA == null ? string.Empty : _codigoBNRA; }
            set { _codigoBNRA = value; }
        }
        [Auditoria]
        public string CuentaTipo
        {
            get { return _cuentaTipo == null ? string.Empty : _cuentaTipo; }
            set { _cuentaTipo = value; }
        }

        public string Descripcion
        {
            get { return _descripcion == null ? string.Empty : _descripcion; }
            set { _descripcion = value; }
        }
        public XmlDocument LoteDetalles
        {
            get
            {
                return _loteDetalles;
            }
            set
            {
                _loteDetalles = value;
            }
        }
        [Auditoria]
        public int? IdCuentaContable { get => _idCuentaContable; set => _idCuentaContable = value; }
        public string CuentaContable { get => _cuentaContable; set => _cuentaContable = value; }
        public List<AhoCuentasTiposCuentasContables> CuentasContables
        {
            get { return _cuentasContables == null ? (_cuentasContables = new List<AhoCuentasTiposCuentasContables>()) : _cuentasContables; }
            set { _cuentasContables = value; }
        }

        #endregion
    }
    [Serializable]
    public partial class AhoCuentasTiposCuentasContables : Objeto
    {
        int _idCuentaTipoCuentaContable;
        int _idCuentaTipo;
        int _idCuentaContable;
        string _cuentaContableDescripcion;
        TGEMonedas _moneda;

        public int IdCuentaTipoCuentaContable { get => _idCuentaTipoCuentaContable; set => _idCuentaTipoCuentaContable = value; }
        public int IdCuentaTipo { get => _idCuentaTipo; set => _idCuentaTipo = value; }
        public int IdCuentaContable { get => _idCuentaContable; set => _idCuentaContable = value; }
        public TGEMonedas Moneda
        {
            get { return _moneda == null ? (_moneda = new TGEMonedas()) : _moneda; }
            set { _moneda = value; }
        }

        public string CuentaContableDescripcion { get => _cuentaContableDescripcion; set => _cuentaContableDescripcion = value; }
    }
}
