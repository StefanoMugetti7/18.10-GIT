using Comunes.Entidades;
using Facturas.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Seguridad
{
    public partial class SegSectoresPuntosVentasModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                VTASectoresPuntosVentas panteon = new VTASectoresPuntosVentas();

                if (!this.MisParametrosUrl.Contains("IdSectorPuntoVenta"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Seguridad/SegSectoresPuntosVentasListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdSectorPuntoVenta"]);
                panteon.IdSectorPuntoVenta = parametro;

                ModificarDatos.IniciarControl(panteon, Gestion.Modificar);
            }
        }

        private void ModificarDatos_ControlModificarDatosCancelar()
        {
            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Seguridad/SegSectoresPuntosVentasListar.aspx");
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString());
            else
                this.Response.Redirect(url, true);
        }
    }
}