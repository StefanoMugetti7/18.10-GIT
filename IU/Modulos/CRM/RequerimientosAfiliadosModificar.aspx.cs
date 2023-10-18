using Comunes.Entidades;
using CRM.Entidades;
using System;

namespace IU.Modulos.CRM
{
    public partial class RequerimientosAfiliadosModificar : PaginaAfiliados
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos2.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                CRMRequerimientos req= new CRMRequerimientos();

                if (!this.MisParametrosUrl.Contains("IdRequerimiento"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CRM/RequerimientosAfiliadosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdRequerimiento"]);
                req.IdRequerimiento = parametro;
                req.Afiliado = this.MiAfiliado;

                ModificarDatos2.IniciarControl(req, Gestion.Modificar,req.Afiliado);
            }
        }

        private void ModificarDatos_ControlModificarDatosCancelar()
        {
            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CRM/RequerimientosAfiliadosListar.aspx");
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString());
            else
            this.Response.Redirect(url, true);
        }
    }
}