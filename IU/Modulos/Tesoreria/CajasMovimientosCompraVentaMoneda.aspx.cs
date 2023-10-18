using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tesorerias.Entidades;

namespace IU.Modulos.Tesoreria
{
    public partial class CajasMovimientosCompraVentaMoneda : PaginaCajas
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ModificarDatos.ModificarDatosAceptar += new Controles.CajasMovimientosDatos.ControlDatosAceptarEventHandler(ModificarDatos_ModificarDatosAceptar);
            if (!this.IsPostBack)
            {
                TESCajasMovimientos movimiento = new TESCajasMovimientos();
                this.ModificarDatos.IniciarControl(movimiento, this.MiCaja, Gestion.Agregar);
            }
        }
    }
}