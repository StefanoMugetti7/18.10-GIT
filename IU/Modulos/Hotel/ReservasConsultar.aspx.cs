using Comunes.Entidades;
using Hoteles.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Hotel
{
    public partial class ReservasConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                HTLReservas reserva = new HTLReservas();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdReserva"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/ReservasListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdReserva"]);
                reserva.IdReserva = parametro;

                ModificarDatos.IniciarControl(reserva, Gestion.Consultar);
            }
        }

        private void ModificarDatos_ControlModificarDatosCancelar()
        {
            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/ReservasListar.aspx");
            //if (this.ViewState["UrlReferrer"] != null)
            //    this.Response.Redirect(this.ViewState["UrlReferrer"].ToString());
            //else
                this.Response.Redirect(url, true);
        }
    }
}