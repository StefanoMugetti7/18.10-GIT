using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Generales.Entidades;

namespace IU.Modulos.TGE
{
    public partial class CamposModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrDaos.CampoModificarDatosAceptar += new Control.CamposDatos.CamposDatosAceptarEventHandler(ctrDaos_CampoModificarDatosAceptar);
            this.ctrDaos.CampoModificarDatosCancelar += new Control.CamposDatos.CamposDatosCancelarEventHandler(ctrDaos_CampoModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                TGECampos campo = new TGECampos();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdCampo"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/CamposListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdCampo"]);
                campo.IdCampo = parametro;
                this.ctrDaos.IniciarControl(campo, Gestion.Modificar);
            }
        }

        void ctrDaos_CampoModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/CamposListar.aspx"), true);
        }

        void ctrDaos_CampoModificarDatosAceptar(object sender, TGECampos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/CamposListar.aspx"), true);
        }
    }
}