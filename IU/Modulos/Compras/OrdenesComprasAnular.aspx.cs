using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Compras.Entidades;

namespace IU.Modulos.Compras
{
    public partial class OrdenesComprasAnular : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.OrdenesComprasModificarDatosAceptar += new Controles.OrdenesComprasDatos.OrdenesComprasDatosAceptarEventHandler(ModificarDatos_ModificarDatosAceptar);
            this.ModificarDatos.OrdenesComprasModificarDatosCancelar += new Controles.OrdenesComprasDatos.OrdenesComprasDatosCancelarEventHandler(ModificarDatos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                CmpOrdenesCompras orden = new CmpOrdenesCompras();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdOrdenCompra"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/OrdenesComprasListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdOrdenCompra"]);
                orden.IdOrdenCompra = parametro;
                
                this.ModificarDatos.IniciarControl( orden , Gestion.Anular);
            }
        }

        void ModificarDatos_ModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/OrdenesComprasListar.aspx"), true);
        }

        void ModificarDatos_ModificarDatosAceptar(object sender, global::Compras.Entidades.CmpOrdenesCompras e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/OrdenesComprasListar.aspx"), true);
        }
    }
}