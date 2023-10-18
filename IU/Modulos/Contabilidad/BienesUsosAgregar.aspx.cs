using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Contabilidad.Entidades;

namespace IU.Modulos.Contabilidad
{
    public partial class BienesUsosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.AgregarDatos.BienUsoDatosAceptar += new Controles.BienesUsosDatos.BienUsoDatosAceptarEventHandler(AgregarDatos_BienUsoDatosAceptar);
            this.AgregarDatos.BienUsoDatosCancelar += new Controles.BienesUsosDatos.BienUsoDatosCancelarEventHandler(AgregarDatos_BienUsoDatosCancelar);
            if (!this.IsPostBack)
            {
                this.AgregarDatos.IniciarControl(new CtbBienesUsos(), Gestion.Agregar);
            }
        }

        protected void AgregarDatos_BienUsoDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/BienesUsosListar.aspx"), true);
        }

        protected void AgregarDatos_BienUsoDatosAceptar(object sender, global::Contabilidad.Entidades.CtbBienesUsos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/BienesUsosListar.aspx"), true);
        }
    }
}