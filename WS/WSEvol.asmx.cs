using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WS.Afip;
using Comunes.Entidades;

namespace WS
{
    /// <summary>
    /// Descripción breve de WSEvol
    /// </summary>
    [WebService(Namespace = "https://erp.evol.com.ar/ws")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class WSEvol : System.Web.Services.WebService
    {

        [WebMethod]
        public AfipServiciosWebTickets ObtenerAutenticacion(Objeto pParametro)
        {
            return new WSLogin().ObtenerAutenticacion(pParametro);
        }
    }
}
