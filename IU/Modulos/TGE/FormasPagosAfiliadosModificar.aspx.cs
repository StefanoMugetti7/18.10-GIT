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
using Generales.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.TGE
{
    public partial class FormasPagosAfiliadosModificar : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            this.ctrFormasPagoAfiliadoDatos.FormasPagosAfiliadosModificarDatosAceptar += new IU.Modulos.TGE.Control.FormasPagosAfiliadosDatos.FormasPagosAfiliadosAceptarEventHandler(ctrFormasPagoAfiliadoDatos_FormasPagosAfiliadosModificarDatosAceptar);
            this.ctrFormasPagoAfiliadoDatos.FormasPagosAfiliadosModificarDatosCancelar += new IU.Modulos.TGE.Control.FormasPagosAfiliadosDatos.FormasPagosAfiliadosCancelarEventHandler(ctrFormasPagoAfiliadoDatos_FormasPagosAfiliadosModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                if (!this.MisParametrosUrl.Contains("IdFormaPagoAfiliado"))
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasPagosAfiliadosListar.aspx"), true);
                TGEFormasPagosAfiliados cobroAfiliado = new TGEFormasPagosAfiliados();
                cobroAfiliado.IdFormaPagoAfiliado = Convert.ToInt32(this.MisParametrosUrl["IdFormaPagoAfiliado"]);
                this.ctrFormasPagoAfiliadoDatos.IniciarControl(cobroAfiliado, Gestion.Modificar);
            }
        }

        void ctrFormasPagoAfiliadoDatos_FormasPagosAfiliadosModificarDatosAceptar(object sender, Generales.Entidades.TGEFormasPagosAfiliados e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasPagosAfiliadosListar.aspx"), true);
        }

        void ctrFormasPagoAfiliadoDatos_FormasPagosAfiliadosModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasPagosAfiliadosListar.aspx"), true);
        }
    }
}
