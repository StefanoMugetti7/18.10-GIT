using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Contabilidad
{
    public partial class MonedasConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ConsultarDatos.MonedaDatosAceptar += new Controles.MonedasDatos.MonedaDatosAceptarEventHandler(ConsultarDatos_MonedaDatosAceptar);
            this.ConsultarDatos.MonedaDatosCancelar += new Controles.MonedasDatos.MonedaDatosCancelarEventHandler(ConsultarDatos_MonedaDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdMoneda"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/MonedasListar.aspx"), true);

                CtbMonedas moneda = new CtbMonedas();
                moneda.IdMoneda = Convert.ToInt32(this.MisParametrosUrl["IdMoneda"]);
                this.ConsultarDatos.IniciarControl(moneda, Gestion.Consultar);
            }
        }

        protected void ConsultarDatos_MonedaDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/MonedasListar.aspx"), true);
        }

        protected void ConsultarDatos_MonedaDatosAceptar(object sender, global::Contabilidad.Entidades.CtbMonedas e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/MonedasListar.aspx"), true);
        }
    }
}