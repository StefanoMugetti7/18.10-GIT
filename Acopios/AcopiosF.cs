using Acopios.Entidades;
using Acopios.LogicaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acopios
{
    public class AcopiosF
    {
        public static AcpAcopios AcopiosObtenerDatosCompletos(AcpAcopios pParametro)
        {
            return new AcopiosLN().ObtenerDatosCompletos(pParametro);
        }

        public static bool AcopiosAgregar(AcpAcopios pParametro)
        {
            return new AcopiosLN().Agregar(pParametro);
        }

        public static bool AcopiosModificar(AcpAcopios pParametro)
        {
            return new AcopiosLN().Modificar(pParametro);
        }
    }
}
