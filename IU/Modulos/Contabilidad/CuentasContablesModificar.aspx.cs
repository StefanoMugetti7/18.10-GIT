using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Contabilidad
{
    public partial class CuentasContablesModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ModificarDatos.CuentaContableDatosAceptar += new Controles.CuentasContablesDatos.CuentaContableDatosAceptarEventHandler(ModificarDatos_CuentaContableDatosAceptar);
            //this.ModificarDatos.CuentaContableDatosCancelar += new Controles.CuentasContablesDatos.CuentaContableDatosCancelarEventHandler(ModificarDatos_CuentaContableDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdCuentaContable"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CuentasContablesListar.aspx"), true);

                CtbCuentasContables cuentaContable = new CtbCuentasContables();
                cuentaContable.IdCuentaContable = Convert.ToInt32(this.MisParametrosUrl["IdCuentaContable"]);
                this.ModificarDatos.IniciarControl(cuentaContable, Gestion.Modificar);
            }
        }

        //protected void ModificarDatos_CuentaContableDatosCancelar()
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CuentasContablesListar.aspx"), true);
        //}

        //protected void ModificarDatos_CuentaContableDatosAceptar(object sender, global::Contabilidad.Entidades.CtbCuentasContables e)
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CuentasContablesListar.aspx"), true);
        //}
    }
}