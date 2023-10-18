using CRM.Entidades;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace IU.Modulos.CRM
{
    /// <summary>
    /// Descripción breve de RequerimientosWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
     [System.Web.Script.Services.ScriptService]
    public class RequerimientosWS : System.Web.Services.WebService
    {

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public List<CRMRequerimientos> EntidadesOrigenCombo(string filtro)
        //{
        //    CRMRequerimientos req = new CRMRequerimientos();
        //    req.Tabla = filtro;
        //    return BaseDatos.ObtenerBaseDatos().ObtenerLista<CRMRequerimientos>("CRMRequerimientosSeleccionarAjaxComboEntidadesOrigen", req);
        //} 
        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public List<CRMRequerimientos> EntidadesDestinoCombo(string filtro)
        //{
        //    CRMRequerimientos req = new CRMRequerimientos();
        //   // req.TablaAccion = filtro;
        //    return BaseDatos.ObtenerBaseDatos().ObtenerLista<CRMRequerimientos>("CRMRequerimientosSeleccionarAjaxComboEntidadesDestino", req);
        //}
    }
}
