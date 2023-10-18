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
    public partial class CuentasAgregar : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            this.AgregarDatos.AhorroModificarDatosAceptar += new IU.Modulos.Ahorros.Controles.CuentasModificarDatos.AhorroDatosAceptarEventHandler(AgregarDatos_AhorroModificarDatosAceptar);
            this.AgregarDatos.AhorroModificarDatosCancelar += new IU.Modulos.Ahorros.Controles.CuentasModificarDatos.AhorroDatosCancelarEventHandler(AgregarDatos_AhorroModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                AhoCuentas cuentas = new AhoCuentas();
                cuentas.Afiliado = this.MiAfiliado;
                this.AgregarDatos.IniciarControl(cuentas, Gestion.Agregar);
            }
        }

        protected void AgregarDatos_AhorroModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasListar.aspx"), true);
        }

        protected void AgregarDatos_AhorroModificarDatosAceptar(object sender, global::Ahorros.Entidades.AhoCuentas e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasListar.aspx"), true);
        }
    }
}
