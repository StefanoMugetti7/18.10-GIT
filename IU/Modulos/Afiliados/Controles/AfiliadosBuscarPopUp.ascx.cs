using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Afiliados.Entidades;
using System.Collections.Generic;
using Comunes.Entidades;
using Afiliados;
using Generales.FachadaNegocio;

namespace IU.Modulos.Afiliados.Controles
{
    public partial class AfiliadosBuscarPopUp : ControlesSeguros
    {
        private List<AfiAfiliados> MisAfiliados
        {
            get { return (List<AfiAfiliados>)Session[this.MiSessionPagina + "AfiliadosBuscarPopUpMisAfiliados"]; }
            set { Session[this.MiSessionPagina + "AfiliadosBuscarPopUpMisAfiliados"] = value; }
        }

        public delegate void AfiliadosBuscarEventHandler(AfiAfiliados e);
        public event AfiliadosBuscarEventHandler AfiliadosBuscarSeleccionar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrAfiliados.AfiliadosBuscarSeleccionar += new AfiliadosBuscar.AfiliadosBuscarEventHandler(ctrAfiliados_AfiliadosBuscarSeleccionar);
            this.ctrAfiliados.AfiliadosEventos += new AfiliadosBuscar.AfiliadosEventosEventHandler(ctrAfiliados_AfiliadosEventos);
        }

        void ctrAfiliados_AfiliadosEventos()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalBuscarSocio();", true);
        }

        void ctrAfiliados_AfiliadosBuscarSeleccionar(AfiAfiliados e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalBuscarSocio();", true);
            if (this.AfiliadosBuscarSeleccionar != null)
                this.AfiliadosBuscarSeleccionar(e);
        }

        public void IniciarControl(bool pLimpiarDatos)
        {
            this.ctrAfiliados.IniciarControl(pLimpiarDatos);
        }

        public void IniciarControl(AfiAfiliados pAfiliado, bool pPanelBuscar, EnumAfiliadosTipos pAfiliadoTipo, bool pLimpiarDatos)
        {
            this.ctrAfiliados.IniciarControl(pAfiliado, pPanelBuscar, pAfiliadoTipo, pLimpiarDatos);
        }
    }
}
