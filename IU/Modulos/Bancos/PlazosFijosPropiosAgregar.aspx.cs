using Bancos.Entidades;
using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Bancos
{
    public partial class PlazosFijosPropiosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            ModificarDatos.PlazosFijosPropiosDatosAceptar += new Controles.PlazosFijosPropiosDatos.PlazosFijosPropiosDatosAceptarEventHandler(ModificarDatos_PlazosFijosPropiosModificarDatosAceptar);
            ModificarDatos.PlazosFijosPropiosDatosCancelar += new Controles.PlazosFijosPropiosDatos.PlazosFijosPropiosDatosCancelarEventHandler(ModificarDatos_PlazosFijosPropiosModificarDatosCancelar);
            if (!IsPostBack)
            {
                ModificarDatos.IniciarControl(new TESPlazosFijos(), Gestion.Agregar);
            }
        }

        private void ModificarDatos_PlazosFijosPropiosModificarDatosAceptar(object sender, TESPlazosFijos e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/PlazosFijosPropiosListar.aspx"), true);
        }

        private void ModificarDatos_PlazosFijosPropiosModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/PlazosFijosPropiosListar.aspx"), true);
        }
    }
}