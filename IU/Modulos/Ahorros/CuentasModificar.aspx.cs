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
using Comunes.Entidades;

namespace IU.Modulos.Ahorros
{
    public partial class CuentasModificar : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            this.ModificarDatos.AhorroModificarDatosAceptar += new IU.Modulos.Ahorros.Controles.CuentasModificarDatos.AhorroDatosAceptarEventHandler(ModificarDatos_AhorroModificarDatosAceptar);
            this.ModificarDatos.AhorroModificarDatosCancelar += new IU.Modulos.Ahorros.Controles.CuentasModificarDatos.AhorroDatosCancelarEventHandler(ModificarDatos_AhorroModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                AhoCuentas cuentas = new AhoCuentas();

                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdCuenta"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdCuenta"]);
                cuentas.IdCuenta = parametro;
                cuentas.Afiliado = this.MiAfiliado;
                this.ModificarDatos.IniciarControl(cuentas, Gestion.Modificar);
            }
        }

        protected void ModificarDatos_AhorroModificarDatosAceptar(object sender, global::Ahorros.Entidades.AhoCuentas e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasListar.aspx"), true);
        }

        protected void ModificarDatos_AhorroModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasListar.aspx"), true);
        }
    }
}
