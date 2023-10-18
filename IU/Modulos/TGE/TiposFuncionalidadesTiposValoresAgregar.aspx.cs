using Comunes.Entidades;
using System;
using Generales.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.TGE
{
    public partial class TiposFuncionalidadesTiposValoresAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            ModificarDatos.TiposFuncionalidadesTiposValoresAceptar += new Control.TiposFuncionalidadesTiposValores.TiposFuncionalidadesTiposValoresAceptarEventHandler(ModificarDatos_TiposOperacionesModulosFuncionalidadesDatosAceptar);
            ModificarDatos.TiposFuncionalidadesTiposValoresCancelar += new Control.TiposFuncionalidadesTiposValores.TiposFuncionalidadesTiposValoresCancelarEventHandler(ModificarDatos_TiposOperacionesModulosFuncionalidadesDatosCancelar);
            if (!IsPostBack)
            {
                ModificarDatos.IniciarControl(new TGETiposFuncionalidadesTiposValores(), Gestion.Agregar);
            }
        }

        private void ModificarDatos_TiposOperacionesModulosFuncionalidadesDatosAceptar(object sender, TGETiposFuncionalidadesTiposValores e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposFuncionalidadesTiposValoresListar.aspx"), true);
        }

        private void ModificarDatos_TiposOperacionesModulosFuncionalidadesDatosCancelar()
        {

            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposFuncionalidadesTiposValoresListar.aspx"), true);
        }
    }
}