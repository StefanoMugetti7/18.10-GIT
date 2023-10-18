using Comunes.Entidades;
using Generales.Entidades;
using Haberes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Haberes
{
    public partial class HabSueldosAportesRangosModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            ModificarDatos.HabSueldosAportesRangosAceptar += new Controles.HabSueldosAportesRangosDatos.HabSueldosAportesRangosAceptarEventHandler(ModificarDatos_CondicionesFiscalesTiposFacturasAceptar);
            ModificarDatos.HabSueldosAportesRangosCancelar += new Controles.HabSueldosAportesRangosDatos.HabSueldosAportesRangosCancelarEventHandler(ModificarDatos_CondicionesFiscalesTiposFacturasCancelar);
            if (!IsPostBack)
            {
                HabSueldosAportesRangos modulo = new HabSueldosAportesRangos();
                if (!MisParametrosUrl.Contains("IdSueldoAporteRango"))
                {
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Haberes/HabSueldosAportesRangosListar.aspx"), true);
                }
                int IdSueldoAporteRango = Convert.ToInt32(MisParametrosUrl["IdSueldoAporteRango"]);
                modulo.IdSueldoAporteRango = IdSueldoAporteRango;
                ModificarDatos.IniciarControl(modulo, Gestion.Modificar);
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