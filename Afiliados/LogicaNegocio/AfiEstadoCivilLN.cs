using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Afiliados.Entidades;
using Servicio.AccesoDatos;

namespace Afiliados.LogicaNegocio
{
    class AfiEstadoCivilLN
    {
        public List<AfiEstadoCivil> ObtenerLista()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiEstadoCivil>("AfiEstadoCivilListar");
        }
    }
}
