using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bancos.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Bancos
{
    public partial class BancosCuentasMovimientosConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ModificarDatos.BancosCuentasMovimientosDatosAceptar += new IU.Modulos.Bancos.Controles.BancosCuentasMovimientosDatos.BancosCuentasMovimientosDatosAceptarEventHandler(ModificarDatos_BancosCuentasMovimientosDatosAceptar);
            this.ModificarDatos.BancosCuentasMovimientosDatosCancelar += new IU.Modulos.Bancos.Controles.BancosCuentasMovimientosDatos.BancosCuentasMovimientosDatosCancelarEventHandler(ModificarDatos_BancosCuentasMovimientosDatosCancelar);
            if (!this.IsPostBack)
            {
                TESBancosCuentasMovimientos mov = new TESBancosCuentasMovimientos();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdBancoCuentaMovimiento"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasListar.aspx"), true);

                mov.IdBancoCuentaMovimiento = Convert.ToInt32(this.MisParametrosUrl["IdBancoCuentaMovimiento"]);

                this.ModificarDatos.IniciarControl(mov, Gestion.Consultar);
            }
        }

        void ModificarDatos_BancosCuentasMovimientosDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasMovimientosListar.aspx"), true);
        }

        //void ModificarDatos_BancosCuentasMovimientosDatosAceptar(object sender, global::Bancos.Entidades.TESBancosCuentasMovimientos e)
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasMovimientosListar.aspx"), true);
        //}
    }
}