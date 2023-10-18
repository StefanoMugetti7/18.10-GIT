using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Contabilidad.Entidades
{
    [Serializable]
    public partial class TGEListasValoresSistemasDetallesCuentasContables : Objeto
    {
        #region Private Members
        int _idListaValorSistemaDetalleCuentaContable;
        TGEListasValoresSistemasDetalles _listaValorSistemaDetalle;
        CtbCuentasContables _cuentaContable;
        int _idUsuarioEvento;
        #endregion

        #region Constructors
        public TGEListasValoresSistemasDetallesCuentasContables()
        {
        }
        #endregion

        #region Public Properties
        [PrimaryKey()]
        public int IdListaValorSistemaDetalleCuentaContable
        {
            get { return _idListaValorSistemaDetalleCuentaContable; }
            set { _idListaValorSistemaDetalleCuentaContable = value; }
        }
        [Auditoria()]
        public TGEListasValoresSistemasDetalles ListaValorSistemaDetalle
        {
            get { return _listaValorSistemaDetalle == null ? (_listaValorSistemaDetalle = new TGEListasValoresSistemasDetalles()) : _listaValorSistemaDetalle; }
            set { _listaValorSistemaDetalle = value; }
        }
        [Auditoria()]
        public CtbCuentasContables CuentaContable
        {
            get { return _cuentaContable == null ? (_cuentaContable = new CtbCuentasContables()) : _cuentaContable; }
            set { _cuentaContable = value; }
        }
        [Auditoria()]
        public int IdUsuarioEvento
        {
            get { return _idUsuarioEvento; }
            set { _idUsuarioEvento = value; }
        }

        #endregion
    }
}
