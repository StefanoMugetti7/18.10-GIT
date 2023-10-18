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
using Bancos.Entidades;

namespace IU.Modulos.Bancos
{
    public partial class BancosLotesConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.BancoLotesEnviadosModificarDatosAceptar += new IU.Modulos.Bancos.Controles.BancosLotesDatos.BancoLotesEnviadosDatosAceptarEventHandler(ModificarDatos_BancosLotesEnviadosModificarDatosAceptar);
            this.ModificarDatos.BancoLotesEnviadosModificarDatosCancelar += new IU.Modulos.Bancos.Controles.BancosLotesDatos.BancoLotesEnviadosDatosCancelarEventHandler(ModificarDatos_BancosLotesEnviadosModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                TESBancosLotesEnviados bancoCuenta = new TESBancosLotesEnviados();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdBancoLoteEnvio"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosLotesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdBancoLoteEnvio"]);
                bancoCuenta.IdBancoLoteEnvio = parametro;

                this.ModificarDatos.IniciarControl(bancoCuenta, Gestion.Consultar);
            }
        }

        void ModificarDatos_BancosLotesEnviadosModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosLotesListar.aspx"), true);
        }

        void ModificarDatos_BancosLotesEnviadosModificarDatosAceptar(object sender, TESBancosLotesEnviados e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosLotesListar.aspx"), true);
        }
    }
}