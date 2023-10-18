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
    public partial class CargosAfiliadosAgregar : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            this.AgregarDatos.CargoAfiliadoModificarDatosAceptar += new IU.Modulos.Cargos.Controles.CargoAfiliadoModificarDatos.CargoAfiliadoAceptarEventHandler(AgregarDatos_CargoAfiliadoModificarDatosAceptar);
            this.AgregarDatos.CargoAfiliadoModificarDatosCancelar += new IU.Modulos.Cargos.Controles.CargoAfiliadoModificarDatos.CargoAfiliadoCancelarEventHandler(AgregarDatos_CargoAfiliadoModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                CarTiposCargosAfiliadosFormasCobros cargoAfiliado = new CarTiposCargosAfiliadosFormasCobros();
                cargoAfiliado.IdAfiliado = this.MiAfiliado.IdAfiliado;
                this.AgregarDatos.IniciarControl(cargoAfiliado, Gestion.Agregar);
            }
        }

        protected void AgregarDatos_CargoAfiliadoModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosListar.aspx"), true);
        }

        protected void AgregarDatos_CargoAfiliadoModificarDatosAceptar(object sender, global::Cargos.Entidades.CarTiposCargosAfiliadosFormasCobros e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosListar.aspx"), true);
        }
    }
}
