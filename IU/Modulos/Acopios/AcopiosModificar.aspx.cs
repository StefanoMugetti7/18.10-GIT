using Acopios.Entidades;
using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Acopios
{
    public partial class AcopiosModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ControlModificarDatosCancelar += ModificarDatos_ControlModificarDatosCancelar;
            if (!this.IsPostBack)
            {
                AcpAcopios acopio = new AcpAcopios();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdAcopio") 
                    || !this.MisParametrosUrl.Contains("IdRefTabla"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Proveedores/ProveedoresModificar.aspx"), true);

                acopio.IdAcopio = Convert.ToInt32(this.MisParametrosUrl["IdAcopio"]);
                acopio.IdRefTabla = Convert.ToInt64(this.MisParametrosUrl["IdRefTabla"]);
                acopio.Tabla = this.MisParametrosUrl["Tabla"].ToString();
                acopio.RazonSocial = this.MisParametrosUrl["RazonSocial"].ToString();

                ModificarDatos.IniciarControl(acopio, Gestion.Modificar);
            }
        }

        private void ModificarDatos_ControlModificarDatosCancelar()
        {
            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Proveedores/ProveedoresModificar.aspx");
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString());
            else
                this.Response.Redirect(url, true);
        }
    }
}