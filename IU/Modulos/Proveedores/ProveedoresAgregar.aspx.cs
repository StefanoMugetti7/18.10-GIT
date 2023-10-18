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
    public partial class ProveedoresAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ProveedorModificarDatosAceptar += new Controles.ProveedoresDatos.ProveedoresDatosAceptarEventHandler(ModificarDatos_ProveedorModificarDatosAceptar);
            this.ModificarDatos.ProveedorModificarDatosCancelar += new Controles.ProveedoresDatos.ProveedoresDatosCancelarEventHandler(ModificarDatos_ProveedorModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ModificarDatos.IniciarControl(new CapProveedores(), Gestion.Agregar);
            }
        }

        void ModificarDatos_ProveedorModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Proveedores/ProveedoresListar.aspx"), true);
        }

        void ModificarDatos_ProveedorModificarDatosAceptar(object sender, global::Proveedores.Entidades.CapProveedores e)
        {
            //this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Proveedores/ProveedoresListar.aspx"), true);
            ListaParametros parametros = new ListaParametros(this.MiSessionPagina);
            parametros.Agregar("IdProveedor", e.IdProveedor);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Proveedores/ProveedoresModificar.aspx"), true);
        }
    }
}