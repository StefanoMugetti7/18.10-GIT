using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afiliados.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Afiliados;

namespace IU.Modulos.Afiliados.Controles
{
    public partial class CertificadoSupervivenciaDatos : ControlesSeguros
    {
        private AfiCertificadosSupervivencia MiCertificado
        {
            get { return (AfiCertificadosSupervivencia)Session[this.MiSessionPagina + "CertificadoSupervivenciaDatos"]; }
            set { Session[this.MiSessionPagina + "CertificadoSupervivenciaDatos"] = value; }
        }

        public delegate void AfiCertificadosSupervivenciaAceptarEventHandler(object sender, AfiCertificadosSupervivencia e);
        public event AfiCertificadosSupervivenciaAceptarEventHandler CertificadosSupervivenciaModificarDatosAceptar;

        public delegate void AfiCertificadosSupervivenciaCancelarEventHandler();
        public event AfiCertificadosSupervivenciaCancelarEventHandler CertificadosSupervivenciaModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (this.IsPostBack)
            {
                if (this.MiCertificado == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Solicitud Material
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(AfiCertificadosSupervivencia pCertificado, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiCertificado = pCertificado;
                    this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();
                    this.ddlEstado.Enabled = false;
                    this.ctrArchivos.IniciarControl(this.MiCertificado, Gestion.Agregar);
                    break;
                case Gestion.Modificar:
                    this.MiCertificado = AfiliadosF.CertificadosSupervivenciaObtenerDatosCompletos(pCertificado);
                    this.MapearObjetoAControles(this.MiCertificado);
                    break;
                case Gestion.Consultar:
                    this.MiCertificado = AfiliadosF.CertificadosSupervivenciaObtenerDatosCompletos(pCertificado);
                    this.MapearObjetoAControles(this.MiCertificado);
                    this.ddlEstado.Enabled = false;
                    this.txtFechaCertificado.Enabled = false;
                    this.txtDetalle.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
        }

        private void MapearObjetoAControles(AfiCertificadosSupervivencia pCuenta)
        {
            this.txtFechaCertificado.Text = pCuenta.FechaCertificacion.ToShortDateString();
            this.txtDetalle.Text = pCuenta.Detalle;
            this.ddlEstado.SelectedValue = pCuenta.Estado.IdEstado.ToString();
            this.ctrArchivos.IniciarControl(pCuenta, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pCuenta);
        }

        private void MapearControlesAObjeto(AfiCertificadosSupervivencia pCuenta)
        {
            pCuenta.FechaCertificacion = Convert.ToDateTime( this.txtFechaCertificado.Text);
            pCuenta.Detalle = this.txtDetalle.Text;
            pCuenta.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pCuenta.Archivos = ctrArchivos.ObtenerLista();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiCertificado);
            this.MiCertificado.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiCertificado.UsuarioAlta.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    guardo = AfiliadosF.CertificadosSupervivenciaAgregar(this.MiCertificado);
                    break;
                case Gestion.Modificar:
                    guardo = AfiliadosF.CertificadosSupervivenciaModificar(this.MiCertificado);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.btnAceptar.Visible = false;
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiCertificado.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiCertificado.CodigoMensaje, true, this.MiCertificado.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.CertificadosSupervivenciaModificarDatosCancelar != null)
                this.CertificadosSupervivenciaModificarDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.CertificadosSupervivenciaModificarDatosAceptar != null)
                this.CertificadosSupervivenciaModificarDatosAceptar(null, this.MiCertificado);
        }

    }
}