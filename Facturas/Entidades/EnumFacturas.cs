using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facturas.Entidades
{
    public enum EnumConceptosComprobantes
    {
        Productos=1,
        Servicios=2,
        ProductosYServicios=3,
    }

    public enum EnumAFIPDocumentos
    {
        CUIT=80,
        CUIL=86,
    }
}
