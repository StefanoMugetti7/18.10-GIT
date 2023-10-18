using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bancos.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Bancos
{
    public partial class CobranzasExternasConciliacionesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.CobranzasModificarDatosAceptar += new IU.Modulos.Bancos.Controles.CobranzasExternasConciliacionesDatos.CobranzasModificarDatosAceptarEventHandler(ModificarDatos_CobranzaExternaConciliacionesModificarDatosAceptar);
            this.ModificarDatos.CobranzasModificarDatosCancelar += new IU.Modulos.Bancos.Controles.CobranzasExternasConciliacionesDatos.CobranzasModificarDatosCancelarEventHandler(ModificarDatos_CobranzaExternaConciliacionesModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ModificarDatos.IniciarControl(new TESCobranzasExternasConciliaciones(), Gestion.Agregar);
            }
        }

        void ModificarDatos_CobranzaExternaConciliacionesModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/CobranzasExternasConciliacionesListar.aspx"), true);
        }

        void ModificarDatos_CobranzaExternaConciliacionesModificarDatosAceptar(object sender, TESCobranzasExternasConciliaciones e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/CobranzasExternasConciliacionesListar.aspx"), true);
        }
    }
}