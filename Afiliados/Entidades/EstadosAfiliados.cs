using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Afiliados.Entidades
{
    public enum EstadosAfiliados
    {
        /*
        Normal=0,
        Renuncia=1,
        Fallecido=2,
        Expulsado=3,
        Suspendido=4,
        Vitalicio=6,
        Baja=7,
        */
        Normal = 2,
        Renuncia = 3,
        Fallecido = 4,
        Expulsado = 5,
        Suspendido = 6,
        //Vitalicio = 7,
        Baja = 0,
        Moroso=29,
        AvisodeFallecido=30,
    }
}
