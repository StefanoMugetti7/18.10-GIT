using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Proveedores;
using Proveedores.Entidades;
using Servicio.AccesoDatos;

namespace IU.Modulos.Proveedores
{
    /// <summary>
    /// Descripción breve de ProveedoresWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class ProveedoresWS : System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool IniciarWS()
        {
            return true;
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<CapProveedores> ProveedoresComb(string value, string filtro)
        {
            CapProveedores prov = new CapProveedores();
            int idAfi = 0;
            int.TryParse(value, out idAfi);
            prov.RazonSocial = filtro;
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CapProveedores>("ProveedoresSeleccionarAjaxComboCodigoProveedor", prov);
        }

        //[WebMethod()]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public List<CapProveedores> ProveedoresVisitaCombo(string value, string filtro)
        //{
        //    AfiAfiliados afi = new AfiAfiliados();
        //    int idAfi = 0;
        //    int.TryParse(value, out idAfi);
        //    afi.NumeroDocumento = Convert.ToInt64(filtro);
        //    return BaseDatos.ObtenerBaseDatos().ObtenerLista<CapProveedores>("[AfiAfiliadosSeleccionarVisitasAjaxComboDocumento]", afi);
        //}

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<CapProveedoresDTO> ProveedoresCombo(string value, string filtro)
        {
            CapProveedores afi = new CapProveedores();
            int idAfi = 0;
            int.TryParse(value, out idAfi);
            afi.RazonSocial = filtro;
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CapProveedoresDTO>("ProveedoresSeleccionarAjaxComboCodigoProveedor", afi);
        }
        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public CapProveedores ProveedoresValidarCuit(string cuit)
        {
           
            CapProveedores afi = new CapProveedores();

            afi.CUIT = cuit;
            ProveedoresF.ProveedoresObtenerDatosAFIP(afi);
            return afi;
          
        }
    }
}
