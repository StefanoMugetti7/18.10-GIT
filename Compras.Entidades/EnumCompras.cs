using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compras.Entidades
{
    public enum EnumTiposOrdenesCompras
    {
        Bienes = 272,
        Servicios = 273,
        Terceros = 278,
    }

    public enum EnumProductos
    { 
        VariosGenerico=1,
    }

    public enum EnumTiposProductos
    {
        Compras,
        Ventas,
        ComprasYVentas,
    }
}
