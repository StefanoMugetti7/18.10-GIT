using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facturas.Entidades;
using Servicio.AccesoDatos;
using Afiliados.Entidades;
using System.Data;

namespace Facturas.LogicaNegocio
{
    class VTACuentasCorrientesLN
    {
        /// <summary>
        /// Devuelve una lista de Cuenta Corriente por Cliente
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public List<VTACuentasCorrientes> ObtenerPorCliente(AfiAfiliados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<VTACuentasCorrientes>("VTACuentasCorrientesSeleccionarPorAfiliado", pParametro);
        }

        public DataTable ObtenerPorClienteTable(AfiAfiliados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("VTACuentasCorrientesSeleccionarPorAfiliado", pParametro);
        }
        public DataTable ObtenerPorClienteIdRefTablaTable(AfiAfiliados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("VTACuentasCorrientesSeleccionarPorAfiliadoIdRefTabla", pParametro);
        }
        public DataTable ObtenerTiposValoresImportesPorClienteIdRefTablaTable(AfiAfiliados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("VTACuentasCorrientesSeleccionarTiposValoresImportesPorAfiliadoIdRefTabla", pParametro);
        }
    }

}
