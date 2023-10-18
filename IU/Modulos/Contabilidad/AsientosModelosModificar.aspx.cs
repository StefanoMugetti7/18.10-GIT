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
    public partial class AsientosModelosModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.AsientoModeloDatosAceptar += new Controles.AsientosModelosDatos.AsientoModeloDatosAceptarEventHandler(ModificarDatos_AsientoModeloDatosAceptar);
            this.ModificarDatos.AsientoModeloDatosCancelar += new Controles.AsientosModelosDatos.AsientoModeloDatosCancelarEventHandler(ModificarDatos_AsientoModeloDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdAsientoModelo"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosModelosListar.aspx"), true);

                CtbAsientosModelos asientoModelo = new CtbAsientosModelos();
                asientoModelo.IdAsientoModelo = Convert.ToInt32(this.MisParametrosUrl["IdAsientoModelo"]);
                this.ModificarDatos.IniciarControl(asientoModelo, Gestion.Modificar);
            }
        }

        protected void ModificarDatos_AsientoModeloDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosModelosListar.aspx"), true);
        }

        protected void ModificarDatos_AsientoModeloDatosAceptar(object sender, global::Contabilidad.Entidades.CtbAsientosModelos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosModelosListar.aspx"), true);
        }
    }
}