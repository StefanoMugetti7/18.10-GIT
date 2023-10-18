using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Haberes.Entidades;
using Servicio.AccesoDatos;
using Generales.Entidades;

namespace Haberes.LogicaNegocio
{
    class HabArchivosCabecerasLN
    {
        public List<HabArchivosDetalles> ObtenerDatosCompletos(HabArchivosDetalles pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<HabArchivosDetalles>("HabArchivosDetallesSeleccionarFiltro", pParametro);
        }
    }
}
