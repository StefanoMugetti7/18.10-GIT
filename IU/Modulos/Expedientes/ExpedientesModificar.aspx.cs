using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Expedientes.Entidades;

namespace IU.Modulos.Expedientes
{
    public partial class ExpedientesModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ctrDatos.ExpedienteModificarDatosAceptar += new IU.Modulos.Expedientes.Controles.ExpedientesModificarDatos.ExpedientesDatosAceptarEventHandler(ctrDatos_ExpedienteModificarDatosAceptar);
            this.ctrDatos.ExpedienteModificarDatosCancelar += new IU.Modulos.Expedientes.Controles.ExpedientesModificarDatos.ExpedientesDatosCancelarEventHandler(ctrDatos_ExpedienteModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                ExpExpedientes expediente = new ExpExpedientes();
                //Control y Validacion de Parametros
                if (! (this.MisParametrosUrl.Contains("IdExpediente")
                    || this.MisParametrosUrl.Contains("Gestion"))
                    )
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Expedientes/ExpedientesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdExpediente"]);
                Gestion gestion = (Gestion)Enum.Parse(typeof(Gestion), this.MisParametrosUrl["Gestion"].ToString());
                expediente.IdExpediente = parametro;
                this.ctrDatos.IniciarControl(expediente, gestion);
            }
        }

        void ctrDatos_ExpedienteModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Expedientes/ExpedientesListar.aspx"), true);
        }

        //void ctrDatos_ExpedienteModificarDatosAceptar(object sender, ExpExpedientes e)
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Expedientes/ExpedientesListar.aspx"), true);
        //}
    }
}
