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
    public partial class CategoriasConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ConsultarDatos.CategoriaAfiliadoModificarDatosCancelar += new IU.Modulos.Afiliados.Controles.CategoriasModificarDatos.CategoriaAfiliadoDatosCancelarEventHandler(ConsultarDatos_CategoriaAfiliadoModificarDatosCancelar);
            this.ConsultarDatos.CategoriaAfiliadoModificarDatosAceptar += new IU.Modulos.Afiliados.Controles.CategoriasModificarDatos.CategoriaAfiliadoDatosAceptarEventHandler(ConsultarDatos_CategoriaAfiliadoModificarDatosAceptar);
            if (!this.IsPostBack)
            {
                if(!this.MisParametrosUrl.Contains("IdCategoria"))
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/CategoriasListar.aspx"), true);
                AfiCategorias categoria = new AfiCategorias();
                categoria.IdCategoria = Convert.ToInt32(this.MisParametrosUrl["IdCategoria"]);
                this.ConsultarDatos.IniciarControl(categoria, Gestion.Consultar);
            }
        }

        protected void ConsultarDatos_CategoriaAfiliadoModificarDatosAceptar(object sender, global::Afiliados.Entidades.AfiCategorias e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/CategoriasListar.aspx"), true);
        }

        protected void ConsultarDatos_CategoriaAfiliadoModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/CategoriasListar.aspx"), true);
        }
    }
}
