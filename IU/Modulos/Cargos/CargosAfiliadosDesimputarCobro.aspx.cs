using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cargos.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Cargos
{
    public partial class CargosAfiliadosDesimputarCobro : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            this.ModificarDatos.ModificarDatosAceptar += new Controles.CargoAfiliadoModificarDatosFormaCobro.CargoAfiliadoModificarDatosFormaCobroAceptarEventHandler(ModificarDatos_ModificarDatosAceptar);
            this.ModificarDatos.ModificarDatosCancelar += new Controles.CargoAfiliadoModificarDatosFormaCobro.CargoAfiliadoModificarDatosFormaCobroCancelarEventHandler(ModificarDatos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                CarCuentasCorrientes cargoAfiliado = new CarCuentasCorrientes();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdCuentaCorriente"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdCuentaCorriente"]);
                cargoAfiliado.IdCuentaCorriente = parametro;
                cargoAfiliado.IdAfiliado = this.MiAfiliado.IdAfiliado;
                this.ModificarDatos.IniciarControl(cargoAfiliado, Gestion.AnularConfirmar);
            }
        }

        void ModificarDatos_ModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosListar.aspx"), true);
        }

        void ModificarDatos_ModificarDatosAceptar(global::Cargos.Entidades.CarCuentasCorrientes e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosListar.aspx"), true);
        }
    }
}