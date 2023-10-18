using Afiliados.Entidades;
using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Afiliados.Controles
{
    public partial class AfipPadronTXTBuscar : ControlesSeguros
    {
        public delegate void AfiliadosBuscarEventHandler(AfiAfiliados e);
        public event AfiliadosBuscarEventHandler AfiliadosBuscarSeleccionar;
        private List<AfiAfiliados> MisAfiliados
        {
            get { return (List<AfiAfiliados>)Session[this.MiSessionPagina + "AfiliadoPadronMisAfiliados"]; }
            set { Session[this.MiSessionPagina + "AfiliadoPadronMisAfiliados"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }

        public void IniciarControl(List<AfiAfiliados> pParametro)
        {
            this.MisAfiliados = pParametro;
            gvDatos.DataSource = pParametro;
            gvDatos.DataBind();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalBuscarPadronTXT();", true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                ))
                return;
            AfiAfiliados afi = new AfiAfiliados();
            int index = Convert.ToInt32(e.CommandArgument);
            afi.CUIL = Convert.ToInt64(((GridView)sender).DataKeys[index].Value.ToString());
            afi = this.MisAfiliados.Find(x => x.CUIL == afi.CUIL);
            if (e.CommandName == Gestion.Consultar.ToString())
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalBuscarPadronTXT();", true);
                if (this.AfiliadosBuscarSeleccionar != null)
                {
                    this.AfiliadosBuscarSeleccionar(afi);
                }
            }
        }

    }
}