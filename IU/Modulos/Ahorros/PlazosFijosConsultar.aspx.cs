using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Ahorros.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Ahorros
{
    public partial class PlazosFijosConsultar : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            this.ConsultarDatos.PlazosFijosDatosAceptar += new IU.Modulos.Ahorros.Controles.PlazosFijosDatos.PlazosFijosDatosAceptarEventHandler(ConsultarDatos_PlazosFijosDatosAceptar);
            this.ConsultarDatos.PlazosFijosDatosCancelar += new IU.Modulos.Ahorros.Controles.PlazosFijosDatos.PlazosFijosDatosCancelarEventHandler(ConsultarDatos_PlazosFijosDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdPlazoFijo"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosFijosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdPlazoFijo"]);
                AhoPlazosFijos plazosFijos = new AhoPlazosFijos();
                plazosFijos.IdPlazoFijo = parametro;
                plazosFijos.IdAfiliado = this.MiAfiliado.IdAfiliado;
                this.ConsultarDatos.IniciarControl(plazosFijos, Gestion.Consultar);
            }
        }

        protected void ConsultarDatos_PlazosFijosDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosFijosListar.aspx"), true);
        }

        protected void ConsultarDatos_PlazosFijosDatosAceptar(object sender, global::Ahorros.Entidades.AhoPlazosFijos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosFijosListar.aspx"), true);
        }

    }
}