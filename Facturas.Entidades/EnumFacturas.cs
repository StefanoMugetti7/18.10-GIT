using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facturas.Entidades
{
    public enum EnumConceptosComprobantes
    {
        Productos=289,
        Servicios=290,
        ProductosYServicios=291,
    }

    public enum EnumAFIPDocumentos
    {
        CUIT=80,
        CUIL=86,
    }

    public enum EnumAFIPTiposPuntosVentas
    {
        ComprobanteManual=292,
        ComprobanteEnLinea=293,
        WebServiceFacturaElectronica=294,
    }

    public enum EnumTiposFacturasLotes
    {
        Cargos=464,
        Convenios=466,
        PrestamosInteres=465,
        PrestamosCuotas = 467,
        InteresCuotasPrestamos=475,
        DesdeArchivo=476,
    }
}
