using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facturas.Entidades
{
    public enum EstadosFacturas
    {
        Baja=0,
        Activo=1,
        FESinValidadaAfip=76,
        CobradaParcial=37,
        Cobrada=35,
        Imputado = 80,
        ImputadoParcial = 81,
    }

    public enum EstadosFacturasDetalles
    {
        Activo=1,
    }

    public enum EstadosRemitos
    {
        Baja = 0,
        //Activo = 1,
        //Autorizado = 28,
        PendienteEntrega=86,
        EnDistribucion=87,
        Entregado=88,
        EnDespacho=89,
    }

    public enum EstadosNotasPedidos
    {
        Baja = 0,
        //Activo = 1,
        //Autorizado = 28,
        PendienteEntrega = 86,
        EnDistribucion = 87,
        Entregado = 88,
        EnDespacho = 89,
    }
}
