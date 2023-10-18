using Generales.Entidades;
using Reportes.FachadaNegocio;
using System;
using Tesorerias;

namespace IU.Modulos.Tesoreria
{
    public partial class CajasCerrar : PaginaCajas
    {
        protected override void PageLoadEventCajas(object sender, EventArgs e)
        {
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(this.popUpMensajes_popUpMensajesPostBackAceptar);
            base.PageLoadEventCajas(sender, e);
            if (!this.IsPostBack)
            {
                this.txtUsuario.Text = this.UsuarioActivo.ApellidoNombre;
                this.txtFecha.Text = DateTime.Now.ToString();
            }
        }
        void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            //   this.CargarReporte(this.MiCaja, Generales.Entidades.EnumTGEComprobantes.CajasParteDiario, true);
            //this.ctrPopUpComprobantes.CargarReporte(this.MiCaja, Generales.Entidades.EnumTGEComprobantes.CajasParteDiario);
            //this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
            byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CajasParteDiario, "TesCajasComprobantes", MiCaja, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            ExportPDF.ExportarPDF(pdf, this, "TesCajasComprobantes", this.UsuarioActivo);
            this.btnImprimir.Visible = true;
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CajasParteDiario, "TesCajasComprobantes", MiCaja, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            ExportPDF.ExportarPDF(pdf, this, "TesCajasComprobantes", this.UsuarioActivo);
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.MiCaja.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.MiCaja.TraspasarFondos = this.chkTraspasarFondos.Checked;

            if (TesoreriasF.CajasCerrar(this.MiCaja))
            {
                this.btnAceptar.Visible = false;
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiCaja.CodigoMensaje));
            }
            else
                this.MostrarMensaje(this.ObtenerMensajeSistema(this.MiCaja.CodigoMensaje), true);
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosAutomaticos.aspx"), true);
        }
    }
}