using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Comunes.Entidades;
using Contabilidad;

namespace IU.Modulos.Contabilidad
{
    public partial class AsientosModelosConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ConsultarDatos.AsientoModeloDatosAceptar += new Controles.AsientosModelosDatos.AsientoModeloDatosAceptarEventHandler(ConsultarDatos_AsientoModeloDatosAceptar);
            this.ConsultarDatos.AsientoModeloDatosCancelar += new Controles.AsientosModelosDatos.AsientoModeloDatosCancelarEventHandler(ConsultarDatos_AsientoModeloDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdAsientoModelo"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosModelosListar.aspx"), true);

                CtbAsientosModelos asientoModelo = new CtbAsientosModelos();
                asientoModelo.IdAsientoModelo = Convert.ToInt32(this.MisParametrosUrl["IdAsientoModelo"]);
                this.ConsultarDatos.IniciarControl(asientoModelo, Gestion.Consultar);                
            }
        }

        protected void ConsultarDatos_AsientoModeloDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosModelosListar.aspx"), true);
        }

        protected void ConsultarDatos_AsientoModeloDatosAceptar(object sender, CtbAsientosModelos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosModelosListar.aspx"), true);
        }
    }
}