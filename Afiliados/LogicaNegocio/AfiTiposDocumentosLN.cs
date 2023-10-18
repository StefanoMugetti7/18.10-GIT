using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Afiliados.Entidades;
using Servicio.AccesoDatos;

namespace Afiliados.LogicaNegocio
{
    class AfiTiposDocumentosLN 
    {
        public List<AfiTiposDocumentos> ObtenerLista()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiTiposDocumentos>("AfiTiposDocumentosListar");
        }
    }
}
