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
    public partial class AfiliadoCompensacionesModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModifDatos.CompensacionesModificarDatosAceptar += new IU.Modulos.Afiliados.Controles.AfiliadoCompensaciones.AfiliadoCompensacionesDatosAceptarEventHandler(ModificarDatos_AfiliadoCompensacionesModificarDatosAceptar);
            this.ModifDatos.CompensacionesModificarDatosCancelar += new IU.Modulos.Afiliados.Controles.AfiliadoCompensaciones.AfiliadoCompensacionesDatosCancelarEventHandler(ModificarDatos_AfiliadoCompensacionesModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                AfiCompensaciones parametro = new AfiCompensaciones();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdCompensacion"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadoCompensacionesListar.aspx"), true);

                int valor = Convert.ToInt32(this.MisParametrosUrl["IdCompensacion"]);
                parametro.IdCompensacion = valor;
                this.ModifDatos.IniciarControl(parametro, Gestion.Modificar);
            }
        }

        protected void ModificarDatos_AfiliadoCompensacionesModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadoCompensacionesListar.aspx"), true);
        }

        protected void ModificarDatos_AfiliadoCompensacionesModificarDatosAceptar(object sender, global::Afiliados.Entidades.AfiCompensaciones e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadoCompensacionesListar.aspx"), true);
        }
    }
}