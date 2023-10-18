using Ahorros.Entidades;
using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Ahorros
{
    public partial class PlazosFijosRenovacionAnticipada : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            this.CancelarDatos.PlazosFijosDatosAceptar += new IU.Modulos.Ahorros.Controles.PlazosFijosDatos.PlazosFijosDatosAceptarEventHandler(CancelarDatos_PlazosFijosDatosAceptar);
            this.CancelarDatos.PlazosFijosDatosCancelar += new IU.Modulos.Ahorros.Controles.PlazosFijosDatos.PlazosFijosDatosCancelarEventHandler(CancelarDatos_PlazosFijosDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdPlazoFijo"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosFijosListar.aspx"), true);
                if (!this.MisParametrosUrl.Contains("IdPlazoFijoAnterior"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosFijosListar.aspx"), true);
                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdPlazoFijo"]);
                int parametroPlazoFijoAnterior = Convert.ToInt32(this.MisParametrosUrl["IdPlazoFijoAnterior"]);
                AhoPlazosFijos plazosFijos = new AhoPlazosFijos();
                plazosFijos.IdPlazoFijo = parametro;
                plazosFijos.IdPlazoFijoAnterior = parametroPlazoFijoAnterior;
                plazosFijos.IdAfiliado = this.MiAfiliado.IdAfiliado;
                this.CancelarDatos.IniciarControl(plazosFijos, Gestion.RenovacionAnticipada);
            }
        }

        protected void CancelarDatos_PlazosFijosDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosFijosListar.aspx"), true);
        }

        protected void CancelarDatos_PlazosFijosDatosAceptar(object sender, global::Ahorros.Entidades.AhoPlazosFijos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/PlazosFijosListar.aspx"), true);
        }
    }
}