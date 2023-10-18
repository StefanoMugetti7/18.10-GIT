using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Expedientes.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Expedientes
{
    public partial class ExpedientesConsultar : PaginaSegura
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
                if (!this.MisParametrosUrl.Contains("IdExpediente"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Expedientes/ExpedientesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdExpediente"]);
                expediente.IdExpediente = parametro;
                this.ctrDatos.IniciarControl(expediente, Gestion.Consultar);
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
