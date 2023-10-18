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

namespace IU.Modulos.Seguridad
{
    public partial class SegMenuesModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.MenuesModificarDatosAceptar += new IU.Modulos.Seguridad.Controles.SegMenuesModificarDatos.SegMenuesModificarDatosAceptarEventHandler(ModificarDatos_MenuesModificarDatosAceptar);
            this.ModificarDatos.MenuesModificarDatosCancelar += new IU.Modulos.Seguridad.Controles.SegMenuesModificarDatos.SegMenuesModificarDatosCancelarEventHandler(ModificarDatos_MenuesModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                if (!this.MisParametrosUrl.Contains("IdMenu"))
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Seguridad/SegMenuesListar.aspx"), true);
                Menues menu = new Menues();
                menu.IdMenu = Convert.ToInt32(this.MisParametrosUrl["IdMenu"]);
                this.ModificarDatos.IniciarControl(menu, Gestion.Modificar);
            }
        }

        void ModificarDatos_MenuesModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Seguridad/SegMenuesListar.aspx"), true);
        }

        void ModificarDatos_MenuesModificarDatosAceptar(object sender, Menues e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Seguridad/SegMenuesListar.aspx"), true);
        }
    }
}
