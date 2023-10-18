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
    public partial class PaquetesConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                TurPaquetes requerimiento = new TurPaquetes();

                if (!this.MisParametrosUrl.Contains("IdPaquete"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Paquetes/PaquetesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdPaquete"]);
                requerimiento.IdPaquete= parametro;

                ModificarDatos.IniciarControl(requerimiento, Gestion.Consultar);
            }
        }

        private void ModificarDatos_ControlModificarDatosCancelar()
        {
            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Paquetes/PaquetesListar.aspx");
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString());
            else
                this.Response.Redirect(url, true);
        }
    }
}