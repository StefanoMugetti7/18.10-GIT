using Comunes.Entidades;
using ProcesosDatos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.ProcesosDatos
{
    public partial class ProcesosDatosConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModifDatos.ControlModificarDatosCancelar += new IU.Modulos.ProcesosDatos.Controles.ProcesosDatosModificarDatosDetalles.ControlDatosCancelarEventHandler(ModifDatos_ProcesosDatosModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                SisProcesosProcesamiento proc = new SisProcesosProcesamiento();

                if (!this.MisParametrosUrl.Contains("IdProcesoProcesamiento"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/ProcesosDatos/ProcesosDatosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdProcesoProcesamiento"]);
                proc.IdProcesoProcesamiento = parametro;

                ModifDatos.IniciarControl(proc,Gestion.Consultar);
            }
        }


        void ModifDatos_ProcesosDatosModificarDatosCancelar()
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/ProcesosDatos/ProcesosDatosListar.aspx"), true);
        }
    }
}
