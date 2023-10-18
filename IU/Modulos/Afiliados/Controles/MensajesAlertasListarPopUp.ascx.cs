using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afiliados;
using Afiliados.Entidades;

namespace IU.Modulos.Afiliados.Controles
{
    public partial class MensajesAlertasListarPopUp : ControlesSeguros
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            { 
            }
        }

        public void IniciarControl(AfiAfiliados pParametro)
        {
            AfiMensajesAlertas filtro = new AfiMensajesAlertas();
            filtro.IdAfiliado = pParametro.IdAfiliado;
            filtro.Estado.IdEstado = (int)EstadosMensajesAlertas.Activo;
            List<AfiMensajesAlertas> lista = AfiliadosF.MensajesAlertasObtenerListaFiltro(filtro);
            if (lista.Count > 0)
            {
                AyudaProgramacion.CargarGrillaListas<AfiMensajesAlertas>(lista, false, this.gvDatos, false);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModalMensajesAlertas", "ShowModalPopUpMensajesAlertas();", true);
            }
            pParametro.MostrarMensajesAlertas = true;
        }
        protected void btnVolver_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModalMensajesAlertas", "HideModalPopUpMensajesAlertas();", true);
        }
    }
}