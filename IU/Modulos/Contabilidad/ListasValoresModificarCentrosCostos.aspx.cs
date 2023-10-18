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
using Generales.FachadaNegocio;

namespace IU.Modulos.Contabilidad
{
    public partial class ListasValoresModificarCentrosCostos : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ListasValoresDatosAceptar += new IU.Modulos.TGE.Control.ListasValoresDetalles.ListasValoresDatosAceptarEventHandler(ModificarDatos_ListasValoresDatosAceptar);
            this.ModificarDatos.ListasValoresDatosCancelar += new IU.Modulos.TGE.Control.ListasValoresDetalles.ListasValoresDatosCancelarEventHandler(ModificarDatos_ListasValoresDatosCancelar);
            if (!this.IsPostBack)
            {
                TGEListasValores lista = new TGEListasValores();
                lista = TGEGeneralesF.ListasValoresObtenerLista(EnumTGEListasValoresCodigos.CentrosCostos);
                this.ModificarDatos.IniciarControl(lista, Gestion.Modificar);
            }
        }

        void ModificarDatos_ListasValoresDatosAceptar(object sender, TGEListasValores e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
        }

        void ModificarDatos_ListasValoresDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
        }
    }
}