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
    public partial class TiposOperacionesModulosFuncionalidadesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            ModificarDatos.TiposOperacionesModulosFuncionalidadesAceptar += new Control.TiposOperacionesModulosFuncionalidades.TiposOperacionesModulosFuncionalidadesAceptarEventHandler(ModificarDatos_TiposOperacionesModulosFuncionalidadesDatosAceptar);
            ModificarDatos.TiposOperacionesModulosFuncionalidadesCancelar += new Control.TiposOperacionesModulosFuncionalidades.TiposOperacionesModulosFuncionalidadesCancelarEventHandler(ModificarDatos_TiposOperacionesModulosFuncionalidadesDatosCancelar);
            if (!IsPostBack)
            {
                ModificarDatos.IniciarControl(new TGETiposOperacionesModulosFuncionalidades(), Gestion.Agregar);
            }
        }

        private void ModificarDatos_TiposOperacionesModulosFuncionalidadesDatosAceptar(object sender, TGETiposOperacionesModulosFuncionalidades e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposOperacionesModulosFuncionalidadesListar.aspx"), true);
        }

        private void ModificarDatos_TiposOperacionesModulosFuncionalidadesDatosCancelar()
        {
        
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposOperacionesModulosFuncionalidadesListar.aspx"), true);
        }
    }
}