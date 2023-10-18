using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facturas.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Facturas
{
    public partial class NotasPedidosModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ModificarDatos.RemitosModificarDatosAceptar += new Controles.RemitosDatos.RemitosDatosAceptarEventHandler(ModificarDatos_RemitoModificarDatosAceptar);
            this.ModificarDatos.NotasPedidosModificarDatosCancelar += new Controles.NotasPedidosDatos.NotasPedidosDatosEventHandler(ModificarDatos_RemitoModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                VTANotasPedidos NotaPedido = new VTANotasPedidos();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdNotaPedido"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/NotasPedidosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdNotaPedido"]);
                NotaPedido.IdNotaPedido = parametro;

                this.ModificarDatos.IniciarControl(NotaPedido, Gestion.Modificar);
            }
        }

        void ModificarDatos_RemitoModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/RemitosListar.aspx"), true);
        }
    }
}