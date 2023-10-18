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
    public partial class ConceptosContablesConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ConsultarDatos.ConceptosContablesDatosAceptar += new Controles.ConceptosContablesDatos.ConceptosContablesDatosAceptarEventHandler(ConsultarDatos_ConceptosContablesDatosAceptar);
            this.ConsultarDatos.ConceptosContablesDatosCancelar += new Controles.ConceptosContablesDatos.ConceptosContablesDatosCancelarEventHandler(ConsultarDatos_ConceptosContablesDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdConceptoContable"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/ConceptosContablesListar.aspx"), true);

                CtbConceptosContables conceptoContable = new CtbConceptosContables();
                conceptoContable.IdConceptoContable = Convert.ToInt32(this.MisParametrosUrl["IdConceptoContable"]);
                this.ConsultarDatos.IniciarControl(conceptoContable, Gestion.Consultar);
            }
        }

        protected void ConsultarDatos_ConceptosContablesDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/ConceptosContablesListar.aspx"), true);
        }

        protected void ConsultarDatos_ConceptosContablesDatosAceptar(object sender, global::Contabilidad.Entidades.CtbConceptosContables e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/ConceptosContablesListar.aspx"), true);
        }
    }
}