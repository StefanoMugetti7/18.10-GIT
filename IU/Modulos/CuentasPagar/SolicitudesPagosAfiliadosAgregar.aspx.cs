using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuentasPagar.Entidades;
using Comunes.Entidades;
using Generales.Entidades;

namespace IU.Modulos.CuentasPagar
{
    public partial class SolicitudesPagosAfiliadosAgregar : PaginaAfiliados
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.SolicitudPagoModificarDatosAceptar += new Controles.SolicitudPagoDatos.SolicitudPagoDatosAceptarEventHandler(ModificarDatos_SolicitudModificarDatosAceptar);
            this.ModificarDatos.SolicitudPagoModificarDatosCancelar += new Controles.SolicitudPagoDatos.SolicitudPagoDatosCancelarEventHandler(ModificarDatos_SolicitudModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                CapSolicitudPago sp = new CapSolicitudPago();
                sp.TipoOperacion.IdTipoOperacion=(int)EnumTGETiposOperaciones.SolicitudesPagosTerceros;
                sp.Afiliado = this.MiAfiliado;
                this.ModificarDatos.IniciarControl(sp, Gestion.Agregar);
            }
        }

        void ModificarDatos_SolicitudModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosAfiliadosListar.aspx"), true);
        }

        void ModificarDatos_SolicitudModificarDatosAceptar(object sender, CapSolicitudPago e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosAfiliadosListar.aspx"), true);
        }
        
    }
}