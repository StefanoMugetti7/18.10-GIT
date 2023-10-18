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
    public partial class EjerciciosContablesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.AgregarDatos.EjercicioContableDatosAceptar += new Controles.EjerciciosContablesDatos.EjercicioContableDatosAceptarEventHandler(AgregarDatos_EjercicioContableDatosAceptar);
            this.AgregarDatos.EjercicioContableDatosCancelar += new Controles.EjerciciosContablesDatos.EjercicioContableDatosCancelarEventHandler(AgregarDatos_EjercicioContableDatosCancelar);
            if (!this.IsPostBack)
            {
                this.AgregarDatos.IniciarControl(new CtbEjerciciosContables(), Gestion.Agregar);
            }
        }

        protected void AgregarDatos_EjercicioContableDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/EjerciciosContablesListar.aspx"), true);
        }

        protected void AgregarDatos_EjercicioContableDatosAceptar(object sender, global::Contabilidad.Entidades.CtbEjerciciosContables e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/EjerciciosContablesListar.aspx"), true);
        }
    }
}