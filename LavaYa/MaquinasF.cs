using LavaYa.Entidades;
using LavaYa.LogicaNegocio;
using System.Collections.Generic;
using System.Data;

namespace LavaYa
{
    public class MaquinasF
    {
        public static LavMaquinas MaquinasObtenerDatosCompletos(LavMaquinas pPuntoVenta)
        {
            return new MaquinasLN().ObtenerDatosCompletos(pPuntoVenta);
        }
        public static bool MaquinasAgregar(LavMaquinas pParametro)
        {
            return new MaquinasLN().Agregar(pParametro);
        }

        public static bool MaquinasModificar(LavMaquinas pParametro)
        {
            return new MaquinasLN().Modificar(pParametro);
        }

        public static DataTable MaquinasObtenerListaGrilla(LavMaquinas pParametro)
        {
            return new MaquinasLN().ObtenerListaGrilla(pParametro);
        }

        public static List<LavMaquinas> MaquinasObtenerMarcas ()
        {
            return new MaquinasLN().ObtenerMarcas();
        }
        public static List<LavMaquinas> MaquinasObtenerModelos(LavMaquinas pParametro)
        {
            return new MaquinasLN().ObtenerModelos(pParametro);
        }
        public static List<LavMaquinas> MaquinasObtenerEdificios()
        {
            return new MaquinasLN().ObtenerEdificios();
        }
        public static List<LavMaquinas> MaquinasObtenerTiposMaquinas()
        {
            return new MaquinasLN().ObtenerTiposMaquinas();
        }   
        public static List<LavMaquinas> MaquinasEdificiosObtenerMaquinas()
        {
            return new MaquinasLN().ObtenerMaquinas();
        }
    }
}
