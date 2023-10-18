using Bancos.Entidades;
using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Bancos
{
    public partial class BancosLotesFinalizar: PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos2.BancoLotesEnviadosModificarDatosAceptar += new IU.Modulos.Bancos.Controles.BancosLotesDatos.BancoLotesEnviadosDatosAceptarEventHandler(ModificarDatos_BancosLotesEnviadosModificarDatosAceptar);
            this.ModificarDatos2.BancoLotesEnviadosModificarDatosCancelar += new IU.Modulos.Bancos.Controles.BancosLotesDatos.BancoLotesEnviadosDatosCancelarEventHandler(ModificarDatos_BancosLotesEnviadosModificarDatosCancelar);
            if (!IsPostBack)
            {
                TESBancosLotesEnviados bancoLote = new TESBancosLotesEnviados();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdBancoLoteEnvio"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosLotesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdBancoLoteEnvio"]);
                bancoLote.IdBancoLoteEnvio = parametro;

                this.ModificarDatos2.IniciarControl(bancoLote, Gestion.Anular);
            }
        }

        private void ModificarDatos_BancosLotesEnviadosModificarDatosAceptar(object sender, TESBancosLotesEnviados e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosLotesListar.aspx"), true);
        }

        private void ModificarDatos_BancosLotesEnviadosModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosLotesListar.aspx"), true);
        }
    }
}