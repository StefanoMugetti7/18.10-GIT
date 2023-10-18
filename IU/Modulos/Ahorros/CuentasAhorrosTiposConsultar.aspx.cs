using Ahorros.Entidades;
using Comunes.Entidades;
using System;

namespace IU.Modulos.Ahorros
{
    public partial class CuentasAhorrosTiposConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ConsultarDatos.AhorroModificarDatosAceptar += new IU.Modulos.Ahorros.Controles.CuentasAhorrosTiposDatos.AhorroDatosAceptarEventHandler(ConsultarDatos_AhorroModificarDatosAceptar);
            this.ConsultarDatos.AhorroModificarDatosCancelar += new IU.Modulos.Ahorros.Controles.CuentasAhorrosTiposDatos.AhorroDatosCancelarEventHandler(ConsultarDatos_AhorroModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                AhoCuentasTipos cuentas = new AhoCuentasTipos();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdCuentaTipo"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasAhorrosTiposListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdCuentaTipo"]);
                cuentas.IdCuentaTipo = parametro;
                //cuentas.Afiliado = this.MiAfiliado;
                this.ConsultarDatos.IniciarControl(cuentas, Gestion.Consultar);
            }
        }

        protected void ConsultarDatos_AhorroModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasAhorrosTiposListar.aspx"), true);
        }

        protected void ConsultarDatos_AhorroModificarDatosAceptar(object sender, global::Ahorros.Entidades.AhoCuentasTipos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasAhorrosTiposListar.aspx"), true);
        }
    }
}
