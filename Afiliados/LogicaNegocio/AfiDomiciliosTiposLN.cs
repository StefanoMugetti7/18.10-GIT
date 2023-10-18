using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Afiliados.Entidades;
using Servicio.AccesoDatos;

namespace Afiliados.LogicaNegocio
{
    class AfiDomiciliosTiposLN
    {
        /// <summary>
        /// Devuelve una lista de Tipos Domicilios
        /// </summary>
        /// <param name="pAfiliado"></param>
        /// <returns></returns>
        public List<AfiDomiciliosTipos> ObtenerLista()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiDomiciliosTipos>("AfiDomiciliosTiposListar");
        }
    }
}
