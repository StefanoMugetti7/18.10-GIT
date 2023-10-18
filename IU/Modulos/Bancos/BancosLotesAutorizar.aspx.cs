using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuentasPagar.Entidades;
using Comunes.Entidades;
using Bancos.Entidades;

namespace IU.Modulos.Bancos
{
    public partial class BancosLotesAutorizar : PaginaSegura
    {

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.BancoLotesEnviadosModificarDatosAceptar += new IU.Modulos.Bancos.Controles.BancosLotesDatos.BancoLotesEnviadosDatosAceptarEventHandler(ModificarDatos_BancosLotesEnviadosModificarDatosAceptar);
            this.ModificarDatos.BancoLotesEnviadosModificarDatosCancelar += new IU.Modulos.Bancos.Controles.BancosLotesDatos.BancoLotesEnviadosDatosCancelarEventHandler(ModificarDatos_BancosLotesEnviadosModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                TESBancosLotesEnviados parametro = new TESBancosLotesEnviados();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdBancoLoteEnvio"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosLotesListar.aspx"), true);

                int valor = Convert.ToInt32(this.MisParametrosUrl["IdBancoLoteEnvio"]);
                parametro.IdBancoLoteEnvio = valor;
                this.ModificarDatos.IniciarControl(parametro, Gestion.Autorizar);
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