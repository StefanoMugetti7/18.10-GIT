using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using CuentasPagar.Entidades;

namespace IU.Modulos.CuentasPagar
{
    public partial class SolicitudesPagosConsultar : PaginaSegura
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

                if (this.MisParametrosUrl.Contains("IdTipoOperacion"))
                    solicitud.TipoOperacion.IdTipoOperacion = Convert.ToInt32(this.MisParametrosUrl["IdTipoOperacion"]);

                this.ModificarDatos.IniciarControl(solicitud, Gestion.Consultar);
            }
        }

        void ModificarDatos_SolicitudPagoModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosListar.aspx"), true);
        }

        void ModificarDatos_SolicitudPagoModificarDatosAceptar(object sender, global:: CuentasPagar.Entidades.CapSolicitudPago e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosListar.aspx"), true);
        }
    }
}