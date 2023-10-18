using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Medicina.Entidades;

namespace IU.Modulos.Medicina
{
    public partial class EstudiosModificar : PaginaSegura
    {

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            ctrModificarDatos.ModificarDatosCancelar += new Controles.EstudiosDatos.ModificarDatosCancelarEventHandler(ctrModificarDatos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                MedEstudios parametro = new MedEstudios();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdEstudio"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PacientesModificar.aspx"), true);

                int valor = Convert.ToInt32(this.MisParametrosUrl["IdEstudio"]);
                parametro.IdEstudio = valor;
                this.ctrModificarDatos.IniciarControl(parametro, Gestion.Modificar);
            }
        }
        void ctrModificarDatos_ModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PacientesModificar.aspx"), true);
        }

    }
}