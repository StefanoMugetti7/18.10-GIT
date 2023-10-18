using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Compras.Entidades
{
    [Serializable]
    public class CmpTipoStockMovimiento : TGEListasValoresDetalles
    {
        [PrimaryKey]
        public int IdTipoStockMovimiento { get; set; }
    }
}
