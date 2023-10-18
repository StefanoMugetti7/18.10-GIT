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
    public partial class CentrosCostosProrrateosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.AgregarDatos.ControlDatosCancelar += new Controles.CentrosCostosProrrateosDatos.ControlDatosCancelarEventHandler(AgregarDatos_ControlDatosCancelar);
            if (!this.IsPostBack)
            {
                this.AgregarDatos.IniciarControl(new CtbCentrosCostosProrrateos(), Gestion.Agregar);
            }
        }

        void AgregarDatos_ControlDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CentrosCostosProrrateosListar.aspx"), true);
        }
    }
}