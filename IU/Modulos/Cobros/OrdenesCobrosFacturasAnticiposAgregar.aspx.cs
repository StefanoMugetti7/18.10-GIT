using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cobros.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Cobros
{
    public partial class OrdenesCobrosFacturasAnticiposAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.AgregarDatos.OrdenesDeCobroDatosAceptar += new Controles.OrdenesCobrosFacturasAnticiposDatos.OrdenesDeCobroDatosAceptarEventHandler(AgregarDatos_OrdenesDeCobroDatosAceptar);
            this.AgregarDatos.OrdenesDeCobroDatosCancelar += new Controles.OrdenesCobrosFacturasAnticiposDatos.OrdenesDeCobroDatosCancelarEventHandler(AgregarDatos_OrdenesDeCobroDatosCancelar);
            if (!this.IsPostBack)
            {
                CobOrdenesCobros parametro = new CobOrdenesCobros();
                //Control y Validacion de Parametros
                if (this.MisParametrosUrl.Contains("IdAfiliado"))
                    parametro.Afiliado.IdAfiliado = Convert.ToInt32(this.MisParametrosUrl["IdAfiliado"]);

                this.AgregarDatos.IniciarControl(parametro, Gestion.Agregar);
            }
        }

        protected void AgregarDatos_OrdenesDeCobroDatosCancelar()
        {
            this.Response.Redirect("~/Modulos/Cobros/OrdenesCobrosFacturasListar.aspx", true);
        }

        protected void AgregarDatos_OrdenesDeCobroDatosAceptar(global::Cobros.Entidades.CobOrdenesCobros e)
        {
            this.Response.Redirect("~/Modulos/Cobros/OrdenesCobrosFacturasListar.aspx", true);
        }
    }
}