using Comunes.Entidades;
using Producciones.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Produccion
{
    public partial class ProduccionesConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                PrdProducciones produccion = new PrdProducciones();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdProduccion"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Produccion/ProducconesListar.aspx"), true);

                Int64 parametro = Convert.ToInt64(this.MisParametrosUrl["IdProduccion"]);
                produccion.IdProduccion = parametro;

                ModificarDatos.IniciarControl(produccion, Gestion.Consultar);
            }
        }

        private void ModificarDatos_ControlModificarDatosCancelar()
        {
            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Produccion/ProducconesListar.aspx");
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString());
            else
                this.Response.Redirect(url, true);
        }
    }
}