using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Collections;
using Comunes.Entidades;

namespace IU
{
    public partial class IngresoSistema : System.Web.UI.Page
    {

        protected Usuarios UsuarioActivo
        {
            get
            {
                if (Session["UsuarioActivo"] != null)
                { return (Usuarios)Session["UsuarioActivo"]; }
                else
                { return new Usuarios(); }
            }
            set { Session["UsuarioActivo"] = value; }
        }

        protected Label Mensaje
        {
            get { return (Label)Master.FindControl("lblMensaje"); }
            set
            {
                Label _mensaje = (Label)Master.FindControl("lblMensaje");
                _mensaje = value;
            }
        }

        private bool UsaCaptcha
        {
            get 
            {
                if (Session["IngresoSistemaUsaCaptcha"] != null)
                { return (bool)Session["IngresoSistemaUsaCaptcha"]; }
                else
                { return new bool(); }
            }
            set { Session["IngresoSistemaUsaCaptcha"] = value; }            
        }


        GoogleReCaptcha.GoogleReCaptcha recaptcha = new GoogleReCaptcha.GoogleReCaptcha();
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            recaptcha.PublicKey = "6LdXAQkUAAAAAK9erfis3cCCE4Nc2wT75jwCMbP3";
            recaptcha.PrivateKey = "6LdXAQkUAAAAADUHcEWwlNSaR_Css_Y1pzik1rbI";
            this.pnlGoogleReCaptcha.Controls.Add(recaptcha);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (UsuarioActivo != null && UsuarioActivo.IdUsuario > 0)
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("InicioSistema.aspx"), true);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtContrasenia, this.btnAceptar);

                this.UsaCaptcha = false;
                Objeto parametro = new Objeto();
                
                    this.btnAceptar.Visible = true;
                    this.UsaCaptcha = true;
                    this.recaptcha.Enabled = this.UsaCaptcha;
                    this.recaptcha.Visible = this.recaptcha.Enabled;

                txtUsuario.Focus();
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            this.Session.Clear();
            this.Session.Abandon();
            HttpContext.Current.Session.Abandon();
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("IngresoSistema.aspx"));
        }

        protected void btnCodigo_Click(object sender, EventArgs e)
        {
            if (this.UsaCaptcha && recaptcha.Enabled && !recaptcha.Validate())
            {
                this.MostrarMensaje("Captcha incorrecto! Intente de nuevo!", true);
                this.RecargarCarptcha();
                return;
            }

            string codigo = txtCodigo.Text;

            if (codigo != "30714930083")
            {
                this.MostrarMensaje("El código ingresado no es validos.", true);
            }
            else
            {
                Usuarios usuario = new Usuarios();
                usuario.Usuario = "admin";
                usuario.IdUsuario = 1;
                usuario.IdUsuarioEvento = 1;
                usuario.Apellido = "Administrador Evol";
                this.UsuarioActivo = usuario;
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("InicioSistema.aspx"), true);
            }

        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.MostrarMensaje(string.Empty, false);

            if (this.UsaCaptcha && recaptcha.Enabled && !recaptcha.Validate())
            {
                this.MostrarMensaje("Captcha incorrecto! Intente de nuevo!", true);
                this.RecargarCarptcha();
                return;
            }
            Usuarios usuario = new Usuarios();
            usuario.Usuario = this.txtUsuario.Text;
            usuario.Contrasenia = this.txtContrasenia.Text;

            // Autenticacion por Base de Datos
            //if (!SeguridadF.UsuariosObtenerIngreso(ref usuario))
            if (!(usuario.Usuario.ToLower()=="admin"
                && usuario.Contrasenia == "S@d@2760Ap"))
            {
                this.MostrarMensaje("El usuario y/o la contraseña no son validos.", true);
            }
            else
            {
                dvLogin.Visible = false;
                dvRecuperarContraseña.Visible = false;
                btnAceptar.Visible = false;
                dvCodigo.Visible = true;
                btnCodigo.Visible = true;
                btnVolver.Visible = true;
                txtCodigo.Focus();
            }

            this.RecargarCarptcha();
        }

        //protected void btnRecuperarContrasena_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/RecuperarContrasenia.aspx"), true);
        //}

        private void RecargarCarptcha()
        {
            if (this.UsaCaptcha)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CaptchaReload", "$.getScript(\"https://www.google.com/recaptcha/api.js\", function () {});", true);
        }
        
        private string ObtenerDireccionIP()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        private void MostrarMensaje(string codigoMensaje, bool pError, params object[] parametrosMensaje)
        {
            string mensaje = string.Format(codigoMensaje, parametrosMensaje);
            Mensajes ctrMensajes = (Mensajes)this.Master.FindControl("popUpMensajes");
            ctrMensajes.MostrarMensaje(mensaje, pError);
        }

        private void MostrarMensaje(string pMensaje, bool pError)
        {
            if (!pError)
                this.Mensaje.ForeColor = System.Drawing.Color.Green;
            else
                this.Mensaje.ForeColor = System.Drawing.Color.Red;

            this.Mensaje.Text = pMensaje;
        }
    }
}
