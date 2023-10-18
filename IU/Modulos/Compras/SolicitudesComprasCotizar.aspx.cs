using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Compras.Entidades;

namespace IU.Modulos.Compras
{
    public partial class SolicitudesComprasCotizar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.SolicitudesComprasModificarDatosAceptar += new Controles.SolicitudesComprasDatos.SolicitudesComprasDatosAceptarEventHandler(ModificarDatos_SolicitudModificarDatosAceptar);
            this.ModificarDatos.SolicitudesComprasModificarDatosCancelar += new Controles.SolicitudesComprasDatos.SolicitudesComprasDatosCancelarEventHandler(ModificarDatos_SolicitudModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                CmpSolicitudesCompras solicitud = new CmpSolicitudesCompras();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdSolicitudCompra"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/SolicitudesComprasListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdSolicitudCompra"]);
                solicitud.IdSolicitudCompra = parametro;

                this.ModificarDatos.IniciarControl(solicitud, Gestion.Modificar);
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