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
    public partial class MonedasAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.AgregarDatos.MonedaDatosAceptar += new Controles.MonedasDatos.MonedaDatosAceptarEventHandler(AgregarDatos_MonedaDatosAceptar);
            this.AgregarDatos.MonedaDatosCancelar += new Controles.MonedasDatos.MonedaDatosCancelarEventHandler(AgregarDatos_MonedaDatosCancelar);
            if (!this.IsPostBack)
            {
                this.AgregarDatos.IniciarControl(new CtbMonedas(), Gestion.Agregar);
            }
        }

        protected void AgregarDatos_MonedaDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/MonedasListar.aspx"), true);
        }

        protected void AgregarDatos_MonedaDatosAceptar(object sender, global::Contabilidad.Entidades.CtbMonedas e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/MonedasListar.aspx"), true);
        }
    }
}