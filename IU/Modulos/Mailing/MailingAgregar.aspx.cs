using Comunes.Entidades;
using Mailing.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Mailing
{
    public partial class MailingAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.MailingModificarDatosCancelar += new IU.Modulos.Mailing.Controles.MailingDatos.MailingDatosCancelarEventHandler(ModificarDatos_MailingModificarDatosCancelar);

            if (!this.IsPostBack)
            {
                this.ModificarDatos.IniciarControl(new TGEMailing(), Gestion.Agregar);
            }
        }

        void ModificarDatos_MailingModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Mailing/MailingListar.aspx"), true);
        }
    }
}