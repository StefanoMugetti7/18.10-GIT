using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CuentasPagar.Entidades
{
    public enum EstadosSolicitudesPagos
    {
        Baja=0,
        Activo=1,
        Autorizado=28,
        EnOrdenPago=58,
        EnOrdenPagoParcial=59,
        PagadoParcial=60,

    }

    public enum EstadosOrdenesPago
    {
        Baja=0,
        Activo=1,
        Autorizado=28,
        Pagado=17,
        //Confirmado=12,
    }
}
