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
    public partial class ListasEleccionesModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                EleListasElecciones requerimiento = new EleListasElecciones();

                if (!this.MisParametrosUrl.Contains("IdListaEleccion"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Elecciones/ListasEleccionesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdListaEleccion"]);
                requerimiento.IdListaEleccion = parametro;

                ModificarDatos.IniciarControl(requerimiento, Gestion.Modificar);
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