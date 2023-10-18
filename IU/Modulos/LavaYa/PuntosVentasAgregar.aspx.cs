using Comunes.Entidades;
using Hoteles.Entidades;
using LavaYa.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.LavaYa
{
    public partial class PuntosVentasAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                LavPuntosVentas puntoVenta = new LavPuntosVentas();

                if (this.MisParametrosUrl.Contains("IdPuntoVenta"))
                {
                    int parametro = Convert.ToInt32(this.MisParametrosUrl["IdPuntoVenta"]);
                    puntoVenta.IdPuntoVenta = parametro;
                }

                this.ModificarDatos.IniciarControl(puntoVenta, Gestion.Agregar);
            }
        }

        private void ModificarDatos_ControlModificarDatosCancelar()
        {
            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/LavaYa/EdificiosListar.aspx");
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString());
            else
                this.Response.Redirect(url, true);
        }
    }
}