
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace Contabilidad.Entidades
{
    [Serializable]
    public partial class CtbFilialesCuentasContables : Objeto
    {
        // Class CtbFilialesCuentasContables
        #region "Private Members"
        TGEFiliales _filial;
        TGEMonedas _moneda;
        TGEModulosSistema _moduloSistema;
        TGETiposValores _tipoValor;
        CtbCuentasContables _cuentaContable;
        #endregion

        #region "Constructors"
        public CtbFilialesCuentasContables()
        {
        }
        #endregion

        #region "Public Properties"
        public TGEFiliales Filial
        {
            get { return _filial==null? (_filial=new TGEFiliales()):_filial; }
            set { _filial = value; }
        }

        public TGEMonedas Moneda
        {
            get { return _moneda==null?(_moneda=new TGEMonedas()):_moneda; }
            set { _moneda = value; }
        }

        public TGEModulosSistema ModuloSistema
        {
            get { return _moduloSistema==null?(_moduloSistema=new TGEModulosSistema()):_moduloSistema; }
            set { _moduloSistema = value; }
        }

        public TGETiposValores TipoValor
        {
            get { return _tipoValor==null?(_tipoValor=new TGETiposValores()):_tipoValor; }
            set { _tipoValor = value; }
        }

        public CtbCuentasContables CuentaContable
        {
            get { return _cuentaContable==null?(_cuentaContable=new CtbCuentasContables()):_cuentaContable; }
            set { _cuentaContable = value; }
        }

        #endregion
    }
}
