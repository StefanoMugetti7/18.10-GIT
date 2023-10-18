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
    public partial class RubrosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.AgregarDatos.RubroDatosAceptar += new Controles.RubrosDatos.RubroDatosAceptarEventHandler(AgregarDatos_RubroDatosAceptar);
            this.AgregarDatos.RubroDatosCancelar += new Controles.RubrosDatos.RubroDatosCancelarEventHandler(AgregarDatos_RubroDatosCancelar);
            if (!this.IsPostBack)
            {
                this.AgregarDatos.IniciarControl(new CtbRubros(), Gestion.Agregar);
            }
        }

        protected void AgregarDatos_RubroDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/RubrosListar.aspx"), true);
        }

        protected void AgregarDatos_RubroDatosAceptar(object sender, global::Contabilidad.Entidades.CtbRubros e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/RubrosListar.aspx"), true);
        }
    }
}