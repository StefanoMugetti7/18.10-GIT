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
    public partial class TiposCargosLotesEnviadosConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModifDatos.ControlModificarDatosCancelar += new IU.Modulos.ProcesosDatos.Controles.TiposCargosLotesEnviadosDatos.ControlDatosCancelarEventHandler(ModifDatos_ProcesosDatosModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                CarTiposCargosLotesEnviados lote = new CarTiposCargosLotesEnviados();

                if (!this.MisParametrosUrl.Contains("IdTipoCargoLoteEnviado"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/ProcesosDatos/TiposCargosLotesEnviadosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdTipoCargoLoteEnviado"]);
                lote.IdTipoCargoLoteEnviado = parametro;

                ModifDatos.IniciarControl(lote, Gestion.Consultar);
            }
        }


        void ModifDatos_ProcesosDatosModificarDatosCancelar()
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/ProcesosDatos/TiposCargosLotesEnviadosListar.aspx"), true);
        }
    }
}
