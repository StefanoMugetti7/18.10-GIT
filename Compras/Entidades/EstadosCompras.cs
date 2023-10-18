using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compras.Entidades
{
    public enum EstadosSolicitudesCompras
    {
        Baja = 0,
        Activo = 1,
        Autorizado = 28,
        EnOrdenPago = 58,
        EnOrdenPagoParcial = 59,
        PagadoParcial = 60,
        EnOrdenCompra = 70,
        Cotizado = 71,

    }

    public enum EstadosOrdenesCompras
    {
        Baja = 0,
        Activo = 1,
        Autorizado = 28,
        Pagado = 17,
        PagadoParcial = 60,
        Confirmado = 12,

    }

    public enum EstadosCotizaciones
    {
        Baja = 0,
        Activo = 1,
        Autorizado = 28,
    }
}
