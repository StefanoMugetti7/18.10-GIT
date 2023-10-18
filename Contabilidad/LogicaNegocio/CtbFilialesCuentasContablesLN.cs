using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contabilidad.Entidades;
using Comunes;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Servicio.AccesoDatos;

namespace Contabilidad.LogicaNegocio
{
    internal class CtbFilialesCuentasContablesLN : BaseLN<CtbFilialesCuentasContables>
    {
        public override bool Agregar(CtbFilialesCuentasContables pParametro)
        {
            throw new NotImplementedException();
        }

        public override bool Modificar(CtbFilialesCuentasContables pParametro)
        {
            throw new NotImplementedException();
        }

        public override CtbFilialesCuentasContables ObtenerDatosCompletos(CtbFilialesCuentasContables pParametro)
        {
            throw new NotImplementedException();
        }

        public CtbFilialesCuentasContables ObtenerDatosCompletos(CtbFilialesCuentasContables pParametro, Database bd, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CtbFilialesCuentasContables>("CtbFilialesCuentasContablesSeleccionar", pParametro, bd, tran);
        }

        public override List<CtbFilialesCuentasContables> ObtenerListaFiltro(CtbFilialesCuentasContables pParametro)
        {
            throw new NotImplementedException();
        }
    }
}
