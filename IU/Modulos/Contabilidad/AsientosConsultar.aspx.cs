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
    public partial class AsientosConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ConsultarDatos.AsientoContableDatosAceptar += new Controles.AsientosDatos.AsientoContableDatosAceptarEventHandler(ConsultarDatos_AsientoContableDatosAceptar);
            this.ConsultarDatos.AsientoContableDatosCancelar += new Controles.AsientosDatos.AsientoContableDatosCancelarEventHandler(ConsultarDatos_AsientoContableDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdAsientoContable"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosListar.aspx"), true);

                CtbAsientosContables asientoContable = new CtbAsientosContables();
                asientoContable.IdAsientoContable = Convert.ToInt32(this.MisParametrosUrl["IdAsientoContable"]);
                this.ConsultarDatos.IniciarControl(asientoContable, Gestion.Consultar);
            }
        }

        protected void ConsultarDatos_AsientoContableDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosListar.aspx"), true);
        }

        protected void ConsultarDatos_AsientoContableDatosAceptar(object sender, global::Contabilidad.Entidades.CtbAsientosContables e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosListar.aspx"), true);
        }
    }
}