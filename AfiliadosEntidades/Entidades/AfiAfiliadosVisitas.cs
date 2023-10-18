using Comunes.Entidades;
using System;

namespace Afiliados.Entidades.Entidades
{
    [Serializable]
    public class AfiAfiliadosVisitas : AfiAfiliados
    {
        [PrimaryKey]
        public int IdAfiliadoVisita { get; set; }
    }
}
