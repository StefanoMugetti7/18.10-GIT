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
    public partial class NichosModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                NCHNichos nicho = new NCHNichos();

                if (!this.MisParametrosUrl.Contains("IdNicho"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/NichosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdNicho"]);
                nicho.IdNicho = parametro;

                ModificarDatos.IniciarControl(nicho, Gestion.Modificar);
            }
        }

        private void ModificarDatos_ControlModificarDatosCancelar()
        {
            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/NichosListar.aspx");
            //if (this.ViewState["UrlReferrer"] != null)
            //    this.Response.Redirect(this.ViewState["UrlReferrer"].ToString());
            //else
            this.Response.Redirect(url, true);
        }
    }
}