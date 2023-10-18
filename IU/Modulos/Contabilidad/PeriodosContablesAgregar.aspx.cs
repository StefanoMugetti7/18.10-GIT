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
    public partial class PeriodosContablesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.AgregarDatos.PeriodoContableDatosAceptar += new Controles.PeriodosContablesDatos.PeriodoContableDatosAceptarEventHandler(AgregarDatos_PeriodoContableDatosAceptar);
            this.AgregarDatos.PeriodoContableDatosCancelar += new Controles.PeriodosContablesDatos.PeriodoContableDatosCancelarEventHandler(AgregarDatos_PeriodoContableDatosCancelar);
            if (!this.IsPostBack)
            {
                this.AgregarDatos.IniciarControl(new CtbPeriodosContables(), Gestion.Agregar);
            }
        }

        protected void AgregarDatos_PeriodoContableDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/PeriodosContablesListar.aspx"), true);
        }

        protected void AgregarDatos_PeriodoContableDatosAceptar(object sender, global::Contabilidad.Entidades.CtbPeriodosContables e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/PeriodosContablesListar.aspx"), true);
        }
    }
}