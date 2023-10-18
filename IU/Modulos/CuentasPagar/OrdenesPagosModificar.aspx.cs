using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuentasPagar.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.CuentasPagar
{
    public partial class OrdenesPagosModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.OrdenesDePagoDatosAceptar += new Controles.OrdenesPagosDatos.OrdenesDePagoDatosAceptarEventHandler(ModificarDatos_OrdenesDePagoDatosAceptar);
            this.ModificarDatos.OrdenesDePagoDatosCancelar += new Controles.OrdenesPagosDatos.OrdenesDePagoDatosCancelarEventHandler(ModificarDatos_OrdenesDePagoDatosCancelar);
            if (!this.IsPostBack)
            {
                CapOrdenesPagos parametro = new CapOrdenesPagos();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdOrdenPago"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/OrdenesPagosListar.aspx"), true);

                int valor = Convert.ToInt32(this.MisParametrosUrl["IdOrdenPago"]);
                parametro.IdOrdenPago = valor;

                //this.ViewState["UrlReferrer"] = Request.UrlReferrer;

                this.ModificarDatos.IniciarControl(parametro, Gestion.Modificar);
            }
        }

        void ModificarDatos_OrdenesDePagoDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/OrdenesPagosListar.aspx"), true);
        }

        void ModificarDatos_OrdenesDePagoDatosAceptar(global::CuentasPagar.Entidades.CapOrdenesPagos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/OrdenesPagosListar.aspx"), true);
        }
    }
}