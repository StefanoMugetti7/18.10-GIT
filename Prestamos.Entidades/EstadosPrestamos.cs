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
        CanceladoCuotasPendientes=78,
        AnuladoPostConfirmado = 94,
        Pendiente = 38,
    }
    public enum EstadosCuotas
    {
        Baja=0,
        Activa = 22,
        Anulada = 23,
        Cobrada = 24,
        Cancelada = 25,
        PendienteCancelacion = 33,
        CobradaParcial = 37,
    }

    public enum EstadosCesiones
    {
        Baja = 0,
        Activa = 1,
        Autorizado=28,
        Reservado=70,
    }

    public enum EstadosIPSCAD
    {
        Activo=1,
        Baja=0,
        Tomado=83,
    }

    public enum EstadosPrestamosLotes
    {
        Activo=1,
        Baja=0,
        PendienteConfirmacion=13,
        Confirmado=12,
    }
}