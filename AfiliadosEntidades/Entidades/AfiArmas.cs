using Comunes.Entidades;
using System;

namespace Afiliados.Entidades
{
    [Serializable]
    public partial class AfiArmas : TGEListasValoresDetalles
    {
        [PrimaryKey]
        public int IdArma { get; set; }

    }
}
