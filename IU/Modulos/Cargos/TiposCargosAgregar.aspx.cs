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
using Generales.FachadaNegocio;
using Cargos;
using Cargos.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Cargos
{
    public partial class TiposCargosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.AgregarDatos.TipoCargoModificarDatosAceptar += new IU.Modulos.Cargos.Controles.TipoCargoModificarDatos.AfiliadoDatosAceptarEventHandler(AgregarDatos_TipoCargoModificarDatosAceptar);
            this.AgregarDatos.TipoCargoModificarDatosCancelar += new IU.Modulos.Cargos.Controles.TipoCargoModificarDatos.AfiliadoDatosCancelarEventHandler(AgregarDatos_TipoCargoModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.AgregarDatos.IniciarControl(new CarTiposCargos(), Gestion.Agregar);
            }
        }

        void AgregarDatos_TipoCargoModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/TiposCargosListar.aspx"), true);
        }

        void AgregarDatos_TipoCargoModificarDatosAceptar(object sender, CarTiposCargos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/TiposCargosListar.aspx"), true);
        }
    }
}
