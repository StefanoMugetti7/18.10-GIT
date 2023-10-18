using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using ProcesosDatos.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.ProcesosDatos
{
    public partial class ProcesosDatosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModifDatos.ProcesosDatosModificarDatosAceptar += new IU.Modulos.ProcesosDatos.Controles.ProcesosDatosModificarDatos.ProcesosDatosAceptarEventHandler(ModifDatos_ProcesosDatosModificarDatosAceptar);
            this.ModifDatos.ProcesosDatosModificarDatosCancelar += new IU.Modulos.ProcesosDatos.Controles.ProcesosDatosModificarDatos.ProcesosDatosCancelarEventHandler(ModifDatos_ProcesosDatosModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdProceso"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/ProcesosDatos/ProcesosDatosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdProceso"]);

                SisProcesos proceso = new SisProcesos();
                proceso.IdProceso = parametro;
                this.ModifDatos.IniciarControl(proceso);
            }
        }

        void ModifDatos_ProcesosDatosModificarDatosCancelar()
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/ProcesosDatos/ProcesosDatosListar.aspx"), true);
        }

        void ModifDatos_ProcesosDatosModificarDatosAceptar(object sender, global::ProcesosDatos.Entidades.SisProcesosProcesamiento e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/ProcesosDatos/ProcesosDatosListar.aspx"), true);
        }
    }
}
