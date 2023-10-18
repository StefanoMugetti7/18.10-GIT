using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;

namespace IU.Modulos.TGE
{
    public partial class FilialesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.FilialModificarDatosAceptar += new Control.FilialesDatos.FilialesDatosAceptarEventHandler(ModificarDatos_FilialModificarDatosAceptar);
            this.ModificarDatos.FilialModificarDatosCancelar += new Control.FilialesDatos.FilialesDatosCancelarEventHandler(ModificarDatos_FilialModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ModificarDatos.IniciarControl(new TGEFiliales(), Gestion.Agregar);
            }
        }

        void ModificarDatos_FilialModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FilialesListar.aspx"), true);
        }

        void ModificarDatos_FilialModificarDatosAceptar(object sender, TGEFiliales e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FilialesListar.aspx"), true);
        }
    }
}
