using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CuentasPagar.Entidades;
using Servicio.AccesoDatos;


namespace CuentasPagar.LogicaNegocio
{
    class CapTiposSolicitudPagoLN
    {
        internal List<CapTiposSolicitudPago> ObtenerLista()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CapTiposSolicitudPago>("CapTiposSolicitudPagoListar");
        }
    }
}
