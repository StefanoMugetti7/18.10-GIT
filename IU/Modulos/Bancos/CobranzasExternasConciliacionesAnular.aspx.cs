using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Bancos.Entidades;

namespace IU.Modulos.Bancos
{
    public partial class CobranzasExternasConciliacionesAnular : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.CobranzasModificarDatosAceptar += new IU.Modulos.Bancos.Controles.CobranzasExternasConciliacionesDatos.CobranzasModificarDatosAceptarEventHandler(ModificarDatos_CobranzasExternasConciliacionesModificarDatosAceptar);
            this.ModificarDatos.CobranzasModificarDatosCancelar += new IU.Modulos.Bancos.Controles.CobranzasExternasConciliacionesDatos.CobranzasModificarDatosCancelarEventHandler(ModificarDatos_CobranzasExternasConciliacionesModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                TESCobranzasExternasConciliaciones pCobranza = new TESCobranzasExternasConciliaciones();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdCobranzaExternaConciliacion"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/CobranzasExternasConciliacionesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdCobranzaExternaConciliacion"]);
                pCobranza.IdCobranzaExternaConciliacion = parametro;
                this.ModificarDatos.IniciarControl(pCobranza, Gestion.Anular);
            }
        }
        
        void ModificarDatos_CobranzasExternasConciliacionesModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/CobranzasExternasConciliacionesListar.aspx"), true);
        }

        void ModificarDatos_CobranzasExternasConciliacionesModificarDatosAceptar(object sender, TESCobranzasExternasConciliaciones e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/CobranzasExternasConciliacionesListar.aspx"), true);
        }
    }
}