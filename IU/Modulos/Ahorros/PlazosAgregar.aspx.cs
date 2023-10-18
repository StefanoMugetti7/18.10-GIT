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
    public partial class PlazosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.AgregarDatos.PlazosDatosAceptar += new IU.Modulos.Ahorros.Controles.PlazosDatos.PlazosDatosAceptarEventHandler(AgregarDatos_PlazosDatosAceptar);
            this.AgregarDatos.PlazosDatosCancelar += new IU.Modulos.Ahorros.Controles.PlazosDatos.PlazosDatosCancelarEventHandler(AgregarDatos_PlazosDatosCancelar);
            if (!this.IsPostBack)
            {
                this.AgregarDatos.IniciarControl(new AhoPlazos(), Gestion.Agregar);
            }
        }

        protected void AgregarDatos_PlazosDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosListar.aspx"), true);
        }

        protected void AgregarDatos_PlazosDatosAceptar(object sender, global::Ahorros.Entidades.AhoPlazos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosListar.aspx"), true);
        }
    }
}