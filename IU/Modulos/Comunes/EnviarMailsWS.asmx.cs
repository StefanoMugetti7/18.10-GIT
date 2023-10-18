using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Comunes.Entidades;

namespace IU.Modulos.Comunes
{
    /// <summary>
    /// Summary description for EnviarMailsWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class EnviarMailsWS : System.Web.Services.WebService
    {
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(EnableSession = true)]
        public ProgressBarDTO ObetenerProgressBar()
        { 
            ProgressBarDTO proc = (ProgressBarDTO)HttpRuntime.Cache.Get(Session.SessionID + "CacheProgressBarDTOEnviarMails");
            if (proc == null)
                proc = new ProgressBarDTO();
            return proc;
        }
    }
}
