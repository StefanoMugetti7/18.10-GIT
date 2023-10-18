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
using Ahorros.Entidades;
using Generales.Entidades;

namespace IU.Modulos.Ahorros
{
    public partial class CuentasMovimientosExtraer : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            this.ExtraerDatos.CuentaMovimientosModificarDatosAceptar += new IU.Modulos.Ahorros.Controles.CuentasMovimientosModificarDatos.CuentaMovimientosDatosAceptarEventHandler(ExtraerDatos_CuentaMovimientosModificarDatosAceptar);
            this.ExtraerDatos.CuentaMovimientosModificarDatosCancelar += new IU.Modulos.Ahorros.Controles.CuentasMovimientosModificarDatos.CuentaMovimientosDatosCancelarEventHandler(ExtraerDatos_CuentaMovimientosModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                AhoCuentasMovimientos cuentasMovimientos = new AhoCuentasMovimientos();
                cuentasMovimientos.Cuenta.Afiliado = this.MiAfiliado;
                this.ExtraerDatos.IniciarControl(cuentasMovimientos, EnumTGETiposMovimientos.Debito);
            }
        }

        protected void ExtraerDatos_CuentaMovimientosModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasListar.aspx"), true);
        }

        protected void ExtraerDatos_CuentaMovimientosModificarDatosAceptar(object sender, global::Ahorros.Entidades.AhoCuentasMovimientos e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdCuenta", e.Cuenta.IdCuenta);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasMovimientosListar.aspx"), true);
        }
    }
}
