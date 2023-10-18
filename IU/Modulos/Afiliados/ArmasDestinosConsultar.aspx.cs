using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Afiliados.Entidades;

namespace IU.Modulos.Afiliados
{
    public partial class ArmasDestinosConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ArmasDestinosModificarDatosAceptar += new Controles.ArmasDestinosModificarDatos.ArmasDestinosDatosAceptarEventHandler(ModificarDatos_ArmasDestinosModificarDatosAceptar);
            this.ModificarDatos.ArmasDestinosModificarDatosCancelar += new Controles.ArmasDestinosModificarDatos.ArmasDestinosDatosCancelarEventHandler(ModificarDatos_ArmasDestinosModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                AfiArmasDestinos ArmaDest = new AfiArmasDestinos();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdArmaDestino"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ArmasDestinosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdArmaDestino"]);
                ArmaDest.IdArmaDestino = parametro;

                this.ModificarDatos.IniciarControl(ArmaDest, Gestion.Consultar);
            }
        }

        void ModificarDatos_ArmasDestinosModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ArmasDestinosListar.aspx"), true);
        }

        void ModificarDatos_ArmasDestinosModificarDatosAceptar(object sender, global::Afiliados.Entidades.AfiArmasDestinos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ArmasDestinosListar.aspx"), true);
        }
    }
}