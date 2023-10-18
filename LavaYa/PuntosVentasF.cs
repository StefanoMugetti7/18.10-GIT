using LavaYa.Entidades;
using LavaYa.LogicaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LavaYa
{
    public class PuntosVentasF
    {
        public static LavPuntosVentas PuntosVentasObtenerDatosCompletos(LavPuntosVentas pPuntoVenta)
        {
            return new PuntosVentasLN().ObtenerDatosCompletos(pPuntoVenta);
        }
        public static bool PuntosVentasAgregar(LavPuntosVentas pParametro)
        {
            return new PuntosVentasLN().Agregar(pParametro);
        }

        public static bool PuntosVentasModificar(LavPuntosVentas pParametro)
        {
            return new PuntosVentasLN().Modificar(pParametro);
        }

        public static DataTable PuntosVentasObtenerListaGrilla(LavPuntosVentas pParametro)
        {
            return new PuntosVentasLN().ObtenerListaGrilla(pParametro);
        }

    }
}
