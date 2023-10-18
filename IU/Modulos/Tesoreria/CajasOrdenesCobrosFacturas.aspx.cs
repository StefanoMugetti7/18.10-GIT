using Cobros.Entidades;
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
    public partial class CajasOrdenesCobrosFacturas : PaginaCajas
    {
        protected override void PageLoadEventCajas(object sender, EventArgs e)
        {
            base.PageLoadEventCajas(sender, e);
            ctrlDatos.OrdenesDeCobroDatosAceptar += CtrlDatos_OrdenesDeCobroDatosAceptar;
            ctrlDatos.OrdenesDeCobroDatosCancelar += CtrlDatos_OrdenesDeCobroDatosCancelar;
            if (!this.IsPostBack)
            {
                CobOrdenesCobros ordenCobro = new CobOrdenesCobros();
                ordenCobro.ModuloTesoreriaCajas = true;
                this.ctrlDatos.IniciarControl(ordenCobro, Gestion.Agregar);
            }
        }

        private void CtrlDatos_OrdenesDeCobroDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasAfiliadosInicio.aspx"), true);
        }

        private void CtrlDatos_OrdenesDeCobroDatosAceptar(global::Cobros.Entidades.CobOrdenesCobros e)
        {
        
            if (e.Estado.IdEstado == (int)EstadosOrdenesCobro.Cobrado)
                return;

            this.MiCajaMovimientoPendiente = new TESCajasMovimientos();
            this.MiCajaMovimientoPendiente.Afiliado.IdAfiliado = e.Afiliado.IdAfiliado;
            this.MiCajaMovimientoPendiente.Afiliado.Apellido = e.Afiliado.Apellido;
            this.MiCajaMovimientoPendiente.Afiliado.Nombre = e.Afiliado.Nombre;
            this.MiCajaMovimientoPendiente.Afiliado.NumeroDocumento = e.Afiliado.NumeroDocumento;
            this.MiCajaMovimientoPendiente.Importe = e.ImporteTotal;
            this.MiCajaMovimientoPendiente.IdRefTipoOperacion = e.IdOrdenCobro;
            this.MiCajaMovimientoPendiente.TipoOperacion.IdTipoOperacion = e.TipoOperacion.IdTipoOperacion;
            this.MiCajaMovimientoPendiente.TipoOperacion.TipoOperacion = e.TipoOperacion.TipoOperacion;
            this.MiCajaMovimientoPendiente.SelloTiempo = e.SelloTiempo;
            this.MiCajaMovimientoPendiente.MonedaCotizacion = e.MonedaCotizacion;
            this.MiCajaMovimientoPendiente.CajaMoneda.Moneda = e.Moneda;
            this.MiCajaMovimientoPendiente.Fecha = e.FechaConfirmacion.Value;
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosConfirmar.aspx"), true);
        }
    }
}