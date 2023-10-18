using Generales.Entidades;
using ProcesosDatos.Entidades;
using System;
using System.Collections.Generic;
using System.Data;

namespace ProcesosDatos
{
    public class LotesCobranzasF
    {
        public static CarTiposCargosLotesEnviados TiposCargosLotesEnviadosObtenerDatosCompletos(CarTiposCargosLotesEnviados pParametro)
        {
            return new LotesCobranzasLN().ObtenerDatosCompletos(pParametro);
        }
        public static bool TiposCargosLotesEnviadosAgregar(CarTiposCargosLotesEnviados pParametro)
        {
            return new LotesCobranzasLN().Agregar(pParametro);
        }
        public static bool TiposCargosLotesEnviadosModificar(CarTiposCargosLotesEnviados pParametro)
        {
            return new LotesCobranzasLN().Modificar(pParametro);
        }
        public static List<TGEFormasCobros> TiposCargosLotesEnviadosObtenerFormasCobros()
        {
            return new LotesCobranzasLN().ObtenerFormasCobros();
        }
        public static DataTable TiposCargosLotesEnviadosObtenerGrilla(CarTiposCargosLotesEnviados pParametro)
        {
            return new LotesCobranzasLN().ObtenerListaDetalles(pParametro);
        }
        public static List<CarTiposCargosLotesEnviados> TiposCargosLotesEnviadosObtenerPeriodos()
        {
            return new LotesCobranzasLN().ObtenerPeriodos();
        }
        public static List<CarTiposCargosLotesEnviadosDetalles> TiposCargosLotesEnviadosObtenerDetalles(CarTiposCargosLotesEnviadosDetalles pParametro)
        {
            return new LotesCobranzasLN().ObtenerDetalles(pParametro);
        }
        public static List<CarTiposCargosLotesEnviadosDetalles> TiposCargosLotesEnviadosObtenerDetallesPorAfiliado(CarTiposCargosLotesEnviadosDetalles pParametro)
        {
            return new LotesCobranzasLN().ObtenerDetallesPorAfiliado(pParametro);
        }
        public static DataTable TiposCargosLotesObtenerListaGrillaPaginado(CarTiposCargosLotesEnviadosDetalles pParametro)
        {
            return new LotesCobranzasLN().ObtenerListaDetallesPaginado(pParametro);
        }

        public static List<CarTiposCargosLotesEnviadosDetalles> TiposCargosLotesObtenerListaDetallesPorID(CarTiposCargosLotesEnviados pParametro)
        {
            return new LotesCobranzasLN().ObtenerDetallesPorId(pParametro); 
        }
    }
}
