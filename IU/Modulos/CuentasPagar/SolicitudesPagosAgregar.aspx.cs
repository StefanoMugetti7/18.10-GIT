using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using CuentasPagar.Entidades;
using Generales.Entidades;

namespace IU.Modulos.CuentasPagar
{
    public partial class SolicitudesPagosAgregar : PaginaSegura
    {

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.SolicitudPagoModificarDatosAceptar += new Controles.SolicitudPagoDatos.SolicitudPagoDatosAceptarEventHandler(ModificarDatos_SolicitudModificarDatosAceptar);
            this.ModificarDatos.SolicitudPagoModificarDatosCancelar += new Controles.SolicitudPagoDatos.SolicitudPagoDatosCancelarEventHandler(ModificarDatos_SolicitudModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                CapSolicitudPago sp = new CapSolicitudPago();
                sp.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.SolicitudesPagosCompras;
                this.ModificarDatos.IniciarControl(sp, Gestion.Agregar);
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
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosListar.aspx"), true);
        }


        
    }
}