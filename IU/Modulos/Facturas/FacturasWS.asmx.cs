using Compras.Entidades;
using Comunes.LogicaNegocio;
using Facturas.Entidades;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace IU.Modulos.Facturas
{
    /// <summary>
    /// Descripción breve de FacturasWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class FacturasWS : System.Web.Services.WebService
    {

        [Serializable]
        public class ProgressBarDTO // as select2 is formed like id and text so we used DTO
        {
            public int number { get; set; }
            public string text { get; set; }
        }

        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        [WebMethod(EnableSession = true)]
        public List<ProgressBarDTO> ObetenerMensajes()
        {
            List<ProgressBarDTO> resultado = new List<ProgressBarDTO>();
            ProgressBarDTO item;
            int num = 0;
            object proc = HttpRuntime.Cache.Get(Session.SessionID + "CacheMensajes");
            List<string> MisMensajes = new List<string>();
            if (proc != null)
                MisMensajes = AyudaProgramacionLN.Clone<List<string>>((List<string>)proc);

            string conta = string.Empty;

            if (MisMensajes != null && MisMensajes.Count > 0)
            {
                if (MisMensajes.Exists(x => x.Contains("#CONTA#")))
                {
                    conta = MisMensajes.FindLast(x => x.Contains("#CONTA#"));
                    MisMensajes.RemoveAll(x => x.Contains("#CONTA#"));
                }

                foreach (string s in MisMensajes)
                {
                    item = new ProgressBarDTO();
                    item.number = s.Contains('|') ? int.TryParse(s.Split('|')[0], out num) ? num : 0 : 0;
                    item.text = s.Contains('|') ? s.Split('|')[1] : s;
                    resultado.Add(item);
                }
                if (conta.Length > 0)
                {
                    item = new ProgressBarDTO();
                    item.number = conta.Contains('|') ? int.TryParse(conta.Split('|')[0], out num) ? num : 0 : 0;
                    item.text = conta.Contains('|') ? conta.Split('|')[1] : conta;
                    item.text = item.text.Replace("#CONTA#", "");
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

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<VTAFacturasDetalles> FacturasLotesEnviadosFacturasDetallesSeleccionar(string IdFacturaLoteEnviadoFactura)
        {
            VTAFacturasLotesEnviadosFacturas filtro = new VTAFacturasLotesEnviadosFacturas();
            filtro.IdFacturaLoteEnviadoFactura = Convert.ToInt32(IdFacturaLoteEnviadoFactura);
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<VTAFacturasDetalles>("VTAFacturasLotesEnviadosFacturasDetallesSeleccionar", filtro);
        }


        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<CMPListasPreciosDetallesDTO> FacturasSeleccionarAjaxComboProductos(string value, string filtro, string cantidadCuotas, string idAfiliado, string idFilialPredeterminada, string idMoneda, string idUsuarioEvento, string idListaPrecio = null)
        {
            CMPListasPreciosDetalles listaDetalle = new CMPListasPreciosDetalles();
            int id = 0;
            int.TryParse(value, out id);
            string sp = "VTAFacturasSeleccionarAjaxComboProductos";
            listaDetalle.IdFilial = Convert.ToInt32(idFilialPredeterminada);
            listaDetalle.ListaPrecio.IdAfiliado = string.IsNullOrWhiteSpace(idAfiliado) ? 0 : Convert.ToInt32(idAfiliado);
            listaDetalle.Filtro = filtro;
            listaDetalle.IdMoneda = Convert.ToInt32(idMoneda);
            listaDetalle.CantidadCuotas = Convert.ToInt32(cantidadCuotas);
            listaDetalle.UsuarioLogueado.IdUsuarioEvento = Convert.ToInt32(idUsuarioEvento);
            listaDetalle.IdListaPrecioFiltro = string.IsNullOrEmpty(idListaPrecio) ? (Nullable<int>)null : Convert.ToInt32(idListaPrecio);
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CMPListasPreciosDetallesDTO>(sp, listaDetalle);
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<CMPListasPreciosDetallesDTO> RemitosSeleccionarAjaxComboProductosListaPrecio(string value, string filtro, string filialEntrega, string cantidadCuotas, string fecha)
        {
            CMPListasPreciosDetalles listaDetalle = new CMPListasPreciosDetalles();
            int id = 0;
            int.TryParse(value, out id);
            string sp = "VTARemitosSeleccionarAjaxComboProductosListaPrecio";
            //if (filialEntrega == "")
            //{
            //    listaDetalle.ValidarConfirmarMensajes("Debe seleccionar una Filial de Entrega");
            //    return new List<CMPListasPreciosDetallesDTO>();
            //}
            listaDetalle.IdFilial = Convert.ToInt32(filialEntrega);
            listaDetalle.NoIncluidoEnAcopio = true;
            listaDetalle.Producto.Descripcion = filtro;
            listaDetalle.CantidadCuotas = Convert.ToInt32(cantidadCuotas);
            string[] arrFecha = fecha != null ? fecha.Split('/') : null;
            if (arrFecha != null && arrFecha.Length > 2)
                listaDetalle.Fecha = new DateTime(Convert.ToInt32(arrFecha[2]), Convert.ToInt32(arrFecha[1]), Convert.ToInt32(arrFecha[0]));
            //int idVal = 0;
            //if (!Int32.TryParse(filtro, out idVal) && filtro.Length < 4)
            //    return new List<CMPListasPreciosDetallesDTO>();

            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CMPListasPreciosDetallesDTO>(sp, listaDetalle);
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<CMPProductosDTO> RemitosSeleccionarAjaxComboProductos(string value, string filtro, string filialEntrega)
        {
            CMPProductos cmpProductos = new CMPProductos();
            int id = 0;
            int.TryParse(value, out id);
            string sp = "VTARemitosSeleccionarAjaxComboProductos";
            //if (filialEntrega == "")
            //{
            //    return new List<CMPProductosDTO>();
            //}
            cmpProductos.IdFilial = Convert.ToInt32(filialEntrega);
            cmpProductos.Descripcion = filtro;
            cmpProductos.Venta = true;
            cmpProductos.Compra = false;
            //listaDetalle.ListaPrecio.IdAfiliado = ;
            //int idVal = 0;
            //if (!Int32.TryParse(filtro, out idVal) && filtro.Length < 4)
            //    return new List<CMPProductosDTO>();

            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CMPProductosDTO>(sp, cmpProductos);

        }
    }
}
