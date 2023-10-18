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
    public partial class SubRubrosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.AgregarDatos.SubRubroDatosAceptar += new Controles.SubRubrosDatos.SubRubroDatosAceptarEventHandler(AgregarDatos_SubRubroDatosAceptar);
            this.AgregarDatos.SubRubroDatosCancelar += new Controles.SubRubrosDatos.SubRubroDatosCancelarEventHandler(AgregarDatos_SubRubroDatosCancelar);
            if (!this.IsPostBack)
            {
                this.AgregarDatos.IniciarControl(new CtbSubRubros(), Gestion.Agregar);
            }
        }

        protected void AgregarDatos_SubRubroDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/SubRubrosListar.aspx"), true);
        }

        protected void AgregarDatos_SubRubroDatosAceptar(object sender, global::Contabilidad.Entidades.CtbSubRubros e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/SubRubrosListar.aspx"), true);
        }
    }
}