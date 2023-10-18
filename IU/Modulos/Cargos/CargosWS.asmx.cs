using Cargos.Entidades;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace IU.Modulos.Cargos
{
    /// <summary>
    /// Descripción breve de CargosWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class CargosWS : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public CarTiposCargosAfiliadosFormasCobros CargosAfiliadosObtenerABonificar(string filtro, string idAfiliado)
        {            
            CarTiposCargosAfiliadosFormasCobros tiposCargos = new CarTiposCargosAfiliadosFormasCobros();
            tiposCargos.IdReferenciaRegistro = filtro == string.Empty ? 0 : Convert.ToInt32(filtro.Split('|')[0]);
            tiposCargos.IdAfiliadoRef = filtro == string.Empty ? 0 : Convert.ToInt32(filtro.Split('|')[1]);
            tiposCargos.IdAfiliado = idAfiliado == string.Empty ? 0 : Convert.ToInt32(idAfiliado);
            CarTiposCargosAfiliadosFormasCobros hola =  BaseDatos.ObtenerBaseDatos().Obtener<CarTiposCargosAfiliadosFormasCobros>("CarTiposCargosSeleccionarABonificar", tiposCargos);
            return hola;
                //BaseDatos.ObtenerBaseDatos().Obtener<CarTiposCargosAfiliadosFormasCobros>("CarTiposCargosSeleccionarABonificar", tiposCargos);
        }
    }
}
