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

namespace IU.Modulos.TGE
{
    public partial class TiposFuncionalidadesPorEstadosModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrModifDatos.TiposFuncionalidadesPorEstadosDatosCancelar += new IU.Modulos.TGE.Control.TiposFuncionalidadesPorEstadosModificarDatos.TiposFuncionalidadesPorEstadosDatosCancelarEventHandler(ctrModifDatos_TiposFuncionalidadesPorEstadosDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ctrModifDatos.IniciarControl(Gestion.Modificar);
            }
        }

        void ctrModifDatos_TiposFuncionalidadesPorEstadosDatosCancelar()
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/IngresoSistema.aspx"), true);
        }
    }
}
