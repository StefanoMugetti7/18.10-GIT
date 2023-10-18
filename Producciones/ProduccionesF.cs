using Producciones.Entidades;
using Producciones.LogicaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Producciones
{
    public class ProduccionesF
    {
        #region Producciones

        public static DataTable ProduccionesObtenerListaGrilla(PrdProducciones pParametro)
        {
            return new ProduccionesLN().ObtenerListaGrilla(pParametro);
        }

        public static DataTable ProduccionesObtenerTotalesProductoPorProduccion(PrdProducciones pParametro)
        {
            return new ProduccionesLN().ObtenerTotalesProductoPorProduccion(pParametro);
        }

        public static PrdProducciones ProduccionesObtenerDatosCompletos(PrdProducciones pProducciones)
        {
            return new ProduccionesLN().ObtenerDatosCompletos(pProducciones);
        }

        public static bool ProduccionesAgregar(PrdProducciones pParametro)
        {
            return new ProduccionesLN().Agregar(pParametro);
        }

        public static bool ProduccionesModificar(PrdProducciones pParametro)
        {
            return new ProduccionesLN().Modificar(pParametro);
        }

        #endregion
    }
}
