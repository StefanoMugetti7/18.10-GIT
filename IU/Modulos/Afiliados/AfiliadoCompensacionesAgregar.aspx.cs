using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Afiliados.Entidades;

namespace IU.Modulos.Afiliados
{
    public partial class AfiliadoCompensacionesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.CompensacionesModificarDatosAceptar += new IU.Modulos.Afiliados.Controles.AfiliadoCompensaciones.AfiliadoCompensacionesDatosAceptarEventHandler(ModificarDatos_AfiliadoCompensacionesModificarDatosAceptar);
            this.ModificarDatos.CompensacionesModificarDatosCancelar += new IU.Modulos.Afiliados.Controles.AfiliadoCompensaciones.AfiliadoCompensacionesDatosCancelarEventHandler(ModificarDatos_AfiliadoCompensacionesModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ModificarDatos.IniciarControl(new AfiCompensaciones(), Gestion.Agregar);
            }
        }

        protected void ModificarDatos_AfiliadoCompensacionesModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadoCompensacionesListar.aspx"), true);
        }

        protected void ModificarDatos_AfiliadoCompensacionesModificarDatosAceptar(object sender, global::Afiliados.Entidades.AfiCompensaciones e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadoCompensacionesListar.aspx"), true);
        }
    }
}