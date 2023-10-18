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
    public partial class TiposOperacionesComprobantesConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ModificarDatos.TiposOperacionesModificarDatosAceptar += new Control.TiposOperacionesDatos.TiposOperacionesDatosAceptarEventHandler(ModificarDatos_TiposOperacionesModificarDatosAceptar);
            this.ModificarDatos.TiposOperacionesTiposFacturasModificarDatosCancelar += new Control.TiposOperacionesComprobantesDatos.TGETiposOperacionesTiposFacturasDatosCancelarEventHandler(ModificarDatos_TiposOperacionesComprobantesDatosCancelar);

            if (!this.IsPostBack)
            {
                TGETiposOperacionesTiposFacturas tipoOperacion = new TGETiposOperacionesTiposFacturas();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdTipoOperacionTipoFactura"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposOperacionesComprobantesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdTipoOperacionTipoFactura"]);
                tipoOperacion.IdTipoOperacionTipoFactura = parametro;

                this.ModificarDatos.IniciarControl(tipoOperacion, Gestion.Consultar);
            }
        }

        void ModificarDatos_TiposOperacionesComprobantesDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposOperacionesComprobantesListar.aspx"), true);
        }
    }
}