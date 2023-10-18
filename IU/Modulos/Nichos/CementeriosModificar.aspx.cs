using Comunes.Entidades;
using Hoteles.Entidades;
using Nichos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Nichos
{
    public partial class CementeriosModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                NCHCementerios cementerio = new NCHCementerios();

                if (!this.MisParametrosUrl.Contains("IdCementerio"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/CementeriosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdCementerio"]);
                cementerio.IdCementerio = parametro;

                ModificarDatos.IniciarControl(cementerio, Gestion.Modificar);
            }
        }

        private void ModificarDatos_ControlModificarDatosCancelar()
        {
            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/CementeriosListar.aspx");
            //if (this.ViewState["UrlReferrer"] != null)
            //    this.Response.Redirect(this.ViewState["UrlReferrer"].ToString());
            //else
            this.Response.Redirect(url, true);
        }
    }
}