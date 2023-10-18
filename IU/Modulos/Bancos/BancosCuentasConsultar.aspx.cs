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
using Tesorerias.Entidades;
using Comunes.Entidades;
using Bancos.Entidades;

namespace IU.Modulos.Tesoreria
{
    public partial class BancosCuentasConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.BancoCuentaModificarDatosAceptar += new IU.Modulos.Tesoreria.Controles.BancosCuentasDatos.BancosCuentasDatosAceptarEventHandler(ModificarDatos_BancoCuentaModificarDatosAceptar);
            this.ModificarDatos.BancoCuentaModificarDatosCancelar += new IU.Modulos.Tesoreria.Controles.BancosCuentasDatos.BancosCuentasDatosCancelarEventHandler(ModificarDatos_BancoCuentaModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                TESBancosCuentas bancoCuenta = new TESBancosCuentas();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdBancoCuenta"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdBancoCuenta"]);
                bancoCuenta.IdBancoCuenta = parametro;

                this.ModificarDatos.IniciarControl(bancoCuenta, Gestion.Consultar);
            }
        }

        void ModificarDatos_BancoCuentaModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasListar.aspx"), true);
        }

        void ModificarDatos_BancoCuentaModificarDatosAceptar(object sender, TESBancosCuentas e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasListar.aspx"), true);
        }
    }
}