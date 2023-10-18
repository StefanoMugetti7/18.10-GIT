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
    public partial class StockMovimientosAnular : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.StockMovimientosModificarDatosAceptar += new Controles.StockMovimientosDatos.StockMovimientosAceptarEventHandler(ModificarDatos_SolicitudModificarDatosAceptar);
            this.ModificarDatos.StockMovimientosModificarDatosCancelar += new Controles.StockMovimientosDatos.StockMovimientosCancelarEventHandler(ModificarDatos_SolicitudModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                CmpStockMovimientos stockMov = new CmpStockMovimientos();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdStockMovimiento"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/StockMovimientosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdStockMovimiento"]);
                stockMov.IdStockMovimiento = parametro;
                this.ModificarDatos.IniciarControl(stockMov, Gestion.Anular);
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