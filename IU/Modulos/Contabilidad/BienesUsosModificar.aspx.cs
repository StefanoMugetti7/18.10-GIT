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
    public partial class BienesUsosModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.BienUsoDatosAceptar += new Controles.BienesUsosDatos.BienUsoDatosAceptarEventHandler(ModificarDatos_BienUsoDatosAceptar);
            this.ModificarDatos.BienUsoDatosCancelar += new Controles.BienesUsosDatos.BienUsoDatosCancelarEventHandler(ModificarDatos_BienUsoDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdBienUso"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/BienesUsosListar.aspx"), true);

                CtbBienesUsos bienUso = new CtbBienesUsos();
                bienUso.IdBienUso = Convert.ToInt32(this.MisParametrosUrl["IdBienUso"]);
                this.ModificarDatos.IniciarControl(bienUso, Gestion.Modificar);
            }
        }

        protected void ModificarDatos_BienUsoDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/BienesUsosListar.aspx"), true);
        }

        protected void ModificarDatos_BienUsoDatosAceptar(object sender, global::Contabilidad.Entidades.CtbBienesUsos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/BienesUsosListar.aspx"), true);
        }
    }
}