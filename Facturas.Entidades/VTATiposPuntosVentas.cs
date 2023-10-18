using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Facturas.Entidades
{
    [Serializable]
    public class VTATiposPuntosVentas : TGEListasValoresSistemasDetalles
    {
        int? _idTipoPuntoVenta;

        [PrimaryKey]
        public int? IdTipoPuntoVenta
        {
            get { return _idTipoPuntoVenta; }
            set { _idTipoPuntoVenta = value; }
        }
    }
}
