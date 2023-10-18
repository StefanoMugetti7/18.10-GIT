using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Tesorerias.Entidades;
using Tesorerias;
using IU.Modulos.Comunes;
using Comunes.Entidades;
using Generales.Entidades;
using IU.Modulos.Tesoreria;
using System.Collections;
using System.Collections.Generic;

namespace IU
{
    public class PaginaCajas : PaginaSegura
    {
        protected TESCajas MiCaja
        {
            get
            {
                if (Session[this.MiSessionPagina + "PaginaCajasMiCaja"] != null)
                    return (TESCajas)Session[this.MiSessionPagina + "PaginaCajasMiCaja"];
                else
                {
                    return (TESCajas)(Session[this.MiSessionPagina + "PaginaCajasMiCaja"] = new TESCajas());
                }
            }
            set { Session[this.MiSessionPagina + "PaginaCajasMiCaja"] = value; }
        }

        public void Guardar(string key, TESCajas pObj)
        {
            Session[key + "PaginaCajasMiCaja"] = pObj;
        }

        public TESCajas Obtener(string key)
        {
            return (TESCajas)Session[key + "PaginaCajasMiCaja"];
        }

        protected TESCajasMovimientos MiCajaMovimientoPendiente
        {
            get
            {
                if (Session[this.MiSessionPagina + "PaginaCajasMiCajaMovimientoPendiente"] != null)
                    return (TESCajasMovimientos)Session[this.MiSessionPagina + "PaginaCajasMiCajaMovimientoPendiente"];
                else
                {
                    return (TESCajasMovimientos)(Session[this.MiSessionPagina + "PaginaCajasMiCajaMovimientoPendiente"] = new TESCajasMovimientos());
                }
            }
            set { Session[this.MiSessionPagina + "PaginaCajasMiCajaMovimientoPendiente"] = value; }
        }

        public void GuardarMovimiento(string key, TESCajasMovimientos pObj)
        {
            Session[key + "PaginaCajasMiCajaMovimientoPendiente"] = pObj;
        }

        public TESCajasMovimientos ObtenerMovimiento(string key)
        {
            return (TESCajasMovimientos)Session[key + "PaginaCajasMiCajaMovimientoPendiente"];
        }

        public void CargarReporte(Objeto pDatosReporte, EnumTGEComprobantes pComprobante, bool pGenerarComprobante)
        {
            popUpComprobantes comprobante = (popUpComprobantes)this.MaestraPrincipal.FindControl("ContentPlaceHolder1").FindControl("ctrPopUpComprobantes");
            comprobante.CargarReporte(pDatosReporte, pComprobante, pGenerarComprobante);
            UpdatePanel up = (UpdatePanel)this.MaestraPrincipal.FindControl("ContentPlaceHolder1").FindControl("UpdatePanel2");
            up.Update();
        }
        public void CargarReporte(List<TGEArchivos> pArchivos, string nombre)
        {
            popUpComprobantes comprobante = (popUpComprobantes)this.MaestraPrincipal.FindControl("ContentPlaceHolder1").FindControl("ctrPopUpComprobantes");
            comprobante.CargarArchivo(pArchivos, nombre);
            UpdatePanel up = (UpdatePanel)this.MaestraPrincipal.FindControl("ContentPlaceHolder1").FindControl("UpdatePanel2");
            up.Update();
        }
        
        virtual protected void PageLoadEventCajas(object sender, System.EventArgs e) { }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                this.MenuPadre = EnumMenues.Cajas;
                ///Valido que este abierta la Tesoreria
                TESTesorerias teso = new TESTesorerias();
                teso.Filial.IdFilial = UsuarioActivo.FilialPredeterminada.IdFilial;
                teso.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                teso = TesoreriasF.TesoreriasObtenerAbierta(teso);
                if (teso.IdTesoreria == 0)
                {
                    this.MostrarMensaje("TesoreriaValidarAbierta", true);
                    //Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/TesTesoreriasAbrir.aspx"), true);
                }

                //if (this.MiCaja.IdCaja == 0)
                //{
                TESCajas caja = new TESCajas();
                caja.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                caja.Usuario = this.UsuarioActivo;
                caja.Tesoreria = teso;
                caja.FechaAbrir = teso.FechaAbrir;
                caja.FechaAbrirEvento = DateTime.Now;
                caja.Usuario.FilialPredeterminada = this.UsuarioActivo.FilialPredeterminada;

                //Valida que tenga la Caja Abierta
                if (TesoreriasF.CajasValidarAbierta(caja))
                {
                    this.MiCaja = TesoreriasF.CajasObtenerDatosCompletos(caja);
                }
                else
                {
                    this.MiCaja = new TESCajas();
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasAbrir.aspx"), true);
                    //this.MisParametrosUrl = new Hashtable();
                    //this.MisParametrosUrl.Add("CodigoMensaje", "TesoreriaValidarCerrada");
                    //this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/ControlMensajes.aspx"), true);
                }
                //}
            }

            this.PageLoadEventCajas(sender, e);
        }
    }
}
