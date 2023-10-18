using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Plantillas
{
    public partial class PlantillasAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.PlanesDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                TGEPlantillas Plantilla = new TGEPlantillas();
                if (!this.MisParametrosUrl.Contains("IdPlantillaRef"))
                    this.ModificarDatos.IniciarControl(new TGEPlantillas(), Gestion.Agregar);
                else
                {
                    int parametro = Convert.ToInt32(this.MisParametrosUrl["IdPlantillaRef"]);
                    Plantilla.IdPlantillaRef = parametro;

                    this.ModificarDatos.IniciarControl(Plantilla, Gestion.Agregar);
                }

               
            }
        }

        private void ModificarDatos_ControlModificarDatosCancelar()
        {
            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Plantillas/PlantillasListar.aspx");
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString());
            else
                this.Response.Redirect(url, true);
        }
    }
}