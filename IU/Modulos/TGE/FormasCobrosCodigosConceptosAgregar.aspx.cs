using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Generales.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.TGE
{
    public partial class FormasCobrosCodigosConceptosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.FormasCobrosModificarDatosAceptar += new Control.FormasCobrosCodigosConceptosTiposCargosCategorias.FormasCobrosAceptarEventHandler(ModificarDatos_FormasCobrosModificarDatosAceptar);
            this.ModificarDatos.FormasCobrosModificarDatosCancelar += new Control.FormasCobrosCodigosConceptosTiposCargosCategorias.FormasCobrosCancelarEventHandler(ModificarDatos_FormasCobrosModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ModificarDatos.IniciarControl(new TGEFormasCobrosCodigosConceptosTiposCargosCategorias(), Gestion.Agregar);
            }
        }

        void ModificarDatos_FormasCobrosModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasCobrosCodigosConceptosListar.aspx"), true);
        }

        void ModificarDatos_FormasCobrosModificarDatosAceptar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasCobrosCodigosConceptosListar.aspx"), true);
        }
    }
}