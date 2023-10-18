using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuentasPagar.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.CuentasPagar
{
    public partial class OrdenesPagosAgregar : PaginaSegura
    {

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.OrdenesDePagoDatosAceptar += new Controles.OrdenesPagosDatos.OrdenesDePagoDatosAceptarEventHandler(ModificarDatos_OrdenesDePagoDatosAceptar);
            this.ModificarDatos.OrdenesDePagoDatosCancelar += new Controles.OrdenesPagosDatos.OrdenesDePagoDatosCancelarEventHandler(ModificarDatos_OrdenesDePagoDatosCancelar);
            if (!this.IsPostBack)
            {
                CapOrdenesPagos op = new CapOrdenesPagos();
                if (this.MisParametrosUrl.Contains("IdEntidad"))
                    op.Entidad.IdEntidad = Convert.ToInt32(this.MisParametrosUrl["IdEntidad"]);
                if (this.MisParametrosUrl.Contains("IdRefEntidad"))
                    op.Entidad.IdRefEntidad = Convert.ToInt32(this.MisParametrosUrl["IdRefEntidad"]);

                this.ModificarDatos.IniciarControl(op, Gestion.Agregar);
            }
        }

        void ModificarDatos_OrdenesDePagoDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/OrdenesPagosListar.aspx"), true);
        }

        void ModificarDatos_OrdenesDePagoDatosAceptar(global::CuentasPagar.Entidades.CapOrdenesPagos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/OrdenesPagosListar.aspx"), true);
        }
    }
}