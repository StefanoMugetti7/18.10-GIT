using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Compras.Entidades
{
    [Serializable]
    public partial class CmpOrdenesComprasDetallesSolicitudesComprasDetalles : Objeto
    {
        int _idOrdenCompraDetalle;
        int _idSolicitudCompraDetalle;

        public CmpOrdenesComprasDetallesSolicitudesComprasDetalles()
        { 
        }

        public int IdOrdenCompraDetalle
        {
            get { return _idOrdenCompraDetalle; }
            set { _idOrdenCompraDetalle = value; }
        }

        public int IdSolicitudCompraDetalle
        {
            get { return _idSolicitudCompraDetalle; }
            set { _idSolicitudCompraDetalle = value; }
        }
    }
}
