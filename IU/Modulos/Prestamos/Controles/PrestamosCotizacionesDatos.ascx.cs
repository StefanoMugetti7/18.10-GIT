using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Prestamos;
using Prestamos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Prestamos.Controles
{
    public partial class PrestamosCotizacionesDatos : ControlesSeguros
    {
        public PrePrestamosCotizacionesUnidades MiCotizacion
        {
            get { return this.PropiedadObtenerValor<PrePrestamosCotizacionesUnidades>("CotizacionesDatosMiCotizacion"); }
            set { this.PropiedadGuardarValor("CotizacionesDatosMiCotizacion", value); }
        }

        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }

        public void IniciarControl(PrePrestamosCotizacionesUnidades pParametro, Gestion pGestion)
        {
            this.MiCotizacion = pParametro;
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    string mensaje = ObtenerMensajeSistema("ValidarPrestamosCotizaciones");
                    string funcion = string.Format("showConfirm(this,'{0}');return false;",mensaje);
                    btnAceptar.Attributes.Add("OnClick", funcion);
                    break;
                default:
                    break;
            }
        }

        private void CargarCombos()
        {
            this.ddlTipoUnidad.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.TiposUnidades);
            this.ddlTipoUnidad.DataValueField = "IdListaValorDetalle";
            this.ddlTipoUnidad.DataTextField = "Descripcion";
            this.ddlTipoUnidad.DataBind();
            if (this.ddlTipoUnidad.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoUnidad, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void MapearControlesAObjeto(PrePrestamosCotizacionesUnidades pCotizaciones)
        {
            pCotizaciones.TipoUnidad.IdTiposUnidades = ddlTipoUnidad.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlTipoUnidad.SelectedValue);
            pCotizaciones.Coeficiente = txtCoeficiente.Text == string.Empty ? 0 : Convert.ToDecimal(txtCoeficiente.Text);
            pCotizaciones.FechaDesdeAplica = txtFechaDesdeAplica.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(txtFechaDesdeAplica.Text);
        }

        private void MapearObjetoAControles(PrePrestamosCotizacionesUnidades pCotizaciones)
        {
            txtCoeficiente.Text = pCotizaciones.Coeficiente.ToString();
            lblFechaDesdeAplica.Text = pCotizaciones.FechaDesdeAplica.ToShortDateString();
            ddlTipoUnidad.SelectedValue = pCotizaciones.TipoUnidad.IdTiposUnidades.ToString();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.btnAceptar.Visible = false;
            this.MapearControlesAObjeto(this.MiCotizacion);

            if (this.MiCotizacion.Coeficiente <= 0)
            {
                MostrarMensaje("ValidarCoeficienteCotizaciones", true);
                this.btnAceptar.Visible = true;
                return;
            } 

            this.MiCotizacion.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = PrePrestamosF.PrestamosCotizacionesUnidadesAgregar(this.MiCotizacion);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiCotizacion.CodigoMensaje, false);
                    }
                    break;
                default:
                    break;
            }
            if (!guardo)
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(this.MiCotizacion.CodigoMensaje, true, this.MiCotizacion.CodigoMensajeArgs);

                if (this.MiCotizacion.dsResultado != null)
                {
                    this.MiCotizacion.dsResultado = null;
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ControlModificarDatosCancelar != null)
                this.ControlModificarDatosCancelar();
        }
    }
}