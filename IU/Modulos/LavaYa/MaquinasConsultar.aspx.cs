using Comunes.Entidades;
using LavaYa.Entidades;
using System;

namespace IU.Modulos.LavaYa
{
    public partial class MaquinasConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos2.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                LavMaquinas maquina = new LavMaquinas();

                if (!this.MisParametrosUrl.Contains("IdMaquina"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/LavaYa/MaquinasListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdMaquina"]);
                maquina.IdMaquina = parametro;

                ModificarDatos2.IniciarControl(maquina, Gestion.Consultar);
            }
        }

        private void ModificarDatos_ControlModificarDatosCancelar()
        {
            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/LavaYa/MaquinasListar.aspx");
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString());
            else
                this.Response.Redirect(url, true);
        }
    }
}
