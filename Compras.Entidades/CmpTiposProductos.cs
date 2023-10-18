using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Compras.Entidades
{
    [Serializable]
    public class CmpTiposProductos : TGEListasValoresDetalles
    {
        [PrimaryKey]
        [Auditoria()]
        public int? IdTipoProducto { get; set; }
    }
}
