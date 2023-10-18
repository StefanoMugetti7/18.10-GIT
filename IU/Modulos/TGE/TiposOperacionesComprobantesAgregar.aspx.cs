using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.TGE
{
    public partial class TiposOperacionesComprobantesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.TiposOperacionesTiposFacturasModificarDatosAceptar += ModificarDatos_TiposOperacionesTiposFacturasModificarDatosAceptar;
            this.ModificarDatos.TiposOperacionesTiposFacturasModificarDatosCancelar += new Control.TiposOperacionesComprobantesDatos.TGETiposOperacionesTiposFacturasDatosCancelarEventHandler(ModificarDatos_TiposOperacionesTiposFacturasModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ModificarDatos.IniciarControl(new Generales.Entidades.TGETiposOperacionesTiposFacturas(), Gestion.Agregar);
            }
        }

        private void ModificarDatos_TiposOperacionesTiposFacturasModificarDatosAceptar(object sender, Generales.Entidades.TGETiposOperacionesTiposFacturas e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposOperacionesComprobantesListar.aspx"), true);
        }

        void ModificarDatos_TiposOperacionesTiposFacturasModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposOperacionesComprobantesListar.aspx"), true);
        }

        void ModificarDatos_TiposOperacionesTiposFacturasModificarDatosCancelar(object sender, TiposOperacionesComprobantesAgregar e)
        {
            
        }
    }
}
