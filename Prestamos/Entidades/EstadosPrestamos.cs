using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prestamos.Entidades
{
    public enum EstadosPrestamos
    {
        Activo = 1,
        Anulado = 19,
        Finalizado = 20,
        Cancelado = 16,
        Autorizado =28,
        Confirmado = 12,
        PreAutorizado = 32,
        PendienteCancelacion =33,
        RenovacionPendienteConfirmacion=47,
    }
    public enum EstadosCuotas
    {
        Baja=0,
        Activa = 22,
        Anulada = 23,
        Cobrada = 24,
        Cancelada = 25,
        PendienteCancelacion = 33,
    }

    public enum EstadosCesiones
    {
        Baja = 0,
        Activa = 1,
        Autorizado=28,
        Reservado=70,
    }
}