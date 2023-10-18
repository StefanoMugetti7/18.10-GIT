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
    public partial class SubRubrosModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.SubRubroDatosAceptar += new Controles.SubRubrosDatos.SubRubroDatosAceptarEventHandler(ModificarDatos_SubRubroDatosAceptar);
            this.ModificarDatos.SubRubroDatosCancelar += new Controles.SubRubrosDatos.SubRubroDatosCancelarEventHandler(ModificarDatos_SubRubroDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdSubRubro"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/SubRubrosListar.aspx"), true);

                CtbSubRubros subRubro = new CtbSubRubros();
                subRubro.IdSubRubro = Convert.ToInt32(this.MisParametrosUrl["IdSubRubro"]);
                this.ModificarDatos.IniciarControl(subRubro, Gestion.Modificar);
            }
        }

        protected void ModificarDatos_SubRubroDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/SubRubrosListar.aspx"), true);
        }

        protected void ModificarDatos_SubRubroDatosAceptar(object sender, global::Contabilidad.Entidades.CtbSubRubros e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/SubRubrosListar.aspx"), true);
        }
    }
}