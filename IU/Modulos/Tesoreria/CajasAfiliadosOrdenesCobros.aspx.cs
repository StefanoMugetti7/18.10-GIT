using Cobros.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tesorerias.Entidades;

namespace IU.Modulos.Tesoreria
{
    public partial class CajasAfiliadosOrdenesCobros : PaginaCajasAfiliados
    {
        protected override void PageLoadEventCajas(object sender, EventArgs e)
        {
            base.PageLoadEventCajas(sender, e);
            this.ctrlDatos.OrdenesCobrosModificarDatosAceptar +=new IU.Modulos.Cobros.Controles.OrdenesCobrosAfiliadosDatos.OrdenesCobrosDatosAceptarEventHandler(ctrlDatos_OrdenesCobrosModificarDatosAceptar);
            this.ctrlDatos.OrdenesCobrosModificarDatosCancelar += new IU.Modulos.Cobros.Controles.OrdenesCobrosAfiliadosDatos.OrdenesCobrosDatosCancelarEventHandler(ctrlDatos_OrdenesCobrosModificarDatosCancelar);

            if (!this.IsPostBack)
            {
                CobOrdenesCobros ordenCobro = new CobOrdenesCobros();
                ordenCobro.Afiliado = this.MiAfiliado;
                ordenCobro.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.OrdenesCobrosAfiliados);
                ordenCobro.ModuloTesoreriaCajas = true;
                this.ctrlDatos.IniciarControl(ordenCobro, Gestion.Agregar);
            }
        }

        void ctrlDatos_OrdenesCobrosModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasAfiliadosInicio.aspx"), true);
        }

        void ctrlDatos_OrdenesCobrosModificarDatosAceptar(CobOrdenesCobros e)
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