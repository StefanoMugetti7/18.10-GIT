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
using Comunes.Entidades;
using Ahorros.Entidades;

namespace IU.Modulos.Ahorros
{
    public partial class PlazosModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.PlazosDatosAceptar += new IU.Modulos.Ahorros.Controles.PlazosDatos.PlazosDatosAceptarEventHandler(AgregarDatos_PlazosDatosAceptar);
            this.ModificarDatos.PlazosDatosCancelar += new IU.Modulos.Ahorros.Controles.PlazosDatos.PlazosDatosCancelarEventHandler(AgregarDatos_PlazosDatosCancelar);
            if (!this.IsPostBack)
            {
                if (!this.MisParametrosUrl.Contains("IdPlazo"))
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosListar.aspx"), true);
                AhoPlazos plazos = new AhoPlazos();
                plazos.IdPlazos = Convert.ToInt32(this.MisParametrosUrl["IdPlazo"]);
                this.ModificarDatos.IniciarControl(plazos, Gestion.Modificar);
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