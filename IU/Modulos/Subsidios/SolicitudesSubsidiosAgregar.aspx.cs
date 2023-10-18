using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuentasPagar.Entidades;
using Comunes.Entidades;
using Generales.Entidades;

namespace IU.Modulos.Subsidios
{
    public partial class SolicitudesSubsidiosAgregar : PaginaAfiliados
    {

        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            this.ModificarDatos.SolicitudPagoModificarDatosAceptar += new Controles.SolicitudesSubsidiosDatos.SolicitudPagoDatosAceptarEventHandler(ModificarDatos_SolicitudModificarDatosAceptar);
            this.ModificarDatos.SolicitudPagoModificarDatosCancelar += new Controles.SolicitudesSubsidiosDatos.SolicitudPagoDatosCancelarEventHandler(ModificarDatos_SolicitudModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                CapSolicitudPago solpa = new CapSolicitudPago();
                solpa.Entidad.IdEntidad = (int)EnumTGEEntidades.Afiliados;
                solpa.Entidad.IdRefEntidad = this.MiAfiliado.IdAfiliado;
                this.ModificarDatos.IniciarControl(solpa, Gestion.Agregar);
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