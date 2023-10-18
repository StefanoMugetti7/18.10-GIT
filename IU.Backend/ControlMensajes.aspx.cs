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
    public partial class ControlMensajes : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("CodigoMensaje"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/IngresoSistema.aspx"), true);

                string parametro = this.MisParametrosUrl["CodigoMensaje"].ToString();
                this.MostrarMensaje(parametro,true);
            }
        }
    }
}
