using Comunes.Entidades;
using Proveedores.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Proveedores
{
    public partial class ProveedoresPorcentajesComisionesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ModificarDatosAceptar += new Controles.ProveedoresPorcentajesComisionesDatos.ModificarDatosAceptarEventHandler(ModificarDatos_ProveedorModificarDatosAceptar);
            this.ModificarDatos.ModificarDatosCancelar += new Controles.ProveedoresPorcentajesComisionesDatos.ModificarDatosCancelarEventHandler(ModificarDatos_ProveedorModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ModificarDatos.IniciarControl(new CapProveedoresPorcentajesComisiones(), Gestion.Agregar);
            }
        }

        void ModificarDatos_ProveedorModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Proveedores/ProveedoresPorcentajesComisionesListar.aspx"), true);
        }

        void ModificarDatos_ProveedorModificarDatosAceptar(object sender, CapProveedoresPorcentajesComisiones e)
        {
            //this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Proveedores/ProveedoresListar.aspx"), true);
            ListaParametros parametros = new ListaParametros(this.MiSessionPagina);
            parametros.Agregar("IdProveedorPorcentajeComision", e.IdProveedorPorcentajeComision);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Proveedores/ProveedoresPorcentajesComisionesListar.aspx"), true);
        }
    }
}