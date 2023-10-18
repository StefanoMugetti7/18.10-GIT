using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ahorros.Entidades
{
    public enum EstadosAhorrosCuentas
    {
        CuentaAbierta=8,
        CuentaCerrada=9,
        CuentaBloqueada=10,
        CuentaInmovilizada=11,
    }

    public enum EstadosAhorrosCuentasMovimientos
    {
        Confirmado=12,
        PendienteConfirmacion=13,
        Baja=0,
        PendienteAcreditacionBancos=65,
        //PendienteAcreditacion=14,
        Rechazado=15
    }

    public enum EstadosPlazosFijos
    {
        Baja = 0,
        Confirmado = 12,
        Cancelado=16,
        Pagado=17,
        PendienteConfirmacion = 13,
        PendientePago=31,
        PendienteCancelacion = 33,
        Finalizado=46,
        RenovacionPendienteConfirmacion = 47,
    }
}
