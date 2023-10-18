using System;
using System.Collections;
using System.Configuration;
using System.Data;
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
using System.Collections.Generic;
using Generales.Entidades;
using Reportes.FachadaNegocio;

namespace IU.Modulos.Tesoreria.Controles
{
    public enum TipoCierre
    {
        CierreDia = 1,
        CierreAnterior = 2,
    }

    public partial class TesoreriaCerrarDatos : ControlesSeguros
    {
        public TESTesorerias MiTesoreria
        {
            get
            {
                if (Session[this.MiSessionPagina + "TesoreriaCerrarDatosMiTesoreria"] != null)
                    return (TESTesorerias)Session[this.MiSessionPagina + "TesoreriaCerrarDatosMiTesoreria"];
                else
                {
                    return (TESTesorerias)(Session[this.MiSessionPagina + "TesoreriaCerrarDatosMiTesoreria"] = new TESTesorerias());
                }
            }
            set { Session[this.MiSessionPagina + "TesoreriaCerrarDatosMiTesoreria"] = value; }
        }

        public TipoCierre MiTipoCierre
        {
            get
            {
                if (Session[this.MiSessionPagina + "TesoreriaCerrarDatosMiTipoCierre"] != null)
                    return (TipoCierre)Session[this.MiSessionPagina + "TesoreriaCerrarDatosMiTipoCierre"];
                else
                {
                    return (TipoCierre)(Session[this.MiSessionPagina + "TesoreriaCerrarDatosMiTipoCierre"] = new TipoCierre());
                }
            }
            set { Session[this.MiSessionPagina + "TesoreriaCerrarDatosMiTipoCierre"] = value; }
        }

        public List<TESCajas> MisCajasAbiertas
        {
            get
            {
                if (Session[this.MiSessionPagina + "TesoreriaCerrarDatosMisCajasAbiertas"] != null)
                    return (List<TESCajas>)Session[this.MiSessionPagina + "TesoreriaCerrarDatosMisCajasAbiertas"];
                else
                {
                    return (List<TESCajas>)(Session[this.MiSessionPagina + "TesoreriaCerrarDatosMisCajasAbiertas"] = new List<TESCajas>());
                }
            }
            set { Session[this.MiSessionPagina + "TesoreriaCerrarDatosMisCajasAbiertas"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
                //string mensaje;
                //TESCajas cajaFiltro = new TESCajas();
                //cajaFiltro.Tesoreria.IdTesoreria = this.MiTesoreria.IdTesoreria;
                //if (TesoreriasF.CajasObtenerAbiertas(cajaFiltro).Count() > 0)
                //{
                //    mensaje = this.ObtenerMensajeSistema("TesoreriaCerrarConCajas");
                //}
                //else
                //     mensaje = this.ObtenerMensajeSistema("TesoreriaConfirmarCierre");
                //string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                //this.btnAceptar.Attributes.Add("OnClick", funcion);
            }
        }

        void popUpMensajes_popUpMensajesPostBackAceptar()
        {
                this.ctrPopUpComprobantes.CargarReporte(this.MiTesoreria, Generales.Entidades.EnumTGEComprobantes.TesoreriasParteDiario);  
        }

        public void IniciarControl(TESTesorerias pTesoreria, TipoCierre pCierre)
        {
            this.MiTesoreria = pTesoreria;
            #region Cargar mensaje Confirmacion y guardar cajas abiertas
            string mensaje;
            TESCajas cajaFiltro = new TESCajas();
            cajaFiltro.Tesoreria.IdTesoreria = this.MiTesoreria.IdTesoreria;
            this.MisCajasAbiertas = TesoreriasF.CajasObtenerAbiertas(cajaFiltro);
            if (this.MisCajasAbiertas.Count() > 0)
            {
                mensaje = this.ObtenerMensajeSistema("TesoreriaCerrarConCajas");
            }
            else
                mensaje = this.ObtenerMensajeSistema("TesoreriaConfirmarCierre");
            string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
            this.btnAceptar.Attributes.Add("OnClick", funcion);
            #endregion
            this.MiTipoCierre = pCierre;
            this.MiTesoreria.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.MiTesoreria = TesoreriasF.TesoreriasObtenerDatosCompletos(this.MiTesoreria);
            this.CargarLista(this.MiTesoreria);
        }

        private void CargarLista(TESTesorerias pTesoreria)
        {
            List<TESTesorerias> lista = new List<TESTesorerias>();
            lista.Add(pTesoreria);
            this.gvDatosCabecera.DataSource = lista;
            this.gvDatosCabecera.DataBind();

            this.gvDatos.DataSource = pTesoreria.TesoreriasMonedas;
            this.gvDatos.DataBind();
        }

        protected void btnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            //this.ctrPopUpComprobantes.CargarReporte(this.MiTesoreria, EnumTGEComprobantes.TesoreriasParteDiario);
            byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.TesoreriasParteDiario, "TesTesoreriaComprobante", this.MiTesoreria, AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo));
            ExportPDF.ExportarPDF(pdf, UpdatePanel2, "TesTesoreriaComprobante", UsuarioActivo);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool resultado = false;
            this.MiTesoreria.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            //cargo las cajas a cerrar 
            this.MiTesoreria.Cajas = this.MisCajasAbiertas;
            switch (this.MiTipoCierre)
            {
                case TipoCierre.CierreDia:

                    resultado = TesoreriasF.TesoreriasCerrar(this.MiTesoreria);
                    break;
                case TipoCierre.CierreAnterior:
                    resultado = TesoreriasF.TesoreriasCierreAutomatico(this.MiTesoreria);
                    break;
                default:
                    break;
            }
            if (resultado)
            {
                this.btnAceptar.Visible = false;
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiTesoreria.CodigoMensaje));
            }
            else
                this.MostrarMensaje(this.ObtenerMensajeSistema(this.MiTesoreria.CodigoMensaje), true, this.MiTesoreria.CodigoMensajeArgs);

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
        }

    }
}