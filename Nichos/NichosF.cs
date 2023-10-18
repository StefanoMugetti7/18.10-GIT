
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nichos.LogicaNegocio;
using Nichos.Entidades;
using Afiliados;
using Afiliados.Entidades;

namespace Nichos
{
    public class NichosF
    {
        #region Panteones
        public static List<NCHNichos> NichosObtenerListaActiva(NCHNichos pParametro)
        {
            return new NichosLN().ObtenerListaActiva(pParametro);
        }
        public static List<NCHNichos> NichosObtenerListaSegunPanteon(NCHNichos pParametro)
        {
            return new NichosLN().ObtenerListaSegunPanteon(pParametro);
        }

        public static DataTable NichosObtenerListaGrilla(NCHNichos pParametro)
        {
            return new NichosLN().ObtenerListaGrilla(pParametro);
        }
        public static DataTable NichosObtenerDisponibles(NCHNichos pParametro)
        {
            return new NichosLN().ObtenerDisponibles(pParametro);
        }

        public static DataTable NichosObtenerImporte(NCHNichos pParametro)
        {
            return new NichosLN().ObtenerImporte(pParametro);
        }
        public static NCHNichos NichosObtenerDatosCompletos(NCHNichos pParametro)
        {
            return new NichosLN().ObtenerDatosCompletos(pParametro);
        }

        public static bool NichosAgregar(NCHNichos pParametro)
        {
            return new NichosLN().Agregar(pParametro);
        }

        public static bool NichosModificar(NCHNichos pParametro)
        {
            return new NichosLN().Modificar(pParametro);
        }

        public static DataTable NichosCargarCardsBootStrap(NCHNichos pParametro)
        {
            return new NichosLN().ObtenerCardsBootStrap(pParametro);
        } 
        public static DataTable NichosObtenerAfiliados(NCHNichos pParametro)
        {
            return new NichosLN().ObtenerNichosAfiliados(pParametro);
        }

        #endregion
    }
}
