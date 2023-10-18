using Comunes.Entidades;
using Elecciones.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Elecciones
{
    public partial class EleccionesModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                EleElecciones requerimiento = new EleElecciones();

                if (!this.MisParametrosUrl.Contains("IdEleccion"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Elecciones/EleccionesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdEleccion"]);
                requerimiento.IdEleccion = parametro;

                ModificarDatos.IniciarControl(requerimiento, Gestion.Modificar);
            }
        }

        private void ModificarDatos_ControlModificarDatosCancelar()
        {
            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Elecciones/EleccionesListar.aspx");
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString());
            else
                this.Response.Redirect(url, true);
        }
    }
}