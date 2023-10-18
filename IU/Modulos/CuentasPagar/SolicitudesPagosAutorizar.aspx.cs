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
    public partial class SolicitudesPagosAutorizar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.SolicitudPagoModificarDatosAceptar += new Controles.SolicitudPagoDatos.SolicitudPagoDatosAceptarEventHandler(ModificarDatos_SolicitudPagoModificarDatosAceptar);
            this.ModificarDatos.SolicitudPagoModificarDatosCancelar += new Controles.SolicitudPagoDatos.SolicitudPagoDatosCancelarEventHandler(ModificarDatos_SolicitudPagoModificarDatosCancelar);

            if (!this.IsPostBack)
            {
                CapSolicitudPago solicitud = new CapSolicitudPago();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdSolicitudPago"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdSolicitudPago"]);
                solicitud.IdSolicitudPago = parametro;

                this.ModificarDatos.IniciarControl(solicitud, Gestion.Autorizar);
            }
        }

        void ModificarDatos_SolicitudPagoModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosListar.aspx"), true);
        }

        void ModificarDatos_SolicitudPagoModificarDatosAceptar(object sender, global::CuentasPagar.Entidades.CapSolicitudPago e)
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosListar.aspx"), true);
        }
    }
}