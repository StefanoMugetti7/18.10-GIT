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
    public partial class HabitacionesConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                HTLHabitaciones habitacion = new HTLHabitaciones();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdHabitacion"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/HabitacionesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdHabitacion"]);
                habitacion.IdHabitacion = parametro;

                ModificarDatos.IniciarControl(habitacion, Gestion.Consultar);
            }
        }

        private void ModificarDatos_ControlModificarDatosCancelar()
        {
            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/HabitacionesListar.aspx");
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString());
            else
                this.Response.Redirect(url, true);
        }
    }
}