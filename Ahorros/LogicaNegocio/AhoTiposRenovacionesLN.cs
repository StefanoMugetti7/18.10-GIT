using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ahorros.Entidades;
using Servicio.AccesoDatos;

namespace Ahorros.LogicaNegocio
{
    class AhoTiposRenovacionesLN
    {
        public List<AhoTiposRenovaciones> ObtenerLista(AhoTiposRenovaciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AhoTiposRenovaciones>("AhoTiposRenovacionesListar", pParametro);
        }
    }
}
