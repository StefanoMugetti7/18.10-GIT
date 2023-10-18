using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hoteles.Entidades
{
    public enum EstadosReservas
    {
        Baja = 0,
        Confirmado = 12,
        PendienteConfirmacion = 13,
        Finalizada = 20,
    }

    public enum EstadosDescuentos
    {
        Activo = 1,
        Baja = 0,
    }
}
