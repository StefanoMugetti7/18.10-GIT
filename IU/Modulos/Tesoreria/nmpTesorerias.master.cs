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
using Comunes.Entidades;
using Generales.Entidades;
using Reportes.FachadaNegocio;

namespace IU.Modulos.Tesoreria
{
    public partial class nmpTesorerias : System.Web.UI.MasterPage
    {
        //private TESTesorerias MiTesoreria
        //{
        //    get { return (TESTesorerias)Session[this.MiSessionPagina + "TesoreriasMovimientosMiTesoreria"]; }
        //    set { Session[this.MiSessionPagina + "TesoreriasMovimientosMiTesoreria"] = value; }
        //}
    
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //if (!new PaginaSegura().paginaActual.URL.Contains("TesoreriasCerrar.aspx"))
                //    this.btnCerrarTesoreria.Visible = new PaginaSegura().ValidarPermiso("TesoreriasCerrar.aspx");
                PaginaTesoreria paginaTeso = new PaginaTesoreria();
                Maestra master = (Maestra)this.Master;
                TESTesorerias teso = paginaTeso.Obtener(master.MiSessionPagina);
                this.CargarLista(teso);
                if (teso.FechaAbrir.Date < DateTime.Now.Date)
                {
                    PaginaSegura paginaSegura = new PaginaSegura();
                    this.lblMensajeTesoreria.Text = string.Format(paginaSegura.ObtenerMensajeSistema("TesoreriaAbiertaFueraDeFechaMsg"), teso.FechaAbrir.ToShortDateString());
                    this.lblMensajeTesoreria.Visible = true;
                }
            }
        }

        private void CargarLista(TESTesorerias tesoreria)
        {
            AyudaProgramacion.CargarGrillaListas<TESTesoreriasMonedas>(tesoreria.TesoreriasMonedas, false, this.gvDatos, true);
        
            gvDatos2.DataSource = TesoreriasF.TesoreriasObtenerDatosCompletosSaldosCajas(tesoreria); ;
            gvDatos2.DataBind();

        }

        public void ActualizarGrilla(TESTesorerias tesoreria)
        {
            this.CargarLista(tesoreria);
            this.UpdatePanel2.Update();
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            if (e.CommandName == Gestion.Impresion.ToString())
            {
                PaginaTesoreria paginaTeso = new PaginaTesoreria();
                Maestra master = (Maestra)this.Master;
                TESTesorerias teso = paginaTeso.Obtener(master.MiSessionPagina);
                //this.ctrPopUpComprobantes.CargarReporte(teso, EnumTGEComprobantes.TesoreriasParteDiario);
                
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.TesoreriasParteDiario, "TesTesoreriaComprobante", teso, AyudaProgramacion.ObtenerDatosUsuario(paginaTeso.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, UpdatePanel2, "TesTesoreriaComprobante", paginaTeso.UsuarioActivo);
            }
        }
        protected void gvDatos2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            if (e.CommandName == Gestion.Impresion.ToString())
            {
                PaginaTesoreria paginaTeso = new PaginaTesoreria();
                Maestra master = (Maestra)this.Master;
                TESTesorerias teso = paginaTeso.Obtener(master.MiSessionPagina);
                this.ctrPopUpComprobantes.CargarReporte(teso, EnumTGEComprobantes.TesoreriasParteDiario);
            }
        }
    }
}
