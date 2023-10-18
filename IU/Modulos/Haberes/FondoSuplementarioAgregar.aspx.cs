using Comunes.Entidades;
using Haberes;
using Haberes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Haberes
{
    public partial class FondoSuplementarioAgregar : PaginaAfiliados
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
                if (!(parametro.SueldoBruto > 0)) //Si tiene sueldo bruto significa que ya tiene guardado el calculo
                    this.ctrModificarDatos.IniciarControl(parametro, Gestion.Agregar);
                else
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Haberes/FondoSuplementarioListar.aspx"), true);
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