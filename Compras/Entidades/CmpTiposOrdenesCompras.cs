﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Compras.Entidades
{
    public class CmpTiposOrdenesCompras : TGEListasValoresDetalles
    {
        [PrimaryKey]
        public int IdTipoOrdenCompra { get; set; }
    }
}
