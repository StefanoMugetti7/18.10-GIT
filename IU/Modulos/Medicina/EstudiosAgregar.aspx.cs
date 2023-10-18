using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Medicina.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Medicina
{
    public partial class EstudiosAgregar : PaginaSegura
    {

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            ctrModificarDatos.ModificarDatosCancelar += new Controles.EstudiosDatos.ModificarDatosCancelarEventHandler(ctrModificarDatos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ctrModificarDatos.IniciarControl(new MedEstudios(), Gestion.Agregar);
            }
        }

        void ctrModificarDatos_ModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PacientesModificar.aspx"), true);
        }
    }
}