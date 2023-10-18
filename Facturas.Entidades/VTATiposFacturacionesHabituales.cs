using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Facturas.Entidades
{
    [Serializable]
    public class VTATiposFacturacionesHabituales : TGEListasValoresDetalles
    {
        int _idTipoFacturacionHabitual;

        [PrimaryKey]
        public int IdTipoFacturacionHabitual
        {
            get { return _idTipoFacturacionHabitual; }
            set { _idTipoFacturacionHabitual = value; }
        }
    }
}
