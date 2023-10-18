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
    public partial class FacturasAnular : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ModificarDatos.FacturaModificarDatosAceptar += new Controles.FacturasDatos.FacturasDatosAceptarEventHandler(ModificarDatos_FacturaModificarDatosAceptar);
            this.ModificarDatos.FacturaModificarDatosCancelar += new Controles.FacturasDatos.FacturasDatosCancelarEventHandler(ModificarDatos_FacturaModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                VTAFacturas Factura = new VTAFacturas();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdFactura"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturasListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdFactura"]);
                Factura.IdFactura = parametro;

                this.ModificarDatos.IniciarControl(Factura, Gestion.Anular);
            }
        }

        void ModificarDatos_FacturaModificarDatosCancelar()
        {

            string url = "~/Modulos/Cobros/~/Modulos/Facturas/FacturasListar.aspx";
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString());
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(url), true);

        }

        //void ModificarDatos_FacturaModificarDatosAceptar(object sender, global::Facturas.Entidades.VTAFacturas e)
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturasListar.aspx"), true);
        //}
    }
}