using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Generales.Entidades;
using System.Collections.Generic;

namespace IU.Modulos.Comunes
{
    public partial class popUpMensajesPostBack : System.Web.UI.UserControl
    {
        public delegate void popUpMensajesPostBackAceptarEventHandler();
        public event popUpMensajesPostBackAceptarEventHandler popUpMensajesPostBackAceptar;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void MostrarMensaje(string mensaje)
        {
            this.MostrarMensaje(mensaje, false);
        }

        public void MostrarMensaje(string mensaje, bool confirmarAccion)
        {
            this.lblPopUpConfirmarMensaje.Text = mensaje;
            this.btnCancelar.Visible = confirmarAccion;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModal", "ShowModalMensajesPostBack();", true);
            //this.mpePopUp.Show();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //this.mpePopUp.Hide();
            if (this.popUpMensajesPostBackAceptar != null)
                this.popUpMensajesPostBackAceptar();
        }

    }
}