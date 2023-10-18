using Compras;
using Compras.Entidades;
using Comunes.Entidades;
using Producciones.Entidades;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace IU.Modulos.Produccion
{
    /// <summary>
    /// Summary description for ProduccionWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ProduccionWS : System.Web.Services.WebService
    {

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<PrdProduccionesDetalles> ProduccionSeleccionarAjaxComboProductosStock(string value, string filtro, string idFilial, string idProduccion)
        {
            PrdProduccionesDetallesAjax prodDetalle = new PrdProduccionesDetallesAjax();
            int id = 0;
            int.TryParse(value, out id);
            string sp = "PrdProduccionesDetallesSeleccionarAjaxComboProductosStock";
            if (id == -1)
                sp = "PrdProduccionesDetallesSeleccionarAjaxComboProductosDevolucion";
            prodDetalle.IdFilial = Convert.ToInt32(idFilial);
            prodDetalle.Descripcion = filtro;
            prodDetalle.IdProduccion = idProduccion == string.Empty ? 0 : Convert.ToInt32(idProduccion);
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<PrdProduccionesDetalles>(sp, prodDetalle);
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<Select2DTO> ProduccionSeleccionarAjaxComboProductos(string filtro)
        {
            CMPProductos producto = new CMPProductos();
            producto.Estado.IdEstado = (int)Estados.Activo;
            producto.Descripcion = filtro;
            List<CMPProductos> lista = ComprasF.ProductosObtenerListaFiltro(producto);
            List<Select2DTO> resultado = new List<Select2DTO>();
            Select2DTO item;
            foreach (CMPProductos p in lista)
            {
                item = new Select2DTO();
                item.id = p.IdProducto;
                item.text = p.Descripcion;
                resultado.Add(item);
            }
            return resultado;
        }
    }
}
