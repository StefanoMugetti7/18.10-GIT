using Mailing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;

namespace IU.Modulos.Mailing
{
    /// <summary>
    /// Descripción breve de MailingWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class MailingWS : System.Web.Services.WebService
    {
        //[SoapHeader("Evol")]
        [WebMethod]
        public XmlDocument GenerarDatosEnviosV2(string idMailing, string idMailingProcesamiento,string txtAsunto, string key, string value)
        {
            XmlDocument doc = MailingF.TGEMailingGenerarDatosEnviosV2(idMailing,idMailingProcesamiento, txtAsunto, key, value);
            return doc;
        }

        [WebMethod]
        public XmlDocument GenerarDatosEnvios(string idMailing, string key, string value)
        {
            XmlDocument doc = MailingF.TGEMailingGenerarDatosEnvios(idMailing, key, value);
            return doc;
        }
    }

    public class AuthHeader : SoapHeader
    {
        public string apikey;
    }
}
