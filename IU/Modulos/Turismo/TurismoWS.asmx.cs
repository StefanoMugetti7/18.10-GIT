using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Turismo;
using Turismo.Entidades;

namespace IU.Modulos.Turismo
{
    /// <summary>
    /// Descripción breve de TurismoWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    [ScriptService]
    public class TurismoWS : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Select2DTO ObtenerPorcentajeGanancia(int idTipoServicio)
        {
            Select2DTO aux = new Select2DTO();
            aux.id = idTipoServicio;
            return TurismoF.TurismoObtenerPorcentajePorServicio(aux);
        }
    }
}
