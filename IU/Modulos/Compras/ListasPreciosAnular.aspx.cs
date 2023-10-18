using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Compras
{
    public partial class ListasPreciosAnular : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ModificarDatosAceptar += new Controles.ListasPreciosDatos.ModificarDatosAceptarEventHandler(ModificarDatos_ModificarDatosAceptar);
            this.ModificarDatos.ModificarDatosCancelar += new Controles.ListasPreciosDatos.ModificarDatosCancelarEventHandler(ModificarDatos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                CMPListasPrecios parametro = new CMPListasPrecios();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdListaPrecio"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/ListasPreciosListar.aspx"), true);

                int valor = Convert.ToInt32(this.MisParametrosUrl["IdListaPrecio"]);
                parametro.IdListaPrecio = valor;

                this.ModificarDatos.IniciarControl(parametro, Gestion.Anular);
            }
        }

        void ModificarDatos_ModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/ListasPreciosListar.aspx"), true);
        }

        void ModificarDatos_ModificarDatosAceptar(object sender, global::Compras.Entidades.CMPListasPrecios e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/ListasPreciosListar.aspx"), true);
        }
    }
}