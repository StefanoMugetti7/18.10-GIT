using Comunes.Entidades;
using Seguridad.FachadaNegocio;
using System;

namespace IU.Modulos.Seguridad
{
    public partial class SegGestionarUsuariosContrasenia : ControlesSeguros
    {
        private Usuarios MiUsuario
        {
            get { return (Usuarios)Session[this.MiSessionPagina + "SegGestionarUsuarioContraseniaMiUsuario"]; }
            set { Session[this.MiSessionPagina + "SegGestionarUsuarioContraseniaMiUsuario"] = value; }
        }
        public delegate void UsuariosCambiarContraseniaEventHandler(object sender, Usuarios e);
        public event UsuariosCambiarContraseniaEventHandler UsuariosCambiarContrasenia;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            if (!this.IsPostBack) this.MiUsuario = new Usuarios();
        }
        public void IniciarControl(Usuarios pUsuario)
        {
            pUsuario.ResetearContrasenia = true;
            pUsuario.CambiarContrasenia = true;
            this.txtContrasenia.Text = string.Empty;
            this.txtContraseniaVerificar.Text = string.Empty;
            this.MiUsuario = pUsuario;
            this.mpePopUp.Show();
        }
        protected void btnPopUpAceptar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("UsuariosCambiarContrasenia");
            if (!this.Page.IsValid)
                return;

            this.MiUsuario.Contrasenia = this.txtContrasenia.Text;
            this.MiUsuario.EstadoColeccion = EstadoColecciones.Modificado;

            if (SeguridadF.UsuariosModificar(this.MiUsuario))
            {
                if (this.UsuariosCambiarContrasenia != null)
                    this.UsuariosCambiarContrasenia(sender, this.MiUsuario);

                this.txtContrasenia.Text = string.Empty;
                this.txtContraseniaVerificar.Text = string.Empty;
                this.MostrarMensaje(this.MiUsuario.CodigoMensaje, false);
            }
            else
                this.MostrarMensaje(this.MiUsuario.CodigoMensaje, true);

            this.mpePopUp.Hide();
        }
        protected void btnPopUpCancelar_Click(object sender, EventArgs e)
        {
            this.txtContrasenia.Text = string.Empty;
            this.txtContraseniaVerificar.Text = string.Empty;
            this.mpePopUp.Hide();
        }
    }
}