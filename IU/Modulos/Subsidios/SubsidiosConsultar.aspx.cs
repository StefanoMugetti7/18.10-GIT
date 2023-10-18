using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subsidios.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Subsidios
{
    public partial class SubsidiosConsultar : PaginaSegura
    {

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.SubsidiosModificarDatosAceptar += new Controles.SubsidiosDatos.SubsidiosDatosAceptarEventHandler(ModificarDatos_SubsidiosModificarDatosAceptar);
            this.ModificarDatos.SubsidiosModificarDatosCancelar += new Controles.SubsidiosDatos.SubsidiosDatosCancelarEventHandler(ModificarDatos_SubsidiosModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                SubSubsidios solicitud = new SubSubsidios();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdSubsidio"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Subsidios/SubsidiosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdSubsidio"]);
                solicitud.IdSubsidio = parametro;
                this.ModificarDatos.IniciarControl(solicitud, Gestion.Consultar);
            }
        }

        void ModificarDatos_SubsidiosModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Subsidios/SubsidiosListar.aspx"), true);
        }

        void ModificarDatos_SubsidiosModificarDatosAceptar(object sender, global::Subsidios.Entidades.SubSubsidios e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Subsidios/SubsidiosListar.aspx"), true);
        }
    }
}