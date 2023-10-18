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
    public partial class ConveniosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                TurConvenios cmr = new TurConvenios();

                if (this.MisParametrosUrl.Contains("IdConvenio"))
                {
                    int parametro = Convert.ToInt32(this.MisParametrosUrl["IdConvenio"]);
                    cmr.IdConvenio = parametro;
                }

                this.ModificarDatos.IniciarControl(cmr, Gestion.Agregar);
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