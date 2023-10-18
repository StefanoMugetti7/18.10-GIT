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
    public partial class MailingModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.MailingModificarDatosCancelar += new IU.Modulos.Mailing.Controles.MailingDatos.MailingDatosCancelarEventHandler(ModificarDatos_MailingModificarDatosCancelar);

            if (!this.IsPostBack)
            {
                TGEMailing mailing = new TGEMailing();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdMailing"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Mailing/MailingListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdMailing"]);
                mailing.IdMailing = parametro;

                ModificarDatos.IniciarControl(mailing, Gestion.Modificar);
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