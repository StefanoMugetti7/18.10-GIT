using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bancos.Entidades
{
    public enum EstadosCheques
    {
        EnCaja=39,
        EnTesoreria=56,
        EnviadoSectorBancos=40,
        EnSectorBancos=41,
        EnviadoDepositar=42,
        Depositado=43,
        Rechazado=15,
        Entregado=69,
    }

    public enum EstadosBancosCuentasMovimientos
    {
        PendienteConciliacion=44,
        Confirmado=12,
        Rechazado = 15,
        Baja = 0,
        PendienteConfirmacion=13,
    }
}
