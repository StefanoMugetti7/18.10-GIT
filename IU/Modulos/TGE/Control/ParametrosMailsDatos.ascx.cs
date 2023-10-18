using Auditoria;
using Auditoria.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Generales.LogicaNegocio;
using ProcesosDatos;
using Seguridad.FachadaNegocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.TGE.Control
{
    public partial class ParametrosMailsDatos : ControlesSeguros
    {
        private TGEParametrosMails MiParametroMail
        {
            get { return (TGEParametrosMails)Session[this.MiSessionPagina + "ParametrosMailsDatosMiParametroMail"]; }
            set { Session[this.MiSessionPagina + "ParametrosMailsDatosMiParametroMail"] = value; }
        }

        public delegate void ParametrosDatosAceptarEventHandler(object sender, TGEParametrosMails e);
        public event ParametrosDatosAceptarEventHandler ParametrosMailsDatosAceptar;

        public delegate void ParametrosDatosCancelarEventHandler();
        public event ParametrosDatosCancelarEventHandler ParametrosMailsDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
            }
        }

        public void IniciarControl(TGEParametrosMails pParametro, Gestion pGestion)
        {
            this.MiParametroMail = pParametro;
            this.GestionControl = pGestion;
            btnVerificarPrueba.Visible = false;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiParametroMail.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    this.MiParametroMail = TGEGeneralesF.ParametrosMailsObtenerDatosCompletos(MiParametroMail);
                    break;
                case Gestion.Modificar:
                    this.MiParametroMail = TGEGeneralesF.ParametrosMailsObtenerDatosCompletos(MiParametroMail);
                    this.MapearObjetoAControles(this.MiParametroMail);
                    break;
                default:
                    break;
            }
            this.ctrAuditoria.IniciarControl(MiParametroMail);
        }

        private void MapearObjetoAControles(TGEParametrosMails pParametro)
        {
            this.txtDireccionCorreo.Text = pParametro.DireccionCorreo;
            this.txtNombre.Text = pParametro.Nombre;
            this.txtServidor.Text = pParametro.Servidor;
            this.chkHabilitar.Checked = pParametro.HabilitarSSL;
            this.txtPuerto.Text = pParametro.Puerto.ToString();
            this.txtUsuario.Text = pParametro.Usuario;
            this.txtContrasena.Attributes.Add("value", pParametro.Contrasena);
        }

        private void MapearControlesAObjeto(TGEParametrosMails pParametro)
        {
            pParametro.DireccionCorreo = this.txtDireccionCorreo.Text;
            pParametro.Nombre = this.txtNombre.Text;
            pParametro.Servidor = this.txtServidor.Text;
            pParametro.HabilitarSSL = this.chkHabilitar.Checked;
            pParametro.Puerto = this.txtPuerto.Text == string.Empty ? 0 : Convert.ToInt32(this.txtPuerto.Text);
            pParametro.Usuario = this.txtUsuario.Text;
            pParametro.Contrasena = this.txtContrasena.Text;
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = false;
            switch (this.GestionControl)
            {
                case Gestion.Modificar:
                    guardo = this.GuardarDatos();
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.MostrarMensaje(this.MiParametroMail.CodigoMensaje, false);
                this.IniciarControl(this.MiParametroMail, Gestion.Modificar);
                //this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiParametroMail.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiParametroMail.CodigoMensaje, true, this.MiParametroMail.CodigoMensajeArgs);
            }
        }

        private bool GuardarDatos()
        {
            bool guardo = false;
            this.MiParametroMail.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.MapearControlesAObjeto(this.MiParametroMail);
            guardo = TGEGeneralesF.ParametrosMailsActualizarModificar(this.MiParametroMail);
            return guardo;
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.ParametrosMailsDatosAceptar != null)
                this.ParametrosMailsDatosAceptar(null, this.MiParametroMail);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ParametrosMailsDatosCancelar != null)
                this.ParametrosMailsDatosCancelar();
        }

        protected void btnPrueba_Click(object sender, EventArgs e)
        {
            gvDatos.Visible = false;
            this.GuardarDatos();
            this.IniciarControl(this.MiParametroMail, Gestion.Modificar);

            MailMessage mail = new MailMessage();
            mail.To.Add(new MailAddress(UsuarioActivo.CorreoElectronico));
            mail.Subject = "Prueba de configuración de mail";
            mail.Body = "Prueba de configuración de mail enviado desde Evol S.R.L";

            bool resultado = AuditoriaF.MailsEnviosAgregar(mail, this.MiParametroMail, 13);

            if (resultado)
            {
                TGEGeneralesF.EnviarProcesoMails();
            }

            if (resultado)
            {
                btnVerificarPrueba.Visible = true;
                MostrarMensaje("Se ha enviado un correo electronico a la direccion: " + UsuarioActivo.CorreoElectronico, false);
            }
            else
            {
                MostrarMensaje("No se ha podido enviar el correo electronico, revise los datos ingresados.", true);
                btnVerificarPrueba.Visible = false;
            }
        }

        

        protected void btnVerificarPrueba_Click(object sender, EventArgs e)
        {
            MailsEnvios mailsEnvios = new MailsEnvios();
            mailsEnvios.IdMailEnvio = MiParametroMail.HashTransaccion;
            gvDatos.DataSource = AuditoriaF.MailsEnviosObtenerLog(mailsEnvios);
            gvDatos.DataBind();
            gvDatos.Visible = true;
        }
    }
}