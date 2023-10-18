using Comunes.Entidades;
using LavaYa.Entidades;
using System;

namespace IU.Modulos.LavaYa
{
    public partial class PuntosVentasConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos2.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                LavPuntosVentas puntoVenta = new LavPuntosVentas();

                if (!this.MisParametrosUrl.Contains("IdPuntoVenta"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/LavaYa/PuntosVentasListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdPuntoVenta"]);
                puntoVenta.IdPuntoVenta = parametro;

                ModificarDatos2.IniciarControl(puntoVenta, Gestion.Consultar);
            }
        }

        private void ModificarDatos_ControlModificarDatosCancelar()
        {
            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/LavaYa/PuntosVentasListar.aspx");
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString());
            else
                this.Response.Redirect(url, true);
        }
    }
}
