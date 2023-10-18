using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Compras.Entidades
{
    [Serializable]
    public class CmpOrdenesComprasDetallesSolicitudPagoDetalle : Objeto
    {
        int _idOrdenCompraDetalleSolicitudPagoDetalle;
        int _idOrdenCompraDetalle;
        int _idSolicitudPagoDetalle;

        [PrimaryKey()]
        public int IdOrdenCompraDetalleSolicitudPagoDetalle
        {
            get { return _idOrdenCompraDetalleSolicitudPagoDetalle; }
            set { _idOrdenCompraDetalleSolicitudPagoDetalle = value; }
        }

        public int IdOrdenCompraDetalle
        {
            get { return _idOrdenCompraDetalle; }
            set { _idOrdenCompraDetalle = value; }
        }

        public int IdSolicitudPagoDetalle
        {
            get { return _idSolicitudPagoDetalle; }
            set { _idSolicitudPagoDetalle = value; }
        }
    }
}
