using Mailing;
using Mailing.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Mailing.Controles
{
    public partial class MailingVistaPrevia : ControlesSeguros
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

        }

        public void IniciarControl(AudMailsEnvios pParametro)
        {
            pParametro = MailingF.MailsEnviosObtenerMensaje(pParametro);
            ltrDatos.Text = pParametro.Cuerpo;
            txtDe.Text = "De: " + pParametro.De + " - " + pParametro.DeMostrar;
            txtPara.Text = "Para: " + pParametro.A + " - " + pParametro.AMostrar;
            txtAsunto.Text = "Asunto: " +  pParametro.Asunto;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModal", "ShowModalVistaPrevia();", true);
        }
    }
}