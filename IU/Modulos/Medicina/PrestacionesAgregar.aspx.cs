using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Medicina.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Medicina
{
    public partial class PrestacionesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrModificarDatos.PrestacionesModificarDatosAceptar += new Controles.PrestacionesModificarDatos.PrestacionesModificarDatosAceptarEventHandler(ctrModificarDatos_PrestacionesModificarDatosAceptar);
            this.ctrModificarDatos.PrestacionesModificarDatosCancelar += new Controles.PrestacionesModificarDatos.PrestacionesModificarDatosCancelarEventHandler(ctrModificarDatos_PrestacionesModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                MedPrestaciones atencion = new MedPrestaciones();
                if (this.MisParametrosUrl.Contains("Turno"))
                {
                    MedTurnos turno = (MedTurnos)MisParametrosUrl["Turno"];
                    AyudaProgramacion.MatchObjectProperties(turno, atencion.Turno);
                    AyudaProgramacion.MatchObjectProperties(turno.Prestador, atencion.Prestador);
                    AyudaProgramacion.MatchObjectProperties(turno.Especializacion, atencion.Especializacion);
                    AyudaProgramacion.MatchObjectProperties(turno.ObraSocial, atencion.ObraSocial);
                    this.MisParametrosUrl.Remove("Turno");
                }
                this.ctrModificarDatos.IniciarControl(atencion, Gestion.Agregar);

            } 
        }

        void ctrModificarDatos_PrestacionesModificarDatosAceptar(MedPrestaciones e)
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PrestacionesListar.aspx"), true);
        }

        void ctrModificarDatos_PrestacionesModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PrestacionesListar.aspx"), true);
        }
    }
}