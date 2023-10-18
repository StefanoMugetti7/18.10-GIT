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
    public partial class BienesUsosConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ConsultarDatos.BienUsoDatosAceptar += new Controles.BienesUsosDatos.BienUsoDatosAceptarEventHandler(ConsultarDatos_BienUsoDatosAceptar);
            this.ConsultarDatos.BienUsoDatosCancelar += new Controles.BienesUsosDatos.BienUsoDatosCancelarEventHandler(ConsultarDatos_BienUsoDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdBienUso"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/BienesUsosListar.aspx"), true);

                CtbBienesUsos bienUso = new CtbBienesUsos();
                bienUso.IdBienUso = Convert.ToInt32(this.MisParametrosUrl["IdBienUso"]);
                this.ConsultarDatos.IniciarControl(bienUso, Gestion.Consultar);
            }
        }

        protected void ConsultarDatos_BienUsoDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/BienesUsosListar.aspx"), true);
        }

        protected void ConsultarDatos_BienUsoDatosAceptar(object sender, CtbBienesUsos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/BienesUsosListar.aspx"), true);
        }
    }
}