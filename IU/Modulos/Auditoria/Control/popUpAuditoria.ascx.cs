using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;

namespace IU.Modulos.Auditoria.Control
{
    public partial class popUpAuditoria : ControlesSeguros
    {

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }

        public void IniciarControl(Objeto pObjeto)
        {
            this.ctrAuditoria.IniciarControl(pObjeto);
            this.mpePopUp.Show();
        }
    }
}
