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
using Comunes.Entidades;
using Prestamos;
using Generales.FachadaNegocio;
using Generales.Entidades;


namespace IU.Modulos.Prestamos.Controles
{
    public partial class PrestamoSimulacionModificarDatos : ControlesSeguros
    {

        private PrePrestamos MiPrePrestamos
        {
            get { return (PrePrestamos)Session[this.MiSessionPagina + "SimulacionMiPrePrestamos"]; }
            set { Session[this.MiSessionPagina + "SimulacionMiPrePrestamos"] = value; }
        }

        //public delegate void PrestamosAfiliadosDatosAceptarEventHandler(object sender, PrePrestamos e);
        //public event PrestamosAfiliadosDatosAceptarEventHandler PrestamosAfiliadosModificarDatosAceptar;

        public delegate void PrestamosAfiliadosDatosCancelarEventHandler();
        public event PrestamosAfiliadosDatosCancelarEventHandler PrestamosAfiliadosModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (this.IsPostBack)
            {
                if (this.MiPrePrestamos == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Solicitud Material
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(PrePrestamos pPrestamos, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiPrePrestamos = pPrestamos;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.ddlEstado.Enabled = false;
                    this.txtFechaAlta.Text = DateTime.Today.ToShortDateString();
                    this.TabPanel1.Visible = false;
                    break;
                case Gestion.Consultar:
                    this.MiPrePrestamos = PrePrestamosF.SimulacionObtenerDatosCompletos(pPrestamos);
                    this.MapearObjetoAControles(this.MiPrePrestamos);
                    this.DeshabilitarControles();
                    this.btnAceptar.Visible = false;
                    this.txtMonto.Enabled = false;
                    this.TabPanel1.Visible = true;
                    this.ddlTipoValor.Enabled = false;
                    break;
                default:
                    break;
            }
        }

        private void DeshabilitarControles()
        {
            this.txtFechaAlta.Enabled = false;
            this.ddlEstado.Enabled = false;
            this.ddlPlan.Enabled = false;
            this.txtTasaInteres.Enabled = false;
            this.txtCantidadCuotas.Enabled = false;
            this.ddlTipoOperacion.Enabled = false;
            this.ddlMoneda.Enabled = false;
            this.ddlTipoValor.Enabled = false;
        }

        private void CargarCombos()
        {
            this.ddlTipoOperacion.DataSource = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(EnumTGETiposFuncionalidades.Prestamos);
            this.ddlTipoOperacion.DataValueField = "IdTipoOperacion";
            this.ddlTipoOperacion.DataTextField = "TipoOperacion";
            this.ddlTipoOperacion.DataBind();
            if (this.ddlTipoOperacion.Items.Count>1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoOperacion, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlPlan.DataSource = PrePrestamosF.PrestamosPlanesObtenerLista(new PrePrestamosPlanes());
            this.ddlPlan.DataValueField = "IdPrestamoPlan";
            this.ddlPlan.DataTextField = "Descripcion";
            this.ddlPlan.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlPlan, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlMoneda.DataSource = TGEGeneralesF.MonedasObtenerListaActiva();
            this.ddlMoneda.DataValueField = "IdMoneda";
            this.ddlMoneda.DataTextField = "miMonedaDescripcion";
            this.ddlMoneda.DataBind();
            if (this.ddlMoneda.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlMoneda, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTipoValor.DataSource = TGEGeneralesF.TiposValoresObtenerLista();
            this.ddlTipoValor.DataValueField = "IdTipoValor";
            this.ddlTipoValor.DataTextField = "TipoValor";
            this.ddlTipoValor.DataBind();
            if (this.ddlTipoValor.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoValor, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosPrestamos));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.SelectedValue = ((int)EstadosPrestamos.Activo).ToString();
        }

        private void MapearControlesAObjeto(PrePrestamos pPrestamos)
        {
            pPrestamos.FechaPrestamo = Convert.ToDateTime(this.txtFechaAlta.Text);
            pPrestamos.CantidadCuotas = Convert.ToInt32(this.txtCantidadCuotas.Text);
            pPrestamos.ImporteSolicitado = Convert.ToDecimal(this.txtMonto.Text);
            pPrestamos.TipoOperacion.IdTipoOperacion = Convert.ToInt32(this.ddlTipoOperacion.SelectedValue);
            pPrestamos.PrestamoPlan.IdPrestamoPlan = Convert.ToInt32(this.ddlPlan.SelectedValue);
            pPrestamos.Moneda.IdMoneda = Convert.ToInt32(this.ddlMoneda.SelectedValue);
            pPrestamos.TipoValor.IdTipoValor = Convert.ToInt32(this.ddlTipoValor.SelectedValue);

        }

        private void MapearObjetoAControles(PrePrestamos pPrestamos)
        {
            this.txtFechaAlta.Text = pPrestamos.FechaEvento.ToShortDateString();
            this.txtCantidadCuotas.Text = pPrestamos.CantidadCuotas.ToString();
            this.txtMonto.Text = pPrestamos.ImporteSolicitado.ToString();
            this.ddlTipoOperacion.SelectedValue = pPrestamos.TipoOperacion.IdTipoOperacion.ToString();

            //Por si el Plan esta dado de baja.
            ListItem item = this.ddlPlan.Items.FindByValue(pPrestamos.PrestamoPlan.IdPrestamoPlan.ToString());
            if (item == null)
                this.ddlPlan.Items.Add(new ListItem(pPrestamos.PrestamoPlan.Descripcion, pPrestamos.PrestamoPlan.IdPrestamoPlan.ToString()));
            this.ddlPlan.SelectedValue = pPrestamos.PrestamoPlan.IdPrestamoPlan.ToString();          
            this.txtTasaInteres.Text = pPrestamos.PrestamoPlan.PrestamoPlanTasa.Tasa.ToString();
            this.ddlMoneda.SelectedValue = pPrestamos.Moneda.IdMoneda.ToString();

            AyudaProgramacion.CargarGrillaListas<PrePrestamosCuotas>(pPrestamos.PrestamosCuotas, false, this.gvDatos, true);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiPrePrestamos);
            this.MiPrePrestamos.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    //this.MiPrePrestamos.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    //this.MiPrePrestamos.ImporteAutorizado = this.MiPrePrestamos.ImporteSolicitado;
                    guardo = PrePrestamosF.SimulacionAgregar(this.MiPrePrestamos);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.btnAceptar.Visible = false;
                this.IniciarControl(this.MiPrePrestamos, Gestion.Consultar);
                this.UpdatePanel2.Update();
                //this.ctrPopUpComprobantes.CargarReporte(this.MiPrePrestamos, EnumTGEComprobantes.PreSimulaciones);
                //this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiPrePrestamos.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiPrePrestamos.CodigoMensaje, true, this.MiPrePrestamos.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.PrestamosAfiliadosModificarDatosCancelar != null)
                this.PrestamosAfiliadosModificarDatosCancelar();
        }

        protected void ddlPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlPlan.SelectedValue))
            {
                this.MiPrePrestamos.PrestamoPlan.IdPrestamoPlan = Convert.ToInt32(this.ddlPlan.SelectedValue);
                this.MiPrePrestamos.PrestamoPlan.Descripcion = this.ddlPlan.SelectedItem.Text;
                this.MiPrePrestamos.PrestamoPlan.PrestamoPlanTasa = PrePrestamosF.PrestamosPlanesTasasObtenerTasaActiva(this.MiPrePrestamos.PrestamoPlan);
                this.txtTasaInteres.Text = this.MiPrePrestamos.PrestamoPlan.PrestamoPlanTasa.Tasa.ToString();
            }
            else
            {
                this.txtTasaInteres.Text = string.Empty;
            }

        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MiPrePrestamos.PrestamosCuotas;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MiPrePrestamos.PrestamosCuotas = this.OrdenarGrillaDatos<PrePrestamosCuotas>(this.MiPrePrestamos.PrestamosCuotas, e);
            this.gvDatos.DataSource = this.MiPrePrestamos.PrestamosCuotas;
            this.gvDatos.DataBind();
        }

    }
}