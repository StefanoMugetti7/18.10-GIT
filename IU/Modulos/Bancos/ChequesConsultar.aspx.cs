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
using Bancos;
using Comunes.Entidades;

namespace IU.Modulos.Bancos
{
    public partial class ChequesConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ChequesModificarDatosAceptar += new Controles.ChequesModificarDatos.ChequesModificarDatosAceptarEventHandler(ModificarDatos_ChequesModificarDatosAceptar);
            this.ModificarDatos.ChequesModificarDatosCancelar += new Controles.ChequesModificarDatos.ChequesModificarDatosCancelarEventHandler(ModificarDatos_ChequesModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                TESCheques cheque = new TESCheques();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdCheque"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/ChequesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdCheque"]);
                cheque.IdCheque = parametro;

                this.ModificarDatos.IniciarControl(cheque, Gestion.Consultar);
            }
        }

        void ModificarDatos_ChequesModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/ChequesListar.aspx"), true);
        }

        void ModificarDatos_ChequesModificarDatosAceptar(TESCheques e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/ChequesListar.aspx"), true);
        }
    }
}