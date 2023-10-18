using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Hoteles;
using Hoteles.Entidades;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml;

namespace IU.Modulos.Hotel
{
    /// <summary>
    /// Summary description for HotelWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class HotelWS : System.Web.Services.WebService
    {

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<HTLReservasDetallesDTO> ReservasSeleccionarAjaxComboDetalleGastos(HTLReservasDetalles filtro, List<HTLReservasDetalles> gastosSeleccionados)//, List<TGECamposValores> camposValores)
        {
            return HotelesF.ReservasDetallesObtenerAjaxComboDetalleGastos<HTLReservasDetallesDTO>(filtro, gastosSeleccionados);
            //filtro.LoteCamposValores = new XmlDocument();
            //XmlNode nodos = filtro.LoteCamposValores.CreateElement("ReservasDetalles");
            //filtro.LoteCamposValores.AppendChild(nodos);

            //XmlNode itemNodo;
            //XmlAttribute itemAttribute;
            //foreach (HTLReservasDetalles item in gastosSeleccionados)
            //{
            //    if (item.IdHabitacion.HasValue)
            //    {
            //        itemNodo = filtro.LoteCamposValores.CreateElement("ReservaDetalle");
            //        itemAttribute = filtro.LoteCamposValores.CreateAttribute("IdHabitacion");
            //        itemAttribute.Value = item.IdHabitacion.ToString();
            //        itemNodo.Attributes.Append(itemAttribute);

            //        itemAttribute = filtro.LoteCamposValores.CreateAttribute("FechaIngreso");
            //        itemAttribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(item.FechaIngreso.Value);
            //        itemNodo.Attributes.Append(itemAttribute);
            //        nodos.AppendChild(itemNodo);

            //        itemAttribute = filtro.LoteCamposValores.CreateAttribute("FechaEgreso");
            //        itemAttribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(item.FechaEgreso.Value);
            //        itemNodo.Attributes.Append(itemAttribute);
            //        nodos.AppendChild(itemNodo);

            //        itemAttribute = filtro.LoteCamposValores.CreateAttribute("Compartida");
            //        itemAttribute.Value = item.Compartida.ToString();
            //        itemNodo.Attributes.Append(itemAttribute);
            //        nodos.AppendChild(itemNodo);

            //        itemAttribute = filtro.LoteCamposValores.CreateAttribute("IdHabitacionDetalle");
            //        itemAttribute.Value = item.HabitacionDetalle.IdHabitacionDetalle.ToString();
            //        itemNodo.Attributes.Append(itemAttribute);
            //        nodos.AppendChild(itemNodo);
            //    }
            //}

            //return BaseDatos.ObtenerBaseDatos().ObtenerLista<HTLReservasDetallesDTO>("HTLReservasSeleccionarAjaxComboDetalleGastos", filtro);
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<Select2DTO> ListasPreciosHotelesCombo(string idHotel, string fecha)
        {
            HTLReservas reserva = new HTLReservas();
            reserva.FechaIngreso = string.IsNullOrEmpty(fecha) ? default(DateTime?) : Convert.ToDateTime(fecha);
            reserva.IdHotel = idHotel == string.Empty ? 0 : Convert.ToInt32(idHotel);
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<Select2DTO>("CMPListasPreciosSeleccionarHoteles", reserva);
        }
        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public HTLHoteles FechasHotelesReservas(string idHotel)
        {
            HTLHoteles hotel = new HTLHoteles();
          
            hotel.IdHotel = idHotel == string.Empty ? 0 : Convert.ToInt32(idHotel);
            return BaseDatos.ObtenerBaseDatos().Obtener<HTLHoteles>("HTLHotelesSeleccionarWS", hotel);
        }
        //[WebMethod()]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public List<Select2DTO> HabitacionesMoviliarioDisponible(string idHabitacion, string fechaIngreso, string fechaEgreso)
        //{
        //    HTLReservasDetalles reservaDet = new HTLReservasDetalles();
        //    reservaDet.FechaIngreso = string.IsNullOrEmpty(fechaIngreso) ? default(DateTime?) : Convert.ToDateTime(fechaIngreso);
        //    reservaDet.FechaEgreso = string.IsNullOrEmpty(fechaEgreso) ? default(DateTime?) : Convert.ToDateTime(fechaEgreso);
        //    reservaDet.IdHabitacion = idHabitacion == string.Empty ? 0 : Convert.ToInt32(idHabitacion);
        //    return BaseDatos.ObtenerBaseDatos().ObtenerLista<Select2DTO>("HTLReservasSeleccionarAjaxComboDetalleMoviliario", reservaDet);
        //}

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public HTLDescuentosDTO DescuentosObtenerDatos(HTLDescuentosFiltros descuento)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<HTLDescuentosDTO>("HTLDescuentosSeleccionar", descuento);
        }
    }
}
