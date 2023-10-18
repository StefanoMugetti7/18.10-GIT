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
    public partial class TiposCargosLotesEnviadosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ModifDatos.ProcesosDatosModificarDatosAceptar += new IU.Modulos.ProcesosDatos.Controles.ProcesosDatosModificarDatos.ProcesosDatosAceptarEventHandler(ModifDatos_ProcesosDatosModificarDatosAceptar);
            this.ModifDatos.ControlModificarDatosCancelar += new IU.Modulos.ProcesosDatos.Controles.TiposCargosLotesEnviadosDatos.ControlDatosCancelarEventHandler(ModifDatos_ProcesosDatosModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ModifDatos.IniciarControl(new CarTiposCargosLotesEnviados(),Gestion.Agregar);
            }
        }

        void ModifDatos_ProcesosDatosModificarDatosCancelar()
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/ProcesosDatos/TiposCargosLotesEnviadosListar.aspx"), true);
        }

        void ModifDatos_ProcesosDatosModificarDatosAceptar(object sender, global::ProcesosDatos.Entidades.SisProcesosProcesamiento e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/ProcesosDatos/TiposCargosLotesEnviadosListar.aspx"), true);
        }
    }
}
