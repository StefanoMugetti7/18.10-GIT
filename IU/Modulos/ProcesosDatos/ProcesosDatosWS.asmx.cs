using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using Comunes.Entidades;

namespace IU.Modulos.ProcesosDatos
{
    /// <summary>
    /// Descripción breve de ProcesosDatosWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class ProcesosDatosWS : System.Web.Services.WebService
    {
        //private List<string> MisMensajes
        //{
        //    get { return (List<string>)Session["ProcesosDatosModificarMisMensajes"]; }
        //    set { Session["ProcesosDatosModificarMisMensajes"] = value; }
        //}

        //private bool procesando
        //{
        //    get { return (bool)Session["ProcesosDatosModificarDatosProcesando"]; }
        //    set { Session["ProcesosDatosModificarDatosProcesando"] = value; }
        //}

        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(EnableSession = true)]
        public List<ProgressBarDTO> ObetenerMensajes()
        {
            List<ProgressBarDTO> resultado = new List<ProgressBarDTO>();
            ProgressBarDTO item;
            int num = 0;
            object proc = HttpRuntime.Cache.Get(Session.SessionID + "CacheMensajes");
            List<string> MisMensajes = proc != null ? (List<string>)proc : new List<string>(); 
            if (MisMensajes != null && MisMensajes.Count > 0)
            {
                foreach (string s in MisMensajes)
                {
                    item = new ProgressBarDTO();
                    item.number = s.Contains('|') ? int.TryParse(s.Split('|')[0], out num) ? num : 0 : 0;
                    item.text = s.Contains('|') ? s.Split('|')[1] : s;
                    resultado.Add(item);
                }
            }
            return resultado;
        }

        [WebMethod(EnableSession = true)]
        public string Procesando()
        {
            //return this.procesando;
            object proc = HttpRuntime.Cache.Get(Session.SessionID + "CacheProcesoProcesando");
            if (proc != null)
                return proc.ToString();
            else
                return string.Empty;
        }
    }
}
