using Compras.Entidades;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Comunes.Entidades;
using System.Reflection;
using Wordpress;

namespace IU.Modulos.Wordpress
{
    public partial class Integracion : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }

        protected async void btnAgregarProductos(object sender, EventArgs e)
        {
            Objeto resultado = new Objeto();
            int cantidadProductos = await WordpressClass.AgregarVariosProductos(resultado);
            if(cantidadProductos > 0)
            {
                lblResultadoProducto.Text = "Se han cargado " + resultado.Link + " productos y actualizado " + resultado.Filtro + ".";
            }
            else
            {
                lblResultadoProducto.Text = "Error al cargar productos";
            }
            
            MostrarMensaje(resultado.CodigoMensaje, false);
        }

        protected async void btnImportarCompras(object sender, EventArgs e)
        {
            Objeto obj = new Objeto();
            obj.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            bool resultado = await WordpressClass.ImportarPedidos(obj);

            lblResultadoCompras.Text = "Se han importado " + obj.HashTransaccion + " compras";

            MostrarMensaje(obj.CodigoMensaje, false);
        }
    }
}