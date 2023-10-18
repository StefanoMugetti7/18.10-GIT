using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Servicio.AccesoDatos;
using System.Data;
using System.Web.Script.Services;

namespace IU.Modulos.Comunes
{
    /// <summary>
    /// Descripción breve de CamposValoresWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    // Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class CamposValoresWS : System.Web.Services.WebService
    {

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<object> ListaGenerica2(string sp, string value, string filtro, string idRefTablaValor, string idRefValor)
        {
            TGECamposValores parametro = new TGECamposValores();
            parametro.Valor = value;
            parametro.Filtro = filtro;
            parametro.IdRefTablaValor = string.IsNullOrEmpty(idRefTablaValor) ? 0 : Convert.ToInt32(idRefTablaValor);
            parametro.IdRefValor = string.IsNullOrEmpty(idRefValor) ? default(int?) : Convert.ToInt32(idRefValor);
         
            DataSet ds = BaseDatos.ObtenerBaseDatos().ObtenerDataSet(sp, parametro);

            List<object> resultado = new List<object>();
            Select2DTO item;
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow fila in ds.Tables[0].Rows)
                {
                    item = new Select2DTO();
                    item.id = Convert.ToInt32(fila["IdListaValorDetalle"].ToString());
                    item.text = fila["Descripcion"].ToString();
                    resultado.Add(item);
                }
            }
            return resultado;
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<object> ListaGenerica(string sp, string value, string filtro)
        {
            TGECamposValores parametro = new TGECamposValores();
            parametro.Valor = value;
            parametro.Filtro = filtro;
            //parametro.IdRefTablaValor = string.IsNullOrEmpty(idRefTablaValor) ? 0 : Convert.ToInt32(idRefTablaValor);
            DataSet ds = BaseDatos.ObtenerBaseDatos().ObtenerDataSet(sp, parametro);

            List<object> resultado = new List<object>();
            Select2DTO item;
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow fila in ds.Tables[0].Rows)
                {
                    item = new Select2DTO();
                    item.id = Convert.ToInt32(fila["IdListaValorDetalle"].ToString());
                    item.text = fila["Descripcion"].ToString();
                    resultado.Add(item);
                }
            }
            return resultado;
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public TGECamposValores ObtenerGenerico(string sp, string value, string filtro)
        {
            TGECamposValores parametro = new TGECamposValores();
            parametro.Valor = value;
            parametro.Filtro = filtro;
            return BaseDatos.ObtenerBaseDatos().Obtener<TGECamposValores>(sp, parametro);
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public TGECamposValores ObtenerGenerico2(string sp, string value, string filtro, string idRefTablaValor, string idRefValor)
        {
            TGECamposValores parametro = new TGECamposValores();
            parametro.Valor = value;
            parametro.Filtro = filtro;
            parametro.IdRefTablaValor = string.IsNullOrEmpty(idRefTablaValor) ? 0 : Convert.ToInt32(idRefTablaValor);
            parametro.IdRefValor = string.IsNullOrEmpty(idRefValor) ? default(int?) : Convert.ToInt32(idRefValor);

            return BaseDatos.ObtenerBaseDatos().Obtener<TGECamposValores>(sp, parametro);
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<TGECamposDTO> ReservasSeleccionarAjaxComboDetalleGastos(TGECampos filtro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TGECamposDTO>("HTLReservasSeleccionarAjaxComboDetalleGastos", filtro);
        }
    }
}
