using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU
{
    public partial class Mensajes : System.Web.UI.UserControl
    {
        public void MostrarMensaje(string pMensaje, bool pError)
        {
            if (!pError)
                this.lblMensaje.ForeColor = System.Drawing.Color.Green;
            else
                this.lblMensaje.ForeColor = System.Drawing.Color.Red;


            this.lblMensaje.Text = pMensaje;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModal", "ShowModal();", true);
            //this.mpePopUp.Show();
        }

        public void MostrarMensaje(string pMensaje, System.Drawing.Color pColor, bool pBold)
        {
            this.lblMensaje.ForeColor = pColor;
            this.lblMensaje.Font.Bold = pBold; 
            this.lblMensaje.Text = pMensaje;
            //this.mpePopUp.Show();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModal", "ShowModal();", true);
        }
    }
}