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
    public partial class CapitulosConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ConsultarDatos.CapituloDatosAceptar += new Controles.CapitulosDatos.CapituloDatosAceptarEventHandler(ConsultarDatos_CapituloDatosAceptar);
            this.ConsultarDatos.CapituloDatosCancelar += new Controles.CapitulosDatos.CapituloDatosCancelarEventHandler(ConsultarDatos_CapituloDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdCapitulo"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CapitulosListar.aspx"), true);

                CtbCapitulos capitulo = new CtbCapitulos();
                capitulo.IdCapitulo = Convert.ToInt32(this.MisParametrosUrl["IdCapitulo"]);
                this.ConsultarDatos.IniciarControl(capitulo, Gestion.Consultar);
            }
        }

        protected void ConsultarDatos_CapituloDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CapitulosListar.aspx"), true);
        }

        protected void ConsultarDatos_CapituloDatosAceptar(object sender, global::Contabilidad.Entidades.CtbCapitulos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CapitulosListar.aspx"), true);
        }
    }
}