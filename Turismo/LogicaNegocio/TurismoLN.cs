using Comunes;
using Comunes.Entidades;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turismo.Entidades;

namespace Turismo.LogicaNegocio
{
    public class TurismoLN
    {
        public DataSet ObtenerReservasDetallesDesdeXML(TGECampos pCampo)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerDataSet("TurTurismoDetalleObtenerDesdeXML", pCampo);
        }

        public TurTurismo ObtenerProximoNumeroReserva(TurTurismo pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<TurTurismo>("TurTurismoSeleccionarProximoNumeroReserva", pParametro);
        }

        public Select2DTO ObtenerPorcentajeServicio(Select2DTO pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<Select2DTO>("TurTurismoObtenerPorcentaje", pParametro);
        }
        
        //public List<TurTurismoDetalle> ObtenerTiposServicios(TurTurismoDetalle pParametro)
        //{
        //    return BaseDatos.ObtenerBaseDatos().ObtenerLista<TurTurismoDetalle>("TurTurismoObtenerTiposServicios", pParametro);
        //}
        //public List<TurTurismoDetalle> ObtenerTiposHabitaciones(TurTurismoDetalle pParametro)
        //{
        //    return BaseDatos.ObtenerBaseDatos().ObtenerLista<TurTurismoDetalle>("TurTurismoObtenerHabitaciones", pParametro);
        //}
        //public List<TurTurismoDetalle> ObtenerTiposRegimenes(TurTurismoDetalle pParametro)
        //{
        //    return BaseDatos.ObtenerBaseDatos().ObtenerLista<TurTurismoDetalle>("TurTurismoObtenerRegimenPension", pParametro);
        //}
        //public List<TurTurismoDetalle> ObtenerTiposTransporte(TurTurismoDetalle pParametro)
        //{
        //    return BaseDatos.ObtenerBaseDatos().ObtenerLista<TurTurismoDetalle>("TurTurismoObtenerTransportes", pParametro);
        //}
        //public DataTable ManejadorControles(TurTurismoDetalle pParametro)
        //{
        //    return BaseDatos.ObtenerBaseDatos().ObtenerLista("TurTurismoManejadorControles", pParametro);

        //}
    }
}
