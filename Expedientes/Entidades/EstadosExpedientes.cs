using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Expedientes.Entidades
{
    public enum EstadosExpedientes
    {
        Activo=1,
        Baja=0,
        Cerrado=52,
    }

    public enum EstadosExpedientesTracking
    {
        Activo=1,
        //Baja=0,
        Derivado=50,
        DerivacionRechazada=51,
    }
}
