using Comunes.Entidades;
using CRM.Entidades;
using LavaYa.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.CRM
{
    public partial class RequerimientosConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos2.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                CRMRequerimientos requerimiento = new CRMRequerimientos();

                if (!this.MisParametrosUrl.Contains("IdRequerimiento"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Requerimientos/RequerimientosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdRequerimiento"]);
                requerimiento.IdRequerimiento = parametro;

                ModificarDatos2.IniciarControl(requerimiento, Gestion.Consultar);
            }
        }

        private void ModificarDatos_ControlModificarDatosCancelar()
        {
            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CRM/RequerimientosListar.aspx");
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString());
            else
                this.Response.Redirect(url, true);
        }
    }
}
