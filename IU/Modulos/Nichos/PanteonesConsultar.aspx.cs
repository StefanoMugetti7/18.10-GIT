using Comunes.Entidades;
using Hoteles.Entidades;
using Nichos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Nichos
{
    public partial class PanteonesConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                NCHPanteones panteon = new NCHPanteones();

                if (!this.MisParametrosUrl.Contains("IdPanteon"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/PanteonesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdPanteon"]);
                panteon.IdPanteon = parametro;

                ModificarDatos.IniciarControl(panteon, Gestion.Consultar);
            }
        }

        private void ModificarDatos_ControlModificarDatosCancelar()
        {
            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/PanteonesListar.aspx");
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString());
            else
                this.Response.Redirect(url, true);
        }
    }
}