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
    public partial class ConveniosModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                TurConvenios requerimiento = new TurConvenios();

                if (!this.MisParametrosUrl.Contains("IdConvenio"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Turismo/ConveniosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdConvenio"]);
                requerimiento.IdConvenio = parametro;

                ModificarDatos.IniciarControl(requerimiento, Gestion.Modificar);
            }
        }

        private void ModificarDatos_ControlModificarDatosCancelar()
        {
            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Turismo/ConveniosListar.aspx");
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString());
            else
                this.Response.Redirect(url, true);
        }
    }
}