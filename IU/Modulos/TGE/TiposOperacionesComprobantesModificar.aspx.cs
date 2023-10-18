using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Generales.Entidades;

namespace IU.Modulos.TGE
{
    public partial class TiposOperacionesComprobantesModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.TiposOperacionesTiposFacturasModificarDatosAceptar += new Control.TiposOperacionesComprobantesDatos.TGETiposOperacionesTiposFacturasDatosAceptarEventHandler(ModificarDatos_TiposOperacionesComprobantesDatosAceptar);
            this.ModificarDatos.TiposOperacionesTiposFacturasModificarDatosCancelar += new Control.TiposOperacionesComprobantesDatos.TGETiposOperacionesTiposFacturasDatosCancelarEventHandler(ModificarDatos_TiposOperacionesComprobantesDatosCancelar);
            if (!this.IsPostBack)
            {
                TGETiposOperacionesTiposFacturas TiposOperacionesComprobantes = new TGETiposOperacionesTiposFacturas();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdTipoOperacionTipoFactura"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposOperacionesComprobantesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdTipoOperacionTipoFactura"]);
                TiposOperacionesComprobantes.IdTipoOperacionTipoFactura = parametro;
                this.ModificarDatos.IniciarControl(TiposOperacionesComprobantes, Gestion.Modificar);
            }
        }

        void ModificarDatos_TiposOperacionesComprobantesDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposOperacionesComprobantesListar.aspx"), true);
        }

        void ModificarDatos_TiposOperacionesComprobantesDatosAceptar(object sender, TGETiposOperacionesTiposFacturas e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposOperacionesComprobantesListar.aspx"), true);
        }


    }
}