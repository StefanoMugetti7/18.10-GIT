using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bancos.Entidades;
using Comunes.Entidades;
using IU.Modulos.Bancos.Controles;

namespace IU.Modulos.Bancos
{
    public partial class BancosCuentasMovimientosMultiplesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ModificarDatos.BancosCuentasMovimientosDatosAceptar += new IU.Modulos.Bancos.Controles.BancosCuentasMovimientosDatos.BancosCuentasMovimientosDatosAceptarEventHandler(ModificarDatos_BancosCuentasMovimientosDatosAceptar);

            this.ModificarDatos.BancosCuentasMovimientosDatosCancelar += new IU.Modulos.Bancos.Controles.BancosCuentasMovimientosMultiplesDatos.BancosCuentasMovimientosDatosCancelarEventHandler(ModificarDatos_BancosCuentasMovimientosDatosCancelar);
            if (!this.IsPostBack)
            {
                TESBancosCuentasMovimientos mov = new TESBancosCuentasMovimientos();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdBancoCuenta"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasListar.aspx"), true);

                mov.BancoCuenta.IdBancoCuenta = Convert.ToInt32(this.MisParametrosUrl["IdBancoCuenta"]);

                this.ModificarDatos.IniciarControl(mov, Gestion.Agregar);
            }
        }

        void ModificarDatos_BancosCuentasMovimientosDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasMovimientosListar.aspx"), true);
        }

        //void ModificarDatos_BancosCuentasMovimientosDatosAceptar(object sender, global::Bancos.Entidades.TESBancosCuentasMovimientos e)
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasMovimientosListar.aspx"), true);
        //}
    }
}