using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generales.Entidades;
using Comunes.Entidades;

namespace Afiliados.Entidades
{
    public partial class AfiArmas : TGEListasValoresDetalles
    {
        [PrimaryKey]
        public int IdArma { get; set; }

    }
}
