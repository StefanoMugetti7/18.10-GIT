using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU
{
    public partial class ProgressBar : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!this.IsPostBack)
            //    this.ShowProgressBar = true;
        }

        public void MensajesOpcionales(string msg)
        {
            this.lblMensajesOpcionales.Text = msg;
        }
    }
}