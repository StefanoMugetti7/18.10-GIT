using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seguridad.FachadaNegocio;
using Comunes.Entidades;
using Servicio.Encriptacion;


namespace IU.Modulos.Seguridad
{
    public partial class SegCambiarContrasenia : PaginaSegura
    {
        private Usuarios MiUsuario
        {
            get { return (Usuarios)Session[this.MiSessionPagina + "SegCambiarContraseniaMiUsuario"]; }
            set { Session[this.MiSessionPagina + "SegCambiarContraseniaMiUsuario"] = value; }
        }


        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
        }

        public bool ValidarContraseñaAnterior()
        {
            string passVieja = Encriptar.EncriptarHash(this.txtContraseniaAnterior.Text);
            if (this.MiUsuario.Contrasenia != passVieja)
            {
                this.MiUsuario.CodigoMensaje = "ContraseñaNoValida";
                this.MostrarMensaje(this.MiUsuario.CodigoMensaje, true);
                return false;
            }
            return true;
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Validate();
            if (!this.IsValid)
                return;
            this.MiUsuario = SeguridadF.UsuariosObtenerDatosCompleto(this.UsuarioActivo);
            
            if (!this.ValidarContraseñaAnterior())
                return;

            this.MiUsuario.Contrasenia = txtContrasenia.Text;
            this.MiUsuario.CambiarContrasenia = false;
            this.MiUsuario.ResetearContrasenia = true;
            this.MiUsuario.EstadoColeccion = EstadoColecciones.Modificado;
            this.MiUsuario.IdUsuarioEvento = this.UsuarioActivo.IdUsuario;
            //this.UsuarioActivo.IdAgencias = this.UsuarioActivo.ObtenerAgenciaActual().IdAgencias;
            
            if (SeguridadF.UsuariosModificar(this.MiUsuario))
            {
                this.btnAceptar.Visible = false;
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiUsuario.CodigoMensaje));
                //para que salga de la pagina cambiar contraseña FALSE
                this.UsuarioActivo.CambiarContrasenia = false; 
            }
            else
            {
                this.MostrarMensaje(MiUsuario.CodigoMensaje, true);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
        }
    }
}
