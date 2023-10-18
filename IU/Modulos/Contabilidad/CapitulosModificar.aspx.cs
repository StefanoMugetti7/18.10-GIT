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
    public partial class CapitulosModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.CapituloDatosAceptar += new Controles.CapitulosDatos.CapituloDatosAceptarEventHandler(ModificarDatos_CapituloDatosAceptar);
            this.ModificarDatos.CapituloDatosCancelar += new Controles.CapitulosDatos.CapituloDatosCancelarEventHandler(ModificarDatos_CapituloDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdCapitulo"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CapitulosListar.aspx"), true);

                CtbCapitulos capitulo = new CtbCapitulos();
                capitulo.IdCapitulo = Convert.ToInt32(this.MisParametrosUrl["IdCapitulo"]);
                this.ModificarDatos.IniciarControl(capitulo, Gestion.Modificar);
            }
        }

        protected void ModificarDatos_CapituloDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CapitulosListar.aspx"), true);
        }

        protected void ModificarDatos_CapituloDatosAceptar(object sender, global::Contabilidad.Entidades.CtbCapitulos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CapitulosListar.aspx"), true);
        }
    }
}