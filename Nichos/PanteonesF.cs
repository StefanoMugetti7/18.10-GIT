
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nichos.LogicaNegocio;
using Nichos.Entidades;

namespace Nichos
{
    public class PanteonesF
    {
        #region Panteones
        public static List<NCHPanteones> PanteonesObtenerListaActiva(NCHPanteones pParametro)
        {
            return new PanteonesLN().ObtenerListaActiva(pParametro);
        }

        public static DataTable PanteonesObtenerListaGrilla(NCHPanteones pParametro)
        {
            return new PanteonesLN().ObtenerListaGrilla(pParametro);
        }

        public static DataTable PanteonesObtenerAgenda(NCHPanteones pParametro)
        {
            return new DataTable();
        }

        public static NCHPanteones PanteonesObtenerDatosCompletos(NCHPanteones pParametro)
        {
            return new PanteonesLN().ObtenerDatosCompletos(pParametro);
        }

        public static bool PanteonesAgregar(NCHPanteones pParametro)
        {
            return new PanteonesLN().Agregar(pParametro);
        }

        public static bool PanteonesModificar(NCHPanteones pParametro)
        {
            return new PanteonesLN().Modificar(pParametro);
        }

        #endregion
    }
}
