using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haberes.Entidades
{
    public enum EstadosRemesas
    {
        Baja=0,
        Activo=1,
        Cerrada=27,
    }

    public enum EstadosRemesasDetalles
    {
        Baja=0,
        Activo=1,
        Depositado=43,
        SinValidar=49,
    }

    public enum EstadosRemesasAfiliados
    {
        NoDefinido=48,
    }
}
