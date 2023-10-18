using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Contabilidad.Entidades;

namespace IU.Modulos.Contabilidad
{
    public partial class CapitulosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.AgregarDatos.CapituloDatosAceptar += new Controles.CapitulosDatos.CapituloDatosAceptarEventHandler(AgregarDatos_CapituloDatosAceptar);
            this.AgregarDatos.CapituloDatosCancelar += new Controles.CapitulosDatos.CapituloDatosCancelarEventHandler(AgregarDatos_CapituloDatosCancelar);
            if (!this.IsPostBack)
            {
                this.AgregarDatos.IniciarControl(new CtbCapitulos(), Gestion.Agregar);
            }
        }

        protected void AgregarDatos_CapituloDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CapitulosListar.aspx"), true);
        }

        protected void AgregarDatos_CapituloDatosAceptar(object sender, CtbCapitulos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CapitulosListar.aspx"), true);
        }
    }
}