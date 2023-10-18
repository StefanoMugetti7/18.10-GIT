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
    public partial class MonedasModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.MonedaDatosAceptar += new Controles.MonedasDatos.MonedaDatosAceptarEventHandler(ModificarDatos_MonedaDatosAceptar);
            this.ModificarDatos.MonedaDatosCancelar += new Controles.MonedasDatos.MonedaDatosCancelarEventHandler(ModificarDatos_MonedaDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdMoneda"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/MonedasListar.aspx"), true);

                CtbMonedas moneda = new CtbMonedas();
                moneda.IdMoneda = Convert.ToInt32(this.MisParametrosUrl["IdMoneda"]);
                this.ModificarDatos.IniciarControl(moneda, Gestion.Modificar);
            }
        }

        protected void ModificarDatos_MonedaDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/MonedasListar.aspx"), true);
        }

        protected void ModificarDatos_MonedaDatosAceptar(object sender, global::Contabilidad.Entidades.CtbMonedas e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/MonedasListar.aspx"), true);
        }
    }
}