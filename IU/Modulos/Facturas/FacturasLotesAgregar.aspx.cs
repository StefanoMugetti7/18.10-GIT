using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facturas.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Facturas
{
    public partial class FacturasLotesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ModificarDatos.FacturasLotesDatosAceptar += new Controles.FacturasLotesDatos.FacturasLotesDatosAceptarEventHandler(ModificarDatos_FacturasLotesDatosAceptar);
            this.ModificarDatos.FacturasLotesDatosCancelar += new Controles.FacturasLotesDatos.FacturasLotesDatosCancelarEventHandler(ModificarDatos_FacturasLotesDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ModificarDatos.IniciarControl(new VTAFacturasLotesEnviados(), Gestion.Agregar);
            }
        }

        void ModificarDatos_FacturasLotesDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturasLotesListar.aspx"), true);
        }

        //void ModificarDatos_FacturasLotesDatosAceptar()
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturasLotesListar.aspx"), true);
        //}

    }
}