using Comunes.Entidades;
using Generales.Entidades;
using Seguridad.FachadaNegocio;
using System;
using System.Collections.Generic;
using System.Web.Script.Services;
using System.Web.Services;

namespace IU.Modulos.Seguridad
{
    /// <summary>
    /// Descripción breve de RequerimientosWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class SeguridadWS : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool NotificacionesGuardar(string filtro)
        {
            var arrayId = filtro.Split(',');
            List<SegNotificaciones> notis = new List<SegNotificaciones>();
            foreach (string id in arrayId)
            {
                SegNotificaciones noti = new SegNotificaciones();
                noti.IdNotificacion = Convert.ToInt32(id);
                noti.Estado.IdEstado = (int)EstadosNotificaciones.Leido;
                notis.Add(noti);
            }
            Usuarios user = new Usuarios();
            user.Notificaciones = notis;

            return SeguridadF.UsuariosModificarNotificaciones(user);
        }
    }
}
