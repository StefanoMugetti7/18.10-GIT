using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CuentasPagar.Entidades
{
    public class CapSolicitudPagoDeatallesSolicitudPago : CapSolicitudPagoDetalles
    {
        CapSolicitudPago _solicitudPago;

        public CapSolicitudPago SolicitudPago
        {
            get { return _solicitudPago == null ? (_solicitudPago = new CapSolicitudPago()) : _solicitudPago; }
            set { _solicitudPago = value; }
        }
    }
}
