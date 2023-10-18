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
    public partial class RemitosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ModificarDatos.RemitosModificarDatosAceptar += new Controles.RemitosDatos.RemitosDatosAceptarEventHandler(ModificarDatos_FacturaRemitosDatosAceptar);
            this.ModificarDatos.RemitosModificarDatosCancelar += new Controles.RemitosDatos.RemitosDatosCancelarEventHandler(ModificarDatos_RemitosModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ModificarDatos.IniciarControl(new VTARemitos(), Gestion.Agregar);
            }
        }

        void ModificarDatos_RemitosModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/RemitosListar.aspx"), true);
        }

        //void ModificarDatos_FacturaRemitosDatosAceptar(object sender, VTARemitos e)
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/RemitosListar.aspx"), true);
        //}
    }
}