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
    public partial class RubrosConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ConsultarDatos.RubroDatosAceptar += new Controles.RubrosDatos.RubroDatosAceptarEventHandler(ConsultarDatos_RubroDatosAceptar);
            this.ConsultarDatos.RubroDatosCancelar += new Controles.RubrosDatos.RubroDatosCancelarEventHandler(ConsultarDatos_RubroDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdRubro"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/RubrosListar.aspx"), true);

                CtbRubros rubro = new CtbRubros();
                rubro.IdRubro = Convert.ToInt32(this.MisParametrosUrl["IdRubro"]);
                this.ConsultarDatos.IniciarControl(rubro, Gestion.Consultar);
            }
        }

        protected void ConsultarDatos_RubroDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/RubrosListar.aspx"), true);
        }

        protected void ConsultarDatos_RubroDatosAceptar(object sender, global::Contabilidad.Entidades.CtbRubros e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/RubrosListar.aspx"), true);
        }
    }
}