using Afiliados;
using Afiliados.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Seguridad.FachadaNegocio;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Web.Script.Services;
using System.Web.Services;

namespace IU.Modulos.Afiliados
{
    /// <summary>
    /// Descripción breve de AfiliadosWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class AfiliadosWS : System.Web.Services.WebService
    {
        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool IniciarWS()
        {
            return true;
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public AfiAfiliadosDTO ObtenerEdad(string idAfiliado, string fechaEgreso)
        {
            //return 54;
            AfiAfiliados afi = new AfiAfiliados();
            int idAfi = 0;
            int.TryParse(idAfiliado, out idAfi);
            afi.FechaHasta = string.IsNullOrEmpty(fechaEgreso) ? default(DateTime?) : Convert.ToDateTime(fechaEgreso);
            afi.IdAfiliado = idAfi;
            return BaseDatos.ObtenerBaseDatos().Obtener<AfiAfiliadosDTO>("AfiAfiliadosObtenerEdad", afi);
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<AfiAfiliadosDTO> AfiliadosCombo(string value, string filtro)
        {
            AfiAfiliados afi = new AfiAfiliados();
            int idAfi = 0;
            int.TryParse(value, out idAfi);
            afi.Apellido = filtro;
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiAfiliadosDTO>("AfiAfiliadosSeleccionarAjaxComboApellido", afi);
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<AfiAfiliadosDTO> AfiliadosVisitasCombo(string value, string filtro)
        {
            AfiAfiliados afi = new AfiAfiliados();
            int idAfi = 0;
            int.TryParse(value, out idAfi);
            afi.NumeroDocumento = Convert.ToInt64(filtro);
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiAfiliadosDTO>("[AfiAfiliadosSeleccionarVisitasAjaxComboDocumento]", afi);
        }


        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public AfiAfiliados AfiliadosSeleccionar(string IdAfiliado)
        {
            AfiAfiliados afi = new AfiAfiliados();
            int idAfi = 0;
            int.TryParse(IdAfiliado, out idAfi);
            afi.IdAfiliado = idAfi;
            return AfiliadosF.AfiliadosObtenerDatos(afi);
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<AfiAfiliadosDTO> AfiliadosClienteCombo(string value, string filtro)
        {
            AfiAfiliados afi = new AfiAfiliados();
            int idAfi = 0;
            int.TryParse(value, out idAfi);
            afi.Apellido = filtro;
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiAfiliadosDTO>("AfiAfiliadosSeleccionarAjaxComboCliente", afi);
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<AfiAfiliadosDTO> AfiliadosClienteComboCaja(string value, string filtro, string idUsuarioEvento)
        {
            AfiAfiliados afi = new AfiAfiliados();
            int idAfi = 0;
            int idUser = 0;
            int.TryParse(value, out idAfi);
            int.TryParse(idUsuarioEvento, out idUser);
            afi.Apellido = filtro;
            afi.UsuarioLogueado.IdUsuarioEvento = idUser;
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiAfiliadosDTO>("AfiAfiliadosSeleccionarAjaxComboClienteCaja", afi);
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<AfiAfiliadosDTO> AfiliadosFormasCobroCombo(string idformacobro, string value, string filtro)
        {
            AfiAfiliadosDTO afi = new AfiAfiliadosDTO();
            int id = 0;
            int.TryParse(value, out id);
            afi.IdAfiliado = id;
            int IdFormaCobro = 0;
            int.TryParse(idformacobro, out IdFormaCobro);
            afi.IdFormaCobro = IdFormaCobro;
            afi.Apellido = filtro;
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiAfiliadosDTO>("AfiAfiliadosSeleccionarAjaxFormasCobro", afi);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool NotificacionesGuardar(string filtro)
        {
            var arrayId = filtro.Split(',');
            List<SegNotificaciones> notis = new List<SegNotificaciones>();
            foreach (string id in arrayId)
            {
                int idParse = -1;
                if (int.TryParse(id, out idParse)){
                    SegNotificaciones noti = new SegNotificaciones();
                    noti.IdNotificacion = idParse;
                    noti.Estado.IdEstado = (int)EstadosNotificaciones.Leido;
                    notis.Add(noti);
                }
            }
            Usuarios user = new Usuarios();
            user.Notificaciones = notis;

            return SeguridadF.UsuariosModificarNotificaciones(user);
        }
    }
}
