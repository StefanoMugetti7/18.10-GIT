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
    public partial class EleccionesVotar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                EleElecciones cmr = new EleElecciones();

                if (this.MisParametrosUrl.Contains("IdEleccion"))
                {
                    int parametro = Convert.ToInt32(this.MisParametrosUrl["IdEleccion"]);
                    cmr.IdEleccion = parametro;
                }

                this.ModificarDatos.IniciarControl(cmr, Gestion.Agregar);
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