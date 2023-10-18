using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contabilidad.Entidades
{
    [Serializable]
    public class CtbCuentasContablesCostoMercaderiaVendida : CtbCuentasContables
    {
        #region "Private Members"
        int? _idCuentaContableCostoMercaderiaVendida;

        #endregion

        #region "Constructors"
        public CtbCuentasContablesCostoMercaderiaVendida()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        [Auditoria()]
        public int? IdCuentaContableCostoMercaderiaVendida
        {
            get { return _idCuentaContableCostoMercaderiaVendida; }
            set { _idCuentaContableCostoMercaderiaVendida = value; }
        }
        #endregion
}
}
