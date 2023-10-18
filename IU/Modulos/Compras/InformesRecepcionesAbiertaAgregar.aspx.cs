using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Compras
{
    public partial class InformesRecepcionesAbiertaAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.InformesRecepcionesModificarDatosAceptar += new Controles.InformesRecepcionesAbiertoDatos.InformeRecepcionDatosAceptarEventHandler(ModificarDatos_ModificarDatosAceptar);
            this.ModificarDatos.InformesRecepcionesModificarDatosCancelar += new Controles.InformesRecepcionesAbiertoDatos.InformeRecepcionDatosCancelarEventHandler(ModificarDatos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                CmpInformesRecepciones inf = new CmpInformesRecepciones();
                if (this.MisParametrosUrl.Contains("IdProveedor"))
                    inf.Proveedor.IdProveedor = Convert.ToInt32(this.MisParametrosUrl["IdProveedor"]);
                this.ModificarDatos.IniciarControl(inf, Gestion.Agregar);
            }
        }

        void ModificarDatos_ModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/InformesRecepcionesListar.aspx"), true);
        }

        void ModificarDatos_ModificarDatosAceptar(object sender, global::Compras.Entidades.CmpInformesRecepciones e)
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/InformesRecepcionesListar.aspx"), true);
        }
    }
}