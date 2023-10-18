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
    public partial class CuentasContablesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.AgregarDatos.CuentaContableDatosAceptar += new Controles.CuentasContablesDatos.CuentaContableDatosAceptarEventHandler(AgregarDatos_CuentaContableDatosAceptar);
            //this.AgregarDatos.CuentaContableDatosCancelar += new Controles.CuentasContablesDatos.CuentaContableDatosCancelarEventHandler(AgregarDatos_CuentaContableDatosCancelar);
            if (!this.IsPostBack)
            {
                this.AgregarDatos.IniciarControl(new CtbCuentasContables(), Gestion.Agregar);
            }
        }

        //protected void AgregarDatos_CuentaContableDatosCancelar()
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CuentasContablesListar.aspx"), true);
        //}

        //protected void AgregarDatos_CuentaContableDatosAceptar(object sender, global::Contabilidad.Entidades.CtbCuentasContables e)
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CuentasContablesListar.aspx"), true);
        //}
    }
}