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
        Recibido = 72,
        ParcialmenteRecibido = 73,

    }

    public enum EstadosCotizaciones
    {
        Baja = 0,
        Activo = 1,
        Autorizado = 28,
    }

    public enum EstadosInformesRecepciones
    {
        Baja = 0,
        Activo = 1,
        Recibido = 72,
        PatcialmenteRecibido = 73,
    }

    public enum EstadosListasPrecios
    {
        Baja = 0,
        Activo = 1,
    }

    public enum EstadosStock
    {
        Baja = 0,
        Activo = 1,
        Confirmado = 12,
        PendienteConfirmacion = 13,
    }
}
