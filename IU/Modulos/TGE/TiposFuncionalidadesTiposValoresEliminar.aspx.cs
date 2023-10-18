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
    public partial class TiposFuncionalidadesTiposValoresEliminar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            ModificarDatos.TiposFuncionalidadesTiposValoresAceptar += new Control.TiposFuncionalidadesTiposValores.TiposFuncionalidadesTiposValoresAceptarEventHandler(ModificarDatos_TiposOperacionesModulosFuncionalidadesDatosAceptar);
            ModificarDatos.TiposFuncionalidadesTiposValoresCancelar += new Control.TiposFuncionalidadesTiposValores.TiposFuncionalidadesTiposValoresCancelarEventHandler(ModificarDatos_TiposOperacionesModulosFuncionalidadesDatosCancelar);
            if (!IsPostBack)
            {
                TGETiposFuncionalidadesTiposValores modulo = new TGETiposFuncionalidadesTiposValores();
                if (!MisParametrosUrl.Contains("IdTipoValor"))
                {
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposFuncionalidadesTiposValoresListar.aspx"), true);
                }
                int IdTipoValor = Convert.ToInt32(MisParametrosUrl["IdTipoValor"]);
                int IdTipoFuncionalidad = Convert.ToInt32(MisParametrosUrl["IdTipoFuncionalidad"]);
                modulo.IdTipoValor = IdTipoValor;
                modulo.IdTipoFuncionalidad = IdTipoFuncionalidad;
                ModificarDatos.IniciarControl(modulo, Gestion.Anular);
            }
        }

        private void ModificarDatos_TiposOperacionesModulosFuncionalidadesDatosAceptar(object sender, TGETiposFuncionalidadesTiposValores e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposFuncionalidadesTiposValoresListar.aspx"), true);
        }

        private void ModificarDatos_TiposOperacionesModulosFuncionalidadesDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposFuncionalidadesTiposValoresListar.aspx"), true);
        }
    }
}