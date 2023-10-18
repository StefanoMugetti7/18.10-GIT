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
    public partial class FormasCobrosAfiliadosConsultar : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);

            this.ctrFormasCobroAfiliadoDatos.FormasCobrosAfiliadosModificarDatosAceptar += new IU.Modulos.TGE.Control.FormasCobrosAfiliadosDatos.FormasCobrosAfiliadosAceptarEventHandler(ctrFormasCobroAfiliadoDatos_FormasCobrosAfiliadosModificarDatosAceptar);
            this.ctrFormasCobroAfiliadoDatos.FormasCobrosAfiliadosModificarDatosCancelar += new IU.Modulos.TGE.Control.FormasCobrosAfiliadosDatos.FormasCobrosAfiliadosCancelarEventHandler(ctrFormasCobroAfiliadoDatos_FormasCobrosAfiliadosModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                if (!this.MisParametrosUrl.Contains("IdFormaCobroAfiliado"))
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasCobrosAfiliadosListar.aspx"), true);

                TGEFormasCobrosAfiliados cobroAfiliado = new TGEFormasCobrosAfiliados();
                cobroAfiliado.IdFormaCobroAfiliado = Convert.ToInt32(this.MisParametrosUrl["IdFormaCobroAfiliado"]); ;
                this.ctrFormasCobroAfiliadoDatos.IniciarControl(cobroAfiliado, Gestion.Consultar);
            }
        }

        void ctrFormasCobroAfiliadoDatos_FormasCobrosAfiliadosModificarDatosAceptar(object sender, Generales.Entidades.TGEFormasCobrosAfiliados e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasCobrosAfiliadosListar.aspx"), true);
        }

        void ctrFormasCobroAfiliadoDatos_FormasCobrosAfiliadosModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasCobrosAfiliadosListar.aspx"), true);
        }
    }
}