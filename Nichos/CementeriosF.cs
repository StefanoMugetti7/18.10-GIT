
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
    public class CementeriosF
    {
        #region CEMENTERIOS
        public static List<NCHCementerios> CementeriosObtenerListaActiva(NCHCementerios pParametro)
        {
            return new CementeriosLN().ObtenerListaActiva(pParametro);
        }

        public static DataTable CementeriosObtenerListaGrilla(NCHCementerios pParametro)
        {
            return new CementeriosLN().ObtenerListaGrilla(pParametro);
        }

        public static DataTable CementeriosObtenerAgenda(NCHCementerios pParametro)
        {
            //return new HotelesLN().ObtenerAgenda(pParametro);
            return new DataTable();
        }


        public static NCHCementerios CementeriosObtenerDatosCompletos(NCHCementerios pCementerios)
        {
            return new CementeriosLN().ObtenerDatosCompletos(pCementerios);
        }

        public static bool CementeriosAgregar(NCHCementerios pParametro)
        {
            return new CementeriosLN().Agregar(pParametro);
        }

        public static bool CementeriosModificar(NCHCementerios pParametro)
        {
            return new CementeriosLN().Modificar(pParametro);
        }

        #endregion
    }
}
