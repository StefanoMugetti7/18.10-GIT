using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Afiliados.Entidades;
using Servicio.AccesoDatos;
using Comunes.Entidades;

namespace Afiliados.LogicaNegocio
{
    class AfiParentescoLN
    {
        public List<AfiParentesco> ObtenerListaActiva()
        {
            AfiParentesco filtro = new AfiParentesco();
            filtro.Estado.IdEstado = (int)Estados.Activo;
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiParentesco>("AfiParentescoSeleccionarFiltro", filtro);
        }
    }
}
