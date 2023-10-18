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
    public partial class FormasCobrosCodigosConceptosModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.FormasCobrosModificarDatosAceptar += new Control.FormasCobrosCodigosConceptosTiposCargosCategorias.FormasCobrosAceptarEventHandler(ModificarDatos_FormasCobrosModificarDatosAceptar);
            this.ModificarDatos.FormasCobrosModificarDatosCancelar += new Control.FormasCobrosCodigosConceptosTiposCargosCategorias.FormasCobrosCancelarEventHandler(ModificarDatos_FormasCobrosModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                TGEFormasCobrosCodigosConceptosTiposCargosCategorias formaCobro = new TGEFormasCobrosCodigosConceptosTiposCargosCategorias();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdFormaCobroCodigoConceptoTipoCargoCategoria"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasCobrosCodigosConceptosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdFormaCobroCodigoConceptoTipoCargoCategoria"]);
                formaCobro.IdFormaCobroCodigoConceptoTipoCargoCategoria = parametro;
                this.ModificarDatos.IniciarControl(formaCobro, Gestion.Modificar);
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