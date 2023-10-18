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
    public partial class ConceptosContablesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.AgregarDatos.ConceptosContablesDatosAceptar += new Controles.ConceptosContablesDatos.ConceptosContablesDatosAceptarEventHandler(AgregarDatos_ConceptosContablesDatosAceptar);
            this.AgregarDatos.ConceptosContablesDatosCancelar += new Controles.ConceptosContablesDatos.ConceptosContablesDatosCancelarEventHandler(AgregarDatos_ConceptosContablesDatosCancelar);
            if (!this.IsPostBack)
            {
                this.AgregarDatos.IniciarControl(new CtbConceptosContables(), Gestion.Agregar);
            }
        }

        protected void AgregarDatos_ConceptosContablesDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/ConceptosContablesListar.aspx"), true);
        }

        protected void AgregarDatos_ConceptosContablesDatosAceptar(object sender, global::Contabilidad.Entidades.CtbConceptosContables e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/ConceptosContablesListar.aspx"), true);
        }
    }
}