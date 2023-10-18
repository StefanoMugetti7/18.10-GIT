using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Subsidios.Entidades;
using Subsidios.LogicaNegocio;

namespace Subsidios
{
    public class SubsidiosF
    {
        public static SubSubsidios SubsidiosObtenerDatosCompletos(SubSubsidios pParametro)
        {
            return new SubSubsidiosLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<SubSubsidios> SubsidiosObtenerListaFiltro(SubSubsidios pParametro)
        {
            return new SubSubsidiosLN().ObtenerListaFiltro(pParametro);
        }

        public static DataTable SubsidiosObtenerListaFiltroDT(SubSubsidios pParametro)
        {
            return new SubSubsidiosLN().ObtenerListaFiltroDT(pParametro);
        }

        public static bool SubsidiosAgregar(SubSubsidios pParametro)
        {
            return new SubSubsidiosLN().Agregar(pParametro);
        }

        public static bool SubsidiosModificar(SubSubsidios pParametro)
        {
            return new SubSubsidiosLN().Modificar(pParametro);
        }

        public static DataTable CuentasCorrientesObtenerListaFiltroFechasDT(SubSubsidios pParametro)
        {
            return new SubSubsidiosLN().ObtenerCuentaCorrienteDT(pParametro);
        }

    }
}
