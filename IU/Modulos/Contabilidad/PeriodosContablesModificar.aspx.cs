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
    public partial class PeriodosContablesModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.PeriodoContableDatosAceptar += new Controles.PeriodosContablesDatos.PeriodoContableDatosAceptarEventHandler(ModificarDatos_PeriodoContableDatosAceptar);
            this.ModificarDatos.PeriodoContableDatosCancelar += new Controles.PeriodosContablesDatos.PeriodoContableDatosCancelarEventHandler(ModificarDatos_PeriodoContableDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdPeriodoContable"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/PeriodosContablesListar.aspx"), true);

                CtbPeriodosContables periodoContable = new CtbPeriodosContables();
                periodoContable.IdPeriodoContable = Convert.ToInt32(this.MisParametrosUrl["IdPeriodoContable"]);
                this.ModificarDatos.IniciarControl(periodoContable, Gestion.Modificar);
            }
        }

        protected void ModificarDatos_PeriodoContableDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/PeriodosContablesListar.aspx"), true);
        }

        protected void ModificarDatos_PeriodoContableDatosAceptar(object sender, global::Contabilidad.Entidades.CtbPeriodosContables e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/PeriodosContablesListar.aspx"), true);
        }
    }
}