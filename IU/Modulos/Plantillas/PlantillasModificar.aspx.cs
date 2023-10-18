using System;
using Generales.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Plantillas
{
    public partial class PlantillasModificar : PaginaSegura
    {
       
            protected override void PageLoadEvent(object sender, EventArgs e)
            {
                base.PageLoadEvent(sender, e);
                this.ModificarDatos.PlanesDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
                if (!this.IsPostBack)
                {
                    TGEPlantillas Plantilla = new TGEPlantillas();
                    //Control y Validacion de Parametros
                    if (!this.MisParametrosUrl.Contains("IdPlantilla"))
                        this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Plantillas/PlantillasListar.aspx"), true);

                    int parametro = Convert.ToInt32(this.MisParametrosUrl["IdPlantilla"]);
                Plantilla.IdPlantilla = parametro;


                ModificarDatos.IniciarControl(Plantilla, Gestion.Modificar);
                }
            }

            private void ModificarDatos_ControlModificarDatosCancelar()
            {
           
                string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Plantillas/PlantillasListar.aspx");
               
                    this.Response.Redirect(url, true);
            }
        
    }
}