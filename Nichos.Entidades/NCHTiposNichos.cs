using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nichos.Entidades
{
    [Serializable]
    public class NCHTiposNichos : TGEListasValoresDetalles
    {
        public int IdTipoNicho { get; set; }
        public string TipoNicho { get; set; }
    }
}
