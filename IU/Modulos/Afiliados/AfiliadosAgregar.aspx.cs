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
using Afiliados.Entidades;
using Afiliados;

namespace IU.Modulos.Afiliados
{
    public partial class AfiliadosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModifDatos.AfiliadosModificarDatosAceptar += new IU.Modulos.Afiliados.Controles.AfiliadoModificarDatos.AfiliadoDatosAceptarEventHandler(ModifDatos_AfiliadosModificarDatosAceptar);
            this.ModifDatos.AfiliadosModificarDatosCancelar += new IU.Modulos.Afiliados.Controles.AfiliadoModificarDatos.AfiliadoDatosCancelarEventHandler(ModifDatos_AfiliadosModificarDatosCancelar);

            if (!this.IsPostBack)
            {
                AfiAfiliados afiliado = new AfiAfiliados();
                afiliado.AfiliadoTipo.IdAfiliadoTipo = (int)EnumAfiliadosTipos.Socios;
                this.ModifDatos.IniciarControl(afiliado, Gestion.Agregar);
            }
        }

        void ModifDatos_AfiliadosModificarDatosCancelar()
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosListar.aspx"), true);
        }

        void ModifDatos_AfiliadosModificarDatosAceptar(object sender, global::Afiliados.Entidades.AfiAfiliados e)
        {
            //Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosDatos.aspx"), true);
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", Gestion.Modificar);
            this.MisParametrosUrl.Add("IdAfiliado", e.IdAfiliado);
            e = AfiliadosF.AfiliadosObtenerDatos(e);
            PaginaAfiliados paginaAfi = new PaginaAfiliados();
            paginaAfi.Guardar(this.MiSessionPagina, e);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosModificar.aspx"), true);
        }
    }
}
