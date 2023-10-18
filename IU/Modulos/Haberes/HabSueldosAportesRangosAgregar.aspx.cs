using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;

using Haberes.Entidades;

namespace IU.Modulos.Haberes
{
    public partial class HabSueldosAportesRangosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            ModificarDatos.HabSueldosAportesRangosAceptar += new Controles.HabSueldosAportesRangosDatos.HabSueldosAportesRangosAceptarEventHandler(ModificarDatos_CondicionesFiscalesTiposFacturasAceptar);
            ModificarDatos.HabSueldosAportesRangosCancelar += new Controles.HabSueldosAportesRangosDatos.HabSueldosAportesRangosCancelarEventHandler(ModificarDatos_CondicionesFiscalesTiposFacturasCancelar);
            if (!IsPostBack)
            {
                ModificarDatos.IniciarControl(new HabSueldosAportesRangos(), Gestion.Agregar);
            }
        }

        private void ModificarDatos_CondicionesFiscalesTiposFacturasAceptar()
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Haberes/HabSueldosAportesRangosListar.aspx"), true);
        }

        private void ModificarDatos_CondicionesFiscalesTiposFacturasCancelar()
        {

            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Haberes/HabSueldosAportesRangosListar.aspx"), true);
        }
    }
}