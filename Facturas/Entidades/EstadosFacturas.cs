using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facturas.Entidades
{
    public enum EstadosFacturas
    {
        Activo,
        ValidadaAfip,
        CobradaParcial,
        Cobrada,
    }

    public enum EstadosFacturasDetalles
    {
        Activo,
    }

}
