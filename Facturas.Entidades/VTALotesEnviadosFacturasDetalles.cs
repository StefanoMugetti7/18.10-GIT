using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Facturas.Entidades
{
    [Serializable]
    public class VTALotesEnviadosFacturasDetalles : VTAFacturasDetalles
    {
        int _idFacturaLoteEnviadoFacturaDetalle;
        string _tabla;
        Int64? _idRefTabla;

        [PrimaryKey()]
        public int IdFacturaLoteEnviadoFacturaDetalle
        {
            get { return _idFacturaLoteEnviadoFacturaDetalle; }
            set { _idFacturaLoteEnviadoFacturaDetalle = value; }
        }

        public string Tabla
        {
            get { return _tabla == null ? string.Empty : _tabla; }
            set { _tabla = value; }
        }

        public Int64? IdRefTabla
        {
            get { return _idRefTabla; }
            set { _idRefTabla = value; }
        }
    }
}
