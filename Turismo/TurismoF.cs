using Cargos.Entidades;
using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turismo.Entidades;
using Turismo.LogicaNegocio;

namespace Turismo
{
    public class TurismoF
    {
        #region TURISMO
        public static DataSet TurismoObtenerReservasDetallesDesdeXML(TGECampos pParametro)
        {
            return new TurismoLN().ObtenerReservasDetallesDesdeXML(pParametro);
        }
   
        public static TurTurismo TurismoObtenerProximoNumeroReserva(TurTurismo pParametro)
        {
            return new TurismoLN().ObtenerProximoNumeroReserva(pParametro);
        }

        public static Select2DTO TurismoObtenerPorcentajePorServicio(Select2DTO pParametro)
        {
            return new TurismoLN().ObtenerPorcentajeServicio(pParametro);
        }

        //public static List<TurTurismoDetalle> TurismoObtenerTiposServicios(TurTurismoDetalle pParametro)
        //{
        //    return new TurismoLN().ObtenerTiposServicios(pParametro);
        //}
        //public static List<TurTurismoDetalle> TurismoObtenerTiposTransporte(TurTurismoDetalle pParametro)
        //{
        //    return new TurismoLN().ObtenerTiposTransporte(pParametro);
        //}
        //public static List<TurTurismoDetalle> TurismoObtenerTiposRegimenes(TurTurismoDetalle pParametro)
        //{
        //    return new TurismoLN().ObtenerTiposRegimenes(pParametro);
        //}
        //public static List<TurTurismoDetalle> TurismoObtenerTiposHabitaciones(TurTurismoDetalle pParametro)
        //{
        //    return new TurismoLN().ObtenerTiposHabitaciones(pParametro);
        //}
        //public static DataTable TurismoManejadorControles(TurTurismoDetalle pParametro)
        //{
        //    return new TurismoLN().ManejadorControles(pParametro);
        //}
        #endregion
        #region PAQUETES

        public static TurPaquetes PaquetesObtenerDatosCompletos(TurPaquetes pParametro)
        {
            return new PaquetesLN().ObtenerDatosCompletos(pParametro);
        }

        public static bool PaquetesAgregar(TurPaquetes pParametro)
        {
            return new PaquetesLN().Agregar(pParametro);
        }  
        public static bool PaquetesModificar(TurPaquetes pParametro)
        {
            return new PaquetesLN().Modificar(pParametro);
        }

        public static List<CarTiposCargos> PaquetesObtenerTiposCargos(TurPaquetes pParametro)
        {
            return new PaquetesLN().ObtenerTiposCargos(pParametro);
        }
        public static DataSet PaquetesObtenerReservasDetallesDesdeXML(TGECampos pParametro)
        {
            return new PaquetesLN().ObtenerReservasDetallesDesdeXML(pParametro);
        }

        public static DataTable PaquetesObtenerListaGrilla(TurPaquetes pParametro)
        {
            return new PaquetesLN().ObtenerListaGrilla(pParametro);
        }

        public static List<TurPaquetesDetalles> PaquetesObtenerDetalles(TurPaquetes pParametro)
        {
            return new PaquetesLN().ObtenerPaquetes(pParametro);
        }

        public static DataSet PaquetesObtenerReservasDetallesDesdeXMLATurismo(TGECampos pParametro)
        {
            return new PaquetesLN().ObtenerReservasDetallesDesdeXMLATurismo(pParametro);
        }
        #endregion
        #region CONVENIOS
        public static DataTable ConveniosObtenerListaGrilla(TurConvenios pParametro)
        {
            return new ConveniosLN().ObtenerListaGrilla(pParametro);
        }

        public static bool ConveniosAgregar(TurConvenios pParametro)
        {
            return new ConveniosLN().Agregar(pParametro); 
        }

        public static bool ConveniosModificar(TurConvenios pParametro)
        {
            return new ConveniosLN().Modificar(pParametro);
        }

        public static TurConvenios ConveniosObtenerDatosCompletos(TurConvenios pParametro)
        {
            return new ConveniosLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<TurConvenios> ConveniosObtenerHoteles(TurConvenios pParametro)
        {
            return new ConveniosLN().ObtenerHoteles(pParametro);
        }
        #endregion
    }
}
