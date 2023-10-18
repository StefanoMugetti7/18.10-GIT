using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afiliados.Entidades;
using Comunes.Entidades;
using Afiliados;
using Generales.FachadaNegocio;

namespace IU.Modulos.Afiliados.Controles
{
    public partial class MensajesAlertasDatos : ControlesSeguros
    {
        private AfiMensajesAlertas MiMensaje
        {
            get { return (AfiMensajesAlertas)Session[this.MiSessionPagina + "MensajesAlertasDatosMiMensaje"]; }
            set { Session[this.MiSessionPagina + "MensajesAlertasDatosMiMensaje"] = value; }
        }

        public delegate void MensajesAlertasDatosAceptarEventHandler(object sender, AfiMensajesAlertas e);
        public event MensajesAlertasDatosAceptarEventHandler MensajesAlertasDatosAceptar;
        public delegate void MensajesAlertasDatosCancelarEventHandler();
        public event MensajesAlertasDatosCancelarEventHandler MensajesAlertasDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);

            if (this.IsPostBack)
            {
                if (this.MiMensaje == null)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/ControlSesion.aspx"), true);
                }
            }
        }

        public void IniciarControl(AfiMensajesAlertas pMensaje, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            this.MiMensaje = pMensaje;
            

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.txtMensaje.ReadOnly = false;
                    this.ddlEstados.Enabled = true;
                    this.btnAceptar.Visible = true;
                    break;
                case Gestion.Modificar:
                    this.txtMensaje.ReadOnly = false;
                    this.ddlEstados.Enabled = true;
                    this.MiMensaje = AfiliadosF.MensajesAlertasObtenerDatosCompletos(pMensaje);
                    MapearObjetoAControles(this.MiMensaje);
                    this.btnAceptar.Visible = true;
                    break;
                case Gestion.Consultar:
                    this.MiMensaje = AfiliadosF.MensajesAlertasObtenerDatosCompletos(pMensaje);
                    MapearObjetoAControles(this.MiMensaje);
                    this.ddlEstados.Enabled = false;
                    
                    break;
            }
        }

        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosMensajesAlertas));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            
        }

        #region Mapeo de Datos
        private void MapearObjetoAControles(AfiMensajesAlertas pMensaje)
        {
            this.txtMensaje.Text = this.MiMensaje.Mensaje;
            this.ddlEstados.SelectedValue = this.MiMensaje.Estado.IdEstado.ToString();
            this.ctrAuditoria.IniciarControl(pMensaje);
        }

        private void MapearControlesAObjeto(AfiMensajesAlertas pMensaje)
        {
            this.MiMensaje.Mensaje = this.txtMensaje.Text;
            this.MiMensaje.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
        }
        #endregion

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("MensajesAlertasDatosAceptar");
            if (!this.Page.IsValid)
                return;
            bool  guardo = true;
            this.MapearControlesAObjeto(this.MiMensaje);
            this.MiMensaje.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiMensaje.FechaAlta = DateTime.Now;
                    {
                        MiMensaje.EstadoColeccion = EstadoColecciones.Agregado;
                    }
                    guardo = AfiliadosF.MensajesAlertasAgregar(this.MiMensaje);
                    break;
                case Gestion.Modificar:
                    this.MiMensaje.FechaEvento = DateTime.Now;
                    {
                        MiMensaje.EstadoColeccion = EstadoColecciones.Modificado;
                    }
                    guardo = AfiliadosF.MensajesAlertasModificar(this.MiMensaje);
                    break;
                default:
                    break;
            }

            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiMensaje.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiMensaje.CodigoMensaje, true, this.MiMensaje.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.MensajesAlertasDatosCancelar != null)
                this.MensajesAlertasDatosCancelar();
        }

        void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.MensajesAlertasDatosAceptar != null)
                this.MensajesAlertasDatosAceptar(null, this.MiMensaje);
        }
    }
}