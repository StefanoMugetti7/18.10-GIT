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
    public partial class PeriodosContablesConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ConsultarDatos.PeriodoContableDatosAceptar += new Controles.PeriodosContablesDatos.PeriodoContableDatosAceptarEventHandler(ConsultarDatos_PeriodoContableDatosAceptar);
            this.ConsultarDatos.PeriodoContableDatosCancelar += new Controles.PeriodosContablesDatos.PeriodoContableDatosCancelarEventHandler(ConsultarDatos_PeriodoContableDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdPeriodoContable"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/PeriodosContablesListar.aspx"), true);

                CtbPeriodosContables periodoContable = new CtbPeriodosContables();
                periodoContable.IdPeriodoContable = Convert.ToInt32(this.MisParametrosUrl["IdPeriodoContable"]);
                this.ConsultarDatos.IniciarControl(periodoContable, Gestion.Consultar);
            }
        }

        protected void ConsultarDatos_PeriodoContableDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/PeriodosContablesListar.aspx"), true);
        }

        protected void ConsultarDatos_PeriodoContableDatosAceptar(object sender, global::Contabilidad.Entidades.CtbPeriodosContables e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/PeriodosContablesListar.aspx"), true);
        }
    }
}