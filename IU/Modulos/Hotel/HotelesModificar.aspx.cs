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
    public partial class HotelesModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                HTLHoteles hotel = new HTLHoteles();

                if (!this.MisParametrosUrl.Contains("IdHotel"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/HotelesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdHotel"]);
                hotel.IdHotel = parametro;

                ModificarDatos.IniciarControl(hotel, Gestion.Modificar);
            }
        }

        private void ModificarDatos_ControlModificarDatosCancelar()
        {
            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/HotelesListar.aspx");
            //if (this.ViewState["UrlReferrer"] != null)
            //    this.Response.Redirect(this.ViewState["UrlReferrer"].ToString());
            //else
                this.Response.Redirect(url, true);
        }
    }
}