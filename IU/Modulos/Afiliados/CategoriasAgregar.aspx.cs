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
using Afiliados.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Afiliados
{
    public partial class CategoriasAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.AgregarDatos.CategoriaAfiliadoModificarDatosAceptar += new IU.Modulos.Afiliados.Controles.CategoriasModificarDatos.CategoriaAfiliadoDatosAceptarEventHandler(AgregarDatos_CategoriaAfiliadoModificarDatosAceptar);
            this.AgregarDatos.CategoriaAfiliadoModificarDatosCancelar += new IU.Modulos.Afiliados.Controles.CategoriasModificarDatos.CategoriaAfiliadoDatosCancelarEventHandler(AgregarDatos_CategoriaAfiliadoModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.AgregarDatos.IniciarControl(new AfiCategorias(), Gestion.Agregar);
            }
        }

        protected void AgregarDatos_CategoriaAfiliadoModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/CategoriasListar.aspx"), true);
        }

        protected void AgregarDatos_CategoriaAfiliadoModificarDatosAceptar(object sender, global::Afiliados.Entidades.AfiCategorias e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/CategoriasListar.aspx"), true);
        }
    }
}
