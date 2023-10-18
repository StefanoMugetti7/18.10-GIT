using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Prestamos.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;

namespace IU.Modulos.Prestamos.Controles
{
    public partial class PlanesIpsDatosPopUp : ControlesSeguros
    {
        private PrePrestamosIpsPlanes MiPlanIps
        {
            get { return (PrePrestamosIpsPlanes)Session[this.MiSessionPagina + "PrePrestamosIpsPlanesMiPlanIps"]; }
            set { Session[this.MiSessionPagina + "PrePrestamosIpsPlanesMiPlanIps"] = value; }
        }

        public delegate void PlanesIpsDatosPopUpAceptarEventHandler(PrePrestamosIpsPlanes e);
        public event PlanesIpsDatosPopUpAceptarEventHandler PrePrestamosPlanesIpsAceptar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.MiPlanIps = new PrePrestamosIpsPlanes();
            }
        }

        public void IniciarControl(PrePrestamosIpsPlanes pParametro)
        {
            this.CargarCombos();
            this.MiPlanIps = pParametro;
            this.txtFechaAlta.Text = DateTime.Now.ToShortDateString();
            this.txtFechaDesde.Text = DateTime.Now.ToShortDateString();
            this.txtImporteTotal.Decimal = 0;
            this.txtCantidadCuotas.Decimal = 0;
            this.txtImporteCuota.Decimal = 0;

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalPopUp();", true);

        }

        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.MiPlanIps.FechaDesde = Convert.ToDateTime(this.txtFechaDesde.Text);
            this.MiPlanIps.ImporteTotal = this.txtImporteTotal.Decimal;
            this.MiPlanIps.CantidadCuotas = Convert.ToInt32(this.txtCantidadCuotas.Decimal);
            this.MiPlanIps.ImporteCuota = this.txtImporteCuota.Decimal;
            this.MiPlanIps.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            this.MiPlanIps.Estado.Descripcion = this.ddlEstados.SelectedItem.Text;
            this.MiPlanIps.EstadoColeccion = EstadoColecciones.Agregado;
            this.MiPlanIps.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalPopUp();", true);
            if (this.PrePrestamosPlanesIpsAceptar != null)
                this.PrePrestamosPlanesIpsAceptar(this.MiPlanIps);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalPopUp();", true);
        }
    }
}