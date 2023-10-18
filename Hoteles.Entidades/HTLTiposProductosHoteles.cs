using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hoteles.Entidades
{
    [Serializable]
    public class HTLTiposProductosHoteles : TGEListasValoresSistemasDetalles
    {
        [PrimaryKey]
        public int IdTipoProductoHotel { get; set; }
    }
}
