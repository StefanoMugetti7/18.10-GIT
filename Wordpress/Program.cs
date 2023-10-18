using Compras.Entidades;
using Comunes.Entidades;
using Facturas.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.v3;

namespace Wordpress
{
    public class WordpressClass
    {
        public static async Task<int> AgregarVariosProductos(Objeto obj)
        {
            WCObject wc = ObtenerConexion();

            DataTable productos = BaseDatos.ObtenerBaseDatos().ObtenerLista("WordpressCmpProductosSeleccionar", obj);

            ProductBatch pb = new ProductBatch();
            ProductCategoryBatch cb = new ProductCategoryBatch();

            List<Product> create = new List<Product>();
            List<Product> update = new List<Product>();

            List<ProductCategory> categoriasWC = new List<ProductCategory>();
            List<ProductCategory> categoriasSIM = BaseDatos.ObtenerBaseDatos().ObtenerLista<ProductCategory>("WordpressCmpProductosCategorias", obj);

            int aumento = 100;
            int page = 1;

            while (page != 0)
            {
                List<ProductCategory> newCategorias = await wc.Category.GetAll(new Dictionary<string, string>() { { "per_page", aumento.ToString() }, { "order", "asc" }, { "page", page.ToString() } });

                if (newCategorias.Count > 0)
                {
                    categoriasWC.AddRange(newCategorias);
                    page++;
                }
                else
                {
                    page = 0;
                }
            }

            List<ProductCategory> categoriasNuevas = categoriasSIM.Where(p => !categoriasWC.Any(p2 => p2.slug == p.slug)).ToList();
            categoriasNuevas.RemoveAll(x => x.display == "0");
            List<ProductCategory> categoriasSIMActualizar = categoriasSIM.Where(p => categoriasWC.Any(p2 => p2.slug == p.slug)).ToList();
            List<ProductCategory> categoryUpdate = new List<ProductCategory>();
            List<int> categoryDelete = new List<int>();
            ProductCategory filter;
;            foreach (ProductCategory catSim in categoriasSIMActualizar)
            {
                filter = categoriasWC.FirstOrDefault(p2 => p2.slug == catSim.slug);
                if (catSim.display == "0")
                    categoryDelete.Add(Convert.ToInt32(filter.id));
                else if (catSim.name != filter.name)
                    categoryUpdate.Add(catSim);
            }

            cb.create = categoriasNuevas;
            cb.update = categoryUpdate;
            cb.delete = categoryDelete;
            await wc.Category.UpdateRange(cb);

            if (categoriasNuevas.Count > 0)
            {
                categoriasWC.Clear();

                page = 1;

                while (page != 0)
                {
                    List<ProductCategory> newCategorias = await wc.Category.GetAll(new Dictionary<string, string>() { { "per_page", aumento.ToString() }, { "order", "asc" }, { "page", page.ToString() } });

                    if (newCategorias.Count > 0)
                    {
                        categoriasWC.AddRange(newCategorias);
                        page++;
                    }
                    else
                    {
                        page = 0;
                    }
                }
            }

            foreach (DataRow prod in productos.Rows)
            {
                Product p = new Product();

                p.name = prod["Descripcion"].ToString();
                p.regular_price = prod["Precio"] != DBNull.Value ? Convert.ToDecimal(prod["Precio"]) : 0;
                p.status = "publish";
                p.sku = prod["IdProducto"].ToString();
                
                CMPProductos x = new CMPProductos();
                x.IdProducto = (int)prod["IdProducto"];                

                p.categories = BaseDatos.ObtenerBaseDatos().ObtenerLista<ProductCategoryLine>("WordpressCmpProductosSeleccionarCategorias", x);

                foreach(ProductCategoryLine pcl in p.categories)
                {
                    foreach (ProductCategory pc in categoriasWC)
                    {
                        if(pcl.slug == pc.slug)
                        {
                            pcl.id = pc.id;
                            break;
                        }
                    }
                }

                p.manage_stock = prod["ManageStock"] != DBNull.Value ? Convert.ToBoolean(prod["ManageStock"]) : default(bool?);
                p.stock_quantity = prod["StockQuantity"] != DBNull.Value ? Convert.ToInt32(prod["StockQuantity"]) : default(int?);

                if (prod["IdWooCommerce"].ToString() == "")
                {
                    create.Add(p);
                }
                else
                {
                    p.id = (uint?)Convert.ToInt32(prod["IdWooCommerce"].ToString());
                    if (!(Convert.ToBoolean(prod["Valor"])))
                    {
                        p.status = "draft";
                    }

                    update.Add(p);
                }
            }

            obj.Link = create.Count.ToString();
            obj.Filtro = update.Count.ToString();

            
            pb.create = create;
            pb.update = update;

            List<List<Product>> auxCreate = SplitList(pb.create);
            int i = 0;
            foreach (var item in auxCreate)
            {
                ProductBatch aux = new ProductBatch();
                aux.create = auxCreate[i];
                await wc.Product.UpdateRange(aux);
                i++;
            }

            List<List<Product>> auxUpdate= SplitList(pb.update);
            i = 0;
            foreach (var item in auxUpdate)
            {
                ProductBatch aux = new ProductBatch();
                aux.update = auxUpdate[i];
                await wc.Product.UpdateRange(aux);
                i++;
            }

            //await wc.Product.UpdateRange(pb);


            List<Product> productos2 = new List<Product>();

            page = 1;

            while (page != 0)
            {
                List<Product> newProductos = await wc.Product.GetAll(new Dictionary<string, string>() { { "per_page", aumento.ToString() }, { "order", "asc" }, { "page", page.ToString() } });

                 if (newProductos.Count > 0)
                {
                    productos2.AddRange(newProductos);
                    page++;
                }
                else
                {
                    page = 0;
                }
            }
            
            CMPProductos param = new CMPProductos();

            param.LoteCamposValores = new XmlDocument();

            XmlNode items = param.LoteCamposValores.CreateElement("Productos");
            param.LoteCamposValores.AppendChild(items);

            XmlNode itemNodo;
            XmlNode ValorNode;

            foreach (Product item in productos2)
            {
                itemNodo = param.LoteCamposValores.CreateElement("Producto");

                ValorNode = param.LoteCamposValores.CreateElement("IdWooCommerce");
                ValorNode.InnerText = item.id.ToString();
                itemNodo.AppendChild(ValorNode);

                ValorNode = param.LoteCamposValores.CreateElement("IdProducto");
                ValorNode.InnerText = item.sku;
                itemNodo.AppendChild(ValorNode);

                items.AppendChild(itemNodo);
            }

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    var resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(param, bd, tran, "WordpressCmpProductosActualizar");
                    if (resultado)
                    {
                        tran.Commit();
                        obj.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    obj.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    obj.CodigoMensajeArgs.Add(ex.Message);
                }
            }
            return productos2.Count();
        }

        public static List<List<Product>> SplitList(List<Product> locations, int nSize = 50)
        {
            var list = new List<List<Product>>();

            for (int i = 0; i < locations.Count; i += nSize)
            {
                list.Add(locations.GetRange(i, Math.Min(nSize, locations.Count - i)));
            }

            return list;
        }
        public static async Task<bool> ImportarPedidos(Objeto obj)
        {
            WCObject wc = ObtenerConexion();

            int aumento = 100;
            bool resultado = true;

            List<Order> ordenes = new List<Order>();

            DataTable ordenesEnBase = BaseDatos.ObtenerBaseDatos().ObtenerLista("WordpressVTANotasPedidosSeleccionar", obj);

            int page = 1;

            while (page != 0)
            {
                List<Order> newOrder = await wc.Order.GetAll(new Dictionary<string, string>() { { "per_page", aumento.ToString() }, { "order", "asc" }, { "page", page.ToString() }, {"status", "completed" } });

                if (newOrder.Count > 0)
                {
                    ordenes.AddRange(newOrder);
                    page++;
                }
                else
                {
                    page = 0;
                }
            }

            VTANotasPedidos param = new VTANotasPedidos();

            param.LoteCamposValores = new XmlDocument();

            XmlNode notas = param.LoteCamposValores.CreateElement("Notas");
            param.LoteCamposValores.AppendChild(notas);

            XmlNode nota;
            XmlNode atributoNota;

            XmlNode notasDetalles;

            XmlNode notaDetalle;
            XmlNode atributoNotaDetalle;
            int idAuxiliar = 0;

            foreach (Order x in ordenes)
            {
                CMPProductos p = new CMPProductos();

                idAuxiliar++;
                string descripcion = "Pago a traves de " + x.payment_method_title + ". Pagado el " + x.date_completed.ToString() + ". IP del cliente: " + x.customer_ip_address;
                nota = param.LoteCamposValores.CreateElement("Nota");

                atributoNota = param.LoteCamposValores.CreateElement("IdWordpress");
                atributoNota.InnerText = x.customer_id.ToString();
                nota.AppendChild(atributoNota);

                atributoNota = param.LoteCamposValores.CreateElement("Descripcion");
                atributoNota.InnerText = descripcion;
                nota.AppendChild(atributoNota);

                atributoNota = param.LoteCamposValores.CreateElement("FechaAlta");
                atributoNota.InnerText = x.date_completed.ToString();
                nota.AppendChild(atributoNota);

                atributoNota = param.LoteCamposValores.CreateElement("ImporteTotal");
                atributoNota.InnerText = x.total.ToString();
                nota.AppendChild(atributoNota);

                atributoNota = param.LoteCamposValores.CreateElement("DomicilioEntrega");
                atributoNota.InnerText = x.billing.address_1.ToString();
                nota.AppendChild(atributoNota);

                atributoNota = param.LoteCamposValores.CreateElement("IdAuxiliar");
                atributoNota.InnerText = idAuxiliar.ToString();
                nota.AppendChild(atributoNota);

                atributoNotaDetalle = param.LoteCamposValores.CreateElement("IdWooCommerce");
                atributoNotaDetalle.InnerText = x.id.ToString();
                nota.AppendChild(atributoNotaDetalle);

                notasDetalles = param.LoteCamposValores.CreateElement("NotaDetalles");

                foreach (WooCommerceNET.WooCommerce.v2.OrderLineItem itemDetalles in x.line_items)
                {
                    notaDetalle = param.LoteCamposValores.CreateElement("NotaDetalle");

                    atributoNotaDetalle = param.LoteCamposValores.CreateElement("IdListaPrecioDetalle");
                    atributoNotaDetalle.InnerText = idAuxiliar.ToString();
                    notaDetalle.AppendChild(atributoNotaDetalle);

                    atributoNotaDetalle = param.LoteCamposValores.CreateElement("IdProductoWooCommerce");
                    atributoNotaDetalle.InnerText = itemDetalles.product_id.ToString();
                    notaDetalle.AppendChild(atributoNotaDetalle);

                    atributoNotaDetalle = param.LoteCamposValores.CreateElement("DescripcionProducto");
                    atributoNotaDetalle.InnerText = itemDetalles.name.ToString();
                    notaDetalle.AppendChild(atributoNotaDetalle);

                    atributoNotaDetalle = param.LoteCamposValores.CreateElement("Cantidad");
                    atributoNotaDetalle.InnerText = itemDetalles.quantity.ToString();
                    notaDetalle.AppendChild(atributoNotaDetalle);

                    atributoNotaDetalle = param.LoteCamposValores.CreateElement("IdAuxiliar");
                    atributoNotaDetalle.InnerText = idAuxiliar.ToString();
                    notaDetalle.AppendChild(atributoNotaDetalle);

                    notasDetalles.AppendChild(notaDetalle);
                }

                nota.AppendChild(notasDetalles);
                notas.AppendChild(nota);
            }

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(param, bd, tran, "WordpressVTANotasPedidosInsertar");
                    if (resultado)
                    {
                        tran.Commit();
                        obj.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    obj.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    obj.CodigoMensajeArgs.Add(ex.Message);
                    resultado = false;
                }
            }
            obj.HashTransaccion = ordenes.Count;
            
            return resultado;
        }

        public static WCObject ObtenerConexion()
        {
            string keyPublica = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.WooCommerceClavePublica).ParametroValor;
            string keyPrivada = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.WooCommerceClavePrivada).ParametroValor;
            string url = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.WordPressUrl).ParametroValor + "/wp-json/wc/v3/";
            //url = string.Concat(url, "?consumer_key=", keyPublica, "&consumer_secret=", keyPrivada);
            RestAPI rest = new RestAPI(url, keyPublica, keyPrivada);
            rest.WCAuthWithJWT = false;
            WCObject wc = new WCObject(rest);

            return wc;
        }
    }
}
