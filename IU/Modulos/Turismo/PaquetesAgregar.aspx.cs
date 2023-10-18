using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Turismo.Entidades;

namespace IU.Modulos.Turismo
{
    public partial class PaquetesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                TurPaquetes cmr = new TurPaquetes();

                if (this.MisParametrosUrl.Contains("IdPaquete"))
                {
                    int parametro = Convert.ToInt32(this.MisParametrosUrl["IdPaquete"]);
                    cmr.IdPaquete = parametro;
                }

                this.ModificarDatos.IniciarControl(cmr, Gestion.Agregar);
            }
        }

        private void ModificarDatos_ControlModificarDatosCancelar()
        {
            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Turismo/PaquetesListar.aspx");
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString());
            else
                this.Response.Redirect(url, true);
        }
    }
}