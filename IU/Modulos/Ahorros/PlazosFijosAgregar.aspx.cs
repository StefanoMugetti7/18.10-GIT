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
    public partial class PlazosFijosAgregar : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            this.AgregarDatos.PlazosFijosDatosAceptar += new IU.Modulos.Ahorros.Controles.PlazosFijosDatos.PlazosFijosDatosAceptarEventHandler(AgregarDatos_PlazosFijosDatosAceptar);
            this.AgregarDatos.PlazosFijosDatosCancelar += new IU.Modulos.Ahorros.Controles.PlazosFijosDatos.PlazosFijosDatosCancelarEventHandler(AgregarDatos_PlazosFijosDatosCancelar);
            if (!this.IsPostBack)
            {
                AhoPlazosFijos plazosFijos = new AhoPlazosFijos();
                plazosFijos.IdAfiliado = this.MiAfiliado.IdAfiliado;
                this.AgregarDatos.IniciarControl(plazosFijos, Gestion.Agregar);
            }
        }

        protected void AgregarDatos_PlazosFijosDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosFijosListar.aspx"), true);
        }

        protected void AgregarDatos_PlazosFijosDatosAceptar(object sender, global::Ahorros.Entidades.AhoPlazosFijos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosFijosListar.aspx"), true);
        }

    }
}