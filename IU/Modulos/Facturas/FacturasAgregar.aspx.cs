using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facturas.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Facturas
{
    public partial class FacturasAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ModificarDatos.FacturaModificarDatosAceptar +=new Controles.FacturasDatos.FacturasDatosAceptarEventHandler(ModificarDatos_FacturaModificarDatosAceptar);
            this.ModificarDatos.FacturaModificarDatosCancelar +=new Controles.FacturasDatos.FacturasDatosCancelarEventHandler(ModificarDatos_FacturaModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                ScriptManager.RegisterClientScriptResource(this, typeof(WebResources), "IU.tinymce.js.tinymce.tinymce.min.js");
                this.ModificarDatos.IniciarControl(new VTAFacturas(), Gestion.Agregar);
            }
        }

        void  ModificarDatos_FacturaModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturasListar.aspx"), true);
        }

        //void  ModificarDatos_FacturaModificarDatosAceptar(object sender, VTAFacturas e)
        //{
        //    if (this.ViewState["UrlReferrer"] != null)
        //        this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
        //    else
        //        this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturasListar.aspx"), true);
        //}
        
    }
}