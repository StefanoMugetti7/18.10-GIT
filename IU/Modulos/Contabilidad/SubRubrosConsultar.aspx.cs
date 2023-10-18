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
    public partial class SubRubrosConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ConsultarDatos.SubRubroDatosAceptar += new Controles.SubRubrosDatos.SubRubroDatosAceptarEventHandler(ConsultarDatos_SubRubroDatosAceptar);
            this.ConsultarDatos.SubRubroDatosCancelar += new Controles.SubRubrosDatos.SubRubroDatosCancelarEventHandler(ConsultarDatos_SubRubroDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdSubRubro"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/SubRubrosListar.aspx"), true);

                CtbSubRubros subRubro = new CtbSubRubros();
                subRubro.IdSubRubro = Convert.ToInt32(this.MisParametrosUrl["IdSubRubro"]);
                this.ConsultarDatos.IniciarControl(subRubro, Gestion.Consultar);
            }
        }

        protected void ConsultarDatos_SubRubroDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/SubRubrosListar.aspx"), true);
        }

        protected void ConsultarDatos_SubRubroDatosAceptar(object sender, global::Contabilidad.Entidades.CtbSubRubros e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/SubRubrosListar.aspx"), true);
        }
    }
}