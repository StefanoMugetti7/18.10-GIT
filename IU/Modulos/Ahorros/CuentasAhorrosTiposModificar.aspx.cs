using Ahorros.Entidades;
using Comunes.Entidades;
using System;

namespace IU.Modulos.Ahorros
{
    public partial class CuentasAhorrosTiposModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            this.ModificarDatos.AhorroModificarDatosAceptar += new IU.Modulos.Ahorros.Controles.CuentasAhorrosTiposDatos.AhorroDatosAceptarEventHandler(ModificarDatos_AhorroModificarDatosAceptar);
            this.ModificarDatos.AhorroModificarDatosCancelar += new IU.Modulos.Ahorros.Controles.CuentasAhorrosTiposDatos.AhorroDatosCancelarEventHandler(ModificarDatos_AhorroModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                AhoCuentasTipos cuentas = new AhoCuentasTipos();

                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdCuentaTipo"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasAhorrosTiposListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdCuentaTipo"]);
                cuentas.IdCuentaTipo = parametro;
                this.ModificarDatos.IniciarControl(cuentas, Gestion.Modificar);
            }
        }

        protected void ModificarDatos_AhorroModificarDatosAceptar(object sender, global::Ahorros.Entidades.AhoCuentasTipos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasAhorrosTiposListar.aspx"), true);
        }

        protected void ModificarDatos_AhorroModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasAhorrosTiposListar.aspx"), true);
        }
    }
}
