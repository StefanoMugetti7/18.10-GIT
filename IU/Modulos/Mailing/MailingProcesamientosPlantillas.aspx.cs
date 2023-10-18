using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mailing.Entidades;

namespace IU.Modulos.Mailing
{
    public partial class MailingProcesamientosPlantillas : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.PlanesDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                TGEMailingProcesamientosPlantillas Plantilla = new TGEMailingProcesamientosPlantillas();
                //Control y Validacion de Parametros
                //if (!this.MisParametrosUrl.Contains("IdPlantilla"))
                //    ModificarDatos.IniciarControl(Plantilla, Gestion.Agregar);
                //else
                //{
                //    int parametro = Convert.ToInt32(this.MisParametrosUrl["IdPlantilla"]);
                //    Plantilla.IdPlantilla = parametro;


                //    ModificarDatos.IniciarControl(Plantilla, Gestion.Modificar);
                //}

                if (!this.MisParametrosUrl.Contains("IdPlantillaRef"))
                {
                    int parametro = Convert.ToInt32(this.MisParametrosUrl["IdPlantilla"]);
                    int procesamiento = Convert.ToInt32(this.MisParametrosUrl["IdMailingProcesamiento"]);
                    Plantilla.IdPlantilla = parametro;
                    Plantilla.IdMailingProcesamiento = procesamiento;


                    ModificarDatos.IniciarControl(Plantilla, Gestion.Modificar); 
                }

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

            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Mailing/MailingProcesamientosEnvios.aspx");

            this.Response.Redirect(url, true);
        }
    }
}