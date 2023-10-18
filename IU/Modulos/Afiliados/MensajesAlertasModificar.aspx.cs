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
    public partial class MensajesAlertasModificar : PaginaAfiliados
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.MensajesAlertasDatosAceptar += new IU.Modulos.Afiliados.Controles.MensajesAlertasDatos.MensajesAlertasDatosAceptarEventHandler(ModifDatos_AfiliadosModificarDatosAceptar);
            this.ModificarDatos.MensajesAlertasDatosCancelar += new IU.Modulos.Afiliados.Controles.MensajesAlertasDatos.MensajesAlertasDatosCancelarEventHandler(ModifDatos_AfiliadosModificarDatosCancelar);

            if (!this.IsPostBack)
            {

                AfiMensajesAlertas mensaje = new AfiMensajesAlertas();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdMensajeAlerta"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/MensajesAlertasListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdMensajeAlerta"]);
                mensaje.IdMensajeAlerta = parametro;

                this.ModificarDatos.IniciarControl(mensaje, Gestion.Modificar);
            }
        }

        void ModifDatos_AfiliadosModificarDatosCancelar()
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/MensajesAlertasListar.aspx"), true);
        }

        void ModifDatos_AfiliadosModificarDatosAceptar(object sender, global::Afiliados.Entidades.AfiMensajesAlertas e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/MensajesAlertasListar.aspx"), true);
        }
    }
}