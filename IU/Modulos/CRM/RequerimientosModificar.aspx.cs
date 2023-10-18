using Comunes.Entidades;
using CRM;
using CRM.Entidades;
using System;
using static CRM.Entidades.CRMRequerimientos;

namespace IU.Modulos.CRM
{
    public partial class RequerimientosModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos2.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                CRMRequerimientos req= new CRMRequerimientos();

                if (!this.MisParametrosUrl.Contains("IdRequerimiento"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CRM/RequerimientosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdRequerimiento"]);
                req.IdRequerimiento = parametro;
                req = RequerimientosF.RequerimientosObtenerDatosCompletos(req);
                if(req.Estado.IdEstado == (int)EstadosRequerimientos.Solucionado)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CRM/RequerimientosListar.aspx"),true);
                }
                else
                {
                    ModificarDatos2.IniciarControl(req, Gestion.Modificar);
                }
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