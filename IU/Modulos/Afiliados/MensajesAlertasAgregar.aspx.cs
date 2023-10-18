using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afiliados.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Afiliados
{
    public partial class MensajesAlertasAgregar : PaginaAfiliados
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.MensajesAlertasDatosAceptar += new IU.Modulos.Afiliados.Controles.MensajesAlertasDatos.MensajesAlertasDatosAceptarEventHandler(ModifDatos_AfiliadosModificarDatosAceptar);
            this.ModificarDatos.MensajesAlertasDatosCancelar += new IU.Modulos.Afiliados.Controles.MensajesAlertasDatos.MensajesAlertasDatosCancelarEventHandler(ModifDatos_AfiliadosModificarDatosCancelar);

            if (!this.IsPostBack)
            {
                PaginaAfiliados paginaAfi = new PaginaAfiliados();
                AfiAfiliados afiliado = paginaAfi.Obtener(this.MiSessionPagina);
                AfiMensajesAlertas mensaje = new AfiMensajesAlertas();
                mensaje.IdAfiliado = afiliado.IdAfiliado;
                this.ModificarDatos.IniciarControl(mensaje, Gestion.Agregar);
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