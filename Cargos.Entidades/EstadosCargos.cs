using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cargos.Entidades
{
    public enum EstadosCargos
    {
        Baja=0,
        Activo=1,
        PeriodicidadMensual=53,
        Facturado=63,
        Facturandose=64,
        Pendiente=38,
        PeriodicidadAnual=114,
    }

    public enum EstadosCuentasCorrientes
    {
        Activo=1,
        Rechazado=15,
        Pendiente=38,
        EnviadoAlCobro = 34,
        Cobrado = 35,
        CobroDevuelto =36,
        CobroParcial = 37,
    }
}
