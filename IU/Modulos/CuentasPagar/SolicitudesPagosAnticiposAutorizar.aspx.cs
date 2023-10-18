using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuentasPagar.Entidades;
using Generales.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.CuentasPagar
{
    public partial class SolicitudesPagosAnticiposAutorizar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.SolicitudPagoAnticipoModificarDatosAceptar += new Controles.SolicitudesPagosAnticipos.SolicitudPagoAnticiposAceptarEventHandler(ModificarDatos_SolicitudModificarDatosAceptar);
            this.ModificarDatos.SolicitudPagoAnticipoModificarDatosCancelar += new Controles.SolicitudesPagosAnticipos.SolicitudPagoAnticiposCancelarEventHandler(ModificarDatos_SolicitudModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                CapSolicitudPago sp = new CapSolicitudPago();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdSolicitudPago"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdSolicitudPago"]);
                sp.IdSolicitudPago = parametro;
                this.ModificarDatos.IniciarControl(sp, Gestion.Autorizar);
            }
        }

        void ModificarDatos_SolicitudModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosListar.aspx"), true);
        }

        void ModificarDatos_SolicitudModificarDatosAceptar(object sender, CapSolicitudPago e)
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosListar.aspx"), true);
        }
    }
}