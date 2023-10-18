using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Haberes;
using Haberes.Entidades;

namespace IU.Modulos.Haberes
{
    public partial class FondoSuplementarioConsultar : PaginaAfiliados
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrModificarDatos.FondoSuplementarioDatosAceptar += new Controles.FondoSuplementarioDatos.FondoSuplementarioDatosAceptarEventHandler(ctrModificarDatos_FondoSuplementarioDatosAceptar);
            this.ctrModificarDatos.FondoSuplementarioDatosCancelar += new Controles.FondoSuplementarioDatos.FondoSuplementarioDatosCancelarEventHandler(ctrModificarDatos_FondoSuplementarioDatosCancelar);
            if (!this.IsPostBack)
            {
                HabFondoSuplementario parametro = new HabFondoSuplementario();
                parametro.Afiliado = MiAfiliado;
                parametro = HaberesF.FondoSuplementarioObtenerDatosCompletos(parametro);
                parametro.Afiliado = MiAfiliado;
                this.ctrModificarDatos.IniciarControl(parametro, Gestion.Consultar);

            }
        }

        void ctrModificarDatos_FondoSuplementarioDatosAceptar(HabFondoSuplementario e)
        {
            if (this.ViewState["UrlReferrer"] != null)
            {
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            }
            else
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Haberes/FondoSuplementarioListar.aspx"), true);
            }
        }

        void ctrModificarDatos_FondoSuplementarioDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
            {
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            }
            else
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Haberes/FondoSuplementarioListar.aspx"), true);
            }
        }
    }
}