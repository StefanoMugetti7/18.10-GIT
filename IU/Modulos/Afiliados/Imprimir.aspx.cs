using Afiliados;
using Afiliados.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Reportes.FachadaNegocio;
using System;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tesorerias.Entidades;

namespace IU.Modulos.Afiliados
{
    public partial class Imprimir : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            if (!this.IsPostBack)

                this.CargarCombos();
        }
        protected void btnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlComprobantes.SelectedValue))
            {
                TGEComprobantes comprobante = new TGEComprobantes();
                comprobante.IdComprobante = Convert.ToInt32(this.ddlComprobantes.SelectedValue);
                comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(comprobante);
                //TGEPlantillas plantilla = new TGEPlantillas();
                //plantilla.Codigo = comprobante.CodigoPlantilla;
                //plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

                TESCajasMovimientos movimiento = new TESCajasMovimientos();
                movimiento.IdRefTipoOperacion = ddlSocio.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlSocio.SelectedValue);
                movimiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
                movimiento.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.Afiliados;
                movimiento.Filtro = comprobante.CodigoPlantilla;
                TGEPlantillas miPlantilla = TGEGeneralesF.PlantillasObtenerDatosPorTipoOperacion(movimiento);

                AfiAfiliados afiImprimir = new AfiAfiliados();
                afiImprimir.IdAfiliado = this.ddlSocio.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlSocio.SelectedValue);
                afiImprimir.FechaDesde = this.txtFechaDesde.Text == string.Empty ? default(DateTime) : Convert.ToDateTime(this.txtFechaDesde.Text);
                afiImprimir.FechaHasta = this.txtFechaHasta.Text == string.Empty ? default(DateTime) : Convert.ToDateTime(this.txtFechaHasta.Text);
                UsuarioLogueado usulog = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF((EnumTGEComprobantes)comprobante.IdComprobante, miPlantilla.Codigo, afiImprimir, usulog);
                ExportPDF.ExportarPDF(pdf, this, usulog);
            }
        }
        protected void btnFirmarDocumento_Click(object sender, EventArgs e)
        {
            TGEParametrosValores paramVal = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.FirmaDigitalHabilitar);
            bool firmaDigital = paramVal.ParametroValor == string.Empty ? false : Convert.ToBoolean(paramVal.ParametroValor);
            if (!firmaDigital)
            {
                this.MostrarMensaje(this.ObtenerMensajeSistema("ValidarFirmaDigitalManuscritaHabilitar"), true);
                return;
            }
            MailMessage mail = new MailMessage();
            UsuarioLogueado usulog = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            if (AfiliadosF.AfiliadosArmarMailLinkFirmarDocumento(this.MiAfiliado, mail))
            {
                this.popUpMail.IniciarControl(mail, this.MiAfiliado);
            }
            else
            {
                this.MostrarMensaje(this.MiAfiliado.CodigoMensaje, true, this.MiAfiliado.CodigoMensajeArgs);
                return;
            }
        }
        protected void ddlComprobantes_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlComprobantes.SelectedValue))
            {
                if (Convert.ToInt32(this.ddlComprobantes.SelectedValue) == 25)
                {

                    TGEComprobantes comprobante = new TGEComprobantes();
                    comprobante.IdComprobante = Convert.ToInt32(this.ddlComprobantes.SelectedValue);
                    comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(comprobante);

                    TGEFirmarDocumentos firmarDoc = new TGEFirmarDocumentos();
                    firmarDoc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    firmarDoc.IdRefTabla = this.MiAfiliado.IdAfiliado;
                    firmarDoc.Tabla = "AfiAfiliados";

                    PropertyInfo prop = MiAfiliado.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() == 1).ToList()[0];
                    firmarDoc.Key = prop.Name;
                    firmarDoc.CodigoPlantilla = comprobante.CodigoPlantilla;

                    this.btnFirmarDocumento.Visible = TGEGeneralesF.FirmarDocumentosValidar(firmarDoc);
                    this.btnWhatsAppFirmarDocumento.Visible = this.btnFirmarDocumento.Visible;
                    this.hfLinkFirmarDocumento.Value = TGEGeneralesF.FirmarDocumentosArmarLinkFirmarDocumento(firmarDoc).Link;
                    this.copyClipboard.Visible = this.btnFirmarDocumento.Visible;

                    this.btnFirmarDocumentoBaja.Visible = !this.btnFirmarDocumento.Visible;
                    if (this.btnFirmarDocumentoBaja.Visible)
                    {
                        string funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarFirmaDigitalManuscritaBaja"));
                        this.btnFirmarDocumentoBaja.Attributes.Add("OnClick", funcion);
                    }
                    this.btnImprimir.Visible = true;
                }
                else
                {
                    if (Convert.ToInt32(this.ddlComprobantes.SelectedValue) == 44)
                    {
                        this.FechaDesde.Visible = true;
                        //  FechaHasta.Visible = true;
                    }
                    this.btnFirmarDocumento.Visible = false;
                    this.btnWhatsAppFirmarDocumento.Visible = false;
                    this.copyClipboard.Visible = false;
                    this.btnImprimir.Visible = true;
                    this.btnFirmarDocumentoBaja.Visible = false;

                }
            }
            else
            {
                this.FechaDesde.Visible = false;
                //  FechaHasta.Visible = false;
                this.btnFirmarDocumento.Visible = false;
                this.btnWhatsAppFirmarDocumento.Visible = false;
                this.btnFirmarDocumentoBaja.Visible = false;
                this.btnImprimir.Visible = false;
                this.copyClipboard.Visible = false;
            }
        }
        protected void btnWhatsAppFirmarDocumento_Click(object sender, EventArgs e)
        {
            TGEParametrosValores paramVal = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.FirmaDigitalHabilitar);
            bool firmaDigital = paramVal.ParametroValor == string.Empty ? false : Convert.ToBoolean(paramVal.ParametroValor);
            if (!firmaDigital)
            {
                this.MostrarMensaje(this.ObtenerMensajeSistema("ValidarFirmaDigitalManuscritaHabilitar"), true);
                return;
            }
            TGEComprobantes comprobante = new TGEComprobantes();
            comprobante.IdComprobante = Convert.ToInt32(this.ddlComprobantes.SelectedValue);
            comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(comprobante);

            TGEFirmarDocumentos firmarDoc = new TGEFirmarDocumentos();
            firmarDoc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            firmarDoc.IdRefTabla = this.MiAfiliado.IdAfiliado;
            PropertyInfo prop = MiAfiliado.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() == 1).ToList()[0];
            firmarDoc.Key = prop.Name;
            firmarDoc.CodigoPlantilla = comprobante.CodigoPlantilla;
            firmarDoc.Tabla = "AfiAfiliados";

            string text = string.Concat("Estimado ", this.MiAfiliado.ApellidoNombre, " haga clic en el siguiente link para firmar el documento ", TGEGeneralesF.FirmarDocumentosArmarLinkFirmarDocumento(firmarDoc).Link);
            AfiTelefonos cel = AfiliadosF.AfiliadosObtenerTelefonoCelular(this.MiAfiliado);
            string numero = /*cel.Prefijo.ToString() + */cel.Numero.ToString();
            string urlwa = string.Format("https://api.whatsapp.com/send?phone={0}&text={1}", numero, HttpUtility.UrlEncode(text));
            ScriptManager.RegisterStartupScript(this.UpdatePanel2, this.UpdatePanel2.GetType(), "scriptWa", string.Format("EnviarWhatsApp('{0}');", urlwa), true);
        }
        protected void btnFirmarDocumentoBaja_Click(object sender, EventArgs e)
        {
            TGEFirmarDocumentos firmarDoc = new TGEFirmarDocumentos();
            firmarDoc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            firmarDoc.IdRefTabla = this.MiAfiliado.IdAfiliado;
            firmarDoc.Tabla = "AfiAfiliados";
            firmarDoc.IdTipoOperacion = 0;
            firmarDoc.Estado.IdEstado = (int)Estados.Baja;
            bool resultado = TGEGeneralesF.FirmarDocumentosModificar(firmarDoc);
            if (resultado)
            {
                this.btnFirmarDocumentoBaja.Visible = false;
                this.btnFirmarDocumento.Visible = true;
                this.btnWhatsAppFirmarDocumento.Visible = true;
                this.copyClipboard.Visible = true;
                this.MostrarMensaje(firmarDoc.CodigoMensaje, false);
            }
            else
            {
                this.MostrarMensaje(firmarDoc.CodigoMensaje, true, firmarDoc.CodigoMensajeArgs);
            }
        }
        private void CargarCombos()
        {
            this.ddlSocio.DataSource = AfiliadosF.AfiliadosObtenerTitularFamiliares(MiAfiliado);
            this.ddlSocio.DataValueField = "IdAfiliado";
            this.ddlSocio.DataTextField = "ApellidoNombre";
            this.ddlSocio.DataBind();
            this.ddlSocio.SelectedValue = MiAfiliado.IdAfiliado.ToString();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlSocio, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlComprobantes.DataSource = TGEGeneralesF.ComprobantesObtenerListaAfiliado();
            this.ddlComprobantes.DataValueField = "IdComprobante";
            this.ddlComprobantes.DataTextField = "Nombre";
            this.ddlComprobantes.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlComprobantes, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
    }
}
