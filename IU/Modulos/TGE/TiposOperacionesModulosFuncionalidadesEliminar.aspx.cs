using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.TGE
{
    public partial class TiposOperacionesModulosFuncionalidadesEliminar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            ModificarDatos.TiposOperacionesModulosFuncionalidadesAceptar += new Control.TiposOperacionesModulosFuncionalidades.TiposOperacionesModulosFuncionalidadesAceptarEventHandler(ModificarDatos_TiposOperacionesModulosFuncionalidadesDatosAceptar);
            ModificarDatos.TiposOperacionesModulosFuncionalidadesCancelar += new Control.TiposOperacionesModulosFuncionalidades.TiposOperacionesModulosFuncionalidadesCancelarEventHandler(ModificarDatos_TiposOperacionesModulosFuncionalidadesDatosCancelar);
            if (!IsPostBack)
            {
                TGETiposOperacionesModulosFuncionalidades modulo = new TGETiposOperacionesModulosFuncionalidades();
                if (!MisParametrosUrl.Contains("IdTipoOperacion"))
                {
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposOperacionesModulosFuncionalidadesListar.aspx"), true);
                }
                int IdTipoOperacion = Convert.ToInt32(MisParametrosUrl["IdTipoOperacion"]);
                int IdModuloSistema = Convert.ToInt32(MisParametrosUrl["IdModuloSistema"]);
                int IdTipoFuncionalidad = Convert.ToInt32(MisParametrosUrl["IdTipoFuncionalidad"]);
                modulo.IdTipoOperacion = IdTipoOperacion;
                modulo.IdModuloSistema = IdModuloSistema;
                modulo.IdTipoFuncionalidad = IdTipoFuncionalidad;
                ModificarDatos.IniciarControl(modulo, Gestion.Anular);
            }
        }

        private void ModificarDatos_TiposOperacionesModulosFuncionalidadesDatosAceptar(object sender, TGETiposOperacionesModulosFuncionalidades e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposOperacionesModulosFuncionalidadesListar.aspx"), true);
        }

        private void ModificarDatos_TiposOperacionesModulosFuncionalidadesDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposOperacionesModulosFuncionalidadesListar.aspx"), true);
        }
    }
}