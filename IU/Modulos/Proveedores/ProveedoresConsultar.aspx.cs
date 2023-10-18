using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Proveedores.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Proveedores
{
    public partial class ProveedoresConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ProveedorModificarDatosAceptar += new Controles.ProveedoresDatos.ProveedoresDatosAceptarEventHandler(ModificarDatos_ProveedorModificarDatosAceptar);
            this.ModificarDatos.ProveedorModificarDatosCancelar += new Controles.ProveedoresDatos.ProveedoresDatosCancelarEventHandler(ModificarDatos_ProveedorModificarDatosCancelar);

            if (!this.IsPostBack)
            {
                CapProveedores proveedor = new CapProveedores();
                //Control y Validacion de Parametros
                //if (!this.MisParametrosUrl.Contains("IdProveedor"))
                ListaParametros listaparametros = new ListaParametros(this.MiSessionPagina);
                if (!listaparametros.Existe("IdProveedor"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Proveedores/ProveedoresListar.aspx"), true);

                //int parametro = Convert.ToInt32(this.MisParametrosUrl["IdProveedor"]);
                int parametro = listaparametros.ObtenerValor("IdProveedor");
                proveedor.IdProveedor = parametro;

                this.ModificarDatos.IniciarControl(proveedor, Gestion.Consultar);
            }
        }

        void ModificarDatos_ProveedorModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Proveedores/ProveedoresListar.aspx"), true);
        }

        void ModificarDatos_ProveedorModificarDatosAceptar(object sender, global::Proveedores.Entidades.CapProveedores e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Proveedores/ProveedoresListar.aspx"), true);
        }
    }
}