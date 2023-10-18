using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Facturas.Entidades
{
    [Serializable]
    public class VTAFacturasLotesEnviadosFacturas : VTAFacturas
    {
        int _idFacturaLoteEnviadoFactura;
        int _idFacturaLoteEnviado;

        [PrimaryKey()]
        public int IdFacturaLoteEnviadoFactura
        {
            get { return _idFacturaLoteEnviadoFactura; }
            set { _idFacturaLoteEnviadoFactura = value; }
        }

        public int IdFacturaLoteEnviado
        {
            get { return _idFacturaLoteEnviado; }
            set { _idFacturaLoteEnviado = value; }
        }
    }
}
