using Comunes.Entidades;
using CuentasPagar.Entidades;
using System;

namespace IU.Modulos.Compras
{
    public partial class AnticipoReservaModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.SolicitudPagoModificarDatosAceptar += new Controles.AnticipoReservaDatos.SolicitudPagoDatosAceptarEventHandler(ModificarDatos_SolicitudPagoModificarDatosAceptar);
            this.ModificarDatos.SolicitudPagoModificarDatosCancelar += new Controles.AnticipoReservaDatos.SolicitudPagoDatosCancelarEventHandler(ModificarDatos_SolicitudPagoModificarDatosCancelar);

            if (!this.IsPostBack)
            {
                CapSolicitudPago solicitud = new CapSolicitudPago();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdSolicitudPago"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/AnticipoReservaListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdSolicitudPago"]);
                solicitud.IdSolicitudPago = parametro;

                this.ModificarDatos.IniciarControl(solicitud, Gestion.Modificar);
            }
        }
        void ModificarDatos_SolicitudPagoModificarDatosCancelar()
        {

            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/AnticipoReservaListar.aspx"), true);
        }
        void ModificarDatos_SolicitudPagoModificarDatosAceptar(object sender, global::CuentasPagar.Entidades.CapSolicitudPago e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/AnticipoReservaListar.aspx"), true);
        }
    }
}