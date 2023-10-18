using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;

namespace IU.Modulos.TGE
{
    public partial class TiposOperacionesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.TiposOperacionesModificarDatosAceptar += new Control.TiposOperacionesDatos.TiposOperacionesDatosAceptarEventHandler(ModificarDatos_TiposOperacionesModificarDatosAceptar);
            this.ModificarDatos.TiposOperacionesModificarDatosCancelar += new Control.TiposOperacionesDatos.TiposOperacionesDatosCancelarEventHandler(ModificarDatos_TiposOperacionesModificarDatosCancelar);

            if (!this.IsPostBack)
            {
                TGETiposOperaciones tipoOperacion = new TGETiposOperaciones();

                this.ModificarDatos.IniciarControl(tipoOperacion, Gestion.Agregar);
            }
        }

        void ModificarDatos_TiposOperacionesModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposOperacionesListar.aspx"), true);
        }

        void ModificarDatos_TiposOperacionesModificarDatosAceptar(object sender, global::Comunes.Entidades.TGETiposOperaciones e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposOperacionesListar.aspx"), true);
        }
    }
}