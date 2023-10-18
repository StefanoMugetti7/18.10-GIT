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
    public partial class CargosAfiliadosModificar : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            this.ModificarDatos.CargoAfiliadoModificarDatosAceptar += new IU.Modulos.Cargos.Controles.CargoAfiliadoModificarDatos.CargoAfiliadoAceptarEventHandler(ModificarDatos_CargoAfiliadoModificarDatosAceptar);
            this.ModificarDatos.CargoAfiliadoModificarDatosCancelar += new IU.Modulos.Cargos.Controles.CargoAfiliadoModificarDatos.CargoAfiliadoCancelarEventHandler(ModificarDatos_CargoAfiliadoModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                CarTiposCargosAfiliadosFormasCobros cargoAfiliado = new CarTiposCargosAfiliadosFormasCobros();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdTipoCargoAfiliadoFormaCobro"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdTipoCargoAfiliadoFormaCobro"]);
                cargoAfiliado.IdTipoCargoAfiliadoFormaCobro = parametro;
                cargoAfiliado.IdAfiliado = this.MiAfiliado.IdAfiliado;
                this.ModificarDatos.IniciarControl(cargoAfiliado, Gestion.Modificar);
            }      
        }

        protected void ModificarDatos_CargoAfiliadoModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosListar.aspx"), true);
        }

        protected void ModificarDatos_CargoAfiliadoModificarDatosAceptar(object sender, global::Cargos.Entidades.CarTiposCargosAfiliadosFormasCobros e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosListar.aspx"), true);
        }
    }
}
