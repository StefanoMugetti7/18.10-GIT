using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using CuentasPagar.Entidades;
using Servicio.AccesoDatos;
using System.Data;

namespace CuentasPagar.LogicaNegocio
{
    class CapSolicitudPagoDetalleLN : BaseLN<CapSolicitudPagoDetalles>
    {

        public override bool Agregar(CapSolicitudPagoDetalles pParametro)
        {
            throw new NotImplementedException();
        }

        public override bool Modificar(CapSolicitudPagoDetalles pParametro)
        {
            throw new NotImplementedException();
        }

        public override CapSolicitudPagoDetalles ObtenerDatosCompletos(CapSolicitudPagoDetalles pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CapSolicitudPagoDetalles>("CapSolicitudPagoDetalleSeleccionar", pParametro);
        }

        public override List<CapSolicitudPagoDetalles> ObtenerListaFiltro(CapSolicitudPagoDetalles pParametro)
        {
            throw new NotImplementedException();
        }

        public DataTable ObtenerItemsBienesUsoGrilla(CapSolicitudPagoDeatallesSolicitudPago pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CapSolicitudPagoDetalleSeleccionarBienesUso", pParametro);
        }
    }
}
