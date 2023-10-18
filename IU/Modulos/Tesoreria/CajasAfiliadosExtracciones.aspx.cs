using Afiliados;
using Ahorros.Entidades;
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
    public partial class CajasAfiliadosExtracciones : PaginaCajasAfiliados
    {
        protected override void PageLoadEventCajas(object sender, EventArgs e)
        {
            base.PageLoadEventCajas(sender, e);
            this.ExtraerDatos.CuentaMovimientosModificarDatosAceptar += new IU.Modulos.Ahorros.Controles.CuentasMovimientosModificarDatos.CuentaMovimientosDatosAceptarEventHandler(DepositarDatos_CuentaMovimientosModificarDatosAceptar);
            this.ExtraerDatos.CuentaMovimientosModificarDatosCancelar += new IU.Modulos.Ahorros.Controles.CuentasMovimientosModificarDatos.CuentaMovimientosDatosCancelarEventHandler(DepositarDatos_CuentaMovimientosModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                AhoCuentasMovimientos cuentasMovimientos = new AhoCuentasMovimientos();
                cuentasMovimientos.Cuenta.Afiliado = this.MiAfiliado;
                this.ExtraerDatos.IniciarControl(cuentasMovimientos, EnumTGETiposMovimientos.Debito);
            }
        }

        protected void DepositarDatos_CuentaMovimientosModificarDatosAceptar(object sender, global::Ahorros.Entidades.AhoCuentasMovimientos e)
        {

            this.MiCajaMovimientoPendiente = new TESCajasMovimientos();

            MiCajaMovimientoPendiente.Afiliado = AfiliadosF.AfiliadosObtenerDatosCompletos(e.Cuenta.Afiliado);

            //this.MiCajaMovimientoPendiente.Afiliado.NumeroDocumento = e.Cuenta.Afiliado.NumeroDocumento;
            this.MiCajaMovimientoPendiente.Importe = e.Importe;
            this.MiCajaMovimientoPendiente.IdRefTipoOperacion = e.IdCuentaMovimiento;
            this.MiCajaMovimientoPendiente.TipoOperacion.IdTipoOperacion = e.TipoOperacion.IdTipoOperacion;
            this.MiCajaMovimientoPendiente.TipoOperacion.TipoOperacion = e.TipoOperacion.TipoOperacion;
            this.MiCajaMovimientoPendiente.CajaMoneda.Moneda = e.Cuenta.Moneda;
            this.MiCajaMovimientoPendiente.MonedaCotizacion = e.MonedaCotizacion;
            this.MiCajaMovimientoPendiente.SelloTiempo = e.SelloTiempo;
            this.MiCajaMovimientoPendiente.Fecha = MiCaja.FechaAbrir;
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosConfirmar.aspx"), true);

        }

        protected void DepositarDatos_CuentaMovimientosModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasAfiliadosInicio.aspx"), true);


        }
    }
}