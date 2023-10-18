using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Comunes.Entidades;
using Generales.Entidades;

namespace IU.Modulos.Compras
{
    public partial class StockMovimientosTransferir : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.StockMovimientosModificarDatosAceptar += new Controles.StockMovimientosDatos.StockMovimientosAceptarEventHandler(ModificarDatos_SolicitudModificarDatosAceptar);
            this.ModificarDatos.StockMovimientosModificarDatosCancelar += new Controles.StockMovimientosDatos.StockMovimientosCancelarEventHandler(ModificarDatos_SolicitudModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                CmpStockMovimientos stockMov = new CmpStockMovimientos();
                stockMov.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.StockTransferencia;
                this.ModificarDatos.IniciarControl(stockMov, Gestion.Agregar);
            }
        }

        void ModificarDatos_SolicitudModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/StockMovimientosListar.aspx"), true);
        }

        void ModificarDatos_SolicitudModificarDatosAceptar(object sender, CmpStockMovimientos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/StockMovimientosListar.aspx"), true);
        }
    }
}