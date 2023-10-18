using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Contabilidad
{
    public partial class EjercicioContableModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.EjercicioContableDatosAceptar += new Controles.EjerciciosContablesDatos.EjercicioContableDatosAceptarEventHandler(ModificarDatos_EjercicioContableDatosAceptar);
            this.ModificarDatos.EjercicioContableDatosCancelar += new Controles.EjerciciosContablesDatos.EjercicioContableDatosCancelarEventHandler(ModificarDatos_EjercicioContableDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdEjercicioContable"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/EjerciciosContablesListar.aspx"), true);

                CtbEjerciciosContables ejerciciodoContable = new CtbEjerciciosContables();
                ejerciciodoContable.IdEjercicioContable = Convert.ToInt32(this.MisParametrosUrl["IdEjercicioContable"]);
                this.ModificarDatos.IniciarControl(ejerciciodoContable, Gestion.Modificar);
            }
        }

        protected void ModificarDatos_EjercicioContableDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/EjerciciosContablesListar.aspx"), true);
        }

        protected void ModificarDatos_EjercicioContableDatosAceptar(object sender, global::Contabilidad.Entidades.CtbEjerciciosContables e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/EjerciciosContablesListar.aspx"), true);
        }
    }
}