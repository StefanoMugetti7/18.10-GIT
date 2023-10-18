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
    public partial class RubrosModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.RubroDatosAceptar += new Controles.RubrosDatos.RubroDatosAceptarEventHandler(ModificarDatos_RubroDatosAceptar);
            this.ModificarDatos.RubroDatosCancelar += new Controles.RubrosDatos.RubroDatosCancelarEventHandler(ModificarDatos_RubroDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdRubro"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/RubrosListar.aspx"), true);

                CtbRubros rubro = new CtbRubros();
                rubro.IdRubro = Convert.ToInt32(this.MisParametrosUrl["IdRubro"]);
                this.ModificarDatos.IniciarControl(rubro, Gestion.Modificar);
            }
        }

        protected void ModificarDatos_RubroDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/RubrosListar.aspx"), true);
        }

        protected void ModificarDatos_RubroDatosAceptar(object sender, global::Contabilidad.Entidades.CtbRubros e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/RubrosListar.aspx"), true);
        }
    }
}