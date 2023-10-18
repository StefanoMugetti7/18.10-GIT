using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.TGE
{
    public partial class ParametrosMailsModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrParametrosMails.ParametrosMailsDatosAceptar += new IU.Modulos.TGE.Control.ParametrosMailsDatos.ParametrosDatosAceptarEventHandler(ctrParametrosMails_ParametrosMailsDatosAceptar);
            this.ctrParametrosMails.ParametrosMailsDatosCancelar += new IU.Modulos.TGE.Control.ParametrosMailsDatos.ParametrosDatosCancelarEventHandler(ctrParametrosMails_ParametrosMailsDatosCancelar);
            if (!this.IsPostBack)
            {
                //if (!this.MisParametrosUrl.Contains("IdParametroMail"))
                //    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/Control/ParametrosMailsDatos.ascx"), true);
                TGEParametrosMails parametro = new TGEParametrosMails();
                parametro.IdParametroMail = Convert.ToInt32(this.MisParametrosUrl["IdParametroMail"]);
                this.ctrParametrosMails.IniciarControl(parametro, Gestion.Modificar);
            }
        }


        void ctrParametrosMails_ParametrosMailsDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
        }

        void ctrParametrosMails_ParametrosMailsDatosAceptar(object sender, Generales.Entidades.TGEParametrosMails e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
        }
    }
}