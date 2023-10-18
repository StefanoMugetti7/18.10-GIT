using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Servicio.AccesoDatos;
using Proveedores.Entidades;
using System.Data;

namespace Proveedores.LogicaNegocio
{
    class CapCuentasCorrientesLN
    {
        /// <summary>
        /// Devuelve una lista de Cuenta Corriente por Cliente
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public List<CapCuentasCorrientes> ObtenerPorProveedor(CapCuentasCorrientes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CapCuentasCorrientes>("CapCuentasCorrientesSeleccionarPorProveedor", pParametro);
        }

        public List<CapCuentasCorrientes> ObtenerPorFiltroFecha(CapProveedores pParametro)
        {

            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CapCuentasCorrientes>("CapProveedoresSeleccionarCuentaCorriente", pParametro);
        }

        public DataTable ObtenerPorFiltroFechaDT(CapProveedores pParametro)
        {

            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CapProveedoresSeleccionarCuentaCorriente", pParametro);
        }
    }
}
