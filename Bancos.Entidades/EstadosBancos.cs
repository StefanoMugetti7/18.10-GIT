using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bancos.Entidades
{
    public enum EstadosCheques
    {
        Baja=0,
        Activo=1,
        EnCaja=39,
        EnTesoreria=56,
        EnviadoSectorBancos=40,
        EnSectorBancos=41,
        EnviadoDepositar=42,
        Depositado=43,
        Rechazado=15,
        Entregado=69,
        Traspaso=108,
    }

    public enum EstadosBancosCuentasMovimientos
    {
        PendienteConciliacion=44,
        Confirmado=12,
        Rechazado = 15,
        Baja = 0,
        PendienteConfirmacion=13,
        //Solo se usa para Filtrar
        Pendiente = 38
    }
    public enum EstadosBancosLotesEnviadosDetalles
    {
        Rechazado = 15,
        Conciliado = 45,
    }
}
