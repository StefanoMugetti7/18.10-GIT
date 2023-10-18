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
    public partial class OrdenesCobrosFacturasConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.OrdenesDeCobroDatosAceptar += new Controles.OrdenesCobrosFacturasDatos.OrdenesDeCobroDatosAceptarEventHandler(ModificarDatos_OrdenesDeCobroDatosAceptar);
            this.ModificarDatos.OrdenesDeCobroDatosCancelar += new Controles.OrdenesCobrosFacturasDatos.OrdenesDeCobroDatosCancelarEventHandler(ModificarDatos_OrdenesDeCobroDatosCancelar);
            if (!this.IsPostBack)
            {
                CobOrdenesCobros parametro = new CobOrdenesCobros();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdOrdenCobro"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosFacturasListar.aspx"), true);

                int valor = Convert.ToInt32(this.MisParametrosUrl["IdOrdenCobro"]);
                parametro.IdOrdenCobro = valor;

                this.ModificarDatos.IniciarControl(parametro, Gestion.Consultar);
            }
        }

        protected void ModificarDatos_OrdenesDeCobroDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosFacturasListar.aspx"), true);
        }

        protected void ModificarDatos_OrdenesDeCobroDatosAceptar(global::Cobros.Entidades.CobOrdenesCobros e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosFacturasListar.aspx"), true);
        }
    }
}