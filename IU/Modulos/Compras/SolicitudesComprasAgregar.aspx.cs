using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Compras
{
    public partial class SolicitudesComprasAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.SolicitudesComprasModificarDatosAceptar += new Controles.SolicitudesComprasDatos.SolicitudesComprasDatosAceptarEventHandler(ModificarDatos_SolicitudModificarDatosAceptar);
            this.ModificarDatos.SolicitudesComprasModificarDatosCancelar += new Controles.SolicitudesComprasDatos.SolicitudesComprasDatosCancelarEventHandler(ModificarDatos_SolicitudModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ModificarDatos.IniciarControl(new CmpSolicitudesCompras(), Gestion.Agregar);
            }
        }

        void ModificarDatos_SolicitudModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/SolicitudesComprasListar.aspx"), true);
        }

        void ModificarDatos_SolicitudModificarDatosAceptar(object sender, CmpSolicitudesCompras e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/SolicitudesComprasListar.aspx"), true);
        }
    }
}