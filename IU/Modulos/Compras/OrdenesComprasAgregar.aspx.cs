using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Compras
{
    public partial class OrdenesComprasAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.OrdenesComprasModificarDatosAceptar += new Controles.OrdenesComprasDatos.OrdenesComprasDatosAceptarEventHandler(ModificarDatos_ModificarDatosAceptar);
            this.ModificarDatos.OrdenesComprasModificarDatosCancelar += new Controles.OrdenesComprasDatos.OrdenesComprasDatosCancelarEventHandler(ModificarDatos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ModificarDatos.IniciarControl(new CmpOrdenesCompras(), Gestion.Agregar);
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