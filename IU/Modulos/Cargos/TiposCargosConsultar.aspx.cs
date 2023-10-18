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
using Cargos.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Cargos
{
    public partial class TiposCargosConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.MostrarDatos.TipoCargoModificarDatosAceptar += new IU.Modulos.Cargos.Controles.TipoCargoModificarDatos.AfiliadoDatosAceptarEventHandler(MostrarDatos_TipoCargoModificarDatosAceptar);
            this.MostrarDatos.TipoCargoModificarDatosCancelar += new IU.Modulos.Cargos.Controles.TipoCargoModificarDatos.AfiliadoDatosCancelarEventHandler(MostrarDatos_TipoCargoModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                if (!this.MisParametrosUrl.Contains("IdCargo"))
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/TiposCargosListar.aspx"), true);
                CarTiposCargos tipoCargos = new CarTiposCargos();
                tipoCargos.IdTipoCargo = Convert.ToInt32(this.MisParametrosUrl["IdCargo"]);
                this.MostrarDatos.IniciarControl(tipoCargos, Gestion.Consultar);      
            }
        }

        void MostrarDatos_TipoCargoModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/TiposCargosListar.aspx"), true);
        }

        void MostrarDatos_TipoCargoModificarDatosAceptar(object sender, CarTiposCargos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/TiposCargosListar.aspx"), true);
        }
    }
}
