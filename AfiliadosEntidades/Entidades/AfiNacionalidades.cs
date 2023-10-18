using Comunes.Entidades;
using System;

namespace Afiliados.Entidades
{
    [Serializable]
    public class AfiNacionalidades : TGEListasValoresDetalles
    {
        [PrimaryKey]
        public int? IdNacionalidad { get; set; }
        public string Nacionalidad { get; set; }
    }
}
