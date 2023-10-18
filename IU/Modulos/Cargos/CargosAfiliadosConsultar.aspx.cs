using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Cargos.Entidades;

namespace IU.Modulos.Cargos
{
    public partial class CargosAfiliadosConsultar : PaginaAfiliados
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
                this.ModificarDatos.IniciarControl(cargoAfiliado, Gestion.Consultar);
            }
       
        }

        protected void ModificarDatos_CargoAfiliadoModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosListar.aspx"), true);
        }

        protected void ModificarDatos_CargoAfiliadoModificarDatosAceptar(object sender, global::Cargos.Entidades.CarTiposCargosAfiliadosFormasCobros e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosListar.aspx"), true);
        }
    }
}