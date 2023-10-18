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
    public partial class PeriodosIvasAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.PeriodoIVADatosAceptar += new Controles.PeriodosIvasDatos.PeriodoIVADatosAceptarEventHandler(ModificarDatos_PeriodoIvaDatosAceptar);
            this.ModificarDatos.PeriodoIVADatosCancelar += new Controles.PeriodosIvasDatos.PeriodoIVADatosCancelarEventHandler(ModificarDatos_PeriodoIvaDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ModificarDatos.IniciarControl(new CtbPeriodosIvas(), Gestion.Agregar);
            }
        }

        protected void ModificarDatos_PeriodoIvaDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/PeriodosIvasListar.aspx"), true);
        }

        protected void ModificarDatos_PeriodoIvaDatosAceptar(object sender, global::Contabilidad.Entidades.CtbPeriodosIvas e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/PeriodosIvasListar.aspx"), true);
        }
    }
}