using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuentasPagar.Entidades;

namespace IU.Modulos.CuentasPagar
{
    public partial class ModificarComprobantes : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ModificarDatosCancelar += new IU.Modulos.CuentasPagar.Controles.ModificarComprobantesDatos.ModificarDatosCancelarEventHandler(ModificarDatos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ModificarDatos.IniciarControl(new List<CapSolicitudPago>(), global::Comunes.Entidades.Gestion.Agregar);
            }
        }

        void ModificarDatos_ModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosListar.aspx"), true);
        }
    }
}