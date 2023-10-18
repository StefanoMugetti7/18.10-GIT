using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Afiliados.Entidades;
using Servicio.AccesoDatos;

namespace Afiliados.LogicaNegocio
{
    class AfiEmpresasTelefonicasLN
    {
        /// <summary>
        /// Devuelve una lista de Tipos Telefonos
        /// </summary>
        /// <param name="pAfiliado"></param>
        /// <returns></returns>
        public List<AfiEmpresasTelefonicas> ObtenerLista()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiEmpresasTelefonicas>("AfiEmpresasTelefonicasListar");
        }
    }
}
