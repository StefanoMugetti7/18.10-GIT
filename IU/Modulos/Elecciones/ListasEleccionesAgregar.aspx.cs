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
    public partial class ListasEleccionesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                EleListasElecciones cmr = new EleListasElecciones();

                if (this.MisParametrosUrl.Contains("IdListaEleccion"))
                {
                    int parametro = Convert.ToInt32(this.MisParametrosUrl["IdListaEleccion"]);
                    cmr.IdListaEleccion = parametro;
                }

                this.ModificarDatos.IniciarControl(cmr, Gestion.Agregar);
            }
        }

        private void ModificarDatos_ControlModificarDatosCancelar()
        {
            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Elecciones/ListasEleccionesListar.aspx");
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString());
            else
                this.Response.Redirect(url, true);
        }
    }
}