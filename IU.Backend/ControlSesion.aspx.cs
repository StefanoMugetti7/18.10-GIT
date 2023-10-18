using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace IU
{
    public partial class ControlSesion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.mpePopUp.Show();
            }
        }

        protected void btnAceptarPnlMensajes_Click(object sender, EventArgs e)
        {
            this.mpePopUp.Hide();
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("IngresoSistema.aspx"), true);
        }
    }
}
