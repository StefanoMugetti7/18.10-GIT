using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Medicina.Entidades;

namespace IU.Modulos.Medicina
{
    public partial class PrestacionesModificar : PaginaSegura
    {

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrModificarDatos.PrestacionesModificarDatosAceptar += new Controles.PrestacionesModificarDatos.PrestacionesModificarDatosAceptarEventHandler(ctrModificarDatos_PrestacionesModificarDatosAceptar);
            this.ctrModificarDatos.PrestacionesModificarDatosCancelar += new Controles.PrestacionesModificarDatos.PrestacionesModificarDatosCancelarEventHandler(ctrModificarDatos_PrestacionesModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                MedPrestaciones parametro = new MedPrestaciones();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdPrestacion"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PrestacionesListar.aspx"), true);

                int valor = Convert.ToInt32(this.MisParametrosUrl["IdPrestacion"]);
                parametro.IdPrestacion = valor;
                this.ctrModificarDatos.IniciarControl(parametro, Gestion.Modificar);
            }
        }

        void ctrModificarDatos_PrestacionesModificarDatosAceptar(MedPrestaciones e)
        {
            if (this.ViewState["UrlReferrer"] != null)
            {
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            }
            else
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PrestacionesListar.aspx"), true);
            }
        }

        void ctrModificarDatos_PrestacionesModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
            {
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            }
            else
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PrestacionesListar.aspx"), true);
            }
        }

    }
}