using Comunes.Entidades;
using Facturas.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Facturas
{
    public partial class NotasPedidosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.NotasPedidosModificarDatosCancelar += ModificarDatos_NotasPedidosDatosCancelar;
            if (!this.IsPostBack)
            {
                if (this.MisParametrosUrl.Contains("Gestion")
                   && (Gestion)this.MisParametrosUrl["Gestion"] == Gestion.Copiar)
                {
                    VTANotasPedidos presu = new VTANotasPedidos();
                    presu.IdNotaPedido = Convert.ToInt32(this.MisParametrosUrl["IdNotaPedido"]);
                    this.ModificarDatos.IniciarControl(presu, Gestion.Copiar);
                }
                else
                    this.ModificarDatos.IniciarControl(new VTANotasPedidos(), Gestion.Agregar);
            }
        }

        private void ModificarDatos_NotasPedidosDatosCancelar()
        {        
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/NotasPedidosListar.aspx"), true);
        }
    }
}