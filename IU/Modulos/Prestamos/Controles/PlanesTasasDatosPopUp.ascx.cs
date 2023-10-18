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
using Prestamos.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using Generales.Entidades;

namespace IU.Modulos.Prestamos.Controles
{
    public partial class PlanesTasasDatosPopUp : ControlesSeguros
    {
        private PrePrestamosPlanesTasas MiPlanTasa
        {
            get { return (PrePrestamosPlanesTasas)Session[this.MiSessionPagina + "PlanesTasasDatosPopUpMiPlanTasa"]; }
            set { Session[this.MiSessionPagina + "PlanesTasasDatosPopUpMiPlanTasa"] = value; }
        }

        public delegate void PlanesTasasDatosPopUpAceptarEventHandler(PrePrestamosPlanesTasas e);
        public event PlanesTasasDatosPopUpAceptarEventHandler PrePrestamosPlanesTasasAceptar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.MiPlanTasa = new PrePrestamosPlanesTasas();
                //this.txtTasaEfectivaMensual.Attributes.Add("onchange", "CalcularTasaEfectivaMensual();");
                //this.txtTasaEfectivaAnual.Attributes.Add("onchange", "CalcularTasaEfectivaAnual();");
                this.txtTasaNominalAnual.Attributes.Add("onchange", "CalcularTasaNominalAnual();");
            }
        }

        public void IniciarControl(PrePrestamosPlanesTasas pParametro)
        {
            this.CargarCombos();
            this.MiPlanTasa = pParametro;
            this.txtFechaAlta.Text = DateTime.Now.ToShortDateString();
            this.txtFechaDesde.Text = DateTime.Now.ToShortDateString();
            this.txtTasaEfectivaMensual.Text = string.Empty;
            this.txtTasaEfectivaAnual.Text = string.Empty;
            this.txtTasaNominalAnual.Text = string.Empty;
            this.txtCantindadCuotas.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalPlanesTasasPopUp();", true);

        }

        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();
        }
        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalPlanesTasasPopUp();", true);
        }
            protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.MiPlanTasa.FechaInicioVigencia = Convert.ToDateTime(this.txtFechaDesde.Text);
            this.MiPlanTasa.TasaEfectivaMensual = this.txtTasaEfectivaMensual.Decimal;
            this.MiPlanTasa.TasaEfectivaAnual = this.txtTasaEfectivaAnual.Decimal;
            this.MiPlanTasa.Tasa = this.txtTasaNominalAnual.Decimal;
            this.MiPlanTasa.CantidadCuotas = Convert.ToInt32(this.txtCantindadCuotas.Text);
            this.MiPlanTasa.CantidadCuotasHasta = Convert.ToInt32(this.txtCanidadCuotasHasta.Text);
            this.MiPlanTasa.ImporteDesde = this.txtImporteDesde.Decimal == 0 ? default(decimal?) : this.txtImporteDesde.Decimal;
            this.MiPlanTasa.ImporteHasta = this.txtImporteHasta.Decimal == 0 ? default(decimal?) : this.txtImporteHasta.Decimal;
            this.MiPlanTasa.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            this.MiPlanTasa.Estado.Descripcion = this.ddlEstados.SelectedItem.Text;
            this.MiPlanTasa.EstadoColeccion = EstadoColecciones.Agregado;

            if(this.MiPlanTasa.CantidadCuotasHasta < this.MiPlanTasa.CantidadCuotas)
            {
                this.MostrarMensaje("ValidarCantidadCuotasHastaMenorCantidadCuotas", true);

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalPlanesTasasPopUp();", true);
                return;
            }

            if (this.MiPlanTasa.ImporteDesde.HasValue)
            {
                if (!this.MiPlanTasa.ImporteHasta.HasValue || this.MiPlanTasa.ImporteHasta.Value < this.MiPlanTasa.ImporteDesde.Value)
                {
                    this.MostrarMensaje("ValidarImporteDesdeMenorImporteHasta", true);

                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalPlanesTasasPopUp();", true);
                    return;
                }
            }



            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalPlanesTasasPopUp();", true);
            if (this.PrePrestamosPlanesTasasAceptar != null)
                this.PrePrestamosPlanesTasasAceptar(this.MiPlanTasa);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalPlanesTasasPopUp();", true);
        
        }
    }
}