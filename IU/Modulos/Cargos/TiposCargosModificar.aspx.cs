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
    public partial class TiposCargosModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.TipoCargoModificarDatosAceptar += new IU.Modulos.Cargos.Controles.TipoCargoModificarDatos.AfiliadoDatosAceptarEventHandler(ModificarDatos_TipoCargoModificarDatosAceptar);
            this.ModificarDatos.TipoCargoModificarDatosCancelar += new IU.Modulos.Cargos.Controles.TipoCargoModificarDatos.AfiliadoDatosCancelarEventHandler(ModificarDatos_TipoCargoModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                if (!this.MisParametrosUrl.Contains("IdCargo"))
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/TiposCargosListar.aspx"), true);
                CarTiposCargos tipoCargos = new CarTiposCargos();
                tipoCargos.IdTipoCargo = Convert.ToInt32(this.MisParametrosUrl["IdCargo"]);
                this.ModificarDatos.IniciarControl(tipoCargos, Gestion.Modificar);                
            }
        }

        void ModificarDatos_TipoCargoModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/TiposCargosListar.aspx"), true);
        }

        void ModificarDatos_TipoCargoModificarDatosAceptar(object sender, CarTiposCargos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/TiposCargosListar.aspx"), true);
        }
    }
}
