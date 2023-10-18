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
    public partial class PlazosFijosCancelar : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            this.CancelarDatos.PlazosFijosDatosAceptar += new IU.Modulos.Ahorros.Controles.PlazosFijosDatos.PlazosFijosDatosAceptarEventHandler(CancelarDatos_PlazosFijosDatosAceptar);
            this.CancelarDatos.PlazosFijosDatosCancelar += new IU.Modulos.Ahorros.Controles.PlazosFijosDatos.PlazosFijosDatosCancelarEventHandler(CancelarDatos_PlazosFijosDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdPlazoFijo"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosFijosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdPlazoFijo"]);
                AhoPlazosFijos plazosFijos = new AhoPlazosFijos();
                plazosFijos.IdPlazoFijo = parametro;
                plazosFijos.IdAfiliado = this.MiAfiliado.IdAfiliado;
                this.CancelarDatos.IniciarControl(plazosFijos, Gestion.Cancelar);
            }
        }

        protected void CancelarDatos_PlazosFijosDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosFijosListar.aspx"), true);
        }

        protected void CancelarDatos_PlazosFijosDatosAceptar(object sender, global::Ahorros.Entidades.AhoPlazosFijos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosFijosListar.aspx"), true);
        }

    }
}