using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesorerias.Entidades
{
    public enum EstadosTesorerias
    {
        Abierta=26,
        Cerrada=27,
    }

    public enum EstadosTesoreriasMonedas
    {
        Baja = 0,
        Activo = 1,
    }

    public enum EstadosTesoreriasMovimientos
    {
        Baja =0,
        Activo = 1,
        PendienteConfirmacion=13,
    }

    public enum EstadosCajas
    {
        Todas = -1,
        Abierta = 26,
        Cerrada = 27,
    }

    public enum EstadosCajasMonedas
    {
        Baja = 0,
        Activo = 1,
    }

    public enum EstadosCajasMovimientos
    {
        Baja = 0,
        Activo = 1,
    }

}
