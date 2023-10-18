using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Security.Principal;
using Seguridad.Entidades;
using Generales;
using Generales.LogicaNegocio;
using Seguridad;
using Seguridad.LogicaNegocio;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Seguridad.FachadaNegocio;
using System.DirectoryServices.AccountManagement;
using Generales.Entidades;
using System.Collections;
using System.Net;
using Google.Authenticator;
using System.Web.Configuration;
using IU.Modulos.TGE.Control;

namespace IU
{
    public partial class IngresoSistema : System.Web.UI.Page
    {
        //public string MiSessionPagina
        //{
        //    get
        //    {
        //        return (string)this.ViewState["UniqueTabGuid"];
        //    }
        //    set
        //    {
        //        this.ViewState["UniqueTabGuid"] = value;
        //    }
        //    //get
        //    //{
        //    //    if (this.ViewState[this.AppRelativeVirtualPath] == null)
        //    //        this.ViewState[this.AppRelativeVirtualPath] = Guid.NewGuid().ToString();
        //    //    return (string)this.ViewState[this.AppRelativeVirtualPath];
        //    //}
        //    //set { this.ViewState[this.AppRelativeVirtualPath] = value; }
        //}

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

        private List<TGEListasValoresDetalles> TiposAutenticacion
        {
            get
            {
                if (Session["TiposAutenticacion"] != null)
                { return (List<TGEListasValoresDetalles>)Session["TiposAutenticacion"]; }
                else
                { return new List<TGEListasValoresDetalles>(); }
            }
            set { Session["TiposAutenticacion"] = value; }
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
                this.UsuarioEmpresa = TGEGeneralesF.EmpresasSeleccionar();

                string base64String = this.UsuarioEmpresa.Logo == null ? string.Empty : Convert.ToBase64String(this.UsuarioEmpresa.Logo, 0, this.UsuarioEmpresa.Logo.Length);
                if (base64String != string.Empty)
                    this.imgLogo.ImageUrl = "data:image/png;base64," + base64String;
                else
                    this.imgLogo.ImageUrl = "Imagenes/Logo.png";
                
                this.imgLogo.Visible = true;
                
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
                    TGEListasValores lv = new TGEListasValores();
                    lv.CodigoValor = "TipoAutenticacion";
                    this.TiposAutenticacion = TGEGeneralesF.ListasValoresObtenerListaDetalle(lv);
                    this.ddlTipoAutenticacion.DataSource = this.TiposAutenticacion;
                    this.ddlTipoAutenticacion.DataValueField = "CodigoValor";
                    this.ddlTipoAutenticacion.DataTextField = "Descripcion";
                    this.ddlTipoAutenticacion.DataBind();
                    AyudaProgramacion.InsertarItemSeleccione(this.ddlTipoAutenticacion, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                    this.btnAceptar.Visible = true;
                    TGEParametrosValores paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.AutenticacionCaptcha);
                    this.UsaCaptcha = paramValor.ParametroValor.Length == 0 ? false : Convert.ToBoolean(paramValor.ParametroValor);

                    if (this.TiposAutenticacion.Exists(x => x.CodigoValor == "GoogleRecaptcha"))
                    {
                        this.ddlTipoAutenticacion.SelectedValue = "GoogleRecaptcha";
                        this.ddlTipoAutenticacion_SelectedIndexChanged(null, EventArgs.Empty);
                    }
                }
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //this.MostrarMensaje(string.Empty, false);
            Usuarios usuario = new Usuarios();
            usuario.Usuario = this.txtUsuario.Text;
            usuario.Contrasenia = this.txtContrasenia.Text;
            usuario.DireccionIP = this.ObtenerDireccionIP();

            if (!string.IsNullOrEmpty(ddlTipoAutenticacion.SelectedValue))
            {
                switch (ddlTipoAutenticacion.SelectedValue)
                {
                    case "GoogleRecaptcha":
                        if (this.UsaCaptcha && recaptcha.Enabled && !recaptcha.Validate())
                        {
                            this.MostrarMensaje("Captcha incorrecto! Intente de nuevo!", true);
                            this.RecargarCarptcha();
                            return;
                        }
                        ProcesarLogin(usuario);
                        this.RecargarCarptcha();
                        break;
                    case "GoogleAuthenticator":
                        TGEListasValoresDetalles lvd = TiposAutenticacion.FirstOrDefault(x => x.CodigoValor == ddlTipoAutenticacion.SelectedValue);
                        if (lvd.ConsultaDinamicaCombo.Trim() == String.Empty)
                        {
                            this.MostrarMensaje("Falta definir la clave para Google Autenticator!", true);
                            return;
                        }
                        dvLogin.Visible = false;
                        dvGoogleAuthenticator.Visible = true;
                        this.ViewState["userLogin"] = usuario;
                        break;
                    default:
                        return;
                }
            }
            
           
        }
        protected void btnValidar_Click(object sender, EventArgs e)
        {
            dvIncorrecto.Visible = false;
            TGEListasValoresDetalles lvd = TiposAutenticacion.FirstOrDefault(x => x.CodigoValor == ddlTipoAutenticacion.SelectedValue);
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            TimeSpan ts = new TimeSpan(0, 1, 0);
            bool isCorrectPIN = tfa.ValidateTwoFactorPIN(lvd.ConsultaDinamicaCombo.Trim(), this.txtCodigo.Text.Trim(), ts);
            if (isCorrectPIN)
            {
                Usuarios usuario = this.ViewState["userLogin"] != null ? (Usuarios)this.ViewState["userLogin"] : new Usuarios();
                ProcesarLogin(usuario);
            }
            else
                dvIncorrecto.Visible = true;
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("IngresoSistema.aspx"), true);
        }

        private void ProcesarLogin(Usuarios usuario)
        {
            TGEParametrosValores autenticarPorDominio = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.AutenticacionPorDominio);
            TGEParametrosValores dominio = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.NombreDominio);

            if (autenticarPorDominio.ParametroValor.Length == 0)
            {
                autenticarPorDominio.ParametroValor = "false";
            }

            if (Boolean.Parse(autenticarPorDominio.ParametroValor))
            {
                try
                {
                    // Autenticacion por Dominio
                    using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, dominio.ParametroValor))
                    {
                        if (pc == null)
                        {
                            this.MostrarMensaje("ValidarDominio", true, new object[] { dominio.ParametroValor });
                        }

                        // validate the credentials
                        bool isValid = pc.ValidateCredentials(usuario.Usuario.Trim(), usuario.Contrasenia.Trim());
                        if (isValid)
                        {
                            if (!SeguridadF.UsuariosObtenerIngresoAutenticado(ref usuario))
                            {
                                this.MostrarMensaje(usuario.CodigoMensaje, true);
                            }
                            else
                            {
                                this.UsuarioActivo = usuario;
                                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("InicioSistema.aspx"), true);
                            }
                        }

                    }
                }
                catch (PrincipalServerDownException ex)
                {
                    this.MostrarMensaje(ex.InnerException.Message, true);
                }
            }
            else
            {
                // Autenticacion por Base de Datos
                if (!SeguridadF.UsuariosObtenerIngreso(ref usuario))
                {
                    this.MostrarMensaje(usuario.CodigoMensaje, true);
                }
                else
                {
                    if (!usuario.EsAdministradorGeneral)
                    {
                        /*Validacion de DNS*/
                        TGEParametrosValores dnsVal = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.FiltroDNSCliente);
                        List<string> listaDns = dnsVal.ParametroValor.Trim().Length > 0 ? dnsVal.ParametroValor.Trim().Split(';').ToList() : new List<string>();
                        if (listaDns.Count > 0)
                        {
                            List<IPAddress> listaIp = new List<IPAddress>();
                            foreach (string s in listaDns)
                            {
                                try
                                {
                                    var ipadresses = Dns.GetHostAddresses(s);
                                    foreach (IPAddress ip in ipadresses)
                                        listaIp.Add(ip);
                                }
                                catch (System.Net.Sockets.SocketException)
                                { }
                            }
                            //MostrarMensaje(this.ObtenerDireccionIP(), true);
                            string ipCliente = this.ObtenerDireccionIP();
                            if (!string.IsNullOrEmpty(ipCliente))
                            {
                                string ipvarias = string.Empty;
                                foreach (IPAddress i in listaIp)
                                    ipvarias = string.Concat(ipvarias, " - ", i.ToString());
                                //MostrarMensaje(ipvarias, false);
                                //MostrarMensaje(iphe.HostName, false);
                                if (!listaIp.Exists(x => x.ToString() == ipCliente))
                                {
                                    this.MostrarMensaje(this.ObtenerMensajeSistema("ValidarFiltroDNS"), true, ipCliente);
                                    return;
                                }
                            }
                        }
                    }
                    this.UsuarioActivo = usuario;
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("InicioSistema.aspx"), true);
                }
            }
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

        private void MostrarMensaje(string pMensaje, bool pError)
        {
            if (!pError)
                this.Mensaje.ForeColor = System.Drawing.Color.Green;
            else
                this.Mensaje.ForeColor = System.Drawing.Color.Red;

            this.Mensaje.Text = this.ObtenerMensajeSistema(pMensaje);
        }

        protected void ddlTipoAutenticacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            dvGoogleReCaptcha.Visible = false;
            dvGoogleAuthenticator.Visible = false;
            if (!string.IsNullOrEmpty(ddlTipoAutenticacion.SelectedValue))
            {
                switch (ddlTipoAutenticacion.SelectedValue)
                {
                    case "GoogleRecaptcha":
                        dvGoogleReCaptcha.Visible = true;
                        this.recaptcha.Enabled = this.UsaCaptcha;
                        this.recaptcha.Visible = this.recaptcha.Enabled;
                        this.RecargarCarptcha();
                        break;
                    case "GoogleAuthenticator":
                        //dvGoogleAuthenticator.Visible = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
