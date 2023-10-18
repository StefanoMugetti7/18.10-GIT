using Compras.Entidades;
using Comunes.Entidades;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace IU.Modulos.Compras
{
    /// <summary>
    /// Descripción breve de ComprasWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class ComprasWS : System.Web.Services.WebService
    {

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<CMPProductosDTO> CMPProductosSeleccionarFiltro(string value, string filtro, string proveedor)
        {
            CMPProductos cmpProductos = new CMPProductos();
            int id = 0;
            int.TryParse(value, out id);
            string sp = "CMPProductosSeleccionarAjaxCombo";
            //if (filialEntrega == "")
            //{
            //    return new List<CMPProductosDTO>();
            //}
            cmpProductos.IdProveedor = string.IsNullOrWhiteSpace(proveedor) ? 0 : Convert.ToInt32(proveedor);
            cmpProductos.Descripcion = filtro;
            cmpProductos.Venta = false;
            cmpProductos.Compra = true;
            //listaDetalle.ListaPrecio.IdAfiliado = ;
            //int idVal = 0;
            //if (!Int32.TryParse(filtro, out idVal) && filtro.Length < 4)
            //    return new List<CMPProductosDTO>();

            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CMPProductosDTO>(sp, cmpProductos);

        }
        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<CMPProductosDTO> CMPProductosSeleccionarStockFiltro(string value, string filtro, string idFilial)
        {
            CMPProductos cmpProductos = new CMPProductos();
            int id = 0;
            int.TryParse(value, out id);
            string sp = "CMPProductosSeleccionarStockAjaxCombo";
            //if (filialEntrega == "")
            //{
            //    return new List<CMPProductosDTO>();
            //}
            cmpProductos.IdFilial = string.IsNullOrWhiteSpace(idFilial) ? 0 : Convert.ToInt32(idFilial);
            cmpProductos.Descripcion = filtro;
            cmpProductos.Venta = false;
            cmpProductos.Compra = true;
            //listaDetalle.ListaPrecio.IdAfiliado = ;
            //int idVal = 0;
            //if (!Int32.TryParse(filtro, out idVal) && filtro.Length < 4)
            //    return new List<CMPProductosDTO>();

            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CMPProductosDTO>(sp, cmpProductos);

        }
        /// <summary>
        /// OBTIENE TODAS LAS RESERVAS ACTIVAS, METODO UTILIZADO PARA REIMPUTACION DE ANTICIPOS
        /// </summary>
        /// <param name="filtro">Dato del combo ajax</param>
        /// <returns>Lista de reservas activas</returns>
        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<Select2DTO> ObtenerReservasActivas(string filtro)
        {
            Select2DTO obj = new Select2DTO();
            string sp = "CapSolicitudPagoObtenerReservas";
            obj.Filtro = filtro;
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<Select2DTO>(sp, obj);

        }
        //[WebMethod()]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public List<CMPListasPreciosDetallesDTO> CMPProductosSeleccionarListaPrecios(string value, string filtro, string filialEntrega, string cantidadCuotas)
        //{
        //    CMPListasPreciosDetalles listaDetalle = new CMPListasPreciosDetalles();
        //    int id = 0;
        //    int.TryParse(value, out id);
        //    string sp = "VTARemitosSeleccionarAjaxComboProductosListaPrecio";
        //    //if (filialEntrega == "")
        //    //{
        //    //    listaDetalle.ValidarConfirmarMensajes("Debe seleccionar una Filial de Entrega");
        //    //    return new List<CMPListasPreciosDetallesDTO>();
        //    //}

        //    listaDetalle.NoIncluidoEnAcopio = true;
        //    listaDetalle.Producto.Descripcion = filtro;
        //    listaDetalle.CantidadCuotas = Convert.ToInt32(cantidadCuotas);
        //    //int idVal = 0;
        //    //if (!Int32.TryParse(filtro, out idVal) && filtro.Length < 4)
        //    //    return new List<CMPListasPreciosDetallesDTO>();

        //    return BaseDatos.ObtenerBaseDatos().ObtenerLista<CMPListasPreciosDetallesDTO>(sp, listaDetalle);
        //}

    }
}
