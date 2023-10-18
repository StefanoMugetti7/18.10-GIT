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
using Tesorerias.Entidades;
using Bancos.Entidades;

namespace IU.Modulos.Tesoreria
{
    public partial class BancosCuentasAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos2.BancoCuentaModificarDatosAceptar += new IU.Modulos.Tesoreria.Controles.BancosCuentasDatos.BancosCuentasDatosAceptarEventHandler(ModificarDatos_BancoCuentaModificarDatosAceptar);
            this.ModificarDatos2.BancoCuentaModificarDatosCancelar += new IU.Modulos.Tesoreria.Controles.BancosCuentasDatos.BancosCuentasDatosCancelarEventHandler(ModificarDatos_BancoCuentaModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ModificarDatos2.IniciarControl(new TESBancosCuentas(), Gestion.Agregar);
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