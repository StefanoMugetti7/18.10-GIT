using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Comunes.Entidades;
using System.Xml;
using Servicio.Encriptacion;
using Seguridad.FachadaNegocio;
using System.Net.Mail;
using System.IO;
using Auditoria;

namespace IU
{
    public partial class RecuperarContrasenia : System.Web.UI.Page
    {
        public string MiSessionPagina
        {
            get
            {
                if (this.ViewState[this.AppRelativeVirtualPath] == null)
                    this.ViewState[this.AppRelativeVirtualPath] = Guid.NewGuid().ToString();
                return (string)this.ViewState[this.AppRelativeVirtualPath];
            }
            set { this.ViewState[this.AppRelativeVirtualPath] = value; }
        }

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

        protected TGEEmpresas UsuarioEmpresa
        {
            get
            {
                if (this.Application["UsuarioEmpresa"] != null)
                { return (TGEEmpresas)this.Application["UsuarioEmpresa"]; }
                else
                { return new TGEEmpresas(); }
            }
            set { this.Application["UsuarioEmpresa"] = value; }
        }

        private XmlDocument MensajesSistema
        {
            get
            {
                if (this.Application["MensajesSistema"] != null)
                { return (XmlDocument)this.Application["MensajesSistema"]; }
                else
                {
                    XmlDocument xmlDoc = TGEGeneralesF.TGELeerXMLObtenerMensajesSistema();
                    this.Application.Add("MensajesSistema", xmlDoc);
                    return xmlDoc;
                }
            }
            set { this.Application["MensajesSistema"] = value; }
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
                ScriptManager.RegisterClientScriptResource(this, typeof(WebResources), "IU.assets.js.jquery-3.1.1.min.js");
                ScriptManager.RegisterClientScriptResource(this, typeof(WebResources), "IU.assets.js.jquery-migrate-3.0.0.min.js");

                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCorreoElectronico, this.btnAceptar);
                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCUI, this.btnAceptar);
                this.UsuarioEmpresa = TGEGeneralesF.EmpresasSeleccionar();

                string base64String = this.UsuarioEmpresa.Logo == null ? string.Empty : Convert.ToBase64String(this.UsuarioEmpresa.Logo, 0, this.UsuarioEmpresa.Logo.Length);
                if (base64String != string.Empty)
                    this.imgLogo.ImageUrl = "data:image/png;base64," + base64String;
                else
                    this.imgLogo.ImageUrl = "Imagenes/Logo.png";
                this.imgLogo.AlternateText = this.UsuarioEmpresa.Empresa;

                if (UsuarioActivo != null && UsuarioActivo.IdUsuario > 0)
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("InicioSistema.aspx"), true);

                this.UsaCaptcha = false;
                Objeto parametro = new Objeto();
                if (!TGEGeneralesF.BaseDatosValidar(parametro))
                {
                    this.MostrarMensaje(this.ObtenerMensajeSistema(parametro.CodigoMensaje), true, parametro.CodigoMensajeArgs.ToArray());
                    this.btnAceptar.Visible = false;
                    return;
                }
                else
                {
                    this.btnAceptar.Visible = true;
                    TGEParametrosValores paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.AutenticacionCaptcha);
                    this.UsaCaptcha = paramValor.ParametroValor.Length == 0 ? false : Convert.ToBoolean(paramValor.ParametroValor);
                    this.recaptcha.Enabled = this.UsaCaptcha;
                    this.recaptcha.Visible = this.recaptcha.Enabled;
                }
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

            //if (this.UsuarioEmpresa.CUIT != this.txtCUI.Text)
            //{
            //    this.MostrarMensaje("El numero de CUIT es Incorrecto", true);
            //    this.RecargarCarptcha();
            //    return;
            //}

            Usuarios usuario = new Usuarios();
            usuario.CorreoElectronico = this.txtCorreoElectronico.Text;
            if (SeguridadF.UsuariosValidarCorreoElectronico(usuario))
            {
                usuario = SeguridadF.UsuariosObtenerPorCorreoElectronico(usuario);
                string url = string.Concat(this.Page.Request.Url.Scheme, "://", this.Page.Request.Url.Host, this.ObtenerAppPath(), "RecuperarContraseniaPaso2.aspx?parametros=");
                string parametros = string.Concat(this.txtCorreoElectronico.Text, "|", DateTime.Now.Year.ToString().PadLeft(2, '0'), DateTime.Now.Month.ToString().PadLeft(2, '0'), DateTime.Now.Day.ToString().PadLeft(2, '0'));
                string link = string.Concat(url, Encriptar.EncriptarTexto(parametros));

                MailMessage mail = new MailMessage();
                mail.To.Add(new MailAddress(usuario.CorreoElectronico));
                mail.Subject = "Recupera tu usuario y clave de ERP EVOL";

                mail.IsBodyHtml = true;
                string template = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Templates\\MailEnviarRecuperarContrasena.htm");
                mail.Body = new StreamReader(template).ReadToEnd();
                mail.Body = mail.Body.Replace("%ApellidoNombre%", usuario.ApellidoNombre);
                mail.Body = mail.Body.Replace("%Usuario%", usuario.Usuario);
                mail.Body = mail.Body.Replace("%Link%", link);
                mail.Body = mail.Body.Replace("%Empresa%", this.UsuarioEmpresa.Empresa);

                bool resultado = AuditoriaF.MailsEnviosAgregar(mail, usuario, 13);

                if (resultado)
                {
                    TGEGeneralesF.EnviarProcesoMails();

                    this.btnAceptar.Visible = false;
                    this.btnVolver.Visible = true;
                    this.MostrarMensaje("Se ha enviado un correo electronico a la direccion ingresada.", false);
                }
                else
                {
                    this.MostrarMensaje(usuario.CodigoMensaje, true, usuario.CodigoMensajeArgs);
                }
            }
            else
            {
                this.MostrarMensaje(usuario.CodigoMensaje, true);
                this.RecargarCarptcha();
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/IngresoSistema.aspx"), true);
        }

        protected string ObtenerAppPath()
        {
            return this.Page.Request.ApplicationPath.EndsWith("/") ? this.Page.Request.ApplicationPath : string.Concat(this.Page.Request.ApplicationPath, "/");
        }

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

        private string ObtenerMensajeSistema(string codigoMensaje)
        {
            XmlNode nodo = this.MensajesSistema.GetElementsByTagName(codigoMensaje).Item(0);
            if (nodo != null)
                return nodo.InnerText;
            else
                return codigoMensaje;
        }

        private void MostrarMensaje(string codigoMensaje, bool pError, params object[] parametrosMensaje)
        {
            string mensaje = string.Format(this.ObtenerMensajeSistema(codigoMensaje), parametrosMensaje);
            Mensajes ctrMensajes = (Mensajes)this.Master.FindControl("popUpMensajes");
            ctrMensajes.MostrarMensaje(mensaje, pError);
        }
    }
}
