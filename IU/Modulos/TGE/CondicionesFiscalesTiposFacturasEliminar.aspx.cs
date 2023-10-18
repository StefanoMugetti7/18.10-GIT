using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.TGE
{
    public partial class CondicionesFiscalesTiposFacturasEliminar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            ModificarDatos.CondicionesFiscalesTiposFacturasAceptar += new Control.CondicionesFiscalesTiposFacturasDatos.CondicionesFiscalesTiposFacturasAceptarEventHandler(ModificarDatos_CondicionesFiscalesTiposFacturasAceptar);
            ModificarDatos.CondicionesFiscalesTiposFacturasCancelar += new Control.CondicionesFiscalesTiposFacturasDatos.CondicionesFiscalesTiposFacturasCancelarEventHandler(ModificarDatos_CondicionesFiscalesTiposFacturasCancelar);
            if (!IsPostBack)
            {
                TGECondicionesFiscalesTiposFacturas modulo = new TGECondicionesFiscalesTiposFacturas();
                if (!MisParametrosUrl.Contains("IdCondicionFiscal"))
                {
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/CondicionesFiscalesTiposFacturasListar.aspx"), true);
                }
                int IdCondicionFiscal = Convert.ToInt32(MisParametrosUrl["IdCondicionFiscal"]);
                int IdTipoFactura = Convert.ToInt32(MisParametrosUrl["IdTipoFactura"]);
                modulo.IdCondicionFiscal = IdCondicionFiscal;
                modulo.IdTipoFactura = IdTipoFactura;
                ModificarDatos.IniciarControl(modulo, Gestion.Anular);
            }
        }

        private void ModificarDatos_CondicionesFiscalesTiposFacturasAceptar(object sender, TGECondicionesFiscalesTiposFacturas e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/CondicionesFiscalesTiposFacturasListar.aspx"), true);
        }

        private void ModificarDatos_CondicionesFiscalesTiposFacturasCancelar()
        {

            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/CondicionesFiscalesTiposFacturasListar.aspx"), true);
        }
    }
}