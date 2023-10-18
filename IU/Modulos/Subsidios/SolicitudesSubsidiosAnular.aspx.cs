using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using CuentasPagar.Entidades;

namespace IU.Modulos.Subsidios
{
    public partial class SolicitudesSubsidiosAnular : PaginaAfiliados
    {

        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            this.ModificarDatos.SolicitudPagoModificarDatosAceptar += new Controles.SolicitudesSubsidiosDatos.SolicitudPagoDatosAceptarEventHandler(ModificarDatos_SolicitudModificarDatosAceptar);
            this.ModificarDatos.SolicitudPagoModificarDatosCancelar += new Controles.SolicitudesSubsidiosDatos.SolicitudPagoDatosCancelarEventHandler(ModificarDatos_SolicitudModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                CapSolicitudPago solicitud = new CapSolicitudPago();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdSolicitudPago"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Subsidios/SolicitudesSubsidiosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdSolicitudPago"]);
                solicitud.IdSolicitudPago = parametro;
                this.ModificarDatos.IniciarControl(solicitud, Gestion.Anular);
            }
        }

        void ModificarDatos_SolicitudModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Subsidios/SolicitudesSubsidiosListar.aspx"), true);
        }

        void ModificarDatos_SolicitudModificarDatosAceptar(object sender, CapSolicitudPago e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Subsidios/SolicitudesSubsidiosListar.aspx"), true);
        }



    }
}