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
    public partial class CentrosCostosProrrateosConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.AgregarDatos.ControlDatosCancelar += new Controles.CentrosCostosProrrateosDatos.ControlDatosCancelarEventHandler(AgregarDatos_ControlDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdCentroCostoProrrateo"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CentrosCostosProrrateosListar.aspx"), true);

                CtbCentrosCostosProrrateos asientoContable = new CtbCentrosCostosProrrateos();
                asientoContable.IdCentroCostoProrrateo = Convert.ToInt32(this.MisParametrosUrl["IdCentroCostoProrrateo"]);
                this.AgregarDatos.IniciarControl(asientoContable, Gestion.Consultar);
            }
        }

        void AgregarDatos_ControlDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/CentrosCostosProrrateosListar.aspx"), true);
        }
    }
}