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
using Bancos.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Bancos
{
    public partial class BancosCuentasMovimientosConciliar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.BancoCuentaModificarDatosCancelar += new IU.Modulos.Bancos.Controles.BancosCuentasMovimientosListar.BancosCuentasDatosCancelarEventHandler(ModificarDatos_BancoCuentaModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                TESBancosCuentas bancoCuenta = new TESBancosCuentas();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdBancoCuenta"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdBancoCuenta"]);
                bancoCuenta.IdBancoCuenta = parametro;

                this.ModificarDatos.IniciarControl(bancoCuenta, Gestion.Modificar);
            }
        }

        void ModificarDatos_BancoCuentaModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasListar.aspx"), true);
        }
    }
}