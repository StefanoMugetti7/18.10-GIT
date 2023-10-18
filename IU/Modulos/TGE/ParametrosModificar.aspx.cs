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
using Comunes.Entidades;
using Generales.Entidades;

namespace IU.Modulos.TGE
{
    public partial class ParametrosModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrParametros.ParametrosDatosAceptar += new IU.Modulos.TGE.Control.ParametrosDatos.ParametrosDatosAceptarEventHandler(ctrParametros_ParametrosDatosAceptar);
            this.ctrParametros.ParametrosDatosCancelar += new IU.Modulos.TGE.Control.ParametrosDatos.ParametrosDatosCancelarEventHandler(ctrParametros_ParametrosDatosCancelar);
            if (!this.IsPostBack)
            {
                if (!this.MisParametrosUrl.Contains("IdParametro"))
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/ParametrosListar.aspx"), true);
                TGEParametros parametro = new TGEParametros();
                parametro.IdParametro = Convert.ToInt32(this.MisParametrosUrl["IdParametro"]);
                this.ctrParametros.IniciarControl(parametro, Gestion.Modificar);
            }
        }

        void ctrParametros_ParametrosDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/ParametrosListar.aspx"), true);
        }

        void ctrParametros_ParametrosDatosAceptar(object sender, Generales.Entidades.TGEParametros e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/ParametrosListar.aspx"), true);
        }
    }
}
