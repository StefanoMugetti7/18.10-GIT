using Comunes.Entidades;
using CRM.Entidades;
using System;


namespace IU.Modulos.CRM
{
    public partial class RequerimientosAfiliadosAgregar : PaginaAfiliados
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos2.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                CRMRequerimientos cmr= new CRMRequerimientos();

                if (this.MisParametrosUrl.Contains("IdRequerimiento"))
                {
                    int parametro = Convert.ToInt32(this.MisParametrosUrl["IdRequerimiento"]);
                    cmr.IdRequerimiento = parametro;
                }
                cmr.Afiliado = this.MiAfiliado;

                this.ModificarDatos2.IniciarControl(cmr, Gestion.Agregar,cmr.Afiliado);
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