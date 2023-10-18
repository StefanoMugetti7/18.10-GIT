using Ahorros.Entidades;
using Comunes.Entidades;
using System;

namespace IU.Modulos.Ahorros
{
    public partial class CuentasAhorrosTiposAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.AgregarDatos.AhorroModificarDatosAceptar += new IU.Modulos.Ahorros.Controles.CuentasAhorrosTiposDatos.AhorroDatosAceptarEventHandler(AgregarDatos_AhorroModificarDatosAceptar);
            this.AgregarDatos.AhorroModificarDatosCancelar += new IU.Modulos.Ahorros.Controles.CuentasAhorrosTiposDatos.AhorroDatosCancelarEventHandler(AgregarDatos_AhorroModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                AhoCuentasTipos cuentas = new AhoCuentasTipos();
                this.AgregarDatos.IniciarControl(cuentas, Gestion.Agregar);
                //cuentas.Afiliado = this.MiAfiliado;
            }
        }

        protected void AgregarDatos_AhorroModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasAhorrosTiposListar.aspx"), true);
        }

        protected void AgregarDatos_AhorroModificarDatosAceptar(object sender, global::Ahorros.Entidades.AhoCuentasTipos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasAhorrosTiposListar.aspx"), true);
        }
    }
}
