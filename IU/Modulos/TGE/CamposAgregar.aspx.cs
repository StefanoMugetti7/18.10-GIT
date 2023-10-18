using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Generales.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.TGE
{
    public partial class CamposAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrDaos.CampoModificarDatosAceptar += new Control.CamposDatos.CamposDatosAceptarEventHandler(ctrDaos_CampoModificarDatosAceptar);
            this.ctrDaos.CampoModificarDatosCancelar += new Control.CamposDatos.CamposDatosCancelarEventHandler(ctrDaos_CampoModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ctrDaos.IniciarControl(new TGECampos(), Gestion.Agregar);
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
