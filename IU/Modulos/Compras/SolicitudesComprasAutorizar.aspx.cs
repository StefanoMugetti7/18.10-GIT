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
    public partial class SolicitudesComprasAutorizar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.SolicitudesComprasModificarDatosAceptar += new Controles.SolicitudesComprasDatos.SolicitudesComprasDatosAceptarEventHandler(ModificarDatos_SolicitudesComprasModificarDatosAceptar);
            this.ModificarDatos.SolicitudesComprasModificarDatosCancelar += new Controles.SolicitudesComprasDatos.SolicitudesComprasDatosCancelarEventHandler(ModificarDatos_SolicitudesComprasModificarDatosCancelar);

            if (!this.IsPostBack)
            {
                CmpSolicitudesCompras solicitud = new CmpSolicitudesCompras();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdSolicitudCompra"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/SolicitudesComprasListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdSolicitudCompra"]);
                solicitud.IdSolicitudCompra = parametro;

                this.ModificarDatos.IniciarControl(solicitud, Gestion.Autorizar);
            }
        }

        void ModificarDatos_SolicitudesComprasModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/SolicitudesComprasListar.aspx"), true);
        }

        void ModificarDatos_SolicitudesComprasModificarDatosAceptar(object sender, global::Compras.Entidades.CmpSolicitudesCompras e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/SolicitudesComprasListar.aspx"), true);
        }
    }
}