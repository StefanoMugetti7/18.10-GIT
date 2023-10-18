using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Generales.Entidades;
using Tesorerias.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using Tesorerias;
using Contabilidad;
using Contabilidad.Entidades;

namespace IU.Modulos.Tesoreria
{
    public partial class CajasMovimientosAgregar : PaginaCajas
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ModificarDatos.ModificarDatosAceptar += new Controles.CajasMovimientosDatos.ControlDatosAceptarEventHandler(ModificarDatos_ModificarDatosAceptar);
            this.ModificarDatos.ModificarDatosCancelar += new Controles.CajasMovimientosDatos.ControlDatosCancelarEventHandler(ModificarDatos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                TESCajasMovimientos movimiento = new TESCajasMovimientos();
                this.ModificarDatos.IniciarControl(movimiento, this.MiCaja, Gestion.Agregar);
            }
        }
        void ModificarDatos_ModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);

            else
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosAutomaticos.aspx"), true);
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