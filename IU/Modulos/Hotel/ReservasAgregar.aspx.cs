using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Hoteles.Entidades;

namespace IU.Modulos.Hotel
{
    public partial class ReservasAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                if (this.MisParametrosUrl.Contains("HTLReservas"))
                {
                    HTLReservas reserva = (HTLReservas)this.MisParametrosUrl["HTLReservas"];
                    this.ModificarDatos.IniciarControl(reserva, Gestion.Copiar);
                }
                else
                    this.ModificarDatos.IniciarControl(new HTLReservas(), Gestion.Agregar);
            }
        }

        private void ModificarDatos_ControlModificarDatosCancelar()
        {
            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/ReservasAgenda.aspx");
            string urlReferrer = this.ViewState["UrlReferrer"] != null ? this.ViewState["UrlReferrer"].ToString() : string.Empty;
            if (urlReferrer.Length > 0 && !(urlReferrer.Contains("Facturas") || urlReferrer.Contains("Cobros")))
                this.Response.Redirect(urlReferrer, true);
            else
                this.Response.Redirect(url, true);
        }
    }
}