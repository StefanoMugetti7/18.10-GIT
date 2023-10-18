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
using Afiliados.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Afiliados
{
    public partial class ApoderadosAgregar : PaginaAfiliados
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModifDatos.AfiliadosModificarDatosAceptar += new IU.Modulos.Afiliados.Controles.AfiliadoModificarDatos.AfiliadoDatosAceptarEventHandler(ModifDatos_AfiliadosModificarDatosAceptar);
            this.ModifDatos.AfiliadosModificarDatosCancelar += new IU.Modulos.Afiliados.Controles.AfiliadoModificarDatos.AfiliadoDatosCancelarEventHandler(ModifDatos_AfiliadosModificarDatosCancelar);

            if (!this.IsPostBack)
            {
                AfiAfiliados familiar = new AfiAfiliados();
                AfiAfiliados titular = this.MiAfiliado;
                familiar.IdAfiliadoRef = titular.IdAfiliado;
                familiar.AfiliadoTipo.IdAfiliadoTipo = (int)EnumAfiliadosTipos.Apoderados;
                familiar.NumeroSocio = titular.NumeroSocio;
                this.ModifDatos.IniciarControl(familiar, Gestion.Agregar);
            }
        }

        void ModifDatos_AfiliadosModificarDatosCancelar()
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosModificar.aspx"), true);
        }

        void ModifDatos_AfiliadosModificarDatosAceptar(object sender, global::Afiliados.Entidades.AfiAfiliados e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosModificar.aspx"), true);
        }
    }
}
