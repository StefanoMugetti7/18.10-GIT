using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cobros.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Cobros
{
    public partial class OrdenesCobrosFacturasAnularConfirmada : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.AnularDatos.OrdenesDeCobroDatosAceptar += new Controles.OrdenesCobrosFacturasDatos.OrdenesDeCobroDatosAceptarEventHandler(AnularDatos_OrdenesDeCobroDatosAceptar);
            this.AnularDatos.OrdenesDeCobroDatosCancelar += new Controles.OrdenesCobrosFacturasDatos.OrdenesDeCobroDatosCancelarEventHandler(AnularDatos_OrdenesDeCobroDatosCancelar);
            if (!this.IsPostBack)
            {
                CobOrdenesCobros parametro = new CobOrdenesCobros();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdOrdenCobro"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosFacturasListar.aspx"), true);

                int valor = Convert.ToInt32(this.MisParametrosUrl["IdOrdenCobro"]);
                parametro.IdOrdenCobro = valor;
                this.AnularDatos.IniciarControl(parametro, Gestion.AnularConfirmar);
            }
        }

        protected void AnularDatos_OrdenesDeCobroDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosFacturasListar.aspx"), true);
        }

        protected void AnularDatos_OrdenesDeCobroDatosAceptar(global::Cobros.Entidades.CobOrdenesCobros e)
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosFacturasListar.aspx"), true);
        }
    }
}