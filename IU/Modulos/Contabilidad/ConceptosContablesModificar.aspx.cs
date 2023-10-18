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
    public partial class ConceptosContablesModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ConceptosContablesDatosAceptar += new Controles.ConceptosContablesDatos.ConceptosContablesDatosAceptarEventHandler(ModificarDatos_ConceptosContablesDatosAceptar);
            this.ModificarDatos.ConceptosContablesDatosCancelar += new Controles.ConceptosContablesDatos.ConceptosContablesDatosCancelarEventHandler(ModificarDatos_ConceptosContablesDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdConceptoContable"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/ConceptosContablesListar.aspx"), true);

                CtbConceptosContables conceptosContables = new CtbConceptosContables();
                conceptosContables.IdConceptoContable = Convert.ToInt32(this.MisParametrosUrl["IdConceptoContable"]);
                this.ModificarDatos.IniciarControl(conceptosContables, Gestion.Modificar);
            }
        }

        protected void ModificarDatos_ConceptosContablesDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/ConceptosContablesListar.aspx"), true);
        }

        protected void ModificarDatos_ConceptosContablesDatosAceptar(object sender, global::Contabilidad.Entidades.CtbConceptosContables e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/ConceptosContablesListar.aspx"), true);
        }
    }
}