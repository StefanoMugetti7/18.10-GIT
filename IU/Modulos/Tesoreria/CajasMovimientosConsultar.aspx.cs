using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tesorerias.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Tesoreria
{
    public partial class CajasMovimientosConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ModificarDatos.ModificarDatosAceptar += new Controles.CajasMovimientosDatos.ControlDatosAceptarEventHandler(ModificarDatos_ModificarDatosAceptar);
            this.ModificarDatos.ModificarDatosCancelar += new Controles.CajasMovimientosDatos.ControlDatosCancelarEventHandler(ModificarDatos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                TESCajasMovimientos movimiento = new TESCajasMovimientos();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdCajaMovimiento"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdCajaMovimiento"]);
                movimiento.IdCajaMovimiento = parametro;

                this.ModificarDatos.IniciarControl(movimiento, new TESCajas(), Gestion.Consultar);
            }
        }

        void ModificarDatos_ModificarDatosCancelar()
        {
            string url = AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosListar.aspx");
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString());
            else
                this.Response.Redirect(url, true);
        }

        //void ModificarDatos_ModificarDatosAceptar(TESCajasMovimientos e)
        //{
        //    string url = "~/Modulos/Tesoreria/CajasMovimientosListar.aspx";
        //    if (this.ViewState["UrlReferrer"] != null)
        //        this.Response.Redirect( this.ViewState["UrlReferrer"].ToString());
        //    else
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(url), true);
        //}
    }
}