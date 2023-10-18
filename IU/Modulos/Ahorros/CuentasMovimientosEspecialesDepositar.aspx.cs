using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ahorros.Entidades;
using Generales.Entidades;
using System.Collections;

namespace IU.Modulos.Ahorros
{
    public partial class CuentasMovimientosEspecialesDepositar : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            //this.DepositarDatos.CuentaMovimientosModificarDatosAceptar += new IU.Modulos.Ahorros.Controles.CuentasMovimientosModificarDatos.CuentaMovimientosDatosAceptarEventHandler(DepositarDatos_CuentaMovimientosModificarDatosAceptar);
            this.DepositarDatos.CuentaMovimientosModificarDatosCancelar += new IU.Modulos.Ahorros.Controles.CuentasMovimientosModificarDatos.CuentaMovimientosDatosCancelarEventHandler(DepositarDatos_CuentaMovimientosModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                AhoCuentasMovimientos cuentasMovimientos = new AhoCuentasMovimientos();
                cuentasMovimientos.Cuenta.Afiliado = this.MiAfiliado;
                this.DepositarDatos.IniciarControl(cuentasMovimientos, EnumTGETiposMovimientos.Credito);
            }
        }

        //protected void DepositarDatos_CuentaMovimientosModificarDatosAceptar(object sender, global::Ahorros.Entidades.AhoCuentasMovimientos e)
        //{
        //    this.MisParametrosUrl = new Hashtable();
        //    this.MisParametrosUrl.Add("IdCuenta", e.Cuenta.IdCuenta);
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasMovimientosListar.aspx"), true);
        //}

        protected void DepositarDatos_CuentaMovimientosModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Ahorros/CuentasListar.aspx"), true);
        }
    }
}
