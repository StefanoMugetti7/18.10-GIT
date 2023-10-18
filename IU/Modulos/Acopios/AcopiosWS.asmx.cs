using Acopios.Entidades;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace IU.Modulos.Acopios
{
    /// <summary>
    /// Summary description for AcopiosWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class AcopiosWS : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<AcpAcopiosImportes> AcopiosImportesSeleccionarAjaxComboComprobantes(AcpAcopios acopio)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AcpAcopiosImportes>("AcopiosImportesSeleccionarAjaxComboOrdenesPagos", acopio);
        }
    }
}
