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
using Generales.Entidades;
using IU.Modulos.Comunes;
using Comunes.Entidades;
using Tesorerias;
using Reportes.FachadaNegocio;

namespace IU.Modulos.Tesoreria
{
    public partial class nmpCajas : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //if (!new PaginaSegura().paginaActual.URL.Contains("CajasCerrar.aspx"))
                //    this.btnCerrarCaja.Visible = new PaginaSegura().ValidarPermiso("CajasCerrar.aspx");
                PaginaCajas paginaCaja = new PaginaCajas();
                Maestra master = (Maestra)this.Master;
                TESCajas caja = paginaCaja.Obtener(master.MiSessionPagina);
                this.CargarLista(caja);
                if (caja.FechaAbrir.Date < DateTime.Now.Date)
                {
                    this.lblMensajeCaja.Text = string.Format(paginaCaja.ObtenerMensajeSistema("CajaAbiertaFueraDeFechaMsg"), caja.FechaAbrir.ToShortDateString());
                    this.lblMensajeCaja.Visible = true;
                }
            }
        }

        private void CargarLista(TESCajas caja)
        {
            //AyudaProgramacion.CargarGrillaListas<TESCajasMonedas>(caja.CajasMonedas, false, this.gvDatos, true);
            this.gvDatos.DataSource = TesoreriasF.CajasMonedasSeleccionarTotalesTipoValorPorCaja(caja);
            this.gvDatos.DataBind();
        }

        public void ActualizarGrilla(TESCajas caja)
        {
            this.CargarLista(caja);
            this.UpdatePanel2.Update();
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            if (e.CommandName == Gestion.Impresion.ToString())
            {
                PaginaCajas paginaCaja = new PaginaCajas();
                Maestra master = (Maestra)this.Master;
                TESCajas caja = paginaCaja.Obtener(master.MiSessionPagina);
                //this.ctrPopUpComprobantes.CargarReporte(caja, EnumTGEComprobantes.CajasParteDiario);

                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CajasParteDiario, "TesCajasComprobantes", caja, AyudaProgramacion.ObtenerDatosUsuario(paginaCaja.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, UpdatePanel2, "TesCajasComprobantes", paginaCaja.UsuarioActivo);
            }
        }
    }
}
